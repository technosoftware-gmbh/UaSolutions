#region Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Manages the montioredItems for a NodeManager
    /// </summary>
    public class MonitoredNodeMonitoredItemManager : IUaMonitoredItemManager
    {
        /// <inheritdoc/>
        public MonitoredNodeMonitoredItemManager(UaStandardNodeManager nodeManager)
        {
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
            // check if the node is already being monitored.

            if (!MonitoredNodes.TryGetValue(handle.Node.NodeId, out UaMonitoredNode monitoredNode))
            {
                NodeState cachedNode = addNodeToComponentCache(context, handle, handle.Node);
                MonitoredNodes[handle.Node.NodeId]
                    = monitoredNode = new UaMonitoredNode(m_nodeManager, cachedNode);
            }

            handle.Node = monitoredNode.Node;
            handle.MonitoredNode = monitoredNode;

            // Allocate the monitored item id
            uint monitoredItemId;
            do
            {
                monitoredItemId = monitoredItemIdFactory.GetNextId();
            } while (!MonitoredItems.TryAdd(monitoredItemId, null));

            // create the item.
            IUaSampledDataChangeMonitoredItem datachangeItem = new UaMonitoredItem(
                server,
                m_nodeManager,
                handle,
                subscriptionId,
                monitoredItemId,
                itemToCreate.ItemToMonitor,
                diagnosticsMasks,
                timestampsToReturn,
                itemToCreate.MonitoringMode,
                itemToCreate.RequestedParameters.ClientHandle,
                filterToUse,
                filterToUse,
                euRange,
                samplingInterval,
                revisedQueueSize,
                itemToCreate.RequestedParameters.DiscardOldest,
                0,
                createDurable);

            // now save the monitored item.
            monitoredNode.Add(datachangeItem);
            Debug.Assert(MonitoredItems[monitoredItemId] == null);
            MonitoredItems[monitoredItemId] = datachangeItem;

            return datachangeItem;
        }

        /// <inheritdoc/>
        public void ApplyChanges()
        {
            //only needed for sampling groups
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
        }

        /// <inheritdoc/>
        public StatusCode DeleteMonitoredItem(
            UaServerContext context,
            IUaSampledDataChangeMonitoredItem monitoredItem,
            UaNodeHandle handle)
        {
            // check if the node is already being monitored.
            if (MonitoredNodes.TryGetValue(handle.NodeId, out UaMonitoredNode monitoredNode))
            {
                monitoredNode.Remove(monitoredItem);
                MonitoredItems.TryRemove(monitoredItem.Id, out _);

                // check if node is no longer being monitored.
                if (!monitoredNode.HasMonitoredItems)
                {
                    MonitoredNodes.Remove(handle.NodeId);
                }
            }
            else
            {
                return StatusCodes.BadMonitoredItemIdInvalid;
            }

            return StatusCodes.Good;
        }

        /// <inheritdoc/>
        public (ServiceResult, MonitoringMode?) SetMonitoringMode(
            UaServerContext context,
            IUaSampledDataChangeMonitoredItem monitoredItem,
            MonitoringMode monitoringMode,
            UaNodeHandle handle)
        {
            // update monitoring mode.
            MonitoringMode previousMode = monitoredItem.SetMonitoringMode(monitoringMode);

            // must send the latest value after enabling a disabled item.
            if (monitoringMode == MonitoringMode.Reporting &&
                previousMode == MonitoringMode.Disabled)
            {
                handle.MonitoredNode.QueueValue(context, handle.Node, monitoredItem);
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
            // check if the node is already being monitored.
            if (!MonitoredNodes.TryGetValue(handle.Node.NodeId, out UaMonitoredNode monitoredNode))
            {
                NodeState cachedNode = addNodeToComponentCache(context, handle, handle.Node);
                MonitoredNodes[handle.Node.NodeId]
                    = monitoredNode = new UaMonitoredNode(m_nodeManager, cachedNode);
            }

            handle.Node = monitoredNode.Node;
            handle.MonitoredNode = monitoredNode;

            // create the item.
            IUaSampledDataChangeMonitoredItem datachangeItem = new UaMonitoredItem(
                server,
                nodeManager,
                handle,
                storedMonitoredItem);

            // update monitored item list.
            monitoredItem = datachangeItem;

            // save the monitored item.
            monitoredNode.Add(datachangeItem);

            return true;
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
            // modify the monitored item parameters.
            return monitoredItem.ModifyAttributes(
                diagnosticsMasks,
                timestampsToReturn,
                itemToModify.RequestedParameters.ClientHandle,
                filterToUse,
                filterToUse,
                euRange,
                samplingInterval,
                revisedQueueSize,
                itemToModify.RequestedParameters.DiscardOldest);
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
            if ((source is not BaseObjectState instance) ||
                (instance.EventNotifier & EventNotifiers.SubscribeToEvents) == 0)
            {
                if ((source is not ViewState view) ||
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
            if (!MonitoredItems.TryAdd(monitoredItem.Id, monitoredItem))
            {
                return (monitoredNode, StatusCodes.BadUnexpectedError);
            }

            return (monitoredNode, ServiceResult.Good);
        }

        private readonly UaStandardNodeManager m_nodeManager;
    }
}
