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
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// An object which periodically reads the items and updates the cache.
    /// </summary>
    public class SamplingGroup : IDisposable
    {
        /// <summary>
        /// Creates a new instance of a sampling group.
        /// </summary>
        public SamplingGroup(
            IUaServerData server,
            IUaNodeManager nodeManager,
            List<SamplingRateGroup> samplingRates,
            UaServerOperationContext context,
            double samplingInterval,
            IUserIdentity savedOwnerIdentity = null)
        {
            m_server = server ?? throw new ArgumentNullException(nameof(server));
            m_nodeManager = nodeManager ?? throw new ArgumentNullException(nameof(nodeManager));
            m_samplingRates = samplingRates ??
                throw new ArgumentNullException(nameof(samplingRates));
            m_session = context.Session;
            if (m_session == null)
            {
                m_effectiveIdentity =
                    savedOwnerIdentity
                    ?? throw new ArgumentNullException(
                        nameof(savedOwnerIdentity),
                        "Either a context with a Session or an owner identity need to be provided");
            }
            m_diagnosticsMask = context.DiagnosticsMask & DiagnosticsMasks.OperationAll;
            m_samplingInterval = AdjustSamplingInterval(samplingInterval);

            m_itemsToAdd = [];
            m_itemsToRemove = [];
            m_items = [];

            // create a event to signal shutdown.
            m_shutdownEvent = new ManualResetEvent(true);
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
                lock (m_lock)
                {
                    m_shutdownEvent.Set();
                    m_samplingRates.Clear();
                }

                if (m_samplingTask != null)
                {
                    try
                    {
                        m_samplingTask.GetAwaiter().GetResult();
                    }
                    catch (AggregateException)
                    { /* Ignore exceptions on shutdown */
                    }
                }

                Utils.SilentDispose(m_samplingTask);
                Utils.SilentDispose(m_shutdownEvent);
            }
        }

        /// <summary>
        /// Starts the sampling thread which periodically reads the items in the group.
        /// </summary>
        public void Startup()
        {
            lock (m_lock)
            {
                m_shutdownEvent.Reset();

                m_samplingTask = Task.Factory.StartNew(
                    () => SampleMonitoredItems(m_samplingInterval),
                    TaskCreationOptions.LongRunning | TaskCreationOptions.DenyChildAttach);
            }
        }

        /// <summary>
        /// Stops the sampling thread.
        /// </summary>
        public void Shutdown()
        {
            lock (m_lock)
            {
                m_shutdownEvent.Set();
                m_items.Clear();
                m_samplingTask = null;
            }
        }

        /// <summary>
        /// Checks if the monitored item can be handled by the group.
        /// </summary>
        /// <returns>
        /// True if the item was added to the group.
        /// </returns>
        /// <remarks>
        /// The ApplyChanges() method must be called to actually start sampling the item.
        /// </remarks>
        public bool StartMonitoring(
            UaServerOperationContext context,
            IUaSampledDataChangeMonitoredItem monitoredItem,
            IUserIdentity savedOwnerIdentity = null)
        {
            lock (m_lock)
            {
                if (MeetsGroupCriteria(context, monitoredItem, savedOwnerIdentity))
                {
                    m_itemsToAdd.Add(monitoredItem);
                    monitoredItem.SetSamplingInterval(m_samplingInterval);
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Checks if the monitored item can still be handled by the group.
        /// </summary>
        /// <returns>
        /// False if the item has be marked for removal from the group.
        /// </returns>
        /// <remarks>
        /// The ApplyChanges() method must be called to actually stop sampling the item.
        /// </remarks>
        public bool ModifyMonitoring(
            UaServerOperationContext context,
            IUaSampledDataChangeMonitoredItem monitoredItem)
        {
            lock (m_lock)
            {
                if (m_items.ContainsKey(monitoredItem.Id))
                {
                    if (MeetsGroupCriteria(context, monitoredItem))
                    {
                        monitoredItem.SetSamplingInterval(m_samplingInterval);
                        return true;
                    }

                    m_itemsToRemove.Add(monitoredItem);
                }

                return false;
            }
        }

        /// <summary>
        /// Stops monitoring the item.
        /// </summary>
        /// <returns>
        /// Returns true if the items was marked for removal from the group.
        /// </returns>
        public bool StopMonitoring(IUaSampledDataChangeMonitoredItem monitoredItem)
        {
            lock (m_lock)
            {
                if (m_items.ContainsKey(monitoredItem.Id))
                {
                    m_itemsToRemove.Add(monitoredItem);
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Updates the group by apply any pending changes.
        /// </summary>
        /// <returns>
        /// Returns true if the group has no more items and can be dropped.
        /// </returns>
        public bool ApplyChanges()
        {
            lock (m_lock)
            {
                // add items.
                var itemsToSample = new List<IUaSampledDataChangeMonitoredItem>();

                for (int ii = 0; ii < m_itemsToAdd.Count; ii++)
                {
                    IUaSampledDataChangeMonitoredItem monitoredItem = m_itemsToAdd[ii];

                    if (m_items.TryAdd(monitoredItem.Id, monitoredItem) &&
                        monitoredItem.MonitoringMode != MonitoringMode.Disabled)
                    {
                        itemsToSample.Add(monitoredItem);
                    }
                }

                m_itemsToAdd.Clear();

                // collect first sample.
                if (itemsToSample.Count > 0)
                {
                    Task.Run(() => DoSample(itemsToSample));
                }

                // remove items.
                for (int ii = 0; ii < m_itemsToRemove.Count; ii++)
                {
                    m_items.Remove(m_itemsToRemove[ii].Id);
                }

                m_itemsToRemove.Clear();

                // start the group if it is not running.
                if (m_samplingTask == null && m_items.Count > 0)
                {
                    Startup();
                }
                // stop the group if it is running.
                else if (m_items.Count == 0)
                {
                    Shutdown();
                }

                // can be shutdown if no items left.
                return m_items.Count == 0;
            }
        }

        /// <summary>
        /// Checks if the item meets the group's criteria.
        /// </summary>
        private bool MeetsGroupCriteria(
            UaServerOperationContext context,
            IUaSampledDataChangeMonitoredItem monitoredItem,
            IUserIdentity savedOwnerIdentity = null)
        {
            // can only sample variables.
            if ((monitoredItem.MonitoredItemType & MonitoredItemTypeMask.DataChange) == 0)
            {
                return false;
            }

            // can't sample disabled items.
            if (monitoredItem.MonitoringMode == MonitoringMode.Disabled)
            {
                return false;
            }

            // check sampling interval.
            if (AdjustSamplingInterval(monitoredItem.SamplingInterval) != m_samplingInterval)
            {
                return false;
            }

            if (m_session == null && context?.SessionId == null)
            {
                //fallback to compare user Identity if session is not set.
                if (savedOwnerIdentity?.GetIdentityToken() != m_effectiveIdentity
                    .GetIdentityToken())
                {
                    return false;
                }
            }
            else
            {
                //compare session
                if (context?.SessionId != m_session?.Id)
                {
                    return false;
                }
            }

            // check the diagnostics marks.
            return m_diagnosticsMask == (context.DiagnosticsMask & DiagnosticsMasks.OperationAll);
        }

        /// <summary>
        /// Ensures the requested sampling interval lines up with one of the supported sampling rates.
        /// </summary>
        private double AdjustSamplingInterval(double samplingInterval)
        {
            foreach (SamplingRateGroup samplingRate in m_samplingRates)
            {
                // groups are ordered by start rate.
                if (samplingInterval <= samplingRate.Start)
                {
                    return samplingRate.Start;
                }

                // check if within range specified by the group.
                double maxSamplingRate = samplingRate.Start;

                if (samplingRate.Increment > 0)
                {
                    maxSamplingRate += samplingRate.Increment * samplingRate.Count;
                }

                if (samplingInterval > maxSamplingRate)
                {
                    continue;
                }

                // find sampling rate within rate group.
                if (samplingInterval == maxSamplingRate)
                {
                    return maxSamplingRate;
                }

                for (double ii = samplingRate.Start;
                    ii <= maxSamplingRate;
                    ii += samplingRate.Increment)
                {
                    if (ii >= samplingInterval)
                    {
                        return ii;
                    }
                }
            }

            return samplingInterval;
        }

        /// <summary>
        /// Periodically checks if the sessions have timed out.
        /// </summary>
        private void SampleMonitoredItems(object data)
        {
            try
            {
                Utils.LogTrace("Server: {0} Thread Started.", Thread.CurrentThread.Name);

                int sleepCycle = Convert.ToInt32(data, CultureInfo.InvariantCulture);
                int timeToWait = sleepCycle;

                while (m_server.IsRunning)
                {
                    DateTime start = DateTime.UtcNow;

                    // wait till next sample.
                    if (m_shutdownEvent.WaitOne(timeToWait))
                    {
                        break;
                    }

                    // get current list of items to sample.
                    var items = new List<IUaSampledDataChangeMonitoredItem>();

                    lock (m_lock)
                    {
                        uint disabledItemCount = 0;
                        Dictionary<uint, IUaSampledDataChangeMonitoredItem>.Enumerator enumerator =
                            m_items.GetEnumerator();

                        while (enumerator.MoveNext())
                        {
                            IUaSampledDataChangeMonitoredItem monitoredItem = enumerator.Current
                                .Value;

                            if (monitoredItem.MonitoringMode == MonitoringMode.Disabled)
                            {
                                disabledItemCount++;
                                continue;
                            }

                            // check whether the item should be sampled.
                            //if (!monitoredItem.SamplingIntervalExpired())
                            //{
                            //    continue;
                            //}

                            items.Add(monitoredItem);
                        }
                    }

                    // sample the values.
                    DoSample(items);

                    int delay = (int)(DateTime.UtcNow - start).TotalMilliseconds;
                    timeToWait = sleepCycle;

                    if (delay > sleepCycle)
                    {
                        timeToWait = (2 * sleepCycle) - delay;

                        if (timeToWait < 0)
                        {
                            Utils.LogWarning(
                                "WARNING: SamplingGroup cannot sample fast enough. TimeToSample={0}ms, SamplingInterval={1}ms",
                                delay,
                                sleepCycle);
                            timeToWait = sleepCycle;
                        }
                    }
                }

                Utils.LogTrace("Server: {0} Thread Exited Normally.", Thread.CurrentThread.Name);
            }
            catch (Exception e)
            {
                Utils.LogError(e, "Server: SampleMonitoredItems Thread Exited Unexpectedly.");
            }
        }

        /// <summary>
        /// Samples the values of the items.
        /// </summary>
        private void DoSample(object state)
        {
            try
            {
                // read values for all enabled items.
                if (state is List<IUaSampledDataChangeMonitoredItem> items && items.Count > 0)
                {
                    var itemsToRead = new ReadValueIdCollection(items.Count);
                    var values = new DataValueCollection(items.Count);
                    var errors = new List<ServiceResult>(items.Count);

                    // allocate space for results.
                    for (int ii = 0; ii < items.Count; ii++)
                    {
                        ReadValueId readValueId = items[ii].GetReadValueId();
                        readValueId.Processed = false;
                        itemsToRead.Add(readValueId);

                        values.Add(null);
                        errors.Add(null);
                    }

                    UaServerOperationContext context;

                    if (m_session != null)
                    {
                        context = new UaServerOperationContext(m_session, m_diagnosticsMask);
                    }
                    else
                    {
                        // if session of the Sampling group is not set yet, use the first monitored item to create the context.
                        IUaMonitoredItem firstItem = items[0];
                        context = new UaServerOperationContext(firstItem);
                        m_session = firstItem.Session;
                    }

                    // read values.
                    m_nodeManager.Read(context, 0, itemsToRead, values, errors);

                    // update monitored items.
                    for (int ii = 0; ii < items.Count; ii++)
                    {
                        if (values[ii] == null)
                        {
                            values[ii] = new DataValue(
                                StatusCodes.BadInternalError,
                                DateTime.UtcNow);
                        }

                        items[ii].QueueValue(values[ii], errors[ii]);
                    }
                }
            }
            catch (Exception e)
            {
                Utils.LogError(e, "Server: Unexpected error sampling values.");
            }
        }

        private readonly Lock m_lock = new();
        private readonly IUaServerData m_server;
        private readonly IUaNodeManager m_nodeManager;
        private IUaSession m_session;
        private readonly IUserIdentity m_effectiveIdentity;
        private readonly DiagnosticsMasks m_diagnosticsMask;
        private readonly double m_samplingInterval;
        private readonly List<IUaSampledDataChangeMonitoredItem> m_itemsToAdd;
        private readonly List<IUaSampledDataChangeMonitoredItem> m_itemsToRemove;
        private readonly Dictionary<uint, IUaSampledDataChangeMonitoredItem> m_items;
        private readonly ManualResetEvent m_shutdownEvent;
        private readonly List<SamplingRateGroup> m_samplingRates;
        private Task m_samplingTask;
    }
}
