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

using Opc.Ua;
#endregion

namespace Technosoftware.UaServer.Subscriptions
{
    /// <summary>
    /// A factory for <see cref="IUaDataChangeMonitoredItemQueue"> and </see> <see cref="IUaEventMonitoredItemQueue"/>
    /// </summary>
    public class MonitoredItemQueueFactory : IUaMonitoredItemQueueFactory
    {
        /// <inheritdoc/>
        public bool SupportsDurableQueues => false;
        /// <inheritdoc/>
        public IUaDataChangeMonitoredItemQueue CreateDataChangeQueue(bool createDurable, uint monitoredItemId)
        {
            return new DataChangeMonitoredItemQueue(createDurable, monitoredItemId);
        }

        /// <inheritdoc/>
        public IUaEventMonitoredItemQueue CreateEventQueue(bool createDurable, uint monitoredItemId)
        {
            return new EventMonitoredItemQueue(createDurable, monitoredItemId);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            //only needed for managed resources
        }
    }
}
