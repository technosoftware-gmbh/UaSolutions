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
    /// Interface for storing subscriptions on server shutdown and restoring on startup
    /// </summary>
    public interface IUaSubscriptionStore
    {
        /// <summary>
        /// Restore subscriptions from storage, called on server startup
        /// </summary>
        /// <returns>the result of the restore operation</returns>
        RestoreSubscriptionResult RestoreSubscriptions();

        /// <summary>
        /// Store subscriptions in storage, called on server shutdown
        /// </summary>
        /// <param name="subscriptions">the subscription templates to store</param>
        /// <returns>true if storing was successful</returns>
        bool StoreSubscriptions(IEnumerable<IUaStoredSubscription> subscriptions);

        /// <summary>
        /// Restore a DataChangeMonitoredItemQueue from storage
        /// </summary>
        /// <param name="monitoredItemId">Id of the UaMonitoredItem owning the queue</param>
        /// <returns>the queue</returns>
        IUaDataChangeMonitoredItemQueue RestoreDataChangeMonitoredItemQueue(uint monitoredItemId);

        /// <summary>
        /// Restore an EventMonitoredItemQueue from storage
        /// </summary>
        /// <param name="monitoredItemId">Id of the UaMonitoredItem owning the queue</param>
        /// <returns>the queue</returns>
        IUaEventMonitoredItemQueue RestoreEventMonitoredItemQueue(uint monitoredItemId);

        /// <summary>
        /// Signals created Subscription ids incl. UaMonitoredItem ids to the SubscriptionStore instance, to signal cleanup can take place
        /// The store shall clean all stored subscriptions, monitoredItems, and only keep the persitent queues for the monitoredItem ids provided
        /// <param name="createdSubscriptions"> key = subscription id, value = monitoredItem ids </param>
        /// </summary>
        void OnSubscriptionRestoreComplete(Dictionary<uint, uint[]> createdSubscriptions);
    }

    /// <summary>
    /// Result of a restore operation
    /// </summary>
    public class RestoreSubscriptionResult
    {
        /// <summary>
        /// Creates a new instance of the result
        /// </summary>
        public RestoreSubscriptionResult(
            bool succcess,
            IEnumerable<IUaStoredSubscription> subscriptions)
        {
            Success = succcess;
            Subscriptions = subscriptions;
        }

        /// <summary>
        /// If the restore operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The restored subscriptions
        /// </summary>
        public IEnumerable<IUaStoredSubscription> Subscriptions { get; set; }
    }
}
