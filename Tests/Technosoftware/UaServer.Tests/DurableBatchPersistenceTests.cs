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
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Opc.Ua;
using SampleCompany.NodeManagers.DurableSubscription;
using Technosoftware.Tests;
#endregion Using Directives

namespace Technosoftware.UaServer.Tests
{
    /// <summary>
    /// Verifies durable batches and queues reach the disk and come back unchanged.
    /// A failed store is only logged, so the queue tests alone cannot tell.
    /// </summary>
    [TestFixture]
    [Category("MonitoredItem")]
    [SetCulture("en-us")]
    [SetUICulture("en-us")]
    [NonParallelizable]
    public sealed class DurableBatchPersistenceTests
    {
        private const int kQueueItems = 3000;
        private static readonly DateTimeUtc s_sourceTimestamp =
            new(new DateTime(2026, 3, 14, 15, 9, 26, DateTimeKind.Utc));
        private static readonly DateTimeUtc s_serverTimestamp =
            new(new DateTime(2026, 3, 14, 15, 9, 27, DateTimeKind.Utc));

        private string m_baseDirectory;

        [SetUp]
        public void SetUp()
        {
            m_baseDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(m_baseDirectory);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(m_baseDirectory, true);
        }

        [Test]
        public void DataChangeBatchRoundTripKeepsValuesAndTimestamps()
        {
            var persistor = new BatchPersistor(NUnitTelemetryContext.Create());
            var values = new List<(DataValue, ServiceResult)>
            {
                (CreateValue(new Variant(42u), StatusCodes.Good), null),
                (CreateValue(new Variant("text"), StatusCodes.UncertainLastUsableValue), null),
                (CreateValue(new Variant(s_sourceTimestamp), StatusCodes.Good), null),
                (CreateValue(default, StatusCodes.BadNodeIdUnknown), new ServiceResult(StatusCodes.BadNodeIdUnknown))
            };
            var batch = new DataChangeBatch([.. values], 1000, 4711);

            persistor.PersistSynchronously(batch);

            Assert.That(batch.IsPersisted, Is.True, "The batch was not stored.");
            Assert.That(batch.Values, Is.Null);

            batch.RestoreInProgress = true;
            persistor.RestoreSynchronously(batch);

            Assert.That(batch.IsPersisted, Is.False);
            Assert.That(batch.RestoreInProgress, Is.False);
            Assert.That(batch.Values, Has.Count.EqualTo(values.Count));
            for (int i = 0; i < values.Count; i++)
            {
                AssertValue(batch.Values[i].Item1, values[i].Item1);
                Assert.That(
                    batch.Values[i].Item2?.StatusCode ?? StatusCodes.Good,
                    Is.EqualTo(values[i].Item2?.StatusCode ?? StatusCodes.Good));
            }
        }

        [Test]
        public void EventBatchRoundTripKeepsFields()
        {
            var persistor = new BatchPersistor(NUnitTelemetryContext.Create());
            var batch = new EventBatch(
                [
                    new EventFieldList
                    {
                        ClientHandle = 7,
                        EventFields = [new Variant(s_sourceTimestamp), new Variant("event")]
                    }
                ],
                1000,
                4712);

            persistor.PersistSynchronously(batch);

            Assert.That(batch.IsPersisted, Is.True, "The batch was not stored.");
            Assert.That(batch.Events, Is.Null);

            batch.RestoreInProgress = true;
            persistor.RestoreSynchronously(batch);

            Assert.That(batch.IsPersisted, Is.False);
            Assert.That(batch.Events, Has.Count.EqualTo(1));
            Assert.That(batch.Events[0].ClientHandle, Is.EqualTo(7u));
            Assert.That(batch.Events[0].EventFields.Count, Is.EqualTo(2));
            Assert.That(batch.Events[0].EventFields[0], Is.EqualTo(new Variant(s_sourceTimestamp)));
            Assert.That(batch.Events[0].EventFields[1], Is.EqualTo(new Variant("event")));
        }

