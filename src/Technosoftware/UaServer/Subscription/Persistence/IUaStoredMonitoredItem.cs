#region Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
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
#endregion Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// A monitored item in a format to be persited by an <see cref="IUaSubscriptionStore"/>
    /// </summary>
    public interface IUaStoredMonitoredItem
    {
        /// <summary>
        /// If the item was restored by a node manager
        /// </summary>
        bool IsRestored { get; set; }

        /// <summary>
        /// Alwasys report Updates
        /// </summary>
        bool AlwaysReportUpdates { get; set; }

        /// <summary>
        /// The attribute to monitor
        /// </summary>
        uint AttributeId { get; set; }

        /// <summary>
        /// Identifier of the client
        /// </summary>
        uint ClientHandle { get; set; }

        /// <summary>
        /// The diagnostics masks
        /// </summary>
        DiagnosticsMasks DiagnosticsMasks { get; set; }

        /// <summary>
        /// If the oldes or newest entry shall be discarded on queue overflw
        /// </summary>
        bool DiscardOldest { get; set; }

        /// <summary>
        /// The encoding to use
        /// </summary>
        QualifiedName Encoding { get; set; }

        /// <summary>
        /// The Id of the monitored Item
        /// </summary>
        uint Id { get; set; }

        /// <summary>
        /// The Index Range
        /// </summary>
        string IndexRange { get; set; }

        /// <summary>
        /// The parsed index range
        /// </summary>
        NumericRange ParsedIndexRange { get; set; }

        /// <summary>
        /// If the monitored item is child of a durable subscription
        /// </summary>
        bool IsDurable { get; set; }

        /// <summary>
        /// The last error to notify
        /// </summary>
        ServiceResult LastError { get; set; }

        /// <summary>
        /// THe last value to notify
        /// </summary>
        DataValue LastValue { get; set; }

        /// <summary>
        /// The Monitoring Mode
        /// </summary>
        MonitoringMode MonitoringMode { get; set; }

        /// <summary>
        /// The NodeId being monitored
        /// </summary>
        NodeId NodeId { get; set; }

        /// <summary>
        /// The monitoring filter to use
        /// </summary>
        MonitoringFilter FilterToUse { get; set; }

        /// <summary>
        /// The original monitoring filter
        /// </summary>
        MonitoringFilter OriginalFilter { get; set; }

        /// <summary>
        /// The queue size
        /// </summary>
        uint QueueSize { get; set; }

        /// <summary>
        /// The Range
        /// </summary>
        double Range { get; set; }

        /// <summary>
        /// The sampling invterval to use
        /// </summary>
        double SamplingInterval { get; set; }

        /// <summary>
        /// The source sampling interval
        /// </summary>
        int SourceSamplingInterval { get; set; }

        /// <summary>
        /// The id of the subscription owning the monitored item
        /// </summary>
        uint SubscriptionId { get; set; }

        /// <summary>
        /// The timestamps to return
        /// </summary>
        TimestampsToReturn TimestampsToReturn { get; set; }

        /// <summary>
        /// The type mask
        /// </summary>
        int TypeMask { get; set; }
    }
}
