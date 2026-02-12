#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
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
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved

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
    /// State object that is used for snapshotting the subscription state
    /// </summary>
    [DataContract(Namespace = Namespaces.OpcUaXsd)]
    public record class SubscriptionState : SubscriptionOptions
    {
        /// <summary>
        /// Create subscription state
        /// </summary>
        public SubscriptionState()
        {
        }

        /// <summary>
        /// Create subscription state with current options
        /// </summary>
        /// <param name="options"></param>
        public SubscriptionState(SubscriptionOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Allows the list of monitored items to be saved/restored
        /// when the object is serialized.
        /// </summary>
        [DataMember(Order = 11)]
        public required MonitoredItemStateCollection MonitoredItems { get; init; }

        /// <summary>
        /// The current publishing interval.
        /// </summary>
        [DataMember(Order = 20)]
        public double CurrentPublishingInterval { get; init; }

        /// <summary>
        /// The current keep alive count.
        /// </summary>
        [DataMember(Order = 21)]
        public uint CurrentKeepAliveCount { get; init; }

        /// <summary>
        /// The current lifetime count.
        /// </summary>
        [DataMember(Order = 22)]
        public uint CurrentLifetimeCount { get; init; }

        /// <summary>
        /// When the state was created.
        /// </summary>
        [DataMember(Order = 23)]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// A collection of subscription states.
    /// </summary>
    [CollectionDataContract(
        Name = "ListOfSubscription",
        Namespace = Namespaces.OpcUaXsd,
        ItemName = "Subscription")]
    public class SubscriptionStateCollection : List<SubscriptionState>, ICloneable
    {
        /// <summary>
        /// Initializes an empty collection.
        /// </summary>
        public SubscriptionStateCollection()
        {
        }

        /// <summary>
        /// Initializes the collection from another collection.
        /// </summary>
        /// <param name="collection">The existing collection to use as
        /// the basis of creating this collection</param>
        public SubscriptionStateCollection(IEnumerable<SubscriptionState> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes the collection with the specified capacity.
        /// </summary>
        /// <param name="capacity">The max. capacity of the collection</param>
        public SubscriptionStateCollection(int capacity)
            : base(capacity)
        {
        }

        /// <inheritdoc/>
        public virtual object Clone()
        {
            var clone = new SubscriptionStateCollection();
            clone.AddRange(this.Select(item => item with { }));
            return clone;
        }
    }
}
