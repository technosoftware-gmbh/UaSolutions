#region Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is subject to the Technosoftware GmbH Software License 
// Agreement, which can be found here:
// https://technosoftware.com/documents/Source_License_Agreement.pdf
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// The factory for an <see cref="SyncNodeManagerAdapter"/>
    /// </summary>
    public static class SyncNodeManagerAdapterFactory
    {
        /// <summary>
        /// returns an instance of <see cref="IUaNodeManager"/>
        /// if the NodeManager does not implement the interface uses the <see cref="SyncNodeManagerAdapter"/>
        /// to create an ISyncNodeManager compatible object
        /// </summary>
        public static IUaStandardNodeManager ToSyncNodeManager(this IUaStandardAsyncNodeManager nodeManager)
        {
            if (nodeManager is IUaStandardNodeManager syncNodeManager)
            {
                return syncNodeManager;
            }
            return new SyncNodeManagerAdapter(nodeManager);
        }
    }

    /// <summary>
    /// An adapter that makes a asynchronous IAsyncNodeManager conform to the INodeManager interface.
    /// </summary>
    /// <remarks>
    /// This allows asynchronous nodeManagers to be treated as synchronous, which can help
    /// compatibility with existing code.
    /// </remarks>
    public class SyncNodeManagerAdapter : IUaStandardNodeManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncNodeManagerAdapter"/> class.
        /// </summary>
        /// <param name="nodeManager">The asynchronous node manager to wrap.</param>
        /// <exception cref="ArgumentNullException"><paramref name="nodeManager"/> is <c>null</c>.</exception>
        public SyncNodeManagerAdapter(IUaStandardAsyncNodeManager nodeManager)
        {
            m_nodeManager = nodeManager ?? throw new ArgumentNullException(nameof(nodeManager));
        }

        /// <inheritdoc/>
        public IEnumerable<string> NamespaceUris => m_nodeManager.NamespaceUris;

        /// <inheritdoc/>
        public void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            m_nodeManager.CreateAddressSpaceAsync(externalReferences).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void DeleteAddressSpace()
        {
            m_nodeManager.DeleteAddressSpaceAsync().AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public object GetManagerHandle(NodeId nodeId)
        {
            return m_nodeManager.GetManagerHandleAsync(nodeId).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void AddReferences(IDictionary<NodeId, IList<IReference>> references)
        {
            m_nodeManager.AddReferencesAsync(references).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public ServiceResult DeleteReference(object sourceHandle, NodeId referenceTypeId, bool isInverse, ExpandedNodeId targetId, bool deleteBidirectional)
        {
            return m_nodeManager.DeleteReferenceAsync(sourceHandle, referenceTypeId, isInverse, targetId, deleteBidirectional)
                .AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public UaNodeMetadata GetNodeMetadata(UaServerOperationContext context, object targetHandle, BrowseResultMask resultMask)
        {
            return m_nodeManager.GetNodeMetadataAsync(context, targetHandle, resultMask).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void Browse(UaServerOperationContext context, ref UaContinuationPoint continuationPoint, IList<ReferenceDescription> references)
        {
            continuationPoint = m_nodeManager.BrowseAsync(context, continuationPoint, references).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void TranslateBrowsePath(
            UaServerOperationContext context,
            object sourceHandle,
            RelativePathElement relativePath,
            IList<ExpandedNodeId> targetIds,
            IList<NodeId> unresolvedTargetIds)
        {
            m_nodeManager.TranslateBrowsePathAsync(context, sourceHandle, relativePath, targetIds, unresolvedTargetIds).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void Read(UaServerOperationContext context, double maxAge, IList<ReadValueId> nodesToRead, IList<DataValue> values, IList<ServiceResult> errors)
        {
            m_nodeManager.ReadAsync(context, maxAge, nodesToRead, values, errors).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void HistoryRead(
            UaServerOperationContext context,
            HistoryReadDetails details,
            TimestampsToReturn timestampsToReturn,
            bool releaseContinuationPoints,
            IList<HistoryReadValueId> nodesToRead,
            IList<HistoryReadResult> results,
            IList<ServiceResult> errors)
        {
            m_nodeManager.HistoryReadAsync(context, details, timestampsToReturn, releaseContinuationPoints, nodesToRead, results, errors)
                .AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void Write(UaServerOperationContext context, IList<WriteValue> nodesToWrite, IList<ServiceResult> errors)
        {
            m_nodeManager.WriteAsync(context, nodesToWrite, errors).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void HistoryUpdate(
            UaServerOperationContext context,
            Type detailsType,
            IList<HistoryUpdateDetails> nodesToUpdate,
            IList<HistoryUpdateResult> results,
            IList<ServiceResult> errors)
        {
            m_nodeManager.HistoryUpdateAsync(context, detailsType, nodesToUpdate, results, errors).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void Call(UaServerOperationContext context, IList<CallMethodRequest> methodsToCall, IList<CallMethodResult> results, IList<ServiceResult> errors)
        {
            m_nodeManager.CallAsync(context, methodsToCall, results, errors).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public ServiceResult SubscribeToEvents(
            UaServerOperationContext context,
            object sourceId,
            uint subscriptionId,
            IUaEventMonitoredItem monitoredItem,
            bool unsubscribe)
        {
            return m_nodeManager.SubscribeToEventsAsync(context, sourceId, subscriptionId, monitoredItem, unsubscribe).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public ServiceResult SubscribeToAllEvents(UaServerOperationContext context, uint subscriptionId, IUaEventMonitoredItem monitoredItem, bool unsubscribe)
        {
            return m_nodeManager.SubscribeToAllEventsAsync(context, subscriptionId, monitoredItem, unsubscribe).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public ServiceResult ConditionRefresh(UaServerOperationContext context, IList<IUaEventMonitoredItem> monitoredItems)
        {
            m_nodeManager.ConditionRefreshAsync(context, monitoredItems).AsTask().GetAwaiter().GetResult();
            return ServiceResult.Good;
        }

        /// <inheritdoc/>
        public void CreateMonitoredItems(
            UaServerOperationContext context,
            uint subscriptionId,
            double publishingInterval,
            TimestampsToReturn timestampsToReturn,
            IList<MonitoredItemCreateRequest> itemsToCreate,
            IList<ServiceResult> errors,
            IList<MonitoringFilterResult> filterErrors,
            IList<IUaMonitoredItem> monitoredItems,
            bool createDurable,
            MonitoredItemIdFactory monitoredItemIdFactory)
        {
            m_nodeManager.CreateMonitoredItemsAsync(context, subscriptionId, publishingInterval, timestampsToReturn,
                itemsToCreate, errors, filterErrors, monitoredItems, createDurable, monitoredItemIdFactory)
                .AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void RestoreMonitoredItems(
            IList<IUaStoredMonitoredItem> itemsToRestore,
            IList<IUaMonitoredItem> monitoredItems,
            IUserIdentity savedOwnerIdentity)
        {
            m_nodeManager.RestoreMonitoredItemsAsync(itemsToRestore, monitoredItems, savedOwnerIdentity).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void ModifyMonitoredItems(
            UaServerOperationContext context,
            TimestampsToReturn timestampsToReturn,
            IList<IUaMonitoredItem> monitoredItems,
            IList<MonitoredItemModifyRequest> itemsToModify,
            IList<ServiceResult> errors,
            IList<MonitoringFilterResult> filterErrors)
        {
            m_nodeManager.ModifyMonitoredItemsAsync(context, timestampsToReturn, monitoredItems, itemsToModify, errors, filterErrors)
                .AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void DeleteMonitoredItems(
            UaServerOperationContext context,
            IList<IUaMonitoredItem> monitoredItems,
            IList<bool> processedItems,
            IList<ServiceResult> errors)
        {
            m_nodeManager.DeleteMonitoredItemsAsync(context, monitoredItems, processedItems, errors).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void TransferMonitoredItems(
            UaServerOperationContext context,
            bool sendInitialValues,
            IList<IUaMonitoredItem> monitoredItems,
            IList<bool> processedItems,
            IList<ServiceResult> errors)
        {
            m_nodeManager.TransferMonitoredItemsAsync(context, sendInitialValues, monitoredItems, processedItems, errors).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void SetMonitoringMode(
            UaServerOperationContext context,
            MonitoringMode monitoringMode,
            IList<IUaMonitoredItem> monitoredItems,
            IList<bool> processedItems,
            IList<ServiceResult> errors)
        {
            m_nodeManager.SetMonitoringModeAsync(context, monitoringMode, monitoredItems, processedItems, errors).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void SessionClosing(UaServerOperationContext context, NodeId sessionId, bool deleteSubscriptions)
        {
            m_nodeManager.SessionClosingAsync(context, sessionId, deleteSubscriptions).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public bool IsNodeInView(UaServerOperationContext context, NodeId viewId, object nodeHandle)
        {
            return m_nodeManager.IsNodeInViewAsync(context, viewId, nodeHandle).AsTask().GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public UaNodeMetadata GetPermissionMetadata(
            UaServerOperationContext context,
            object targetHandle,
            BrowseResultMask resultMask,
            Dictionary<NodeId, List<object>> uniqueNodesServiceAttributesCache,
            bool permissionsOnly)
        {
            return m_nodeManager.GetPermissionMetadataAsync(context, targetHandle, resultMask, uniqueNodesServiceAttributesCache, permissionsOnly)
                .AsTask().GetAwaiter().GetResult();
        }

        private readonly IUaStandardAsyncNodeManager m_nodeManager;
    }
}
