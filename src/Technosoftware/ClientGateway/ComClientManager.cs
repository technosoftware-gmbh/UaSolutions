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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Technosoftware.Common;
using Technosoftware.UaServer;
#endregion Using Directives

namespace Technosoftware.ClientGateway
{
    /// <summary>
    /// Manages the COM connections used by the UA server.
    /// </summary>
    /// <exclude />
    internal class ComClientManager : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ComClientManager"/> class.
        /// </summary>
        public ComClientManager(ITelemetryContext telemetry)
        {
            m_telemetry = telemetry;
            m_logger = telemetry.CreateLogger<ComClientManager>();
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
            if (disposing)
            {
                Utils.SilentDispose(m_statusTimer);
                m_statusTimer = null;

                Utils.SilentDispose(m_defaultClient);
                m_defaultClient = null;
            }
        }
        #endregion IDisposable Members

        #region Public Members
        /// <summary>
        /// Selects the COM client to use for the current context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="useDefault">The whether to use the default context.</param>
        /// <returns>A COM client instance.</returns>
        public virtual ComClient SelectClient(UaServerContext context, bool useDefault)
        {
            if (useDefault)
            {
                return DefaultClient;
            }

            return GetLocalizedClient(context.UserIdentity, context.PreferredLocales);
        }

        /// <summary>
        /// Initializes the manager by creating the default instance.
        /// </summary>
        public void Initialize(
            UaServerContext context,
            ComClientConfiguration configuration,
            ComServerStatusState statusNode,
            object statusNodeLock,
            WaitCallback reconnectCallback)
        {
            m_defaultSystemContext = context;
            m_configuration = configuration;
            m_statusNode = statusNode;
            m_statusNodeLock = statusNodeLock;
            m_statusUpdateInterval = m_configuration.MaxReconnectWait;
            m_reconnectCallback = reconnectCallback;

            // limit status updates to once per 10 seconds.
            if (m_statusUpdateInterval < 10000)
            {
                m_statusUpdateInterval = 10000;
            }

            StartStatusTimer(OnStatusTimerExpired);
        }

        /// <summary>
        /// Returns a localized client based on the preferred locales.
        /// </summary>
        /// <param name="identity">The identity to use.</param>
        /// <param name="preferredLocales">The locales to use.</param>
        /// <returns>A localized client.</returns>
        public ComClient GetLocalizedClient(IUserIdentity identity, IList<string> preferredLocales)
        {
            return GetLocalizedClient(identity, ComClient.SelectLocaleId(AvailableLocaleIds, preferredLocales));
        }

