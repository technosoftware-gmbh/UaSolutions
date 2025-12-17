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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaClient
{
    /// <summary>
    /// Extensions for lru node cache
    /// </summary>
    public static class LruNodeCacheExtensions
    {
        /// <summary>
        /// Get node from cache
        /// </summary>
        public static ValueTask<INode> GetNodeAsync(
            this IUaLruNodeCache cache,
            ExpandedNodeId expandedNodeId,
            CancellationToken ct = default)
        {
            var nodeId = ExpandedNodeId.ToNodeId(expandedNodeId, cache.NamespaceUris);
            return cache.GetNodeAsync(nodeId, ct);
        }

        /// <summary>
        /// Get nodes from cache
        /// </summary>
        public static ValueTask<IReadOnlyList<INode>> GetNodesAsync(
            this IUaLruNodeCache cache,
            IReadOnlyList<ExpandedNodeId> expandedNodeIds,
            CancellationToken ct = default)
        {
            var nodeIds = expandedNodeIds
                .Select(expandedNodeId => ExpandedNodeId.ToNodeId(
                    expandedNodeId,
                    cache.NamespaceUris))
                .ToList();
            return cache.GetNodesAsync(nodeIds, ct);
        }

        /// <summary>
        /// Get node from cache
        /// </summary>
        public static ValueTask<DataValue> GetValueAsync(
            this IUaLruNodeCache cache,
            ExpandedNodeId expandedNodeId,
            CancellationToken ct = default)
        {
            var nodeId = ExpandedNodeId.ToNodeId(expandedNodeId, cache.NamespaceUris);
            return cache.GetValueAsync(nodeId, ct);
        }

        /// <summary>
        /// Get nodes from cache
        /// </summary>
        public static ValueTask<IReadOnlyList<DataValue>> GetValuesAsync(
            this IUaLruNodeCache cache,
            IReadOnlyList<ExpandedNodeId> expandedNodeIds,
            CancellationToken ct = default)
        {
            var nodeIds = expandedNodeIds
                .Select(expandedNodeId => ExpandedNodeId.ToNodeId(
                    expandedNodeId,
                    cache.NamespaceUris))
                .ToList();
            return cache.GetValuesAsync(nodeIds, ct);
        }

        /// <summary>
        /// Get references from cache
        /// </summary>
        public static ValueTask<IReadOnlyList<INode>> GetReferencesAsync(
            this IUaLruNodeCache cache,
            ExpandedNodeId expandedNodeId,
            NodeId referenceTypeId,
            bool isInverse,
            bool includeSubtypes = true,
            CancellationToken ct = default)
        {
            var nodeId = ExpandedNodeId.ToNodeId(expandedNodeId, cache.NamespaceUris);
            return cache.GetReferencesAsync(
                nodeId,
                referenceTypeId,
                isInverse,
                includeSubtypes,
                ct);
        }

        /// <summary>
        /// Get references from cache
        /// </summary>
        public static ValueTask<IReadOnlyList<INode>> GetReferencesAsync(
            this IUaLruNodeCache cache,
            IReadOnlyList<ExpandedNodeId> expandedNodeIds,
            IReadOnlyList<NodeId> referenceTypeIds,
            bool isInverse,
            bool includeSubtypes = true,
            CancellationToken ct = default)
        {
            var nodeIds = expandedNodeIds
                .Select(expandedNodeId => ExpandedNodeId.ToNodeId(
                    expandedNodeId,
                    cache.NamespaceUris))
                .ToList();
            return cache.GetReferencesAsync(
                nodeIds,
                referenceTypeIds,
                isInverse,
                includeSubtypes,
                ct);
        }

        /// <summary>
        /// Get references from cache
        /// </summary>
        public static ValueTask<IReadOnlyList<INode>> GetReferencesAsync(
            this IUaLruNodeCache cache,
            IReadOnlyList<ExpandedNodeId> expandedNodeIds,
            NodeId referenceTypeId,
            bool isInverse,
            bool includeSubtypes = true,
            CancellationToken ct = default)
        {
            var nodeIds = expandedNodeIds
                .Select(expandedNodeId => ExpandedNodeId.ToNodeId(
                    expandedNodeId,
                    cache.NamespaceUris))
                .ToList();
            return cache.GetReferencesAsync(
                nodeIds,
                [referenceTypeId],
                isInverse,
                includeSubtypes,
                ct);
        }

        /// <summary>
        /// Get super type from cache
        /// </summary>
        public static ValueTask<NodeId> GetSuperTypeAsync(
            this IUaLruNodeCache cache,
            ExpandedNodeId expandedNodeId,
            CancellationToken ct = default)
        {
            var nodeId = ExpandedNodeId.ToNodeId(expandedNodeId, cache.NamespaceUris);
            return cache.GetSuperTypeAsync(nodeId, ct);
        }

        /// <summary>
        /// Is the subTypeId a subtype of the superTypeId?
        /// </summary>
        public static bool IsTypeOf(
            this IUaLruNodeCache cache,
            ExpandedNodeId subTypeId,
            NodeId superTypeId)
        {
            var nodeId = ExpandedNodeId.ToNodeId(subTypeId, cache.NamespaceUris);
            return cache.IsTypeOf(nodeId, superTypeId);
        }

        /// <summary>
        /// Returns the BuiltInType type for the DataTypeId.
        /// </summary>
        public static async Task<BuiltInType> GetBuiltInTypeAsync(
            this IUaLruNodeCache cache,
            NodeId datatypeId,
            CancellationToken ct = default)
        {
            NodeId typeId = datatypeId;
            while (!NodeId.IsNull(typeId))
            {
                if (typeId.NamespaceIndex == 0 && typeId.IdType == IdType.Numeric)
                {
                    var id = (BuiltInType)(int)(uint)typeId.Identifier;
                    if (id is > BuiltInType.Null and <= BuiltInType.Enumeration and not BuiltInType.DiagnosticInfo)
                    {
                        return id;
                    }
                }
                typeId = await cache.GetSuperTypeAsync(typeId, ct).ConfigureAwait(false);
            }
            return BuiltInType.Null;
        }
    }
}
