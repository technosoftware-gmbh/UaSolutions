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
using Moq;
using NUnit.Framework;
using Technosoftware.Tests;
using Opc.Ua;
using Technosoftware.UaServer;
#endregion Using Directives

namespace Technosoftware.UaServer.Tests
{
    /// <summary>
    /// Queue size revision of event monitored items in <see cref="EventManager"/>.
    /// </summary>
    /// <remarks>
    /// Part 4 §7.21: for event monitored items a requested queueSize of 0 means
    /// the server default and 1 the minimum queue size the server requires for
    /// Event Notifications. Neither may be taken literally, otherwise a burst of
    /// events (for example the OpenSecureChannel, CreateSession and
    /// ActivateSession audit events of one connect) collapses to the last event
    /// before the next publish.
    /// </remarks>
    [TestFixture]
    [Category("EventManager")]
    [SetCulture("en-us")]
    [SetUICulture("en-us")]
    [Parallelizable]
    public class EventManagerTests
    {
        [TestCase(0u)]
        [TestCase(1u)]
        public void CreateMonitoredItemRevisesSpecialEventQueueSizes(uint requestedQueueSize)
        {
            EventManager manager = CreateManager(10000, 10000, out IUaNodeManager nodeManager);

            IUaEventMonitoredItem item = CreateItem(manager, nodeManager, requestedQueueSize);

            Assert.That(((UaMonitoredItem)item).QueueSize, Is.EqualTo(EventManager.DefaultEventQueueSize));
        }

        /// <summary>
        /// The server default is still limited by the configured maximum, and a
        /// configured maximum of 1 is honored rather than read as another
        /// queueSize 1 request.
        /// </summary>
        [TestCase(0u, 3u)]
        [TestCase(1u, 3u)]
        [TestCase(0u, 1u)]
        [TestCase(1u, 1u)]
        public void CreateMonitoredItemClampsDefaultEventQueueSizeToMax(
            uint requestedQueueSize,
            uint maxQueueSize)
        {
            EventManager manager = CreateManager(maxQueueSize, 10000, out IUaNodeManager nodeManager);

            IUaEventMonitoredItem item = CreateItem(manager, nodeManager, requestedQueueSize);

            Assert.That(((UaMonitoredItem)item).QueueSize, Is.EqualTo(maxQueueSize));
        }

        [Test]
        public void CreateMonitoredItemKeepsOtherEventQueueSizes()
        {
            EventManager manager = CreateManager(10000, 10000, out IUaNodeManager nodeManager);

            IUaEventMonitoredItem item = CreateItem(manager, nodeManager, 5);

            Assert.That(((UaMonitoredItem)item).QueueSize, Is.EqualTo(5));
        }

        [Test]
        public void ModifyMonitoredItemRevisesEventQueueSizeOne()
        {
            EventManager manager = CreateManager(10000, 10000, out IUaNodeManager nodeManager);
            IUaEventMonitoredItem item = CreateItem(manager, nodeManager, 5);

            manager.ModifyMonitoredItem(
                NewContext(),
                item,
                TimestampsToReturn.Both,
                new MonitoredItemModifyRequest
                {
                    RequestedParameters = new MonitoringParameters
                    {
                        ClientHandle = 42,
                        SamplingInterval = 1000.0,
                        QueueSize = 1,
                        DiscardOldest = true
                    }
                },
                new EventFilter());

            Assert.That(((UaMonitoredItem)item).QueueSize, Is.EqualTo(EventManager.DefaultEventQueueSize));
        }

        /// <summary>
        /// An item persisted with a literal queue size of 1 (before this revision) is
        /// restored with the revised size, and so is its persisted queue: otherwise the
        /// item reports 1000 while the queue still keeps one event.
        /// </summary>
        [Test]
        public void RestoreMonitoredItemResizesPersistedEventQueueSizeOne()
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();
            var queueFactory = new MonitoredItemQueueFactory(telemetry);
            IUaEventMonitoredItemQueue persistedQueue = queueFactory.CreateEventQueue(false, 7);
            persistedQueue.SetQueueSize(1, true);
            var store = new Mock<IUaSubscriptionStore>();
            store.Setup(s => s.RestoreEventMonitoredItemQueue(7)).Returns(persistedQueue);
            EventManager manager = CreateManager(10000, 10000, out IUaNodeManager nodeManager, store.Object);

            IUaEventMonitoredItem item = manager.RestoreMonitoredItem(
                nodeManager,
                null,
                new StoredMonitoredItem
                {
                    SubscriptionId = 1,
                    Id = 7,
                    TypeMask = MonitoredItemTypeMask.Events,
                    NodeId = ObjectIds.Server,
                    AttributeId = Attributes.EventNotifier,
                    TimestampsToReturn = TimestampsToReturn.Both,
                    ClientHandle = 42,
                    MonitoringMode = MonitoringMode.Reporting,
                    OriginalFilter = new EventFilter(),
                    FilterToUse = new EventFilter(),
                    SamplingInterval = 1000.0,
                    QueueSize = 1,
                    DiscardOldest = true
                });

            Assert.That(((UaMonitoredItem)item).QueueSize, Is.EqualTo(EventManager.DefaultEventQueueSize));
            Assert.That(persistedQueue.QueueSize, Is.EqualTo(EventManager.DefaultEventQueueSize));

            // a burst, e.g. the audit events of one connect, is kept until the next publish
            for (int i = 0; i < 3; i++)
            {
                ((UaMonitoredItem)item).QueueEvent(new EventFieldList { ClientHandle = 42 });
            }
            Assert.That(((UaMonitoredItem)item).ItemsInQueue, Is.EqualTo(3));
        }

        private static EventManager CreateManager(
            uint maxQueueSize,
            uint maxDurableQueueSize,
            out IUaNodeManager nodeManager,
            IUaSubscriptionStore subscriptionStore = null)
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();
            var serverMock = new Mock<IUaServerData>();
            serverMock.Setup(s => s.Telemetry).Returns(telemetry);
            serverMock.Setup(s => s.NamespaceUris).Returns(new NamespaceTable());
            serverMock.Setup(s => s.TypeTree).Returns(new TypeTable(new NamespaceTable()));
            serverMock.Setup(s => s.MonitoredItemQueueFactory)
                .Returns(new MonitoredItemQueueFactory(telemetry));
            serverMock.Setup(s => s.SubscriptionStore).Returns(subscriptionStore);
            nodeManager = new Mock<IUaNodeManager>().Object;
            return new EventManager(serverMock.Object, maxQueueSize, maxDurableQueueSize);
        }

        private static IUaEventMonitoredItem CreateItem(
            EventManager manager,
            IUaNodeManager nodeManager,
            uint queueSize)
        {
            return manager.CreateMonitoredItem(
                NewContext(),
                nodeManager,
                null,
                1,
                new MonitoredItemIdFactory(),
                TimestampsToReturn.Both,
                1000.0,
                new MonitoredItemCreateRequest
                {
                    ItemToMonitor = new ReadValueId(),
                    MonitoringMode = MonitoringMode.Reporting,
                    RequestedParameters = new MonitoringParameters
                    {
                        ClientHandle = 42,
                        SamplingInterval = 1000.0,
                        QueueSize = queueSize,
                        DiscardOldest = true
                    }
                },
                new EventFilter(),
                false);
        }

        private static UaServerOperationContext NewContext()
        {
            return new UaServerOperationContext(new RequestHeader(), null, RequestType.CreateMonitoredItems);
        }
    }
}
