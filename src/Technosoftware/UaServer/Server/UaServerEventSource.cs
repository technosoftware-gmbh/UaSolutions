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
using System;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// The local EventSource for the server library.
    /// </summary>
    public static partial class UaServerUtils
    {
        /// <summary>
        /// The EventSource log interface.
        /// </summary>
        internal static UaServerEventSource EventLog { get; } = new UaServerEventSource();
    }

    /// <summary>
    /// Event source for high performance logging.
    /// </summary>
    [EventSource(Name = "Technosoftware.UaServer", Guid = "C99C4CD3-A6A9-44F2-9E2D-590ABF06CF3E")]
    internal sealed class UaServerEventSource : EventSource
    {
        /// <summary>
        /// client event ids
        /// </summary>
        private const int kSendResponseId = 1;
        private const int kServerCallId = kSendResponseId + 1;
        private const int kSessionStateId = kServerCallId + 1;
        private const int kMonitoredItemReadyId = kSessionStateId + 1;

        /// <summary>
        /// A server call message, called from ServerCallNative. Do not call directly..
        /// </summary>
        [Event(
            kServerCallId,
            Message = "Server Call={0}, Id={1}",
            Level = EventLevel.Informational)]
        public void ServerCall(string requestType, uint requestId)
        {
            if (IsEnabled())
            {
                WriteEvent(kServerCallId, requestType, requestId);
            }
        }

        /// <summary>
        /// The state of the session.
        /// </summary>
        [Event(
            kSessionStateId,
            Message = "Session {0}, Id={1}, Name={2}, ChannelId={3}, User={4}",
            Level = EventLevel.Informational)]
        public void SessionState(
            string context,
            string sessionId,
            string sessionName,
            string secureChannelId,
            string identity)
        {
            if (IsEnabled())
            {
                WriteEvent(
                    kSessionStateId,
                    context,
                    sessionId,
                    sessionName,
                    secureChannelId,
                    identity);
            }
        }

        /// <summary>
        /// The state of the server session.
        /// </summary>
        [Event(
            kMonitoredItemReadyId,
            Message = "IsReadyToPublish[{0}] {1}",
            Level = EventLevel.Verbose)]
        public void MonitoredItemReady(uint id, string state)
        {
            if (IsEnabled())
            {
                WriteEvent(kMonitoredItemReadyId, id, state);
            }
        }
    }
}
