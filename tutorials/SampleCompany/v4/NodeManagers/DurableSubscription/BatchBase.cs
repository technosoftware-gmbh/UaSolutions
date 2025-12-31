#region Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Threading;
using Opc.Ua;
using Technosoftware.UaServer;
using Technosoftware.UaServer.Subscriptions;
#endregion Using Directives

namespace SampleCompany.NodeManagers.DurableSubscription
{
    /// <summary>
    /// Base class for a batch of data
    /// </summary>
    public abstract class BatchBase
    {
        protected BatchBase(uint batchSize, uint monitoredItemId)
        {
            BatchSize = batchSize;
            Id = Guid.NewGuid();
            IsPersisted = false;
            MonitoredItemId = monitoredItemId;
        }

        /// <summary>
        /// The unique Id of the batch
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The number of values in the batch
        /// </summary>
        public uint BatchSize { get; }

        /// <summary>
        /// The Id of the Monitored Item owning the batch
        /// </summary>
        public uint MonitoredItemId { get; }

        /// <summary>
        /// The batch has been persisted to disk
        /// </summary>
        public bool IsPersisted { get; protected set; }

        /// <summary>
        /// Restore is currently in progress in a background thread
        /// </summary>
        public bool RestoreInProgress { get; set; }

        /// <summary>
        /// Peristing is currently in progress in a background thread
        /// </summary>
        public bool PersistingInProgress { get; set; }

        /// <summary>
        /// Marks the batch as persisted and removes the data from memory
        /// </summary>
        public abstract void SetPersisted();

        /// <summary>
        /// Cancel this token to stop the persisting of the batch
        /// </summary>
        public CancellationTokenSource CancelBatchPersist { get; set; }
    }
}
