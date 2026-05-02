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
using Technosoftware.Rcw;
using Technosoftware.UaServer;

#endregion Using Directives

namespace Technosoftware.ClientGateway.Da
{
    /// <summary>
    /// Manages the DA COM connections used by the UA server.
    /// </summary>
    /// <exclude />
    internal class ComDaClientManager : ComClientManager
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ComDaClientManager"/> class.
        /// </summary>
        public ComDaClientManager(ITelemetryContext telemetry) : base(telemetry)
        {
            m_telemetry = telemetry;
            m_logger = telemetry.CreateLogger<ComAeClientManager>();
        }
        #endregion Constructors

        #region Public Members
        /// <summary>
        /// Selects the DA COM client to use for the current context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="useDefault">True if the the default instance should be returned.</param>
        /// <returns>A DA COM client instance.</returns>
        public new ComDaClient SelectClient(UaServerContext context, bool useDefault)
        {
            return (ComDaClient)base.SelectClient(context, useDefault);
        }
        #endregion Public Members

        #region Protected Members
        /// <summary>
        /// Gets or sets the default COM client instance.
        /// </summary>
        /// <value>The default client.</value>
        protected new ComDaClient DefaultClient
        {
            get { return base.DefaultClient as ComDaClient; }
            set { base.DefaultClient = value; }
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        protected new ComDaClientConfiguration Configuration
        {
            get { return base.Configuration as ComDaClientConfiguration; }
        }

        /// <summary>
        /// Creates a new client object.
        /// </summary>
        protected override ComClient CreateClient()
        {
            return new ComDaClient(Configuration, m_telemetry);
        }

        /// <summary>
        /// Updates the status node.
        /// </summary>
        protected override bool UpdateStatus()
        {
            // get the status from the server.
            ComDaClient client = DefaultClient;
            OPCSERVERSTATUS? status = client.GetStatus();

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

                    StatusNode.ServerState.Value = Utils.Format("{0}", status.Value.dwServerState);
                    StatusNode.CurrentTime.Value = ComUtils.GetDateTime(status.Value.ftCurrentTime);
                    StatusNode.LastUpdateTime.Value = ComUtils.GetDateTime(status.Value.ftLastUpdateTime);
                    StatusNode.StartTime.Value = ComUtils.GetDateTime(status.Value.ftStartTime);
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
