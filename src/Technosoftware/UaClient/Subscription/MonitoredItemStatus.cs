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
using System;

using Opc.Ua;
#endregion

namespace Technosoftware.UaClient
{
    /// <summary>
    /// The current status of monitored item.
    /// </summary>
    public class MonitoredItemStatus
    {
        #region Constructors
        /// <summary>
        /// Creates a empty object.
        /// </summary>
        internal MonitoredItemStatus()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_id = 0;
            m_nodeId = null;
            m_attributeId = Attributes.Value;
            m_indexRange = null;
            m_encoding = null;
            m_monitoringMode = MonitoringMode.Disabled;
            m_clientHandle = 0;
            m_samplingInterval = 0;
            m_filter = null;
            m_filterResult = null;
            m_queueSize = 0;
            m_discardOldest = true;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// The identifier assigned by the server.
        /// </summary>
        public uint Id { get => m_id; set => m_id = value; }

        /// <summary>
        /// Whether the item has been created on the server.
        /// </summary>
        public bool Created => m_id != 0;

        /// <summary>
        /// Any error condition associated with the monitored item.
        /// </summary>
        public ServiceResult Error => m_error;

        /// <summary>
        /// The node id being monitored.
        /// </summary>
        public NodeId NodeId => m_nodeId;

        /// <summary>
        /// The attribute being monitored.
        /// </summary>
        public uint AttributeId => m_attributeId;

        /// <summary>
        /// The range of array indexes to being monitored.
        /// </summary>
        public string IndexRange => m_indexRange;

        /// <summary>
        /// The encoding to use when returning notifications.
        /// </summary>
        public QualifiedName DataEncoding => m_encoding;

        /// <summary>
        /// The monitoring mode.
        /// </summary>
        public MonitoringMode MonitoringMode => m_monitoringMode;

        /// <summary>
        /// The identifier assigned by the client.
        /// </summary>
        public uint ClientHandle => m_clientHandle;

        /// <summary>
        /// The sampling interval.
        /// </summary>
        public double SamplingInterval => m_samplingInterval;

        /// <summary>
        /// The filter to use to select values to return.
        /// </summary>
        public MonitoringFilter Filter => m_filter;

        /// <summary>
        /// The result of applying the filter
        /// </summary>
        public MonitoringFilterResult FilterResult => m_filterResult;

        /// <summary>
        /// The length of the queue used to buffer values.
        /// </summary>
        public uint QueueSize => m_queueSize;

        /// <summary>
        /// Whether to discard the oldest entries in the queue when it is full.
        /// </summary>
        public bool DiscardOldest => m_discardOldest;
        #endregion

        #region Public Methods
        /// <summary>
        /// Updates the monitoring mode.
        /// </summary>
        public void SetMonitoringMode(MonitoringMode monitoringMode)
        {
            m_monitoringMode = monitoringMode;
        }

        /// <summary>
        /// Updates the object with the results of a translate browse paths request.
        /// </summary>
        internal void SetResolvePathResult(
            BrowsePathResult result,
            ServiceResult error)
        {
            m_error = error;
        }

        /// <summary>
        /// Updates the object with the results of a create monitored item request.
        /// </summary>
        internal void SetCreateResult(
            MonitoredItemCreateRequest request,
            MonitoredItemCreateResult result,
            ServiceResult error)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (result == null) throw new ArgumentNullException(nameof(result));

            m_nodeId = request.ItemToMonitor.NodeId;
            m_attributeId = request.ItemToMonitor.AttributeId;
            m_indexRange = request.ItemToMonitor.IndexRange;
            m_encoding = request.ItemToMonitor.DataEncoding;
            m_monitoringMode = request.MonitoringMode;
            m_clientHandle = request.RequestedParameters.ClientHandle;
            m_samplingInterval = request.RequestedParameters.SamplingInterval;
            m_queueSize = request.RequestedParameters.QueueSize;
            m_discardOldest = request.RequestedParameters.DiscardOldest;
            m_filter = null;
            m_filterResult = null;
            m_error = error;

            if (request.RequestedParameters.Filter != null)
            {
                m_filter = Utils.Clone(request.RequestedParameters.Filter.Body) as MonitoringFilter;
            }

            if (ServiceResult.IsGood(error))
            {
                m_id = result.MonitoredItemId;
                m_samplingInterval = result.RevisedSamplingInterval;
                m_queueSize = result.RevisedQueueSize;

                if (result.FilterResult != null)
                {
                    m_filterResult = Utils.Clone(result.FilterResult.Body) as MonitoringFilterResult;
                }
            }
        }

        /// <summary>
        /// Updates the object with the results of a transfer monitored item request.
        /// </summary>
        internal void SetTransferResult(MonitoredItem monitoredItem)
        {
            if (monitoredItem == null) throw new ArgumentNullException(nameof(monitoredItem));

            m_nodeId = monitoredItem.ResolvedNodeId;
            m_attributeId = monitoredItem.AttributeId;
            m_indexRange = monitoredItem.IndexRange;
            m_encoding = monitoredItem.Encoding;
            m_monitoringMode = monitoredItem.MonitoringMode;
            m_clientHandle = monitoredItem.ClientHandle;
            m_samplingInterval = monitoredItem.SamplingInterval;
            m_queueSize = monitoredItem.QueueSize;
            m_discardOldest = monitoredItem.DiscardOldest;
            m_filter = null;
            m_filterResult = null;

            if (monitoredItem.Filter != null)
            {
                m_filter = Utils.Clone(monitoredItem.Filter) as MonitoringFilter;
            }
        }

        /// <summary>
        /// Updates the object with the results of a modify monitored item request.
        /// </summary>
        internal void SetModifyResult(
            MonitoredItemModifyRequest request,
            MonitoredItemModifyResult result,
            ServiceResult error)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (result == null) throw new ArgumentNullException(nameof(result));

            m_error = error;

            if (ServiceResult.IsGood(error))
            {
                m_clientHandle = request.RequestedParameters.ClientHandle;
                m_samplingInterval = request.RequestedParameters.SamplingInterval;
                m_queueSize = request.RequestedParameters.QueueSize;
                m_discardOldest = request.RequestedParameters.DiscardOldest;
                m_filter = null;
                m_filterResult = null;

                if (request.RequestedParameters.Filter != null)
                {
                    m_filter = Utils.Clone(request.RequestedParameters.Filter.Body) as MonitoringFilter;
                }

                m_samplingInterval = result.RevisedSamplingInterval;
                m_queueSize = result.RevisedQueueSize;

                if (result.FilterResult != null)
                {
                    m_filterResult = Utils.Clone(result.FilterResult.Body) as MonitoringFilterResult;
                }
            }
        }

        /// <summary>
        /// Updates the object with the results of a delete item request.
        /// </summary>
        internal void SetDeleteResult(ServiceResult error)
        {
            m_id = 0;
            m_error = error;
        }

        /// <summary>
        /// Sets the error state for the monitored item status.
        /// </summary>
        internal void SetError(ServiceResult error)
        {
            m_error = error;
        }
        #endregion

        #region Private Fields
        private uint m_id;
        private ServiceResult m_error;
        private NodeId m_nodeId;
        private uint m_attributeId;
        private string m_indexRange;
        private QualifiedName m_encoding;
        private MonitoringMode m_monitoringMode;
        private uint m_clientHandle;
        private double m_samplingInterval;
        private MonitoringFilter m_filter;
        private MonitoringFilterResult m_filterResult;
        private uint m_queueSize;
        private bool m_discardOldest;
        #endregion
    }
}
