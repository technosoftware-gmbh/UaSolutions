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
using Opc.Ua.Bindings;
#endregion Using Directives

namespace Technosoftware.UaServer.Tests
{
    /// <summary>
    /// Builds the transport binding registry the test fixtures listen with.
    /// </summary>
    /// <remarks>
    /// 1.5 loaded Opc.Ua.Bindings.Https by reflection the first time an
    /// https:// or opc.https:// endpoint was touched. 2.0 removed that
    /// auto-load: a server listens only on the schemes its registry was given,
    /// and the default one carries opc.tcp alone. Without this the https
    /// fixtures start a server that never binds a port and every test in them
    /// fails with a connection refused.
    /// </remarks>
    public static class TestTransportBindings
    {
        /// <summary>
        /// A registry with the default opc.tcp bindings plus the listener
        /// factories from Opc.Ua.Bindings.Https.
        /// </summary>
        public static DefaultTransportBindingRegistry WithAllSchemes()
        {
            DefaultTransportBindingRegistry registry = DefaultTransportBindingRegistry
                .WithDefaultTcp();
            registry.RegisterListenerFactory(new HttpsTransportListenerFactory());
            registry.RegisterListenerFactory(new OpcHttpsTransportListenerFactory());
            registry.RegisterListenerFactory(new WssTransportListenerFactory());
            registry.RegisterListenerFactory(new OpcWssTransportListenerFactory());
            registry.RegisterChannelFactory(new WssTransportChannelFactory());
            registry.RegisterChannelFactory(new OpcWssTransportChannelFactory());
            registry.RegisterChannelFactory(new WssJsonTransportChannelFactory());
            return registry;
        }
    }
}
