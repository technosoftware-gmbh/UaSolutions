#region Copyright (c) 2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com
//
// The Software is subject to the Technosoftware GmbH MIT License, which can
// be found here:
// https://technosoftware.com/license/mit/
//
// The Software is based on the OPC Foundation UA Stack and the OPC Foundation
// MIT License. The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Technosoftware.UaServer;
#endregion Using Directives

namespace SampleCompany.NodeManagers.DurableSubscription
{
    /// <summary>
    /// Persists subscriptions and their durable monitored item queues to local files.
    /// </summary>
    /// <remarks>
    /// The store wrote JSON until the 2.0 stack turned the built-ins into value
    /// types, which a general purpose serializer cannot round-trip: a
    /// ByteString is a ReadOnlyMemory and reflection walks straight into it.
    /// The UA binary encoder is used instead, field by field, which is also
    /// what the stack itself uses for state it has to read back.
    /// </remarks>
    public class SubscriptionStore : IUaSubscriptionStore
    {
        private static readonly string s_storage_path = Path.Combine(
            Environment.CurrentDirectory,
            "Durable Subscriptions");

        private const string kFilename = "subscriptionsStore.bin";
        private const uint kStoreMagic = 0x44535541;
        private const uint kStoreVersion = 1;

        private readonly DurableMonitoredItemQueueFactory m_durableMonitoredItemQueueFactory;
        private readonly ILogger m_logger;
        private readonly IServiceMessageContext m_messageContext;

        /// <summary>
        /// Initializes the store with the server's message context and queue factory.
        /// </summary>
        public SubscriptionStore(IUaServerData server)
        {
            m_logger = server.Telemetry.CreateLogger<SubscriptionStore>();
            m_messageContext = server.MessageContext;
            m_durableMonitoredItemQueueFactory = server
                .MonitoredItemQueueFactory as DurableMonitoredItemQueueFactory;
        }

        /// <inheritdoc/>
        public bool StoreSubscriptions(IEnumerable<IUaStoredSubscription> subscriptions)
        {
            try
            {
                if (!Directory.Exists(s_storage_path))
                {
                    Directory.CreateDirectory(s_storage_path);
                }

                List<StoredSubscription> subs = subscriptions
                    .Cast<StoredSubscription>()
                    .ToList();

                // Check every identity before the file is created, so a
                // subscription that cannot be persisted does not truncate a
                // store that still holds good ones.
                foreach (StoredSubscription sub in subs)
                {
                    _ = SanitizeUserIdentityToken(sub.UserIdentityToken);
                }

                using (FileStream fileStream = File.Create(
                    Path.Combine(s_storage_path, kFilename)))
                using (var encoder = new BinaryEncoder(fileStream, m_messageContext, true))
                {
                    WriteStoreHeader(encoder);
                    encoder.WriteStringArray(null, m_messageContext.NamespaceUris.ToArrayOf());
                    encoder.WriteStringArray(null, m_messageContext.ServerUris.ToArrayOf());

                    encoder.WriteInt32(null, subs.Count);
                    foreach (StoredSubscription sub in subs)
                    {
                        EncodeSubscription(encoder, sub);
                    }
                }

                if (m_durableMonitoredItemQueueFactory != null)
                {
                    IEnumerable<uint> ids = subscriptions.SelectMany(
                        s => s.MonitoredItems.Select(m => m.Id));
                    m_durableMonitoredItemQueueFactory.PersistQueues(ids, s_storage_path);
                }
                return true;
            }
            catch (Exception ex)
            {
                m_logger.LogWarning(ex, "Failed to store subscriptions");
            }
            return false;
        }

        /// <inheritdoc/>
        public RestoreSubscriptionResult RestoreSubscriptions()
        {
            string filePath = Path.Combine(s_storage_path, kFilename);
            try
            {
                if (File.Exists(filePath))
                {
                    List<IUaStoredSubscription> result;

                    using (FileStream fileStream = File.OpenRead(filePath))
                    using (var decoder = new BinaryDecoder(fileStream, m_messageContext, true))
                    {
                        uint version = ValidateStoreHeader(decoder);
                        ArrayOf<string> nsUris = decoder.ReadStringArray(null);
                        ArrayOf<string> serverUris = decoder.ReadStringArray(null);
                        decoder.SetMappingTables(
                            new NamespaceTable(nsUris.Memory.ToArray()),
                            new StringTable(serverUris.Memory.ToArray()));

                        int count = decoder.ReadInt32(null);
                        result = new List<IUaStoredSubscription>(count);
                        for (int ii = 0; ii < count; ii++)
                        {
                            result.Add(DecodeSubscription(decoder, version));
                        }
                    }

                    File.Delete(filePath);

                    return new RestoreSubscriptionResult(true, result);
                }
            }
            catch (Exception ex)
            {
                m_logger.LogWarning(ex, "Failed to restore subscriptions");
            }

            return new RestoreSubscriptionResult(false, null);
        }

