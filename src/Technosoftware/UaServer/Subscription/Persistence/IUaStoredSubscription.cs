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
using System.Collections.Generic;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// A subscription in a format to be persited by an <see cref="IUaSubscriptionStore"/>
    /// </summary>
    public interface IUaStoredSubscription
    {
        /// <summary>
        /// The Id of the subscription
        /// </summary>
        uint Id { get; set; }

        /// <summary>
        /// If the subscription is a durable susbscrition
        /// </summary>
        bool IsDurable { get; set; }

        /// <summary>
        /// The lifetime counter
        /// </summary>
        uint LifetimeCounter { get; set; }

        /// <summary>
        /// The max lifetime count
        /// </summary>
        uint MaxLifetimeCount { get; set; }

        /// <summary>
        /// the max keepalive count
        /// </summary>
        uint MaxKeepaliveCount { get; set; }

        /// <summary>
        /// The max message count
        /// </summary>
        uint MaxMessageCount { get; set; }

        /// <summary>
        /// The max notifications being sent to a client in a single publish message
        /// </summary>
        uint MaxNotificationsPerPublish { get; set; }

        /// <summary>
        /// The monitored items being owned by the subscription
        /// </summary>
        IEnumerable<IUaStoredMonitoredItem> MonitoredItems { get; set; }

        /// <summary>
        /// The priority of the subscription
        /// </summary>
        byte Priority { get; set; }

        /// <summary>
        /// The publishing interval
        /// </summary>
        double PublishingInterval { get; set; }

        /// <summary>
        /// The last messages sent to the client / queued for sending
        /// </summary>
        List<NotificationMessage> SentMessages { get; set; }

        /// <summary>
        /// The last message sent by the subscription
        /// </summary>
        int LastSentMessage { get; set; }

        /// <summary>
        /// The sequence number
        /// </summary>
        long SequenceNumber { get; set; }

        /// <summary>
        /// The user identity of the subscription
        /// </summary>
        UserIdentityToken UserIdentityToken { get; set; }
    }
}
