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
using Technosoftware.ClientGateway.Ae;
using Technosoftware.Common;
#endregion Using Directives

namespace Technosoftware.ClientGateway.Hda
{
    /// <summary>
    /// Manages the DA COM connections used by the UA server.
    /// </summary>
    /// <exclude />
    internal class ComHdaClientManager : ComClientManager
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ComHdaClientManager"/> class.
        /// </summary>
        public ComHdaClientManager(ITelemetryContext telemetry) : base(telemetry)
        {
            m_telemetry = telemetry;
            m_logger = telemetry.CreateLogger<ComAeClientManager>();
        }
        #endregion Constructors

        #region Public Members
        #endregion Public Members

        #region Protected Members
        /// <summary>
        /// Gets or sets the default COM client instance.
        /// </summary>
        /// <value>The default client.</value>
        protected new ComHdaClient DefaultClient
        {
            get { return base.DefaultClient as ComHdaClient; }
            set { base.DefaultClient = value; }
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        protected new ComHdaClientConfiguration Configuration
        {
            get { return base.Configuration as ComHdaClientConfiguration; }
        }

        /// <summary>
        /// Creates a new client object.
        /// </summary>
        protected override ComClient CreateClient()
        {
            return new ComHdaClient(Configuration, m_telemetry);
        }

        /// <summary>
        /// Updates the status node.
        /// </summary>
        protected override bool UpdateStatus()
        {
            // get the status from the server.
            ComHdaClient client = DefaultClient;
            ComHdaClient.ServerStatus? status = client.GetStatus();

            // check the client has been abandoned.
            if (!Object.ReferenceEquals(client, DefaultClient))
            {
                return false;
            }

            // update the server status.
            lock (StatusNodeLock)
            {
                StatusNode.ServerUrl.Value = Configuration.ServerUrl;

                if (status != null)
                {
                    StatusNode.SetStatusCode(DefaultSystemContext, StatusCodes.Good, DateTime.UtcNow);

                    if (String.IsNullOrEmpty(status.Value.szStatusString))
                    {
                        StatusNode.ServerState.Value = Utils.Format("{0}", status.Value.wStatus);
                    }
                    else
                    {
                        StatusNode.ServerState.Value = Utils.Format("{0} '{1}'", status.Value.wStatus, status.Value.szStatusString);
                    }

                    StatusNode.CurrentTime.Value = status.Value.ftCurrentTime;
                    StatusNode.LastUpdateTime.Value = DateTime.MinValue;
                    StatusNode.StartTime.Value = status.Value.ftStartTime;
                    StatusNode.VendorInfo.Value = status.Value.szVendorInfo;
                    StatusNode.SoftwareVersion.Value = Utils.Format(
                        "{0}.{1}.{2}",
                        status.Value.wMajorVersion,
                        status.Value.wMinorVersion,
                        status.Value.wBuildNumber);
                }
                else
                {
                    StatusNode.SetStatusCode(DefaultSystemContext, StatusCodes.BadOutOfService, DateTime.UtcNow);
                }

                StatusNode.ClearChangeMasks(DefaultSystemContext, true);
                return status != null;
            }
        }
        #endregion Protected Members

        #region Private Fields
        private readonly ILogger m_logger;
        private readonly ITelemetryContext m_telemetry;
        #endregion Private Fields
    }
}
