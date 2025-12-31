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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Opc.Ua;
#endregion Using Directives

namespace SampleCompany.NodeManagers.DurableSubscription
{
    /// <inheritdoc/>
    public class BatchPersistor : IBatchPersistor
    {
        #region Constants
        private static readonly JsonSerializerSettings s_settings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private static readonly string s_storage_path = Path.Combine(
            Environment.CurrentDirectory,
            "Durable Subscriptions",
            "Batches");

        private const string kBaseFilename = "_batch.txt";
        #endregion Constants

        #region Constructors
        public BatchPersistor(ITelemetryContext telemetry)
        {
            m_logger = telemetry.CreateLogger<DurableDataChangeMonitoredItemQueue>();
            m_telemetry = telemetry;
        }
        #endregion Constructors

        #region IBatchPersistor Members
        /// <inheritdoc/>
        public void RequestBatchPersist(BatchBase batch)
        {
            lock (batch)
            {
                if (batch.IsPersisted || batch.PersistingInProgress || batch.RestoreInProgress)
                {
                    return;
                }
                batch.PersistingInProgress = true;

                if (m_batchesToPersist.TryAdd(batch.Id, batch))
                {
                    _ = Task.Run(() => PersistSynchronously(batch));
                }
            }
        }

        /// <inheritdoc/>
        public void RequestBatchRestore(BatchBase batch)
        {
            lock (batch)
            {
                if (!batch.IsPersisted || batch.RestoreInProgress || batch.PersistingInProgress)
                {
                    if (batch.PersistingInProgress)
                    {
                        batch.CancelBatchPersist?.Cancel();
                    }
                    return;
                }

                batch.RestoreInProgress = true;

                if (m_batchesToRestore.TryAdd(batch.Id, batch))
                {
                    _ = Task.Run(() => RestoreSynchronously(batch));
                }
            }
        }

        /// <inheritdoc/>
        public void RestoreSynchronously(BatchBase batch)
        {
            string filePath = Path.Combine(
                s_storage_path,
                $"{batch.MonitoredItemId}_{batch.Id}{kBaseFilename}");
            object result = null;
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    using IDisposable scope = AmbientMessageContext.SetScopedContext(m_telemetry);
                    result = JsonConvert.DeserializeObject(json, batch.GetType(), s_settings);

                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                m_logger.LogError(ex, "Failed to restore batch");

                batch.RestoreInProgress = false;
                m_batchesToRestore.TryRemove(batch.Id, out _);

                return;
            }
            lock (batch)
            {
                if (batch is DataChangeBatch dataChangeBatch)
                {
                    var newBatch = result as DataChangeBatch;
                    dataChangeBatch.Restore(newBatch.Values);
                }
                else if (batch is EventBatch eventBatch)
                {
                    var newBatch = result as EventBatch;
                    eventBatch.Restore(newBatch.Events);
                }
                m_batchesToRestore.TryRemove(batch.Id, out _);
            }
        }

        /// <inheritdoc/>
        public void PersistSynchronously(BatchBase batch)
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            batch.CancelBatchPersist = cancellationTokenSource;
            try
            {
                using IDisposable scope = AmbientMessageContext.SetScopedContext(m_telemetry);
                string result = JsonConvert.SerializeObject(batch, s_settings);

                if (!Directory.Exists(s_storage_path))
                {
                    Directory.CreateDirectory(s_storage_path);
                }

                string filePath = Path.Combine(
                    s_storage_path,
                    $"{batch.MonitoredItemId}_{batch.Id}{kBaseFilename}");

                File.WriteAllText(filePath, result);

                if (cancellationTokenSource.IsCancellationRequested)
                {
                    File.Delete(filePath);
                    lock (batch)
                    {
                        batch.PersistingInProgress = false;
                        batch.CancelBatchPersist = null;
                    }
                }
                else
                {
                    lock (batch)
                    {
                        batch.SetPersisted();
                    }
                }
                m_batchesToPersist.TryRemove(batch.Id, out _);
            }
            catch (Exception ex)
            {
                m_logger.LogWarning(ex, "Failed to store batch");
                lock (batch)
                {
                    batch.PersistingInProgress = false;
                    m_batchesToPersist.TryRemove(batch.Id, out _);
                    batch.CancelBatchPersist = null;
                }
            }
        }
        #endregion IBatchPersistor Members

        #region Public Methods
        /// <inheritdoc/>
        public void DeleteBatches(IEnumerable<uint> batchesToKeep)
        {
            try
            {
                if (Directory.Exists(s_storage_path))
                {
                    var directory = new DirectoryInfo(s_storage_path);

                    // Create a single regex pattern that matches any of the batches to keep
                    string pattern = string.Join(
                        "|",
                        batchesToKeep.Select(batch => $"{batch}_.*{kBaseFilename}$"));
                    var regex = new Regex(pattern, RegexOptions.Compiled);

                    foreach (FileInfo file in directory.GetFiles())
                    {
                        if (!regex.IsMatch(file.Name))
                        {
                            file.Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                m_logger.LogWarning(ex, "Failed to clean up batches");
            }
        }

        public void DeleteBatch(BatchBase batchToRemove)
        {
            try
            {
                if (Directory.Exists(s_storage_path))
                {
                    var directory = new DirectoryInfo(s_storage_path);
                    var regex = new Regex(
                        $"{batchToRemove.MonitoredItemId}_.{batchToRemove.Id}._{kBaseFilename}$",
                        RegexOptions.Compiled);

                    foreach (FileInfo file in directory.GetFiles())
                    {
                        if (!regex.IsMatch(file.Name))
                        {
                            file.Delete();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                m_logger.LogWarning(ex, "Failed to clean up single batch");
            }
        }
        #endregion Public Methods

        #region Private Fields
        private readonly ConcurrentDictionary<Guid, BatchBase> m_batchesToRestore = new();
        private readonly ConcurrentDictionary<Guid, BatchBase> m_batchesToPersist = new();
        private readonly ILogger m_logger;
        private readonly ITelemetryContext m_telemetry;
        #endregion Private Fields
    }
}
