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

namespace SampleCompany.NodeManagers.TestData
{
    /// <summary>
    /// Stores the configuration the test node manager
    /// </summary>
    /// <remarks>
    /// ApplicationConfiguration.ParseExtension&lt;T&gt; constrains T to
    /// IEncodeable, so the configuration is described with [DataType] and its
    /// encodeable implementation is generated rather than hand-written; the
    /// DataContractSerializer attributes it carried before are gone.
    /// Namespaces.TestData is emitted by the same generator pass and so cannot
    /// be referenced here - the namespace URI is spelled out instead.
    /// </remarks>
    [DataType(Namespace = "http://samplecompany.com/SampleServer/NodeManagers/TestData")]
    public partial class TestDataNodeManagerConfiguration
    {
        /// <summary>
        /// The path to the file that stores state of the node manager.
        /// </summary>
        [DataTypeField(Order = 1)]
        public string SaveFilePath { get; set; }

        /// <summary>
        /// The maximum length for a monitored item sampling queue.
        /// </summary>
        [DataTypeField(Order = 2)]
        public uint MaxQueueSize { get; set; } = 100;

        /// <summary>
        /// The next unused value that can be assigned to new nodes.
        /// </summary>
        [DataTypeField(Order = 3)]
        public uint NextUnusedId { get; set; }
    }
}
