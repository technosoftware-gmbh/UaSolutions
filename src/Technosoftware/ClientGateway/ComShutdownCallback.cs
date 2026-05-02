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
using System.Runtime.Remoting.Contexts;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Technosoftware.ClientGateway.Ae;
#endregion Using Directives

namespace Technosoftware.ClientGateway
{
    /// <summary>
    /// A class that implements the IOPCShutdown interface.
    /// </summary>
    internal class ShutdownCallback : Rcw.IOPCShutdown, IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes the object with the containing subscription object.
        /// </summary>
        public ShutdownCallback(object server, ServerShutdownEventHandler handler, ITelemetryContext telemetry)
        {
            try
            {
                m_server = server;
                m_handler = handler;
                m_telemetry = telemetry;
                m_logger = m_telemetry.CreateLogger<ShutdownCallback>();

                // create connection point.
                m_connectionPoint = new ConnectionPoint(server, typeof(Technosoftware.Rcw.IOPCShutdown).GUID);

                // advise.
                m_connectionPoint.Advise(this);
            }
            catch (Exception e)
            {
                throw new ServiceResultException(e, StatusCodes.BadOutOfService);
            }
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
                m_connectionPoint.Dispose();
                m_connectionPoint = null;
            }
        }
        #endregion IDisposable Members

        #region IOPCShutdown Members
        /// <summary>
        /// Called when a data changed event is received.
        /// </summary>
        public void ShutdownRequest(string szReason)
        {
            try
            {
                m_handler?.Invoke(m_server, szReason);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error processing callback.");
            }
        }
        #endregion IOPCShutdown Members

        #region Private Members
        private object m_server;
        private ServerShutdownEventHandler m_handler;
        private ConnectionPoint m_connectionPoint;
        private readonly ILogger m_logger;
        private readonly ITelemetryContext m_telemetry;
        #endregion Private Members
    }

    /// <summary>
    /// A delegate used to receive server shutdown events.
    /// </summary>
    public delegate void ServerShutdownEventHandler(object sender, string reason);
}
