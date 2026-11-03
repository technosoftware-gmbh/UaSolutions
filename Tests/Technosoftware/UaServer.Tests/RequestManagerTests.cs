#region Copyright (c) 2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com
//
// The Software is subject to the Technosoftware GmbH MIT License, which can
// be found here:
// https://technosoftware.com/license/mit/
//
// The Software is based on the OPC Foundation UA Stack and the OPC Foundation
// MIT License. The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using Moq;
using NUnit.Framework;
using Technosoftware.Tests;
using Opc.Ua;
using Technosoftware.UaServer;
#endregion Using Directives

namespace Technosoftware.UaServer.Tests
{
    [TestFixture]
    [Category("Server")]
    [Parallelizable]
    public class RequestManagerTests
    {
        [Test]
        public void CancelRequestsShouldCancelActivateSessionRequestWithoutSession()
        {
            const uint requestHandle = 1234;
            var serverMock = new Mock<IUaServerData>();
            serverMock.Setup(s => s.Telemetry).Returns(NUnitTelemetryContext.Create());

            using var requestManager = new RequestManager(serverMock.Object);
            var context = new UaServerOperationContext(
                new RequestHeader { RequestHandle = requestHandle },
                null,
                RequestType.ActivateSession);

            requestManager.RequestReceived(context);

            uint cancelCount = 0;
            Assert.DoesNotThrow(
                () => requestManager.CancelRequests(context.SessionId, requestHandle, out cancelCount));

            Assert.That(cancelCount, Is.EqualTo(1));
            Assert.That(
                context.OperationStatus.Code,
                Is.EqualTo(StatusCodes.BadRequestCancelledByRequest));
        }

        [Test]
        public void CancelRequestsDoesNotCancelMatchingHandleFromDifferentSession()
        {
            var serverMock = new Mock<IUaServerData>();
            serverMock.Setup(s => s.Telemetry).Returns(NUnitTelemetryContext.Create());

            using var requestManager = new RequestManager(serverMock.Object);

            var cancellingSession = new Mock<IUaSession>();
            cancellingSession.Setup(s => s.Id).Returns(new NodeId(1));

            var otherSession = new Mock<IUaSession>();
            otherSession.Setup(s => s.Id).Returns(new NodeId(2));

            const uint requestHandle = 42;

            var ownContext = new UaServerOperationContext(
                new RequestHeader { RequestHandle = requestHandle },
                null,
                RequestType.Read,
                cancellingSession.Object);
            var otherContext = new UaServerOperationContext(
                new RequestHeader { RequestHandle = requestHandle },
                null,
                RequestType.Read,
                otherSession.Object);

            requestManager.RequestReceived(ownContext);
            requestManager.RequestReceived(otherContext);

            requestManager.CancelRequests(cancellingSession.Object.Id, requestHandle, out uint cancelCount);

            Assert.That(cancelCount, Is.EqualTo(1));
            Assert.That(
                ownContext.OperationStatus.Code,
                Is.EqualTo(StatusCodes.BadRequestCancelledByRequest));
            Assert.That(
                otherContext.OperationStatus.Code,
                Is.EqualTo(StatusCodes.Good));
        }
    }
}
