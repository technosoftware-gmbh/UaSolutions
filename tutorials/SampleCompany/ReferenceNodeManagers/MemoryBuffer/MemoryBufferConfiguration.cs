#region Copyright (c) 2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com
//
// The Software is subject to the Technosoftware GmbH MIT License, which can
// be found here:
// https://technosoftware.com/license/mit/
//
// The Software is based on the OPC Foundation UA Stack and the OPC Foundation
// MIT License. The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using Opc.Ua;
#endregion Using Directives

namespace SampleCompany.NodeManagers.MemoryBuffer
{
    /// <summary>
    /// Stores the configuration the test node manager
    /// </summary>
    /// <remarks>
    /// ApplicationConfiguration.ParseExtension&lt;T&gt; constrains T to
    /// IEncodeable in 2.0, so the configuration is described with [DataType]
    /// and its encodeable implementation is generated rather than
    /// hand-written; the DataContractSerializer attributes it carried before
    /// are gone. Namespaces.MemoryBuffer is emitted by the same generator
    /// pass and so cannot be referenced here - the namespace URI is spelled
    /// out instead.
    /// </remarks>
    [DataType(Namespace = "http://samplecompany.com/SampleServer/NodeManagers/MemoryBuffer")]
    public partial class MemoryBufferConfiguration
    {
        /// <summary>
        /// The buffers exposed by the memory
        /// </summary>
        [DataTypeField(Order = 1, StructureHandling = StructureHandling.Inline)]
        public ArrayOf<MemoryBufferInstance> Buffers { get; set; }
    }

    /// <summary>
    /// Stores the configuration for a memory buffer instance.
    /// </summary>
    /// <remarks>
    /// See <see cref="MemoryBufferConfiguration"/> for why the namespace URI
    /// is spelled out rather than taken from Namespaces.MemoryBuffer.
    /// </remarks>
    [DataType(Namespace = "http://samplecompany.com/SampleServer/NodeManagers/MemoryBuffer")]
    public partial class MemoryBufferInstance
    {
        /// <summary>
        /// The browse name for the instance.
        /// </summary>
        [DataTypeField(Order = 1)]
        public string Name { get; set; }

        /// <summary>
        /// The number of tags in the buffer.
        /// </summary>
        [DataTypeField(Order = 2)]
        public int TagCount { get; set; }

        /// <summary>
        /// The data type of the tags in the buffer.
        /// </summary>
        [DataTypeField(Order = 3)]
        public string DataType { get; set; }
    }
}
