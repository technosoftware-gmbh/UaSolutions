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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnit.Framework;
using Opc.Ua;
using Technosoftware.Tests;
#endregion Using Directives

namespace Technosoftware.UaConfiguration.Tests
{
    /// <summary>
    /// Tests for the security configuration class.
    /// </summary>
    [TestFixture]
    [Category("SecurityConfiguration")]
    [SetCulture("en-us")]
    public class SecurityConfigurationTests
    {
        [Test]
        public void ValidConfgurationPasses()
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();

            var configuration = new SecurityConfiguration
            {
                ApplicationCertificate = new CertificateIdentifier { RawData = [] },
                TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" }
            };

            configuration.Validate(telemetry);
        }

        [TestCaseSource(nameof(GetInvalidConfigurations))]
        public void InvalidConfigurationThrows(SecurityConfiguration configuration)
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();
            Assert.Throws<ServiceResultException>(() => configuration.Validate(telemetry));
        }

        [Test]
        public async Task LoadingConfigurationWithApplicationCertificateShouldMarkItDeprecatedAsync()
        {
            string file = Path.Combine(TestContext.CurrentContext.WorkDirectory, "testlegacyconfig.xml");

            using var stream = new FileStream(file, FileMode.Open);
            ApplicationConfiguration reloadedConfiguration =
                DecodeApplicationConfiguration(stream);

            Assert.That(
                reloadedConfiguration.SecurityConfiguration.IsDeprecatedConfiguration,
                Is.True);
        }

        [Test]
        public async Task LoadingConfigurationWithApplicationCertificateAndApplicationCertificatesShouldNotMarkItDeprecatedAsync()
        {
            string file = Path.Combine(TestContext.CurrentContext.WorkDirectory, "testhybridconfig.xml");

            using var stream = new FileStream(file, FileMode.Open);
            ApplicationConfiguration reloadedConfiguration =
                DecodeApplicationConfiguration(stream);

            Assert.That(
                reloadedConfiguration.SecurityConfiguration.IsDeprecatedConfiguration,
                Is.False);
        }

        [Test]
        public void SavingConfigurationShouldNotMarkItDeprecated()
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();

            var securityConfiguration = new SecurityConfiguration
            {
                ApplicationCertificates =
                [
                    new CertificateIdentifier
                    {
                        StoreType = CertificateStoreType.Directory,
                        StorePath = "pki/own",
                        CertificateType = ObjectTypeIds.RsaSha256ApplicationCertificateType
                    }
                ],
                TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" }
            };

            var configuration = new ApplicationConfiguration(telemetry)
            {
                ApplicationName = "DeprecatedConfigurationTest",
                ApplicationUri = "urn:localhost:DeprecatedConfigurationTest",
                ApplicationType = ApplicationType.Server,
                SecurityConfiguration = securityConfiguration
            };

            using var stream = new MemoryStream(
                Encoding.UTF8.GetBytes(EncodeApplicationConfiguration(configuration)));

            ApplicationConfiguration reloadedConfiguration =
                DecodeApplicationConfiguration(stream);

            Assert.That(
                reloadedConfiguration.SecurityConfiguration.IsDeprecatedConfiguration,
                Is.False,
                "Deserializing a configuration that uses ApplicationCertificates should not mark it deprecated via the legacy ApplicationCertificate setter.");
        }

        [Test]
        public void DeprecatedConfigurationRoundTripsWithLegacyElement()
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();

            var configuration = new ApplicationConfiguration(telemetry)
            {
                ApplicationName = "DeprecatedConfigurationTest",
                ApplicationUri = "urn:localhost:DeprecatedConfigurationTest",
                ApplicationType = ApplicationType.Server,
                SecurityConfiguration = new SecurityConfiguration
                {
                    TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" }
                }
            };

            configuration.SecurityConfiguration.ApplicationCertificate = new CertificateIdentifier
            {
                StoreType = CertificateStoreType.Directory,
                StorePath = "pki/own",
                CertificateType = ObjectTypeIds.RsaSha256ApplicationCertificateType
            };

            string xml = EncodeApplicationConfiguration(configuration);

            var document = XDocument.Parse(xml);
            ApplicationConfiguration roundTripped = DecodeApplicationConfiguration(
                new MemoryStream(Encoding.UTF8.GetBytes(xml)));

            Assert.That(roundTripped, Is.Not.Null);
            Assert.That(configuration.SecurityConfiguration.IsDeprecatedConfiguration, Is.True);
            Assert.That(
                document.Descendants(XName.Get("ApplicationCertificate", Namespaces.OpcUaConfig)).Any(),
                Is.True,
                "Legacy ApplicationCertificate element should be present for deprecated configurations.");
        }

        [Test]
        public void ModernConfigurationOmitsLegacyElement()
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();

            var configuration = new ApplicationConfiguration(telemetry)
            {
                ApplicationName = "ModernConfigurationTest",
                ApplicationUri = "urn:localhost:ModernConfigurationTest",
                ApplicationType = ApplicationType.Server,
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificates =
                    [
                        new CertificateIdentifier
                        {
                            StoreType = CertificateStoreType.Directory,
                            StorePath = "pki/own",
                            CertificateType = ObjectTypeIds.RsaSha256ApplicationCertificateType
                        }
                    ],
                    TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" }
                }
            };

            string xml = EncodeApplicationConfiguration(configuration);

            var document = XDocument.Parse(xml);
            ApplicationConfiguration roundTripped = DecodeApplicationConfiguration(
                new MemoryStream(Encoding.UTF8.GetBytes(xml)));

            Assert.That(roundTripped, Is.Not.Null);
            Assert.That(configuration.SecurityConfiguration.IsDeprecatedConfiguration, Is.False);
            Assert.That(
                document.Descendants(XName.Get("ApplicationCertificate", Namespaces.OpcUaConfig)).Any(),
                Is.False,
                "Modern configurations should not emit the legacy ApplicationCertificate element.");
            Assert.That(
                document.Descendants(XName.Get("ApplicationCertificates", Namespaces.OpcUaConfig)).Any(),
                Is.True,
                "Modern configurations should emit the ApplicationCertificates element.");
        }

        [Test]
        public void DeprecatedConfigurationAlsoEmitsApplicationCertificatesElement()
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();

            var configuration = new ApplicationConfiguration(telemetry)
            {
                ApplicationName = "DeprecatedNoListConfig",
                ApplicationUri = "urn:localhost:DeprecatedNoListConfig",
                ApplicationType = ApplicationType.Server,
                SecurityConfiguration = new SecurityConfiguration
                {
                    TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" }
                }
            };

            configuration.SecurityConfiguration.ApplicationCertificate = new CertificateIdentifier
            {
                StoreType = CertificateStoreType.Directory,
                StorePath = "pki/own",
                CertificateType = ObjectTypeIds.RsaSha256ApplicationCertificateType
            };

            string xml = EncodeApplicationConfiguration(configuration);

            var document = XDocument.Parse(xml);

            Assert.That(configuration.SecurityConfiguration.IsDeprecatedConfiguration, Is.True);
            Assert.That(
                document.Descendants(XName.Get("ApplicationCertificate", Namespaces.OpcUaConfig)).Any(),
                Is.True,
                "Legacy ApplicationCertificate element should be present for deprecated configurations.");
            Assert.That(
                document.Descendants(XName.Get("ApplicationCertificates", Namespaces.OpcUaConfig)).Any(),
                Is.True,
                "The encoder always emits ApplicationCertificates when the collection is populated.");
        }

        [Test]
        public void HybridConfigurationPrefersModernElementOnSave()
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();

            var legacyCert = new CertificateIdentifier
            {
                StoreType = CertificateStoreType.Directory,
                StorePath = "pki/own",
                CertificateType = ObjectTypeIds.RsaSha256ApplicationCertificateType
            };

            var modernCert = new CertificateIdentifier
            {
                StoreType = CertificateStoreType.Directory,
                StorePath = "pki/own-modern",
                CertificateType = ObjectTypeIds.RsaSha256ApplicationCertificateType
            };

            var configuration = new ApplicationConfiguration(telemetry)
            {
                ApplicationName = "HybridConfiguration",
                ApplicationUri = "urn:localhost:HybridConfiguration",
                ApplicationType = ApplicationType.Server,
                SecurityConfiguration = new SecurityConfiguration
                {
                    TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" }
                }
            };

            // First set legacy to mark deprecated, then set the modern collection.
            configuration.SecurityConfiguration.ApplicationCertificate = legacyCert;
            configuration.SecurityConfiguration.ApplicationCertificates = [modernCert];

            string xml = EncodeApplicationConfiguration(configuration);

            var document = XDocument.Parse(xml);
            ApplicationConfiguration roundTripped = DecodeApplicationConfiguration(
                new MemoryStream(Encoding.UTF8.GetBytes(xml)));

            Assert.That(configuration.SecurityConfiguration.IsDeprecatedConfiguration, Is.False);
            Assert.That(roundTripped.SecurityConfiguration.IsDeprecatedConfiguration, Is.False);
            Assert.That(
                document.Descendants(XName.Get("ApplicationCertificate", Namespaces.OpcUaConfig)).Any(),
                Is.False,
                "Hybrid configurations should serialize as modern and omit the legacy element.");
            Assert.That(
                document.Descendants(XName.Get("ApplicationCertificates", Namespaces.OpcUaConfig)).Any(),
                Is.True,
                "Hybrid configurations should serialize the modern ApplicationCertificates element.");
            Assert.That(roundTripped.SecurityConfiguration.ApplicationCertificates.Count, Is.EqualTo(1));
        }

        private static IEnumerable<TestCaseData> GetInvalidConfigurations()
        {
            yield return new TestCaseData(
                new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { RawData = [] }
                }
            ).SetName("NoStores");

            yield return new TestCaseData(
                new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { RawData = [] },
                    TrustedPeerCertificates = new CertificateTrustList { StorePath = string.Empty },
                    TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" }
                }
            ).SetName("InvalidTrustedStore");

            yield return new TestCaseData(
                new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { RawData = [] },
                    TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedIssuerCertificates = new CertificateTrustList
                    {
                        StorePath = string.Empty
                    }
                }
            ).SetName("InvalidIssuerStore");

            yield return new TestCaseData(
                new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { RawData = [] },
                    TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedHttpsCertificates = new CertificateTrustList { StorePath = "Test" }
                }
            ).SetName("OnlyTrustedHttps");

            yield return new TestCaseData(
                new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { RawData = [] },
                    TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" },
                    HttpsIssuerCertificates = new CertificateTrustList { StorePath = "Test" }
                }
            ).SetName("OnlyIssuerHttps");

            yield return new TestCaseData(
                new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { RawData = [] },
                    TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" },
                    HttpsIssuerCertificates = new CertificateTrustList { StorePath = string.Empty },
                    TrustedHttpsCertificates = new CertificateTrustList { StorePath = "Test" }
                }
            ).SetName("InvalidHttpsIssuer");

            yield return new TestCaseData(
                new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { RawData = [] },
                    TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" },
                    HttpsIssuerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedHttpsCertificates = new CertificateTrustList { StorePath = string.Empty }
                }
            ).SetName("InvalidHttpsTrusted");

            yield return new TestCaseData(
                new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { RawData = [] },
                    TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedUserCertificates = new CertificateTrustList { StorePath = "Test" }
                }
            ).SetName("OnlyTrustedUser");

            yield return new TestCaseData(
                new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { RawData = [] },
                    TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" },
                    UserIssuerCertificates = new CertificateTrustList { StorePath = "Test" }
                }
            ).SetName("OnlyIssuerUser");

            yield return new TestCaseData(
                new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { RawData = [] },
                    TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" },
                    UserIssuerCertificates = new CertificateTrustList { StorePath = string.Empty },
                    TrustedUserCertificates = new CertificateTrustList { StorePath = "Test" }
                }
            ).SetName("InvalidUserIssuer");

            yield return new TestCaseData(
                new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { RawData = [] },
                    TrustedPeerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedIssuerCertificates = new CertificateTrustList { StorePath = "Test" },
                    UserIssuerCertificates = new CertificateTrustList { StorePath = "Test" },
                    TrustedUserCertificates = new CertificateTrustList { StorePath = string.Empty }
                }
            ).SetName("InvalidUserTrusted");
        }

        /// <remarks>
        /// ApplicationConfiguration is no longer a data contract in 2.0; it is
        /// read and written with the stack's own XML encoder, which is what
        /// puts the elements in the configuration namespace.
        /// </remarks>
        private static IServiceMessageContext CreateMessageContext()
        {
            ITelemetryContext telemetry = NUnitTelemetryContext.Create();
            using IDisposable scope = AmbientMessageContext.SetScopedContext(telemetry);
            return AmbientMessageContext.CurrentContext
                ?? ServiceMessageContext.CreateEmpty(telemetry);
        }

        private static ApplicationConfiguration DecodeApplicationConfiguration(Stream stream)
        {
            IServiceMessageContext context = CreateMessageContext();
            var parser = new XmlParser(typeof(ApplicationConfiguration), stream, context);
            var configuration = new ApplicationConfiguration();
            configuration.Decode(parser);
            return configuration;
        }

        private static string EncodeApplicationConfiguration(
            ApplicationConfiguration configuration)
        {
            IServiceMessageContext context = CreateMessageContext();
            using var stream = new MemoryStream();
            XmlWriterSettings settings = Utils.DefaultXmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                var encoder = new XmlEncoder(typeof(ApplicationConfiguration), writer, context);
                configuration.Encode(encoder);
                encoder.Close();
            }

            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
