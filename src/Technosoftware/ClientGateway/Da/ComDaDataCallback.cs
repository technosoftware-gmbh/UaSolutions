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
#endregion Using Directives

namespace Technosoftware.ClientGateway.Da
{
    /// <summary>
    /// A class that implements the IOPCDataCallback interface.
    /// </summary>
    /// <exclude />
    internal class ComDaDataCallback : Rcw.IOPCDataCallback, IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes the object with the containing subscription object.
        /// </summary>
        public ComDaDataCallback(ComDaGroup group, ITelemetryContext telemetry)
        {
            // save group.
            m_group = group;

            // create connection point.
            m_connectionPoint = new ConnectionPoint(group.Unknown, typeof(Rcw.IOPCDataCallback).GUID);

            // advise.
            m_connectionPoint.Advise(this);

            m_telemetry = telemetry;
            m_logger = telemetry.CreateLogger<ComDaDataCallback>();
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
        /// Called when a data changed event is received.
        /// </summary>
        public void OnDataChange(
            int dwTransid,
            int hGroup,
            int hrMasterquality,
            int hrMastererror,
            int dwCount,
            int[] phClientItems,
            object[] pvValues,
            short[] pwQualities,
            System.Runtime.InteropServices.ComTypes.FILETIME[] pftTimeStamps,
            int[] pErrors)
        {
            try
            {
                // unmarshal item values.
                DaValue[] values = ComDaGroup.GetItemValues(
                    dwCount,
                    pvValues,
                    pwQualities,
                    pftTimeStamps,
                    pErrors);

                // invoke the callback.
                m_group.OnDataChange(phClientItems, values);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error processing OnDataChange callback.");
            }
        }

        /// <summary>
        /// Called when an asynchronous read operation completes.
        /// </summary>
        public void OnReadComplete(
            int dwTransid,
            int hGroup,
            int hrMasterquality,
            int hrMastererror,
            int dwCount,
            int[] phClientItems,
            object[] pvValues,
            short[] pwQualities,
            System.Runtime.InteropServices.ComTypes.FILETIME[] pftTimeStamps,
            int[] pErrors)
        {
            try
            {
                // unmarshal item values.
                DaValue[] values = ComDaGroup.GetItemValues(
                    dwCount,
                    pvValues,
                    pwQualities,
                    pftTimeStamps,
                    pErrors);

                // invoke the callback.
                m_group.OnReadComplete(dwTransid, phClientItems, values);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error processing OnReadComplete callback.");
            }
        }

        /// <summary>
        /// Called when an asynchronous write operation completes.
        /// </summary>
        public void OnWriteComplete(
            int dwTransid,
            int hGroup,
            int hrMastererror,
            int dwCount,
            int[] phClientItems,
            int[] pErrors)
        {
            try
            {
                m_group.OnWriteComplete(dwTransid, phClientItems, pErrors);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error processing OnWriteComplete callback.");
            }
        }

        /// <summary>
        /// Called when an asynchronous operation is cancelled.
        /// </summary>
        public void OnCancelComplete(
            int dwTransid,
            int hGroup)
        {
        }
        #endregion IOPCDataCallback Members

        #region Private Members
        private ComDaGroup m_group;
        private ConnectionPoint m_connectionPoint;
        private readonly ILogger m_logger;
        private readonly ITelemetryContext m_telemetry;
        #endregion Private Members
    }
}
