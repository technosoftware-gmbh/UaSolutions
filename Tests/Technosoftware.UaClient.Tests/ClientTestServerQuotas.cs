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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Moq;

using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

using Opc.Ua;
using Opc.Ua.Bindings;
using Opc.Ua.Configuration;

using Technosoftware.UaServer.Tests;
using SampleCompany.NodeManagers.Reference;
#endregion

namespace Technosoftware.UaClient.Tests
{
    public class ClientTestServerQuotas : ClientTestFramework
    {
        const int MaxByteStringLengthForTest = 4096;
        public ClientTestServerQuotas() : base(Utils.UriSchemeOpcTcp)
        {
        }

        public ClientTestServerQuotas(string uriScheme = Utils.UriSchemeOpcTcp) :
            base(uriScheme)
        {
        }

        #region Test Setup
        /// <summary>
        /// Set up a Server and a Client instance.
        /// </summary>
        [OneTimeSetUp]
        public new Task OneTimeSetUp()
        {
            SupportsExternalServerUrl = true;
            return base.OneTimeSetUpAsync();
        }

        /// <summary>
        /// Tear down the Server and the Client.
        /// </summary>
        [OneTimeTearDown]
        public new Task OneTimeTearDownAsync()
        {
            return base.OneTimeTearDownAsync();
        }

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public new Task SetUp()
        {
            return base.SetUp();
        }

        public override async Task CreateReferenceServerFixture(bool enableTracing, bool disableActivityLogging, bool securityNone, TextWriter writer)
        {
            {
                // start Ref server
                ServerFixture = new ServerFixture<ReferenceServer>(enableTracing, disableActivityLogging)
                {
                    UriScheme = UriScheme,
                    SecurityNone = securityNone,
                    AutoAccept = true,
                    AllNodeManagers = true,
                    OperationLimits = true
                };
            }

            if (writer != null)
            {
                ServerFixture.TraceMasks = Utils.TraceMasks.Error | Utils.TraceMasks.Security;
            }

            await ServerFixture.LoadConfiguration(PkiRoot).ConfigureAwait(false);
            ServerFixture.Config.TransportQuotas.MaxMessageSize = TransportQuotaMaxMessageSize;
            ServerFixture.Config.TransportQuotas.MaxByteStringLength = MaxByteStringLengthForTest;
            ServerFixture.Config.TransportQuotas.MaxStringLength = TransportQuotaMaxStringLength;
            ServerFixture.Config.ServerConfiguration.UserTokenPolicies.Add(new UserTokenPolicy(UserTokenType.UserName));
            ServerFixture.Config.ServerConfiguration.UserTokenPolicies.Add(new UserTokenPolicy(UserTokenType.Certificate));
            ServerFixture.Config.ServerConfiguration.UserTokenPolicies.Add(
                new UserTokenPolicy(UserTokenType.IssuedToken) { IssuedTokenType = Opc.Ua.Profiles.JwtUserToken });

            ReferenceServer = await ServerFixture.StartAsync(writer ?? TestContext.Out).ConfigureAwait(false);
            ReferenceServer.TokenValidator = this.TokenValidator;
            ServerFixturePort = ServerFixture.Port;
        }

        /// <summary>
        /// Test teardown.
        /// </summary>
        [TearDown]
        public new Task TearDown()
        {
            return base.TearDown();
        }
        #endregion

        #region Benchmark Setup
        /// <summary>
        /// Global Setup for benchmarks.
        /// </summary>
        [GlobalSetup]
        public new void GlobalSetup()
        {
            base.GlobalSetup();
        }

        /// <summary>
        /// Global cleanup for benchmarks.
        /// </summary>
        [GlobalCleanup]
        public new void GlobalCleanup()
        {
            base.GlobalCleanup();
        }
        #endregion

        #region Test Methods

