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
using Microsoft.Extensions.Logging;
using static Opc.Ua.Utils;
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
        /// The client messages.
        /// </summary>
        internal const string SubscriptionStateMessage =
            "Subscription {0}, Id={1}, LastNotificationTime={2:HH:mm:ss}, GoodPublishRequestCount={3}, PublishingInterval={4}, KeepAliveCount={5}, PublishingEnabled={6}, MonitoredItemCount={7}";

        internal const string NotificationMessage = "Notification: ClientHandle={0}, Value={1}";

        internal const string NotificationReceivedMessage
            = "NOTIFICATION RECEIVED: SubId={0}, SeqNo={1}";

        internal const string PublishStartMessage = "PUBLISH #{0} SENT";
        internal const string PublishStopMessage = "PUBLISH #{0} RECEIVED";

        /// <summary>
        /// The Client Event Ids used for event messages, when calling ILogger.
        /// </summary>
        internal readonly EventId SubscriptionStateMessageEventId = new(
            TraceMasks.Operation,
            nameof(SubscriptionState));

        internal readonly EventId NotificationEventId = new(
            TraceMasks.Operation,
            nameof(Notification));

        internal readonly EventId NotificationReceivedEventId = new(
            TraceMasks.Operation,
            nameof(NotificationReceived));

        internal readonly EventId PublishStartEventId = new(
            TraceMasks.ServiceDetail,
            nameof(PublishStart));

        internal readonly EventId PublishStopEventId = new(
            TraceMasks.ServiceDetail,
            nameof(PublishStop));

        /// <summary>
        /// The state of the client subscription.
        /// </summary>
        [Event(SubscriptionStateId, Message = SubscriptionStateMessage, Level = EventLevel.Verbose)]
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
            if (IsEnabled())
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
            else if (Logger.IsEnabled(LogLevel.Information))
            {
                LogInfo(
                    SubscriptionStateMessageEventId,
                    SubscriptionStateMessage,
                    context,
                    id,
                    lastNotificationTime,
                    goodPublishRequestCount,
                    currentPublishingInterval,
                    currentKeepAliveCount,
                    currentPublishingEnabled,
                    monitoredItemCount);
            }
        }

        /// <summary>
        /// The notification message. Called internally to convert wrapped value.
        /// </summary>
        [Event(NotificationId, Message = NotificationMessage, Level = EventLevel.Verbose)]
        public void Notification(int clientHandle, string value)
        {
            WriteEvent(NotificationId, value, clientHandle);
        }

        /// <summary>
        /// A notification received in Publish complete.
        /// </summary>
        [Event(
            NotificationReceivedId,
            Message = NotificationReceivedMessage,
            Level = EventLevel.Verbose)]
        public void NotificationReceived(int subscriptionId, int sequenceNumber)
        {
            if (IsEnabled())
            {
                WriteEvent(NotificationReceivedId, subscriptionId, sequenceNumber);
            }
            else if (Logger.IsEnabled(LogLevel.Trace))
            {
                LogTrace(
                    NotificationReceivedEventId,
                    NotificationReceivedMessage,
                    subscriptionId,
                    sequenceNumber);
            }
        }

        /// <summary>
        /// A Publish begin received.
        /// </summary>
        [Event(PublishStartId, Message = PublishStartMessage, Level = EventLevel.Verbose)]
        public void PublishStart(int requestHandle)
        {
            if (IsEnabled())
            {
                WriteEvent(PublishStartId, requestHandle);
            }
            else if (Logger.IsEnabled(LogLevel.Trace))
            {
                LogTrace(PublishStartEventId, PublishStartMessage, requestHandle);
            }
        }

        /// <summary>
        /// A Publish complete received.
        /// </summary>
        [Event(PublishStopId, Message = PublishStopMessage, Level = EventLevel.Verbose)]
        public void PublishStop(int requestHandle)
        {
            if (IsEnabled())
            {
                WriteEvent(PublishStopId, requestHandle);
            }
            else if (Logger.IsEnabled(LogLevel.Trace))
            {
                LogTrace(PublishStopEventId, PublishStopMessage, requestHandle);
            }
        }

        /// <summary>
        /// Log a Notification.
        /// </summary>
        [NonEvent]
        public void NotificationValue(uint clientHandle, Variant wrappedValue)
        {
            // expensive operation, only enable if tracemask set
            if ((TraceMask & TraceMasks.OperationDetail) != 0)
            {
                if (IsEnabled())
                {
                    Notification((int)clientHandle, wrappedValue.ToString());
                }
                else if (Logger.IsEnabled(LogLevel.Trace))
                {
                    LogTrace(NotificationEventId, NotificationMessage, clientHandle, wrappedValue);
                }
            }
        }
    }
}
