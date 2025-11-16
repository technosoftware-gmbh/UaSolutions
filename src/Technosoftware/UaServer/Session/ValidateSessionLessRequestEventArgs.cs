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

using Opc.Ua;

#endregion

namespace Technosoftware.UaServer
{
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
