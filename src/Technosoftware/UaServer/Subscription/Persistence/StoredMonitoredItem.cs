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
#endregion

namespace Technosoftware.UaServer.Subscriptions
{
    /// <inheritdoc/>
    public class StoredMonitoredItem : IUaStoredMonitoredItem
    {
        /// <inheritdoc/>
        public bool IsRestored { get; set; } = false;
        /// <inheritdoc/>
        public uint SubscriptionId { get; set; }
        /// <inheritdoc/>
        public uint Id { get; set; }
        /// <inheritdoc/>
        public int TypeMask { get; set; }
        /// <inheritdoc/>
        public NodeId NodeId { get; set; }
        /// <inheritdoc/>
        public uint AttributeId { get; set; }
        /// <inheritdoc/>
        public string IndexRange { get; set; }
        /// <inheritdoc/>
        public QualifiedName Encoding { get; set; }
        /// <inheritdoc/>
        public DiagnosticsMasks DiagnosticsMasks { get; set; }
        /// <inheritdoc/>
        public TimestampsToReturn TimestampsToReturn { get; set; }
        /// <inheritdoc/>
        public uint ClientHandle { get; set; }
        /// <inheritdoc/>
        public MonitoringMode MonitoringMode { get; set; }
        /// <inheritdoc/>
        public MonitoringFilter OriginalFilter { get; set; }
        /// <inheritdoc/>
        public MonitoringFilter FilterToUse { get; set; }
        /// <inheritdoc/>
        public double Range { get; set; }
        /// <inheritdoc/>
        public double SamplingInterval { get; set; }
        /// <inheritdoc/>
        public uint QueueSize { get; set; }
        /// <inheritdoc/>
        public bool DiscardOldest { get; set; }
        /// <inheritdoc/>
        public int SourceSamplingInterval { get; set; }
        /// <inheritdoc/>
        public bool AlwaysReportUpdates { get; set; }
        /// <inheritdoc/>
        public bool IsDurable { get; set; }
        /// <inheritdoc/>
        public DataValue LastValue { get; set; }
        /// <inheritdoc/>
        public ServiceResult LastError { get; set; }
        /// <inheritdoc/>
        public NumericRange ParsedIndexRange { get; set; }
    }

}
