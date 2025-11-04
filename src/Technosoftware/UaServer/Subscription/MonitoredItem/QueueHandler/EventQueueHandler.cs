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
#endregion

namespace Technosoftware.UaServer.Subscriptions
{
    /// <summary>
    /// Mangages an event queue for usage by a MonitoredItem
    /// </summary>
    public class EventQueueHandler : IUaEventQueueHandler
    {
        /// <summary>
        /// Creates a new Queue
        /// </summary>
        /// <param name="createDurable">create a durable queue</param>
        /// <param name="queueFactory">the factory for creating the the factory for <see cref="IUaEventMonitoredItemQueue"/></param>
        /// <param name="monitoredItemId">the id of the monitoredItem associated with the queue</param>
        public EventQueueHandler(bool createDurable, IUaMonitoredItemQueueFactory queueFactory, uint monitoredItemId)
        {
            m_eventQueue = queueFactory.CreateEventQueue(createDurable, monitoredItemId);
            m_discardOldest = false;
            m_overflow = false;
        }

        /// <summary>
        /// Create an EventQueueHandler from an existing queue
        /// Used for restore after a server restart
        /// </summary>
        public EventQueueHandler(
            IUaEventMonitoredItemQueue eventQueue,
            bool discardOldest)
        {
            m_eventQueue = eventQueue;
            m_discardOldest = discardOldest;
            m_overflow = false;
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
        public bool Overflow => m_overflow;

        /// <summary>
        /// Checks the last 1k queue entries if the event is already in there
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool IsEventContainedInQueue(IFilterTarget instance)
        {
            return m_eventQueue.IsEventContainedInQueue(instance);
        }

        /// <summary>
        /// true if queue is already full and discarding is not allowed
        /// </summary>
        /// <returns></returns>
        public bool SetQueueOverflowIfFull()
        {
            if (m_eventQueue.ItemsInQueue >= m_eventQueue.QueueSize)
            {
                if (!m_discardOldest)
                {
                    m_overflow = true;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Dispose the queue
        /// </summary>
        public void Dispose()
        {
            Utils.SilentDispose(m_eventQueue);
        }

        /// <summary>
        /// Adds an event to the queue.
        /// </summary>
        public virtual void QueueEvent(EventFieldList fields)
        {
            // make space in the queue.
            if (m_eventQueue.ItemsInQueue >= m_eventQueue.QueueSize)
            {
                m_overflow = true;
                if (!m_discardOldest)
                {
                    throw new InvalidOperationException("Queue is full and no discarding of old values is allowed");
                }
                m_eventQueue.Dequeue(out _);
            }
            // queue the event.
            m_eventQueue.Enqueue(fields);
        }

        /// <summary>
        /// Publish Events
        /// </summary>
        /// <param name="context"></param>
        /// <param name="notifications"></param>
        /// <param name="maxNotificationsPerPublish">the maximum number of notifications to enqueue per call</param>
        public uint Publish(UaServerOperationContext context, Queue<EventFieldList> notifications, uint maxNotificationsPerPublish)
        {
            uint notificationCount = 0;
            while (notificationCount < maxNotificationsPerPublish && m_eventQueue.Dequeue(out EventFieldList fields))
            {
                foreach (Variant field in fields.EventFields)
                {
                    if (field.Value is StatusResult statusResult)
                    {
                        statusResult.ApplyDiagnosticMasks(context.DiagnosticsMask, context.StringTable);
                    }
                }

                notifications.Enqueue(fields);
                notificationCount++;
            }
            //if overflow event is placed at the end of the queue only set overflow to false if the overflow event still fits into the publish
            m_overflow = m_overflow && notificationCount == maxNotificationsPerPublish && !m_discardOldest;

            return notificationCount;
        }

        private bool m_overflow;
        private bool m_discardOldest;
        private readonly IUaEventMonitoredItemQueue m_eventQueue;
    }
}
