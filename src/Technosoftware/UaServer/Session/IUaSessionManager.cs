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
using System.Threading;
using System.Threading.Tasks;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Allows application components to receive notifications when changes to sessions occur.
    /// </summary>
    /// <remarks>
    /// Sinks that receive these events must not block the thread.
    /// </remarks>
    public interface IUaSessionManager : IDisposable
    {
        #region Event Definitions
        /// <summary>
        /// Raised after a new session is created.
        /// </summary>
        event  EventHandler<SessionEventArgs> SessionCreated;

        /// <summary>
        /// Raised whenever a session is activated and the user identity or preferred locales changed.
        /// </summary>
        event  EventHandler<SessionEventArgs> SessionActivated;

        /// <summary>
        /// Raised before a session is closed.
        /// </summary>
        event  EventHandler<SessionEventArgs> SessionClosing;

        /// <summary>
        /// Raised to signal a channel that the session is still alive.
        /// </summary>
        event  EventHandler<SessionEventArgs> SessionChannelKeepAlive;

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
        ValueTask StartupAsync(CancellationToken cancellationToken = default);

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
        ValueTask<CreateSessionResult> CreateSessionAsync(
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
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Activates an existing session
        /// </summary>
        ValueTask<(bool IdentityContextChanged, byte[] ServerNonce)> ActivateSessionAsync(
            UaServerOperationContext context,
            NodeId authenticationToken,
            SignatureData clientSignature,
            List<SoftwareCertificate> clientSoftwareCertificates,
            ExtensionObject userIdentityToken,
            SignatureData userTokenSignature,
            StringCollection localeIds,
            CancellationToken cancellationToken = default);

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
        UaServerOperationContext ValidateRequest(RequestHeader requestHeader, SecureChannelContext secureChannelContext, RequestType requestType);

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

    /// <summary>
    /// The result of a call to <see cref="IUaSessionManager.CreateSessionAsync"/>.
    /// </summary>
    public class CreateSessionResult
    {
        /// <summary>
        /// The created Session.
        /// </summary>
        public required IUaSession Session { get; init; }

        /// <summary>
        /// The SessionID assigned to the session.
        /// </summary>
        public required NodeId SessionId { get; init; }

        /// <summary>
        /// The authentication token used to identify and authorize the client.
        /// </summary>
        public required NodeId AuthenticationToken { get; init; }

        /// <summary>
        /// The server nonce of the session.
        /// </summary>
        public required byte[] ServerNonce { get; init; }

        /// <summary>
        /// The revised session timeout.
        /// </summary>
        public required double RevisedSessionTimeout { get; init; }
    }

    /// <summary>
    /// The possible reasons for a session related event.
    /// </summary>
    public enum SessionEventReason
    {
        /// <summary>
        /// A new session was created.
        /// </summary>
        Created,

        /// <summary>
        /// A session is being activated with a new user identity.
        /// </summary>
        Impersonating,

        /// <summary>
        /// A session was activated and the user identity or preferred locales changed.
        /// </summary>
        Activated,

        /// <summary>
        /// A session is about to be closed.
        /// </summary>
        Closing,

        /// <summary>
        /// A keep alive to signal a channel that the session is still active.
        /// Triggered by the session manager based on <see cref="ServerConfiguration.MinSessionTimeout"/>.
        /// </summary>
        ChannelKeepAlive
    }

    /// <summary>
    /// A class which provides the event arguments for session related event.
    /// </summary>
    public class ImpersonateUserEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ImpersonateUserEventArgs(
            UserIdentityToken newIdentity,
            UserTokenPolicy userTokenPolicy,
            EndpointDescription endpointDescription = null)
        {
            NewIdentity = newIdentity;
            UserTokenPolicy = userTokenPolicy;
            EndpointDescription = endpointDescription;
        }

        /// <summary>
        /// The new user identity for the session.
        /// </summary>
        public UserIdentityToken NewIdentity { get; }

        /// <summary>
        /// The user token policy selected by the client.
        /// </summary>
        public UserTokenPolicy UserTokenPolicy { get; }

        /// <summary>
        /// An application defined handle that can be used for access control operations.
        /// </summary>
        public IUserIdentity Identity { get; set; }

        /// <summary>
        /// An application defined handle that can be used for access control operations.
        /// </summary>
        public IUserIdentity EffectiveIdentity { get; set; }

        /// <summary>
        /// Set to indicate that an error occurred validating the identity and that it should be rejected.
        /// </summary>
        public ServiceResult IdentityValidationError { get; set; }

        /// <summary>
        /// Get the EndpointDescription
        /// </summary>
        public EndpointDescription EndpointDescription { get; }
    }

    /// <summary>
    /// The delegate for functions used to receive impersonation events.
    /// </summary>
    public delegate void ImpersonateEventHandler(IUaSession session, ImpersonateUserEventArgs args);

    /// <summary>
    /// A class which provides the event arguments for session related event.
    /// </summary>
    public class ValidateSessionLessRequestEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ValidateSessionLessRequestEventArgs(
            NodeId authenticationToken,
            RequestType requestType)
        {
            AuthenticationToken = authenticationToken;
            RequestType = requestType;
        }

        /// <summary>
        /// The request type for the request.
        /// </summary>
        public RequestType RequestType { get; }

        /// <summary>
        /// The new user identity for the session.
        /// </summary>
        public NodeId AuthenticationToken { get; }

        /// <summary>
        /// The identity to associate with the session-less request.
        /// </summary>
        public IUserIdentity Identity { get; set; }

        /// <summary>
        /// Set to indicate that an error occurred validating the session-less request and that it should be rejected.
        /// </summary>
        public ServiceResult Error { get; set; }
    }
}
