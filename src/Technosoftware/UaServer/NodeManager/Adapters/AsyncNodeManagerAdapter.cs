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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// The factory for an <see cref="AsyncNodeManagerAdapter"/>
    /// </summary>
    public static class AsyncNodeManagerAdapterFactory
    {
        /// <summary>
        /// returns an instance of <see cref="IUaStandardAsyncNodeManager"/>
        /// if the NodeManager does not implement the interface uses the <see cref="AsyncNodeManagerAdapter"/>
        /// to create an IAsyncNodeManager compatible object
        /// </summary>
        public static IUaStandardAsyncNodeManager ToAsyncNodeManager(this IUaNodeManager nodeManager)
        {
            if (nodeManager is IUaStandardAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager;
            }
            return new AsyncNodeManagerAdapter(nodeManager);
        }
    }

    /// <summary>
    /// An adapter that makes a synchronous INodeManager conform to the IAsyncNodeManager interface.
    /// </summary>
    /// <remarks>
    /// This allows synchronous, or only partially asynchronous node managers to be treated as asynchronous, which can help
    /// unify the calling logic within the MasterNodeManager.
    /// </remarks>
    public class AsyncNodeManagerAdapter : IUaStandardAsyncNodeManager, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncNodeManagerAdapter"/> class.
        /// </summary>
        /// <param name="nodeManager">The synchronous node manager to wrap.</param>
        /// <exception cref="ArgumentNullException"><paramref name="nodeManager"/> is <c>null</c>.</exception>
        public AsyncNodeManagerAdapter(IUaNodeManager nodeManager)
        {
            SyncNodeManager = nodeManager ?? throw new ArgumentNullException(nameof(nodeManager));
        }

        /// <inheritdoc/>
        public IEnumerable<string> NamespaceUris => SyncNodeManager.NamespaceUris;

        /// <inheritdoc/>
        public IUaNodeManager SyncNodeManager { get; }

        /// <inheritdoc/>
        public ValueTask AddReferencesAsync(
            IDictionary<NodeId, IList<IReference>> references,
            CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaStandardAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.AddReferencesAsync(references, cancellationToken);
            }

            SyncNodeManager.AddReferences(references);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask<UaContinuationPoint> BrowseAsync(
            UaServerOperationContext context,
            UaContinuationPoint continuationPoint,
            IList<ReferenceDescription> references,
            CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaBrowseAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.BrowseAsync(context, continuationPoint, references, cancellationToken);
            }

            SyncNodeManager.Browse(context, ref continuationPoint, references);

            // Return a completed ValueTask since the underlying call is synchronous.
            return new ValueTask<UaContinuationPoint>(continuationPoint);
        }

        /// <inheritdoc/>
        public ValueTask CallAsync(
            UaServerOperationContext context,
            IList<CallMethodRequest> methodsToCall,
            IList<CallMethodResult> results,
            IList<ServiceResult> errors,
            CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaCallAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.CallAsync(context, methodsToCall, results, errors, cancellationToken);
            }

            SyncNodeManager.Call(context, methodsToCall, results, errors);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask ConditionRefreshAsync(
            UaServerOperationContext context,
            IList<IUaEventMonitoredItem> monitoredItems,
            CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaConditionRefreshAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.ConditionRefreshAsync(context, monitoredItems, cancellationToken);
            }

            SyncNodeManager.ConditionRefresh(context, monitoredItems);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask CreateAddressSpaceAsync(
            IDictionary<NodeId, IList<IReference>> externalReferences,
            CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaStandardAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.CreateAddressSpaceAsync(externalReferences, cancellationToken);
            }

            SyncNodeManager.CreateAddressSpace(externalReferences);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask CreateMonitoredItemsAsync(UaServerOperationContext context,
                                                   uint subscriptionId,
                                                   double publishingInterval,
                                                   TimestampsToReturn timestampsToReturn,
                                                   IList<MonitoredItemCreateRequest> itemsToCreate,
                                                   IList<ServiceResult> errors,
                                                   IList<MonitoringFilterResult> filterErrors,
                                                   IList<IUaMonitoredItem> monitoredItems,
                                                   bool createDurable,
                                                   MonitoredItemIdFactory monitoredItemIdFactory,
                                                   CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaCreateMonitoredItemsAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.CreateMonitoredItemsAsync(context,
                                                                  subscriptionId,
                                                                  publishingInterval,
                                                                  timestampsToReturn,
                                                                  itemsToCreate,
                                                                  errors,
                                                                  filterErrors,
                                                                  monitoredItems,
                                                                  createDurable,
                                                                  monitoredItemIdFactory,
                                                                  cancellationToken);
            }

            SyncNodeManager.CreateMonitoredItems(context,
                                              subscriptionId,
                                              publishingInterval,
                                              timestampsToReturn,
                                              itemsToCreate,
                                              errors,
                                              filterErrors,
                                              monitoredItems,
                                              createDurable,
                                              monitoredItemIdFactory);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask DeleteAddressSpaceAsync(CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaStandardAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.DeleteAddressSpaceAsync(cancellationToken);
            }

            SyncNodeManager.DeleteAddressSpace();

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask DeleteMonitoredItemsAsync(UaServerOperationContext context,
                                                   IList<IUaMonitoredItem> monitoredItems,
                                                   IList<bool> processedItems,
                                                   IList<ServiceResult> errors,
                                                   CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaDeleteMonitoredItemsAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.DeleteMonitoredItemsAsync(context, monitoredItems, processedItems, errors, cancellationToken);
            }

            SyncNodeManager.DeleteMonitoredItems(context, monitoredItems, processedItems, errors);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask<ServiceResult> DeleteReferenceAsync(
            object sourceHandle,
            NodeId referenceTypeId,
            bool isInverse,
            ExpandedNodeId targetId,
            bool deleteBidirectional,
            CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaStandardAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.DeleteReferenceAsync(sourceHandle,
                                                             referenceTypeId,
                                                             isInverse,
                                                             targetId,
                                                             deleteBidirectional,
                                                             cancellationToken);
            }

            ServiceResult result = SyncNodeManager.DeleteReference(sourceHandle,
                                                                 referenceTypeId,
                                                                 isInverse,
                                                                 targetId,
                                                                 deleteBidirectional);

            // Return a completed ValueTask since the underlying call is synchronous.
            return new ValueTask<ServiceResult>(result);
        }

        /// <inheritdoc/>
        public ValueTask<object> GetManagerHandleAsync(NodeId nodeId, CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaStandardAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.GetManagerHandleAsync(nodeId, cancellationToken);
            }

            object handle = SyncNodeManager.GetManagerHandle(nodeId);

            // Return a completed ValueTask since the underlying call is synchronous.
            return new ValueTask<object>(handle);
        }

        /// <inheritdoc/>
        public ValueTask<UaNodeMetadata> GetNodeMetadataAsync(
            UaServerOperationContext context,
            object targetHandle,
            BrowseResultMask resultMask,
            CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaStandardAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.GetNodeMetadataAsync(context, targetHandle, resultMask, cancellationToken);
            }

            UaNodeMetadata nodeMetadata = SyncNodeManager.GetNodeMetadata(context, targetHandle, resultMask);

            // Return a completed ValueTask since the underlying call is synchronous.
            return new ValueTask<UaNodeMetadata>(nodeMetadata);
        }

        /// <inheritdoc/>
        public ValueTask<UaNodeMetadata> GetPermissionMetadataAsync(
            UaServerOperationContext context,
            object targetHandle,
            BrowseResultMask resultMask,
            Dictionary<NodeId, List<object>> uniqueNodesServiceAttributesCache,
            bool permissionsOnly,
            CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaStandardAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.GetPermissionMetadataAsync(
                    context,
                    targetHandle,
                    resultMask,
                    uniqueNodesServiceAttributesCache,
                    permissionsOnly,
                    cancellationToken);
            }
            if (SyncNodeManager is IUaStandardNodeManager nodeManager2)
            {
                UaNodeMetadata nodeMetadata = nodeManager2.GetPermissionMetadata(
                    context,
                    targetHandle,
                    resultMask,
                    uniqueNodesServiceAttributesCache,
                    permissionsOnly);
                return new ValueTask<UaNodeMetadata>(nodeMetadata);
            }

            return new ValueTask<UaNodeMetadata>((UaNodeMetadata)null);
        }

        /// <inheritdoc/>
        public ValueTask HistoryReadAsync(
            UaServerOperationContext context,
            HistoryReadDetails details,
            TimestampsToReturn timestampsToReturn,
            bool releaseContinuationPoints,
            IList<HistoryReadValueId> nodesToRead,
            IList<HistoryReadResult> results,
            IList<ServiceResult> errors,
            CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaHistoryReadAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.HistoryReadAsync(context,
                                                         details,
                                                         timestampsToReturn,
                                                         releaseContinuationPoints,
                                                         nodesToRead,
                                                         results,
                                                         errors,
                                                         cancellationToken);
            }

            SyncNodeManager.HistoryRead(context, details, timestampsToReturn, releaseContinuationPoints, nodesToRead, results, errors);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask HistoryUpdateAsync(
            UaServerOperationContext context,
            Type detailsType,
            IList<HistoryUpdateDetails> nodesToUpdate,
            IList<HistoryUpdateResult> results,
            IList<ServiceResult> errors,
            CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaHistoryUpdateAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.HistoryUpdateAsync(context,
                                                           detailsType,
                                                           nodesToUpdate,
                                                           results,
                                                           errors,
                                                           cancellationToken);
            }

            SyncNodeManager.HistoryUpdate(context, detailsType, nodesToUpdate, results, errors);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask<bool> IsNodeInViewAsync(
            UaServerOperationContext context,
            NodeId viewId,
            object nodeHandle,
            CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaStandardAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.IsNodeInViewAsync(context, viewId, nodeHandle, cancellationToken);
            }

            if (SyncNodeManager is IUaStandardNodeManager nodeManager2)
            {
                bool result = nodeManager2.IsNodeInView(context, viewId, nodeHandle);
                return new ValueTask<bool>(result);
            }

            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ValueTask ModifyMonitoredItemsAsync(UaServerOperationContext context,
                                                   TimestampsToReturn timestampsToReturn,
                                                   IList<IUaMonitoredItem> monitoredItems,
                                                   IList<MonitoredItemModifyRequest> itemsToModify,
                                                   IList<ServiceResult> errors,
                                                   IList<MonitoringFilterResult> filterErrors,
                                                   CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaModifyMonitoredItemsAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.ModifyMonitoredItemsAsync(
                    context,
                    timestampsToReturn,
                    monitoredItems,
                    itemsToModify,
                    errors,
                    filterErrors,
                    cancellationToken);
            }

            SyncNodeManager.ModifyMonitoredItems(context, timestampsToReturn, monitoredItems, itemsToModify, errors, filterErrors);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask ReadAsync(UaServerOperationContext context,
                                   double maxAge,
                                   IList<ReadValueId> nodesToRead,
                                   IList<DataValue> values,
                                   IList<ServiceResult> errors,
                                   CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaReadAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.ReadAsync(context, maxAge, nodesToRead, values, errors, cancellationToken);
            }

            SyncNodeManager.Read(context, maxAge, nodesToRead, values, errors);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask RestoreMonitoredItemsAsync(IList<IUaStoredMonitoredItem> itemsToRestore,
                                                    IList<IUaMonitoredItem> monitoredItems,
                                                    IUserIdentity savedOwnerIdentity,
                                                    CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaStandardAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.RestoreMonitoredItemsAsync(itemsToRestore, monitoredItems, savedOwnerIdentity, cancellationToken);
            }

            SyncNodeManager.RestoreMonitoredItems(itemsToRestore, monitoredItems, savedOwnerIdentity);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask SessionClosingAsync(
            UaServerOperationContext context,
            NodeId sessionId,
            bool deleteSubscriptions,
            CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaStandardAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.SessionClosingAsync(context, sessionId, deleteSubscriptions, cancellationToken);
            }

            if (SyncNodeManager is IUaStandardNodeManager nodeManager2)
            {
                nodeManager2.SessionClosing(context, sessionId, deleteSubscriptions);
            }

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask SetMonitoringModeAsync(UaServerOperationContext context,
                                                MonitoringMode monitoringMode,
                                                IList<IUaMonitoredItem> monitoredItems,
                                                IList<bool> processedItems,
                                                IList<ServiceResult> errors,
                                                CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaSetMonitoringModeAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.SetMonitoringModeAsync(context, monitoringMode, monitoredItems, processedItems, errors, cancellationToken);
            }

            SyncNodeManager.SetMonitoringMode(context, monitoringMode, monitoredItems, processedItems, errors);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask<ServiceResult> SubscribeToAllEventsAsync(UaServerOperationContext context,
                                                                  uint subscriptionId,
                                                                  IUaEventMonitoredItem monitoredItem,
                                                                  bool unsubscribe,
                                                                  CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaStandardAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.SubscribeToAllEventsAsync(context, subscriptionId, monitoredItem, unsubscribe, cancellationToken);
            }

            ServiceResult result = SyncNodeManager.SubscribeToAllEvents(context, subscriptionId, monitoredItem, unsubscribe);

            // Return a completed ValueTask since the underlying call is synchronous.
            return new ValueTask<ServiceResult>(result);
        }

        /// <inheritdoc/>
        public ValueTask<ServiceResult> SubscribeToEventsAsync(UaServerOperationContext context,
                                                               object sourceId,
                                                               uint subscriptionId,
                                                               IUaEventMonitoredItem monitoredItem,
                                                               bool unsubscribe,
                                                               CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaStandardAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.SubscribeToEventsAsync(context, sourceId, subscriptionId, monitoredItem, unsubscribe, cancellationToken);
            }

            ServiceResult result = SyncNodeManager.SubscribeToEvents(context, sourceId, subscriptionId, monitoredItem, unsubscribe);

            // Return a completed ValueTask since the underlying call is synchronous.
            return new ValueTask<ServiceResult>(result);
        }

        /// <inheritdoc/>
        public ValueTask TransferMonitoredItemsAsync(UaServerOperationContext context,
                                                     bool sendInitialValues,
                                                     IList<IUaMonitoredItem> monitoredItems,
                                                     IList<bool> processedItems,
                                                     IList<ServiceResult> errors,
                                                     CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaTransferMonitoredItemsAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.TransferMonitoredItemsAsync(context, sendInitialValues, monitoredItems, processedItems, errors, cancellationToken);
            }

            SyncNodeManager.TransferMonitoredItems(context, sendInitialValues, monitoredItems, processedItems, errors);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask TranslateBrowsePathAsync(
            UaServerOperationContext context,
            object sourceHandle,
            RelativePathElement relativePath,
            IList<ExpandedNodeId> targetIds,
            IList<NodeId> unresolvedTargetIds,
            CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaTranslateBrowsePathAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.TranslateBrowsePathAsync(context, sourceHandle, relativePath, targetIds, unresolvedTargetIds, cancellationToken);
            }

            SyncNodeManager.TranslateBrowsePath(context, sourceHandle, relativePath, targetIds, unresolvedTargetIds);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <inheritdoc/>
        public ValueTask WriteAsync(
            UaServerOperationContext context,
            IList<WriteValue> nodesToWrite,
            IList<ServiceResult> errors,
            CancellationToken cancellationToken = default)
        {
            if (SyncNodeManager is IUaWriteAsyncNodeManager asyncNodeManager)
            {
                return asyncNodeManager.WriteAsync(context, nodesToWrite, errors, cancellationToken);
            }

            SyncNodeManager.Write(context, nodesToWrite, errors);

            // Return a completed ValueTask since the underlying call is synchronous.
            return default;
        }

        /// <summary>
        /// Frees any unmanaged resources.
        /// </summary>
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
                Utils.SilentDispose(SyncNodeManager);
            }
        }
    }
}
