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
    /// Cache implementation for Node cache
    /// </summary>
    public interface IUaLruNodeCache
    {
        /// <summary>
        /// The namespaces used in the server
        /// </summary>
        NamespaceTable NamespaceUris { get; }

        /// <summary>
        /// Get node from cache
        /// </summary>
        ValueTask<INode> GetNodeAsync(NodeId nodeId, CancellationToken ct = default);

        /// <summary>
        /// Get nodes from cache
        /// </summary>
        ValueTask<IReadOnlyList<INode>> GetNodesAsync(
            IReadOnlyList<NodeId> nodeIds,
            CancellationToken ct = default);

        /// <summary>
        /// Get node using browse path
        /// </summary>
        ValueTask<INode?> GetNodeWithBrowsePathAsync(
            NodeId nodeId,
            QualifiedNameCollection browsePath,
            CancellationToken ct = default);

        /// <summary>
        /// Get list of references for a node
        /// </summary>
        ValueTask<IReadOnlyList<INode>> GetReferencesAsync(
            IReadOnlyList<NodeId> nodeIds,
            IReadOnlyList<NodeId> referenceTypeIds,
            bool isInverse,
            bool includeSubtypes = true,
            CancellationToken ct = default);

        /// <summary>
        /// Get references for a node
        /// </summary>
        ValueTask<IReadOnlyList<INode>> GetReferencesAsync(
            NodeId nodeId,
            NodeId referenceTypeId,
            bool isInverse,
            bool includeSubtypes = true,
            CancellationToken ct = default);

        /// <summary>
        /// Get super type of a type node.
        /// </summary>
        ValueTask<NodeId> GetSuperTypeAsync(NodeId typeId, CancellationToken ct = default);

        /// <summary>
        /// Get value of a node from cache
        /// </summary>
        ValueTask<DataValue> GetValueAsync(NodeId nodeId, CancellationToken ct = default);

        /// <summary>
        /// Get values of nodes from cache
        /// </summary>
        ValueTask<IReadOnlyList<DataValue>> GetValuesAsync(
            IReadOnlyList<NodeId> nodeIds,
            CancellationToken ct = default);

        /// <summary>
        /// Get built in type for a datatype id.
        /// </summary>
        ValueTask<BuiltInType> GetBuiltInTypeAsync(
            NodeId datatypeId,
            CancellationToken ct = default);

        /// <summary>
        /// Load the type hierarchy for a list of type ids.
        /// </summary>
        ValueTask LoadTypeHierarchyAsync(
            IReadOnlyList<NodeId> typeIds,
            CancellationToken ct = default);

        /// <summary>
        /// Check if node is type of a type using the cache.
        /// Best to load the type hierarchy first.
        /// </summary>
        bool IsTypeOf(NodeId subTypeId, NodeId superTypeId);

        /// <summary>
        /// Clear the cache
        /// </summary>
        void Clear();
    }
}
