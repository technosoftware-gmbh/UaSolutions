#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
// Web: http://www.technosoftware.com
//
// The Software is based on the OPC Foundation’s software and is subject to 
// the OPC Foundation MIT License 1.00, which can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//
// The Software is subject to the Technosoftware GmbH Software License Agreement,
// which can be found here:
// https://technosoftware.com/license-agreement/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Technosoftware.Rcw;
#endregion Using Directives

namespace Technosoftware.ClientGateway.Hda
{
    /// <summary>
    /// A class that implements the IOPCHDA_DataCallback interface.
    /// </summary>
    /// <exclude />
    internal class ComHdaDataCallback : Technosoftware.Rcw.IOPCHDA_DataCallback, IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes the object with the containing subscription object.
        /// </summary>
        public ComHdaDataCallback(ComHdaClient server, ITelemetryContext telemetry)
        {
            // save group.
            m_server = server;
            m_telemetry = telemetry;
            m_logger = m_telemetry.CreateLogger<ComHdaDataCallback>();

            // create connection point.
            m_connectionPoint = new ConnectionPoint(server.Unknown, typeof(Technosoftware.Rcw.IOPCHDA_DataCallback).GUID);

            // advise.
            m_connectionPoint.Advise(this);
        }
        #endregion Constructors

        #region IDisposable Members        
        /// <summary>
        /// Frees any unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// An overrideable version of the Dispose.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (m_connectionPoint != null)
            {
                if (disposing)
                {
                    m_connectionPoint.Dispose();
                    m_connectionPoint = null;
                }
            }
        }
        #endregion IDisposable Members        

        #region Public Properties
        /// <summary>
        /// Whether the callback is connected.
        /// </summary>
        public bool Connected
        {
            get
            {
                return m_connectionPoint != null;
            }
        }
        #endregion Public Properties

        #region IOPCDataCallback Members
        /// <summary>
        /// Called when a data change arrives.
        /// </summary>
        public void OnDataChange(
            int dwTransactionID,
            int hrStatus,
            int dwNumItems,
            OPCHDA_ITEM[] pItemValues,
            int[] phrErrors)
        {
            try
            {
                // TBD
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error processing OnDataChange callback.");
            }
        }

        /// <summary>
        /// Called when an async read completes.
        /// </summary>
        public void OnReadComplete(
            int dwTransactionID,
            int hrStatus,
            int dwNumItems,
            OPCHDA_ITEM[] pItemValues,
            int[] phrErrors)
        {
            try
            {
                // TBD
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error processing OnReadComplete callback.");
            }
        }

        /// <summary>
        /// Called when an async read modified completes.
        /// </summary>
        public void OnReadModifiedComplete(
            int dwTransactionID,
            int hrStatus,
            int dwNumItems,
            OPCHDA_MODIFIEDITEM[] pItemValues,
            int[] phrErrors)
        {
            try
            {
                // TBD
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error processing OnReadModifiedComplete callback.");
            }
        }

        /// <summary>
        /// Called when an async read attributes completes.
        /// </summary>
        public void OnReadAttributeComplete(
            int dwTransactionID,
            int hrStatus,
            int hClient,
            int dwNumItems,
            OPCHDA_ATTRIBUTE[] pAttributeValues,
            int[] phrErrors)
        {
            try
            {
                // TBD
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error processing OnReadAttributeComplete callback.");
            }
        }

        /// <summary>
        /// Called when an async read annotations completes.
        /// </summary>
        public void OnReadAnnotations(
            int dwTransactionID,
            int hrStatus,
            int dwNumItems,
            OPCHDA_ANNOTATION[] pAnnotationValues,
            int[] phrErrors)
        {
            try
            {
                // TBD
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error processing OnReadAnnotations callback.");
            }
        }

        /// <summary>
        /// Called when an async insert annotations completes.
        /// </summary>
        public void OnInsertAnnotations(
            int dwTransactionID,
            int hrStatus,
            int dwCount,
            int[] phClients,
            int[] phrErrors)
        {
            try
            {
                // TBD
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error processing OnInsertAnnotations callback.");
            }
        }

        /// <summary>
        /// Called when a playback result arrives.
        /// </summary>
        public void OnPlayback(
            int dwTransactionID,
            int hrStatus,
            int dwNumItems,
            IntPtr ppItemValues,
            int[] phrErrors)
        {
            try
            {
                // TBD
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error processing OnPlayback callback.");
            }
        }

        /// <summary>
        /// Called when a async update completes.
        /// </summary>
        public void OnUpdateComplete(
            int dwTransactionID,
            int hrStatus,
            int dwCount,
            int[] phClients,
            int[] phrErrors)
        {
            try
            {
                // TBD
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error processing OnUpdateComplete callback.");
            }
        }

        /// <summary>
        /// Called when a async opeartion cancel completes.
        /// </summary>
        public void OnCancelComplete(
            int dwCancelID)
        {
            try
            {
                // TBD
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error processing OnCancelComplete callback.");
            }
        }
        #endregion IOPCDataCallback Members

        #region Private Members
        private ComHdaClient m_server;
        private ConnectionPoint m_connectionPoint;
        private readonly ILogger m_logger;
        private readonly ITelemetryContext m_telemetry;
        #endregion Private Members
    }
}
