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
using System.Diagnostics.Tracing;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaClient
{
    /// <summary>
    /// EventSource for client.
    /// </summary>
    public static partial class UaClientUtils
    {
        /// <summary>
        /// The EventSource log interface.
        /// </summary>
        internal static UaClientEventSource EventLog { get; } = new UaClientEventSource();
    }

    /// <summary>
    /// Event source for high performance logging.
    /// </summary>
    [EventSource(Name = "Technosoftware.UaClient", Guid = "70A02853-C24D-4C36-8A3B-0AD181BE7D46")]
    internal class UaClientEventSource : EventSource
    {
        internal const int SubscriptionStateId = 1;
        internal const int NotificationId = SubscriptionStateId + 1;
        internal const int NotificationReceivedId = NotificationId + 1;
        internal const int PublishStartId = NotificationReceivedId + 1;
        internal const int PublishStopId = PublishStartId + 1;

        /// <summary>
        /// The state of the client subscription.
        /// </summary>
        [Event(
            SubscriptionStateId,
            Message = "Subscription {0}, Id={1}, LastNotificationTime={2:HH:mm:ss}, " +
                "GoodPublishRequestCount={3}, PublishingInterval={4}, KeepAliveCount={5}, " +
                "PublishingEnabled={6}, MonitoredItemCount={7}",
            Level = EventLevel.Verbose)]
        public void SubscriptionState(
            string context,
            uint id,
            DateTime lastNotificationTime,
            int goodPublishRequestCount,
            double currentPublishingInterval,
            uint currentKeepAliveCount,
            bool currentPublishingEnabled,
            uint monitoredItemCount)
        {
            WriteEvent(
                SubscriptionStateId,
                context,
                id,
                lastNotificationTime,
                goodPublishRequestCount,
                currentPublishingInterval,
                currentKeepAliveCount,
                currentPublishingEnabled,
                monitoredItemCount);
        }

        /// <summary>
        /// The notification message. Called internally to convert wrapped value.
        /// </summary>
        [Event(
            NotificationId,
            Message = "Notification: ClientHandle={0}, Value={1}",
            Level = EventLevel.Verbose)]
        public void Notification(int clientHandle, Variant value)
        {
            if (IsEnabled())
            {
                WriteEvent(NotificationId, clientHandle, value.ToString());
            }
        }

        /// <summary>
        /// A notification received in Publish complete.
        /// </summary>
        [Event(
            NotificationReceivedId,
            Message = "NOTIFICATION RECEIVED: SubId={0}, SeqNo={1}",
            Level = EventLevel.Verbose)]
        public void NotificationReceived(int subscriptionId, int sequenceNumber)
        {
            if (IsEnabled())
            {
                WriteEvent(NotificationReceivedId, subscriptionId, sequenceNumber);
            }
        }

        /// <summary>
        /// A Publish begin received.
        /// </summary>
        [Event(
            PublishStartId,
            Message = "PUBLISH #{0} SENT",
            Level = EventLevel.Verbose)]
        public void PublishStart(int requestHandle)
        {
            if (IsEnabled())
            {
                WriteEvent(PublishStartId, requestHandle);
            }
        }

        /// <summary>
        /// A Publish complete received.
        /// </summary>
        [Event(
            PublishStopId,
            Message = "PUBLISH #{0} RECEIVED",
            Level = EventLevel.Verbose)]
        public void PublishStop(int requestHandle)
        {
            if (IsEnabled())
            {
                WriteEvent(PublishStopId, requestHandle);
            }
        }
    }
}
