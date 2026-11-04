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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Technosoftware.UaServer;
#endregion Using Directives

namespace SampleCompany.NodeManagers.DurableSubscription
{
    /// <summary>
    /// A factory for <see cref="IUaDataChangeMonitoredItemQueue"> and </see> <see cref="IUaEventMonitoredItemQueue"/>
    /// </summary>
    public class DurableMonitoredItemQueueFactory : IUaMonitoredItemQueueFactory
    {
        private readonly BatchPersistor m_batchPersistor;
        private readonly ILogger m_logger;
        private readonly ITelemetryContext m_telemetry;
        private readonly IServiceMessageContext m_messageContext;

        private const string kQueueDirectory = "Queues";
        private const string kBase_filename = "_queue.bin";

        private ConcurrentDictionary<uint, DurableDataChangeMonitoredItemQueue> m_dataChangeQueues = new();
        private ConcurrentDictionary<uint, DurableEventMonitoredItemQueue> m_eventQueues = new();

        /// <inheritdoc/>
        public bool SupportsDurableQueues => true;

        public DurableMonitoredItemQueueFactory(
            ITelemetryContext telemetry,
            IServiceMessageContext messageContext = null)
        {
            m_telemetry = telemetry;
            m_messageContext = messageContext;
            m_logger = telemetry.CreateLogger<DurableDataChangeMonitoredItemQueue>();
            m_batchPersistor = new BatchPersistor(telemetry, messageContext);
        }

        /// <inheritdoc/>
        public IUaDataChangeMonitoredItemQueue CreateDataChangeQueue(
            bool isDurable,
            uint monitoredItemId)
        {
            //use durable queue only if MI is durable
            if (isDurable)
            {
                var queue = new DurableDataChangeMonitoredItemQueue(
                    isDurable,
                    monitoredItemId,
                    m_batchPersistor,
                    m_telemetry);
                queue.Disposed += DataChangeQueueDisposed;
                m_dataChangeQueues.AddOrUpdate(monitoredItemId, queue, (_, _) => queue);
                return queue;
            }

            return new DataChangeMonitoredItemQueue(isDurable, monitoredItemId, m_telemetry);
        }

        /// <inheritdoc/>
        public IUaEventMonitoredItemQueue CreateEventQueue(bool isDurable, uint monitoredItemId)
        {
            //use durable queue only if MI is durable
            if (isDurable)
            {
                var queue = new DurableEventMonitoredItemQueue(
                    isDurable,
                    monitoredItemId,
                    m_batchPersistor,
                    m_telemetry);
                queue.Disposed += EventQueueDisposed;
                m_eventQueues.AddOrUpdate(monitoredItemId, queue, (_, _) => queue);
                return queue;
            }

            return new EventMonitoredItemQueue(isDurable, monitoredItemId, m_telemetry);
        }

        private void DataChangeQueueDisposed(object sender, EventArgs eventArgs)
        {
            if (sender is DataChangeMonitoredItemQueue queue)
            {
                m_dataChangeQueues.TryRemove(queue.MonitoredItemId, out _);
            }
        }

        private void EventQueueDisposed(object sender, EventArgs eventArgs)
        {
            if (sender is EventMonitoredItemQueue queue)
            {
                m_eventQueues.TryRemove(queue.MonitoredItemId, out _);
            }
        }

