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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System;
using Moq;
using NUnit.Framework;
using Technosoftware.Tests;
using Opc.Ua;
using Technosoftware.UaServer;
#endregion Using Directives

namespace Technosoftware.UaServer.Tests
{
    [TestFixture]
    [Category("Subscription")]
    [NonParallelizable]
    public sealed class SessionPublishQueueRaceTests
    {
        [Test]
        [CancelAfter(10000)]
        public async Task PublishReadyTransitionCompletesQueuedRequestAsync()
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();
            var subscriptionManager = new Mock<IUaSubscriptionManager>();
            var server = new Mock<IUaServerData>();
            server.Setup(s => s.Telemetry).Returns(telemetry);
            server.Setup(s => s.SubscriptionManager).Returns(subscriptionManager.Object);

            var session = new Mock<IUaSession>();
            session.Setup(s => s.Id).Returns(new NodeId(Guid.NewGuid()));
            session.Setup(s => s.IsSecureChannelValid(It.IsAny<string>())).Returns(true);

            using var queue = new SessionPublishQueue(server.Object, session.Object, 10);
            var subscription = new Mock<IUaSubscription>();
            subscription.Setup(s => s.Id).Returns(1);
            queue.Add(subscription.Object);

            (Lock queueLock, Lock splitPublishLock) = GetPrivateLocks(queue);
            using var publishStarted = new ManualResetEventSlim();
            Task<IUaSubscription> publishTask;

            // The legacy split lock lets PublishAsync finish its ready check
            // before it blocks on the request-queue lock.
            queueLock.Enter();
            try
            {
                splitPublishLock?.Enter();
                try
                {
                    publishTask = Task.Run(() =>
                    {
                        publishStarted.Set();
                        return queue.PublishAsync(
                            "channel1",
                            DateTime.MaxValue,
                            false,
                            CancellationToken.None);
                    });
                    Assert.That(
                        publishStarted.Wait(TimeSpan.FromSeconds(2)),
                        Is.True,
                        "The Publish request did not start.");
                }
                finally
                {
                    splitPublishLock?.Exit();
                }

                if (splitPublishLock != null)
                {
                    WaitForSplitLockTransition(splitPublishLock);
                }

                // System.Threading.Lock is reentrant. Keeping the queue lock
                // held preserves the historical check-before-enqueue window.
                queue.Requeue(subscription.Object);
            }
            finally
            {
                queueLock.Exit();
            }

            Task completed = await Task.WhenAny(
                publishTask,
                Task.Delay(TimeSpan.FromSeconds(2))).ConfigureAwait(false);

            Assert.That(
                completed,
                Is.SameAs(publishTask),
                "The ready Subscription and queued Publish request lost their wake-up.");
            Assert.That(
                await publishTask.ConfigureAwait(false),
                Is.SameAs(subscription.Object));
        }

        [Test]
        public void PublishWithMaximumTimeoutQueuesRequest()
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();
            var subscriptionManager = new Mock<IUaSubscriptionManager>();
            var server = new Mock<IUaServerData>();
            server.Setup(s => s.Telemetry).Returns(telemetry);
            server.Setup(s => s.SubscriptionManager).Returns(subscriptionManager.Object);

            var session = new Mock<IUaSession>();
            session.Setup(s => s.Id).Returns(new NodeId(Guid.NewGuid()));
            session.Setup(s => s.IsSecureChannelValid(It.IsAny<string>())).Returns(true);

            using var queue = new SessionPublishQueue(server.Object, session.Object, 10);
            var subscription = new Mock<IUaSubscription>();
            subscription.Setup(s => s.Id).Returns(1);
            queue.Add(subscription.Object);

            Task<IUaSubscription> publishTask = queue.PublishAsync(
                "channel1",
                DateTime.UtcNow.AddMilliseconds(uint.MaxValue),
                false,
                CancellationToken.None);

