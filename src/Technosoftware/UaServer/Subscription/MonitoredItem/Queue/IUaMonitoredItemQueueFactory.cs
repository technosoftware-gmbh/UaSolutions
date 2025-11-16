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
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Used to create <see cref="IUaDataChangeMonitoredItemQueue"/> / <see cref="IUaEventMonitoredItemQueue"/> and dispose unmanaged resources on server shutdown
    /// Optionally supports durable queues and can be used to perform shared background operations on the queues
    /// </summary>
    public interface IUaMonitoredItemQueueFactory : IDisposable
    {
        /// <summary>
        /// Creates an empty queue for data values.
        /// </summary>
        IUaDataChangeMonitoredItemQueue CreateDataChangeQueue(bool isDurable, uint monitoredItemId);

        /// <summary>
        /// Creates an empty queue for events.
        /// </summary>
        IUaEventMonitoredItemQueue CreateEventQueue(bool isDurable, uint monitoredItemId);

        /// <summary>
        /// If true durable queues can be created by the factory, if false only regular queues with small queue sizes are returned
        /// </summary>
        bool SupportsDurableQueues { get; }
    }
}
