#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// A factory for <see cref="IUaDataChangeMonitoredItemQueue"> and </see> <see cref="IUaEventMonitoredItemQueue"/>
    /// </summary>
    public class MonitoredItemQueueFactory : IUaMonitoredItemQueueFactory
    {
        /// <inheritdoc/>
        public bool SupportsDurableQueues => false;

        /// <summary>
        /// Create monitored item queue factory
        /// </summary>
        /// <param name="telemetry">The telemetry context to use to create obvservability instruments</param>
        public MonitoredItemQueueFactory(ITelemetryContext telemetry)
        {
            m_telemetry = telemetry;
        }

        /// <inheritdoc/>
        public IUaDataChangeMonitoredItemQueue CreateDataChangeQueue(
            bool isDurable,
            uint monitoredItemId)
        {
            return new DataChangeMonitoredItemQueue(isDurable, monitoredItemId, m_telemetry);
        }

        /// <inheritdoc/>
        public IUaEventMonitoredItemQueue CreateEventQueue(bool isDurable, uint monitoredItemId)
        {
            return new EventMonitoredItemQueue(isDurable, monitoredItemId, m_telemetry);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Overridable method to dispose of resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            //only needed for managed resources
        }

        private readonly ITelemetryContext m_telemetry;
    }
}
