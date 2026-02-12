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
using Opc.Ua;
#endregion Using Directives

namespace SampleCompany.NodeManagers.Alarms
{
    /// <summary>
    /// Alarm check
    /// </summary>
    public class AlarmCheck
    {
        /// <summary>
        /// Alarm name
        /// </summary>
        public string AlarmName { get; set; }

        /// <summary>
        /// MethodName
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// ModellingRule
        /// </summary>
        public NodeId MethodDeclarationId { get; set; }

        /// <summary>
        /// ModellingRule Exists
        /// </summary>
        public bool Exists { get; set; }
    }
}
