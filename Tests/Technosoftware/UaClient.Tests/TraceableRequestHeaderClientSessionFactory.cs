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
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Opc.Ua;
using Opc.Ua.Security.Certificates;
#endregion Using Directives

namespace Technosoftware.UaClient
{
    /// <summary>
    /// Object that creates an instance of a Session object.
    /// It can be used to create instances of enhanced Session
    /// classes with added functionality or overridden methods.
    /// </summary>
    public class TraceableRequestHeaderClientSessionFactory : DefaultSessionFactory
    {
        public TraceableRequestHeaderClientSessionFactory(ITelemetryContext telemetry)
            : base(telemetry)
        {
            ReturnDiagnostics = DiagnosticsMasks.SymbolicIdAndText;
        }

        /// <inheritdoc/>
        public override IUaSession Create(
            ITransportChannel channel,
            ApplicationConfiguration configuration,
            ConfiguredEndpoint endpoint,
            Certificate clientCertificate = null,
            CertificateCollection clientCertificateChain = null,
            ArrayOf<EndpointDescription> availableEndpoints = default,
            List<string> discoveryProfileUris = null)
        {
            return new TraceableRequestHeaderClientSession(
                channel,
                configuration,
                endpoint,
                clientCertificate,
                availableEndpoints,
                discoveryProfileUris);
        }
    }
}
