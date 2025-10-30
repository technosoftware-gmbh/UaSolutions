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
using System.Collections.Generic;
using Opc.Ua;
using Technosoftware.UaServer;
using Technosoftware.UaServer.Subscriptions;
#endregion Using Directives

namespace SampleCompany.NodeManagers.DurableSubscription
{
    /// <summary>
    /// Persists batches of queue values to disk
    /// </summary>
    public interface IBatchPersistor
    {
        /// <summary>
        /// Request that a batch shall be persisted in a background thread
        /// </summary>
        void RequestBatchPersist(BatchBase batch);

        /// <summary>
        /// Persist a batch in the main thread
        /// </summary>
        void PersistSynchronously(BatchBase batch);

        /// <summary>
        /// Request that a batch shall be restored in a background thread
        /// </summary>
        void RequestBatchRestore(BatchBase batch);

        /// <summary>
        /// Restore a batch in the main thread
        /// </summary>
        void RestoreSynchronously(BatchBase batch);

        /// <summary>
        /// Delete all batches from disk for a monitored item
        /// </summary>
        /// <param name="batchesToKeep">MonitoredItemIds of the batches to keep on disk</param>
        void DeleteBatches(IEnumerable<uint> batchesToKeep);

        /// <summary>
        /// Delete a single batch from disk
        /// </summary>
        /// <param name="batchToRemove">The Batch to remove</param>
        void DeleteBatch(BatchBase batchToRemove);
    }
}
