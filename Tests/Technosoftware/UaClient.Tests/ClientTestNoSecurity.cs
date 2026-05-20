#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System.Threading.Tasks;
using NUnit.Framework;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaClient.Tests
{
    /// <summary>
    /// Client tests which require security None and are otherwise skipped,
    /// starts the server with additional security None profile.
    /// </summary>
    [TestFixture]
    [Category("Client")]
    [SetCulture("en-us")]
    [SetUICulture("en-us")]
    [TestFixtureSource(nameof(FixtureArgs))]
    public class ClientTestNoSecurity
    {
        private readonly ClientTest m_clientTest;

        public static readonly object[] FixtureArgs =
        [
            new object[] { Utils.UriSchemeOpcTcp }
            // https protocol security None is not supported
            // new object [] { Utils.UriSchemeHttps},
            // new object [] { Utils.UriSchemeOpcHttps},
        ];

        public ClientTestNoSecurity()
        {
            m_clientTest = new ClientTest(Utils.UriSchemeOpcTcp);
        }

        public ClientTestNoSecurity(string uriScheme)
        {
            m_clientTest = new ClientTest(uriScheme);
        }

        /// <summary>
        /// Set up a Server and a Client instance.
        /// </summary>
        [OneTimeSetUp]
        public Task OneTimeSetUpAsync()
        {
            m_clientTest.SupportsExternalServerUrl = true;
            return m_clientTest.OneTimeSetUpCoreAsync(true);
        }

        /// <summary>
        /// Tear down the Server and the Client.
        /// </summary>
        [OneTimeTearDown]
        public Task OneTimeTearDownAsync()
        {
            return m_clientTest.OneTimeTearDownAsync();
        }

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public Task SetUpAsync()
        {
            return m_clientTest.SetUpAsync();
        }

        /// <summary>
        /// Test teardown.
        /// </summary>
        [TearDown]
        public Task TearDownAsync()
        {
            return m_clientTest.TearDownAsync();
        }

        /// <summary>
        /// GetEndpoints on the discovery channel,
        /// the oversized message can pass because security None is enabled.
        /// </summary>
        [Test]
        [Order(105)]
        public Task GetEndpointsOnDiscoveryChannelAsync()
        {
            return m_clientTest.GetEndpointsOnDiscoveryChannelAsync(true);
        }

        [Test]
        [Order(230)]
        public Task ReconnectJWTSecurityNoneAsync()
        {
            return m_clientTest.ReconnectJWTAsync(SecurityPolicies.None);
        }

        [Test]
        [Order(220)]
        public Task ConnectJWTAsync()
        {
            return m_clientTest.ConnectJWTAsync(SecurityPolicies.None);
        }

        /// <summary>
        /// Open a session on a channel, then reconnect (activate)
        /// the same session on a new channel with saved session secrets
        /// </summary>
        [Test]
        [Order(260)]
        [TestCase(true)]
        [TestCase(false)]
        public Task ReconnectSessionOnAlternateChannelWithSavedSessionSecretsSecurityNoneAsync(
            bool anonymous)
        {
            return m_clientTest.ReconnectSessionOnAlternateChannelWithSavedSessionSecretsAsync(
                SecurityPolicies.None,
                anonymous);
        }

        [Theory]
        [Order(400)]
        public Task BrowseFullAddressSpaceSecurityNoneAsync(bool operationLimits)
        {
            return m_clientTest.BrowseFullAddressSpaceAsync(SecurityPolicies.None, operationLimits);
        }

        [Test]
        [Order(201)]
        public Task ConnectAndCloseAsyncNoSecurityAsync()
        {
            return m_clientTest.ConnectAndCloseAsync(SecurityPolicies.None);
        }
    }
}
