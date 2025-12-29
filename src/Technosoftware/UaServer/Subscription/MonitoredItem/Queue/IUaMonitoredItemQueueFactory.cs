#region Using Directives
using System;
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
