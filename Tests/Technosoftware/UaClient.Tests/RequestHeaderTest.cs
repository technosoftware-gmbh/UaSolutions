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
using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Opc.Ua;
using Technosoftware.Tests;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
#endregion Using Directives

namespace Technosoftware.UaClient.Tests
{
    [TestFixture]
    [Category("Client")]
    [Category("SessionClient")]
    [SetCulture("en-us")]
    [SetUICulture("en-us")]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    [Parallelizable]
    public class RequestHeaderTest : ClientTestFramework
    {
        /// <summary>
        /// Setup a server and client fixture.
        /// </summary>
        [OneTimeSetUp]
        public override Task OneTimeSetUpAsync()
        {
            return OneTimeSetUpCoreAsync(
                securityNone: false,
                enableClientSideTracing: false,
                enableServerSideTracing: false);
        }

        /// <summary>
        /// Tear down the Server and the Client.
        /// </summary>
        [OneTimeTearDown]
        public override Task OneTimeTearDownAsync()
        {
            Utils.SilentDispose(ClientFixture);
            return base.OneTimeTearDownAsync();
        }

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public override Task SetUpAsync()
        {
            return base.SetUpAsync();
        }

        /// <summary>
        /// Test teardown.
        /// </summary>
        [TearDown]
        public override Task TearDownAsync()
        {
            return base.TearDownAsync();
        }

        /// <summary>
        /// Global Setup for benchmarks.
        /// </summary>
        [GlobalSetup]
        public override void GlobalSetup()
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();
            ILogger logger = telemetry.CreateLogger<RequestHeaderTest>();
            logger.LogInformation("GlobalSetup: Start Server");
            OneTimeSetUpCoreAsync(
                    enableClientSideTracing: false,
                    enableServerSideTracing: false,
                    disableActivityLogging: false)
                .GetAwaiter()
                .GetResult();
            logger.LogInformation("GlobalSetup: Connecting");
            InitializeSession(
                ClientFixture.ConnectAsync(ServerUrl, SecurityPolicy).GetAwaiter().GetResult());
            logger.LogInformation("GlobalSetup: Ready");
        }

        /// <summary>
        /// Global cleanup for benchmarks.
        /// </summary>
        [GlobalCleanup]
        public override void GlobalCleanup()
        {
            base.GlobalCleanup();
        }

        [Test]
        [Benchmark]
        public async Task ReadValuesWithoutTracingAsync()
        {
            NamespaceTable namespaceUris = Session.NamespaceUris;
            var testSet = new NodeIdCollection(GetTestSetStatic(namespaceUris));
            testSet.AddRange(GetTestSetFullSimulation(namespaceUris));
            (DataValueCollection values, IList<ServiceResult> errors) =
                await Session.ReadValuesAsync(testSet).ConfigureAwait(false);
            Assert.AreEqual(testSet.Count, values.Count);
            Assert.AreEqual(testSet.Count, errors.Count);
        }
    }
}
