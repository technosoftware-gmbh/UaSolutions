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
using System.Collections.Generic;
using Opc.Ua;
using Technosoftware.UaServer;
#endregion Using Directives

namespace Technosoftware.ClientGateway.Da
{
    /// <summary>
    /// A handle that describes how to access a node/attribute via an i/o manager.
    /// </summary>
    public class ComMonitoredItem : UaMonitoredItem
    {
        #region Constructors
        /// <summary>
        /// Initializes the object with its node type.
        /// </summary>
        public ComMonitoredItem(
            IUaServerData server,
            IUaNodeManager nodeManager,
            object mangerHandle,
            uint subscriptionId,
            uint id,
            Session session,
            ReadValueId itemToMonitor,
            DiagnosticsMasks diagnosticsMasks,
            TimestampsToReturn timestampsToReturn,
            MonitoringMode monitoringMode,
            uint clientHandle,
            MonitoringFilter originalFilter,
            MonitoringFilter filterToUse,
            Range range,
            double samplingInterval,
            uint queueSize,
            bool discardOldest,
            double sourceSamplingInterval)
            : base(server,
                    nodeManager,
                    mangerHandle,
                    subscriptionId,
                    id,
                    itemToMonitor,
                    diagnosticsMasks,
                    timestampsToReturn,
                    monitoringMode,
                    clientHandle,
                    originalFilter,
                    filterToUse,
                    range,
                    samplingInterval,
                    queueSize,
                    discardOldest,
                    sourceSamplingInterval,
                    false)
        {
        }
        #endregion Constructors

        #region Private Methods
        /// <summary>
        /// Publishes a single data change notifications.
        /// </summary>
        protected override bool Publish(UaServerOperationContext context,
            Queue<MonitoredItemNotification> notifications,
            Queue<DiagnosticInfo> diagnostics,
            DataValue value,
            ServiceResult error)
        {
            bool result = base.Publish(context, notifications, diagnostics, value, error);
            return result;
        }
        #endregion Private Methods
    }
}
