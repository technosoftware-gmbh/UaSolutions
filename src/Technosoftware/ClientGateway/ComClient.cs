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
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Technosoftware.ClientGateway.Ae;
using Technosoftware.Common;
using Technosoftware.Rcw;
#endregion Using Directives

namespace Technosoftware.ClientGateway
{
    /// <summary>
    /// Provides access to a COM server.
    /// </summary>
    /// <exclude />
    internal class ComClient : ComObject
    {
        #region Constructors
        /// <summary>
        /// Initializes the object with the ProgID of the server being accessed.
        /// </summary>
        public ComClient(ComClientConfiguration configuration, ITelemetryContext telemetry) : base(telemetry)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            m_telemetry = telemetry;
            m_logger = telemetry.CreateLogger<ComClient>();
            m_url = configuration.ServerUrl;
        }
        #endregion Constructors

        #region Public Methods
        /// <summary>
        /// A key that combines the user name and locale id.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The locale id associated with the instance.
        /// </summary>
        public int LocaleId { get; set; }

        /// <summary>
        /// The user identity associated with the instance.
        /// </summary>
        public IUserIdentity UserIdentity { get; set; }

        public ITelemetryContext Telemetry { get; }

        /// <summary>
        /// Creates an instance of the COM server.
        /// </summary>
        public void CreateInstance()
        {
            // multiple calls are not allowed - may block for a while due to network operation.
            lock (m_lock)
            {
                ServerFactory factory = new ServerFactory();

                try
                {
                    // create the server.
                    Unknown = factory.CreateServer(new Uri(m_url), null);

                    // set the locale.
                    SetLocale(LocaleId);

                    if (UserIdentity != null)
                    {
                        SetUserIdentity(UserIdentity);
                    }

                    // do any post-connect processing.
                    OnConnected();
                }
                catch (Exception e)
                {
                    TraceComError(e, "Could not connect to server ({0}).", m_url);

                    // cleanup on error.
                    Close();
                }
                finally
                {
                    factory.Dispose();
                }
            }
        }

        /// <summary>
        /// Fetches the error string from the server.
        /// </summary>
        public string GetErrorString(int error)
        {
            string methodName = "IOPCCommon.GetErrorString";

            try
            {
                IOPCCommon server = BeginComCall<IOPCCommon>(methodName, true);
                string ppString = null;
                server.GetErrorString(error, out ppString);
                return ppString;
            }
            catch (Exception e)
            {
                ComCallError(methodName, e);
                return null;
            }
            finally
            {
                EndComCall(methodName);
            }
        }

        /// <summary>
        /// Sets the current locale.
        /// </summary>
        public void SetLocale(int localeId)
        {
            string methodName = "IOPCCommon.SetLocaleID";

            try
            {
                IOPCCommon server = BeginComCall<IOPCCommon>(methodName, true);
                server.SetLocaleID(localeId);
            }
            catch (Exception e)
            {
                ComCallError(methodName, e);
            }
            finally
            {
                EndComCall(methodName);
            }
        }

        /// <summary>
        /// Sets the current user identity.
        /// </summary>
        public void SetUserIdentity(IUserIdentity identity)
        {
            string methodName = "IOPCSecurityPrivate.Logon";

            try
            {
                IOPCSecurityPrivate server = BeginComCall<IOPCSecurityPrivate>(methodName, true);

                if (server != null)
                {
                    int bAvailable = 0;
                    server.IsAvailablePriv(out bAvailable);

                    if (bAvailable != 0)
                    {
                        bool logoff = true;

                        if (identity != null && identity.TokenType == UserTokenType.UserName)
                        {
                            UserNameIdentityToken identityToken = identity.GetIdentityToken() as UserNameIdentityToken;

                            if (identityToken != null)
                            {
                                server.Logon(identityToken.UserName, identityToken.Password.ToString());
                                logoff = false;
                            }
                        }

                        if (logoff)
                        {
                            server.Logoff();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ComCallError(methodName, e);
            }
            finally
            {
                EndComCall(methodName);
            }
        }

        /// <summary>
        /// Fetches the available locales.
        /// </summary>
        public int[] QueryAvailableLocales()
        {
            string methodName = "IOPCCommon.QueryAvailableLocales";

            try
            {
                IOPCCommon server = BeginComCall<IOPCCommon>(methodName, true);

                // query for available locales.
                int count = 0;
                IntPtr pLocaleIDs = IntPtr.Zero;

                server.QueryAvailableLocaleIDs(out count, out pLocaleIDs);

                // unmarshal results.
                return ComUtils.GetInt32s(ref pLocaleIDs, count, true);
            }
            catch (Exception e)
            {
                ComCallError(methodName, e);
                return null;
            }
            finally
            {
                EndComCall(methodName);
            }
        }

        /// <summary>
        /// Selects the best matching locale id.
        /// </summary>
        public static int SelectLocaleId(IList<int> availableLocaleIds, IList<string> preferredLocales)
        {
            // choose system default if no available locale ids.
            if (availableLocaleIds == null || availableLocaleIds.Count == 0)
            {
                return ComUtils.LOCALE_SYSTEM_DEFAULT;
            }

            // choose system default if no preferred locales.
            if (preferredLocales == null || preferredLocales.Count == 0)
            {
                return availableLocaleIds[0];
            }

            // look for an exact match.
            for (int ii = 0; ii < preferredLocales.Count; ii++)
            {
                for (int jj = 0; jj < availableLocaleIds.Count; jj++)
                {
                    if (ComUtils.CompareLocales(availableLocaleIds[jj], preferredLocales[ii], false))
                    {
                        return availableLocaleIds[jj];
                    }
                }
            }

            // look for a match on the language only.
            for (int ii = 0; ii < preferredLocales.Count; ii++)
            {
                for (int jj = 0; jj < availableLocaleIds.Count; jj++)
                {
                    if (ComUtils.CompareLocales(availableLocaleIds[jj], preferredLocales[ii], true))
                    {
                        return availableLocaleIds[jj];
                    }
                }
            }

            // return the first avialable locale.
            return availableLocaleIds[0];
        }

        /// <summary>
        /// Gracefully closes the connection to the server.
        /// </summary>
        public void Close()
        {
            Dispose();
        }
        #endregion Public Methods

        #region Protected Members
        /// <summary>
        /// Called immediately after connecting to the server.
        /// </summary>
        protected virtual void OnConnected()
        {
        }
        #endregion Protected Members

        #region Private Methods
        #endregion Private Methods

        #region Private Fields
        private object m_lock = new object();
        private string m_url;
        private readonly ILogger m_logger;
        private readonly ITelemetryContext m_telemetry;
        #endregion Private Fields
    }
}
