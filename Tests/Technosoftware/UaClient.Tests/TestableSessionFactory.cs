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
using System.Security.Cryptography.X509Certificates;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaClient.Tests
{
    /// <summary>
    /// Object that creates instances of an Opc.Ua.Client.Session object.
    /// </summary>
    public class TestableSessionFactory : DefaultSessionFactory
    {
        /// <summary>
        /// Force use of the default instance.
        /// </summary>
        public TestableSessionFactory(ITelemetryContext telemetry)
            : base(telemetry)
        {
        }

        /// <inheritdoc/>
        public override IUaSession Create(
            ITransportChannel channel,
            ApplicationConfiguration configuration,
            ConfiguredEndpoint endpoint,
            X509Certificate2 clientCertificate,
            X509Certificate2Collection clientCertificateChain,
            EndpointDescriptionCollection availableEndpoints = null,
            StringCollection discoveryProfileUris = null)
        {
            return new TestableSession(
                channel,
                configuration,
                endpoint,
                clientCertificate,
                availableEndpoints,
                discoveryProfileUris);
        }
    }
}
