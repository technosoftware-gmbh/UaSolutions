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
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Threading;
using System;
using NUnit.Framework;
using Opc.Ua;
using Opc.Ua.Security.Certificates;
using Technosoftware.UaServer;
#endregion Using Directives

namespace Technosoftware.UaServer.Tests
{
    /// <summary>
    /// Regression tests for the session AuthenticationToken and the user
    /// identity token decryption errors returned by ActivateSession.
    /// </summary>
    [TestFixture]
    [Category("Server")]
    [SetCulture("en-us")]
    [SetUICulture("en-us")]
    public class SessionSecurityRegressionTests
    {
        private ServerFixture<UaStandardServer> m_fixture;
        private string m_pkiRoot;

        [OneTimeSetUp]
        public async Task OneTimeSetUpAsync()
        {
            m_pkiRoot = Path.GetTempPath() + Path.GetRandomFileName() + Path.DirectorySeparatorChar;
            m_fixture = new ServerFixture<UaStandardServer>(telemetry => new UaStandardServer(telemetry));
            await m_fixture.LoadConfigurationAsync(m_pkiRoot).ConfigureAwait(false);
            m_fixture.Config.ServerConfiguration.UserTokenPolicies =
                m_fixture.Config.ServerConfiguration.UserTokenPolicies
                    .AddItem(new UserTokenPolicy(UserTokenType.UserName));
            await m_fixture.StartAsync(m_pkiRoot).ConfigureAwait(false);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDownAsync()
        {
            await m_fixture.StopAsync().ConfigureAwait(false);
            try
            {
                if (Directory.Exists(m_pkiRoot))
                {
                    Directory.Delete(m_pkiRoot, true);
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        /// <summary>
        /// Part 6 7.4.1: every HTTPS client of a listener shares one SecureChannelId,
        /// so the AuthenticationToken must be a random value of at least 32 bytes.
        /// A sequential token let a second client derive the token of the session
        /// created just before its own and use that session.
        /// </summary>
        [Test]
        public async Task SharedSecuredChannelGetsRandomTokenAsync()
        {
            // one channel id for both clients, as with the HTTPS ListenerId.
            using SecuredClientChannel sharedChannel = CreateSecuredChannel("https-listener");

            NodeId victim = (await CreateSessionAsync(sharedChannel, "Victim")
                .ConfigureAwait(false)).Response.AuthenticationToken;
            NodeId attacker = (await CreateSessionAsync(sharedChannel, "Attacker")
                .ConfigureAwait(false)).Response.AuthenticationToken;

            foreach (NodeId token in new[] { victim, attacker })
            {
                Assert.That(token.IdType, Is.EqualTo(IdType.Opaque),
                    "The token on a shared secured channel must be a random opaque value.");
                Assert.That(token.TryGetValue(out ByteString identifier), Is.True);
                Assert.That(identifier.Length, Is.GreaterThanOrEqualTo(32));
            }
            Assert.That(victim, Is.Not.EqualTo(attacker));
        }

        /// <summary>
        /// OPC 10000-4 7.40.2.1: a UserName token whose password fails RSA decryption
        /// returns Bad_IdentityTokenInvalid without an inner result, whether the
        /// padding is broken or conforming with an invalid length prefix. Otherwise
        /// the status (or the inner status a client requests with ReturnDiagnostics)
        /// is a padding oracle on the server key.
        /// </summary>
        [Test]
        public async Task PasswordDecryptionFailuresAreIndistinguishableAsync()
        {
            using SecuredClientChannel channel = CreateSecuredChannel("oracle-channel");
            (CreateSessionResponse created, ByteString clientNonce) = await CreateSessionAsync(channel, "Oracle")
                .ConfigureAwait(false);

            Certificate serverCertificate = Utils.ParseCertificateChainBlob(
                channel.Context.EndpointDescription.ServerCertificate,
                m_fixture.Server.MessageContext.Telemetry)[0];
            using RSA rsa = serverCertificate.GetRSAPublicKey();

            // conforming OAEP padding, length prefix larger than the block.
            byte[] block = new byte[16];
            BitConverter.GetBytes(0x7FFFFFFF).CopyTo(block, 0);
            byte[] badLength = rsa.Encrypt(block, RSAEncryptionPadding.OaepSHA1);

            // non conforming padding.
            byte[] badPadding = new byte[badLength.Length];
            badPadding[0] = 0x01;
            badPadding[^1] = 0x01;

            ServiceResultException lengthError = await ActivateWithPasswordAsync(
                channel, created, clientNonce, badLength).ConfigureAwait(false);
            ServiceResultException paddingError = await ActivateWithPasswordAsync(
                channel, created, clientNonce, badPadding).ConfigureAwait(false);

            foreach (ServiceResultException error in new[] { lengthError, paddingError })
            {
                Assert.That(error.StatusCode, Is.EqualTo(StatusCodes.BadIdentityTokenInvalid));
                Assert.That(
                    error.InnerResult == null || StatusCode.IsGood(error.InnerResult.StatusCode),
                    Is.True,
                    "The cause of a token decryption failure must not reach the client.");
            }
            Assert.That(lengthError.Message, Is.EqualTo(paddingError.Message));
        }

        private SecuredClientChannel CreateSecuredChannel(string channelId)
        {
            EndpointDescription endpoint = m_fixture.Server.GetEndpoints().ToArray().First(e =>
                e.SecurityMode == MessageSecurityMode.SignAndEncrypt &&
                e.SecurityPolicyUri == SecurityPolicies.Basic256Sha256);
            // 2.0 needs a client nonce and certificate for CreateSession on a
            // secured channel and a client signature for ActivateSession.
            return SecuredClientChannel.Create(m_fixture.Server, endpoint, channelId);
        }

        private static async Task<(CreateSessionResponse Response, ByteString ClientNonce)> CreateSessionAsync(
            SecuredClientChannel channel,
            string sessionName)
        {
            (CreateSessionResponse response, ByteString clientNonce) = await channel
                .CreateSessionAsync(new RequestHeader(), sessionName)
                .ConfigureAwait(false);
            ServerFixtureUtils.ValidateResponse(response.ResponseHeader);
            return (response, clientNonce);
        }

        private async Task<ServiceResultException> ActivateWithPasswordAsync(
            SecuredClientChannel channel,
            CreateSessionResponse created,
            ByteString clientNonce,
            byte[] password)
        {
            UserTokenPolicy policy = channel.Context.EndpointDescription.UserIdentityTokens
                .ToArray().First(p => p.TokenType == UserTokenType.UserName);
            var token = new UserNameIdentityToken
            {
                PolicyId = policy.PolicyId,
                UserName = "user1",
                Password = ByteString.From(password),
                EncryptionAlgorithm = SecurityAlgorithms.RsaOaep
            };
            var requestHeader = new RequestHeader
            {
                AuthenticationToken = created.AuthenticationToken,
                ReturnDiagnostics = (uint)DiagnosticsMasks.All
            };
            try
            {
                await m_fixture.Server.ActivateSessionAsync(
                    channel.Context,
                    requestHeader,
                    channel.CreateClientSignature(created, clientNonce),
                    [],
                    [],
                    new ExtensionObject(token),
                    null,
                    RequestLifetime.None).ConfigureAwait(false);
            }
            catch (ServiceResultException sre)
            {
                return sre;
            }
            Assert.Fail("ActivateSession with an undecryptable password must fail.");
            return null;
        }
    }
}
