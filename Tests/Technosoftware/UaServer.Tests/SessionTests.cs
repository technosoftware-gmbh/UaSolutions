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

using Assert = NUnit.Framework.Legacy.ClassicAssert;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer.Tests
{
    [TestFixture]
    [Category("Event")]
    public class SessionTests
    {
        [Test]
        public async Task UpdateDiagnosticCounters_RaisesEvent_WhenPerRequestCounterChangedAsync()
        {
            var fixture = new ServerFixture<UaStandardServer>();
            await fixture.StartAsync().ConfigureAwait(false);

            try
            {
                UaStandardServer server = fixture.Server;

                (RequestHeader requestHeader, SecureChannelContext secureChannelContext) =
                    await server.CreateAndActivateSessionAsync("UpdateDiagnosticCountersTest").ConfigureAwait(false);

                IUaSession session = server.CurrentInstance.SessionManager.GetSession(requestHeader.AuthenticationToken);
                Assert.NotNull(session, "Session should exist after Create/Activate.");

                bool eventRaised = false;

                server.CurrentInstance.SessionManager.SessionDiagnosticsChanged += (s, reason)
                    => eventRaised = true;

                uint before = session.SessionDiagnostics.ReadCount.TotalCount;

                // Call ValidateRequest for a request type that maps to a counter (Read).
                session.ValidateRequest(requestHeader, secureChannelContext, RequestType.Read);

                Assert.IsTrue(eventRaised, "SessionDiagnosticsChanged event should be raised when a per-request counter changes.");
                Assert.Greater(session.SessionDiagnostics.ReadCount.TotalCount, before, "ReadCount.TotalCount should have incremented.");
            }
            finally
            {
                await fixture.StopAsync().ConfigureAwait(false);
            }
        }

        [Test]
        [Category("Event")]
        [TestCase(RequestType.Unknown)]
        [TestCase(RequestType.FindServers)]
        [TestCase(RequestType.GetEndpoints)]
        [TestCase(RequestType.CreateSession)]
        [TestCase(RequestType.ActivateSession)]
        [TestCase(RequestType.CloseSession)]
        [TestCase(RequestType.Cancel)]
        public async Task UpdateDiagnosticCounters_DoesNotRaiseEvent_ForIgnoredRequestTypesAsync(RequestType requestType)
        {
            var fixture = new ServerFixture<UaStandardServer>();
            await fixture.StartAsync().ConfigureAwait(false);

            try
            {
                UaStandardServer server = fixture.Server;

                (RequestHeader requestHeader, SecureChannelContext secureChannelContext) =
                    await server.CreateAndActivateSessionAsync("UpdateDiagnosticCountersIgnoredTest").ConfigureAwait(false);

                IUaSession session = server.CurrentInstance.SessionManager.GetSession(requestHeader.AuthenticationToken);
                Assert.NotNull(session, "Session should exist after Create/Activate.");

                bool eventRaised = false;

                server.CurrentInstance.SessionManager.SessionDiagnosticsChanged += (s, reason)
                    => eventRaised = true;

                // Capture total requests before; UpdateDiagnosticCounters always increments TotalRequestCount.
                uint totalBefore = session.SessionDiagnostics.TotalRequestCount.TotalCount;

                // Call ValidateRequest with one of the ignored request types.
                session.ValidateRequest(requestHeader, secureChannelContext, requestType);

                Assert.AreEqual(
                    totalBefore + 1,
                    session.SessionDiagnostics.TotalRequestCount.TotalCount,
                    "TotalRequestCount should increment for all request types.");

                Assert.IsFalse(eventRaised, $"SessionDiagnosticsChanged event must NOT be raised for request type {requestType}.");
            }
            finally
            {
                await fixture.StopAsync().ConfigureAwait(false);
            }
        }
    }
}
