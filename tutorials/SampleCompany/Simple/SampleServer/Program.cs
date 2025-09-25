#region Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
// 
// License: 
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
// SPDX-License-Identifier: MIT
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using SampleCompany.Common;
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
            await output.WriteLineAsync("OPC UA Simple Console Sample Server").ConfigureAwait(false);

            // The application name and config file names
            const string applicationName = "SampleCompany.SimpleSampleServer";
            const string configSectionName = "SampleCompany.SampleServer";

            // command line options
            var showHelp = false;
            var autoAccept = false;
            var logConsole = false;
            var appLog = false;
            var renewCertificate = false;
            string password = null;
            var timeout = -1;

            var usage = Utils.IsRunningOnMono() ? $"Usage: mono {applicationName}.exe [OPTIONS]" : $"Usage: dotnet {applicationName}.dll [OPTIONS]";
            var options = new Mono.Options.OptionSet {
                usage,
                { "h|help", "show this message and exit", h => showHelp = h != null },
                { "a|autoaccept", "auto accept certificates (for testing only)", a => autoAccept = a != null },
                { "c|console", "log to console", c => logConsole = c != null },
                { "l|log", "log app output", c => appLog = c != null },
                { "p|password=", "optional password for private key", p => password = p },
                { "r|renew", "renew application certificate", r => renewCertificate = r != null },
                { "t|timeout=", "timeout in seconds to exit application", (int t) => timeout = t * 1000 },
            };

            try
            {
                // parse command line and set options
                _ = ConsoleUtils.ProcessCommandLine(output, args, options, ref showHelp, "REFSERVER");

                if (logConsole && appLog)
                {
                    output = new LogWriter();
                }

                // create the UA server
                var server = new MyUaServer(output) {
                    AutoAccept = autoAccept,
                    Password = password
                };

                // load the server configuration, validate certificates
                output.WriteLine("Loading configuration from {0}.", configSectionName);
                await server.LoadAsync(applicationName, configSectionName).ConfigureAwait(false);

                // setup the logging
                ConsoleUtils.ConfigureLogging(server.Configuration, applicationName, logConsole, LogLevel.Information);

                // check or renew the certificate
                await output.WriteLineAsync("Check the certificate.").ConfigureAwait(false);
                await server.CheckCertificateAsync(renewCertificate).ConfigureAwait(false);

                // start the server
                await output.WriteLineAsync("Start the server.").ConfigureAwait(false);
                await server.StartAsync().ConfigureAwait(false);

                await output.WriteLineAsync("Server started. Press Ctrl-C to exit...").ConfigureAwait(false);

                // wait for timeout or Ctrl-C
                var quitCts = new CancellationTokenSource();
                ManualResetEvent quitEvent = ConsoleUtils.CtrlCHandler(quitCts);
                var ctrlc = quitEvent.WaitOne(timeout);

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
