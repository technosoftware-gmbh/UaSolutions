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
    /// Provides an optionally durable queue for data changes used by <see cref="DataChangeQueueHandler"/> and created by <see cref="IUaMonitoredItemQueueFactory"/>.
    /// If long running operations are performed by the queue the operation should be performed in a separate thread to avoid blocking the main thread.
    /// Min Enqueue performance: no long running operations, fast enqueue until max queue size is reached
    /// Min Dequeue performance: MaxNotificationsPerPublish * 3 with no delay, in a cycle of 3 * MinPublishingInterval in the least favorable condition (single MI, continous publishing (MinPublishingInterval --, MaxNotificationsPerPublish ++), very large queue)
    /// Queue reset is allowed to be slow
    /// </summary>
    public interface IUaDataChangeMonitoredItemQueue : IDisposable
    {
        /// <summary>
        /// The Id of the UaMonitoredItem associated with the queue
        /// </summary>
        uint MonitoredItemId { get; }

        /// <summary>
        /// True if the queue is in durable mode and persists the queue values / supports a large queue size
        /// </summary>
        bool IsDurable { get; }

        /// <summary>
        /// Gets the current queue size.
        /// </summary>
        uint QueueSize { get; }

        /// <summary>
        /// Gets number of elements actually contained in value queue.
        /// </summary>
        int ItemsInQueue { get; }

        /// <summary>
        /// Resets thew queue, sets the new queue size initializes an empty queue (caller handles existing entries).
        /// </summary>
        /// <param name="queueSize">The new queue size.</param>
        /// <param name="queueErrors">Specifies wether errors should be queued.</param>
        void ResetQueue(uint queueSize, bool queueErrors);

        /// <summary>
        /// Adds the value to the queue.
        /// </summary>
        /// <param name="value">The value to queue.</param>
        /// <param name="error">The error to queue.</param>
        void Enqueue(DataValue value, ServiceResult error);

        /// <summary>
        /// Dequeue the oldest value in the queue.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="error">The error associated with the value.</param>
        /// <returns>True if a value was found. False if the queue is empty.</returns>
        bool Dequeue(out DataValue value, out ServiceResult error);

        /// <summary>
        /// returns the oldest value in the queue without dequeueing. Null if queue is empty
        /// </summary>
        DataValue PeekOldestValue();

        /// <summary>
        /// Replace the last (newest) value in the queue with the provided Value. Used when values are provided faster than the sampling interval
        /// </summary>
        /// <param name="value">The value to queue.</param>
        /// <param name="error">The error to queue.</param>
        void OverwriteLastValue(DataValue value, ServiceResult error);

        /// <summary>
        /// Returns the last (newest) value in the queue without dequeuing
        /// </summary>
        /// <returns>the last value, null if queue is empty</returns>
        DataValue PeekLastValue();
    }
}
