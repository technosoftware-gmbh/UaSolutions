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

using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Technosoftware.UaConfiguration;

namespace Technosoftware.Common.Client
{
    /// <summary>
    /// Validates UserName.
    /// </summary>
    public class UserNameValidator
    {
        /// <summary>
        /// Triple DES Key
        /// </summary>
        private const string strKey = "h13h6m9F";

        /// <summary>
        /// Triple DES initialization vector
        /// </summary>
        private const string strIV = "Zse5";

        #region Constructors
        /// <summary>
        /// The default constructor.
        /// </summary>
        public UserNameValidator(string applicationName, ITelemetryContext telemetry)
        {
            m_telemetry = telemetry;
            m_logger = telemetry.CreateLogger<UserNameValidator>();
            m_UserNameIdentityTokens = UserNameCreator.LoadUserName(applicationName, m_logger);
        }
        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Validates a User.
        /// </summary>
        /// <param name="token">UserNameIdentityToken.</param>
        /// <returns>True if the list contains a valid item.</returns>
        public bool Validate(UserNameIdentityToken token)
        {
            return Validate(token.UserName, token.DecryptedPassword);
        }

        /// <summary>
        /// Validates a User.
        /// </summary>
        /// <param name="name">user name.</param>
        /// <param name="password">password.</param>
        /// <returns>True if the list contains a valid item.</returns>
        public bool Validate(string name, byte[] password)
        {
            lock (m_lock)
            {
                if (!m_UserNameIdentityTokens.ContainsKey(name))
                {
                    return false;
                }

                return (m_UserNameIdentityTokens[name].DecryptedPassword == password);
            }
        }

        #endregion Public Methods

        #region Private Fields
        private object m_lock = new object();
        private Dictionary<string, UserNameIdentityToken> m_UserNameIdentityTokens = new Dictionary<string, UserNameIdentityToken>();
        private readonly ILogger m_logger;
        private readonly ITelemetryContext m_telemetry;
        #endregion Private Fields
    }
}
