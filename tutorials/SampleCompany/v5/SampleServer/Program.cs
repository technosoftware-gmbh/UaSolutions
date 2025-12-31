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
using Technosoftware.UaUtilities.Licensing;
using SampleCompany.Common;
using SampleCompany.NodeManagers;
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
            Console.WriteLine("OPC UA Console Sample Server");

            #region License validation
            //const string licenseData =
            //        @"";
            //bool licensed = Technosoftware.UaServer.LicenseHandler.Validate(licenseData);
            //if (!licensed)
            //{
            //    Console.WriteLine("WARNING: No valid license applied.");
            //}

            string licensedString = $"   Licensed Product     : {Technosoftware.UaUtilities.Licensing.LicenseHandler.LicensedProduct}";
            Console.WriteLine(licensedString);
            licensedString = $"   Licensed Features    : {Technosoftware.UaUtilities.Licensing.LicenseHandler.LicensedFeatures}";
            Console.WriteLine(licensedString);
            if (Technosoftware.UaUtilities.Licensing.LicenseHandler.IsEvaluation)
            {
                licensedString = $"   Evaluation expires at: {Technosoftware.UaUtilities.Licensing.LicenseHandler.LicenseExpirationDate}";
                Console.WriteLine(licensedString);
                licensedString = $"   Days until Expiration: {Technosoftware.UaUtilities.Licensing.LicenseHandler.LicenseExpirationDays}";
                Console.WriteLine(licensedString);
            }
            licensedString = $"   Support Included     : {Technosoftware.UaUtilities.Licensing.LicenseHandler.Support}";
            Console.WriteLine(licensedString);
            if (Technosoftware.UaUtilities.Licensing.LicenseHandler.Support != Technosoftware.UaUtilities.Licensing.SupportType.None)
            {
                licensedString = $"   Support expire at    : {Technosoftware.UaUtilities.Licensing.LicenseHandler.SupportExpirationDate}";
                Console.WriteLine(licensedString);
                licensedString = $"   Days until Expiration: {Technosoftware.UaUtilities.Licensing.LicenseHandler.SupportExpirationDays}";
                Console.WriteLine(licensedString);
            }
            if (Technosoftware.UaUtilities.Licensing.LicenseHandler.IsEvaluation)
            {
                licensedString = $"   Evaluation Period    : {Technosoftware.UaUtilities.Licensing.LicenseHandler.EvaluationPeriod} minutes.";
                Console.WriteLine(licensedString);
            }

            if (!Technosoftware.UaUtilities.Licensing.LicenseHandler.IsLicensed && !Technosoftware.UaUtilities.Licensing.LicenseHandler.IsEvaluation)
            {
                Console.WriteLine("ERROR: No valid license applied.");
            }
            #endregion License validation

            // The application name and config file name
            const string applicationName = "SampleCompany.SampleServer";
            const string configSectionName = "SampleCompany.SampleServer";

            // command line options
            bool showHelp = false;
            bool autoAccept = false;
            bool logConsole = false;
            bool appLog = false;
            bool fileLog = false;
            bool renewCertificate = false;
            char[] password = null;
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
                { "f|file", "log to file", f => fileLog = f != null },
                { "p|password=", "optional password for private key", p => password = p.ToCharArray() },
                { "r|renew", "renew application certificate", r => renewCertificate = r != null },
                { "t|timeout=", "timeout in seconds to exit application", (int t) => timeout = t * 1000 }
            };

            using var telemetry = new ConsoleTelemetry();
            ILogger logger = LoggerUtils.Null.Logger;
            try
            {
                // parse command line and set options
                ConsoleUtils.ProcessCommandLine(args, options, ref showHelp, "SAMPLESERVER");

                // log console output to logger
                if (logConsole && appLog)
                {
                    logger = telemetry.CreateLogger("Main");
                }

                // create the UA server
                var server = new MyUaServer<NodeManagers.Simulation.SimulationServer>(telemetry)
                {
                    AutoAccept = autoAccept,
                    Password = password
                };

                // load the server configuration, validate certificates
                Console.WriteLine($"Loading configuration from {configSectionName}.");
                await server.LoadAsync(applicationName, configSectionName).ConfigureAwait(false);

                // setup the logging
                telemetry.ConfigureLogging(server.Configuration, applicationName, logConsole, fileLog, appLog, LogLevel.Information);

                // check or renew the certificate
                Console.WriteLine("Check the certificate.");
                await server.CheckCertificateAsync(renewCertificate).ConfigureAwait(false);

                // Create and add the node managers
                server.Create(NodeManagerUtils.NodeManagerFactories);

                // start the server
                Console.WriteLine("Start the server.");
                await server.StartAsync().ConfigureAwait(false);

                Console.WriteLine("Server started. Press Ctrl-C to exit...");

                // wait for timeout or Ctrl-C
                var quitCTS = new CancellationTokenSource();
                ManualResetEvent quitEvent = ConsoleUtils.CtrlCHandler(quitCTS);
                bool ctrlc = quitEvent.WaitOne(timeout);

                // stop server. May have to wait for clients to disconnect.
                Console.WriteLine("Server stopped. Waiting for exit...");
                await server.StopAsync().ConfigureAwait(false);

                return (int)ExitCode.Ok;
            }
            catch (ErrorExitException eee)
            {
                Console.WriteLine($"The application exits with error: {eee.Message}");
                return (int)eee.ExitCode;
            }
        }
    }
}
