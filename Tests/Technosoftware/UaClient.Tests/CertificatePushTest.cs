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
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using NUnit.Framework;
using Opc.Ua;
using Opc.Ua.Security.Certificates;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
#endregion Using Directives

namespace Technosoftware.UaClient.Tests
{
    /// <summary>
    /// Tests for the ServerConfiguration certificate push methods.
    /// </summary>
    [TestFixture]
    [Category("Client")]
    [SetCulture("en-us")]
    [SetUICulture("en-us")]
    [NonParallelizable]
    public class CertificatePushTest : ClientTestFramework
    {
        /// <summary>
        /// Set up a Server and a Client instance.
        /// </summary>
        [OneTimeSetUp]
        public override Task OneTimeSetUpAsync()
        {
            return OneTimeSetUpCoreAsync();
        }

        /// <summary>
        /// Tear down the Server and the Client.
        /// </summary>
        [OneTimeTearDown]
        public override Task OneTimeTearDownAsync()
        {
            return base.OneTimeTearDownAsync();
        }

        /// <summary>
        /// UpdateCertificate for the application group must validate the new
        /// certificate against the server's configured trust stores. An issuer
        /// supplied in the request is ignored, so a certificate signed by a CA the
        /// server does not trust is rejected even when that CA is passed along
        /// (OPCFoundation/UA-.NETStandard#4678, #4683).
        /// </summary>
        [Test]
        public async Task UpdateCertificateIgnoresSuppliedUntrustedIssuerAsync()
        {
            IUaSession session = await ClientFixture
                .ConnectAsync(
                    ServerUrl,
                    SecurityPolicies.Basic256Sha256,
                    default,
                    new UserIdentity("sysadmin", "demo"u8))
                .ConfigureAwait(false);
            try
            {
                if (session.Endpoint.SecurityMode != MessageSecurityMode.SignAndEncrypt)
                {
                    Assert.Inconclusive("UpdateCertificate requires a SignAndEncrypt session.");
                }

                using Certificate serverCertificate = Utils.ParseCertificateBlob(
                    session.ConfiguredEndpoint.Description.ServerCertificate,
                    Telemetry);
                using RSA serverPublicKey = serverCertificate.GetRSAPublicKey();
                Assert.NotNull(serverPublicKey, "The Basic256Sha256 endpoint must use an RSA certificate.");

                // a CA that is in none of the server's trust lists.
                using Certificate untrustedCa = DefaultCertificateFactory.Instance
                    .CreateCertificate("CN=Untrusted Push Test CA, O=Technosoftware")
                    .SetCAConstraint(-1)
                    .SetRSAKeySize(CertificateFactory.DefaultKeySize)
                    .CreateForRSA();

                // a certificate for the server's existing key pair, issued by that CA.
                using Certificate issuedCertificate = DefaultCertificateFactory.Instance
                    .CreateCertificate(serverCertificate.Subject)
                    .AddExtension(new X509SubjectAltNameExtension(
                        [ServerFixture.Config.ApplicationUri],
                        [Utils.GetHostName(), "localhost"]))
                    .SetIssuer(untrustedCa)
                    .SetRSAPublicKey(serverPublicKey)
                    .CreateForRSA();
                Assert.IsFalse(X509Utils.IsSelfSigned(issuedCertificate));

                var request = new CallMethodRequest
                {
                    ObjectId = ObjectIds.ServerConfiguration,
                    MethodId = MethodIds.ServerConfiguration_UpdateCertificate,
                    InputArguments =
                    [
                        Variant.From(NodeId.Null),
                        Variant.From(ObjectTypeIds.RsaSha256ApplicationCertificateType),
                        Variant.From(issuedCertificate.RawData.ToByteString()),
                        Variant.From(new[] { untrustedCa.RawData.ToByteString() }.ToArrayOf()),
                        Variant.From(string.Empty),
                        Variant.From(ByteString.Empty)
                    ]
                };

                CallResponse response = await session
                    .CallAsync(null, [request], default)
                    .ConfigureAwait(false);

                Assert.AreEqual(1, response.Results.Count);
                // the 2.0 validator reports the missing issuer itself; it is not
                // suppressible, so no accept callback can override it.
                Assert.AreEqual(
                    (StatusCode)StatusCodes.BadCertificateChainIncomplete,
                    response.Results[0].StatusCode,
                    "A certificate signed by an untrusted CA must be rejected, " +
                    "even when the CA is supplied as issuer.");
            }
            finally
            {
                await session.CloseAsync().ConfigureAwait(false);
                session.Dispose();
            }
        }
    }
}
