#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
// Web: http://www.technosoftware.com
//
// The Software is based on the OPC Foundation’s software and is subject to 
// the OPC Foundation MIT License 1.00, which can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//
// The Software is subject to the Technosoftware GmbH Software License Agreement,
// which can be found here:
// https://technosoftware.com/license-agreement/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved


#region Using Directives

using Opc.Ua;

using Technosoftware.Common.Client;
using Technosoftware.UaServer;

#endregion Using Directives

namespace Technosoftware.ClientGateway.Ae
{
    /// <summary>
    /// A class that builds NodeIds used by the DataAccess NodeManager
    /// </summary>
    internal static class AeModelUtils
    {
        /// <summary>
        /// The RootType for a AE Simple Event Type node.
        /// </summary>
        public const int AeSimpleEventType = Technosoftware.Rcw.Constants.SIMPLE_EVENT;

        /// <summary>
        /// The RootType for a AE Tracking Event Type node.
        /// </summary>
        public const int AeTrackingEventType = Technosoftware.Rcw.Constants.TRACKING_EVENT;

        /// <summary>
        /// The RootType for a AE Condition Event Type node.
        /// </summary>
        public const int AeConditionEventType = Technosoftware.Rcw.Constants.CONDITION_EVENT;

        /// <summary>
        /// The RootType for a AE Area
        /// </summary>
        public const int AeArea = 5;

        /// <summary>
        /// The RootType for an AE Source
        /// </summary>
        public const int AeSource = 6;

        /// <summary>
        /// The RootType for an AE Condition
        /// </summary>
        public const int AeCondition = 7;

        /// <summary>
        /// The RootType for a node defined by the UA server.
        /// </summary>
        public const int InternalNode = 8;

        /// <summary>
        /// The RootType for an EventType defined by the AE server.
        /// </summary>
        public const int AeEventTypeMapping = 9;

        /// <summary>
        /// The RootType for a ConditionClass defined by the AE server.
        /// </summary>
        public const int AeConditionClassMapping = 10;

        /// <summary>
        /// Constructs a NodeId from the BrowseName of an internal node.
        /// </summary>
        /// <param name="browseName">The browse name.</param>
        /// <param name="namespaceIndex">Index of the namespace.</param>
        /// <returns>The node id.</returns>
        public static NodeId ConstructIdForInternalNode(QualifiedName browseName, ushort namespaceIndex)
        {
            ParsedNodeId parsedNodeId = new ParsedNodeId();

            parsedNodeId.RootId = browseName.Name;
            parsedNodeId.NamespaceIndex = namespaceIndex;
            parsedNodeId.RootType = InternalNode;

            return parsedNodeId.Construct();
        }

        /// <summary>
        /// Constructs the id for an area.
        /// </summary>
        /// <param name="areaId">The area id.</param>
        /// <param name="namespaceIndex">Index of the namespace.</param>
        /// <returns></returns>
        public static NodeId ConstructIdForArea(string areaId, ushort namespaceIndex)
        {
            ParsedNodeId parsedNodeId = new ParsedNodeId();

            parsedNodeId.RootId = areaId;
            parsedNodeId.NamespaceIndex = namespaceIndex;
            parsedNodeId.RootType = AeArea;

            return parsedNodeId.Construct();
        }

        /// <summary>
        /// Constructs the id for a source.
        /// </summary>
        /// <param name="areaId">The area id.</param>
        /// <param name="sourceName">Name of the source.</param>
        /// <param name="namespaceIndex">Index of the namespace.</param>
        /// <returns></returns>
        public static NodeId ConstructIdForSource(string areaId, string sourceName, ushort namespaceIndex)
        {
            ParsedNodeId parsedNodeId = new ParsedNodeId();

            parsedNodeId.RootType = AeSource;
            parsedNodeId.RootId = areaId;
            parsedNodeId.NamespaceIndex = namespaceIndex;
            parsedNodeId.ComponentPath = sourceName;

            return parsedNodeId.Construct();
        }
    }
}