            Assert.That(publishTask.IsCompleted, Is.False);
        }

        [Test]
        [CancelAfter(10000)]
        public async Task PublishTimerPreservesReadySubscriptionTimestampOrderAsync()
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();
            var subscriptionManager = new Mock<IUaSubscriptionManager>();
            var server = new Mock<IUaServerData>();
            server.Setup(s => s.Telemetry).Returns(telemetry);
            server.Setup(s => s.SubscriptionManager).Returns(subscriptionManager.Object);

            var session = new Mock<IUaSession>();
            session.Setup(s => s.Id).Returns(new NodeId(Guid.NewGuid()));
            session.Setup(s => s.IsSecureChannelValid(It.IsAny<string>())).Returns(true);

            using var queue = new SessionPublishQueue(server.Object, session.Object, 10);
            PublishingState publishingState = PublishingState.Idle;
            var timerOrder = new List<IUaSubscription>();
            var subscription1 = new Mock<IUaSubscription>();
            subscription1.Setup(s => s.Id).Returns(1);
            subscription1.Setup(s => s.Priority).Returns(1);
            subscription1
                .Setup(s => s.PublishTimerExpired())
                .Callback(() => timerOrder.Add(subscription1.Object))
                .Returns(() => publishingState);
            queue.Add(subscription1.Object);

            var subscription2 = new Mock<IUaSubscription>();
            subscription2.Setup(s => s.Id).Returns(2);
            subscription2.Setup(s => s.Priority).Returns(1);
            subscription2
                .Setup(s => s.PublishTimerExpired())
                .Callback(() => timerOrder.Add(subscription2.Object))
                .Returns(() => publishingState);
            queue.Add(subscription2.Object);

            queue.PublishTimerExpired();
            Assert.That(timerOrder, Has.Count.EqualTo(2));

            // Explicitly designate subscription1 as older and subscription2 as newer,
            // independent of the queue's internal timer traversal order.
            IUaSubscription olderSubscription = subscription1.Object;
            IUaSubscription newerSubscription = subscription2.Object;
            queue.PublishCompleted(olderSubscription, false);
            DateTime timestampBoundary = DateTime.UtcNow;
            Assert.That(
                SpinWait.SpinUntil(
                    () => DateTime.UtcNow > timestampBoundary,
                    TimeSpan.FromSeconds(1)),
                Is.True,
                "The clock did not advance while preparing distinct subscription timestamps.");
            queue.PublishCompleted(newerSubscription, false);

            queue.Requeue(newerSubscription);
            queue.Requeue(olderSubscription);
            publishingState = PublishingState.NotificationsAvailable;
            timerOrder.Clear();
            queue.PublishTimerExpired();

            // Both subscriptions must have been triggered by the timer.
            Assert.That(timerOrder, Has.Count.EqualTo(2));
            IUaSubscription result = await queue.PublishAsync(
                "channel1",
                DateTime.MaxValue,
                false,
                CancellationToken.None).ConfigureAwait(false);
            Assert.That(result, Is.SameAs(olderSubscription));
        }

        private static (Lock QueueLock, Lock SplitPublishLock) GetPrivateLocks(
            SessionPublishQueue queue)
        {
            FieldInfo[] lockFields = [.. typeof(SessionPublishQueue)
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(field => field.FieldType == typeof(Lock))];
            if (lockFields.Length is 0 or > 2)
            {
                throw new InvalidOperationException(
                    $"Expected one or two private Lock fields, found: " +
                    $"{string.Join(", ", lockFields.Select(field => field.Name))}.");
            }

            FieldInfo queueLockField = lockFields.FirstOrDefault(field => field.Name == "m_lock") ??
                (lockFields.Length == 1
                    ? lockFields[0]
                    : throw new InvalidOperationException(
                        $"Could not identify the queue lock from: " +
                        $"{string.Join(", ", lockFields.Select(field => field.Name))}."));
            var queueLock = (Lock)queueLockField.GetValue(queue);
            Lock splitPublishLock = lockFields
                .Where(field => !ReferenceEquals(field, queueLockField))
                .Select(field => (Lock)field.GetValue(queue))
                .SingleOrDefault();
            return (queueLock, splitPublishLock);
        }

        private static void WaitForSplitLockTransition(Lock splitPublishLock)
        {
            bool enteredByPublisher = SpinWait.SpinUntil(
                () =>
                {
                    if (!splitPublishLock.TryEnter())
                    {
                        return true;
                    }
                    splitPublishLock.Exit();
                    return false;
                },
                TimeSpan.FromSeconds(2));
            Assert.That(
                enteredByPublisher,
                Is.True,
                "PublishAsync did not enter the legacy split lock.");

            bool releasedByPublisher = SpinWait.SpinUntil(
                () =>
                {
                    if (!splitPublishLock.TryEnter())
                    {
                        return false;
                    }
                    splitPublishLock.Exit();
                    return true;
                },
                TimeSpan.FromSeconds(2));
            Assert.That(
                releasedByPublisher,
                Is.True,
                "PublishAsync did not leave the legacy split lock.");
        }
    }
}
