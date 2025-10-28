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
using System.Threading;

using Opc.Ua;
#endregion

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Stores information used while a thread is completing an operation on behalf of a client.
    /// </summary>
    public class UaServerOperationContext : IOperationContext
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Initializes the context with a session.
        /// </summary>
        /// <param name="requestHeader">The request header.</param>
        /// <param name="requestType">ProductLicenseType of the request.</param>
        /// <param name="identity">The user identity used in the request.</param>
        public UaServerOperationContext(RequestHeader requestHeader, Sessions.RequestType requestType, IUserIdentity identity = null)
        {
            if (requestHeader == null) throw new ArgumentNullException(nameof(requestHeader));
            
            m_channelContext    = SecureChannelContext.Current;
            m_session           = null;
            m_identity          = identity;
            m_preferredLocales  = Array.Empty<string>();
            m_diagnosticsMask   = (DiagnosticsMasks)requestHeader.ReturnDiagnostics;
            m_stringTable       = new StringTable();
            m_auditLogEntryId   = requestHeader.AuditEntryId;
            m_requestId         = Utils.IncrementIdentifier(ref s_lastRequestId);
            m_requestType       = requestType;
            m_clientHandle      = requestHeader.RequestHandle;
            m_operationDeadline = DateTime.MaxValue;

            if (requestHeader.TimeoutHint > 0)
            {
                m_operationDeadline = DateTime.UtcNow.AddMilliseconds(requestHeader.TimeoutHint);
            }
        }

        /// <summary>
        /// Initializes the context with a session.
        /// </summary>
        /// <param name="requestHeader">The request header.</param>
        /// <param name="requestType">ProductLicenseType of the request.</param>
        /// <param name="session">The session.</param>
        /// <exception cref="ArgumentNullException">In case requestHeader or session is null.</exception>
        /// <exception cref="ArgumentNullException">In case session is null.</exception>
        public UaServerOperationContext(RequestHeader requestHeader, Sessions.RequestType requestType, Sessions.Session session)
        {
            if (requestHeader == null) throw new ArgumentNullException(nameof(requestHeader));
            if (session == null) throw new ArgumentNullException(nameof(session));
            
            m_channelContext     = SecureChannelContext.Current;
            m_session            = session;
            m_identity           = session.EffectiveIdentity;
            m_preferredLocales   = session.PreferredLocales;
            m_diagnosticsMask    = (DiagnosticsMasks)requestHeader.ReturnDiagnostics;
            m_stringTable        = new StringTable();
            m_auditLogEntryId    = requestHeader.AuditEntryId;
            m_requestId          = Utils.IncrementIdentifier(ref s_lastRequestId);
            m_requestType        = requestType;
            m_clientHandle       = requestHeader.RequestHandle;
            m_operationDeadline  = DateTime.MaxValue;

            if (requestHeader.TimeoutHint > 0)
            {
                m_operationDeadline = DateTime.UtcNow.AddMilliseconds(requestHeader.TimeoutHint);
            }
        }

        /// <summary>
        /// Initializes the context with a session.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="diagnosticsMasks">The diagnostics masks.</param>
        /// <exception cref="ArgumentNullException">In case session is null.</exception>
        public UaServerOperationContext(Sessions.Session session, DiagnosticsMasks diagnosticsMasks)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            
            m_channelContext    = null;
            m_session           = session;
            m_identity          = session.EffectiveIdentity;
            m_preferredLocales  = session.PreferredLocales;
            m_diagnosticsMask   = diagnosticsMasks;
            m_stringTable       = new StringTable();
            m_auditLogEntryId   = null;
            m_requestId         = 0;
            m_requestType       = Sessions.RequestType.Unknown;
            m_clientHandle      = 0;
            m_operationDeadline = DateTime.MaxValue;
        }

        /// <summary>
        /// Initializes the context with a monitored item.
        /// </summary>
        /// <param name="monitoredItem">The monitored item.</param>
        /// <exception cref="ArgumentNullException">In case monitoredItem is null.</exception>
        public UaServerOperationContext(IUaMonitoredItem monitoredItem)
        {
            if (monitoredItem == null) throw new ArgumentNullException(nameof(monitoredItem));
            
            m_channelContext = null;
            m_identity = monitoredItem.EffectiveIdentity;
            m_session = monitoredItem.Session;

            if (m_session != null)
            {
                m_identity = m_session.Identity;
                m_preferredLocales  = m_session.PreferredLocales;
            }                
                
            m_diagnosticsMask   = DiagnosticsMasks.SymbolicId;
            m_stringTable       = new StringTable();
            m_auditLogEntryId   = null;
            m_requestId         = 0;
            m_requestType       = Sessions.RequestType.Unknown;
            m_clientHandle      = 0;
            m_operationDeadline = DateTime.MaxValue;
            }
        #endregion   
                
        #region Public Properties
        /// <summary>
        /// The context for the secure channel used to send the request.
        /// </summary>
        /// <value>The channel context.</value>
        public SecureChannelContext ChannelContext
        {
            get { return m_channelContext; }
        }

        /// <summary>
        /// The session associated with the context.
        /// </summary>
        /// <value>The session.</value>
        public Sessions.Session Session
        {
            get { return m_session; }
        }

        /// <summary>
        /// The security policy used for the secure channel.
        /// </summary>
        /// <value>The security policy URI.</value>
        public string SecurityPolicyUri
        {
            get 
            { 
                if (m_channelContext != null && m_channelContext.EndpointDescription != null)
                {
                    return m_channelContext.EndpointDescription.SecurityPolicyUri;
                }

                return null;
            }
        }
        
        /// <summary>
        /// The type of request.
        /// </summary>
        /// <value>The type of the request.</value>
        public Sessions.RequestType RequestType
        {
            get { return m_requestType; }
        }

        /// <summary>
        /// A unique identifier assigned to the request by the server.
        /// </summary>
        /// <value>The request id.</value>
        public uint RequestId
        {
            get { return m_requestId; }
        }

        /// <summary>
        /// The handle assigned by the client to the request.
        /// </summary>
        /// <value>The client handle.</value>
        public uint ClientHandle
        {
            get { return m_clientHandle; }
        }

        /// <summary>
        /// Updates the status code (thread safe).
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public void SetStatusCode(StatusCode statusCode)
        {
            Interlocked.Exchange(ref m_operationStatus, (long)statusCode.Code);
        }
        #endregion

        #region IOperationContext Members
        /// <summary>
        /// The identifier for the session (null if multiple sessions are associated with the operation).
        /// </summary>
        /// <value>The session id.</value>
        public NodeId SessionId
        {
            get 
            { 
                if (m_session != null)
                {
                    return m_session.Id;
                }

                return null;
            }
        }

        /// <summary>
        /// The identity context to use when processing the request.
        /// </summary>
        /// <value>The user identity.</value>
        public IUserIdentity UserIdentity
        {
            get { return m_identity; }
        }

        /// <summary>
        /// The locales to use for the operation.
        /// </summary>
        /// <value>The preferred locales.</value>
        public IList<string> PreferredLocales
        {
            get { return m_preferredLocales; }
        }

        /// <summary>
        /// The diagnostics mask specified with the request.
        /// </summary>
        /// <value>The diagnostics mask.</value>
        public DiagnosticsMasks DiagnosticsMask
        {
            get { return m_diagnosticsMask; }
        }

        /// <summary>
        /// A table of diagnostics strings to return in the response.
        /// </summary>
        /// <value>The string table.</value>
        /// <remarks>
        /// This object is thread safe.
        /// </remarks>
        public StringTable StringTable
        {
            get { return m_stringTable; }
        }

        /// <summary>
        /// When the request times out.
        /// </summary>
        /// <value>The operation deadline.</value>
        public DateTime OperationDeadline
        {
            get { return m_operationDeadline; }
        }

        /// <summary>
        /// The current status of the request (used to check for timeouts/client cancel requests).
        /// </summary>
        /// <value>The operation status.</value>
        public StatusCode OperationStatus
        {
            get { return (uint)m_operationStatus; }
        }

        /// <summary>
        /// The audit log entry id provided by the client which must be included in an audit events generated by the server.
        /// </summary>
        /// <value>The audit entry id.</value>
        public string AuditEntryId
        {
            get { return m_auditLogEntryId; }
        }
        #endregion

        #region Private Fields
        private SecureChannelContext m_channelContext;
        private Sessions.Session m_session;
        private IUserIdentity m_identity;
        private IList<string> m_preferredLocales;
        private DiagnosticsMasks m_diagnosticsMask;
        private StringTable m_stringTable;
        private string m_auditLogEntryId;
        private uint m_requestId;        
        private Sessions.RequestType m_requestType;
        private uint m_clientHandle;
        private DateTime m_operationDeadline;
        private long m_operationStatus;
        private static long s_lastRequestId;
        #endregion
    }
}
