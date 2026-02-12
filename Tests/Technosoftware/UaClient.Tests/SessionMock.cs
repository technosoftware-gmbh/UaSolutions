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
using Moq;
using Opc.Ua;
using Technosoftware.UaConfiguration;
using Technosoftware.Tests;
#endregion Using Directives

namespace Technosoftware.UaClient.Tests
{
    /// <summary>
    /// Session with channel mock
    /// </summary>
    public sealed class SessionMock : Session
    {
        /// <summary>
        /// Get private field m_serverNonce from base class using reflection
        /// </summary>
        internal byte[] ServerNonce =>
            (byte[])typeof(Session)
                .GetField(
                    "m_serverNonce",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance)
                .GetValue(this);

        /// <summary>
        /// Create the mock
        /// </summary>
        internal SessionMock(
            Mock<ITransportChannel> channel,
            ApplicationConfiguration configuration,
            ConfiguredEndpoint endpoint)
            : base(
                  channel.Object,
                  configuration,
                  endpoint,
                  null)
        {
            Channel = channel;
        }

        /// <summary>
        /// Create default mock
        /// </summary>
        /// <returns></returns>
        public static SessionMock Create(EndpointDescription endpoint = null)
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();
            var channel = new Mock<ITransportChannel>();
            channel
                .SetupGet(s => s.MessageContext)
                .Returns(new ServiceMessageContext(telemetry));
            channel
                .SetupGet(s => s.SupportedFeatures)
                .Returns(TransportChannelFeatures.Reconnect);
            var configuration = new ApplicationConfiguration(telemetry)
            {
                ClientConfiguration = new ClientConfiguration() // TODO: Reasonable defaults!
            };

            endpoint ??= new EndpointDescription
            {
                SecurityMode = MessageSecurityMode.None,
                SecurityPolicyUri = SecurityPolicies.None,
                EndpointUrl = "opc.tcp://localhost:4840",
                UserIdentityTokens =
                [
                    new UserTokenPolicy()
                ]
            };

            // TODO: Allow mocking of application certificate loading
            var application = new ApplicationInstance(configuration, telemetry);
            if (endpoint.SecurityMode != MessageSecurityMode.None)
            {
                application.CheckApplicationInstanceCertificatesAsync(true).AsTask().GetAwaiter().GetResult();
            }

            return new SessionMock(channel, configuration,
                new ConfiguredEndpoint(null, endpoint ??
                    new EndpointDescription
                    {
                        SecurityMode = MessageSecurityMode.None,
                        SecurityPolicyUri = SecurityPolicies.None,
                        EndpointUrl = "opc.tcp://localhost:4840",
                        UserIdentityTokens =
                        [
                            new UserTokenPolicy()
                        ]
                    }));
        }

        internal void SetConnected()
        {
            SessionCreated(new NodeId("s=connected"), new NodeId("s=auth"));
            RenewUserIdentity += Sut_RenewUserIdentity;
        }

        private IUserIdentity Sut_RenewUserIdentity(IUaSession session, IUserIdentity identity)
        {
            return identity ?? new UserIdentity();
        }

        /// <summary>
        /// Channel mock
        /// </summary>
        public Mock<ITransportChannel> Channel { get; }
    }
}
