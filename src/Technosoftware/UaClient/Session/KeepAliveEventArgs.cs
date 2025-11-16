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
#endregion Using Directives

namespace Technosoftware.UaClient
{
    /// <summary>
    /// The event arguments provided when a keep alive response arrives.
    /// </summary>
    public class KeepAliveEventArgs : EventArgs
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public KeepAliveEventArgs(
            ServiceResult status,
            ServerState currentState,
            DateTime currentTime)
        {
            Status = status;
            CurrentState = currentState;
            CurrentTime = currentTime;
        }
        #endregion Constructors, Destructor, Initialization

        #region Public Properties
        /// <summary>
        /// Gets the status associated with the keep alive operation.
        /// </summary>
        public ServiceResult Status { get; }

        /// <summary>
        /// Gets the current server state.
        /// </summary>
        public ServerState CurrentState { get; }

        /// <summary>
        /// Gets the current server time.
        /// </summary>
        public DateTime CurrentTime { get; }

        /// <summary>
        /// Gets or sets a flag indicating whether the session should send another keep alive.
        /// </summary>
        public bool CancelKeepAlive { get; set; }
        #endregion Public Properties

        #region Private Fields
        #endregion Private Fields
    }
}
