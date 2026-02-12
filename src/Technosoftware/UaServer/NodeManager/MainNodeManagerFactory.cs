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
using Technosoftware.UaConfiguration;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// The factory that creates the main node managers of the server. The main
    /// node managers are the one always present when creating a server.
    /// </summary>
    public class MainNodeManagerFactory : IUaMainNodeManagerFactory
    {
        /// <summary>
        /// Initializes the object with default values.
        /// </summary>
        public MainNodeManagerFactory(
            ApplicationConfiguration applicationConfiguration,
            IUaServerData server)
        {
            m_applicationConfiguration = applicationConfiguration;
            m_server = server;
        }

        /// <inheritdoc/>
        public IUaConfigurationNodeManager CreateConfigurationNodeManager()
        {
            return new ConfigurationNodeManager(m_server, m_applicationConfiguration);
        }

        /// <inheritdoc/>
        public IUaCoreNodeManager CreateCoreNodeManager(ushort dynamicNamespaceIndex)
        {
            return new CoreNodeManager(m_server, m_applicationConfiguration, dynamicNamespaceIndex);
        }

        private readonly ApplicationConfiguration m_applicationConfiguration;
        private readonly IUaServerData m_server;
    }
}
