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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaClient
{
    /// <summary>
    /// A client side cache of the server's type model.
    /// </summary>
    public interface IUaNodeCache : IAsyncNodeTable, IAsyncTypeTable
    {
        /// <summary>
        /// Loads the UA defined types into the cache.
        /// </summary>
        /// <param name="context">The context.</param>
        void LoadUaDefinedTypes(ISystemContext context);

        /// <summary>
        /// Removes all nodes from the cache.
        /// </summary>
        void Clear();

        /// <summary>
        /// Finds a set of nodes in the nodeset,
        /// fetches missing nodes from server.
        /// </summary>
        /// <param name="nodeIds">The node identifier collection.</param>
        /// <param name="ct">Cancelation token to cancel operation with</param>
        Task<IList<INode>> FindAsync(
            IList<ExpandedNodeId> nodeIds,
            CancellationToken ct = default);

        /// <summary>
        /// Fetches a node from the server and updates the cache.
        /// </summary>
        /// <param name="nodeId">Node id to fetch.</param>
        /// <param name="ct">Cancelation token to cancel operation with</param>
        Task<Node> FetchNodeAsync(
            ExpandedNodeId nodeId,
            CancellationToken ct = default);

        /// <summary>
        /// Fetches a node collection from the server and updates the cache.
        /// </summary>
        /// <param name="nodeIds">The node identifier collection.</param>
        /// <param name="ct">Cancelation token to cancel operation with</param>
        Task<IList<Node>> FetchNodesAsync(
            IList<ExpandedNodeId> nodeIds,
            CancellationToken ct = default);

        /// <summary>
        /// Adds the supertypes of the node to the cache.
        /// </summary>
        /// <param name="nodeId">Node id to fetch.</param>
        /// <param name="ct">Cancelation token to cancel operation with</param>
        Task FetchSuperTypesAsync(
            ExpandedNodeId nodeId,
            CancellationToken ct = default);

        /// <summary>
        /// Returns the references of the specified node that
        /// meet the criteria specified.
        /// </summary>
        Task<IList<INode>> FindReferencesAsync(
            ExpandedNodeId nodeId,
            NodeId referenceTypeId,
            bool isInverse,
            bool includeSubtypes,
            CancellationToken ct = default);

        /// <summary>
        /// Returns the references of the specified nodes that
        /// meet the criteria specified.
        /// </summary>
        Task<IList<INode>> FindReferencesAsync(
            IList<ExpandedNodeId> nodeIds,
            IList<NodeId> referenceTypeIds,
            bool isInverse,
            bool includeSubtypes,
            CancellationToken ct = default);

        /// <summary>
        /// Returns a display name for a node.
        /// </summary>
        ValueTask<string> GetDisplayTextAsync(
            INode node,
            CancellationToken ct = default);

        /// <summary>
        /// Returns a display name for a node.
        /// </summary>
        ValueTask<string> GetDisplayTextAsync(
            ExpandedNodeId nodeId,
            CancellationToken ct = default);

        /// <summary>
        /// Returns a display name for the target of a reference.
        /// </summary>
        ValueTask<string> GetDisplayTextAsync(
            ReferenceDescription reference,
            CancellationToken ct = default);

        /// <summary>
        /// Builds the relative path from a type to a node.
        /// </summary>
        NodeId BuildBrowsePath(
            ILocalNode node,
            IList<QualifiedName> browsePath);
    }
}
