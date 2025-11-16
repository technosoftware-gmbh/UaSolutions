#region Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com
//
// The Software is subject to the Technosoftware GmbH Software License
// Agreement, which can be found here:
// https://technosoftware.com/documents/Source_License_Agreement.pdf
//
// The Software is based on the OPC Foundation MIT License.
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaClient
{
    /// <summary>
    /// A session configuration stores all the information
    /// needed to reconnect a session with a new secure channel.
    /// </summary>
    [DataContract(Namespace = Namespaces.OpcUaXsd)]
    [KnownType(typeof(UserIdentityToken))]
    [KnownType(typeof(AnonymousIdentityToken))]
    [KnownType(typeof(X509IdentityToken))]
    [KnownType(typeof(IssuedIdentityToken))]
    [KnownType(typeof(UserIdentity))]
    public class SessionConfiguration
    {
        /// <summary>
        /// Creates a session configuration
        /// </summary>
        public SessionConfiguration(
            IUaSession session,
            Nonce serverNonce,
            string userIdentityTokenPolicy,
            Nonce eccServerEphemeralKey,
            NodeId authenthicationToken)
        {
            Timestamp = DateTime.UtcNow;
            SessionName = session.SessionName;
            SessionId = session.SessionId;
            AuthenticationToken = authenthicationToken;
            Identity = session.Identity;
            ConfiguredEndpoint = session.ConfiguredEndpoint;
            CheckDomain = session.CheckDomain;
            ServerNonce = serverNonce;
            ServerEccEphemeralKey = eccServerEphemeralKey;
            UserIdentityTokenPolicy = userIdentityTokenPolicy;
        }

        /// <summary>
        /// Creates the session configuration from a stream.
        /// </summary>
        public static SessionConfiguration Create(Stream stream)
        {
            // secure settings
            XmlReaderSettings settings = Utils.DefaultXmlReaderSettings();
            using var reader = XmlReader.Create(stream, settings);
            var serializer = new DataContractSerializer(typeof(SessionConfiguration));
            return (SessionConfiguration)serializer.ReadObject(reader);
        }

        /// <summary>
        /// When the session configuration was created.
        /// </summary>
        [DataMember(IsRequired = true, Order = 10)]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The session name used by the client.
        /// </summary>
        [DataMember(IsRequired = true, Order = 20)]
        public string SessionName { get; set; }

        /// <summary>
        /// The session id assigned by the server.
        /// </summary>
        [DataMember(IsRequired = true, Order = 30)]
        public NodeId SessionId { get; set; }

        /// <summary>
        /// The authentication token used by the server to identify the session.
        /// </summary>
        [DataMember(IsRequired = true, Order = 40)]
        public NodeId AuthenticationToken { get; set; }

        /// <summary>
        /// The identity used to create the session.
        /// </summary>
        [DataMember(IsRequired = true, Order = 50)]
        public IUserIdentity Identity { get; set; }

        /// <summary>
        /// The configured endpoint for the secure channel.
        /// </summary>
        [DataMember(IsRequired = true, Order = 60)]
        public ConfiguredEndpoint ConfiguredEndpoint { get; set; }

        /// <summary>
        /// If the client is configured to check the certificate domain.
        /// </summary>
        [DataMember(IsRequired = false, Order = 70)]
        public bool CheckDomain { get; set; }

        /// <summary>
        /// The last server nonce received.
        /// </summary>
        [DataMember(IsRequired = true, Order = 80)]
        public Nonce ServerNonce { get; set; }

        /// <summary>
        /// The user identity token policy which was used to create the session.
        /// </summary>
        [DataMember(IsRequired = true, Order = 90)]
        public string UserIdentityTokenPolicy { get; set; }

        /// <summary>
        /// The last server ecc ephemeral key received.
        /// </summary>
        [DataMember(IsRequired = false, Order = 100)]
        public Nonce ServerEccEphemeralKey { get; set; }
    }
}