        /// <inheritdoc/>
        public IUaDataChangeMonitoredItemQueue RestoreDataChangeMonitoredItemQueue(
            uint monitoredItemId)
        {
            return m_durableMonitoredItemQueueFactory?.RestoreDataChangeQueue(
                monitoredItemId,
                s_storage_path);
        }

        /// <inheritdoc/>
        public IUaEventMonitoredItemQueue RestoreEventMonitoredItemQueue(uint monitoredItemId)
        {
            return m_durableMonitoredItemQueueFactory?.RestoreEventQueue(
                monitoredItemId,
                s_storage_path);
        }

        /// <inheritdoc/>
        public void OnSubscriptionRestoreComplete(Dictionary<uint, uint[]> createdSubscriptions)
        {
            string filePath = Path.Combine(s_storage_path, kFilename);

            //remove old file
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    m_logger.LogWarning(ex, "Failed to cleanup files for stored subscsription");
                }
            }
            //remove old batches & queues
            if (m_durableMonitoredItemQueueFactory != null)
            {
                IEnumerable<uint> ids = createdSubscriptions.SelectMany(s => s.Value);
                m_durableMonitoredItemQueueFactory.CleanStoredQueues(s_storage_path, ids);
            }
        }

        /// <summary>
        /// Writes the store's format marker and version.
        /// </summary>
        internal static void WriteStoreHeader(BinaryEncoder encoder)
        {
            encoder.WriteUInt32(null, kStoreMagic);
            encoder.WriteUInt32(null, kStoreVersion);
        }

        /// <summary>
        /// Reads and checks the store's format marker, returning its version.
        /// </summary>
        /// <exception cref="InvalidDataException">
        /// The header or the version is not one this build writes.
        /// </exception>
        internal static uint ValidateStoreHeader(BinaryDecoder decoder)
        {
            uint magic = decoder.ReadUInt32(null);
            if (magic != kStoreMagic)
            {
                throw new InvalidDataException(
                    "The durable subscription store has an unrecognised header.");
            }

            uint version = decoder.ReadUInt32(null);
            ValidateStoreVersion(version);
            return version;
        }

        /// <summary>
        /// Encodes a subscription, its identity, its sent messages and its items.
        /// </summary>
        public static void EncodeSubscription(
            BinaryEncoder encoder,
            StoredSubscription subscription)
        {
            encoder.WriteUInt32(null, subscription.Id);
            encoder.WriteBoolean(null, subscription.IsDurable);
            encoder.WriteUInt32(null, subscription.LifetimeCounter);
            encoder.WriteUInt32(null, subscription.MaxLifetimeCount);
            encoder.WriteUInt32(null, subscription.MaxKeepaliveCount);
            encoder.WriteUInt32(null, subscription.MaxMessageCount);
            encoder.WriteUInt32(null, subscription.MaxNotificationsPerPublish);
            encoder.WriteDouble(null, subscription.PublishingInterval);
            encoder.WriteByte(null, subscription.Priority);
            encoder.WriteInt32(null, subscription.LastSentMessage);
            encoder.WriteUInt32(null, subscription.SequenceNumber);

            UserIdentityToken sanitizedIdentityToken =
                SanitizeUserIdentityToken(subscription.UserIdentityToken);
            encoder.WriteExtensionObject(
                null,
                sanitizedIdentityToken != null
                    ? new ExtensionObject(sanitizedIdentityToken)
                    : ExtensionObject.Null);

            ExtensionObject[] sentMessages = subscription.SentMessages?
                .Select(message => new ExtensionObject(message))
                .ToArray() ?? [];
            encoder.WriteExtensionObjectArray(
                null,
                new ArrayOf<ExtensionObject>(sentMessages));

            List<StoredMonitoredItem> items = subscription.MonitoredItems?
                .Cast<StoredMonitoredItem>()
                .ToList() ?? [];
            encoder.WriteInt32(null, items.Count);
            foreach (StoredMonitoredItem item in items)
            {
                EncodeMonitoredItem(encoder, item);
            }
        }

        /// <summary>
        /// Encodes a monitored item's settings and its last sampled value.
        /// </summary>
        internal static void EncodeMonitoredItem(
            BinaryEncoder encoder,
            StoredMonitoredItem item)
        {
            encoder.WriteBoolean(null, item.IsRestored);
            encoder.WriteUInt32(null, item.SubscriptionId);
            encoder.WriteUInt32(null, item.Id);
            encoder.WriteInt32(null, item.TypeMask);
            encoder.WriteNodeId(null, item.NodeId);
            encoder.WriteUInt32(null, item.AttributeId);
            encoder.WriteString(null, item.IndexRange);
            encoder.WriteQualifiedName(null, item.Encoding);
            encoder.WriteEnumerated(null, item.DiagnosticsMasks);
            encoder.WriteEnumerated(null, item.TimestampsToReturn);
            encoder.WriteUInt32(null, item.ClientHandle);
            encoder.WriteEnumerated(null, item.MonitoringMode);
            encoder.WriteExtensionObject(
                null,
                item.OriginalFilter != null
                    ? new ExtensionObject(item.OriginalFilter)
                    : ExtensionObject.Null);
            encoder.WriteExtensionObject(
                null,
                item.FilterToUse != null
                    ? new ExtensionObject(item.FilterToUse)
                    : ExtensionObject.Null);
            encoder.WriteDouble(null, item.Range);
            encoder.WriteDouble(null, item.SamplingInterval);
            encoder.WriteUInt32(null, item.QueueSize);
            encoder.WriteBoolean(null, item.DiscardOldest);
            encoder.WriteInt32(null, item.SourceSamplingInterval);
            encoder.WriteBoolean(null, item.AlwaysReportUpdates);
            encoder.WriteBoolean(null, item.IsDurable);
            encoder.WriteDataValue(null, item.LastValue);
            encoder.WriteStatusCode(null, item.LastError?.StatusCode ?? StatusCodes.Good);
            encoder.WriteString(null, item.ParsedIndexRange.ToString());
        }

        /// <summary>
        /// Decodes a subscription written by <see cref="EncodeSubscription"/>.
        /// </summary>
        public static StoredSubscription DecodeSubscription(
            BinaryDecoder decoder,
            uint version = kStoreVersion)
        {
            ValidateStoreVersion(version);

            var subscription = new StoredSubscription
            {
                Id = decoder.ReadUInt32(null),
                IsDurable = decoder.ReadBoolean(null),
                LifetimeCounter = decoder.ReadUInt32(null),
                MaxLifetimeCount = decoder.ReadUInt32(null),
                MaxKeepaliveCount = decoder.ReadUInt32(null),
                MaxMessageCount = decoder.ReadUInt32(null),
                MaxNotificationsPerPublish = decoder.ReadUInt32(null),
                PublishingInterval = decoder.ReadDouble(null),
                Priority = decoder.ReadByte(null),
                LastSentMessage = decoder.ReadInt32(null),
                SequenceNumber = decoder.ReadUInt32(null)
            };

            ExtensionObject identityToken = decoder.ReadExtensionObject(null);
            if (!identityToken.IsNull &&
                identityToken.TryGetValue(out IEncodeable identityTokenBody))
            {
                subscription.UserIdentityToken = identityTokenBody as UserIdentityToken;
            }

            ArrayOf<ExtensionObject> sentMessages = decoder.ReadExtensionObjectArray(null);
            var messages = new List<NotificationMessage>();
            if (!sentMessages.IsNull)
            {
                foreach (ExtensionObject sentMessage in sentMessages.Memory.ToArray())
                {
                    if (!sentMessage.IsNull &&
                        sentMessage.TryGetValue(out IEncodeable messageBody) &&
                        messageBody is NotificationMessage message)
                    {
                        messages.Add(message);
                    }
                }
            }
            subscription.SentMessages = messages;

            int itemCount = decoder.ReadInt32(null);
            var items = new List<IUaStoredMonitoredItem>(itemCount);
            for (int ii = 0; ii < itemCount; ii++)
            {
                items.Add(DecodeMonitoredItem(decoder));
            }
            subscription.MonitoredItems = items;

            return subscription;
        }

        /// <summary>
        /// Decodes a monitored item written by <see cref="EncodeMonitoredItem"/>.
        /// </summary>
        internal static StoredMonitoredItem DecodeMonitoredItem(BinaryDecoder decoder)
        {
            var item = new StoredMonitoredItem
            {
                IsRestored = decoder.ReadBoolean(null),
                SubscriptionId = decoder.ReadUInt32(null),
                Id = decoder.ReadUInt32(null),
                TypeMask = decoder.ReadInt32(null),
                NodeId = decoder.ReadNodeId(null),
                AttributeId = decoder.ReadUInt32(null),
                IndexRange = decoder.ReadString(null),
                Encoding = decoder.ReadQualifiedName(null),
                DiagnosticsMasks = decoder.ReadEnumerated<DiagnosticsMasks>(null),
                TimestampsToReturn = decoder.ReadEnumerated<TimestampsToReturn>(null),
                ClientHandle = decoder.ReadUInt32(null),
                MonitoringMode = decoder.ReadEnumerated<MonitoringMode>(null)
            };

            ExtensionObject originalFilter = decoder.ReadExtensionObject(null);
            if (!originalFilter.IsNull &&
                originalFilter.TryGetValue(out IEncodeable originalFilterBody) &&
                originalFilterBody is MonitoringFilter parsedOriginalFilter)
            {
                item.OriginalFilter = parsedOriginalFilter;
            }

            ExtensionObject filterToUse = decoder.ReadExtensionObject(null);
            if (!filterToUse.IsNull &&
                filterToUse.TryGetValue(out IEncodeable filterToUseBody) &&
                filterToUseBody is MonitoringFilter parsedFilterToUse)
            {
                item.FilterToUse = parsedFilterToUse;
            }

            item.Range = decoder.ReadDouble(null);
            item.SamplingInterval = decoder.ReadDouble(null);
            item.QueueSize = decoder.ReadUInt32(null);
            item.DiscardOldest = decoder.ReadBoolean(null);
            item.SourceSamplingInterval = decoder.ReadInt32(null);
            item.AlwaysReportUpdates = decoder.ReadBoolean(null);
            item.IsDurable = decoder.ReadBoolean(null);
            item.LastValue = decoder.ReadDataValue(null);

            StatusCode lastError = decoder.ReadStatusCode(null);
            item.LastError = lastError == StatusCodes.Good ? null : new ServiceResult(lastError);

            string indexRange = decoder.ReadString(null);
            item.ParsedIndexRange = string.IsNullOrEmpty(indexRange)
                ? NumericRange.Null
                : NumericRange.Parse(indexRange);

            return item;
        }

        /// <summary>
        /// Copies an identity token without its secret.
        /// </summary>
        /// <remarks>
        /// A restored subscription is matched to the session that claims it;
        /// the secret is never needed to do that, so it is not written to disk.
        /// An issued token is nothing but a bearer credential, so a
        /// subscription owned by one is not persisted at all.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// The identity cannot be persisted without storing a credential.
        /// </exception>
        internal static UserIdentityToken SanitizeUserIdentityToken(
            UserIdentityToken identityToken)
        {
            return identityToken switch
            {
                null => null,
                AnonymousIdentityToken anonymous => new AnonymousIdentityToken
                {
                    PolicyId = anonymous.PolicyId
                },
                UserNameIdentityToken userName => new UserNameIdentityToken
                {
                    PolicyId = userName.PolicyId,
                    UserName = userName.UserName,
                    Password = default,
                    EncryptionAlgorithm = null
                },
                X509IdentityToken x509 => new X509IdentityToken
                {
                    PolicyId = x509.PolicyId,
                    CertificateData = x509.CertificateData
                },
                IssuedIdentityToken => throw new NotSupportedException(
                    "A durable subscription owned by an issued token identity cannot be " +
                    "persisted without storing the token itself."),
                _ => throw new NotSupportedException(
                    $"User identity token type '{identityToken.GetType().Name}' cannot be " +
                    "persisted with a durable subscription.")
            };
        }

        /// <exception cref="InvalidDataException">
        /// The store version is not one this build reads.
        /// </exception>
        private static void ValidateStoreVersion(uint version)
        {
            if (version != kStoreVersion)
            {
                throw new InvalidDataException(
                    $"Unsupported durable subscription store version {version}.");
            }
        }
    }
}
