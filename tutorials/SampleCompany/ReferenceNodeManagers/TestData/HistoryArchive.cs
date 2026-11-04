#region Copyright (c) 2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com
//
// The Software is subject to the Technosoftware GmbH MIT License, which can
// be found here:
// https://technosoftware.com/license/mit/
//
// The Software is based on the OPC Foundation UA Stack and the OPC Foundation
// MIT License. The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Opc.Ua;
#endregion Using Directives

namespace SampleCompany.NodeManagers.TestData
{
    /// <summary>
    /// A class that provides access to archived data.
    /// </summary>
    internal sealed class HistoryArchive : IDisposable
    {
        public HistoryArchive(ITelemetryContext telemetry)
        {
            m_logger = telemetry.CreateLogger<HistoryArchive>();
        }

        /// <summary>
        /// Frees any unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            m_updateTimer?.Dispose();
            m_updateTimer = null;
        }

        /// <summary>
        /// Returns an object that can be used to browse the archive.
        /// </summary>
        public HistoryFile GetHistoryFile(NodeId nodeId)
        {
            lock (m_lock)
            {
                if (m_records == null)
                {
                    return null;
                }

                if (!m_records.TryGetValue(nodeId, out HistoryRecord record))
                {
                    return null;
                }

                return new HistoryFile(m_lock, record.RawData);
            }
        }

        /// <summary>
        /// Creates a new record in the archive.
        /// </summary>
        public void CreateRecord(NodeId nodeId, BuiltInType dataType)
        {
            lock (m_lock)
            {
                var record = new HistoryRecord
                {
                    RawData = [],
                    Historizing = true,
                    DataType = dataType
                };

                DateTime now = DateTime.UtcNow;

                for (int ii = 1000; ii >= 0; ii--)
                {
                    // DataValue is immutable in 2.0, so the value is built in
                    // one go rather than assembled field by field.
                    DateTime serverTimestamp = now.AddSeconds(-(ii * 10));
                    var entry = new HistoryEntry
                    {
                        Value = new DataValue(
                            dataType == BuiltInType.Int32 ? Variant.From(ii) : Variant.Null,
                            StatusCodes.Good,
                            serverTimestamp.AddMilliseconds(1234),
                            serverTimestamp),
                        IsModified = false
                    };

                    record.RawData.Add(entry);
                }

                m_records ??= [];

                m_records[nodeId] = record;

                m_updateTimer ??= new Timer(OnUpdate, null, 10000, 10000);
            }
        }

        /// <summary>
        /// Periodically adds new values into the archive.
        /// </summary>
        private void OnUpdate(object state)
        {
            try
            {
                DateTime now = DateTime.UtcNow;

                lock (m_lock)
                {
                    foreach (HistoryRecord record in m_records.Values)
                    {
                        if (!record.Historizing || record.RawData.Count >= 2000)
                        {
                            continue;
                        }

                        Variant value = Variant.Null;

                        if (record.DataType == BuiltInType.Int32)
                        {
                            record.RawData[^1].Value.WrappedValue.TryGetValue(out int lastValue);
                            value = Variant.From(lastValue + 1);
                        }

                        var entry = new HistoryEntry
                        {
                            Value = new DataValue(
                                value,
                                StatusCodes.Good,
                                now.AddMilliseconds(-4567),
                                now),
                            IsModified = false
                        };

                        record.RawData.Add(entry);
                    }
                }
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error updating history.");
            }
        }

        private readonly Lock m_lock = new();
        private Timer m_updateTimer;
        private Dictionary<NodeId, HistoryRecord> m_records;
        private readonly ILogger m_logger;
    }

    /// <summary>
    /// A single entry in the archive.
    /// </summary>
    internal sealed class HistoryEntry
    {
        public DataValue Value;
        public bool IsModified;
    }

    /// <summary>
    /// A record in the archive.
    /// </summary>
    internal sealed class HistoryRecord
    {
        public List<HistoryEntry> RawData;
        public bool Historizing;
        public BuiltInType DataType;
    }
}
