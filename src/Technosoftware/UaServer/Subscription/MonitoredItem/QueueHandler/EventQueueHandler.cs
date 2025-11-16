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
    /// Mangages an event queue for usage by a UaMonitoredItem
    /// </summary>
    public class EventQueueHandler : IUaEventQueueHandler
    {
        /// <summary>
        /// Creates a new Queue
        /// </summary>
        /// <param name="createDurable">create a durable queue</param>
        /// <param name="queueFactory">the factory for creating the factory for <see cref="IUaEventMonitoredItemQueue"/></param>
        /// <param name="monitoredItemId">the id of the monitoredItem associated with the queue</param>
        public EventQueueHandler(
            bool createDurable,
            IUaMonitoredItemQueueFactory queueFactory,
            uint monitoredItemId)
        {
            m_eventQueue = queueFactory.CreateEventQueue(createDurable, monitoredItemId);
            m_discardOldest = false;
            Overflow = false;
        }

        /// <summary>
        /// Create an EventQueueHandler from an existing queue
        /// Used for restore after a server restart
        /// </summary>
        public EventQueueHandler(IUaEventMonitoredItemQueue eventQueue, bool discardOldest)
        {
            m_eventQueue = eventQueue;
            m_discardOldest = discardOldest;
            Overflow = false;
        }

        /// <summary>
        /// Sets the queue size.
        /// </summary>
        /// <param name="queueSize">The new queue size.</param>
        /// <param name="discardOldest">Whether to discard the oldest values if the queue overflows.</param>
        public void SetQueueSize(uint queueSize, bool discardOldest)
        {
            m_discardOldest = discardOldest;
            m_eventQueue.SetQueueSize(queueSize, discardOldest);
        }

        /// <summary>
        /// The number of Items in the queue
        /// </summary>
        public int ItemsInQueue => m_eventQueue.ItemsInQueue;

        /// <summary>
        /// True if the queue is overflowing
        /// </summary>
        public bool Overflow { get; private set; }

        /// <summary>
        /// Checks the last 1k queue entries if the event is already in there
        /// </summary>
        public bool IsEventContainedInQueue(IFilterTarget instance)
        {
            return m_eventQueue.IsEventContainedInQueue(instance);
        }

        /// <summary>
        /// true if queue is already full and discarding is not allowed
        /// </summary>
        public bool SetQueueOverflowIfFull()
        {
            if (m_eventQueue.ItemsInQueue >= m_eventQueue.QueueSize && !m_discardOldest)
            {
                Overflow = true;
                return true;
            }
            return false;
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
                Utils.SilentDispose(m_eventQueue);
            }
        }

        /// <summary>
        /// Adds an event to the queue.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual void QueueEvent(EventFieldList fields)
        {
            // make space in the queue.
            if (m_eventQueue.ItemsInQueue >= m_eventQueue.QueueSize)
            {
                Overflow = true;
                if (!m_discardOldest)
                {
                    throw new InvalidOperationException(
                        "Queue is full and no discarding of old values is allowed");
                }
                m_eventQueue.Dequeue(out _);
            }
            // queue the event.
            m_eventQueue.Enqueue(fields);
        }

        /// <summary>
        /// Publish Events
        /// </summary>
        /// <param name="context">System context</param>
        /// <param name="notifications">Notifications</param>
        /// <param name="maxNotificationsPerPublish">the maximum number of notifications to enqueue per call</param>
        public uint Publish(
            UaServerOperationContext context,
            Queue<EventFieldList> notifications,
            uint maxNotificationsPerPublish)
        {
            uint notificationCount = 0;
            while (notificationCount < maxNotificationsPerPublish &&
                m_eventQueue.Dequeue(out EventFieldList fields))
            {
                foreach (Variant field in fields.EventFields)
                {
                    if (field.Value is StatusResult statusResult)
                    {
                        statusResult.ApplyDiagnosticMasks(
                            context.DiagnosticsMask,
                            context.StringTable);
                    }
                }

                notifications.Enqueue(fields);
                notificationCount++;
            }
            // if overflow event is placed at the end of the queue only set overflow to false if the overflow event
            // still fits into the publish
            Overflow = Overflow &&
                notificationCount == maxNotificationsPerPublish &&
                !m_discardOldest;

            return notificationCount;
        }

        private bool m_discardOldest;
        private readonly IUaEventMonitoredItemQueue m_eventQueue;
    }
}
