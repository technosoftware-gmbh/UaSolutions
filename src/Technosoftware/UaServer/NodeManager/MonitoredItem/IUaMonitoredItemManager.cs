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
    /// Manages the MonitoredItems for a NodeManager
    /// </summary>
    public interface IUaMonitoredItemManager : IDisposable
    {
        /// <summary>
        /// The table of MonitoredItems.
        /// </summary>
        ConcurrentDictionary<uint, IUaMonitoredItem> MonitoredItems { get; }

        /// <summary>
        /// Gets the table of nodes being monitored.
        /// If sampling groups are used only contains the Nodes being monitored for events
        /// </summary>
        NodeIdDictionary<UaMonitoredNode> MonitoredNodes { get; }

        /// <summary>
        /// Apply pending changes to the monitored items.
        /// Currently only relant if sampling groups are used.
        /// </summary>
        void ApplyChanges();

        /// <summary>
        /// Create a MonitoredItem and save it in table of monitored items.
        /// </summary>
        IUaSampledDataChangeMonitoredItem CreateMonitoredItem(
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
            Func<ISystemContext, UaNodeHandle, NodeState, NodeState> addNodeToComponentCache);

        /// <summary>
        /// Modify a monitored item
        /// </summary>
        ServiceResult ModifyMonitoredItem(
            UaServerContext context,
            DiagnosticsMasks diagnosticsMasks,
            TimestampsToReturn timestampsToReturn,
            MonitoringFilter filterToUse,
            Opc.Ua.Range euRange,
            double samplingInterval,
            uint revisedQueueSize,
            IUaSampledDataChangeMonitoredItem monitoredItem,
            MonitoredItemModifyRequest itemToModify);

        /// <summary>
        /// Delete a MonitoredItem and remove it from the table of monitored items.
        /// </summary>
        StatusCode DeleteMonitoredItem(
            UaServerContext context,
            IUaSampledDataChangeMonitoredItem monitoredItem,
            UaNodeHandle handle);

        /// <summary>
        /// Set the monitoring mode for a monitored item
        /// </summary>
        (ServiceResult, MonitoringMode?) SetMonitoringMode(
            UaServerContext context,
            IUaSampledDataChangeMonitoredItem monitoredItem,
            MonitoringMode monitoringMode,
            UaNodeHandle handle);

        /// <summary>
        /// Restore a monitored item
        /// </summary>
        bool RestoreMonitoredItem(
            IUaServerData server,
            IUaNodeManager nodeManager,
            UaServerContext context,
            UaNodeHandle handle,
            IUaStoredMonitoredItem storedMonitoredItem,
            IUserIdentity savedOwnerIdentity,
            Func<ISystemContext, UaNodeHandle, NodeState, NodeState> addNodeToComponentCache,
            out IUaSampledDataChangeMonitoredItem monitoredItem);

        /// <summary>
        /// Subscribe to events of the specified node.
        /// </summary>
        (UaMonitoredNode, ServiceResult) SubscribeToEvents(
            UaServerContext context,
            NodeState source,
            IUaEventMonitoredItem monitoredItem,
            bool unsubscribe);
    }
}
