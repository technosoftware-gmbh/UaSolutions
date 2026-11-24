#region Copyright (c) 2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com  
// 
// Purpose: 
// 
//
// The Software is subject to the Technosoftware GmbH Source Code License Agreement, 
// which can be found here:
// https://technosoftware.com/documents/Source_License_Agreement.pdf
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Technosoftware.UaUtilities;
#endregion Using Directives

namespace Technosoftware.UaUtilities.Tests
{
    /// <summary>
    /// Tests for <see cref="LicenseHandler.CheckLicense"/> (checking a licence before it is
    /// handed over) and for the product type rule at run time.
    /// </summary>
    [TestFixture, Category("Licensing")]
    [SetCulture("en-us"), SetUICulture("en-us")]
    public class LicenseCheckTests
    {
        #region Test Setup
        [OneTimeSetUp]
        protected void OneTimeSetUp()
        {
            // Signed with the throw-away fixture key; see Fixtures/README.md and project doc 13.
            LicenseHandler.PublicKeyOverride = FixtureKeys.PublicKey;
        }

        [OneTimeTearDown]
        protected void OneTimeTearDown()
        {
            // Go back to the built-in evaluation licence for the fixtures that follow.
            LicenseHandler.Instance.Validate(null, string.Empty);
            LicenseHandler.PublicKeyOverride = null;
        }
        #endregion Test Setup

        #region Test Methods (Version)
        [TestCase("7.0.0", "7.0.0")]
        [TestCase("7.0.0", "7.0.1")]
        [TestCase("7.0.0", "7.3.0")]
        [TestCase("7.0.0", "7.3.5")]
        [TestCase("7.1.0", "7.1.2")]
        [TestCase("7.0.0", "6.2.1")]
        [TestCase("6.0.0", "6.2.1")]
        public void LicenseUnlocksVersion(string licenseVersion, string productVersion)
        {
            LicenseCheckResult result = LicenseHandler.CheckLicense(
                CreateLicense(licenseVersion, "ClientAndServer"),
                new Version(productVersion),
                null);

            Assert.That(result.Problems, Is.Empty);
            Assert.That(result.IsValid, Is.True);
        }

        [TestCase("7.1.0", "7.0.1")]
        [TestCase("6.2.0", "7.0.1")]
        [TestCase("5.0.0", "7.0.1")]
        [TestCase("8.0.0", "7.0.1")]
        [TestCase("6.2.0", "6.1.0")]
        public void LicenseDoesNotUnlockVersion(string licenseVersion, string productVersion)
        {
            LicenseCheckResult result = LicenseHandler.CheckLicense(
                CreateLicense(licenseVersion, "ClientAndServer"),
                new Version(productVersion),
                null);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Problems, Has.Count.EqualTo(1));
            Assert.That(result.Problems[0], Does.Contain("does not unlock"));
        }

        [Test]
        public void EarlierMajorWithSupportUnlocksVersion6Only()
        {
            string license = CreateLicense("5.0.0", "ClientAndServer", "Standard", "Thu, 31 Dec 2099");

            Assert.That(LicenseHandler.CheckLicense(license, new Version(6, 2, 1), null).IsValid, Is.True);
            Assert.That(LicenseHandler.CheckLicense(license, new Version(7, 0, 1), null).IsValid, Is.False);
        }

        [Test]
        public void EarlierMajorWithoutSupportDoesNotUnlockVersion6()
        {
            string license = CreateLicense("5.0.0", "ClientAndServer");

            Assert.That(LicenseHandler.CheckLicense(license, new Version(6, 2, 1), null).IsValid, Is.False);
        }

        [Test]
        public void OldFormatLicenseUnlocksVersion6Only()
        {
            const string licenseData =
                    @"<License>
              <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
              <Type>Developer</Type>
              <ProductType>Client</ProductType>
              <SupportType>None</SupportType>
              <ProductFeatures>
                <Feature name=""DataAccess"">yes</Feature>
                <Feature name=""AlarmsConditions"">yes</Feature>
                <Feature name=""HistoricalAccess"">yes</Feature>
                <Feature name=""AllFeatures"">yes</Feature>
                <Feature name=""Product"">OPC UA Solutions</Feature>
                <Feature name=""Version"">4.0.0</Feature>
                <Feature name=""Publish Date"">Tue, 23 Sep 2025</Feature>
              </ProductFeatures>
              <Customer>
                <Name></Name>
                <Domain></Domain>
              </Customer>
              <LicenseAttributes>
                <Attribute name=""Assembly Identity""></Attribute>
                <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
              </LicenseAttributes>
              <Signature>MIGGAkEFU8ozwp7KldVkfH2sXOgkJ7/5u3eHwJEB/paNbTrwztPn8L75GYU7gbvTR2X/q1EkoSDy7Q5LT4bYs6ucnk8PhQJBdSUySEoBHZDgBN/0E/UmMQvrUDYz8GTs7E/KWhTO7U2HSpAn+/sAebIOELLh+WcyFw70GZlT+kcFFm4Z3+RNIHE=</Signature>
            </License>";

            Assert.That(LicenseHandler.CheckLicense(licenseData, new Version(6, 2, 1), null).IsValid, Is.True);
            Assert.That(LicenseHandler.CheckLicense(licenseData, new Version(7, 0, 1), null).IsValid, Is.False);
        }
        #endregion Test Methods (Version)

