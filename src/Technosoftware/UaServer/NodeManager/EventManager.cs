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
using System.Threading;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// An object that manages all events raised within the server.
    /// </summary>
    public class EventManager : IDisposable
    {
        /// <summary>
        /// Creates a new instance of a sampling group.
        /// </summary>
        public EventManager(IUaServerData server, uint maxQueueSize, uint maxDurableQueueSize)
        {
            m_server = server ?? throw new ArgumentNullException(nameof(server));
            m_monitoredItems = [];
            m_maxEventQueueSize = maxQueueSize;
            m_maxDurableEventQueueSize = maxDurableQueueSize;
        }

        /// <summary>
        /// Frees any unmanaged resources.
        /// </summary>
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
                List<IUaEventMonitoredItem> monitoredItems = null;

                lock (m_lock)
                {
                    monitoredItems = [.. m_monitoredItems.Values];
                    m_monitoredItems.Clear();
                }

                foreach (IUaEventMonitoredItem monitoredItem in monitoredItems)
                {
                    Utils.SilentDispose(monitoredItem);
                }
            }
        }

        /// <summary>
        /// Reports an event.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="e"/> is <c>null</c>.</exception>
        public static void ReportEvent(IFilterTarget e, IList<IUaEventMonitoredItem> monitoredItems)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            foreach (IUaEventMonitoredItem monitoredItem in monitoredItems)
            {
                monitoredItem.QueueEvent(e);
            }
        }

        /// <summary>
        /// Creates a set of monitored items.
        /// </summary>
        public IUaEventMonitoredItem CreateMonitoredItem(
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
                uint revisedQueueSize = CalculateRevisedQueueSize(
                    createDurable,
                    itemToCreate.RequestedParameters.QueueSize);

                // create the monitored item.
                IUaEventMonitoredItem monitoredItem = new UaMonitoredItem(
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
        /// Restore a UaMonitoredItem after a restart
        /// </summary>
        public IUaEventMonitoredItem RestoreMonitoredItem(
            IUaNodeManager nodeManager,
            object handle,
            IUaStoredMonitoredItem storedMonitoredItem)
        {
            lock (m_lock)
            {
                // limit the queue size.
                storedMonitoredItem.QueueSize = CalculateRevisedQueueSize(
                    storedMonitoredItem.IsDurable,
                    storedMonitoredItem.QueueSize);

                // create the monitored item.
                var monitoredItem = new UaMonitoredItem(
                    m_server,
                    nodeManager,
                    handle,
                    storedMonitoredItem);

                // save the monitored item.
                m_monitoredItems.Add(monitoredItem.Id, monitoredItem);

                return monitoredItem;
            }
        }

        /// <summary>
        /// calculates a revised queue size based on the application confiugration limits
        /// </summary>
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
                uint revisedQueueSize = CalculateRevisedQueueSize(
                    monitoredItem.IsDurable,
                    itemToModify.RequestedParameters.QueueSize);

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
                return [.. m_monitoredItems.Values];
            }
        }

        private readonly Lock m_lock = new();
        private readonly IUaServerData m_server;
        private readonly Dictionary<uint, IUaEventMonitoredItem> m_monitoredItems;
        private readonly uint m_maxEventQueueSize;
        private readonly uint m_maxDurableEventQueueSize;
    }
}
