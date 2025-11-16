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

using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaClient
{
    /// <summary>
    ///     Represents the event arguments provided when a new notification message arrives.
    /// </summary>
    public class NotificationEventArgs : EventArgs
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public NotificationEventArgs(
            Subscription subscription,
            NotificationMessage notificationMessage,
            IList<string> stringTable)
        {
            Subscription = subscription;
            NotificationMessage = notificationMessage;
            StringTable = stringTable;
        }
        #endregion Constructors, Destructor, Initialization

        #region Public Properties
        /// <summary>
        /// Gets the subscription that the notification applies to.
        /// </summary>
        public Subscription Subscription { get; }

        /// <summary>
        /// Gets the notification message.
        /// </summary>
        public NotificationMessage NotificationMessage { get; }

        /// <summary>
        /// Gets the string table returned with the notification message.
        /// </summary>
        public IList<string> StringTable { get; }
        #endregion Public Properties

        #region Private Fields
        #endregion Private Fields
    }
}
