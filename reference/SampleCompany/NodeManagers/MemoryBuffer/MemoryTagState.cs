#region Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using Opc.Ua;
#endregion Using Directives

namespace SampleCompany.NodeManagers.MemoryBuffer
{
    public partial class MemoryTagState
    {
        /// <summary>
        /// Initializes a memory tag for a buffer.
        /// </summary>
        /// <param name="parent">The buffer that owns the tag.</param>
        /// <param name="offet">The offset of the tag address in the memory buffer.</param>
        public MemoryTagState(MemoryBufferState parent, uint offet)
            : base(parent)
        {
            // these objects are created an discarded during each operation.
            // the metadata is derived from the parameters passed to constructors.
            NodeId = new NodeId(
                Utils.Format("{0}[{1}]", parent.SymbolicName, offet),
                parent.NodeId.NamespaceIndex);
            BrowseName = new QualifiedName(
                Utils.Format("{1:X8}", parent.SymbolicName, offet),
                parent.TypeDefinitionId.NamespaceIndex);
            DisplayName = BrowseName.Name;
            Description = null;
            WriteMask = AttributeWriteMask.None;
            UserWriteMask = AttributeWriteMask.None;
            ReferenceTypeId = ReferenceTypeIds.HasComponent;
            TypeDefinitionId = new NodeId(
                VariableTypes.MemoryTagType,
                parent.TypeDefinitionId.NamespaceIndex);
            ModellingRuleId = null;
            NumericId = offet;
            DataType = new NodeId((uint)parent.ElementType);
            ValueRank = ValueRanks.Scalar;
            ArrayDimensions = null;
            AccessLevel = AccessLevels.CurrentReadOrWrite;
            UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            MinimumSamplingInterval = parent.MaximumScanRate;
            Historizing = false;

            // re-direct read and write operations to the parent.
            OnReadValue = parent.ReadTagValue;
            OnWriteValue = parent.WriteTagValue;

            Offset = offet;
        }

        /// <summary>
        /// The offset of the tag address in the memory buffer.
        /// </summary>
        public uint Offset { get; }
    }
}
