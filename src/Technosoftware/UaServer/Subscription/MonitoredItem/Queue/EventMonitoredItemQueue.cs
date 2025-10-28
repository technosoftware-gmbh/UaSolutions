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
using System.Linq;

using Opc.Ua;
#endregion

namespace Technosoftware.UaServer.Subscriptions
{
    /// <summary>
    /// Provides a queue for events.
    /// </summary>
    public class EventMonitoredItemQueue : IUaEventMonitoredItemQueue
    {
        private const UInt32 kMaxNoOfEntriesCheckedForDuplicateEvents = 1000;

        /// <summary>
        /// Creates an empty queue.
        /// </summary>
        public EventMonitoredItemQueue(bool createDurable, uint monitoredItemId)
        {
            if (createDurable)
            {
                Utils.LogError("EventMonitoredItemQueue does not support durable queues, please provide full implementation of IDurableMonitoredItemQueue using Server.CreateDurableMonitoredItemQueueFactory to supply own factory");
                throw new ArgumentException("DataChangeMonitoredItemQueue does not support durable Queues", nameof(createDurable));
            }
            m_events = new List<EventFieldList>();
            m_monitoredItemId = monitoredItemId;
            QueueSize = 0;
        }

        #region Public Methods
        /// <inheritdoc/>
        public uint MonitoredItemId => m_monitoredItemId;

        /// <inheritdoc/>
        public virtual bool IsDurable => false;

        /// <inheritdoc/>
        public uint QueueSize { get; protected set; }

        /// <inheritdoc/>
        public int ItemsInQueue => m_events.Count;

        /// <inheritdoc/>
        public bool Dequeue(out EventFieldList value)
        {
            value = null;
            if (m_events.Any())
            {
                value = m_events.First();
                m_events.RemoveAt(0);
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            //Only needed for unmanaged resources
        }

        /// <inheritdoc/>
        public void Enqueue(EventFieldList value)
        {
            if (QueueSize == 0)
            {
                throw new ServiceResultException(StatusCodes.BadInternalError, "Error queueing Event. Queue size is set to 0");
            }

            //Discard oldest
            if (m_events.Count == QueueSize)
            {
                Dequeue(out var _);
            }

            m_events.Add(value);
        }

        /// <inheritdoc/>
        public bool IsEventContainedInQueue(IFilterTarget instance)
        {
            int maxCount = m_events.Count > kMaxNoOfEntriesCheckedForDuplicateEvents ? (int)kMaxNoOfEntriesCheckedForDuplicateEvents : m_events.Count;

            for (int i = 0; i < maxCount; i++)
            {
                if (m_events[i] is EventFieldList processedEvent)
                {
                    if (ReferenceEquals(instance, processedEvent.Handle))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <inheritdoc/>
        public void SetQueueSize(uint queueSize, bool discardOldest)
        {
            QueueSize = queueSize;

            if (m_events.Count > QueueSize)
            {
                if (discardOldest)
                {
                    m_events.RemoveRange(0, m_events.Count - (int)queueSize);
                }
                else
                {
                    m_events.RemoveRange((int)queueSize, m_events.Count - (int)queueSize);
                }
            }
        }
        #endregion

        #region Private Fields
        /// <summary>
        /// the contained in the queue
        /// </summary>
        protected List<EventFieldList> m_events;
        private readonly uint m_monitoredItemId;
        #endregion
    }

}
