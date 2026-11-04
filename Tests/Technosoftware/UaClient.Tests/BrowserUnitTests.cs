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
using System.Threading.Tasks;
using System.Threading;
using Moq;
using NUnit.Framework;
using Technosoftware.Tests;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaClient.Tests
{
    /// <summary>
    /// Unit tests for the continuation point handling of the browser.
    /// </summary>
    [TestFixture]
    [Category("Client")]
    [Parallelizable]
    public class BrowserUnitTests
    {
        /// <summary>
        /// A server which signals "no more references" with an empty (zero length)
        /// continuation point must not trigger a BrowseNext call, because such a
        /// call is rejected by the server with BadContinuationPointInvalid.
        /// </summary>
        [Test]
        [TestCase(new byte[0])]
        [TestCase((byte[])null)]
        public async Task BrowseAsyncDoesNotCallBrowseNextWithoutContinuationPointAsync(
            byte[] continuationPoint)
        {
            var sessionMock = new Mock<ISessionClient>(MockBehavior.Strict);
            SetupBrowse(sessionMock, CreateReferences("Browsed"), continuationPoint);

            var browser = new Browser(NUnitTelemetryContext.Create())
            {
                Session = sessionMock.Object
            };

            ArrayOf<ReferenceDescription> references = await browser
                .BrowseAsync(ObjectIds.ObjectsFolder)
                .ConfigureAwait(false);

            Assert.That(references, Has.Count.EqualTo(1));
            Assert.That(references[0].DisplayName.Text, Is.EqualTo("Browsed"));

            sessionMock.Verify(
                x => x.BrowseNextAsync(
                    It.IsAny<RequestHeader>(),
                    It.IsAny<bool>(),
                    It.IsAny<ArrayOf<ByteString>>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        /// <summary>
        /// A non empty continuation point is followed until the server stops
        /// returning one.
        /// </summary>
        [Test]
        public async Task BrowseAsyncFollowsContinuationPointUntilExhaustedAsync()
        {
            var sessionMock = new Mock<ISessionClient>(MockBehavior.Strict);
            SetupBrowse(sessionMock, CreateReferences("First"), [1, 2, 3]);
            SetupBrowseNext(sessionMock, CreateReferences("Second"), []);

            var browser = new Browser(NUnitTelemetryContext.Create())
            {
                Session = sessionMock.Object,
                ContinueUntilDone = true
            };

            ArrayOf<ReferenceDescription> references = await browser
                .BrowseAsync(ObjectIds.ObjectsFolder)
                .ConfigureAwait(false);

            Assert.That(references, Has.Count.EqualTo(2));
            Assert.That(references[0].DisplayName.Text, Is.EqualTo("First"));
            Assert.That(references[1].DisplayName.Text, Is.EqualTo("Second"));

            sessionMock.Verify(
                x => x.BrowseNextAsync(
                    It.IsAny<RequestHeader>(),
                    false,
                    It.IsAny<ArrayOf<ByteString>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private static void SetupBrowse(
            Mock<ISessionClient> sessionMock,
            ArrayOf<ReferenceDescription> references,
            byte[] continuationPoint)
        {
            sessionMock
                .Setup(x => x.BrowseAsync(
                    It.IsAny<RequestHeader>(),
                    It.IsAny<ViewDescription>(),
                    It.IsAny<uint>(),
                    It.IsAny<ArrayOf<BrowseDescription>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BrowseResponse
                {
                    ResponseHeader = new ResponseHeader(),
                    Results = CreateResults(references, continuationPoint),
                    DiagnosticInfos = []
                });
        }

        private static void SetupBrowseNext(
            Mock<ISessionClient> sessionMock,
            ArrayOf<ReferenceDescription> references,
            byte[] continuationPoint)
        {
            sessionMock
                .Setup(x => x.BrowseNextAsync(
                    It.IsAny<RequestHeader>(),
                    It.IsAny<bool>(),
                    It.IsAny<ArrayOf<ByteString>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new BrowseNextResponse
                {
                    ResponseHeader = new ResponseHeader(),
                    Results = CreateResults(references, continuationPoint),
                    DiagnosticInfos = []
                });
        }

        private static ArrayOf<BrowseResult> CreateResults(
            ArrayOf<ReferenceDescription> references,
            byte[] continuationPoint)
        {
            return
            [
                new BrowseResult
                {
                    StatusCode = StatusCodes.Good,
                    ContinuationPoint = ByteString.From(continuationPoint),
                    References = references
                }
            ];
        }

        private static ArrayOf<ReferenceDescription> CreateReferences(string displayName)
        {
            return
            [
                new ReferenceDescription
                {
                    NodeId = new ExpandedNodeId(new NodeId(displayName, 1)),
                    DisplayName = LocalizedText.From(displayName),
                    BrowseName = new QualifiedName(displayName, 1),
                    ReferenceTypeId = ReferenceTypeIds.HasComponent,
                    IsForward = true,
                    NodeClass = NodeClass.Object,
                    TypeDefinition = ObjectTypeIds.BaseObjectType
                }
            ];
        }
    }
}