        /// <summary>
        /// Persist the queues of the monitored items with the provided ids
        /// Deletes the batches of all queues that are not in the list
        /// </summary>
        /// <param name="ids">the MonitoredItem ids of the queues to store</param>
        public void PersistQueues(IEnumerable<uint> ids, string baseDirectory)
        {
            string targetPath = Path.Combine(baseDirectory, kQueueDirectory);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            using IDisposable scope = AmbientMessageContext.SetScopedContext(m_telemetry);
            IServiceMessageContext context = m_messageContext ?? AmbientMessageContext.CurrentContext;
            foreach (uint id in ids)
            {
                try
                {
                    if (m_dataChangeQueues.TryGetValue(
                        id,
                        out DurableDataChangeMonitoredItemQueue queue))
                    {
                        //store
                        using FileStream stream = File.Create(Path.Combine(targetPath, id + kBase_filename));
                        using var encoder = new BinaryEncoder(stream, context, true);
                        WriteMappingTables(encoder, context);
                        EncodeDataChangeQueue(encoder, queue.ToStorableQueue());
                        continue;
                    }

                    if (m_eventQueues.TryGetValue(
                        id,
                        out DurableEventMonitoredItemQueue eventQueue))
                    {
                        //store
                        using FileStream stream = File.Create(Path.Combine(targetPath, id + kBase_filename));
                        using var encoder = new BinaryEncoder(stream, context, true);
                        WriteMappingTables(encoder, context);
                        EncodeEventQueue(encoder, eventQueue.ToStorableQueue());
                        continue;
                    }
                    m_logger.LogWarning(
                        "Failed to persist queue for monitored item with id {MonitoredItemId} as the queue was not known to the server",
                        id);
                }
                catch (Exception ex)
                {
                    m_logger.LogWarning(
                        ex,
                        "Failed to persist queue for monitored item with id {MonitoredItemId}",
                        id);
                }
            }
            // Delete batches of all queues that are not in the list
            m_batchPersistor.DeleteBatches(ids);
        }

        /// <summary>
        /// Restore an Event queue
        /// </summary>
        public IUaEventMonitoredItemQueue RestoreEventQueue(uint id, string baseDirectory)
        {
            try
            {
                string targetFile = Path.Combine(
                    baseDirectory,
                    kQueueDirectory,
                    id + kBase_filename);
                if (!File.Exists(targetFile))
                {
                    return null;
                }
                using IDisposable scope = AmbientMessageContext.SetScopedContext(m_telemetry);
                IServiceMessageContext context = m_messageContext ?? AmbientMessageContext.CurrentContext;
                StorableEventQueue template;
                using (FileStream stream = File.OpenRead(targetFile))
                using (var decoder = new BinaryDecoder(stream, context, true))
                {
                    ReadMappingTables(decoder);
                    template = DecodeEventQueue(decoder);
                }
                File.Delete(targetFile);

                var queue = new DurableEventMonitoredItemQueue(template, m_batchPersistor);
                m_eventQueues.AddOrUpdate(id, queue, (_, _) => queue);

                return queue;
            }
            catch (Exception ex)
            {
                m_logger.LogWarning(ex, "Failed to restore event change queue");
            }
            return null;
        }

        /// <summary>
        /// Restore a DataChange queue
        /// </summary>
        public IUaDataChangeMonitoredItemQueue RestoreDataChangeQueue(uint id, string baseDirectory)
        {
            try
            {
                string targetFile = Path.Combine(
                    baseDirectory,
                    kQueueDirectory,
                    id + kBase_filename);
                if (!File.Exists(targetFile))
                {
                    return null;
                }
                using IDisposable scope = AmbientMessageContext.SetScopedContext(m_telemetry);
                IServiceMessageContext context = m_messageContext ?? AmbientMessageContext.CurrentContext;
                StorableDataChangeQueue template;
                using (FileStream stream = File.OpenRead(targetFile))
                using (var decoder = new BinaryDecoder(stream, context, true))
                {
                    ReadMappingTables(decoder);
                    template = DecodeDataChangeQueue(decoder);
                }
                File.Delete(targetFile);

                var queue = new DurableDataChangeMonitoredItemQueue(template, m_batchPersistor);
                m_dataChangeQueues.AddOrUpdate(id, queue, (_, _) => queue);

                return queue;
            }
            catch (Exception ex)
            {
                m_logger.LogWarning(ex, "Failed to restore data change queue");
            }
            return null;
        }

        /// <summary>
        /// Remove all stored queues and batches that are not in the list
        /// </summary>
        public void CleanStoredQueues(string baseDirectory, IEnumerable<uint> batchesToKeep)
        {
            try
            {
                string targetPath = Path.Combine(baseDirectory, kQueueDirectory);
                if (Directory.Exists(targetPath))
                {
                    Directory.Delete(targetPath, true);
                }
            }
            catch (Exception ex)
            {
                m_logger.LogWarning(ex, "Failed to clean stored queues");
            }

            m_batchPersistor.DeleteBatches(batchesToKeep);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// An overrideable version of the Dispose.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (DurableEventMonitoredItemQueue queue in m_eventQueues.Values)
                {
                    queue?.Dispose();
                }
                foreach (DurableDataChangeMonitoredItemQueue queue in m_dataChangeQueues.Values)
                {
                    queue?.Dispose();
                }
                m_dataChangeQueues = null;
                m_eventQueues = null;
            }
        }

