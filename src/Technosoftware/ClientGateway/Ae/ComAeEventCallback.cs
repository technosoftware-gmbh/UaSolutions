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
using Technosoftware.ClientGateway.Da;
using Technosoftware.Rcw;
#endregion Using Directives

namespace Technosoftware.ClientGateway.Ae
{
    /// <summary>
    /// A class that implements the IOPCEventSink interface.
    /// </summary>
    /// <exclude />
    internal class ComAeEventSink : Technosoftware.Rcw.IOPCEventSink, IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes the object with the containing subscription object.
        /// </summary>
        public ComAeEventSink(ComAeSubscriptionClient subscription, ITelemetryContext telemetry)
        {
            // save group.
            m_subscription = subscription;

            // create connection point.
            m_connectionPoint = new ConnectionPoint(subscription.Unknown, typeof(IOPCEventSink).GUID);

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

        /// <summary>
        /// Gets the cookie returned by the server.
        /// </summary>
        /// <value>The cookie.</value>
        public int Cookie
        {
            get
            {
                if (m_connectionPoint != null)
                {
                    return m_connectionPoint.Cookie;
                }

                return 0;
            }
        }
        #endregion Public Properties

        #region ComAeEventSink Members
        /// <summary>
        /// Called when one or events are produce by the server.
        /// </summary>
        public void OnEvent(
            int hClientSubscription,
            int bRefresh,
            int bLastRefresh,
            int dwCount,
            ONEVENTSTRUCT[] pEvents)
        {
            try
            {
                if (bRefresh == 0)
                {
                    m_subscription.OnEvent(pEvents);
                }
                else
                {
                    m_subscription.OnRefresh(pEvents, bLastRefresh != 0);
                }
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error processing OnEvent callback.");
            }
        }
        #endregion ComAeEventSink Members

        #region Private Members
        private ComAeSubscriptionClient m_subscription;
        private ConnectionPoint m_connectionPoint;
        private readonly ILogger m_logger;
        private readonly ITelemetryContext m_telemetry;
        #endregion Private Members
    }
}