        /// <summary>
        /// A restart restores the queue, including the batches already spilled to disk.
        /// </summary>
        [Test]
        public async Task DataChangeQueueSurvivesRestartAsync()
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();
            const uint id = 4713;
            using var factory = new DurableMonitoredItemQueueFactory(telemetry);
            IUaDataChangeMonitoredItemQueue queue = factory.CreateDataChangeQueue(true, id);
            queue.ResetQueue(kQueueItems, false);
            for (uint i = 0; i < kQueueItems; i++)
            {
                queue.Enqueue(CreateValue(new Variant(i), StatusCodes.Good), null);
            }

            // wait for persisting to take place
            await Task.Delay(1000).ConfigureAwait(false);

            factory.PersistQueues([id], m_baseDirectory);

            using var restartedFactory = new DurableMonitoredItemQueueFactory(telemetry);
            IUaDataChangeMonitoredItemQueue restored =
                restartedFactory.RestoreDataChangeQueue(id, m_baseDirectory);

            Assert.That(restored, Is.Not.Null, "The queue was not stored.");
            Assert.That(restored.ItemsInQueue, Is.EqualTo(kQueueItems));
            for (uint i = 0; i < kQueueItems; i++)
            {
                DataValue value = default;
                Assert.That(
                    SpinWait.SpinUntil(() => restored.Dequeue(out value, out _), 5000),
                    $"Dequeue failed for item {i}");
                AssertValue(value, CreateValue(new Variant(i), StatusCodes.Good));
            }
            Assert.That(restored.ItemsInQueue, Is.Zero);
        }

        /// <summary>
        /// A restart restores the queue, including the batches already spilled to disk.
        /// </summary>
        [Test]
        public async Task EventQueueSurvivesRestartAsync()
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();
            const uint id = 4714;
            using var factory = new DurableMonitoredItemQueueFactory(telemetry);
            IUaEventMonitoredItemQueue queue = factory.CreateEventQueue(true, id);
            queue.SetQueueSize(kQueueItems, false);
            for (uint i = 0; i < kQueueItems; i++)
            {
                queue.Enqueue(new EventFieldList { ClientHandle = i, EventFields = [new Variant(i)] });
            }

            // wait for persisting to take place
            await Task.Delay(1000).ConfigureAwait(false);

            factory.PersistQueues([id], m_baseDirectory);

            using var restartedFactory = new DurableMonitoredItemQueueFactory(telemetry);
            IUaEventMonitoredItemQueue restored = restartedFactory.RestoreEventQueue(id, m_baseDirectory);

            Assert.That(restored, Is.Not.Null, "The queue was not stored.");
            Assert.That(restored.ItemsInQueue, Is.EqualTo(kQueueItems));
            for (uint i = 0; i < kQueueItems; i++)
            {
                EventFieldList value = null;
                Assert.That(
                    SpinWait.SpinUntil(() => restored.Dequeue(out value), 5000),
                    $"Dequeue failed for item {i}");
                Assert.That(value.ClientHandle, Is.EqualTo(i));
            }
            Assert.That(restored.ItemsInQueue, Is.Zero);
        }

        private static DataValue CreateValue(Variant value, StatusCode statusCode)
        {
            return new DataValue(value, statusCode, s_sourceTimestamp, s_serverTimestamp, 100, 200);
        }

        private static void AssertValue(DataValue actual, DataValue expected)
        {
            Assert.That(actual.WrappedValue, Is.EqualTo(expected.WrappedValue));
            Assert.That(actual.StatusCode, Is.EqualTo(expected.StatusCode));
            Assert.That(actual.SourceTimestamp, Is.EqualTo(expected.SourceTimestamp));
            Assert.That(actual.ServerTimestamp, Is.EqualTo(expected.ServerTimestamp));
            Assert.That(actual.SourcePicoseconds, Is.EqualTo(expected.SourcePicoseconds));
            Assert.That(actual.ServerPicoseconds, Is.EqualTo(expected.ServerPicoseconds));
        }
    }
}
