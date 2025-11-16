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
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Mangages an event queue for usage by a UaMonitoredItem
    /// </summary>
    public interface IUaEventQueueHandler : IDisposable
    {
        /// <summary>
        /// Sets the queue size.
        /// </summary>
        /// <param name="queueSize">The new queue size.</param>
        /// <param name="discardOldest">Whether to discard the oldest values if the queue overflows.</param>
        void SetQueueSize(uint queueSize, bool discardOldest);
        /// <summary>
        /// The number of Items in the queue
        /// </summary>
        int ItemsInQueue { get; }

        /// <summary>
        /// True if the queue is overflowing
        /// </summary>
        bool Overflow { get; }

        /// <summary>
        /// Checks the last 1k queue entries if the event is already in there
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        bool IsEventContainedInQueue(IFilterTarget instance);

        /// <summary>
        /// true if queue is already full and discarding is not allowed
        /// </summary>
        /// <returns></returns>
        bool SetQueueOverflowIfFull();

        /// <summary>
        /// Adds an event to the queue.
        /// </summary>
        void QueueEvent(EventFieldList fields);

        /// <summary>
        /// Publish Events
        /// </summary>
        /// <param name="context"></param>
        /// <param name="notifications"></param>
        /// <param name="maxNotificationsPerPublish">the maximum number of notifications to enqueue per call</param>
        /// <returns>the number of events that were added to the notification queue</returns>
        uint Publish(UaServerOperationContext context, Queue<EventFieldList> notifications, uint maxNotificationsPerPublish);
    }
}
