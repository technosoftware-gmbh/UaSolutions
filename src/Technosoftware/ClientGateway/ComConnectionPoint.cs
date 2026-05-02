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

using Opc.Ua;

using Technosoftware.Common;

#endregion Using Directives

namespace Technosoftware.ClientGateway
{
    /// <summary>
    /// Manages a connection point with a COM server.
    /// </summary>
    /// <exclude />
    internal class ConnectionPoint : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes the object by finding the specified connection point.
        /// </summary>
        public ConnectionPoint(object server, Guid iid)
        {
            Technosoftware.Rcw.IConnectionPointContainer cpc = server as Technosoftware.Rcw.IConnectionPointContainer;

            if (cpc == null)
            {
                throw ServiceResultException.Create(StatusCodes.BadCommunicationError, "Server does not support the IConnectionPointContainer interface.");
            }

            cpc.FindConnectionPoint(ref iid, out m_server);
        }

        /// <summary>
        /// Sets private members to default values.
        /// </summary>
        private void Initialize()
        {
            m_server = null;
            m_cookie = 0;
            m_refs = 0;
        }
        #endregion Constructors

        #region IDisposable Members
        /// <summary>
        /// The finializer implementation.
        /// </summary>
        ~ConnectionPoint()
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
            object server = System.Threading.Interlocked.Exchange(ref m_server, null);

            if (server != null)
            {
                ComUtils.ReleaseServer(server);
            }
        }
        #endregion IDisposable Members

        #region Public Properties
        /// <summary>
        /// The cookie returned by the server.
        /// </summary>
        public int Cookie
        {
            get { return m_cookie; }
        }
        #endregion Public Properties

        #region IConnectionPoint Members
        /// <summary>
        /// Establishes a connection, if necessary and increments the reference count.
        /// </summary>
        public int Advise(object callback)
        {
            if (m_refs++ == 0)
            {
                m_server.Advise(callback, out m_cookie);
            }

            return m_refs;
        }

        /// <summary>
        /// Decrements the reference count and closes the connection if no more references.
        /// </summary>
        public int Unadvise()
        {
            if (--m_refs == 0)
            {
                m_server.Unadvise(m_cookie);
            }

            return m_refs;
        }
        #endregion IConnectionPoint Members

        #region Private Members
        private Technosoftware.Rcw.IConnectionPoint m_server;
        private int m_cookie;
        private int m_refs;
        #endregion Private Members
    }
}