        #region Test Methods (Product Type)
        [TestCase("Client", ProductType.Client, true)]
        [TestCase("Client", ProductType.Server, false)]
        [TestCase("Server", ProductType.Server, true)]
        [TestCase("Server", ProductType.Client, false)]
        [TestCase("ClientAndServer", ProductType.Client, true)]
        [TestCase("ClientAndServer", ProductType.Server, true)]
        [TestCase("ClientGateway", ProductType.Server, true)]
        [TestCase("ClientGateway", ProductType.Client, false)]
        public void LicenseCoversProductType(string licensedType, ProductType requiredType, bool expected)
        {
            LicenseCheckResult result = LicenseHandler.CheckLicense(
                CreateLicense("7.0.0", licensedType),
                new Version(7, 0, 1),
                requiredType);

            Assert.That(result.IsValid, Is.EqualTo(expected));
        }

        [Test]
        public void ClientLicenseDoesNotStartServer()
        {
            LicenseHandler.SimulateServicePatch = false;
            bool licensed = LicenseHandler.Instance.Validate(null, CreateLicense("7.0.0", "Client"));
            if (!licensed)
            {
                Assert.Ignore("The test assembly is not built as version 7.0; omitting.");
            }

            Assert.That(LicenseHandler.Instance.IsLicenseValid(ProductType.Client), Is.True);
            Assert.That(LicenseHandler.Instance.IsLicenseValid(ProductType.Server), Is.False);
            Assert.DoesNotThrow(() => LicenseHandler.Instance.ValidateFeatures(
                ProductType.Client, ProductFeature.None, NullLogger.Instance));
            Assert.Throws<NotSupportedException>(() => LicenseHandler.Instance.ValidateFeatures(
                ProductType.Server, ProductFeature.None, NullLogger.Instance));
        }

        [Test]
        public void ServerLicenseStartsServer()
        {
            LicenseHandler.SimulateServicePatch = false;
            bool licensed = LicenseHandler.Instance.Validate(null, CreateLicense("7.0.0", "Server"));
            if (!licensed)
            {
                Assert.Ignore("The test assembly is not built as version 7.0; omitting.");
            }

            Assert.DoesNotThrow(() => LicenseHandler.Instance.ValidateFeatures(
                ProductType.Server, ProductFeature.None, NullLogger.Instance));
        }
        #endregion Test Methods (Product Type)

        #region Test Methods (Other Checks)
        [Test]
        public void TrialLicenseIsReported()
        {
            LicenseCheckResult result = LicenseHandler.CheckLicense(
                CreateLicense("7.0.0", "ClientAndServer", level: LicenseType.Trial),
                new Version(7, 0, 1),
                null);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Problems[0], Does.Contain("Trial"));
        }

        [Test]
        public void ExpiredLicenseIsReported()
        {
            LicenseCheckResult result = LicenseHandler.CheckLicense(
                CreateLicense("7.0.0", "ClientAndServer", expirationDate: "Wed, 01 Jan 2025"),
                new Version(7, 0, 1),
                null);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Problems[0], Does.Contain("expired"));
        }

        [Test]
        public void ChangedLicenseIsRejected()
        {
            string licenseData = CreateLicense("7.0.0", "Client").Replace(
                "<Type>Client</Type>",
                "<Type>ClientAndServer</Type>");

            LicenseCheckResult result = LicenseHandler.CheckLicense(licenseData, new Version(7, 0, 1), null);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Problems[0], Does.Contain("signature"));
        }

        [Test]
        public void UnreadableLicenseIsRejected()
        {
            Assert.That(LicenseHandler.CheckLicense("not a licence", new Version(7, 0, 1), null).IsValid, Is.False);
            Assert.That(LicenseHandler.CheckLicense(string.Empty, new Version(7, 0, 1), null).IsValid, Is.False);
        }

        [Test]
        public void CheckLeavesAppliedLicenseUnchanged()
        {
            LicenseHandler.SimulateServicePatch = false;
            LicenseHandler.Instance.Validate(null, CreateLicense("7.0.0", "Server"));
            ProductType before = LicenseHandler.Instance.LicensedType;

            LicenseHandler.CheckLicense(CreateLicense("7.0.0", "Client"), new Version(7, 0, 1), null);

            Assert.That(LicenseHandler.Instance.LicensedType, Is.EqualTo(before));
        }
        #endregion Test Methods (Other Checks)

        #region Helpers
        private static string CreateLicense(
            string version,
            string productType,
            string supportLevel = "None",
            string supportExpirationDate = "",
            LicenseType level = LicenseType.CompanySite,
            string expirationDate = "")
        {
            string productIdentity = SecureHash.ComputeSHA256Hash("OPC UA Solutions " + FixtureKeys.PublicKey);

            License license = License.New()
                .WithUniqueIdentifier(Guid.NewGuid())
                .WithProduct("OPC UA Solutions", productType, version, "Wed, 01 Jul 2026")
                .WithLicenseInfo(level.ToString(), expirationDate)
                .WithSupport(supportLevel, supportExpirationDate)
                .LicensedTo("Fixture", "Fixture AG", "fixture.invalid")
                .WithProductFeatures(new Dictionary<string, string> { { "AllFeatures", "yes" } })
                .WithAdditionalAttributes(new Dictionary<string, string>
                {
                    { "Product Identity", productIdentity },
                    { "Assembly Identity", string.Empty }
                })
                .CreateAndSignWithPrivateKey(FixtureKeys.PrivateKey, FixtureKeys.PassPhrase);

            return license.ToString();
        }
        #endregion Helpers
    }
}