        /// <summary>
        /// Writes the tables the indexes in the encoded values refer to
        /// </summary>
        internal static void WriteMappingTables(BinaryEncoder encoder, IServiceMessageContext context)
        {
            encoder.WriteStringArray(null, context.NamespaceUris.ToArrayOf());
            encoder.WriteStringArray(null, context.ServerUris.ToArrayOf());
        }

        /// <summary>
        /// Reads the tables written by <see cref="WriteMappingTables"/>
        /// </summary>
        internal static void ReadMappingTables(BinaryDecoder decoder)
        {
            ArrayOf<string> namespaceUris = decoder.ReadStringArray(null);
            ArrayOf<string> serverUris = decoder.ReadStringArray(null);
            decoder.SetMappingTables(
                new NamespaceTable(namespaceUris.Memory.ToArray()),
                new StringTable(serverUris.Memory.ToArray()));
        }

        private static void EncodeDataChangeQueue(BinaryEncoder encoder, StorableDataChangeQueue queue)
        {
            encoder.WriteBoolean(null, queue.IsDurable);
            encoder.WriteUInt32(null, queue.MonitoredItemId);
            encoder.WriteInt32(null, queue.ItemsInQueue);
            encoder.WriteUInt32(null, queue.QueueSize);
            EncodeDataChangeBatch(encoder, queue.EnqueueBatch);
            // a single batch is both the enqueue and the dequeue batch: store it once
            EncodeDataChangeBatch(
                encoder,
                ReferenceEquals(queue.DequeueBatch, queue.EnqueueBatch) ? null : queue.DequeueBatch);

            encoder.WriteInt32(null, queue.DataChangeBatches.Count);
            foreach (DataChangeBatch batch in queue.DataChangeBatches)
            {
                EncodeDataChangeBatch(encoder, batch);
            }
        }

        private static StorableDataChangeQueue DecodeDataChangeQueue(BinaryDecoder decoder)
        {
            var queue = new StorableDataChangeQueue
            {
                IsDurable = decoder.ReadBoolean(null),
                MonitoredItemId = decoder.ReadUInt32(null),
                ItemsInQueue = decoder.ReadInt32(null),
                QueueSize = decoder.ReadUInt32(null),
                EnqueueBatch = DecodeDataChangeBatch(decoder),
                DequeueBatch = DecodeDataChangeBatch(decoder)
            };

            int count = decoder.ReadInt32(null);
            queue.DataChangeBatches = new List<DataChangeBatch>(count);
            for (int i = 0; i < count; i++)
            {
                queue.DataChangeBatches.Add(DecodeDataChangeBatch(decoder));
            }
            return queue;
        }

        /// <summary>
        /// Encodes the batch. A batch already on disk keeps its Id, which names its file,
        /// and is restored from that file when it is needed again.
        /// </summary>
        internal static void EncodeDataChangeBatch(BinaryEncoder encoder, DataChangeBatch batch)
        {
            encoder.WriteBoolean(null, batch != null);
            if (batch == null)
            {
                return;
            }

            lock (batch)
            {
                encoder.WriteGuid(null, batch.Id);
                encoder.WriteUInt32(null, batch.BatchSize);
                encoder.WriteUInt32(null, batch.MonitoredItemId);
                encoder.WriteBoolean(null, batch.IsPersisted);

                int count = batch.Values?.Count ?? 0;
                encoder.WriteInt32(null, count);
                for (int i = 0; i < count; i++)
                {
                    (DataValue value, ServiceResult error) = batch.Values[i];
                    encoder.WriteDataValue(null, value);
                    encoder.WriteStatusCode(null, error?.StatusCode ?? StatusCodes.Good);
                }
            }
        }

