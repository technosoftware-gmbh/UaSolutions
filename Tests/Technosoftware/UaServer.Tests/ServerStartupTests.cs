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
using System.Threading.Tasks;
using NUnit.Framework;
using Opc.Ua;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
#endregion Using Directives

namespace Technosoftware.UaServer.Tests
{
    /// <summary>
    /// Test Standard Server startup.
    /// </summary>
    [TestFixture]
    [Category("Server")]
    [SetCulture("en-us")]
    [SetUICulture("en-us")]
    [Parallelizable]
    public class ServerStartupTests
    {
        [DatapointSource]
        public string[] UriSchemes =
        [
            Utils.UriSchemeOpcTcp,
            Utils.UriSchemeHttps,
            Utils.UriSchemeOpcHttps
        ];

        /// <summary>
        /// Start a server fixture with different uri schemes.
        /// </summary>
        [Theory]
        public async Task StartServerAsync(string uriScheme)
        {
            var fixture = new ServerFixture<UaStandardServer>();
            Assert.NotNull(fixture);
            fixture.UriScheme = uriScheme;
            UaStandardServer server = await fixture.StartAsync().ConfigureAwait(false);
            Assert.NotNull(server);
            await Task.Delay(1000).ConfigureAwait(false);
            await fixture.StopAsync().ConfigureAwait(false);
        }
    }
}
