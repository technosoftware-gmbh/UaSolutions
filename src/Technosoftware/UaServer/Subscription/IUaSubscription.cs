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
using Opc.Ua;
#endregion

namespace Technosoftware.UaServer
{
    /// <summary>
    /// An interface used by the monitored items to signal the subscription.
    /// </summary>
    public interface IUaSubscription
    {
        /// <summary>
        /// The session that owns the monitored item.
        /// </summary>
        Sessions.Session Session { get; }

        /// <summary>
        /// The subscriptions owner identity.
        /// </summary>
        IUserIdentity EffectiveIdentity { get; }

        /// <summary>
        /// The identifier for the item that is unique within the server.
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Called when a monitored item is ready to publish.
        /// </summary>
        void ItemReadyToPublish(IUaMonitoredItem monitoredItem);

        /// <summary>
        /// Called when a monitored item is ready to publish.
        /// </summary>
        void ItemNotificationsAvailable(IUaMonitoredItem monitoredItem);

        /// <summary>
        /// Called when a value of monitored item is discarded in the monitoring queue.
        /// </summary>
        void QueueOverflowHandler();
    }
}
