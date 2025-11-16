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
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    ///     Allows application components to receive notifications when changes to sessions occur.
    /// </summary>
    /// <remarks>
    ///     Sinks that receive these events must not block the thread.
    /// </remarks>
    public interface IUaSessionManager : IDisposable
    {
        #region Event Definitions
        /// <summary>
        /// Raised after a new session is created.
        /// </summary>
        event EventHandler<SessionEventArgs> SessionCreated;

        /// <summary>
        /// Raised whenever a session is activated and the user identity or preferred locales changed.
        /// </summary>
        event EventHandler<SessionEventArgs> SessionActivated;

        /// <summary>
        /// Raised before a session is closed.
        /// </summary>
        event EventHandler<SessionEventArgs> SessionClosing;

        /// <summary>
        /// Raised to signal a channel that the session is still alive.
        /// </summary>
        event EventHandler<SessionEventArgs> SessionChannelKeepAlive;

        /// <summary>
        /// Raised before the user identity for a session is changed.
        /// </summary>
        event EventHandler<ImpersonateUserEventArgs> ImpersonateUser;

        /// <summary>
        /// Raised to validate a session-less request.
        /// </summary>
        event EventHandler<ValidateSessionLessRequestEventArgs> ValidateSessionLessRequest;
        #endregion

        /// <summary>
        /// Starts the session manager.
        /// </summary>
        void Startup();

        /// <summary>
        /// Stops the session manager and closes all sessions.
        /// </summary>
        void Shutdown();

        /// <summary>
        /// Returns all of the sessions known to the session manager.
        /// </summary>
        /// <returns>A list of the sessions.</returns>
        IList<IUaSession> GetSessions();

        /// <summary>
        /// Find and return a session specified by authentication token
        /// </summary>
        /// <returns>The requested session.</returns>
        IUaSession GetSession(NodeId authenticationToken);

        /// <summary>
        /// Creates a new session.
        /// </summary>
        IUaSession CreateSession(
            UaServerOperationContext context,
            X509Certificate2 serverCertificate,
            string sessionName,
            byte[] clientNonce,
            ApplicationDescription clientDescription,
            string endpointUrl,
            X509Certificate2 clientCertificate,
            X509Certificate2Collection clientCertificateChain,
            double requestedSessionTimeout,
            uint maxResponseMessageSize,
            out NodeId sessionId,
            out NodeId authenticationToken,
            out byte[] serverNonce,
            out double revisedSessionTimeout);

        /// <summary>
        /// Activates an existing session
        /// </summary>
        bool ActivateSession(
            UaServerOperationContext context,
            NodeId authenticationToken,
            SignatureData clientSignature,
            List<SoftwareCertificate> clientSoftwareCertificates,
            ExtensionObject userIdentityToken,
            SignatureData userTokenSignature,
            StringCollection localeIds,
            out byte[] serverNonce);

        /// <summary>
        /// Closes the specified session.
        /// </summary>
        /// <remarks>
        /// This method should not throw an exception if the session no longer exists.
        /// </remarks>
        void CloseSession(NodeId sessionId);

        /// <summary>
        /// Validates request header and returns a request context.
        /// </summary>
        /// <remarks>
        /// This method verifies that the session id is valid and that it uses secure channel id
        /// associated with current thread. It also verifies that the timestamp is not too
        /// and that the sequence number is not out of order (update requests only).
        /// </remarks>
        UaServerOperationContext ValidateRequest(RequestHeader requestHeader, RequestType requestType);

        #region Obsolete Event Definitions
        /// <summary>
        /// Raised after a new session is created.
        /// </summary>
        [Obsolete("Use SessionCreated")]
        event EventHandler<SessionEventArgs> SessionCreatedEvent;

        /// <summary>
        /// Raised whenever a session is activated and the user identity or preferred locales changed.
        /// </summary>
        [Obsolete("Use SessionActivated")]
        event EventHandler<SessionEventArgs> SessionActivatedEvent;

        /// <summary>
        /// Raised before a session is closed.
        /// </summary>
        [Obsolete("Use SessionClosing")]
        event EventHandler<SessionEventArgs> SessionClosingEvent;

        /// <summary>
        /// Raised to signal a channel that the session is still alive.
        /// </summary>
        [Obsolete("Use SessionClosing")]
        event EventHandler<SessionEventArgs> SessionChannelKeepAliveEvent;

        /// <summary>
        /// Raised before the user identity for a session is changed.
        /// </summary>
        [Obsolete("Use ImpersonateUser")]
        event EventHandler<UaImpersonateUserEventArgs> ImpersonateUserEvent;

        /// <summary>
        /// Raised to validate a session-less request.
        /// </summary>
        [Obsolete("Use ValidateSessionLessRequest")]
        event EventHandler<ValidateSessionLessRequestEventArgs> ValidateSessionLessRequestEvent;
        #endregion Obsolete Event Definitions
    }
}
