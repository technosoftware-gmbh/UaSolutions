#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
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
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaClient
{
    [JsonSerializable(typeof(BrowserOptions))]
    internal partial class BrowserOptionsContext : JsonSerializerContext;

    /// <summary>
    /// Stores the options to use for a browse operation. Can be serialized and
    /// deserialized.
    /// </summary>
    [DataContract(Namespace = Namespaces.OpcUaXsd)]
    public record class BrowserOptions
    {
        /// <summary>
        /// Request header to use for the browse operations.
        /// </summary>
        [DataMember(Order = 0)]
        public RequestHeader? RequestHeader { get; init; }

        /// <summary>
        /// The view to use for the browse operation.
        /// </summary>
        [DataMember(Order = 1)]
        public ViewDescription? View { get; init; }

        /// <summary>
        /// The maximum number of references to return in a single browse operation.
        /// </summary>
        [DataMember(Order = 2)]
        public uint MaxReferencesReturned { get; init; }

        /// <summary>
        /// The direction to browse.
        /// </summary>
        [DataMember(Order = 3)]
        public BrowseDirection BrowseDirection { get; init; } = BrowseDirection.Forward;

        /// <summary>
        /// The reference type to follow.
        /// </summary>
        [DataMember(Order = 4)]
        public NodeId ReferenceTypeId { get; init; } = NodeId.Null;

        /// <summary>
        /// Whether subtypes of the reference type should be included.
        /// </summary>
        [DataMember(Order = 5)]
        public bool IncludeSubtypes { get; init; } = true;

        /// <summary>
        /// The classes of the target nodes.
        /// </summary>
        [DataMember(Order = 6)]
        public int NodeClassMask { get; init; }

        /// <summary>
        /// The results to return.
        /// </summary>
        [DataMember(Order = 7)]
        public uint ResultMask { get; init; } = (uint)BrowseResultMask.All;

        /// <summary>
        /// gets or set the policy which is used to prevent the allocation
        /// of too many Continuation Points in the browse operation
        /// </summary>
        [DataMember(Order = 8)]
        public ContinuationPointPolicy ContinuationPointPolicy { get; init; }

        /// <summary>
        /// Max nodes to browse in a single operation.
        /// </summary>
        [DataMember(Order = 9)]
        public uint MaxNodesPerBrowse { get; set; }

        /// <summary>
        /// Max continuation points to use when ContinuationPointPolicy is set
        /// to Balanced.
        /// </summary>
        [DataMember(Order = 10)]
        public ushort MaxBrowseContinuationPoints { get; set; }
    }

    /// <summary>
    /// controls how the browser treats continuation points if the server has
    /// restrictions on their number.
    /// </summary>
    public enum ContinuationPointPolicy
    {
        /// <summary>
        /// Ignore how many Continuation Points are in use already.
        /// Rebrowse nodes for which BadNoContinuationPoint or
        /// BadInvalidContinuationPoint was raised. Can be used
        /// whenever the server has no restrictions no the maximum
        /// number of continuation points
        /// </summary>
        Default,

        /// <summary>
        /// Restrict the number of nodes which are browsed in a
        /// single service call to the maximum number of
        /// continuation points the server can allocae
        /// (if set to a value different from 0)
        /// </summary>
        Balanced
    }
}
