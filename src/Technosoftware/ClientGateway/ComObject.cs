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
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Technosoftware.Common;
#endregion Using Directives

namespace Technosoftware.ClientGateway
{
    /// <summary>
    /// A base class for COM server wrappers.
    /// </summary>
    /// <exclude />
    public class ComObject : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes the object.
        /// </summary>
        public ComObject(ITelemetryContext telemetry)
        {
            m_telemetry = telemetry;
            m_logger = telemetry.CreateLogger<ComObject>();
        }
        #endregion Constructors

        #region IDisposable Members
        /// <summary>
        /// The finializer implementation.
        /// </summary>
        ~ComObject()
        {
            Dispose(false);
        }

        /// <summary>
        /// Frees any unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// An overrideable version of the Dispose.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            // release references to the server during garbage collection.
            if (!disposing)
            {
                ReleaseServer();
            }

            // clean up managed objects if 
            if (disposing)
            {
                lock (m_lock)
                {
                    m_disposed = true;

                    // only release server if there are no outstanding calls.
                    // if it is not released here it will be released when the last call completes.
                    if (m_outstandingCalls <= 0)
                    {
                        ReleaseServer();
                    }
                }
            }
        }
        #endregion IDisposable Members

        #region Public Properties
        /// <summary>
        /// Gets an object which is used to synchronize access to the COM object.
        /// </summary>
        /// <value>An object which is used to synchronize access to the COM object.</value>
        public object Lock
        {
            get { return m_lock; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ComObject"/> is disposed.
        /// </summary>
        /// <value><c>true</c> if disposed; otherwise, <c>false</c>.</value>
        public bool Disposed
        {
            get { return m_disposed; }
        }

        /// <summary>
        /// Gets or sets the COM server.
        /// </summary>
        /// <value>The COM server.</value>
        public object Unknown
        {
            get { return m_unknown; }
            set { m_unknown = value; }
        }
        #endregion Public Properties

        #region Protected Members
        /// <summary>
        /// Reports an unexpected exception during a COM operation. 
        /// </summary>
        protected void TraceComError(Exception e, string format, params object[] args)
        {
            string message = Utils.Format(format, args);

            int code = Marshal.GetHRForException(e);

            string error = ResultIds.GetBrowseName(code);

            if (error == null)
            {
                m_logger.LogError(e, message);
                return;
            }

            m_logger.LogError(e, "{Error}: {Message}", error, message);
        }

        /// <summary>
        /// Releases all references to the server.
        /// </summary>
        protected virtual void ReleaseServer()
        {
            lock (m_lock)
            {
                ComUtils.ReleaseServer(m_unknown);
                m_unknown = null;
            }
        }

        /// <summary>
        /// Checks if the server supports the specified interface.
        /// </summary>
        /// <typeparam name="T">The interface to check.</typeparam>
        /// <returns>True if the server supports the interface.</returns>
        protected bool SupportsInterface<T>() where T : class
        {
            lock (m_lock)
            {
                return m_unknown is T;
            }
        }

        /// <summary>
        /// Must be called before any COM call.
        /// </summary>
        /// <typeparam name="T">The interface to used when making the call.</typeparam>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="throwOnError">if set to <c>true</c> an exception is thrown on error.</param>
        /// <returns></returns>
        protected T BeginComCall<T>(string methodName, bool throwOnError) where T : class
        {
            m_logger.LogInformation(Utils.TraceMasks.ExternalSystem, "{0} called.", methodName);

            lock (m_lock)
            {
                m_outstandingCalls++;

                if (m_disposed)
                {
                    if (throwOnError)
                    {
                        throw new ObjectDisposedException("The COM server has been disposed.");
                    }

                    return null;
                }

                T server = m_unknown as T;

                if (throwOnError && server == null)
                {
                    throw new NotSupportedException(Utils.Format("COM interface '{0}' is not supported by server.", typeof(T).Name));
                }

                return server;
            }
        }

        /// <summary>
        /// Must called if a COM call returns an unexpected exception.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="e">The exception.</param>
        /// <remarks>Note that some COM calls are expected to return errors.</remarks>
        protected void ComCallError(string methodName, Exception e)
        {
            TraceComError(e, methodName);
        }

        /// <summary>
        /// Must be called in the finally block after making a COM call.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        protected void EndComCall(string methodName)
        {
            m_logger.LogInformation(Utils.TraceMasks.ExternalSystem, "{0} completed.", methodName);

            lock (m_lock)
            {
                m_outstandingCalls--;

                if (m_disposed && m_outstandingCalls <= 0)
                {
                    ComUtils.ReleaseServer(m_unknown);
                }
            }
        }
        #endregion Protected Members

        #region Private Fields
        private object m_lock = new object();
        private int m_outstandingCalls;
        private bool m_disposed;
        private object m_unknown;
        private readonly ILogger m_logger;
        private readonly ITelemetryContext m_telemetry;
        #endregion Private Fields
    }
}
