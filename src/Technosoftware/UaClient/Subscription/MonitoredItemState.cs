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
using System.Linq;
using System.Runtime.Serialization;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaClient
{
    /// <summary>
    /// State object that is used for snapshotting the monitored item
    /// </summary>
    [DataContract(Namespace = Namespaces.OpcUaXsd)]
    public record class MonitoredItemState : MonitoredItemOptions
    {
        /// <summary>
        /// Create monitored item state
        /// </summary>
        public MonitoredItemState()
        {
        }

        /// <summary>
        /// Create state from options
        /// </summary>
        /// <param name="options"></param>
        public MonitoredItemState(MonitoredItemOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Server-side identifier assigned to this monitored item (the
        /// <c>monitoredItemId</c>). Stored so the client can correlate notifications
        /// and perform Modify/SetMonitoringMode/Delete operations across reconnects.
        /// 0 indicates not yet created or invalidated.
        /// </summary>
        [DataMember(Order = 13)]
        public uint ServerId { get; init; }

        /// <summary>
        /// Client-assigned handle used in Publish notifications (clientHandle)
        /// to quickly map incoming data changes or events to local application
        /// structures without lookups on serverId. Should be unique per subscription.
        /// Typically used as an index/key into client data structures.
        /// </summary>
        [DataMember(Order = 14)]
        public uint ClientId { get; init; }

        /// <summary>
        /// When the state was created.
        /// </summary>
        [DataMember(Order = 15)]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Server-side identifier of the triggering item if this monitored item
        /// is triggered by another item. 0 indicates this item is not triggered.
        /// Used to restore triggering links after session reconnect.
        /// </summary>
        [DataMember(Order = 16)]
        public uint TriggeringItemId { get; init; }

        /// <summary>
        /// Collection of server-side identifiers of monitored items that are
        /// triggered by this item. Empty or null if this item does not trigger
        /// any other items. Used to restore triggering links after session reconnect.
        /// </summary>
        [DataMember(Order = 17)]
        public UInt32Collection? TriggeredItems { get; init; }
    }

    /// <summary>
    /// A collection of monitored item states.
    /// </summary>
    [CollectionDataContract(
        Name = "ListOfMonitoredItems",
        Namespace = Namespaces.OpcUaXsd,
        ItemName = "MonitoredItems")]
    public class MonitoredItemStateCollection : List<MonitoredItemState>, ICloneable
    {
        /// <summary>
        /// Initializes an empty collection.
        /// </summary>
        public MonitoredItemStateCollection()
        {
        }

        /// <summary>
        /// Initializes the collection from another collection.
        /// </summary>
        /// <param name="collection">The existing collection to use as
        /// the basis of creating this collection</param>
        public MonitoredItemStateCollection(IEnumerable<MonitoredItemState> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes the collection with the specified capacity.
        /// </summary>
        /// <param name="capacity">The max. capacity of the collection</param>
        public MonitoredItemStateCollection(int capacity)
            : base(capacity)
        {
        }

        /// <inheritdoc/>
        public virtual object Clone()
        {
            var clone = new MonitoredItemStateCollection();
            clone.AddRange(this.Select(item => item with { }));
            return clone;
        }
    }
}
