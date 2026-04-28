#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System.Runtime.Serialization;
#endregion Using Directives

namespace SampleCompany.NodeManagers.Reference
{
    /// <summary>
    /// Stores the configuration the data access node manager.
    /// </summary>
    [DataContract(Namespace = Namespaces.ReferenceServer)]
    public class ReferenceServerConfiguration
    {
        /// <summary>
        /// Whether the user dialog for accepting invalid certificates should be displayed.
        /// </summary>
        [DataMember(Order = 1)]
        public bool ShowCertificateValidationDialog { get; set; }
    }
}
