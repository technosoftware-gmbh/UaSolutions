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
using System.Collections.Generic;
using System.Threading;

using Opc.Ua;

using Technosoftware.UaServer.Diagnostics;
#endregion

namespace Technosoftware.UaServer.Server
{
    /// <summary>
    /// An object that manages requests from within the server.
    /// </summary>
    public class RequestManager : IDisposable
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Initilizes the manager.
        /// </summary>
        /// <param name="server"></param>
        public RequestManager(IUaServerData server)
        {
            if (server == null) throw new ArgumentNullException(nameof(server));

            m_server = server;
            m_requests = new Dictionary<uint, UaServerOperationContext>();
            m_requestTimer = null;
        }
        #endregion

        #region IDisposable Members
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "m_requestTimer")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                List<UaServerOperationContext> operations = null;

                lock (m_requestsLock)
                {
                    operations = new List<UaServerOperationContext>(m_requests.Values);
                    m_requests.Clear();
                }

                foreach (UaServerOperationContext operation in operations)
                {
                    operation.SetStatusCode(StatusCodes.BadSessionClosed);
                }

                Utils.SilentDispose(m_requestTimer);
                m_requestTimer = null;
            }
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Raised when the status of an outstanding request changes.
        /// </summary>
        public event EventHandler<RequestCancelledEventArgs> RequestCancelledEvent
        {
            add
            {
                lock (m_lock)
                {
                    m_RequestCancelled += value;
                }
            }

            remove
            {
                lock (m_lock)
                {
                    m_RequestCancelled -= value;
                }
            }
        }

        /// <summary>
        /// Called when a new request arrives.
        /// </summary>
        /// <param name="context"></param>
        public void RequestReceived(UaServerOperationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            lock (m_requestsLock)
            {
                m_requests.Add(context.RequestId, context);

                if (context.OperationDeadline < DateTime.MaxValue && m_requestTimer == null)
                {
                    m_requestTimer = new Timer(OnTimerExpired, null, 1000, 1000);
                }
            }
        }

        /// <summary>
        /// Called when a request completes (normally or abnormally).
        /// </summary>
        public void RequestCompleted(UaServerOperationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            lock (m_requestsLock)
            {
                // remove the request.
                m_requests.Remove(context.RequestId);
            }
        }

        /// <summary>
        /// Called when the client wishes to cancel one or more requests.
        /// </summary>
        public void CancelRequests(uint requestHandle, out uint cancelCount)
        {
            var cancelledRequests = new List<uint>();

            // flag requests as cancelled.
            lock (m_requestsLock)
            {
                foreach (UaServerOperationContext request in m_requests.Values)
                {
                    if (request.ClientHandle == requestHandle)
                    {
                        request.SetStatusCode(StatusCodes.BadRequestCancelledByRequest);
                        cancelledRequests.Add(request.RequestId);

                        // report the AuditCancelEventType
                        m_server.ReportAuditCancelEvent(request.Session.Id, requestHandle, StatusCodes.Good);
                    }
                }
            }

            // return the number of requests found.
            cancelCount = (uint)cancelledRequests.Count;

            // raise notifications.
            lock (m_lock)
            {
                for (int ii = 0; ii < cancelledRequests.Count; ii++)
                {
                    if (m_RequestCancelled != null)
                    {
                        try
                        {
                            m_RequestCancelled(this,
                                new RequestCancelledEventArgs(cancelledRequests[ii],
                                    StatusCodes.BadRequestCancelledByRequest));
                        }
                        catch (Exception e)
                        {
                            Utils.LogError(e, "Unexpected error reporting RequestCancelled event.");
                        }
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Checks for any expired requests and changes their status.
        /// </summary>
        private void OnTimerExpired(object state)
        {
            List<uint> expiredRequests = new List<uint>();

            // flag requests as expired.
            lock (m_requestsLock)
            {
                // find the completed request.
                bool deadlineExists = false;

                foreach (UaServerOperationContext request in m_requests.Values)
                {
                    if (request.OperationDeadline < DateTime.UtcNow)
                    {
                        request.SetStatusCode(StatusCodes.BadTimeout);
                        expiredRequests.Add(request.RequestId);
                    }
                    else if (request.OperationDeadline < DateTime.MaxValue)
                    {
                        deadlineExists = true;
                    }
                }

                // check if the timer can be cancelled.
                if (m_requestTimer != null && !deadlineExists)
                {
                    m_requestTimer.Dispose();
                    m_requestTimer = null;
                }
            }

            // raise notifications.
            lock (m_lock)
            {
                for (int ii = 0; ii < expiredRequests.Count; ii++)
                {
                    if (m_RequestCancelled != null)
                    {
                        try
                        {
                            m_RequestCancelled(this,
                                new RequestCancelledEventArgs(expiredRequests[ii], StatusCodes.BadTimeout));
                        }
                        catch (Exception e)
                        {
                            Utils.LogError(e, "Unexpected error reporting RequestCancelled event.");
                        }
                    }
                }
            }
        }
        #endregion

        #region Private Fields
        private readonly object m_lock = new object();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private IUaServerData m_server;
        private Dictionary<uint, UaServerOperationContext> m_requests;
        private readonly object m_requestsLock = new object();
        private Timer m_requestTimer;
        private event EventHandler<RequestCancelledEventArgs> m_RequestCancelled;
        #endregion
    }
}
