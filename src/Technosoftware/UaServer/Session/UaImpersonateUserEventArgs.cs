#region Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved

#region Using Directives

using System;

using Opc.Ua;

#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    ///     A class which provides the event arguments for session related event.
    /// </summary>
    [Obsolete("Use ImpersonateUserEventArgs")]
    public class UaImpersonateUserEventArgs : EventArgs
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        ///     Creates a new instance.
        /// </summary>
        public UaImpersonateUserEventArgs(UserIdentityToken newIdentity, UserTokenPolicy userTokenPolicy, EndpointDescription endpointDescription = null)
        {
            m_newIdentity = newIdentity;
            m_userTokenPolicy = userTokenPolicy;
            m_endpointDescription = endpointDescription;
        }
        #endregion Constructors, Destructor, Initialization

        #region Public Properties
        /// <summary>
        ///     The new user identity for the session.
        /// </summary>
        public UserIdentityToken NewIdentity
        {
            get { return m_newIdentity; }
        }

        /// <summary>
        ///     The user token policy selected by the client.
        /// </summary>
        public UserTokenPolicy UserTokenPolicy
        {
            get { return m_userTokenPolicy; }
        }

        /// <summary>
        ///     An application defined handle that can be used for access control operations.
        /// </summary>
        public IUserIdentity Identity
        {
            get { return m_identity; }
            set { m_identity = value; }
        }

        /// <summary>
        ///     An application defined handle that can be used for access control operations.
        /// </summary>
        public IUserIdentity EffectiveIdentity
        {
            get { return m_effectiveIdentity; }
            set { m_effectiveIdentity = value; }
        }

        /// <summary>
        ///     Set to indicate that an error occurred validating the identity and that it should be rejected.
        /// </summary>
        public ServiceResult IdentityValidationError
        {
            get { return m_identityValidationError; }
            set { m_identityValidationError = value; }
        }

        /// <summary>
        /// Get the EndpointDescription  
        /// </summary>
        public EndpointDescription EndpointDescription
        {
            get { return m_endpointDescription; }
        }
        #endregion Public Properties

        #region Private Fields
        private UserIdentityToken m_newIdentity;
        private UserTokenPolicy m_userTokenPolicy;
        private ServiceResult m_identityValidationError;
        private IUserIdentity m_identity;
        private IUserIdentity m_effectiveIdentity;
        private EndpointDescription m_endpointDescription;
        #endregion Private Fields
    }
}
