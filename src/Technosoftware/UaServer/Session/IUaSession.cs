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
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// A generic session manager object for a server.
    /// </summary>
    public interface IUaSession : IDisposable
    {
        /// <summary>
        /// Whether the session has been activated.
        /// </summary>
        bool Activated { get; }

        /// <summary>
        /// The application instance certificate associated with the client.
        /// </summary>
        X509Certificate2 ClientCertificate { get; }

        /// <summary>
        /// The last time the session was contacted by the client.
        /// </summary>
        DateTime ClientLastContactTime { get; }

        /// <summary>
        /// The client Nonce associated with the session.
        /// </summary>
        byte[] ClientNonce { get; }

        /// <summary>
        /// A lock which must be acquired before accessing the diagnostics.
        /// </summary>
        object DiagnosticsLock { get; }

        /// <summary>
        /// The application defined mapping for user identity provided by the client.
        /// </summary>
        IUserIdentity EffectiveIdentity { get; }

        /// <summary>
        /// Returns the session's endpoint
        /// </summary>
        EndpointDescription EndpointDescription { get; }

        /// <summary>
        /// Whether the session timeout has elapsed since the last communication from the client.
        /// </summary>
        bool HasExpired { get; }

        /// <summary>
        /// Gets the identifier assigned to the session when it was created.
        /// </summary>
        NodeId Id { get; }

        /// <summary>
        /// The user identity provided by the client.
        /// </summary>
        IUserIdentity Identity { get; }

        /// <summary>
        /// The user identity token provided by the client.
        /// </summary>
        UserIdentityToken IdentityToken { get; }

        /// <summary>
        /// The locales requested when the session was created.
        /// </summary>
        string[] PreferredLocales { get; }

        /// <summary>
        /// Returns the session's SecureChannelId
        /// </summary>
        string SecureChannelId { get; }

        /// <summary>
        /// The diagnostics associated with the session.
        /// </summary>
        SessionDiagnosticsDataType SessionDiagnostics { get; }

        /// <summary>
        /// Activates the session and binds it to the current secure channel.
        /// </summary>
        bool Activate(
            UaServerOperationContext context,
            UserIdentityToken identityToken,
            IUserIdentity identity,
            IUserIdentity effectiveIdentity,
            StringCollection localeIds,
            Nonce serverNonce);

        /// <summary>
        /// Closes a session and removes itself from the address space.
        /// </summary>
        void Close();

        /// <summary>
        /// Create new ECC ephemeral key
        /// </summary>
        /// <returns>A new ephemeral key</returns>
        EphemeralKeyType GetNewEccKey();

        /// <summary>
        /// Checks if the secure channel is currently valid.
        /// </summary>
        bool IsSecureChannelValid(string secureChannelId);

        /// <summary>
        /// Restores a continuation point for a session.
        /// </summary>
        /// <remarks>
        /// The caller is responsible for disposing the continuation point returned.
        /// </remarks>
        UaContinuationPoint RestoreContinuationPoint(byte[] continuationPoint);

        /// <summary>
        /// Restores a previously saves history continuation point.
        /// </summary>
        /// <param name="continuationPoint">The identifier for the continuation point.</param>
        /// <returns>The save continuation point. null if not found.</returns>
        object RestoreHistoryContinuationPoint(byte[] continuationPoint);

        /// <summary>
        /// Saves a continuation point for a session.
        /// </summary>
        /// <remarks>
        /// If the session has too many continuation points the oldest one is dropped.
        /// </remarks>
        void SaveContinuationPoint(UaContinuationPoint continuationPoint);

        /// <summary>
        /// Saves a continuation point used for historical reads.
        /// </summary>
        /// <param name="id">The identifier for the continuation point.</param>
        /// <param name="continuationPoint">The continuation point.</param>
        /// <remarks>
        /// If the continuationPoint implements IDisposable it will be disposed when
        /// the Session is closed or discarded.
        /// </remarks>
        void SaveHistoryContinuationPoint(Guid id, object continuationPoint);

        /// <summary>
        /// Set the ECC security policy URI
        /// </summary>
        void SetEccUserTokenSecurityPolicy(string securityPolicyUri);

        /// <summary>
        /// Updates the requested locale ids.
        /// </summary>
        /// <returns>true if the new locale ids are different from the old locale ids.</returns>
        bool UpdateLocaleIds(StringCollection localeIds);

        /// <summary>
        /// Activates the session and binds it to the current secure channel.
        /// </summary>
        void ValidateBeforeActivate(
            UaServerOperationContext context,
            SignatureData clientSignature,
            ExtensionObject userIdentityToken,
            SignatureData userTokenSignature,
            out UserIdentityToken identityToken,
            out UserTokenPolicy userTokenPolicy);

        /// <summary>
        /// Validate the diagnostic info.
        /// </summary>
        void ValidateDiagnosticInfo(RequestHeader requestHeader);

        /// <summary>
        /// Validates the request.
        /// </summary>
        void ValidateRequest(RequestHeader requestHeader, SecureChannelContext secureChannelContext, RequestType requestType);
    }
}
