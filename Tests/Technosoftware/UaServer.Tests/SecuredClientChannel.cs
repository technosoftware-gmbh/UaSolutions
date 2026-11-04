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
using System.Threading.Tasks;
using Opc.Ua;
using Opc.Ua.Security.Certificates;
#endregion Using Directives

namespace Technosoftware.UaServer.Tests
{
    /// <summary>
    /// The client side of a channel for tests that call CreateSession and
    /// ActivateSession on the server directly.
    /// </summary>
    /// <remarks>
    /// Since 2.0 a CreateSession on a secured channel needs a client nonce
    /// (OPC 10000-4 5.7.2.3) and an ActivateSession needs a real client
    /// signature. This builds the client certificate, the channel context, the
    /// nonce and the signature the way <see cref="ServerFixtureUtils"/> does for
    /// <c>CreateAndActivateSessionAsync</c>, for tests that need the
    /// CreateSession and ActivateSession responses themselves. On a None
    /// channel no certificate, nonce or signature is used.
    /// </remarks>
    public sealed class SecuredClientChannel : IDisposable
    {
        #region Constructors, Destructor, Initialization
        private SecuredClientChannel(
            SessionServerBase server,
            SecureChannelContext context,
            Certificate clientCertificate)
        {
            m_server = server;
            Context = context;
            m_clientCertificate = clientCertificate;
        }
        #endregion Constructors, Destructor, Initialization

        #region Public Properties
        /// <summary>
        /// The channel context to pass to the services.
        /// </summary>
        public SecureChannelContext Context { get; }

        /// <summary>
        /// Whether the channel is secured.
        /// </summary>
        public bool IsSecured => m_clientCertificate != null;
        #endregion Public Properties

        #region Public Methods
        /// <summary>
        /// Creates the client side of a channel for an endpoint of the server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="endpoint">The endpoint, with the security mode and policy to use.</param>
        /// <param name="channelId">The secure channel id.</param>
        public static SecuredClientChannel Create(
            SessionServerBase server,
            EndpointDescription endpoint,
            string channelId)
        {
            if (endpoint.SecurityMode == MessageSecurityMode.None)
            {
                return new SecuredClientChannel(
                    server,
                    new SecureChannelContext(channelId, endpoint, RequestEncoding.Binary),
                    null);
            }

            Certificate clientCertificate = CertificateBuilder
                .Create("CN=SecuredClientChannel")
                .AddExtension(
                    new X509SubjectAltNameExtension(
                        kClientApplicationUri,
                        [Utils.GetHostName()]))
                .SetRSAKeySize(CertificateFactory.DefaultKeySize)
                .CreateForRSA();

            byte[] serverChannelCertificate = null;
            if (!endpoint.ServerCertificate.IsEmpty)
            {
                using CertificateCollection serverCertificateChain =
                    Utils.ParseCertificateChainBlob(
                        endpoint.ServerCertificate,
                        server.MessageContext.Telemetry);
                serverChannelCertificate = serverCertificateChain[0].RawData;
            }

            var context = new SecureChannelContext(
                channelId,
                endpoint,
                RequestEncoding.Binary,
                clientCertificate.RawData,
                serverChannelCertificate,
                Nonce.CreateRandomNonceData(32));

            return new SecuredClientChannel(server, context, clientCertificate);
        }

        /// <summary>
        /// Calls CreateSession on the channel, with a client nonce and the client
        /// certificate when the channel is secured.
        /// </summary>
        /// <param name="requestHeader">The request header.</param>
        /// <param name="sessionName">The session name.</param>
        /// <returns>The response and the client nonce that was sent.</returns>
        public async Task<(CreateSessionResponse Response, ByteString ClientNonce)> CreateSessionAsync(
            RequestHeader requestHeader,
            string sessionName)
        {
            ByteString clientNonce = IsSecured
                ? Nonce.CreateRandomNonceData(32).ToByteString()
                : default;
            ByteString clientCertificate = IsSecured
                ? m_clientCertificate.RawData.ToByteString()
                : default;

            CreateSessionResponse response = await m_server.CreateSessionAsync(
                Context,
                requestHeader,
                null,
                null,
                null,
                sessionName,
                clientNonce,
                clientCertificate,
                ServerFixtureUtils.DefaultSessionTimeout,
                ServerFixtureUtils.DefaultMaxResponseMessageSize,
                RequestLifetime.None).ConfigureAwait(false);

            return (response, clientNonce);
        }

        /// <summary>
        /// Creates the client signature for ActivateSession, or <c>null</c> on a
        /// None channel.
        /// </summary>
        /// <param name="created">The CreateSession response.</param>
        /// <param name="clientNonce">The client nonce sent with CreateSession.</param>
        public SignatureData CreateClientSignature(
            CreateSessionResponse created,
            ByteString clientNonce)
        {
            if (!IsSecured)
            {
                return null;
            }

            SecurityPolicyInfo securityPolicy = SecurityPolicies.Default.GetInfo(
                Context.EndpointDescription.SecurityPolicyUri);
            using CertificateCollection serverCertificateChain =
                Utils.ParseCertificateChainBlob(
                    created.ServerCertificate,
                    m_server.MessageContext.Telemetry);
            byte[] dataToSign = securityPolicy.GetClientSignatureData(
                Context.ChannelThumbprint.ToArrayOrNull(),
                created.ServerNonce.ToArray(),
                serverCertificateChain[0].RawData,
                Context.ServerChannelCertificate.ToArrayOrNull(),
                Context.ClientChannelCertificate.ToArrayOrNull(),
                clientNonce.ToArray());
            return SecurityPolicies.Default.CreateSignatureData(
                securityPolicy,
                m_clientCertificate,
                dataToSign);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            m_clientCertificate?.Dispose();
        }
        #endregion Public Methods

        #region Private Fields
        private const string kClientApplicationUri = "urn:localhost:opcfoundation.org:SecuredClientChannel";
        private readonly SessionServerBase m_server;
        private readonly Certificate m_clientCertificate;
        #endregion Private Fields
    }
}
