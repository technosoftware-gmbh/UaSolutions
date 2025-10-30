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
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using SampleCompany.Common;
using SampleCompany.NodeManagers;
using Technosoftware.UaUtilities.Licensing;
#endregion Using Directives

namespace SampleCompany.SampleServer
{
    /// <summary>
    /// The program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static async Task<int> Main(string[] args)
        {
            TextWriter output = Console.Out;
            await output.WriteLineAsync("OPC UA Console Sample Server").ConfigureAwait(false);

            #region License validation
            const string licenseData =
                    @"";
            var licensed = Technosoftware.UaServer.LicenseHandler.Validate(licenseData);
            if (!licensed)
            {
                await output.WriteLineAsync("WARNING: No valid license applied.").ConfigureAwait(false);
            }
            #endregion License validation


            // The application name and config file names
            const string applicationName = "SampleCompany.SampleServer";
            const string configSectionName = "SampleCompany.SampleServer";

            // command line options
            bool showHelp = false;
            bool autoAccept = false;
            bool logConsole = false;
            bool appLog = false;
            bool renewCertificate = false;
            bool shadowConfig = false;
            string password = null;
            int timeout = -1;

            string usage = Utils.IsRunningOnMono()
                ? $"Usage: mono {applicationName}.exe [OPTIONS]"
                : $"Usage: dotnet {applicationName}.dll [OPTIONS]";
            var options = new Mono.Options.OptionSet
            {
                usage,
                { "h|help", "show this message and exit", h => showHelp = h != null },
                { "a|autoaccept", "auto accept certificates (for testing only)", a => autoAccept = a != null },
                { "c|console", "log to console", c => logConsole = c != null },
                { "l|log", "log app output", c => appLog = c != null },
                { "p|password=", "optional password for private key", p => password = p },
                { "r|renew", "renew application certificate", r => renewCertificate = r != null },
                { "t|timeout=", "timeout in seconds to exit application", (int t) => timeout = t * 1000 },
                { "s|shadowconfig", "create configuration in pki root", s => shadowConfig = s != null }
            };

            try
            {
                // parse command line and set options
                ConsoleUtils.ProcessCommandLine(output, args, options, ref showHelp, "REFSERVER");

                var licensedString = $"   Licensed Product     : {LicenseHandler.LicensedProduct}";
                await output.WriteLineAsync(licensedString).ConfigureAwait(false);
                licensedString =     $"   Licensed Features    : {LicenseHandler.LicensedFeatures}";
                await output.WriteLineAsync(licensedString).ConfigureAwait(false);
                if (LicenseHandler.IsEvaluation)
                {
                    licensedString = $"   Evaluation expires at: {LicenseHandler.LicenseExpirationDate}";
                    await output.WriteLineAsync(licensedString).ConfigureAwait(false);
                    licensedString = $"   Days until Expiration: {LicenseHandler.LicenseExpirationDays}";
                    await output.WriteLineAsync(licensedString).ConfigureAwait(false);
                }
                licensedString =     $"   Support Included     : {LicenseHandler.Support}";
                await output.WriteLineAsync(licensedString).ConfigureAwait(false);
                if (LicenseHandler.Support != SupportType.None)
                {
                    licensedString = $"   Support expire at    : {LicenseHandler.SupportExpirationDate}";
                    await output.WriteLineAsync(licensedString).ConfigureAwait(false);
                    licensedString = $"   Days until Expiration: {LicenseHandler.SupportExpirationDays}";
                    await output.WriteLineAsync(licensedString).ConfigureAwait(false);
                }
                if (LicenseHandler.IsEvaluation)
                {
                    licensedString = $"   Evaluation Period    : {LicenseHandler.EvaluationPeriod} minutes.";
                    await output.WriteLineAsync(licensedString).ConfigureAwait(false);
                }

                if (!LicenseHandler.IsLicensed && !LicenseHandler.IsEvaluation)
                {
                    await output.WriteLineAsync("ERROR: No valid license applied.").ConfigureAwait(false);
                }

                if (logConsole && appLog)
                {
                    output = new LogWriter();
                }

                CertificateStoreType.RegisterCertificateStoreType(CustomDirectoryCertificateStoreType.StoreName, new CustomDirectoryCertificateStoreType());

                // create the UA server
                var server = new MyUaServer<NodeManagers.Simulation.SimulationServer>(output) {
                    AutoAccept = autoAccept,
                    Password = password
                };

                // load the server configuration, validate certificates
                output.WriteLine("Loading configuration from {0}.", configSectionName);
                await server.LoadAsync(applicationName, configSectionName).ConfigureAwait(false);

                // use the shadow config to map the config to an externally accessible location
                if (shadowConfig)
                {
                    output.WriteLine("Using shadow configuration.");
                    var shadowPath = Directory.GetParent(Path.GetDirectoryName(
                        Utils.ReplaceSpecialFolderNames(server.Configuration.TraceConfiguration.OutputFilePath))).FullName;
                    var shadowFilePath = Path.Combine(shadowPath, Path.GetFileName(server.Configuration.SourceFilePath));
                    if (!File.Exists(shadowFilePath))
                    {
                        output.WriteLine("Create a copy of the config in the shadow location.");
                        File.Copy(server.Configuration.SourceFilePath, shadowFilePath, true);
                    }
                    output.WriteLine("Reloading configuration from {0}.", shadowFilePath);
                    await server.LoadAsync(applicationName, Path.Combine(shadowPath, configSectionName)).ConfigureAwait(false);
                }

                // setup the logging
                ConsoleUtils.ConfigureLogging(server.Configuration, applicationName, logConsole, LogLevel.Information);

                // check or renew the certificate
                await output.WriteLineAsync("Check the certificate.").ConfigureAwait(false);
                await server.CheckCertificateAsync(renewCertificate).ConfigureAwait(false);

                // Create and add the node managers
                server.Create(NodeManagerUtils.NodeManagerFactories);

                // start the server
                await output.WriteLineAsync("Start the server.").ConfigureAwait(false);
                await server.StartAsync().ConfigureAwait(false);

                await output.WriteLineAsync("Server started. Press Ctrl-C to exit...").ConfigureAwait(false);

                // wait for timeout or Ctrl-C
                var quitCts = new CancellationTokenSource();
                ManualResetEvent quitEvent = ConsoleUtils.CtrlCHandler(quitCts);
                bool ctrlc = quitEvent.WaitOne(timeout);

                // stop server. May have to wait for clients to disconnect.
                await output.WriteLineAsync("Server stopped. Waiting for exit...").ConfigureAwait(false);
                await server.StopAsync().ConfigureAwait(false);

                return (int)ExitCode.Ok;
            }
            catch (ErrorExitException errorExitException)
            {
                output.WriteLine("The application exits with error: {0}", errorExitException.Message);
                return (int)errorExitException.ExitCode;
            }
        }
    }
}
