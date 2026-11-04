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
using Opc.Ua;
#endregion Using Directives

namespace SampleCompany.NodeManagers.TestData
{
    /// <summary>
    /// Wraps a file which contains a list of historical values.
    /// </summary>
    internal sealed class HistoryFile : IHistoryDataSource
    {
        /// <summary>
        /// Creates a new file.
        /// </summary>
        internal HistoryFile(Lock dataLock, List<HistoryEntry> entries)
        {
            m_lock = dataLock;
            m_entries = entries;
        }

        /// <summary>
        /// Returns the next value in the archive.
        /// </summary>
        /// <param name="startTime">The starting time for the search.</param>
        /// <param name="isForward">Whether to search forward in time.</param>
        /// <param name="isReadModified">Whether to return modified data.</param>
        /// <param name="position">A index that must be passed to the NextRaw call. </param>
        /// <returns>The DataValue.</returns>
        public DataValue FirstRaw(
            DateTimeUtc startTime,
            bool isForward,
            bool isReadModified,
            out int position)
        {
            position = -1;

            lock (m_lock)
            {
                if (isForward)
                {
                    for (int ii = 0; ii < m_entries.Count; ii++)
                    {
                        if (m_entries[ii].Value.ServerTimestamp >= startTime)
                        {
                            position = ii;
                            break;
                        }
                    }
                }
                else
                {
                    for (int ii = m_entries.Count - 1; ii >= 0; ii--)
                    {
                        if (m_entries[ii].Value.ServerTimestamp <= startTime)
                        {
                            position = ii;
                            break;
                        }
                    }
                }

                if (position < 0 || position >= m_entries.Count)
                {
                    return default;
                }

                HistoryEntry entry = m_entries[position];

                // DataValue is immutable in 2.0, so the defensive field-by-field
                // copy this used to make is unnecessary - the caller cannot
                // mutate what it is handed.
                return entry.Value;
            }
        }

        /// <summary>
        /// Returns the next value in the archive.
        /// </summary>
        /// <param name="lastTime">The timestamp of the last value returned.</param>
        /// <param name="isForward">Whether to search forward in time.</param>
        /// <param name="isReadModified">Whether to return modified data.</param>
        /// <param name="position">An index previously returned by the reader.</param>
        /// <returns>The DataValue.</returns>
        public DataValue NextRaw(
            DateTimeUtc lastTime,
            bool isForward,
            bool isReadModified,
            ref int position)
        {
            position++;

            lock (m_lock)
            {
                if (position < 0 || position >= m_entries.Count)
                {
                    return default;
                }

                HistoryEntry entry = m_entries[position];

                // DataValue is immutable in 2.0, so the defensive field-by-field
                // copy this used to make is unnecessary - the caller cannot
                // mutate what it is handed.
                return entry.Value;
            }
        }

        private readonly Lock m_lock = new();
        private readonly List<HistoryEntry> m_entries;
    }
}
