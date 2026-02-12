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
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// The interface that a server exposes to objects that it contains.
    /// </summary>
    public static partial class UaServerUtils
    {
        private enum EventType
        {
            WriteValue,
            CreateItem,
            ModifyItem,
            QueueValue,
            FilterValue,
            DiscardValue,
            PublishValue
        }

        private class Event
        {
            public DateTime Timestamp;
            public EventType EventType;
            public NodeId NodeId;
            public uint ServerHandle;
            public DataValue Value;
            public MonitoringParameters Parameters;
            public MonitoringMode MonitoringMode;
        }

        private static readonly Queue<Event> s_events = new();
        private static bool s_eventsEnabled;

        /// <summary>
        /// Whether event queuing is enabled.
        /// </summary>
        public static bool EventsEnabled
        {
            get => s_eventsEnabled;
            set
            {
                if (s_eventsEnabled != value && !value)
                {
                    lock (s_events)
                    {
                        s_events.Clear();
                    }
                }

                s_eventsEnabled = value;
            }
        }

        /// <summary>
        /// Reports a value written.
        /// </summary>
        public static void ReportWriteValue(NodeId nodeId, DataValue value, StatusCode error)
        {
            if (!s_eventsEnabled)
            {
                return;
            }

            lock (s_events)
            {
                var e = new Event
                {
                    EventType = EventType.WriteValue,
                    NodeId = nodeId,
                    ServerHandle = 0,
                    Timestamp = HiResClock.UtcNow,
                    Value = value,
                    Parameters = null,
                    MonitoringMode = MonitoringMode.Disabled
                };

                if (StatusCode.IsBad(error))
                {
                    e.Value = new DataValue(error) { WrappedValue = value.WrappedValue };
                }

                s_events.Enqueue(e);
            }
        }

        /// <summary>
        /// Reports a value queued.
        /// </summary>
        public static void ReportQueuedValue(NodeId nodeId, uint serverHandle, DataValue value)
        {
            if (!s_eventsEnabled)
            {
                return;
            }

            lock (s_events)
            {
                var e = new Event
                {
                    EventType = EventType.QueueValue,
                    NodeId = nodeId,
                    ServerHandle = serverHandle,
                    Timestamp = HiResClock.UtcNow,
                    Value = value,
                    Parameters = null,
                    MonitoringMode = MonitoringMode.Disabled
                };
                s_events.Enqueue(e);
            }
        }

        /// <summary>
        /// Reports a value excluded by the filter.
        /// </summary>
        public static void ReportFilteredValue(NodeId nodeId, uint serverHandle, DataValue value)
        {
            if (!s_eventsEnabled)
            {
                return;
            }

            lock (s_events)
            {
                var e = new Event
                {
                    EventType = EventType.FilterValue,
                    NodeId = nodeId,
                    ServerHandle = serverHandle,
                    Timestamp = HiResClock.UtcNow,
                    Value = value,
                    Parameters = null,
                    MonitoringMode = MonitoringMode.Disabled
                };
                s_events.Enqueue(e);
            }
        }

        /// <summary>
        /// Reports a value discarded because of queue overflow.
        /// </summary>
        public static void ReportDiscardedValue(NodeId nodeId, uint serverHandle, DataValue value)
        {
            if (!s_eventsEnabled)
            {
                return;
            }

            lock (s_events)
            {
                var e = new Event
                {
                    EventType = EventType.DiscardValue,
                    NodeId = nodeId,
                    ServerHandle = serverHandle,
                    Timestamp = HiResClock.UtcNow,
                    Value = value,
                    Parameters = null,
                    MonitoringMode = MonitoringMode.Disabled
                };
                s_events.Enqueue(e);
            }
        }

        /// <summary>
        /// Reports a value published.
        /// </summary>
        public static void ReportPublishValue(NodeId nodeId, uint serverHandle, DataValue value)
        {
            if (!s_eventsEnabled)
            {
                return;
            }

            lock (s_events)
            {
                var e = new Event
                {
                    EventType = EventType.PublishValue,
                    NodeId = nodeId,
                    ServerHandle = serverHandle,
                    Timestamp = HiResClock.UtcNow,
                    Value = value,
                    Parameters = null,
                    MonitoringMode = MonitoringMode.Disabled
                };
                s_events.Enqueue(e);
            }
        }

        /// <summary>
        /// Reports a new monitored item.
        /// </summary>
        public static void ReportCreateMonitoredItem(
            NodeId nodeId,
            uint serverHandle,
            double samplingInterval,
            uint queueSize,
            bool discardOldest,
            MonitoringFilter filter,
            MonitoringMode monitoringMode)
        {
            if (!s_eventsEnabled)
            {
                return;
            }

            lock (s_events)
            {
                var e = new Event
                {
                    EventType = EventType.CreateItem,
                    NodeId = nodeId,
                    ServerHandle = serverHandle,
                    Timestamp = HiResClock.UtcNow,
                    Value = null,
                    Parameters = new MonitoringParameters
                    {
                        SamplingInterval = samplingInterval,
                        QueueSize = queueSize,
                        DiscardOldest = discardOldest,
                        Filter = new ExtensionObject(filter)
                    },
                    MonitoringMode = monitoringMode
                };
                s_events.Enqueue(e);
            }
        }

        /// <summary>
        /// Reports a modified monitored item.
        /// </summary>
        public static void ReportModifyMonitoredItem(
            NodeId nodeId,
            uint serverHandle,
            double samplingInterval,
            uint queueSize,
            bool discardOldest,
            MonitoringFilter filter,
            MonitoringMode monitoringMode)
        {
            if (!s_eventsEnabled)
            {
                return;
            }

            lock (s_events)
            {
                var e = new Event
                {
                    EventType = EventType.ModifyItem,
                    NodeId = nodeId,
                    ServerHandle = serverHandle,
                    Timestamp = HiResClock.UtcNow,
                    Value = null,
                    Parameters = new MonitoringParameters
                    {
                        SamplingInterval = samplingInterval,
                        QueueSize = queueSize,
                        DiscardOldest = discardOldest,
                        Filter = new ExtensionObject(filter)
                    },
                    MonitoringMode = monitoringMode
                };
                s_events.Enqueue(e);
            }
        }

        /// <summary>
        /// Fills in the diagnostic information after an error.
        /// </summary>
        public static uint CreateError(
            uint code,
            UaServerOperationContext context,
            DiagnosticInfoCollection diagnosticInfos,
            int index,
            ILogger logger)
        {
            var error = new ServiceResult(code);

            if ((context.DiagnosticsMask & DiagnosticsMasks.OperationAll) != 0)
            {
                diagnosticInfos[index] = new DiagnosticInfo(
                    error,
                    context.DiagnosticsMask,
                    false,
                    context.StringTable,
                    logger);
            }

            return error.Code;
        }

        /// <summary>
        /// Fills in the diagnostic information after an error.
        /// </summary>
        public static bool CreateError(
            uint code,
            StatusCodeCollection results,
            DiagnosticInfoCollection diagnosticInfos,
            UaServerOperationContext context,
            ILogger logger)
        {
            var error = new ServiceResult(code);
            results.Add(error.Code);

            if ((context.DiagnosticsMask & DiagnosticsMasks.OperationAll) != 0)
            {
                diagnosticInfos.Add(
                    new DiagnosticInfo(error, context.DiagnosticsMask, false, context.StringTable, logger));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Fills in the diagnostic information after an error.
        /// </summary>
        public static bool CreateError(
            uint code,
            StatusCodeCollection results,
            DiagnosticInfoCollection diagnosticInfos,
            int index,
            UaServerOperationContext context,
            ILogger logger)
        {
            var error = new ServiceResult(code);
            results[index] = error.Code;

            if ((context.DiagnosticsMask & DiagnosticsMasks.OperationAll) != 0)
            {
                diagnosticInfos[index] = new DiagnosticInfo(
                    error,
                    context.DiagnosticsMask,
                    false,
                    context.StringTable,
                    logger);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Creates a place holder in the lists for the results.
        /// </summary>
        public static void CreateSuccess(
            StatusCodeCollection results,
            DiagnosticInfoCollection diagnosticInfos,
            UaServerOperationContext context)
        {
            results.Add(StatusCodes.Good);

            if ((context.DiagnosticsMask & DiagnosticsMasks.OperationAll) != 0)
            {
                diagnosticInfos.Add(null);
            }
        }

        /// <summary>
        /// Creates a collection of diagnostics from a set of errors.
        /// </summary>
        public static DiagnosticInfoCollection CreateDiagnosticInfoCollection(
            UaServerOperationContext context,
            IList<ServiceResult> errors,
            ILogger logger)
        {
            // all done if no diagnostics requested.
            if ((context.DiagnosticsMask & DiagnosticsMasks.OperationAll) == 0)
            {
                return null;
            }

            // create diagnostics.
            var results = new DiagnosticInfoCollection(errors.Count);

            foreach (ServiceResult error in errors)
            {
                if (ServiceResult.IsBad(error))
                {
                    results.Add(new DiagnosticInfo(
                        error,
                        context.DiagnosticsMask,
                        false,
                        context.StringTable,
                        logger));
                }
                else
                {
                    results.Add(null);
                }
            }

            return results;
        }

        /// <summary>
        /// Creates a collection of status codes and diagnostics from a set of errors.
        /// </summary>
        public static StatusCodeCollection CreateStatusCodeCollection(
            UaServerOperationContext context,
            IList<ServiceResult> errors,
            out DiagnosticInfoCollection diagnosticInfos,
            ILogger logger)
        {
            diagnosticInfos = null;

            bool noErrors = true;
            var results = new StatusCodeCollection(errors.Count);

            foreach (ServiceResult error in errors)
            {
                if (ServiceResult.IsBad(error))
                {
                    results.Add(error.Code);
                    noErrors = false;
                }
                else
                {
                    results.Add(StatusCodes.Good);
                }
            }

            // only generate diagnostics if errors exist.
            if (noErrors)
            {
                diagnosticInfos = CreateDiagnosticInfoCollection(context, errors, logger);
            }

            return results;
        }

        /// <summary>
        /// Creates the diagnostic info and translates any strings.
        /// </summary>
        [Obsolete("Use CreateDiagnosticInfo with ILogger")]
        public static DiagnosticInfo CreateDiagnosticInfo(
            IUaServerData server,
            UaServerOperationContext context,
            ServiceResult error)
        {
            ILogger logger = AmbientMessageContext.Telemetry.CreateLogger("ServerUtils");
            return CreateDiagnosticInfo(server, context, error, logger);
        }

        /// <summary>
        /// Creates the diagnostic info and translates any strings.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="context">The context containing the string stable.</param>
        /// <param name="error">The error to translate.</param>
        /// <param name="logger">A contextual logger to log to</param>
        /// <returns>The diagnostics with references to the strings in the context string table.</returns>
        public static DiagnosticInfo CreateDiagnosticInfo(
            IUaServerData server,
            UaServerOperationContext context,
            ServiceResult error,
            ILogger logger)
        {
            if (error == null)
            {
                return null;
            }

            ServiceResult translatedError = error;

            if ((context.DiagnosticsMask & DiagnosticsMasks.LocalizedText) != 0)
            {
                translatedError = server.ResourceManager.Translate(context.PreferredLocales, error);
            }

            return new DiagnosticInfo(
                translatedError,
                context.DiagnosticsMask,
                false,
                context.StringTable,
                logger);
        }
    }
}
