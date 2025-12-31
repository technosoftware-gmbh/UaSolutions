#region Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Technosoftware.UaServer.Tests;
#endregion Using Directives

namespace Technosoftware.UaClient.Tests
{
    /// <summary>
    /// Map test services to client session API.
    /// </summary>
    public class ClientTestServices : IServerTestServices
    {
        private readonly IUaSession m_session;

        public ITelemetryContext Telemetry { get; }
        public ILogger Logger { get; }

        public ClientTestServices(IUaSession session, ITelemetryContext telemetry)
        {
            m_session = session;
            Telemetry = telemetry;
            Logger = telemetry.CreateLogger<ClientTestServices>();
        }

        public ValueTask<BrowseResponse> BrowseAsync(
            RequestHeader requestHeader,
            ViewDescription view,
            uint requestedMaxReferencesPerNode,
            BrowseDescriptionCollection nodesToBrowse,
            CancellationToken ct = default)
        {
            return new ValueTask<BrowseResponse>(m_session.BrowseAsync(
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
            return new ValueTask<BrowseNextResponse>(m_session.BrowseNextAsync(
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
            return new ValueTask<CreateSubscriptionResponse>(m_session.CreateSubscriptionAsync(
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
            return new ValueTask<CreateMonitoredItemsResponse>(m_session.CreateMonitoredItemsAsync(
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
            return new ValueTask<ModifySubscriptionResponse>(m_session.ModifySubscriptionAsync(
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
            return new ValueTask<ModifyMonitoredItemsResponse>(m_session.ModifyMonitoredItemsAsync(
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
            return new ValueTask<PublishResponse>(m_session.PublishAsync(
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
            return new ValueTask<SetPublishingModeResponse>(m_session.SetPublishingModeAsync(
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
            return new ValueTask<SetMonitoringModeResponse>(m_session.SetMonitoringModeAsync(
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
            return new ValueTask<RepublishResponse>(m_session.RepublishAsync(
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
            return new ValueTask<DeleteSubscriptionsResponse>(m_session.DeleteSubscriptionsAsync(
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
            return new ValueTask<TransferSubscriptionsResponse>(m_session.TransferSubscriptionsAsync(
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
            return new ValueTask<TranslateBrowsePathsToNodeIdsResponse>(m_session.TranslateBrowsePathsToNodeIdsAsync(
                requestHeader,
                browsePaths,
                ct));
        }
    }
}
