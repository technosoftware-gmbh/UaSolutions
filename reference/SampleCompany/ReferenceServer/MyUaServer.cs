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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using SampleCompany.Common;
using Technosoftware.UaConfiguration;
using Technosoftware.UaServer;
#endregion Using Directives

namespace SampleCompany.ReferenceServer
{
    public class MyUaServer<T>
        where T : UaStandardServer, new()
    {
        public ApplicationInstance Application { get; private set; }

        public ApplicationConfiguration Configuration => Application.ApplicationConfiguration;

        public bool AutoAccept { get; set; }

        public char[] Password { get; set; }

        public ExitCode ExitCode { get; private set; }

        public T Server { get; private set; }

        /// <summary>
        /// Ctor of the server.
        /// </summary>
        /// <param name="telemetry">The telemetry context.</param>
        public MyUaServer(ITelemetryContext telemetry)
        {
            m_telemetry = telemetry;
            m_logger = telemetry.CreateLogger<MyUaServer<T>>();
        }

        /// <summary>
        /// Load the application configuration.
        /// </summary>
        /// <exception cref="ErrorExitException"></exception>
        public async Task LoadAsync(string applicationName, string configSectionName)
        {
            try
            {
                ExitCode = ExitCode.ErrorNotStarted;

                ApplicationInstance.MessageDlg = new ApplicationMessageDlg();
                var passwordProvider = new CertificatePasswordProvider(Password);
                Application = new ApplicationInstance(m_telemetry)
                {
                    ApplicationName = applicationName,
                    ApplicationType = ApplicationType.Server,
                    ConfigSectionName = configSectionName,
                    CertificatePasswordProvider = passwordProvider
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
                        CertificateValidator_CertificateValidation
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
                foreach (string endpoint in Application.Server.GetEndpoints().Select(e => e.EndpointUrl).Distinct())
                {
                    Console.WriteLine(endpoint);
                }

                // start the status thread
                m_status = Task.Run(StatusThreadAsync);

                // print notification on session events
                Server.CurrentInstance.SessionManager.SessionActivated += OnEventStatus;
                Server.CurrentInstance.SessionManager.SessionClosing += OnEventStatus;
                Server.CurrentInstance.SessionManager.SessionCreated += OnEventStatus;
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
                    await server.StopAsync().ConfigureAwait(false);
                }

                ExitCode = ExitCode.Ok;
            }
            catch (Exception ex)
            {
                throw new ErrorExitException(ex.Message, ExitCode.ErrorStopping);
            }
        }

        /// <summary>
        /// The certificate validator is used
        /// if auto accept is not selected in the configuration.
        /// </summary>
        private void CertificateValidator_CertificateValidation(
            CertificateValidator validator,
            CertificateValidationEventArgs e
        )
        {
            if (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted && AutoAccept)
            {
                m_logger.LogInformation(
                    "Accepted Certificate: [{Subject}] [{Thumbprint}]",
                    e.Certificate.Subject,
                    e.Certificate.Thumbprint
                );
                e.Accept = true;
                return;
            }
            m_logger.LogInformation(
                "Rejected Certificate: {Error} [{Subject}] [{Thumbprint}]",
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
            IUaSession session = (IUaSession)sender;

            m_lastEventTime = DateTime.UtcNow;
            LogSessionStatusFull(session, eventArgs.Reason.ToString());
        }

        /// <summary>
        /// Output the status of a connected session.
        /// </summary>
        private void LogSessionStatusLastContact(IUaSession session, string reason)
        {
            lock (session.DiagnosticsLock)
            {
                m_logger.LogInformation(
                    "{Reason,9}:{Session,20}:Last Event:{LastContactTime:HH:mm:ss}",
                    reason,
                    session.SessionDiagnostics.SessionName,
                    session.SessionDiagnostics.ClientLastContactTime.ToLocalTime()
                );
            }
        }

        /// <summary>
        /// Output the status of a connected session.
        /// </summary>
        private void LogSessionStatusFull(IUaSession session, string reason)
        {
            lock (session.DiagnosticsLock)
            {
                m_logger.LogInformation(
                    "{Reason,9}:{Session,20}:Last Event:{LastContactTime:HH:mm:ss}:{UserIdentity,20}:{SessionId}",
                    reason,
                    session.SessionDiagnostics.SessionName,
                    session.SessionDiagnostics.ClientLastContactTime.ToLocalTime(),
                    session.Identity?.DisplayName ?? "Anonymous",
                    session.Id
                );
            }
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
                    IList<IUaSession> sessions = Server.CurrentInstance.SessionManager.GetSessions();
                    for (int ii = 0; ii < sessions.Count; ii++)
                    {
                        IUaSession session = sessions[ii];
                        LogSessionStatusLastContact(session, "-Status-");
                    }
                    m_lastEventTime = DateTime.UtcNow;
                }
                await Task.Delay(1000).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// A dialog which asks for user input.
        /// </summary>
        public class ApplicationMessageDlg : IUaApplicationMessageDlg
        {
            private readonly TextWriter m_output;
            private string m_message = string.Empty;
            private bool m_ask;

            public ApplicationMessageDlg(TextWriter output = null)
            {
                m_output = output ?? Console.Out;
            }

            public override void Message(string text, bool ask)
            {
                m_message = text;
                m_ask = ask;
            }

            public override async Task<bool> ShowAsync()
            {
                if (m_ask)
                {
                    var message = new StringBuilder(m_message);
                    message.Append(" (y/n, default y): ");
                    m_output.Write(message.ToString());

                    try
                    {
                        ConsoleKeyInfo result = Console.ReadKey();
                        m_output.WriteLine();
                        return await Task.FromResult(result.KeyChar is 'y' or 'Y' or '\r')
                            .ConfigureAwait(false);
                    }
                    catch
                    {
                        // intentionally fall through
                    }
                }
                else
                {
                    m_output.WriteLine(m_message);
                }

                return await Task.FromResult(true).ConfigureAwait(false);
            }
        }

        private readonly ITelemetryContext m_telemetry;
        private readonly ILogger m_logger;
        private Task m_status;
        private DateTime m_lastEventTime;
    }
}
