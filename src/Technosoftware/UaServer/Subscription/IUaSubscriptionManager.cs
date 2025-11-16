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

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Provides access to the subscription manager within the server.
    /// </summary>
    /// <remarks>
    /// Sinks that receive these events must not block the thread.
    /// </remarks>
    public interface IUaSubscriptionManager : IDisposable
    {
        /// <summary>
        /// Raised after a new subscription is created.
        /// </summary>
        event EventHandler<SubscriptionEventArgs> SubscriptionCreated;

        /// <summary>
        /// Raised before a subscription is deleted.
        /// </summary>
        event EventHandler<SubscriptionEventArgs> SubscriptionDeleted;

        /// <summary>
        /// Returns all of the subscriptions known to the subscription manager.
        /// </summary>
        /// <returns>A list of the subscriptions.</returns>
        IList<IUaSubscription> GetSubscriptions();

        /// <summary>
        /// Set a subscription into durable mode
        /// </summary>
        ServiceResult SetSubscriptionDurable(
            ISystemContext context,
            uint subscriptionId,
            uint lifetimeInHours,
            out uint revisedLifetimeInHours);

        /// <summary>
        /// Creates a new subscription.
        /// </summary>
        void CreateSubscription(
            UaServerOperationContext context,
            double requestedPublishingInterval,
            uint requestedLifetimeCount,
            uint requestedMaxKeepAliveCount,
            uint maxNotificationsPerPublish,
            bool publishingEnabled,
            byte priority,
            out uint subscriptionId,
            out double revisedPublishingInterval,
            out uint revisedLifetimeCount,
            out uint revisedMaxKeepAliveCount);

        /// <summary>
        /// Starts up the manager makes it ready to create subscriptions.
        /// </summary>
        void Startup();

        /// <summary>
        /// Closes all subscriptions and rejects any new requests.
        /// </summary>
        void Shutdown();

        /// <summary>
        /// Stores durable subscriptions to  be able to restore them after a restart
        /// </summary>
        void StoreSubscriptions();

        /// <summary>
        /// Restore durable subscriptions after a server restart
        /// </summary>
        void RestoreSubscriptions();

        /// <summary>
        /// Deletes group of subscriptions.
        /// </summary>
        void DeleteSubscriptions(
            UaServerOperationContext context,
            UInt32Collection subscriptionIds,
            out StatusCodeCollection results,
            out DiagnosticInfoCollection diagnosticInfos);

        /// <summary>
        /// Publishes a subscription.
        /// </summary>
        NotificationMessage Publish(
            UaServerOperationContext context,
            SubscriptionAcknowledgementCollection subscriptionAcknowledgements,
            AsyncPublishOperation operation,
            out uint subscriptionId,
            out UInt32Collection availableSequenceNumbers,
            out bool moreNotifications,
            out StatusCodeCollection acknowledgeResults,
            out DiagnosticInfoCollection acknowledgeDiagnosticInfos);

        /// <summary>
        /// Completes the publish.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="operation">The asynchronous operation.</param>
        /// <returns>
        /// True if successful. False if the request has been requeued.
        /// </returns>
        bool CompletePublish(UaServerOperationContext context, AsyncPublishOperation operation);

        /// <summary>
        /// Modifies an existing subscription.
        /// </summary>
        void ModifySubscription(
            UaServerOperationContext context,
            uint subscriptionId,
            double requestedPublishingInterval,
            uint requestedLifetimeCount,
            uint requestedMaxKeepAliveCount,
            uint maxNotificationsPerPublish,
            byte priority,
            out double revisedPublishingInterval,
            out uint revisedLifetimeCount,
            out uint revisedMaxKeepAliveCount);

        /// <summary>
        /// Sets the publishing mode for a set of subscriptions.
        /// </summary>
        void SetPublishingMode(
            UaServerOperationContext context,
            bool publishingEnabled,
            UInt32Collection subscriptionIds,
            out StatusCodeCollection results,
            out DiagnosticInfoCollection diagnosticInfos);

        /// <summary>
        /// Attaches a groups of subscriptions to a different session.
        /// </summary>
        void TransferSubscriptions(
            UaServerOperationContext context,
            UInt32Collection subscriptionIds,
            bool sendInitialValues,
            out TransferResultCollection results,
            out DiagnosticInfoCollection diagnosticInfos);

        /// <summary>
        /// Republishes a previously published notification message.
        /// </summary>
        NotificationMessage Republish(
            UaServerOperationContext context,
            uint subscriptionId,
            uint retransmitSequenceNumber);

        /// <summary>
        /// Updates the triggers for the monitored item.
        /// </summary>
        void SetTriggering(
            UaServerOperationContext context,
            uint subscriptionId,
            uint triggeringItemId,
            UInt32Collection linksToAdd,
            UInt32Collection linksToRemove,
            out StatusCodeCollection addResults,
            out DiagnosticInfoCollection addDiagnosticInfos,
            out StatusCodeCollection removeResults,
            out DiagnosticInfoCollection removeDiagnosticInfos);

        /// <summary>
        /// Adds monitored items to a subscription.
        /// </summary>
        void CreateMonitoredItems(
            UaServerOperationContext context,
            uint subscriptionId,
            TimestampsToReturn timestampsToReturn,
            MonitoredItemCreateRequestCollection itemsToCreate,
            out MonitoredItemCreateResultCollection results,
            out DiagnosticInfoCollection diagnosticInfos);

        /// <summary>
        /// Modifies monitored items in a subscription.
        /// </summary>
        void ModifyMonitoredItems(
            UaServerOperationContext context,
            uint subscriptionId,
            TimestampsToReturn timestampsToReturn,
            MonitoredItemModifyRequestCollection itemsToModify,
            out MonitoredItemModifyResultCollection results,
            out DiagnosticInfoCollection diagnosticInfos);

        /// <summary>
        /// Deletes the monitored items in a subscription.
        /// </summary>
        void DeleteMonitoredItems(
            UaServerOperationContext context,
            uint subscriptionId,
            UInt32Collection monitoredItemIds,
            out StatusCodeCollection results,
            out DiagnosticInfoCollection diagnosticInfos);

        /// <summary>
        /// Changes the monitoring mode for a set of items.
        /// </summary>
        void SetMonitoringMode(
            UaServerOperationContext context,
            uint subscriptionId,
            MonitoringMode monitoringMode,
            UInt32Collection monitoredItemIds,
            out StatusCodeCollection results,
            out DiagnosticInfoCollection diagnosticInfos);

        /// <summary>
        /// Signals that a session is closing.
        /// </summary>
        void SessionClosing(UaServerOperationContext context, NodeId sessionId, bool deleteSubscriptions);

        /// <summary>
        /// Deletes the specified subscription.
        /// </summary>
        StatusCode DeleteSubscription(UaServerOperationContext context, uint subscriptionId);

        /// <summary>
        /// Refreshes the conditions for the specified subscription.
        /// </summary>
        void ConditionRefresh(UaServerOperationContext context, uint subscriptionId);

        /// <summary>
        /// Refreshes the conditions for the specified subscription and monitored item.
        /// </summary>
        void ConditionRefresh2(UaServerOperationContext context, uint subscriptionId, uint monitoredItemId);
    }
}
