#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer.Tests
{
    /// <summary>
    /// Interface for common test framework services.
    /// </summary>
    public interface IServerTestServices
    {
        ITelemetryContext Telemetry { get; }

        ILogger Logger { get; }

        ValueTask<BrowseResponse> BrowseAsync(
            RequestHeader requestHeader,
            ViewDescription view,
            uint requestedMaxReferencesPerNode,
            BrowseDescriptionCollection nodesToBrowse,
            CancellationToken ct = default);

        ValueTask<BrowseNextResponse> BrowseNextAsync(
            RequestHeader requestHeader,
            bool releaseContinuationPoints,
            ByteStringCollection continuationPoints,
            CancellationToken ct = default);

        ValueTask<TranslateBrowsePathsToNodeIdsResponse> TranslateBrowsePathsToNodeIdsAsync(
            RequestHeader requestHeader,
            BrowsePathCollection browsePaths,
            CancellationToken ct = default);

        ValueTask<CreateSubscriptionResponse> CreateSubscriptionAsync(
            RequestHeader requestHeader,
            double requestedPublishingInterval,
            uint requestedLifetimeCount,
            uint requestedMaxKeepAliveCount,
            uint maxNotificationsPerPublish,
            bool publishingEnabled,
            byte priority,
            CancellationToken ct = default);

        ValueTask<CreateMonitoredItemsResponse> CreateMonitoredItemsAsync(
            RequestHeader requestHeader,
            uint subscriptionId,
            TimestampsToReturn timestampsToReturn,
            MonitoredItemCreateRequestCollection itemsToCreate,
            CancellationToken ct = default);

        ValueTask<ModifySubscriptionResponse> ModifySubscriptionAsync(
            RequestHeader requestHeader,
            uint subscriptionId,
            double requestedPublishingInterval,
            uint requestedLifetimeCount,
            uint requestedMaxKeepAliveCount,
            uint maxNotificationsPerPublish,
            byte priority,
            CancellationToken ct = default);

        ValueTask<ModifyMonitoredItemsResponse> ModifyMonitoredItemsAsync(
            RequestHeader requestHeader,
            uint subscriptionId,
            TimestampsToReturn timestampsToReturn,
            MonitoredItemModifyRequestCollection itemsToModify,
            CancellationToken ct = default);

        ValueTask<PublishResponse> PublishAsync(
            RequestHeader requestHeader,
            SubscriptionAcknowledgementCollection subscriptionAcknowledgements,
            CancellationToken ct = default);

        ValueTask<SetPublishingModeResponse> SetPublishingModeAsync(
            RequestHeader requestHeader,
            bool publishingEnabled,
            UInt32Collection subscriptionIds,
            CancellationToken ct = default);

        ValueTask<SetMonitoringModeResponse> SetMonitoringModeAsync(
            RequestHeader requestHeader,
            uint subscriptionId,
            MonitoringMode monitoringMode,
            UInt32Collection monitoredItemIds,
            CancellationToken ct = default);

        ValueTask<RepublishResponse> RepublishAsync(
            RequestHeader requestHeader,
            uint subscriptionId,
            uint retransmitSequenceNumber,
            CancellationToken ct = default);

        ValueTask<DeleteSubscriptionsResponse> DeleteSubscriptionsAsync(
            RequestHeader requestHeader,
            UInt32Collection subscriptionIds,
            CancellationToken ct = default);

        ValueTask<TransferSubscriptionsResponse> TransferSubscriptionsAsync(
            RequestHeader requestHeader,
            UInt32Collection subscriptionIds,
            bool sendInitialValues,
            CancellationToken ct = default);
    }

    /// <summary>
    /// Implementation for a standard server.
    /// </summary>
    public class ServerTestServices : IServerTestServices
    {
        private readonly ISessionServer m_server;

        public ITelemetryContext Telemetry { get; }

        public ILogger Logger { get; }

        public SecureChannelContext SecureChannelContext { get; set; }

        public ServerTestServices(ISessionServer server, SecureChannelContext secureChannelContext, ITelemetryContext telemetry)
        {
            Telemetry = telemetry;
            Logger = telemetry.CreateLogger<ServerTestServices>();
            m_server = server;
            SecureChannelContext = secureChannelContext;
        }

        public ValueTask<BrowseResponse> BrowseAsync(
            RequestHeader requestHeader,
            ViewDescription view,
            uint requestedMaxReferencesPerNode,
            BrowseDescriptionCollection nodesToBrowse,
            CancellationToken ct = default)
        {
            return new ValueTask<BrowseResponse>(m_server.BrowseAsync(
                SecureChannelContext,
                requestHeader,
                view,
                requestedMaxReferencesPerNode,
                nodesToBrowse,
                ct));
        }

        public ValueTask<BrowseNextResponse> BrowseNextAsync(
            RequestHeader requestHeader,
            bool releaseContinuationPoints,
            ByteStringCollection continuationPoints,
            CancellationToken ct = default)
        {
            return new ValueTask<BrowseNextResponse>(m_server.BrowseNextAsync(
                SecureChannelContext,
                requestHeader,
                releaseContinuationPoints,
                continuationPoints,
                ct));
        }

        public ValueTask<CreateSubscriptionResponse> CreateSubscriptionAsync(
            RequestHeader requestHeader,
            double requestedPublishingInterval,
            uint requestedLifetimeCount,
            uint requestedMaxKeepAliveCount,
            uint maxNotificationsPerPublish,
            bool publishingEnabled,
            byte priority,
            CancellationToken ct = default)
        {
            return new ValueTask<CreateSubscriptionResponse>(m_server.CreateSubscriptionAsync(
                SecureChannelContext,
                requestHeader,
                requestedPublishingInterval,
                requestedLifetimeCount,
                requestedMaxKeepAliveCount,
                maxNotificationsPerPublish,
                publishingEnabled,
                priority,
                ct));
        }

        public ValueTask<CreateMonitoredItemsResponse> CreateMonitoredItemsAsync(
            RequestHeader requestHeader,
            uint subscriptionId,
            TimestampsToReturn timestampsToReturn,
            MonitoredItemCreateRequestCollection itemsToCreate,
            CancellationToken ct = default)
        {
            return new ValueTask<CreateMonitoredItemsResponse>(m_server.CreateMonitoredItemsAsync(
                SecureChannelContext,
                requestHeader,
                subscriptionId,
                timestampsToReturn,
                itemsToCreate,
                ct));
        }

        public ValueTask<ModifySubscriptionResponse> ModifySubscriptionAsync(
            RequestHeader requestHeader,
            uint subscriptionId,
            double requestedPublishingInterval,
            uint requestedLifetimeCount,
            uint requestedMaxKeepAliveCount,
            uint maxNotificationsPerPublish,
            byte priority,
            CancellationToken ct = default)
        {
            return new ValueTask<ModifySubscriptionResponse>(m_server.ModifySubscriptionAsync(
                SecureChannelContext,
                requestHeader,
                subscriptionId,
                requestedPublishingInterval,
                requestedLifetimeCount,
                requestedMaxKeepAliveCount,
                maxNotificationsPerPublish,
                priority,
                ct));
        }

        public ValueTask<ModifyMonitoredItemsResponse> ModifyMonitoredItemsAsync(
            RequestHeader requestHeader,
            uint subscriptionId,
            TimestampsToReturn timestampsToReturn,
            MonitoredItemModifyRequestCollection itemsToModify,
            CancellationToken ct = default)
        {
            return new ValueTask<ModifyMonitoredItemsResponse>(m_server.ModifyMonitoredItemsAsync(
                SecureChannelContext,
                requestHeader,
                subscriptionId,
                timestampsToReturn,
                itemsToModify,
                ct));
        }

        public ValueTask<PublishResponse> PublishAsync(
            RequestHeader requestHeader,
            SubscriptionAcknowledgementCollection subscriptionAcknowledgements,
            CancellationToken ct = default)
        {
            return new ValueTask<PublishResponse>(m_server.PublishAsync(
                SecureChannelContext,
                requestHeader,
                subscriptionAcknowledgements,
                ct));
        }

        public ValueTask<SetPublishingModeResponse> SetPublishingModeAsync(
            RequestHeader requestHeader,
            bool publishingEnabled,
            UInt32Collection subscriptionIds,
            CancellationToken ct = default)
        {
            return new ValueTask<SetPublishingModeResponse>(m_server.SetPublishingModeAsync(
                SecureChannelContext,
                requestHeader,
                publishingEnabled,
                subscriptionIds,
                ct));
        }

        public ValueTask<SetMonitoringModeResponse> SetMonitoringModeAsync(
            RequestHeader requestHeader,
            uint subscriptionId,
            MonitoringMode monitoringMode,
            UInt32Collection monitoredItemIds,
            CancellationToken ct = default)
        {
            return new ValueTask<SetMonitoringModeResponse>(
                m_server.SetMonitoringModeAsync(
                SecureChannelContext,
                requestHeader,
                subscriptionId,
                monitoringMode,
                monitoredItemIds,
                    ct));
        }

        public ValueTask<RepublishResponse> RepublishAsync(
            RequestHeader requestHeader,
            uint subscriptionId,
            uint retransmitSequenceNumber,
            CancellationToken ct = default)
        {
            return new ValueTask<RepublishResponse>(m_server.RepublishAsync(
                SecureChannelContext,
                requestHeader,
                subscriptionId,
                retransmitSequenceNumber,
                ct));
        }

        public ValueTask<DeleteSubscriptionsResponse> DeleteSubscriptionsAsync(
            RequestHeader requestHeader,
            UInt32Collection subscriptionIds,
            CancellationToken ct = default)
        {
            return new ValueTask<DeleteSubscriptionsResponse>(m_server.DeleteSubscriptionsAsync(
                SecureChannelContext,
                requestHeader,
                subscriptionIds,
                ct));
        }

        public ValueTask<TransferSubscriptionsResponse> TransferSubscriptionsAsync(
            RequestHeader requestHeader,
            UInt32Collection subscriptionIds,
            bool sendInitialValues,
            CancellationToken ct = default)
        {
            return new ValueTask<TransferSubscriptionsResponse>(m_server.TransferSubscriptionsAsync(
                SecureChannelContext,
                requestHeader,
                subscriptionIds,
                sendInitialValues,
                ct));
        }

        public ValueTask<TranslateBrowsePathsToNodeIdsResponse> TranslateBrowsePathsToNodeIdsAsync(
            RequestHeader requestHeader,
            BrowsePathCollection browsePaths,
            CancellationToken ct = default)
        {
            return new ValueTask<TranslateBrowsePathsToNodeIdsResponse>(m_server.TranslateBrowsePathsToNodeIdsAsync(
                SecureChannelContext,
                requestHeader,
                browsePaths,
                ct));
        }
    }
}
