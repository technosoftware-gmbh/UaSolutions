#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Interface of the main node manager factory which helps creating main
    /// node managers used by the server.
    /// </summary>
    public interface IUaMainNodeManagerFactory
    {
        /// <summary>
        /// Creates the configuration node manager.
        /// </summary>
        /// <returns>The configuration node manager.</returns>
        IUaConfigurationNodeManager CreateConfigurationNodeManager();

        /// <summary>
        /// Creates the core node manager.
        /// </summary>
        /// <param name="dynamicNamespaceIndex">The namespace index of the dynamic namespace.</param>
        /// <returns>The core node manager</returns>
        IUaCoreNodeManager CreateCoreNodeManager(ushort dynamicNamespaceIndex);
    }
}
