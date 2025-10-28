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
using System.Collections.Generic;

using Opc.Ua;
#endregion

namespace Technosoftware.UaClient
{
    /// <summary>
    /// Saves the events received from the server.
    /// </summary>
    public class MonitoredItemEventCache
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Constructs a cache for a monitored item.
        /// </summary>
        public MonitoredItemEventCache(int queueSize)
        {
            m_queueSize = queueSize;
            m_events = new Queue<EventFieldList>();
        }
        #endregion

        #region Public Members
        /// <summary>
        /// The size of the queue to maintain.
        /// </summary>
        public int QueueSize => m_queueSize;

        /// <summary>
        /// The last event received.
        /// </summary>
        public EventFieldList LastEvent => m_lastEvent;

        /// <summary>
        /// Returns all events in the queue.
        /// </summary>
        public IList<EventFieldList> Publish()
        {
            EventFieldList[] events = new EventFieldList[m_events.Count];

            for (int ii = 0; ii < events.Length; ii++)
            {
                events[ii] = m_events.Dequeue();
            }

            return events;
        }

        /// <summary>
        /// Saves a notification in the cache.
        /// </summary>
        public void OnNotification(EventFieldList notification)
        {
            m_events.Enqueue(notification);
            m_lastEvent = notification;

            while (m_events.Count > m_queueSize)
            {
                m_events.Dequeue();
            }
        }

        /// <summary>
        /// Changes the queue size.
        /// </summary>
        public void SetQueueSize(int queueSize)
        {
            if (queueSize == m_queueSize)
            {
                return;
            }

            if (queueSize < 1)
            {
                queueSize = 1;
            }

            m_queueSize = queueSize;

            while (m_events.Count > m_queueSize)
            {
                m_events.Dequeue();
            }
        }
        #endregion

        #region Private Fields
        private int m_queueSize;
        private EventFieldList m_lastEvent;
        private readonly Queue<EventFieldList> m_events;
        #endregion
    }
}
