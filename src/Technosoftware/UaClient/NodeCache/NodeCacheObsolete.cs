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

#nullable disable
#region Using Directives
using System;
using System.Collections.Generic;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaClient
{
    /// <summary>
    /// A client side cache of the server's type model.
    /// </summary>
    public static class NodeCacheObsolete
    {
        /// <summary>
        /// Fetches a node from the server and updates the cache.
        /// </summary>
        [Obsolete("Use FetchNodeAsync instead.")]
        public static Node FetchNode(
            this IUaNodeCache nodeCache,
            ExpandedNodeId nodeId)
        {
            return nodeCache.FetchNodeAsync(nodeId)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Finds a set of nodes in the nodeset, fetches missing
        /// nodes from server.
        /// </summary>
        [Obsolete("Use FindAsync instead.")]
        public static IList<INode> Find(
            this IUaNodeCache nodeCache,
            IList<ExpandedNodeId> nodeIds)
        {
            return nodeCache.FindAsync(nodeIds)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Fetches a node collection from the server and updates the cache.
        /// </summary>
        [Obsolete("Use FetchNodesAsync instead.")]
        public static IList<Node> FetchNodes(
            this IUaNodeCache nodeCache,
            IList<ExpandedNodeId> nodeIds)
        {
            return nodeCache.FetchNodesAsync(nodeIds)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Adds the supertypes of the node to the cache.
        /// </summary>
        [Obsolete("Use FetchSuperTypesAsync instead.")]
        public static void FetchSuperTypes(
            this IUaNodeCache nodeCache,
            ExpandedNodeId nodeId)
        {
            nodeCache.FetchSuperTypesAsync(nodeId)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Returns the references of the specified node that
        /// meet the criteria specified.
        /// </summary>
        [Obsolete("Use FindReferencesAsync instead.")]
        public static IList<INode> FindReferences(
            this IUaNodeCache nodeCache,
            ExpandedNodeId nodeId,
            NodeId referenceTypeId,
            bool isInverse,
            bool includeSubtypes)
        {
            return nodeCache.FindReferencesAsync(
                nodeId,
                referenceTypeId,
                isInverse,
                includeSubtypes)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Returns the references of the specified nodes that meet
        /// the criteria specified.
        /// </summary>
        [Obsolete("Use FindReferencesAsync instead.")]
        public static IList<INode> FindReferences(
            this IUaNodeCache nodeCache,
            IList<ExpandedNodeId> nodeIds,
            IList<NodeId> referenceTypeIds,
            bool isInverse,
            bool includeSubtypes)
        {
            return nodeCache.FindReferencesAsync(
                nodeIds,
                referenceTypeIds,
                isInverse,
                includeSubtypes)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Returns a display name for a node.
        /// </summary>
        [Obsolete("Use GetDisplayTextAsync instead.")]
        public static string GetDisplayText(
            this IUaNodeCache nodeCache,
            INode node)
        {
            return nodeCache.GetDisplayTextAsync(node)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Returns a display name for a node.
        /// </summary>
        [Obsolete("Use GetDisplayTextAsync instead.")]
        public static string GetDisplayText(
            this IUaNodeCache nodeCache,
            ExpandedNodeId nodeId)
        {
            return nodeCache.GetDisplayTextAsync(nodeId)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Returns a display name for the target of a reference.
        /// </summary>
        [Obsolete("Use GetDisplayTextAsync instead.")]
        public static string GetDisplayText(
            this IUaNodeCache nodeCache,
            ReferenceDescription reference)
        {
            return nodeCache.GetDisplayTextAsync(reference)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Determines whether a node id is a known type id.
        /// </summary>
        [Obsolete("Use IsKknownAsync instead")]
        public static bool IsKnown(
            this IUaNodeCache nodeCache,
            ExpandedNodeId typeId)
        {
            return nodeCache.IsKnownAsync(typeId)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Determines whether a node id is a known type id.
        /// </summary>
        [Obsolete("Use IsKknownAsync instead")]
        public static bool IsKnown(
            this IUaNodeCache nodeCache,
            NodeId typeId)
        {
            return nodeCache.IsKnownAsync(typeId)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Returns the immediate supertype for the type.
        /// </summary>
        [Obsolete("Use FindSuperTypeAsync instead")]
        public static NodeId FindSuperType(
            this IUaNodeCache nodeCache,
            ExpandedNodeId typeId)
        {
            return nodeCache.FindSuperTypeAsync(typeId)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Returns the immediate supertype for the type.
        /// </summary>
        [Obsolete("Use FindSuperTypeAsync instead")]
        public static NodeId FindSuperType(
            this IUaNodeCache nodeCache,
            NodeId typeId)
        {
            return nodeCache.FindSuperTypeAsync(typeId)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Returns true if the node is in the table.
        /// </summary>
        [Obsolete("Use ExistsAsync instead")]
        public static bool Exists(
            this IUaNodeCache nodeCache,
            ExpandedNodeId nodeId)
        {
            return nodeCache.ExistsAsync(nodeId)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Finds a node in the node set.
        /// </summary>
        [Obsolete("Use FindAsync instead")]
        public static INode Find(
            this IUaNodeCache nodeCache,
            ExpandedNodeId nodeId)
        {
            return nodeCache.FindAsync(nodeId)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Follows the reference from the source and returns the first target
        /// with the specified browse name.
        /// </summary>
        [Obsolete("Use FindAsync instead")]
        public static INode Find(
            this IUaNodeCache nodeCache,
            ExpandedNodeId sourceId,
            NodeId referenceTypeId,
            bool isInverse,
            bool includeSubtypes,
            QualifiedName browseName)
        {
            return nodeCache.FindAsync(
                sourceId,
                referenceTypeId,
                isInverse,
                includeSubtypes,
                browseName)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Follows the reference from the source and returns all target nodes.
        /// </summary>
        [Obsolete("Use FindAsync instead")]
        public static IList<INode> Find(
            this IUaNodeCache nodeCache,
            ExpandedNodeId sourceId,
            NodeId referenceTypeId,
            bool isInverse,
            bool includeSubtypes)
        {
            return nodeCache.FindAsync(
                sourceId,
                referenceTypeId,
                isInverse,
                includeSubtypes)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Returns the immediate subtypes for the type.
        /// </summary>
        [Obsolete("Use FindSubTypesAsync instead")]
        public static IList<NodeId> FindSubTypes(
            this IUaNodeCache nodeCache,
            ExpandedNodeId typeId)
        {
            return nodeCache.FindSubTypesAsync(typeId)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Determines whether a type is a subtype of another type.
        /// </summary>
        [Obsolete("Use IsTypeOfAsync instead")]
        public static bool IsTypeOf(
            this IUaNodeCache nodeCache,
            ExpandedNodeId subTypeId,
            ExpandedNodeId superTypeId)
        {
            return nodeCache.IsTypeOfAsync(subTypeId, superTypeId)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Determines whether a type is a subtype of another type.
        /// </summary>
        [Obsolete("Use IsTypeOfAsync instead")]
        public static bool IsTypeOf(
            this IUaNodeCache nodeCache,
            NodeId subTypeId,
            NodeId superTypeId)
        {
            return nodeCache.IsTypeOfAsync(subTypeId, superTypeId)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Returns the qualified name for the reference type id.
        /// </summary>
        [Obsolete("Use FindReferenceTypeNameAsync instead")]
        public static QualifiedName FindReferenceTypeName(
            this IUaNodeCache nodeCache,
            NodeId referenceTypeId)
        {
            return nodeCache.FindReferenceTypeNameAsync(referenceTypeId)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Returns the node identifier for the reference type with the
        /// specified browse name.
        /// </summary>
        [Obsolete("Use FindReferenceTypeAsync instead")]
        public static NodeId FindReferenceType(
            this IUaNodeCache nodeCache,
            QualifiedName browseName)
        {
            return nodeCache.FindReferenceTypeAsync(browseName)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Checks if the identifier <paramref name="encodingId"/> represents
        /// a that provides encodings for the <paramref name="datatypeId "/>.
        /// </summary>
        [Obsolete("Use IsEncodingForAsync instead")]
        public static bool IsEncodingOf(
            this IUaNodeCache nodeCache,
            ExpandedNodeId encodingId,
            ExpandedNodeId datatypeId)
        {
            return nodeCache.IsEncodingOfAsync(encodingId, datatypeId)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Determines if the value contained in an extension object
        /// <paramref name="value"/> matches the expected data type.
        /// </summary>
        [Obsolete("Use IsEncodingForAsync instead")]
        public static bool IsEncodingFor(
            this IUaNodeCache nodeCache,
            NodeId expectedTypeId,
            ExtensionObject value)
        {
            return nodeCache.IsEncodingForAsync(expectedTypeId, value)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Determines if the value is an encoding of the <paramref name="value"/>
        /// </summary>
        [Obsolete("Use IsEncodingForAsync instead")]
        public static bool IsEncodingFor(
            this IUaNodeCache nodeCache,
            NodeId expectedTypeId,
            object value)
        {
            return nodeCache.IsEncodingForAsync(expectedTypeId, value)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Returns the data type for the specified encoding.
        /// </summary>
        [Obsolete("Use FindDataTypeIdAsync instead")]
        public static NodeId FindDataTypeId(
            this IUaNodeCache nodeCache,
            ExpandedNodeId encodingId)
        {
            return nodeCache.FindDataTypeIdAsync(encodingId)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Returns the data type for the specified encoding.
        /// </summary>
        [Obsolete("Use FindDataTypeIdAsync instead")]
        public static NodeId FindDataTypeId(
            this IUaNodeCache nodeCache,
            NodeId encodingId)
        {
            return nodeCache.FindDataTypeIdAsync(encodingId)
                .AsTask()
                .GetAwaiter()
                .GetResult();
        }
    }
}
