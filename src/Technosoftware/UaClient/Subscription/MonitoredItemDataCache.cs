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

namespace Technosoftware.UaClient
{
    /// <summary>
    /// A client cache which can hold the last monitored items in a queue.
    /// By default (1) only the last value is cached.
    /// </summary>
    public class MonitoredItemDataCache
    {
        #region Constants
        private const int kDefaultMaxCapacity = 100;
        #endregion

        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Constructs a cache for a monitored item.
        /// </summary>
        public MonitoredItemDataCache(int queueSize = 1)
        {
            m_queueSize = queueSize;
            if (queueSize > 1)
            {
                m_values = new Queue<DataValue>(Math.Min(queueSize + 1, kDefaultMaxCapacity));
            }
            else
            {
                m_queueSize = 1;
            }
        }
        #endregion

        #region Public Members
        /// <summary>
        /// The size of the queue to maintain.
        /// </summary>
        public int QueueSize => m_queueSize;

        /// <summary>
        /// The last value received from the server.
        /// </summary>
        public DataValue LastValue => m_lastValue;

        /// <summary>
        /// Returns all values in the queue.
        /// </summary>
        public IList<DataValue> Publish()
        {
            DataValue[] values;
            if (m_values != null)
            {
                values = new DataValue[m_values.Count];
                for (int ii = 0; ii < values.Length; ii++)
                {
                    values[ii] = m_values.Dequeue();
                }
            }
            else
            {
                values = new DataValue[1];
                values[0] = m_lastValue;
            }
            return values;
        }

        /// <summary>
        /// Saves a notification in the cache.
        /// </summary>
        public void OnNotification(MonitoredItemNotification notification)
        {
            m_lastValue = notification.Value;
            UaClientUtils.EventLog.NotificationValue(notification.ClientHandle, m_lastValue.WrappedValue);

            if (m_values != null)
            {
                m_values.Enqueue(notification.Value);
                while (m_values.Count > m_queueSize)
                {
                    m_values.Dequeue();
                }
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

            if (queueSize <= 1)
            {
                queueSize = 1;
                m_values = null;
            }
            else if (m_values == null)
            {
                m_values = new Queue<DataValue>(Math.Min(queueSize + 1, kDefaultMaxCapacity));
            }

            m_queueSize = queueSize;

            while (m_values.Count > m_queueSize)
            {
                m_values.Dequeue();
            }
        }
        #endregion

        #region Private Fields
        private int m_queueSize;
        private DataValue m_lastValue;
        private Queue<DataValue> m_values;
        #endregion
    }
}