        /// <summary>
        /// Decodes a batch written by <see cref="EncodeDataChangeBatch"/>
        /// </summary>
        internal static DataChangeBatch DecodeDataChangeBatch(BinaryDecoder decoder)
        {
            if (!decoder.ReadBoolean(null))
            {
                return null;
            }

            Guid id = decoder.ReadGuid(null);
            uint batchSize = decoder.ReadUInt32(null);
            uint monitoredItemId = decoder.ReadUInt32(null);
            bool isPersisted = decoder.ReadBoolean(null);

            int count = decoder.ReadInt32(null);
            var values = new List<(DataValue, ServiceResult)>(count);
            for (int i = 0; i < count; i++)
            {
                DataValue value = decoder.ReadDataValue(null);
                StatusCode error = decoder.ReadStatusCode(null);
                values.Add((value, error == StatusCodes.Good ? null : new ServiceResult(error)));
            }

            var batch = new DataChangeBatch(id, values, batchSize, monitoredItemId);
            if (isPersisted)
            {
                batch.SetPersisted();
            }
            return batch;
        }

        private static void EncodeEventQueue(BinaryEncoder encoder, StorableEventQueue queue)
        {
            encoder.WriteBoolean(null, queue.IsDurable);
            encoder.WriteUInt32(null, queue.MonitoredItemId);
            encoder.WriteInt32(null, queue.ItemsInQueue);
            encoder.WriteUInt32(null, queue.QueueSize);
            EncodeEventBatch(encoder, queue.EnqueueBatch);
            // a single batch is both the enqueue and the dequeue batch: store it once
            EncodeEventBatch(
                encoder,
                ReferenceEquals(queue.DequeueBatch, queue.EnqueueBatch) ? null : queue.DequeueBatch);

            encoder.WriteInt32(null, queue.EventBatches.Count);
            foreach (EventBatch batch in queue.EventBatches)
            {
                EncodeEventBatch(encoder, batch);
            }
        }

        private static StorableEventQueue DecodeEventQueue(BinaryDecoder decoder)
        {
            var queue = new StorableEventQueue
            {
                IsDurable = decoder.ReadBoolean(null),
                MonitoredItemId = decoder.ReadUInt32(null),
                ItemsInQueue = decoder.ReadInt32(null),
                QueueSize = decoder.ReadUInt32(null),
                EnqueueBatch = DecodeEventBatch(decoder),
                DequeueBatch = DecodeEventBatch(decoder)
            };

            int count = decoder.ReadInt32(null);
            queue.EventBatches = new List<EventBatch>(count);
            for (int i = 0; i < count; i++)
            {
                queue.EventBatches.Add(DecodeEventBatch(decoder));
            }
            return queue;
        }

        /// <summary>
        /// Encodes the batch. A batch already on disk keeps its Id, which names its file,
        /// and is restored from that file when it is needed again.
        /// </summary>
        internal static void EncodeEventBatch(BinaryEncoder encoder, EventBatch batch)
        {
            encoder.WriteBoolean(null, batch != null);
            if (batch == null)
            {
                return;
            }

            lock (batch)
            {
                encoder.WriteGuid(null, batch.Id);
                encoder.WriteUInt32(null, batch.BatchSize);
                encoder.WriteUInt32(null, batch.MonitoredItemId);
                encoder.WriteBoolean(null, batch.IsPersisted);

                int count = batch.Events?.Count ?? 0;
                encoder.WriteInt32(null, count);
                for (int i = 0; i < count; i++)
                {
                    encoder.WriteUInt32(null, batch.Events[i].ClientHandle);
                    encoder.WriteVariantArray(null, batch.Events[i].EventFields);
                }
            }
        }

        /// <summary>
        /// Decodes a batch written by <see cref="EncodeEventBatch"/>
        /// </summary>
        internal static EventBatch DecodeEventBatch(BinaryDecoder decoder)
        {
            if (!decoder.ReadBoolean(null))
            {
                return null;
            }

            Guid id = decoder.ReadGuid(null);
            uint batchSize = decoder.ReadUInt32(null);
            uint monitoredItemId = decoder.ReadUInt32(null);
            bool isPersisted = decoder.ReadBoolean(null);

            int count = decoder.ReadInt32(null);
            var events = new List<EventFieldList>(count);
            for (int i = 0; i < count; i++)
            {
                events.Add(new EventFieldList
                {
                    ClientHandle = decoder.ReadUInt32(null),
                    EventFields = decoder.ReadVariantArray(null)
                });
            }

            var batch = new EventBatch(id, events, batchSize, monitoredItemId);
            if (isPersisted)
            {
                batch.SetPersisted();
            }
            return batch;
        }
    }
}
