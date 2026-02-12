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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// The standard implementation of a UA server.
    /// </summary>
    public interface IUaStandardServer: ISessionServer
    {
        /// <summary>
        /// The async node manager factories that are used on startup of the server.
        /// </summary>
        IEnumerable<IUaAsyncNodeManagerFactory> AsyncNodeManagerFactories { get; }

        /// <summary>
        /// The state object associated with the server.
        /// It provides the shared components for the Server.
        /// </summary>
        /// <value>The current instance.</value>
        /// <exception cref="ServiceResultException"></exception>
        IUaServerData CurrentInstance { get; }

        /// <summary>
        /// The current state of the Server
        /// </summary>
        ServerState CurrentState { get; }
        /// <summary>
        /// The node manager factories that are used on startup of the server.
        /// </summary>
        IEnumerable<IUaNodeManagerFactory> NodeManagerFactories { get; }

        /// <summary>
        /// Add a node manager factory which is used on server start
        /// to instantiate the node manager in the server.
        /// </summary>
        /// <param name="nodeManagerFactory">The node manager factory used to create the NodeManager.</param>
        void AddNodeManager(IUaAsyncNodeManagerFactory nodeManagerFactory);

        /// <summary>
        /// Add a node manager factory which is used on server start
        /// to instantiate the node manager in the server.
        /// </summary>
        /// <param name="nodeManagerFactory">The node manager factory used to create the NodeManager.</param>

        void AddNodeManager(IUaNodeManagerFactory nodeManagerFactory);

        /// <summary>
        /// Registers the server with the discovery server.
        /// </summary>
        /// <returns>Boolean value.</returns>
        ValueTask<bool> RegisterWithDiscoveryServerAsync(CancellationToken ct = default);

        /// <summary>
        /// Remove a node manager factory from the list of node managers.
        /// Does not remove a NodeManager from a running server,
        /// only removes the factory before the server starts.
        /// </summary>
        /// <param name="nodeManagerFactory">The node manager factory to remove.</param>
        void RemoveNodeManager(IUaAsyncNodeManagerFactory nodeManagerFactory);

        /// <summary>
        /// Remove a node manager factory from the list of node managers.
        /// Does not remove a NodeManager from a running server,
        /// only removes the factory before the server starts.
        /// </summary>
        /// <param name="nodeManagerFactory">The node manager factory to remove.</param>
        void RemoveNodeManager(IUaNodeManagerFactory nodeManagerFactory);
    }
}
