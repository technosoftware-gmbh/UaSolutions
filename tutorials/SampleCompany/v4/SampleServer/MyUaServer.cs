#region Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opc.Ua;
using Technosoftware.UaConfiguration;
using Technosoftware.UaServer;
using Technosoftware.UaServer.Sessions;
using SampleCompany.Common;
#endregion Using Directives

namespace SampleCompany.SampleServer
{
    /// <summary>
    /// Main class for the Sample UA server
    /// </summary>
    /// <typeparam name="T">Any class based on the UaStandardServer class.</typeparam>
    public class MyUaServer<T>
        where T : UaStandardServer, new()
    {
        #region Public Properties
        /// <summary>
        /// Application instance used by the UA server.
        /// </summary>
        public ApplicationInstance Application { get; private set; }

        /// <summary>
        /// Application configuration used by the UA server.
        /// </summary>
        public ApplicationConfiguration Configuration => Application.ApplicationConfiguration;

        /// <summary>
        /// Specifies whether a certificate is automatically accepted (True) or not (False).
        /// </summary>
        public bool AutoAccept { get; set; }

        /// <summary>
        /// In case the private key is protected by a password it is specified by this property.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The exit code at the time the server stopped.
        /// </summary>
        public ExitCode ExitCode { get; private set; }

        /// <summary>
        /// The server object
        /// </summary>
        public T Server { get; private set; }
        #endregion Public Properties

        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Ctor of the server.
        /// </summary>
        /// <param name="writer">The text output.</param>
        public MyUaServer(TextWriter writer)
        {
            m_output = writer;
        }
        #endregion Constructors, Destructor, Initialization

        #region Public Methods
        /// <summary>
        /// Load the application configuration.
        /// </summary>
        /// <param name="applicationName">The name of the application.</param>
        /// <param name="configSectionName">The section name within the configuration.</param>
        /// <exception cref="ErrorExitException"></exception>
        public async Task LoadAsync(string applicationName, string configSectionName)
        {
            try
            {
                ExitCode = ExitCode.ErrorNotStarted;

                ApplicationInstance.MessageDlg = new ApplicationMessageDlg(m_output);
                var passwordProvider = new CertificatePasswordProvider(Password);
                Application = new ApplicationInstance
                {
                    ApplicationName = applicationName,
                    ApplicationType = ApplicationType.Server,
                    ConfigSectionName = configSectionName,
                    CertificatePasswordProvider = passwordProvider,
                    DisableCertificateAutoCreation = false
                };

                // load the application configuration.
                await Application.LoadApplicationConfigurationAsync(false).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new ErrorExitException(ex.Message, ExitCode);
            }
        }

        /// <summary>
        /// Load the application configuration.
        /// </summary>
        /// <param name="renewCertificate">Specifies whether the certificate should be renewed (true) or not (false)</param>
        /// <exception cref="ErrorExitException"></exception>
        public async Task CheckCertificateAsync(bool renewCertificate)
        {
            try
            {
                ApplicationConfiguration config = Application.ApplicationConfiguration;
                if (renewCertificate)
                {
                    await Application.DeleteApplicationInstanceCertificateAsync().ConfigureAwait(false);
                }

                // check the application certificate.
                bool haveAppCertificate = await Application
                    .CheckApplicationInstanceCertificatesAsync(false)
                    .ConfigureAwait(false);
                if (!haveAppCertificate)
                {
                    throw new ErrorExitException("Application instance certificate invalid!");
                }

                if (!config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
                {
                    config.CertificateValidator.CertificateValidation += new CertificateValidationEventHandler(
                        OnCertificateValidation
                    );
                }
            }
            catch (Exception ex)
            {
                throw new ErrorExitException(ex.Message, ExitCode);
            }
        }

        /// <summary>
        /// Create server instance and add node managers.
        /// </summary>
        /// <exception cref="ErrorExitException"></exception>
        public void Create(IList<IUaNodeManagerFactory> nodeManagerFactories)
        {
            try
            {
                // create the server.
                Server = new T();
                if (nodeManagerFactories != null)
                {
                    foreach (IUaNodeManagerFactory factory in nodeManagerFactories)
                    {
                        Server.AddNodeManager(factory);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ErrorExitException(ex.Message, ExitCode);
            }
        }

        /// <summary>
        /// Start the server.
        /// </summary>
        /// <exception cref="ErrorExitException"></exception>
        public async Task StartAsync()
        {
            try
            {
                // create the server.
                Server ??= new T();

                // start the server
                await Application.StartAsync(Server).ConfigureAwait(false);

                // save state
                ExitCode = ExitCode.ErrorRunning;

                // print endpoint info
                foreach (string endpoint in Application.BaseServer.GetEndpoints().Select(e => e.EndpointUrl).Distinct())
                {
                    m_output.WriteLine(endpoint);
                }

                // start the status thread
                m_status = Task.Run(StatusThreadAsync);

                // print notification on session events
                Server.CurrentInstance.SessionManager.SessionActivatedEvent += OnEventStatus;
                Server.CurrentInstance.SessionManager.SessionClosingEvent += OnEventStatus;
                Server.CurrentInstance.SessionManager.SessionCreatedEvent += OnEventStatus;
            }
            catch (Exception ex)
            {
                throw new ErrorExitException(ex.Message, ExitCode);
            }
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        /// <exception cref="ErrorExitException"></exception>
        public async Task StopAsync()
        {
            try
            {
                if (Server != null)
                {
                    using T server = Server;
                    // Stop status thread
                    Server = null;
                    await m_status.ConfigureAwait(false);

                    // Stop server and dispose
                    server.Stop();
                }

                ExitCode = ExitCode.Ok;
            }
            catch (Exception ex)
            {
                throw new ErrorExitException(ex.Message, ExitCode.ErrorStopping);
            }
        }
        #endregion Public Methods

        #region Event Handlers
        /// <summary>
        /// The certificate validator is used
        /// if auto accept is not selected in the configuration.
        /// </summary>
        private void OnCertificateValidation(
            CertificateValidator validator,
            CertificateValidationEventArgs e
        )
        {
            if (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted && AutoAccept)
            {
                m_output.WriteLine(
                    "Accepted Certificate: [{0}] [{1}]",
                    e.Certificate.Subject,
                    e.Certificate.Thumbprint
                );
                e.Accept = true;
                return;
            }
            m_output.WriteLine(
                "Rejected Certificate: {0} [{1}] [{2}]",
                e.Error,
                e.Certificate.Subject,
                e.Certificate.Thumbprint
            );
        }

        /// <summary>
        /// Update the session status.
        /// </summary>
        private void OnEventStatus(object sender, SessionEventArgs eventArgs)
        {
            Session session = (Session)sender;

            m_lastEventTime = DateTime.UtcNow;
            PrintSessionStatus(session, eventArgs.Reason.ToString());
        }
        #endregion Event Handlers

        #region Helper Methods
        /// <summary>
        /// Output the status of a connected session.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="reason">The reason</param>
        /// <param name="lastContact">true if the date/time of the last event should also be in the output; false if not.</param>
        private void PrintSessionStatus(Session session, string reason, bool lastContact = false)
        {
            var item = new StringBuilder();
            lock (session.DiagnosticsLock)
            {
                item.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "{0,9}:{1,20}:",
                    reason,
                    session.SessionDiagnostics.SessionName
                );
                if (lastContact)
                {
                    item.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "Last Event:{0:HH:mm:ss}",
                        session.SessionDiagnostics.ClientLastContactTime.ToLocalTime()
                    );
                }
                else
                {
                    if (session.Identity != null)
                    {
                        item.AppendFormat(CultureInfo.InvariantCulture, ":{0,20}", session.Identity.DisplayName);
                    }
                    item.AppendFormat(CultureInfo.InvariantCulture, ":{0}", session.Id);
                }
            }
            m_output.WriteLine(item.ToString());
        }

        /// <summary>
        /// Status thread, prints connection status every 10 seconds.
        /// </summary>
        private async Task StatusThreadAsync()
        {
            while (Server != null)
            {
                if (DateTime.UtcNow - m_lastEventTime > TimeSpan.FromMilliseconds(10000))
                {
                    IList<Session> sessions = Server.CurrentInstance.SessionManager.GetSessions();
                    for (int ii = 0; ii < sessions.Count; ii++)
                    {
                        Session session = sessions[ii];
                        PrintSessionStatus(session, "-Status-", true);
                    }
                    m_lastEventTime = DateTime.UtcNow;
                }
                await Task.Delay(1000).ConfigureAwait(false);
            }
        }
        #endregion Helper Methods

        #region Private Fields
        private readonly TextWriter m_output;
        private Task m_status;
        private DateTime m_lastEventTime;
        #endregion Private Fields
    }
}
