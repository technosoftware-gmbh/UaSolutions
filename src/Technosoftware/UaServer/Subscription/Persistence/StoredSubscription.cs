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
using System.Collections.Generic;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <inheritdoc/>
    public class StoredSubscription : IUaStoredSubscription
    {
        /// <inheritdoc/>
        public uint Id { get; set; }

        /// <inheritdoc/>
        public uint LifetimeCounter { get; set; }

        /// <inheritdoc/>
        public uint MaxLifetimeCount { get; set; }

        /// <inheritdoc/>
        public uint MaxKeepaliveCount { get; set; }

        /// <inheritdoc/>
        public uint MaxMessageCount { get; set; }

        /// <inheritdoc/>
        public uint MaxNotificationsPerPublish { get; set; }

        /// <inheritdoc/>
        public double PublishingInterval { get; set; }

        /// <inheritdoc/>
        public byte Priority { get; set; }

        /// <inheritdoc/>
        public UserIdentityToken UserIdentityToken { get; set; }

        /// <inheritdoc/>
        public int LastSentMessage { get; set; }

        /// <inheritdoc/>
        public bool IsDurable { get; set; }

        /// <inheritdoc/>
        public uint SequenceNumber { get; set; }

        /// <inheritdoc/>
        public List<NotificationMessage> SentMessages { get; set; }

        /// <inheritdoc/>
        public IEnumerable<IUaStoredMonitoredItem> MonitoredItems { get; set; }
    }
}
