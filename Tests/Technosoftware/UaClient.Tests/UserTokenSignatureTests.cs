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
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Opc.Ua;
using Opc.Ua.Security.Certificates;
using Technosoftware.Tests;
#endregion Using Directives

namespace Technosoftware.UaClient.Tests
{
    /// <summary>
    /// Verifies the data a client signs for an X509 user token matches what an
    /// OPC Foundation 2.0.1 server verifies.
    /// </summary>
    [TestFixture]
    [Category("Client")]
    [SetCulture("en-us")]
    [SetUICulture("en-us")]
    public sealed class UserTokenSignatureTests
    {
        /// <summary>
        /// On a None channel with an enhanced user token policy the signature covers
        /// ServerNonce | Hash(ServerCertificate) | ClientNonce, calculated with the
        /// security policy of the token rather than of the channel.
        /// </summary>
        [Test]
        public async Task EnhancedUserTokenOnNoneChannelSignsTheNoneVariantAsync()
        {
            SecurityPolicyInfo policy = SecurityPolicies.Default.GetInfo(SecurityPolicies.RSA_DH_AesGcm);
            if (policy == null)
            {
                Assert.Ignore("The RSA_DH_AesGcm security policy is not supported on this platform.");
            }

            ITelemetryContext telemetry = NUnitTelemetryContext.Create();
            using Certificate serverCertificate = CertificateBuilder.Create("CN=Signature Test Server")
                .AddExtension(new X509SubjectAltNameExtension("urn:localhost:SignatureTest", ["localhost"]))
                .SetRSAKeySize(2048)
                .CreateForRSA();
            using Certificate userCertificate = CertificateBuilder.Create("CN=Signature Test User")
                .SetRSAKeySize(2048)
                .CreateForRSA();
            using Certificate clientCertificate = CertificateBuilder.Create("CN=Signature Test Client")
                .SetRSAKeySize(2048)
                .CreateForRSA();
            using var certificateProvider = new InProcessCertificateProvider(userCertificate);
            var identity = new UserIdentity(
                new X509IdentityTokenHandler(
                    new CertificateIdentifier
                    {
                        Thumbprint = userCertificate.Thumbprint,
                        SubjectName = userCertificate.Subject
                    },
                    new CertificatePasswordProvider(),
                    certificateProvider));

            var endpoint = new EndpointDescription
            {
                SecurityMode = MessageSecurityMode.None,
                SecurityPolicyUri = SecurityPolicies.None,
                EndpointUrl = "opc.tcp://localhost:4840",
                ServerCertificate = serverCertificate.RawData.ToByteString(),
                Server = new ApplicationDescription { ApplicationUri = "urn:localhost:SignatureTest" },
                UserIdentityTokens =
                [
                    new UserTokenPolicy
                    {
                        PolicyId = "certificate",
                        TokenType = UserTokenType.Certificate,
                        SecurityPolicyUri = SecurityPolicies.RSA_DH_AesGcm
                    }
                ]
            };
            var certificateManager = new Mock<ICertificateManager>();
            certificateManager
                .Setup(m => m.ValidateAsync(
                    It.IsAny<CertificateCollection>(),
                    It.IsAny<TrustListIdentifier>(),
                    It.IsAny<Opc.Ua.Security.Certificates.CertificateValidationOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Opc.Ua.CertificateValidationResult.Success);
            var configuration = new ApplicationConfiguration(telemetry)
            {
                ApplicationUri = "urn:localhost:SignatureTestClient",
                SecurityConfiguration = new SecurityConfiguration { NonceLength = 32 },
                ClientConfiguration = new ClientConfiguration(),
                CertificateManager = certificateManager.Object
            };

            byte[] serverNonce = Nonce.CreateRandomNonceData(32);
            CreateSessionRequest createRequest = null;
            ActivateSessionRequest activateRequest = null;

            var channel = new Mock<ITransportChannel>();
            channel
                .SetupGet(s => s.MessageContext)
                .Returns(ServiceMessageContext.Create(telemetry));
            channel
                .SetupGet(s => s.SupportedFeatures)
                .Returns(TransportChannelFeatures.Reconnect);
            channel
                .Setup(c => c.SendRequestAsync(It.IsAny<CreateSessionRequest>(), It.IsAny<CancellationToken>()))
                .Returns((IServiceRequest request, CancellationToken _) =>
                {
                    createRequest = (CreateSessionRequest)request;
                    return new ValueTask<IServiceResponse>(new CreateSessionResponse
                    {
                        SessionId = new NodeId(1u),
                        AuthenticationToken = new NodeId(2u),
                        ServerNonce = serverNonce.ToByteString(),
                        ServerCertificate = serverCertificate.RawData.ToByteString(),
                        ServerEndpoints = [endpoint],
                        RevisedSessionTimeout = 60000
                    });
                });
            channel
                .Setup(c => c.SendRequestAsync(It.IsAny<ActivateSessionRequest>(), It.IsAny<CancellationToken>()))
                .Returns((IServiceRequest request, CancellationToken _) =>
                {
                    // the signature is all this test needs, so the open ends here.
                    activateRequest = (ActivateSessionRequest)request;
                    return ValueTask.FromException<IServiceResponse>(
                        new ServiceResultException(StatusCodes.BadUserAccessDenied));
                });
            channel
                .Setup(c => c.SendRequestAsync(It.IsAny<CloseSessionRequest>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IServiceResponse>(new CloseSessionResponse()));

            // a client certificate is what tells the None variant from the secured
            // channel calculation, which also covers the hash of that certificate.
            using var session = new TestableSession(
                channel.Object,
                configuration,
                new ConfiguredEndpoint(null, endpoint),
                clientCertificate);

            Assert.ThrowsAsync<ServiceResultException>(
                async () => await session.OpenAsync(
                    "signature-test",
                    60000,
                    identity,
                    default,
                    false,
                    CancellationToken.None).ConfigureAwait(false));

            Assert.That(activateRequest, Is.Not.Null);

            byte[] expected = policy.GetUserTokenSignatureData(
                null,
                serverNonce,
                serverCertificate.RawData,
                null,
                clientCertificate.RawData,
                null,
                createRequest.ClientNonce.ToArray(),
                MessageSecurityMode.None);

            bool verified = SecurityPolicies.Default.VerifySignatureData(
                activateRequest.UserTokenSignature,
                SecurityPolicies.RSA_DH_AesGcm,
                userCertificate,
                expected);

            Assert.That(verified, Is.True);
        }
    }
}