        [Test, Order(100)]
        public void ReadDictionaryByteString()
        {
            List<NodeId> dictionaryIds = [VariableIds.OpcUa_BinarySchema, GetTestDataDictionaryNodeId()];

            Session theSession = ((Session)(((TraceableSession)Session).Session));

            foreach (NodeId dataDictionaryId in dictionaryIds)
            {
                ReferenceDescription referenceDescription = new ReferenceDescription();

                referenceDescription.NodeId = NodeId.ToExpandedNodeId(dataDictionaryId, theSession.NodeCache.NamespaceUris);

                // make sure the dictionary is too large to fit in a single message
                ReadValueId readValueId = new ReadValueId
                {
                    NodeId = dataDictionaryId,
                    AttributeId = Attributes.Value,
                    IndexRange = null,
                    DataEncoding = null
                };

                ReadValueIdCollection nodesToRead = [
                readValueId
            ];

                var x = Assert.Throws<ServiceResultException>(() =>
                {
                    theSession.Read(null, 0, TimestampsToReturn.Neither, nodesToRead, out DataValueCollection results, out DiagnosticInfoCollection diagnosticInfos);
                });

                Assert.AreEqual(StatusCodes.BadEncodingLimitsExceeded, x.StatusCode);

                // now ensure we get the dictionary in chunks
                DataDictionary dictionary = theSession.LoadDataDictionary(referenceDescription);
                Assert.IsNotNull(dictionary);

                // Sanity checks: verify that some well-known information is present
                Assert.AreEqual(dictionary.TypeSystemName, "OPC Binary");

                if (dataDictionaryId == dictionaryIds[0])
                {
                    Assert.IsTrue(dictionary.DataTypes.Count > 160);
                    Assert.IsTrue(dictionary.DataTypes.ContainsKey(VariableIds.OpcUa_BinarySchema_Union));
                    Assert.IsTrue(dictionary.DataTypes.ContainsKey(VariableIds.OpcUa_BinarySchema_OptionSet));
                    Assert.AreEqual("http://opcfoundation.org/UA/", dictionary.TypeDictionary.TargetNamespace);
                }
                else if (dataDictionaryId == dictionaryIds[1])
                {
                    Assert.IsTrue(dictionary.DataTypes.Count >= 10);
                    Assert.AreEqual("http://samplecompany.com/SampleServer/NodeManagers/TestData", dictionary.TypeDictionary.TargetNamespace);
                }
            }
        }

        [Test, Order(200)]
        public void TestBoundaryCaseForReadingChunks()
        {

            Session theSession = ((Session)(((TraceableSession)Session).Session));

            int NamespaceIndex = theSession.NamespaceUris.GetIndex("http://samplecompany.com/SampleServer/NodeManagers/Reference");
            NodeId NodeId = new NodeId($"ns={NamespaceIndex};s=Scalar_Static_ByteString");

            Random random = new Random();

            byte[] chunk = new byte[MaxByteStringLengthForTest];
            random.NextBytes(chunk);

            WriteValue WriteValue = new WriteValue
            {
                NodeId = NodeId,
                AttributeId = Attributes.Value,
                Value = new DataValue() { WrappedValue = new Variant(chunk) },
                IndexRange = null
            };
            WriteValueCollection writeValues = [
                    WriteValue
                ];
            theSession.Write(null, writeValues, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos);

            if (results[0] != StatusCodes.Good)
            {
                Assert.Fail($"Write failed with status code {results[0]}");
            }

            byte[] readData = theSession.ReadByteStringInChunks(NodeId);

            Assert.IsTrue(Utils.IsEqual(chunk, readData));
        }
        #endregion // Test Methods

        #region // helper methods

        /// <summary>
        /// retrieve the node id of the test data dictionary without relying on
        /// hard coded identifiers
        /// </summary>
        /// <returns></returns>
        public NodeId GetTestDataDictionaryNodeId()
        {
            BrowseDescription browseDescription = new BrowseDescription()
            {
                NodeId = ObjectIds.OPCBinarySchema_TypeSystem,
                BrowseDirection = BrowseDirection.Forward,
                ReferenceTypeId = ReferenceTypeIds.HasComponent,
                IncludeSubtypes = true,
                NodeClassMask = (uint)NodeClass.Variable,
                ResultMask = (uint)BrowseResultMask.All
            };
            BrowseDescriptionCollection browseDescriptions = [browseDescription];

            Session.Browse(null, null, 0, browseDescriptions, out BrowseResultCollection results, out DiagnosticInfoCollection diagnosticInfos);

            if (results[0] == null || results[0].StatusCode != StatusCodes.Good)
            {
                throw new Exception("cannot read the id of the test dictionary");
            }
            ReferenceDescription referenceDescription = results[0].References.FirstOrDefault(a => a.BrowseName.Name == "SampleCompany.NodeManagers.TestData");
            NodeId result = ExpandedNodeId.ToNodeId(referenceDescription.NodeId, Session.NamespaceUris);
            return result;

        }
        #endregion // helper methods

    }
}
