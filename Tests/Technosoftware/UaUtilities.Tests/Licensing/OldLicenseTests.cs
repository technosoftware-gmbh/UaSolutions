#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com  
// 
// Purpose: 
// 
//
// The Software is subject to the Technosoftware GmbH Source Code License Agreement, 
// which can be found here:
// https://technosoftware.com/documents/Source_License_Agreement.pdf
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
using Technosoftware.UaUtilities;
#endregion Using Directives

namespace Technosoftware.UaUtilities.Tests
{
    /// <summary>
    /// Tests for the NodeState classes.
    /// </summary>
    [TestFixture, Category("Licensing")]
    [SetCulture("en-us"), SetUICulture("en-us")]
    public class OldLicenseTests
    {
        #region Test Setup
        [OneTimeSetUp]
        protected void OneTimeSetUp()
        {
            // The licences in this fixture are signed with the throw-away fixture key,
            // not the production one, so that none of them can license a shipped package.
            // The override only exists in a -p:LicenseTestSeam=true build; see
            // Fixtures/README.md and project doc 13.
            LicenseHandler.PublicKeyOverride = FixtureKeys.PublicKey;
        }

        [OneTimeTearDown]
        protected void OneTimeTearDown()
        {
            LicenseHandler.PublicKeyOverride = null;
        }
        #endregion Test Setup

        #region Test Methods (Old License Rejection)
        /// <summary>
        /// From version 7 on, licenses in the old format are retired and are rejected
        /// whatever product or features they name.
        /// </summary>
        /// <remarks>
        /// This fixture used to hold eleven tests covering product and feature
        /// combinations under the previous behaviour, plus a service patch case. They were removed rather than inverted: inverted, each of
        /// them would have asserted this same single fact. The feature and product
        /// combinations themselves stay covered by <see cref="NewLicenseTests"/>.
        /// </remarks>
        [Test]
        public void OldLicenseIsRejected()
        {
            LicenseHandler.SimulateServicePatch = false;

            const string licenseData =
                    @"<License>
  <Id>3883f548-935e-4f3c-8148-54e438445825</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""UA Client .NET"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGIAkIBRHtMGtH/YsLhbK704YbrzFPT7LBqjN+mU6MBcg+sb5pCkClWz1k0kIrW3ZsDjdleW8cP8ux7RhA1NwtkfCvqMGYCQgE9Ev/XVk9O3XzQZo58sDZ2raidgd3mvCreiMUHjv51iorDVMu6S+vEt725l/a88Bj6OnpW2mCOp0YlQD58gGGtjA==</Signature>
</License>";

            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);

            Assert.IsFalse(licensed, "an old-format license must not license this version");
            Assert.IsFalse(LicenseHandler.Instance.IsVersionSupported());
        }
        #endregion Test Methods (Old License Rejection)

        #region Test Methods (Invalid License Checks)
        /// <summary>
        /// Verify UA Client license.
        /// </summary>
        [Test]
        public void InvalidClientLicenseCheck()
        {
            const string licenseData =
                    @"<License>
  <Id>93410243-92c8-41be-9ed6-1277fc23329d</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""Client .NET"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGIAkIA1lLWjGpToBdz5VWaMP+N1s8zdzk8BAMlkCy1QepKFv3nksczevGcY+V7zTz2MBd8Y9R7x9xM4ofS5dpc4kkvgQwCQgDUWI/q0MBbLMKRjbQ0VrMc7RVlDn3Lj1XGlb0DcA3U+Yo2DyHfg6/UWSvo4vTGCs7YbjMTCzbMw6BS/oWCW7xzXw==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);
            Assert.IsFalse(licensed);
        }

        /// <summary>
        /// Verify UA Server license.
        /// </summary>
        [Test]
        public void InvalidServerLicenseCheck()
        {
            const string licenseData =
                    @"<License>
  <Id>dd0c9a66-e6c6-4c58-8606-315a2e754934</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""Server .NET"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGIAkIBnAsxW69tRO6sesbuoUHwDeWwC8HtPQJtETKtkoOgn/vAKzEJM2Mh0LsRIVP3o72vfliyASlOeeXv8WMhLLJljrACQgHUeVjRj6rol99sMB2S36llO4X2/85NRHNtLWZBkCbgHZXlrMRh9OXttXAcUsvj7NXHQ3Jh/WknhbIYZTBdMoTYGA==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);
            Assert.IsFalse(licensed);
            Assert.IsFalse(LicenseHandler.Instance.LicensedType == ProductType.Server);
        }

        /// <summary>
        /// Verify UA Client & Server license.
        /// </summary>
        [Test]
        public void InvalidClientServerLicenseCheck()
        {
            const string licenseData =
                @"<License>
  <Id>9d46918d-d248-4bfc-9a7c-9aa5a91e9a67</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""Client .NET"">yes</Feature>
    <Feature name=""Server .NET"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGHAkFobfkn6DwUvciKv1WHHvo8sY91Wd9DuiviF8e0a+1yfgTFOstzD/1vE3YgjYHyAuRuX15VCtU1XYVoLkCwv4/HOgJCAOYRLHJyn2nXnm0M09i1Th5omm2kaKBygCTFmEF7BCuZ1RV3dLGIvGEk9EUp86ntG6X61d0om+Z1FokFZyJQmPVZ</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);
            Assert.IsFalse(licensed);
        }

        /// <summary>
        /// Verify UA Bundle license.
        /// </summary>
        [Test]
        public void InvalidBundleLicenseCheck()
        {
            const string licenseData =
                @"<License>
  <Id>2d1296ae-92c5-4a45-a7aa-10e5d0a27258</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""Client .NET"">yes</Feature>
    <Feature name=""Server .NET"">yes</Feature>
    <Feature name=""PubSub .NET"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGHAkELl8IL74AT/OJ6uczEsh5vwxmxxS0IwltOYq0W8bnaD4n0NMvSqaOd1JT890q2CkvlHUBLQ+jtvN7yANDVGJ8dYwJCAbKCBb7fZQZV9uQUv1uFqMH83gWE+i579mETT3MiK6komdQKwlZjV3zqgbRQJRkYsjCdPRR6UMIant64Ssxy8XCF</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);
            Assert.IsFalse(licensed);
        }

        /// <summary>
        /// Verify Invalid license.
        /// </summary>
        [Test]
        public void InvalidLicenseCheck()
        {
            const string licenseData =
                    @"<License>
  <Id>a9b08284-31a5-402d-9bd4-de885a3798f2</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""Client .NET"">yes</Feature>
    <Feature name=""Server .NET"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGHAkIBiBIURf0Nj+swaYrjUZCctxD2/cKtQAalLyWvda2KsADTELBU+QARd9f0fqmyk1N/ENuSCxa/gxODh+jqbC7MVkkCQVdjT9iazoc1fA16Q4TKmOmoC9R06okjUaUN36/ePfWRQKBPhls9dtpB4+lLd/S+naQhVgiJGIbOl4e1U2xJ7n46</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);
            Assert.IsFalse(licensed);
        }
        #endregion Test Methods (Invalid License Checks)
    }
}
