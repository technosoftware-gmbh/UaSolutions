#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Concurrent;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Manages the montioredItems for a NodeManager
    /// </summary>
    public class SamplingGroupMonitoredItemManager : IUaMonitoredItemManager
    {
        /// <inheritdoc/>
        public SamplingGroupMonitoredItemManager(
            UaStandardNodeManager nodeManager,
            IUaServerData server,
            ApplicationConfiguration configuration)
        {
            m_samplingGroupManager = new SamplingGroupManager(
                server,
                nodeManager,
                (uint)configuration.ServerConfiguration.MaxNotificationQueueSize,
                (uint)configuration.ServerConfiguration.MaxDurableNotificationQueueSize,
                configuration.ServerConfiguration.AvailableSamplingRates);

            m_nodeManager = nodeManager;
            MonitoredNodes = [];
            MonitoredItems = new ConcurrentDictionary<uint, IUaMonitoredItem>();
        }

        /// <inheritdoc/>
        public NodeIdDictionary<UaMonitoredNode> MonitoredNodes { get; }

        /// <inheritdoc/>
        public ConcurrentDictionary<uint, IUaMonitoredItem> MonitoredItems { get; }

        /// <inheritdoc/>
        public IUaSampledDataChangeMonitoredItem CreateMonitoredItem(
            IUaServerData server,
            IUaNodeManager nodeManager,
            UaServerContext context,
            UaNodeHandle handle,
            uint subscriptionId,
            double publishingInterval,
            DiagnosticsMasks diagnosticsMasks,
            TimestampsToReturn timestampsToReturn,
            MonitoredItemCreateRequest itemToCreate,
            Opc.Ua.Range euRange,
            MonitoringFilter filterToUse,
            double samplingInterval,
            uint revisedQueueSize,
            bool createDurable,
            MonitoredItemIdFactory monitoredItemIdFactory,
            Func<ISystemContext, UaNodeHandle, NodeState, NodeState> addNodeToComponentCache)
        {
            // set min sampling interval if 0
            if (samplingInterval.CompareTo(0.0) == 0)
            {
                samplingInterval = 1;
            }

            uint monitoredItemId = monitoredItemIdFactory.GetNextId();

            // create monitored item.
            IUaSampledDataChangeMonitoredItem monitoredItem =
                m_samplingGroupManager.CreateMonitoredItem(
                    context.OperationContext,
                    subscriptionId,
                    publishingInterval,
                    timestampsToReturn,
                    monitoredItemId,
                    handle,
                    itemToCreate,
                    euRange,
                    samplingInterval,
                    createDurable);

            // save the monitored item.
            MonitoredItems.AddOrUpdate(
                monitoredItemId,
                monitoredItem,
                (key, oldValue) => monitoredItem);

            return monitoredItem;
        }

        /// <inheritdoc/>
        public void ApplyChanges()
        {
            // update all groups with any new items.
            m_samplingGroupManager.ApplyChanges();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// An overrideable version of the Dispose.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Utils.SilentDispose(m_samplingGroupManager);
            }
        }

        /// <inheritdoc/>
        public StatusCode DeleteMonitoredItem(
            UaServerContext context,
            IUaSampledDataChangeMonitoredItem monitoredItem,
            UaNodeHandle handle)
        {
            // validate monitored item.
            if (!MonitoredItems.TryGetValue(
                monitoredItem.Id,
                out IUaMonitoredItem existingMonitoredItem))
            {
                return StatusCodes.BadMonitoredItemIdInvalid;
            }

            if (!ReferenceEquals(monitoredItem, existingMonitoredItem))
            {
                return StatusCodes.BadMonitoredItemIdInvalid;
            }

            // remove item.
            m_samplingGroupManager.StopMonitoring(monitoredItem);

            // remove association with the group.
            MonitoredItems.TryRemove(monitoredItem.Id, out _);

            // delete successful.
            return StatusCodes.Good;
        }

        /// <inheritdoc/>
        public ServiceResult ModifyMonitoredItem(
            UaServerContext context,
            DiagnosticsMasks diagnosticsMasks,
            TimestampsToReturn timestampsToReturn,
            MonitoringFilter filterToUse,
            Opc.Ua.Range euRange,
            double samplingInterval,
            uint revisedQueueSize,
            IUaSampledDataChangeMonitoredItem monitoredItem,
            MonitoredItemModifyRequest itemToModify)
        {
            // validate monitored item.
            if (!MonitoredItems.TryGetValue(
                monitoredItem.Id,
                out IUaMonitoredItem existingMonitoredItem))
            {
                return StatusCodes.BadMonitoredItemIdInvalid;
            }

            if (!ReferenceEquals(monitoredItem, existingMonitoredItem))
            {
                return StatusCodes.BadMonitoredItemIdInvalid;
            }

            return m_samplingGroupManager.ModifyMonitoredItem(
                context.OperationContext,
                timestampsToReturn,
                monitoredItem,
                itemToModify,
                euRange);
        }

        /// <inheritdoc/>
        public (ServiceResult, MonitoringMode?) SetMonitoringMode(
            UaServerContext context,
            IUaSampledDataChangeMonitoredItem monitoredItem,
            MonitoringMode monitoringMode,
            UaNodeHandle handle)
        {
            if (!MonitoredItems.TryGetValue(
                monitoredItem.Id,
                out IUaMonitoredItem existingMonitoredItem))
            {
                return (StatusCodes.BadMonitoredItemIdInvalid, null);
            }

            if (!ReferenceEquals(monitoredItem, existingMonitoredItem))
            {
                return (StatusCodes.BadMonitoredItemIdInvalid, null);
            }

            // update monitoring mode.
            MonitoringMode previousMode = monitoredItem.SetMonitoringMode(monitoringMode);

            // need to provide an immediate update after enabling.
            if (previousMode == MonitoringMode.Disabled &&
                monitoringMode != MonitoringMode.Disabled)
            {
                var initialValue = new DataValue
                {
                    ServerTimestamp = DateTime.UtcNow,
                    StatusCode = StatusCodes.BadWaitingForInitialData
                };

                // read the initial value.

                if (monitoredItem.ManagerHandle is Node node)
                {
                    ServiceResult error = node.Read(
                        context,
                        monitoredItem.AttributeId,
                        initialValue);

                    if (ServiceResult.IsBad(error))
                    {
                        initialValue.Value = null;
                        initialValue.StatusCode = error.StatusCode;
                    }
                }

                monitoredItem.QueueValue(initialValue, null);
            }

            return (StatusCodes.Good, previousMode);
        }

        /// <inheritdoc/>
        public bool RestoreMonitoredItem(
            IUaServerData server,
            IUaNodeManager nodeManager,
            UaServerContext context,
            UaNodeHandle handle,
            IUaStoredMonitoredItem storedMonitoredItem,
            IUserIdentity savedOwnerIdentity,
            Func<ISystemContext, UaNodeHandle, NodeState, NodeState> addNodeToComponentCache,
            out IUaSampledDataChangeMonitoredItem monitoredItem)
        {
            // create monitored item.
            monitoredItem = m_samplingGroupManager.RestoreMonitoredItem(
                handle,
                storedMonitoredItem,
                savedOwnerIdentity);

            // save monitored item.
            MonitoredItems.TryAdd(monitoredItem.Id, monitoredItem);

            return true;
        }

        /// <inheritdoc/>
        public (UaMonitoredNode, ServiceResult) SubscribeToEvents(
            UaServerContext context,
            NodeState source,
            IUaEventMonitoredItem monitoredItem,
            bool unsubscribe)
        {
            UaMonitoredNode monitoredNode = null;
            // handle unsubscribe.
            if (unsubscribe)
            {
                // check for existing monitored node.
                if (!MonitoredNodes.TryGetValue(source.NodeId, out monitoredNode))
                {
                    return (null, StatusCodes.BadNodeIdUnknown);
                }

                monitoredNode.Remove(monitoredItem);
                MonitoredItems.TryRemove(monitoredItem.Id, out _);

                // check if node is no longer being monitored.
                if (!monitoredNode.HasMonitoredItems)
                {
                    MonitoredNodes.Remove(source.NodeId);
                }

                return (monitoredNode, ServiceResult.Good);
            }

            // only objects or views can be subscribed to.
            if (source is not BaseObjectState instance ||
                (instance.EventNotifier & EventNotifiers.SubscribeToEvents) == 0)
            {
                if (source is not ViewState view ||
                    (view.EventNotifier & EventNotifiers.SubscribeToEvents) == 0)
                {
                    return (null, StatusCodes.BadNotSupported);
                }
            }

            // check for existing monitored node.
            if (!MonitoredNodes.TryGetValue(source.NodeId, out monitoredNode))
            {
                MonitoredNodes[source.NodeId]
                    = monitoredNode = new UaMonitoredNode(m_nodeManager, source);
            }

            // remove existing monitored items with the same Id prior to insertion in order to avoid duplicates
            // this is necessary since the SubscribeToEvents method is called also from ModifyMonitoredItemsForEvents
            monitoredNode.EventMonitoredItems?.RemoveAll(e => e.Id == monitoredItem.Id);

            // this links the node to specified monitored item and ensures all events
            // reported by the node are added to the monitored item's queue.
            monitoredNode.Add(monitoredItem);
            MonitoredItems.TryAdd(monitoredItem.Id, monitoredItem);

            return (monitoredNode, ServiceResult.Good);
        }

        private readonly UaStandardNodeManager m_nodeManager;
        private readonly SamplingGroupManager m_samplingGroupManager;
    }
}
