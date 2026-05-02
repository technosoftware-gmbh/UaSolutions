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
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Threading;
using System.Xml;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Opc.Ua.Test;
using BrowseNames = Opc.Ua.BrowseNames;
using ObjectIds = Opc.Ua.ObjectIds;
using ReferenceTypes = Opc.Ua.ReferenceTypes;
using Range = Opc.Ua.Range;
using Technosoftware.Common;
using Technosoftware.UaServer;
#endregion Using Directives

namespace Technosoftware.ClientGateway
{
    /// <summary>
    /// A node manager for a server that exposes several variables.
    /// </summary>
    /// <exclude />
    public class ComClientNodeManager : UaStandardNodeManager
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Initializes the node manager.
        /// </summary>
        public ComClientNodeManager(
            IUaServerData server,
            string namespaceUri,
            bool ownsTypeModel)
            : base(
                  server,
                  server.Telemetry.CreateLogger<ComClientNodeManager>())
        {
            // check if this node manager owns the COM Interop type model.
            if (ownsTypeModel)
            {
                SetNamespaces(namespaceUri, Technosoftware.Common.Namespaces.ComInterop);
            }
            else
            {
                SetNamespaces(namespaceUri);
            }
        }
        #endregion Constructors, Destructor, Initialization

        #region IDisposable Members
        /// <summary>
        /// An overrideable version of the Dispose.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Utils.SilentDispose(m_metadataUpdateTimer);
                m_metadataUpdateTimer = null;
            }

            base.Dispose(disposing);
        }
        #endregion IDisposable Members

        #region Protected Methods
        /// <summary>
        /// Updates the type cache.
        /// </summary>
        protected void StartMetadataUpdates(WaitCallback callback, object callbackData, int initialDelay, int period)
        {
            lock (Lock)
            {
                if (m_metadataUpdateTimer != null)
                {
                    m_metadataUpdateTimer.Dispose();
                    m_metadataUpdateTimer = null;
                }

                m_metadataUpdateCallback = callback;
                m_metadataUpdateTimer = new Timer(DoMetadataUpdate, callbackData, initialDelay, period);
            }
        }
        #endregion Protected Methods

        #region Private Methods
        /// <summary>
        /// Updates the metadata cached for the server.
        /// </summary>
        private void DoMetadataUpdate(object state)
        {
            try
            {
                if (!ServerData.IsRunning)
                {
                    return;
                }

                ComClientManager system = (ComClientManager)SystemContext.SystemHandle;
                ComClient client = (ComClient)system.SelectClient(SystemContext, true);

                int[] availableLocales = client.QueryAvailableLocales();

                if (availableLocales != null)
                {
                    lock (Lock)
                    {
                        // check if the server is running.
                        if (!ServerData.IsRunning)
                        {
                            return;
                        }

                        // get the LocaleIdArray property.
                        BaseVariableState localeArray = ServerData.DiagnosticsNodeManager
                            .Find(Opc.Ua.VariableIds.Server_ServerCapabilities_LocaleIdArray) as BaseVariableState;

                        List<string> locales = new List<string>();

                        // preserve any existing locales.
                        string[] existingLocales = localeArray.Value as string[];

                        if (existingLocales != null)
                        {
                            locales.AddRange(existingLocales);
                        }

                        for (int ii = 0; ii < availableLocales.Length; ii++)
                        {
                            if (availableLocales[ii] == 0 || availableLocales[ii] == ComUtils.LOCALE_SYSTEM_DEFAULT ||
                                availableLocales[ii] == ComUtils.LOCALE_USER_DEFAULT)
                            {
                                continue;
                            }

                            try
                            {
                                System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo(availableLocales[ii]);

                                if (!locales.Contains(culture.Name))
                                {
                                    locales.Add(culture.Name);
                                }
                            }
                            catch (Exception e)
                            {
                                m_logger.LogError(
                                    Utils.TraceMasks.Error,
                                    e,
                                    "Can't process an invalid locale id: {0:X4}.", availableLocales[ii]);
                            }
                        }

                        localeArray.Value = locales.ToArray();
                    }
                }

                // invoke callback.
                m_metadataUpdateCallback?.Invoke(state);
            }
            catch (Exception e)
            {
                m_logger.LogError(
                    Utils.TraceMasks.Error,
                    e,
                   "Unexpected error updating server metadata.");
            }
        }
        #endregion Private Methods

        #region Private Fields
        private Timer m_metadataUpdateTimer;
        private WaitCallback m_metadataUpdateCallback;
        #endregion Private Fields
    }
}
