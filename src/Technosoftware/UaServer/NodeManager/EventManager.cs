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
using System.Text;
using System.Threading;
using System.Security.Principal;
using Opc.Ua;
using Technosoftware.UaServer.Subscriptions;

#endregion

namespace Technosoftware.UaServer.NodeManager
{
    /// <summary>
    /// An object that manages all events raised within the server.
    /// </summary>
    public class EventManager : IDisposable
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Creates a new instance of a sampling group.
        /// </summary>
        public EventManager(IUaServerData server, uint maxQueueSize, uint maxDurableQueueSize)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            m_server = server;
            m_monitoredItems = [];
            m_maxEventQueueSize = maxQueueSize;
            m_maxDurableEventQueueSize = maxDurableQueueSize;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Frees any unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// An overrideable version of the Dispose.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                List<IUaEventMonitoredItem> monitoredItems = null;

                lock (m_lock)
                {
                    monitoredItems = new List<IUaEventMonitoredItem>(m_monitoredItems.Values);
                    m_monitoredItems.Clear();
                }

                foreach (IUaEventMonitoredItem monitoredItem in monitoredItems)
                {
                    Utils.SilentDispose(monitoredItem);
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Reports an event.
        /// </summary>
        public static void ReportEvent(IFilterTarget e, IList<IUaEventMonitoredItem> monitoredItems)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            foreach (IUaEventMonitoredItem monitoredItem in monitoredItems)
            {
                monitoredItem.QueueEvent(e);
            }
        }

        /// <summary>
        /// Creates a set of monitored items.
        /// </summary>
        public UaMonitoredItem CreateMonitoredItem(
            UaServerOperationContext context,
            IUaNodeManager nodeManager,
            object handle,
            uint subscriptionId,
            uint monitoredItemId,
            TimestampsToReturn timestampsToReturn,
            double publishingInterval,
            MonitoredItemCreateRequest itemToCreate,
            EventFilter filter,
            bool createDurable)
        {
            lock (m_lock)
            {
                // calculate sampling interval.
                double samplingInterval = itemToCreate.RequestedParameters.SamplingInterval;

                if (samplingInterval < 0)
                {
                    samplingInterval = publishingInterval;
                }

                // limit the queue size.
                uint revisedQueueSize = CalculateRevisedQueueSize(createDurable, itemToCreate.RequestedParameters.QueueSize);

                // create the monitored item.
                UaMonitoredItem monitoredItem = new UaMonitoredItem(
                    m_server,
                    nodeManager,
                    handle,
                    subscriptionId,
                    monitoredItemId,
                    itemToCreate.ItemToMonitor,
                    context.DiagnosticsMask,
                    timestampsToReturn,
                    itemToCreate.MonitoringMode,
                    itemToCreate.RequestedParameters.ClientHandle,
                    filter,
                    filter,
                    null,
                    samplingInterval,
                    revisedQueueSize,
                    itemToCreate.RequestedParameters.DiscardOldest,
                    MinimumSamplingIntervals.Continuous,
                    createDurable);

                // save the monitored item.
                m_monitoredItems.Add(monitoredItemId, monitoredItem);

                return monitoredItem;
            }
        }
        /// <summary>
        /// Restore a MonitoredItem after a restart
        /// </summary>
        public UaMonitoredItem RestoreMonitoredItem(
            IUaNodeManager nodeManager,
            object handle,
            IUaStoredMonitoredItem storedMonitoredItem)
        {
            lock (m_lock)
            {
                // limit the queue size.
                storedMonitoredItem.QueueSize = CalculateRevisedQueueSize(storedMonitoredItem.IsDurable, storedMonitoredItem.QueueSize);

                // create the monitored item.
                UaMonitoredItem monitoredItem = new UaMonitoredItem(
                    m_server,
                    nodeManager,
                    handle,
                    storedMonitoredItem);

                // save the monitored item.
                m_monitoredItems.Add(monitoredItem.Id, monitoredItem);

                return monitoredItem;
            }
        }

        //calculates a revised queue size based on the application confiugration limits
        private uint CalculateRevisedQueueSize(bool isDurable, uint queueSize)
        {
            if (queueSize > m_maxEventQueueSize && !isDurable)
            {
                queueSize = m_maxEventQueueSize;
            }

            if (queueSize > m_maxDurableEventQueueSize && isDurable)
            {
                queueSize = m_maxDurableEventQueueSize;
            }

            return queueSize;
        }

        /// <summary>
        /// Modifies a monitored item.
        /// </summary>
        public void ModifyMonitoredItem(
            UaServerOperationContext context,
            IUaEventMonitoredItem monitoredItem,
            TimestampsToReturn timestampsToReturn,
            MonitoredItemModifyRequest itemToModify,
            EventFilter filter)
        {
            lock (m_lock)
            {
                // should never be called with items that it does not own.
                if (!m_monitoredItems.ContainsKey(monitoredItem.Id))
                {
                    return;
                }

                // limit the queue size.
                uint revisedQueueSize = CalculateRevisedQueueSize(monitoredItem.IsDurable, itemToModify.RequestedParameters.QueueSize);

                // modify the attributes.
                monitoredItem.ModifyAttributes(
                    context.DiagnosticsMask,
                    timestampsToReturn,
                    itemToModify.RequestedParameters.ClientHandle,
                    filter,
                    filter,
                    null,
                    itemToModify.RequestedParameters.SamplingInterval,
                    revisedQueueSize,
                    itemToModify.RequestedParameters.DiscardOldest);
            }
        }

        /// <summary>
        /// Deletes a monitored item.
        /// </summary>
        public void DeleteMonitoredItem(uint monitoredItemId)
        {
            lock (m_lock)
            {
                m_monitoredItems.Remove(monitoredItemId);
            }
        }

        /// <summary>
        /// Returns the currently active monitored items.
        /// </summary>
        public IList<IUaEventMonitoredItem> GetMonitoredItems()
        {
            lock (m_lock)
            {
                return new List<IUaEventMonitoredItem>(m_monitoredItems.Values);
            }
        }
        #endregion

        #region Private Fields
        private readonly object m_lock = new object();
        private IUaServerData m_server;
        private Dictionary<uint, IUaEventMonitoredItem> m_monitoredItems;
        private uint m_maxEventQueueSize;
        private uint m_maxDurableEventQueueSize;
        #endregion
    }
}
