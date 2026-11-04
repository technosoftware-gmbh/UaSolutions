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
using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Opc.Ua;
using Opc.Ua.Security.Certificates;
#endregion Using Directives

namespace Technosoftware.UaServer.Tests
{
    /// <summary>
    /// Verifies the client nonce sent with CreateSession on a None channel reaches
    /// the session, so an enhanced user token signature over it verifies.
    /// </summary>
    [TestFixture]
    [Category("Server")]
    [SetCulture("en-us")]
    [SetUICulture("en-us")]
    [NonParallelizable]
    public sealed class CreateSessionNoneChannelNonceTests
    {
        private ServerFixture<UaStandardServer> m_fixture;
        private UaStandardServer m_server;

        [OneTimeSetUp]
        public async Task OneTimeSetUpAsync()
        {
            m_fixture = new ServerFixture<UaStandardServer>(telemetry => new UaStandardServer(telemetry))
            {
                SecurityNone = true
            };
            m_server = await m_fixture.StartAsync().ConfigureAwait(false);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDownAsync()
        {
            await m_fixture.StopAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// A client signs an X509 user token for an enhanced token policy on a None
        /// channel over ServerNonce | Hash(ServerCertificate) | ClientNonce, computed
        /// here by the stack the way an OPC Foundation 2.0.1 client computes it. Only
        /// a session with a client certificate tells this variant from the secured
        /// channel calculation, which also covers the hash of that certificate.
        /// </summary>
        [TestCase(false)]
        [TestCase(true)]
        public async Task EnhancedCertificateUserTokenOnNoneChannelVerifiesAsync(bool withClientCertificate)
        {
            SecurityPolicyInfo policy = GetEnhancedPolicy();
            SecureChannelContext channel = CreateNoneChannel("none-nonce");
            ByteString clientNonce = Nonce.CreateRandomNonceData(32).ToByteString();

            using Certificate clientCertificate = CertificateBuilder.Create("CN=None Channel Client")
                .SetRSAKeySize(2048)
                .CreateForRSA();

            CreateSessionResponse response = await CreateSessionAsync(
                channel,
                "none-nonce",
                clientNonce,
                withClientCertificate ? clientCertificate.RawData.ToByteString() : default)
                .ConfigureAwait(false);
            IUaSession session = FindSession(response.SessionId);
            Assert.That(session.ClientNonce, Is.EqualTo(clientNonce));
            Assert.That(session.ClientCertificate != null, Is.EqualTo(withClientCertificate));

            using CertificateCollection serverChain = Utils.ParseCertificateChainBlob(
                response.ServerCertificate,
                m_server.CurrentInstance.Telemetry);
            using Certificate userCertificate = CertificateBuilder.Create("CN=None Channel User")
                .SetRSAKeySize(2048)
                .CreateForRSA();
            byte[] dataToSign = policy.GetUserTokenSignatureData(
                null,
                response.ServerNonce.ToArray(),
                serverChain[0].RawData,
                null,
                session.ClientCertificate?.RawData,
                null,
                clientNonce.ToArray(),
                MessageSecurityMode.None);
            SignatureData userSignature = await SecurityPolicies.Default.CreateSignatureDataAsync(
                policy,
                userCertificate,
                dataToSign,
                CancellationToken.None).ConfigureAwait(false);
            var identity = new ExtensionObject(new X509IdentityToken
            {
                PolicyId = "certificate",
                CertificateData = userCertificate.RawData.ToByteString()
            });
            var context = new UaServerOperationContext(
                new RequestHeader(),
                channel,
                RequestType.ActivateSession);

            (IUserIdentityTokenHandler token, UserTokenPolicy selected) =
                await session.ValidateBeforeActivateAsync(
                    context,
                    new SignatureData(),
                    identity,
                    userSignature,
                    CancellationToken.None).ConfigureAwait(false);

            Assert.That(token.TokenType, Is.EqualTo(UserTokenType.Certificate));
            Assert.That(selected.PolicyId, Is.EqualTo("certificate"));
        }

        /// <summary>
        /// OPC 10000-4 5.7.2.3 rejects a client nonce longer than 128 bytes on
        /// every channel, including None.
        /// </summary>
        [Test]
        public void OversizedNonceOnNoneChannelIsRejected()
        {
            SecureChannelContext channel = CreateNoneChannel("none-oversized");
            ByteString clientNonce = new byte[129].ToByteString();

            ServiceResultException exception = Assert.ThrowsAsync<ServiceResultException>(
                async () => await CreateSessionAsync(channel, "none-oversized", clientNonce, default)
                    .ConfigureAwait(false));

            Assert.That(exception.StatusCode, Is.EqualTo(StatusCodes.BadNonceInvalid));
        }

        /// <summary>
        /// A short nonce stays accepted on a None channel, where nothing requires one.
        /// </summary>
        [Test]
        public async Task ShortNonceOnNoneChannelIsAcceptedAsync()
        {
            SecureChannelContext channel = CreateNoneChannel("none-short");
            ByteString clientNonce = new byte[] { 1, 2, 3, 4 }.ToByteString();

            CreateSessionResponse response = await CreateSessionAsync(channel, "none-short", clientNonce, default)
                .ConfigureAwait(false);

            Assert.That(FindSession(response.SessionId).ClientNonce, Is.EqualTo(clientNonce));
        }

        /// <summary>
        /// Keeping the nonce must not make a None channel sign the CreateSession
        /// response, which only a secured channel does.
        /// </summary>
        [Test]
        public async Task NoneChannelWithClientCertificateAndNonceIsNotSignedAsync()
        {
            SecureChannelContext channel = CreateNoneChannel("none-unsigned");
            ByteString clientNonce = Nonce.CreateRandomNonceData(32).ToByteString();
            using Certificate clientCertificate = CertificateBuilder.Create("CN=None Channel Client")
                .SetRSAKeySize(2048)
                .CreateForRSA();

            CreateSessionResponse response = await CreateSessionAsync(
                channel,
                "none-unsigned",
                clientNonce,
                clientCertificate.RawData.ToByteString()).ConfigureAwait(false);

            Assert.That(
                response.ServerSignature == null || response.ServerSignature.Signature.IsEmpty,
                Is.True);
        }

        private static SecurityPolicyInfo GetEnhancedPolicy()
        {
            SecurityPolicyInfo policy = SecurityPolicies.Default.GetInfo(SecurityPolicies.RSA_DH_AesGcm);
            if (policy == null)
            {
                Assert.Ignore("The RSA_DH_AesGcm security policy is not supported on this platform.");
            }
            return policy;
        }

        private SecureChannelContext CreateNoneChannel(string channelId)
        {
            foreach (EndpointDescription candidate in m_server.GetEndpoints())
            {
                if (candidate.SecurityPolicyUri != SecurityPolicies.None)
                {
                    continue;
                }

                var endpoint = (EndpointDescription)candidate.Clone();
                endpoint.UserIdentityTokens =
                [
                    new UserTokenPolicy
                    {
                        PolicyId = "certificate",
                        TokenType = UserTokenType.Certificate,
                        SecurityPolicyUri = SecurityPolicies.RSA_DH_AesGcm
                    }
                ];
                return new SecureChannelContext(channelId, endpoint, RequestEncoding.Binary, null, null);
            }

            throw new InvalidOperationException("The server has no None endpoint.");
        }

        private async Task<CreateSessionResponse> CreateSessionAsync(
            SecureChannelContext channel,
            string sessionName,
            ByteString clientNonce,
            ByteString clientCertificate)
        {
            return await m_server.CreateSessionAsync(
                channel,
                new RequestHeader(),
                null,
                null,
                null,
                sessionName,
                clientNonce,
                clientCertificate,
                60000,
                0,
                RequestLifetime.None).ConfigureAwait(false);
        }

        private IUaSession FindSession(NodeId sessionId)
        {
            foreach (IUaSession session in m_server.CurrentInstance.SessionManager.GetSessions())
            {
                if (session.Id == sessionId)
                {
                    return session;
                }
            }

            throw new InvalidOperationException("The session was not created.");
        }
    }
}
