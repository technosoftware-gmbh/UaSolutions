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
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
using NUnit.Framework;
using Opc.Ua;
using SampleCompany.NodeManagers.Reference;
using Technosoftware.UaServer;
#endregion Using Directives

namespace Technosoftware.UaServer.Tests
{
    /// <summary>
    /// CTT Historical Access Read Raw / Err-017: read modified with ReturnBounds.
    /// </summary>
    /// <remarks>
    /// OPC 10000-11 6.5.3.3 (Read modified functionality): "The returnBounds
    /// parameter shall be set to FALSE for this case, otherwise the Server returns
    /// a Bad_InvalidArgument StatusCode." The CTT expects this code as the
    /// operation result of the node, with a Good service result.
    /// </remarks>
    [TestFixture]
    [Category("Server")]
    [Category("HistoryRead")]
    [SetCulture("en-us")]
    [SetUICulture("en-us")]
    public class HistoryReadModifiedCttTests
    {
        private const string kReferenceNode = "Reference";
        private const string kTestDataNode = "TestData";

        private ServerFixture<ReferenceServer> m_fixture;
        private ReferenceServer m_server;
        private RequestHeader m_requestHeader;
        private SecureChannelContext m_secureChannelContext;

        /// <summary>
        /// Both historizing node managers of the Reference Server.
        /// </summary>
        // The SampleCompany reference server does not historize the static Reference
        // variables the upstream Quickstarts server uses, so only TestData is read.
        public static readonly string[] HistoricalNodes = [kTestDataNode];

        [OneTimeSetUp]
        public async Task OneTimeSetUpAsync()
        {
            m_fixture = new ServerFixture<ReferenceServer>
            {
                AllNodeManagers = true,
                OperationLimits = true,
                DurableSubscriptionsEnabled = false,
                UseSamplingGroupsInReferenceNodeManager = false
            };
            m_server = await m_fixture.StartAsync().ConfigureAwait(false);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDownAsync()
        {
            await m_fixture.StopAsync().ConfigureAwait(false);
        }

        [SetUp]
        public async Task SetUpAsync()
        {
            (m_requestHeader, m_secureChannelContext) = await m_server
                .CreateAndActivateSessionAsync(TestContext.CurrentContext.Test.Name)
                .ConfigureAwait(false);
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            m_requestHeader.Timestamp = DateTime.UtcNow;
            await m_server.CloseSessionAsync(
                m_secureChannelContext,
                m_requestHeader,
                true,
                CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// readrawErr017: IsReadModified = true, ReturnBounds = true, StartTime = first
        /// record, EndTime = the record after the next, NumValuesPerNode = 0: the service
        /// succeeds and the operation result is Bad_InvalidArgument without data.
        /// </summary>
        [TestCaseSource(nameof(HistoricalNodes))]
        public async Task ReadRawErr017_ReadModifiedWithBoundsReturnsBadInvalidArgumentAsync(string node)
        {
            NodeId nodeId = GetNodeId(node);
            DateTime[] raw = await ReadFirstRecordsAsync(nodeId, 3).ConfigureAwait(false);

            HistoryReadResponse response = await HistoryReadAsync(
                nodeId,
                new ReadRawModifiedDetails
                {
                    IsReadModified = true,
                    StartTime = raw[0],
                    EndTime = raw[2],
                    NumValuesPerNode = 0,
                    ReturnBounds = true
                }).ConfigureAwait(false);

            Assert.AreEqual((StatusCode)StatusCodes.Good, response.ResponseHeader.ServiceResult);
            Assert.AreEqual(1, response.Results.Count);
            HistoryReadResult result = response.Results[0];
            Assert.AreEqual((StatusCode)StatusCodes.BadInvalidArgument, result.StatusCode);
            Assert.IsTrue(
                result.ContinuationPoint == null || result.ContinuationPoint.Length == 0,
                "No ContinuationPoint expected.");
            Assert.AreEqual(
                0,
                (result.HistoryData?.Body as HistoryData)?.DataValues?.Count ?? 0,
                "No data expected with a Bad operation result.");
        }

        /// <summary>
        /// Controls: only the combination of read modified and bounds is invalid.
        /// A read modified without bounds and a raw read with bounds are not rejected.
        /// </summary>
        [TestCaseSource(nameof(HistoricalNodes))]
        public async Task ReadModifiedWithoutBoundsAndRawWithBoundsAreNotRejectedAsync(string node)
        {
            NodeId nodeId = GetNodeId(node);
            DateTime[] raw = await ReadFirstRecordsAsync(nodeId, 3).ConfigureAwait(false);

            foreach ((bool isReadModified, bool returnBounds) in new[] { (true, false), (false, true) })
            {
                HistoryReadResponse response = await HistoryReadAsync(
                    nodeId,
                    new ReadRawModifiedDetails
                    {
                        IsReadModified = isReadModified,
                        StartTime = raw[0],
                        EndTime = raw[2],
                        NumValuesPerNode = 0,
                        ReturnBounds = returnBounds
                    }).ConfigureAwait(false);

                Assert.AreEqual((StatusCode)StatusCodes.Good, response.ResponseHeader.ServiceResult);
                Assert.AreNotEqual(
                    (StatusCode)StatusCodes.BadInvalidArgument,
                    response.Results[0].StatusCode,
                    $"IsReadModified={isReadModified}, ReturnBounds={returnBounds}");
            }
        }

        private NodeId GetNodeId(string node)
        {
            NamespaceTable namespaceUris = m_server.CurrentInstance.NamespaceUris;
            return node switch
            {
                kReferenceNode => new NodeId(
                    "Scalar_Static_Int32",
                    (ushort)namespaceUris.GetIndex(SampleCompany.NodeManagers.Reference.Namespaces.ReferenceServer)),
                kTestDataNode => new NodeId(
                    SampleCompany.NodeManagers.TestData.Variables.Data_Dynamic_Scalar_Int32Value,
                    (ushort)namespaceUris.GetIndex(SampleCompany.NodeManagers.TestData.Namespaces.TestData)),
                _ => throw new ArgumentOutOfRangeException(nameof(node))
            };
        }

        private Task<HistoryReadResponse> HistoryReadAsync(NodeId nodeId, ReadRawModifiedDetails details)
        {
            m_requestHeader.Timestamp = DateTime.UtcNow;
            return m_server.HistoryReadAsync(
                m_secureChannelContext,
                m_requestHeader,
                new ExtensionObject(details),
                TimestampsToReturn.Source,
                false,
                new HistoryReadValueIdCollection { new HistoryReadValueId { NodeId = nodeId } },
                CancellationToken.None);
        }

        private async Task<DateTime[]> ReadFirstRecordsAsync(NodeId nodeId, uint count)
        {
            HistoryReadResponse response = await HistoryReadAsync(
                nodeId,
                new ReadRawModifiedDetails
                {
                    // the generated defaults of both flags are true
                    IsReadModified = false,
                    ReturnBounds = false,
                    StartTime = DateTime.UtcNow.AddDays(-1),
                    EndTime = DateTime.MinValue,
                    NumValuesPerNode = count
                }).ConfigureAwait(false);
            Assert.AreEqual((StatusCode)StatusCodes.Good, response.ResponseHeader.ServiceResult);
            Assert.IsTrue(
                StatusCode.IsGood(response.Results[0].StatusCode),
                $"Reading the first records failed with {response.Results[0].StatusCode}.");
            DateTime[] times = [.. ((HistoryData)response.Results[0].HistoryData.Body)
                .DataValues.Select(v => v.SourceTimestamp)];
            Assert.AreEqual(count, (uint)times.Length);
            return times;
        }
    }
}
