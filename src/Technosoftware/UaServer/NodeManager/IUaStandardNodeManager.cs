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
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// An interface to an object that manages a set of nodes in the address space.
    /// </summary>
    public interface IUaStandardNodeManager : IUaNodeManager
    {
        /// <summary>
        /// Called when the session is closed.
        /// </summary>
        void SessionClosing(UaServerOperationContext context, NodeId sessionId, bool deleteSubscriptions);

        /// <summary>
        /// Returns true if the node is in the view.
        /// </summary>
        bool IsNodeInView(UaServerOperationContext context, NodeId viewId, object nodeHandle);

        /// <summary>
        /// Returns the metadata needed for validating permissions, associated with the node with
        /// the option to optimize services by using a cache.
        /// </summary>
        /// <remarks>
        /// Returns null if the node does not exist.
        /// It should return null in case the implementation wishes to handover the task to the parent INodeManager.GetNodeMetadata
        /// </remarks>
        UaNodeMetadata GetPermissionMetadata(
            UaServerOperationContext context,
            object targetHandle,
            BrowseResultMask resultMask,
            Dictionary<NodeId, List<object>> uniqueNodesServiceAttributesCache,
            bool permissionsOnly);
    }

    /// <summary>
    /// An asynchronous version of the "Call" method defined on the <see cref="IUaStandardNodeManager"/> interface.
    /// </summary>
    public interface IUaCallAsyncNodeManager
    {
        /// <summary>
        /// Asynchronously calls a method defined on an object.
        /// </summary>
        ValueTask CallAsync(
            UaServerOperationContext context,
            IList<CallMethodRequest> methodsToCall,
            IList<CallMethodResult> results,
            IList<ServiceResult> errors,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An asynchronous version of the "Read" method defined on the <see cref="IUaStandardNodeManager"/> interface.
    /// </summary>
    public interface IUaReadAsyncNodeManager
    {
        /// <summary>
        /// Reads the attribute values for a set of nodes.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The MasterNodeManager pre-processes the nodesToRead and ensures that:
        ///    - the AttributeId is a known attribute.
        ///    - the IndexRange, if specified, is valid.
        ///    - the DataEncoding and the IndexRange are not specified if the AttributeId is not Value.
        /// </para>
        /// <para>
        /// The MasterNodeManager post-processes the values by:
        ///    - sets values[ii].StatusCode to the value of errors[ii].Code
        ///    - creates a instance of DataValue if one does not exist and an errors[ii] is bad.
        ///    - removes timestamps from the DataValue if the client does not want them.
        /// </para>
        /// <para>
        /// The node manager must ignore ReadValueId with the Processed flag set to true.
        /// The node manager must set the Processed flag for any ReadValueId that it processes.
        /// </para>
        /// </remarks>
        ValueTask ReadAsync(
            UaServerOperationContext context,
            double maxAge,
            IList<ReadValueId> nodesToRead,
            IList<DataValue> values,
            IList<ServiceResult> errors,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An asynchronous version of the "Write" method defined on the <see cref="IUaStandardNodeManager"/> interface.
    /// </summary>
    public interface IUaWriteAsyncNodeManager
    {
        /// <summary>
        /// Writes a set of values.
        /// </summary>
        /// <remarks>
        /// Each node manager should only process node ids that it recognizes. If it processes a value it
        /// must set the Processed flag in the WriteValue structure.
        /// </remarks>
        ValueTask WriteAsync(
            UaServerOperationContext context,
            IList<WriteValue> nodesToWrite,
            IList<ServiceResult> errors,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An asynchronous version of the "HistoryRead" method defined on the <see cref="IUaStandardNodeManager"/> interface.
    /// </summary>
    public interface IUaHistoryReadAsyncNodeManager
    {
        /// <summary>
        /// Reads the history of a set of items.
        /// </summary>
        ValueTask HistoryReadAsync(
            UaServerOperationContext context,
            HistoryReadDetails details,
            TimestampsToReturn timestampsToReturn,
            bool releaseContinuationPoints,
            IList<HistoryReadValueId> nodesToRead,
            IList<HistoryReadResult> results,
            IList<ServiceResult> errors,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An asynchronous version of the "HistoryUpdate" method defined on the <see cref="IUaStandardNodeManager"/> interface.
    /// </summary>
    public interface IUaHistoryUpdateAsyncNodeManager
    {
        /// <summary>
        /// Updates the history for a set of nodes.
        /// </summary>
        ValueTask HistoryUpdateAsync(
            UaServerOperationContext context,
            Type detailsType,
            IList<HistoryUpdateDetails> nodesToUpdate,
            IList<HistoryUpdateResult> results,
            IList<ServiceResult> errors,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An asynchronous version of the "ConditionRefresh" method defined on the <see cref="IUaStandardNodeManager"/> interface.
    /// </summary>
    public interface IUaConditionRefreshAsyncNodeManager
    {
        /// <summary>
        /// Tells the NodeManager to refresh any conditions.
        /// </summary>
        ValueTask ConditionRefreshAsync(
            UaServerOperationContext context,
            IList<IUaEventMonitoredItem> monitoredItems,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An asynchronous version of the "TranslateBrowsePath" method defined on the <see cref="IUaStandardNodeManager"/> interface.
    /// </summary>
    public interface IUaTranslateBrowsePathAsyncNodeManager
    {
        /// <summary>
        /// Finds the targets of the relative path from the source node.
        /// </summary>
        /// <param name="context">The context to used when processing the request.</param>
        /// <param name="sourceHandle">The handle for the source node.</param>
        /// <param name="relativePath">The relative path to follow.</param>
        /// <param name="targetIds">The NodeIds for any target at the end of the relative path.</param>
        /// <param name="unresolvedTargetIds">The NodeIds for any local target that is in another NodeManager.</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <remarks>
        /// A null context indicates that the server's internal logic is making the call.
        /// The first target in the list must be the target that matches the instance declaration (if applicable).
        /// Any local targets that belong to other NodeManagers are returned as unresolvedTargetIds.
        /// The caller must check the BrowseName to determine if it matches the relativePath.
        /// The implementor must not throw an exception if the source or target nodes do not exist.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the sourceHandle, relativePath or targetIds parameters are null.</exception>
        ValueTask TranslateBrowsePathAsync(
            UaServerOperationContext context,
            object sourceHandle,
            RelativePathElement relativePath,
            IList<ExpandedNodeId> targetIds,
            IList<NodeId> unresolvedTargetIds,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An asynchronous version of the "Browse" method defined on the <see cref="IUaStandardNodeManager"/> interface.
    /// </summary>
    public interface IUaBrowseAsyncNodeManager
    {
        /// <summary>
        /// Returns the set of references that meet the filter criteria.
        /// </summary>
        /// <param name="context">The context to used when processing the request.</param>
        /// <param name="continuationPoint">The continuation point that stores the state of the Browse operation.</param>
        /// <param name="references">The list of references that meet the filter criteria.</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <remarks>
        /// NodeManagers will likely have references to other NodeManagers which means they will not be able
        /// to apply the NodeClassMask or fill in the attributes for the target Node. In these cases the
        /// NodeManager must return a ReferenceDescription with the NodeId and ReferenceTypeId set. The caller will
        /// be responsible for filling in the target attributes.
        /// The references parameter may already contain references when the method is called. The implementer must
        /// include these references when calculating whether a continuation point must be returned.
        /// </remarks>
        /// <returns>The continuation point that stores the state of the Browse operation or null if there are no more references to return.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the context, continuationPoint or references parameters are null.</exception>
        /// <exception cref="ServiceResultException">Thrown if an error occurs during processing.</exception>
        ValueTask<UaContinuationPoint> BrowseAsync(
            UaServerOperationContext context,
            UaContinuationPoint continuationPoint,
            IList<ReferenceDescription> references,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An asynchronous version of the "SetMonitoringMode" method defined on the <see cref="IUaStandardNodeManager"/> interface.
    /// </summary>
    public interface IUaSetMonitoringModeAsyncNodeManager
    {
        /// <summary>
        /// Changes the monitoring mode for a set of monitored items.
        /// </summary>
        ValueTask SetMonitoringModeAsync(
            UaServerOperationContext context,
            MonitoringMode monitoringMode,
            IList<IUaMonitoredItem> monitoredItems,
            IList<bool> processedItems,
            IList<ServiceResult> errors,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An asynchronous version of the "TransferMonitoredItems" method defined on the <see cref="IUaStandardNodeManager"/> interface.
    /// </summary>
    public interface IUaTransferMonitoredItemsAsyncNodeManager
    {
        /// <summary>
        /// Transfers a set of monitored items.
        /// </summary>
        /// <remarks>
        /// Queue initial values from monitored items in the node managers.
        /// </remarks>
        ValueTask TransferMonitoredItemsAsync(
            UaServerOperationContext context,
            bool sendInitialValues,
            IList<IUaMonitoredItem> monitoredItems,
            IList<bool> processedItems,
            IList<ServiceResult> errors,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An asynchronous version of the "DeleteMonitoredItems" method defined on the <see cref="IUaStandardNodeManager"/> interface.
    /// </summary>
    public interface IUaDeleteMonitoredItemsAsyncNodeManager
    {
        /// <summary>
        /// Deletes a set of monitored items.
        /// </summary>
        ValueTask DeleteMonitoredItemsAsync(
            UaServerOperationContext context,
            IList<IUaMonitoredItem> monitoredItems,
            IList<bool> processedItems,
            IList<ServiceResult> errors,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An asynchronous version of the "ModifyMonitoredItems" method defined on the <see cref="IUaStandardNodeManager"/> interface.
    /// </summary>
    public interface IUaModifyMonitoredItemsAsyncNodeManager
    {
        /// <summary>
        /// Modifies a set of monitored items.
        /// </summary>
        ValueTask ModifyMonitoredItemsAsync(
            UaServerOperationContext context,
            TimestampsToReturn timestampsToReturn,
            IList<IUaMonitoredItem> monitoredItems,
            IList<MonitoredItemModifyRequest> itemsToModify,
            IList<ServiceResult> errors,
            IList<MonitoringFilterResult> filterErrors,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An asynchronous version of the "CreateMonitoredItems" method defined on the <see cref="IUaStandardNodeManager"/> interface.
    /// </summary>
    public interface IUaCreateMonitoredItemsAsyncNodeManager
    {
        /// <summary>
        /// Creates a set of monitored items.
        /// </summary>
        ValueTask CreateMonitoredItemsAsync(
            UaServerOperationContext context,
            uint subscriptionId,
            double publishingInterval,
            TimestampsToReturn timestampsToReturn,
            IList<MonitoredItemCreateRequest> itemsToCreate,
            IList<ServiceResult> errors,
            IList<MonitoringFilterResult> filterErrors,
            IList<IUaMonitoredItem> monitoredItems,
            bool createDurable,
            MonitoredItemIdFactory monitoredItemIdFactory,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An interface to an object that creates a IAsyncNodeManager object.
    /// </summary>
    public interface IUaAsyncNodeManagerFactory
    {
        /// <summary>
        /// The IAsyncNodeManager factory.
        /// </summary>
        /// <param name="server">The server instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="cancellationToken">The cancellation token</param>
        ValueTask<IUaStandardAsyncNodeManager> CreateAsync(
            IUaServerData server,
            ApplicationConfiguration configuration,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// The namespace table of the NodeManager.
        /// </summary>
        StringCollection NamespacesUris { get; }
    }

    /// <summary>
    /// An asynchronous verison of the <see cref="IUaStandardNodeManager"/> interface.
    /// This interface is in active development and will be extended in future releases.
    /// Please use the sub interfaces to implement async support for specific service calls.
    /// </summary>
    [Experimental("UA_NETStandard_1")]
    public interface IUaStandardAsyncNodeManager :
        IUaCallAsyncNodeManager,
        IUaReadAsyncNodeManager,
        IUaWriteAsyncNodeManager,
        IUaHistoryReadAsyncNodeManager,
        IUaHistoryUpdateAsyncNodeManager,
        IUaConditionRefreshAsyncNodeManager,
        IUaTranslateBrowsePathAsyncNodeManager,
        IUaBrowseAsyncNodeManager,
        IUaSetMonitoringModeAsyncNodeManager,
        IUaTransferMonitoredItemsAsyncNodeManager,
        IUaDeleteMonitoredItemsAsyncNodeManager,
        IUaModifyMonitoredItemsAsyncNodeManager,
        IUaCreateMonitoredItemsAsyncNodeManager
    {
        /// <summary>
        /// Returns the NamespaceUris for the Nodes belonging to the NodeManager.
        /// </summary>
        /// <remarks>
        /// <para>By default the MasterNodeManager uses the namespaceIndex to determine who owns an Node.</para>
        /// <para>
        /// Servers that do not wish to partition their address space this way must provide their own
        /// implementation of MasterNodeManager.GetManagerHandle().
        /// </para>
        /// <para>NodeManagers which depend on a custom partitioning scheme must return a null value.</para>
        /// </remarks>
        IEnumerable<string> NamespaceUris { get; }

        /// <summary>
        /// Shall never be null.
        /// Allows access to synchronous methods for compatibility
        /// If the NodeManager does not support synchronous calls natively use <see cref="SyncNodeManagerAdapter"/>
        /// to wrap the async calls.
        /// </summary>
        IUaNodeManager SyncNodeManager { get; }

        /// <summary>
        /// Creates the address space by loading any configuration information an connecting to an underlying system (if applicable).
        /// </summary>
        /// <returns>A table of references that need to be added to other node managers.</returns>
        /// <remarks>
        /// A node manager owns a set of nodes. These nodes may be known in advance or they may be stored in an
        /// external system are retrived on demand. These nodes may have two way references to nodes that are owned
        /// by other node managers. In these cases, the node managers only manage one half of those references. The
        /// other half of the reference should be returned to the MasterNodeManager.
        /// </remarks>
        ValueTask CreateAddressSpaceAsync(
            IDictionary<NodeId, IList<IReference>> externalReferences,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the metadata associated with the node.
        /// </summary>
        /// <remarks>
        /// Returns null if the node does not exist.
        /// </remarks>
        ValueTask<UaNodeMetadata> GetNodeMetadataAsync(
            UaServerOperationContext context,
            object targetHandle,
            BrowseResultMask resultMask,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the address by releasing all resources and disconnecting from any underlying system.
        /// </summary>
        ValueTask DeleteAddressSpaceAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns an opaque handle identifying to the node to the node manager.
        /// </summary>
        /// <returns>A node handle, null if the node manager does not recognize the node id.</returns>
        /// <remarks>
        /// The method must not block by querying an underlying system. If the node manager wraps an
        /// underlying system then it must check to see if it recognizes the syntax of the node id.
        /// The handle in this case may simply be a partially parsed version of the node id.
        /// </remarks>
        ValueTask<object> GetManagerHandleAsync(NodeId nodeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds references to the node manager.
        /// </summary>
        /// <remarks>
        /// The node manager checks the dictionary for nodes that it owns and ensures the associated references exist.
        /// </remarks>
        ValueTask AddReferencesAsync(
            IDictionary<NodeId, IList<IReference>> references,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a reference.
        /// </summary>
        ValueTask<ServiceResult> DeleteReferenceAsync(
            object sourceHandle,
            NodeId referenceTypeId,
            bool isInverse,
            ExpandedNodeId targetId,
            bool deleteBidirectional,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Called when the session is closed.
        /// </summary>
        ValueTask SessionClosingAsync(
            UaServerOperationContext context,
            NodeId sessionId,
            bool deleteSubscriptions,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns true if the node is in the view.
        /// </summary>
        ValueTask<bool> IsNodeInViewAsync(
            UaServerOperationContext context,
            NodeId viewId,
            object nodeHandle,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the metadata needed for validating permissions, associated with the node with
        /// the option to optimize services by using a cache.
        /// </summary>
        /// <remarks>
        /// Returns null if the node does not exist.
        /// It should return null in case the implementation wishes to handover the task to the parent INodeManager.GetNodeMetadata
        /// </remarks>
        ValueTask<UaNodeMetadata> GetPermissionMetadataAsync(
            UaServerOperationContext context,
            object targetHandle,
            BrowseResultMask resultMask,
            Dictionary<NodeId, List<object>> uniqueNodesServiceAttributesCache,
            bool permissionsOnly,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Tells the NodeManager to report events from the specified notifier.
        /// </summary>
        /// <remarks>
        /// This method may be called multiple times for the name monitoredItemId if the
        /// context for that MonitoredItem changes (i.e. UserIdentity and/or Locales).
        /// </remarks>
        ValueTask<ServiceResult> SubscribeToEventsAsync(
            UaServerOperationContext context,
            object sourceId,
            uint subscriptionId,
            IUaEventMonitoredItem monitoredItem,
            bool unsubscribe,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Tells the NodeManager to report events all events from all sources.
        /// </summary>
        /// <remarks>
        /// This method may be called multiple times for the name monitoredItemId if the
        /// context for that MonitoredItem changes (i.e. UserIdentity and/or Locales).
        /// </remarks>
        ValueTask<ServiceResult> SubscribeToAllEventsAsync(
            UaServerOperationContext context,
            uint subscriptionId,
            IUaEventMonitoredItem monitoredItem,
            bool unsubscribe,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Restore a set of monitored items after a restart.
        /// </summary>
        ValueTask RestoreMonitoredItemsAsync(
            IList<IUaStoredMonitoredItem> itemsToRestore,
            IList<IUaMonitoredItem> monitoredItems,
            IUserIdentity savedOwnerIdentity,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Stores metadata required to process requests related to a node.
    /// </summary>
    public class UaNodeMetadata
    {
        /// <summary>
        /// Initializes the object with its handle and NodeId.
        /// </summary>
        public UaNodeMetadata(object handle, NodeId nodeId)
        {
            Handle = handle;
            NodeId = nodeId;
        }

        /// <summary>
        /// The handle assigned by the NodeManager that owns the Node.
        /// </summary>
        public object Handle { get; }

        /// <summary>
        /// The canonical NodeId for the Node.
        /// </summary>
        public NodeId NodeId { get; }

        /// <summary>
        /// The NodeClass for the Node.
        /// </summary>
        public NodeClass NodeClass { get; set; }

        /// <summary>
        /// The BrowseName for the Node.
        /// </summary>
        public QualifiedName BrowseName { get; set; }

        /// <summary>
        /// The DisplayName for the Node.
        /// </summary>
        public LocalizedText DisplayName { get; set; }

        /// <summary>
        /// The type definition for the Node (if one exists).
        /// </summary>
        public ExpandedNodeId TypeDefinition { get; set; }

        /// <summary>
        /// The modelling for the Node (if one exists).
        /// </summary>
        public NodeId ModellingRule { get; set; }

        /// <summary>
        /// Specifies which attributes are writeable.
        /// </summary>
        public AttributeWriteMask WriteMask { get; set; }

        /// <summary>
        /// Whether the Node can be used with event subscriptions or for historial event queries.
        /// </summary>
        public byte EventNotifier { get; set; }

        /// <summary>
        /// Whether the Node can be use to read or write current or historical values.
        /// </summary>
        public byte AccessLevel { get; set; }

        /// <summary>
        /// Whether the Node is a Method that can be executed.
        /// </summary>
        public bool Executable { get; set; }

        /// <summary>
        /// The DataType of the Value attribute for Variable or VariableType nodes.
        /// </summary>
        public NodeId DataType { get; set; }

        /// <summary>
        /// The ValueRank for the Value attribute for Variable or VariableType nodes.
        /// </summary>
        public int ValueRank { get; set; }

        /// <summary>
        /// The ArrayDimensions for the Value attribute for Variable or VariableType nodes.
        /// </summary>
        public IList<uint> ArrayDimensions { get; set; }

        /// <summary>
        /// Specifies the AccessRestrictions that apply to a Node.
        /// </summary>
        public AccessRestrictionType AccessRestrictions { get; set; }

        /// <summary>
        /// The value reflects the DefaultAccessRestrictions Property of the NamespaceMetadata Object for the Namespace
        /// to which the Node belongs.
        /// </summary>
        public AccessRestrictionType DefaultAccessRestrictions { get; set; }

        /// <summary>
        /// The RolePermissions for the Node.
        /// Specifies the Permissions that apply to a Node for all Roles which have access to the Node.
        /// </summary>
        public RolePermissionTypeCollection RolePermissions { get; set; }

        /// <summary>
        /// The DefaultRolePermissions of the Node's name-space meta-data
        /// The value reflects the DefaultRolePermissions Property from the NamespaceMetadata Object associated with the Node.
        /// </summary>
        public RolePermissionTypeCollection DefaultRolePermissions { get; set; }

        /// <summary>
        /// The UserRolePermissions of the Node.
        /// Specifies the Permissions that apply to a Node for all Roles granted to current Session.
        /// </summary>
        public RolePermissionTypeCollection UserRolePermissions { get; set; }

        /// <summary>
        /// The DefaultUserRolePermissions of the Node.
        /// The value reflects the DefaultUserRolePermissions Property from the NamespaceMetadata Object associated with the Node.
        /// </summary>
        public RolePermissionTypeCollection DefaultUserRolePermissions { get; set; }
    }
}
