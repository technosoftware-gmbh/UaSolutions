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

using System.Runtime.Serialization;

#endregion Using Directives

namespace Technosoftware.ClientGateway
{
    /// <summary>
    /// Stores the configuration for UA that wraps a COM server. 
    /// </summary>
    /// <exclude />
    [DataContract(Namespace = Common.Namespaces.ComInterop)]
    public class ComWrapperServerConfiguration
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// The default constructor.
        /// </summary>
        public ComWrapperServerConfiguration()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the object during deserialization.
        /// </summary>
        [OnDeserializing()]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        /// <summary>
        /// Sets private members to default values.
        /// </summary>
        private void Initialize()
        {
        }
        #endregion Constructors, Destructor, Initialization

        #region Public Properties
        /// <summary>
        /// The list of COM servers wrapped by the UA server. 
        /// </summary>
        [DataMember(Order = 1)]
        public ComClientConfigurationCollection WrappedServers { get; set; }

        /// <summary>
        /// The address to use for the classic wrapper, e.g. "opc.tcp://localhost:55300/TechnosoftwareClassicWrapper". 
        /// </summary>
        [DataMember(Order = 2)]
        public string ClassicWrapperAddress { get; set; }

        #endregion Public Properties
    }
}
