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

        #region Test Methods (Valid Old License Check)
        /// <summary>
        /// Verify UA Client .NET (All Features) license
        /// </summary>
        [Test]
        public void ClientLicenseCheck()
        {
            LicenseHandler.SimulateServicePatch = false;

            #region License validation
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
            if (!licensed)
            {
                Console.WriteLine("WARNING: No valid license applied.");
            }

            string licensedString = $"   Licensed Product     : {LicenseHandler.Instance.LicensedProduct}";
            Console.WriteLine(licensedString);
                   licensedString = $"   Licensed Product Type: {LicenseHandler.Instance.LicensedProductType}";
            Console.WriteLine(licensedString);
            licensedString = $"   Licensed Features    : {LicenseHandler.Instance.LicensedFeatures}";
            Console.WriteLine(licensedString);
            if (LicenseHandler.Instance.IsEvaluation)
            {
                licensedString = $"   Evaluation expires at: {LicenseHandler.Instance.LicenseExpirationDate}";
                Console.WriteLine(licensedString);
                licensedString = $"   Days until Expiration: {LicenseHandler.Instance.LicenseExpirationDays}";
                Console.WriteLine(licensedString);
            }
            licensedString = $"   Support Included     : {LicenseHandler.Instance.Support}";
            Console.WriteLine(licensedString);
            if (LicenseHandler.Instance.Support != SupportLevel.None)
            {
                licensedString = $"   Support expire at    : {LicenseHandler.Instance.SupportExpirationDate}";
                Console.WriteLine(licensedString);
                licensedString = $"   Days until Expiration: {LicenseHandler.Instance.SupportExpirationDays}";
                Console.WriteLine(licensedString);
            }
            if (LicenseHandler.Instance.IsEvaluation)
            {
                licensedString = $"   Evaluation Period    : {LicenseHandler.Instance.EvaluationPeriod} minutes.";
                Console.WriteLine(licensedString);
            }

            if (!LicenseHandler.Instance.IsLicensed && !LicenseHandler.Instance.IsEvaluation)
            {
                Console.WriteLine("ERROR: No valid license applied.");
            }
            #endregion License validation

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.Client);
        }

        /// <summary>
        /// Verify UA Server .NET (All Features) license.
        /// </summary>
        [Test]
        public void ServerLicenseCheck()
        {
            LicenseHandler.SimulateServicePatch = false;

            #region License validation
            const string licenseData =
                    @"<License>
  <Id>fd868f80-d289-484c-9e79-d839019dbb50</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""UA Server .NET"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGIAkIBuS8YAXOujJ7nF/o8qoPwG7C1HUXuUrGf1T79LmhuiSx/6SxfOPX7/gxYj6qgH5HsY6JWo64tUR+rF0HgR6iZX/wCQgFmktfQlE+I7OF9uH9eLnlLohjarnkEL/yug6h43US9ZJXYKfjTP6xV6fFv1oKx0HBiTamG76nmeWVeyBmypR1l2A==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Server, licenseData);
            if (!licensed)
            {
                Console.WriteLine("WARNING: No valid license applied.");
            }

            string licensedString = $"   Licensed Product     : {LicenseHandler.Instance.LicensedProduct}";
            Console.WriteLine(licensedString);
                   licensedString = $"   Licensed Product Type: {LicenseHandler.Instance.LicensedProductType}";
            Console.WriteLine(licensedString);
            licensedString = $"   Licensed Features    : {LicenseHandler.Instance.LicensedFeatures}";
            Console.WriteLine(licensedString);
            if (LicenseHandler.Instance.IsEvaluation)
            {
                licensedString = $"   Evaluation expires at: {LicenseHandler.Instance.LicenseExpirationDate}";
                Console.WriteLine(licensedString);
                licensedString = $"   Days until Expiration: {LicenseHandler.Instance.LicenseExpirationDays}";
                Console.WriteLine(licensedString);
            }
            licensedString = $"   Support Included     : {LicenseHandler.Instance.Support}";
            Console.WriteLine(licensedString);
            if (LicenseHandler.Instance.Support != SupportLevel.None)
            {
                licensedString = $"   Support expire at    : {LicenseHandler.Instance.SupportExpirationDate}";
                Console.WriteLine(licensedString);
                licensedString = $"   Days until Expiration: {LicenseHandler.Instance.SupportExpirationDays}";
                Console.WriteLine(licensedString);
            }
            if (LicenseHandler.Instance.IsEvaluation)
            {
                licensedString = $"   Evaluation Period    : {LicenseHandler.Instance.EvaluationPeriod} minutes.";
                Console.WriteLine(licensedString);
            }

            if (!LicenseHandler.Instance.IsLicensed && !LicenseHandler.Instance.IsEvaluation)
            {
                Console.WriteLine("ERROR: No valid license applied.");
            }
            #endregion License validation

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.Server);
        }

        /// <summary>
        /// Verify UA Bundle .NET (All Features) license.
        /// </summary>
        [Test]
        public void BundleLicenseCheck()
        {
            LicenseHandler.SimulateServicePatch = false;
            const string licenseData =
                    @"<License>
  <Id>0ba3f5b9-64b4-4933-a10d-d813316f6f09</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""UA Bundle .NET"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGHAkIBLg6DL0zespeq578BRfhq6ICGQ8X2BY6TrkaMBXH5krYa8MM4kd8vJSPsHuwqpEoV0s+vj7hkEOn0MwDLgc0XW+oCQX8WPeOSOrgtR7rIt39vNagLeok/qC00rZPBuZP3iyspGGdRSSSCNaqcoXI1o9vvE/yHPmkWMggq15E3MuCHybbO</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.ClientAndServer, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.ClientAndServer);
        }
        #endregion Test Methods (Valid Old License Check)

        #region Test Methods (Support Contract Check)
        /// <summary>
        /// Verify Support Contract
        /// </summary>
        [Test]
        public void ServicePatchandNoSupportContractCheck()
        {
            const string licenseData =
                    @"<License>
  <Id>3883f548-935e-4f3c-8148-54e438445825</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""UA Client .NET"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGIAkIA8fPxDgZlMV8vC9kdlCTavkMnyZ+zd2uIJRKkyWakxquGrVb+TYK1AcHEwxQwyE+euB31q+SwlHlgdhuH26plOjQCQgH8U7fQBI//QWG+GiNK/9jrRUlqalsamBd4BpmcI7mzn02sibiDK9zvdXyV1h06liCxZLVyFhguI7UnCx6f9PoWHQ==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);
            if (!LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for service patches. Omitting fixture.");
            }
            Assert.IsFalse(licensed);
            Assert.IsFalse(LicenseHandler.Instance.IsVersionSupported());
        }
        #endregion Test Methods (Support Contract Check)

        #region Test Methods (Valid Client Product Features Check)
        /// <summary>
        /// Verify UA Client .NET (All Features) license
        /// </summary>
        [Test]
        public void ClientAllFeatureCheck()
        {
            LicenseHandler.SimulateServicePatch = false;
            const string licenseData =
                    @"<License>
  <Id>3883f548-935e-4f3c-8148-54e438445825</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""UA Client .NET"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGIAkIBcVqJ4PkyLqwhyZwpm8YUhphQUW9ilQYyzxE715mI3YwbRr4YvDA9/wYXp3CxRSTl+jERFilpLQFIyCOF1FEcNhgCQgHJHacJlOuJtE5uiZPtE4ZVepLK98wHMUABkYurwUfEMyVtIKzW2ZqDkvdazjdQUXT6xAgj1K4mndgpGHJl70AyqQ==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.Client);
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AllFeatures));
        }
        /// <summary>
        /// Verify UA Client .NET (All Features) license
        /// </summary>
        [Test]
        public void ClientDataAccessFeatureCheck()
        {
            LicenseHandler.SimulateServicePatch = false;
            const string licenseData =
                    @"<License>
  <Id>1d4480a4-3e8b-4137-93ee-d2461c2a0ef4</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""UA Client .NET"">yes</Feature>
    <Feature name=""DataAccess"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGIAkIBN9HyqMOBZeKTFNXSYpaS+C1vNclHf5VGZ3ZYXeWq/5UBfLIQSkDsFav9q93NBWKVu2kuiM5WmNIWO0Qrl1B+HYUCQgG6phbwSMcRo7CRaTi3Za5tX+rn07Oni7GhFbJ0/fTc3QEXGAOIFBvJmMygl0WaYqnGroulLFxxAPQDYxJ22Fyf4A==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.Client);
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.DataAccess));
            Assert.IsFalse(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AlarmsConditions));
            Assert.IsFalse(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.HistoricalAccess));
            Assert.IsFalse(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AllFeatures));
        }

        /// <summary>
        /// Verify UA Client .NET (DataAccess, AlarmsConditions) license
        /// </summary>
        [Test]
        public void ClientDataAccessAlarmConditionsFeatureCheck()
        {
            LicenseHandler.SimulateServicePatch = false;
            const string licenseData =
                    @"<License>
  <Id>33669cdb-5851-4834-8749-6782dd68cf1a</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""UA Client .NET"">yes</Feature>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGGAkE+fxJUCeSIPZ77ggyugIMShUSnLHKmHZfgEPCQ9jSx916vS8QE/zpzR1Nt3fvDxiksVqzCUPcQVz6/afo8/QVjJQJBVU/inkWIXhU+usavd1MDY8kTXyeipBm7H5cyugR/QBRLHpekCL2QrUaqRcVqSWK6pUk+6Ptf90I3zf/WKyhoV6o=</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.Client);
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.DataAccess));
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AlarmsConditions));
            Assert.IsFalse(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.HistoricalAccess));
            Assert.IsFalse(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AllFeatures));
        }

        /// <summary>
        /// Verify UA Client .NET (DataAccess, AlarmsConditions, HistoricalAccess) license
        /// </summary>
        [Test]
        public void ClientDataAccessAlarmConditionsHistoricalAccessFeatureCheck()
        {
            LicenseHandler.SimulateServicePatch = false;
            const string licenseData =
                    @"<License>
  <Id>e276424b-438d-4b9b-b66b-e8b18468b22b</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""UA Client .NET"">yes</Feature>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">yes</Feature>
    <Feature name=""HistoricalAccess"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGHAkIBfi2n8FN47Z6NRLlqrRpZNLUA3Bf97gveBa/xJQl334MvZVSJxOXIgGi8zEV6wp627DcwHEyZ0nHb1KUfbqgHgJYCQRDbFWM6a4J4WFSDyohCBqrYxnX2p4k2hNmZteytI/018i9u0xOeIR30lW8JWI074Yq21SuXy2L9+Fqd7vC4fcPq</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.Client);
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.DataAccess));
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AlarmsConditions));
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.HistoricalAccess));
            Assert.IsFalse(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AllFeatures));
        }
        #endregion Test Methods (Valid Client Product Features Check)

        #region Test Methods (Valid Server Product Features Check)
        /// <summary>
        /// Verify UA Server .NET (All Features) license
        /// </summary>
        [Test]
        public void ServerAllFeatureCheck()
        {
            LicenseHandler.SimulateServicePatch = false;
            const string licenseData =
                    @"<License>
  <Id>fd868f80-d289-484c-9e79-d839019dbb50</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""UA Server .NET"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGHAkIA3qf4wZwqHfpYahoZ+jD15GfGYsvtf+eB9UWmg272wmYVxihIHXGl3VL3HwLaPr+IEVqtNP3Jt2HoGq8vj52jkucCQWfJ0hJFW6u1NJQnNKer239ZFgnspPSs2PbTE+3MKHIjMtnp6q02C6wfkDxg0X35Nijap8SUAlX03dFU5XyE4rfd</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Server, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.Server);
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AllFeatures));
        }
        /// <summary>
        /// Verify UA Server .NET (All Features) license
        /// </summary>
        [Test]
        public void ServerDataAccessFeatureCheck()
        {
            LicenseHandler.SimulateServicePatch = false;
            const string licenseData =
                    @"<License>
  <Id>0f98f610-40ec-431c-b4c7-0634ca128de0</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""UA Server .NET"">yes</Feature>
    <Feature name=""DataAccess"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGIAkIAt/jHdCEqOeAOrOiy6QZP+und79f80ns1BHUC8ZHbGHoIMCsGk5/xGhGyxCJz5M+XUvAlz28p8J4RH2IqJriFVmMCQgDTqrAUQigS2rQudmna+LGsnpD0leyt9x1MXOQmyFifdNN7kKSJpO6vMq5w7Cy/LuZR73BPm0KA/63ELNjOm3pOTg==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Server, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.Server);
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.DataAccess));
            Assert.IsFalse(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AlarmsConditions));
            Assert.IsFalse(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.HistoricalAccess));
            Assert.IsFalse(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AllFeatures));
        }

        /// <summary>
        /// Verify UA Server .NET (DataAccess, AlarmsConditions) license
        /// </summary>
        [Test]
        public void ServerDataAccessAlarmConditionsFeatureCheck()
        {
            LicenseHandler.SimulateServicePatch = false;
            const string licenseData =
                    @"<License>
  <Id>9d41cbb5-2255-466d-b892-a9fb3d7a0211</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""UA Server .NET"">yes</Feature>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGHAkIBE++LBncCMsrEjmeFK6JnKkcIpgfJOMWsUyoBZBp5AUJu05Y4A0ag6Z6C1bW7MwW4y10c4jp4XQYyOW+Au4zR/UkCQWSbyfIykUSxZqIbgKjysQ6tJxObPzIp0cuhpsAUZGFgXKLZcVNUYxyql7WYAG3avI0J51EUTTaVhqXSD9/f/PBv</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Server, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.Server);
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.DataAccess));
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AlarmsConditions));
            Assert.IsFalse(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.HistoricalAccess));
            Assert.IsFalse(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AllFeatures));
        }

        /// <summary>
        /// Verify UA Server .NET (DataAccess, AlarmsConditions, HistoricalAccess) license
        /// </summary>
        [Test]
        public void ServerDataAccessAlarmConditionsHistoricalAccessFeatureCheck()
        {
            LicenseHandler.SimulateServicePatch = false;
            const string licenseData =
                    @"<License>
  <Id>a43e085b-71de-4664-8fa6-5d6f88bbae3f</Id>
  <Type>Standard</Type>
  <ProductFeatures>
    <Feature name=""UA Server .NET"">yes</Feature>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">yes</Feature>
    <Feature name=""HistoricalAccess"">yes</Feature>
  </ProductFeatures>
  <Signature>MIGIAkIA+6c0N50m/jH1tAYRwq5HmjAJlAjSzLUOWI9MwavNb2x3a8RIpc9HexckC9oqciyyn/h19AVoKszb5uolIHBaSicCQgGnrRjwLJedwq2DstH0oJLYC5GQXLly0Ex6zFjTQT6n62kjbYIyE6GYTgnFd3QYFFGqdF2AQJzWXD289IB7VYZ5VA==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Server, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.Server);
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.DataAccess));
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AlarmsConditions));
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.HistoricalAccess));
            Assert.IsFalse(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AllFeatures));
        }
        #endregion Test Methods (Valid Server Product Features Check)

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