        /// <summary>
        /// Returns a localized client for the specified locale id.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="localeId">The locales id.</param>
        /// <returns>A localized client.</returns>
        public ComClient GetLocalizedClient(IUserIdentity identity, int localeId)
        {
            // check if a logon is required.
            string userName = null;

            if (identity != null && identity.TokenType == UserTokenType.UserName)
            {
                UserIdentityToken token = identity.GetIdentityToken();
                if (token is not null and UserNameIdentityToken)
                {
#pragma warning disable RCS1202 
                    userName = (token as UserNameIdentityToken).UserName;
#pragma warning restore RCS1202 
                }
            }

            if (String.IsNullOrEmpty(userName) && localeId == ComUtils.LOCALE_SYSTEM_DEFAULT)
            {
                m_logger.LogInformation("COM Client Selected: DEFAULT (no match for locale)");
                return DefaultClient;
            }

            // create the key.
            StringBuilder buffer = new StringBuilder();
            buffer.Append(localeId);

            if (!String.IsNullOrEmpty(userName))
            {
                buffer.Append(':');
                buffer.Append(userName);
            }

            string key = buffer.ToString();

            if (m_localizedClients == null)
            {
                m_localizedClients = new Dictionary<string, ComClient>();
            }

            ComClient client = null;

            if (!m_localizedClients.TryGetValue(key, out client))
            {
                client = CreateClient();
                client.Key = key;
                client.LocaleId = localeId;
                client.UserIdentity = identity;
                client.CreateInstance();
                m_localizedClients[key] = client;
            }

            // Utils.Trace("COM Client Seleted: {0}", key);
            return client;
        }
        #endregion Public Members

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
        /// Gets the default system context.
        /// </summary>
        /// <value>The default system context.</value>
        protected UaServerContext DefaultSystemContext
        {
            get { return m_defaultSystemContext; }
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        protected ComClientConfiguration Configuration
        {
            get { return m_configuration; }
        }

        /// <summary>
        /// Gets or sets the default COM client instance.
        /// </summary>
        /// <value>The default client.</value>
        protected ComClient DefaultClient
        {
            get { return m_defaultClient; }
            set { m_defaultClient = value; }
        }

        /// <summary>
        /// The locale ids supported by the server.
        /// </summary>
        protected int[] AvailableLocaleIds { get; private set; }

        /// <summary>
        /// Gets the status node.
        /// </summary>
        /// <value>The status node.</value>
        protected ComServerStatusState StatusNode
        {
            get { return m_statusNode; }
        }

        /// <summary>
        /// Gets the synchronization object for the status node.
        /// </summary>
        /// <value>The ynchronization object for the status node.</value>
        protected object StatusNodeLock
        {
            get { return m_statusNodeLock; }
        }

        /// <summary>
        /// Starts the status timer.
        /// </summary>
        /// <param name="callback">The callback to use when the timer expires.</param>
        protected void StartStatusTimer(TimerCallback callback)
        {
            if (m_statusTimer != null)
            {
                m_statusTimer.Dispose();
                m_statusTimer = null;
            }

            m_statusTimer = new Timer(callback, null, 0, m_statusUpdateInterval);
        }

        /// <summary>
        /// Creates a new client object.
        /// </summary>
        protected virtual ComClient CreateClient()
        {
            return null;
        }

        /// <summary>
        /// Updates the status node.
        /// </summary>
        /// <returns>True if the update was successful.</returns>
        protected virtual bool UpdateStatus()
        {
            return false;
        }
        #endregion Protected Members

        #region Private Methods
        /// <summary>
        /// Called when thes status timer expires.
        /// </summary>
        /// <param name="state">The state.</param>
        private void OnStatusTimerExpired(object state)
        {
            try
            {
                // create the client if it has not already been created.
                if (m_defaultClient == null)
                {
                    m_defaultClient = CreateClient();
                    m_defaultClient.Key = String.Empty;

                    // set default locale.
                    if (AvailableLocaleIds != null && AvailableLocaleIds.Length > 0)
                    {
                        m_defaultClient.LocaleId = AvailableLocaleIds[0];
                    }
                    else
                    {
                        m_defaultClient.LocaleId = ComUtils.LOCALE_SYSTEM_DEFAULT;
                    }

                    m_defaultClient.CreateInstance();
                    m_lastStatusUpdate = DateTime.UtcNow;
                }

                // check if the last status updates appear to be hanging.
                bool reconnected = false;

                if (m_lastStatusUpdate.AddMilliseconds(m_statusUpdateInterval * 1.1) < DateTime.UtcNow)
                {
                    if (m_reconnecting)
                    {
                        return;
                    }

                    m_reconnecting = true;

                    try
                    {
                        m_logger.LogInformation("Communication with COM server failed. Disposing server and reconnecting.");

                        // dispose existing client in the background in case it blocks.
                        ThreadPool.QueueUserWorkItem(OnDisposeClient, m_defaultClient);

                        // dispose localized clients.
                        if (m_localizedClients != null)
                        {
                            foreach (ComClient localizedClient in m_localizedClients.Values)
                            {
                                ThreadPool.QueueUserWorkItem(OnDisposeClient, localizedClient);
                            }

                            m_localizedClients.Clear();
                        }

                        // create a new client.
                        m_defaultClient = CreateClient();
                        m_defaultClient.CreateInstance();

                        reconnected = true;
                    }
                    finally
                    {
                        m_reconnecting = false;
                    }
                }

                // fetches the status from the server and updates the status node. 
                if (UpdateStatus())
                {
                    AvailableLocaleIds = m_defaultClient.QueryAvailableLocales();
                    m_lastStatusUpdate = DateTime.UtcNow;

                    if (reconnected && m_reconnectCallback != null)
                    {
                        m_reconnectCallback(this);
                    }
                }
            }
            catch (Exception e)
            {
                TraceComError(e, "Error fetching status from the COM server.");
            }
        }

        /// <summary>
        /// Called when a misbehaving client object needs to be disposed.
        /// </summary>
        /// <param name="state">The client object.</param>
        private void OnDisposeClient(object state)
        {
            Utils.SilentDispose(state);
        }
        #endregion Private Methods

        #region Private Fields
        private ComClientConfiguration m_configuration;
        private UaServerContext m_defaultSystemContext;
        private ComServerStatusState m_statusNode;
        private object m_statusNodeLock;
        private ComClient m_defaultClient;
        private Timer m_statusTimer;
        private DateTime m_lastStatusUpdate;
        private int m_statusUpdateInterval;
        private WaitCallback m_reconnectCallback;
        private bool m_reconnecting;
        private Dictionary<string, ComClient> m_localizedClients;
        private readonly ILogger m_logger;
        private readonly ITelemetryContext m_telemetry;
        #endregion Private Fields
    }
}
