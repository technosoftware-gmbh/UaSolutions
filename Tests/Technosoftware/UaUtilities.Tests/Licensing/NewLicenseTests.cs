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
using Technosoftware.UaUtilities;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
#endregion Using Directives

namespace Technosoftware.UaUtilities.Tests
{
    /// <summary>
    /// Tests for the NodeState classes.
    /// </summary>
    [TestFixture]
    [Category("Licensing")]
    [SetCulture("en-us"), SetUICulture("en-us")]
    public class NewLicenseTests
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

        #region Test Methods (Applicatiopn License Checks)
        /// <summary>
        /// Verify Empty license
        /// </summary>
        [Test]
        public void ApplicationLicensewithValidateCheck()
        {

            LicenseHandler.SimulateServicePatch = false;
            #region License validation
            const string licenseData =
                    @"";
            bool licensed = LicenseHandler.Instance.Validate(Technosoftware.UaUtilities.ProductType.Client, licenseData);
            if (!licensed)
            {
                Console.WriteLine("WARNING: No valid license applied.");
            }

            string licensedString;

            if (LicenseHandler.Instance.IsLicensed)
            {
                licensedString = $"   Customer             : {LicenseHandler.Instance.Customer.Name}";
                Console.WriteLine(licensedString);
                licensedString = $"   Days until Expiration: {LicenseHandler.Instance.LicenseExpirationDays}";
                Console.WriteLine(licensedString);
            }

            licensedString = $"   Licensed Product     : {LicenseHandler.Instance.LicensedProduct}";
            Console.WriteLine(licensedString);
            licensedString = $"   Licensed Type        : {LicenseHandler.Instance.LicensedType}";
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
            if (LicenseHandler.Instance.Support != Technosoftware.UaUtilities.SupportLevel.None)
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
            Assert.IsFalse(LicenseHandler.Instance.IsExpired);
            Assert.IsTrue(LicenseHandler.Instance.IsEvaluation);
            Assert.AreEqual(120, LicenseHandler.Instance.EvaluationPeriod);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.ClientAndServer);
        }

        /// <summary>
        /// Verify Empty license
        /// </summary>
        [Test]
        public void ApplicationLicensewithoutValidateCheck()
        {
            LicenseHandler.SimulateServicePatch = false;

            #region License validation
            string licensedString = $"   Licensed Product     : {LicenseHandler.Instance.LicensedProduct}";
            Console.WriteLine(licensedString);
            licensedString = $"   Licensed Type        : {LicenseHandler.Instance.LicensedType}";
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
            if (LicenseHandler.Instance.Support != Technosoftware.UaUtilities.SupportLevel.None)
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
            Assert.IsFalse(LicenseHandler.Instance.IsExpired);
            Assert.IsTrue(LicenseHandler.Instance.IsEvaluation);
            Assert.AreEqual(120, LicenseHandler.Instance.EvaluationPeriod);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.ClientAndServer);
        }
        #endregion Test Methods (No license provided Check)

        #region Test Methods (Trial License Check)
        /// <summary>
        /// Verify Trial license
        /// </summary>
        [Test]
        public void ExpiredTrialLicenseCheck()
        {
            LicenseHandler.SimulateServicePatch = false;
            const string licenseData =
                    @"<License>
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>Client</Type>
    <Version>5.0.0</Version>
    <PublishedDate>Mon, 27 Apr 2026</PublishedDate>
  </Product>
  <License>
    <Level>Trial</Level>
    <ExpirationDate>Wed, 30 Apr 2025</ExpirationDate>
  </License>
  <Support>
    <Level>None</Level>
    <ExpirationDate />
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">yes</Feature>
    <Feature name=""HistoricalAccess"">yes</Feature>
    <Feature name=""AllFeatures"">yes</Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGGAkEbWVRXmyG2LvQ6X+fPmmHqb1iOKDAU9ZnKlQpKlqWFsalmYlRNODWsERoERoPb48pd9gdUJHd5c1MqZYNDcGFEVAJBFZh1ohHy6YFfs5maXzLGC51plUB7yFq2WZdLHCg+favo+LX9tKlZASddDHZZ7HZd/WQWUytLN/nG2Mfh5NQVtRE=</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);
            Assert.IsFalse(licensed);
            Assert.IsTrue(LicenseHandler.Instance.IsExpired);
        }

        /// <summary>
        /// Verify Trial license
        /// </summary>
        [Test]
        public void TrialLicenseCheck()
        {
            LicenseHandler.SimulateServicePatch = false;
            const string licenseData =
                    @"<License>
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>Client</Type>
    <Version>7.0.0</Version>
    <PublishedDate>Wed, 01 Jul 2026</PublishedDate>
  </Product>
  <License>
    <Level>Trial</Level>
    <ExpirationDate>Wed, 30 Jun 2027</ExpirationDate>
  </License>
  <Support>
    <Level>None</Level>
    <ExpirationDate />
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">yes</Feature>
    <Feature name=""HistoricalAccess"">yes</Feature>
    <Feature name=""AllFeatures"">yes</Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGIAkIBeL66WScWrcfCyy3T/5OkDYhCHTkzkwlbW/iZkUtQ7tsggpGEOXR6WTpH5H8lAXdb5lIei15HPUils9h2bbOFIWkCQgCgzi9jHz9OY4PQdP48B/sBoHXRlv6ZzekmH6AvcqtoEpLh1x9pMC0jgWPTjjd9i7asn3OTkY89lwC+TJElkOR1kA==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.IsEvaluation);
            Assert.AreEqual(120, LicenseHandler.Instance.EvaluationPeriod);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.Client);
        }
        #endregion Test Methods (Trial License Check)

        #region Test Methods (Support Contract Check)
        /// <summary>
        /// Verify Support Contract
        /// </summary>
        [Test]
        public void ServicePatchandNoSupportContractCheck()
        {
            LicenseHandler.SimulateServicePatch = true;
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
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);
            if (!LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for service patches. Omitting fixture.");
            }
            Assert.IsFalse(licensed);
            Assert.IsFalse(LicenseHandler.Instance.IsVersionSupported());
        }

        [Test]
        public void ServicePatchandandExpiredSupportContractCheck()
        {
            LicenseHandler.SimulateServicePatch = true;
            const string licenseData =
                    @"<License>
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>ClientAndServer</Type>
    <Version>5.0.0</Version>
    <PublishedDate>Mon, 27 Apr 2026</PublishedDate>
  </Product>
  <License>
    <Level>Trial</Level>
    <ExpirationDate>Mon, 30 Apr 2029</ExpirationDate>
  </License>
  <Support>
    <Level>Standard</Level>
    <ExpirationDate>Wed, 30 Apr 2025</ExpirationDate>
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">yes</Feature>
    <Feature name=""HistoricalAccess"">yes</Feature>
    <Feature name=""AllFeatures"">yes</Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGIAkIBPlGAq+1MjFDLc6amaNvcZyhzALomHUBK/fZM/sozFIDoR5YI/soZZbESaERRvMi8TQIvTZsYN7ycVw7e/6ODGsgCQgD9NqiDOoPA89JYo0E51euM/D5i/PEMOqgReBEZaIKNxSZnWxszPmX1ulnMEEI0jZDf3DEv5CujlpHRcjhBtMEYvg==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);
            Assert.IsFalse(licensed);
            Assert.IsFalse(LicenseHandler.Instance.IsVersionSupported());
        }

        [Test]
        public void LowerMajorLicenseIsNotCarriedOverBySupportContract()
        {
            LicenseHandler.SimulateServicePatch = true;
            const string licenseData =
                    @"<License>
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>ClientAndServer</Type>
    <Version>5.0.0</Version>
    <PublishedDate>Mon, 27 Apr 2026</PublishedDate>
  </Product>
  <License>
    <Level>SingleDeveloper</Level>
    <ExpirationDate>Mon, 31 Dec 2029</ExpirationDate>
  </License>
  <Support>
    <Level>Standard</Level>
    <ExpirationDate>Mon, 31 Dec 2029</ExpirationDate>
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess""></Feature>
    <Feature name=""AlarmsConditions""></Feature>
    <Feature name=""HistoricalAccess""></Feature>
    <Feature name=""AllFeatures""></Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGIAkIBDt3b8UyXFGchPQ3PbO0wh2eZn/nymNJBPWCzz937RgD3FOoqYEJ2lmetE9LBzrUdzwU23oAsumTtN6nX3W+aQykCQgEF2Og/PP5yw9WnDWz6289d2UkZwXvJ78yDTL4bTijg/B9R6FkiGfrU+b2G7m9hnncAecKiyU624yoLUFxw8+yCEg==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);

            // The license names version 5 and its support contract runs to 2029. Before the
            // version 7 licensing change that combination licensed the current version, and
            // this asserted so. A support contract no longer carries a license across a
            // major version: customers holding one when the new major is released are
            // issued a new license for it instead, so the run-time answer is no.
            Assert.IsFalse(licensed);
            Assert.IsFalse(LicenseHandler.Instance.IsVersionSupported());
        }
        #endregion Test Methods (Support Contract Check)

        #region Test Methods (Valid New License Check)
        /// <summary>
        /// Verify UA Client .NET (All Features) license
        /// </summary>
        [Test]
        public void ClientLicenseCheck()
        {
            LicenseHandler.SimulateServicePatch = false;
            const string licenseData =
                    @"<License>
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>Client</Type>
    <Version>7.0.0</Version>
    <PublishedDate>Wed, 01 Jul 2026</PublishedDate>
  </Product>
  <License>
    <Level>SingleDeveloper</Level>
    <ExpirationDate />
  </License>
  <Support>
    <Level>None</Level>
    <ExpirationDate />
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess""></Feature>
    <Feature name=""AlarmsConditions""></Feature>
    <Feature name=""HistoricalAccess""></Feature>
    <Feature name=""AllFeatures""></Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGIAkIBa4JQTvMdcAwffvPVTqxwtVOhBs6qo5WkSbwLP9MBJEiFoaFslrd9iQGBlEzc7C8GMTZXfnUvQnTE/mD3vRpsr98CQgGyqEL1plY+DKbTXBvRWfe5idP0WKXh8lhPKiXWiQgLvsmfo7I+iqNXPtclzBbm5R7IhFwsKm23nBxHCNDo0YH9OQ==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);

            string licensedString;

            if (LicenseHandler.Instance.IsLicensed)
            {
                licensedString = $"   Customer             : {LicenseHandler.Instance.Customer.Name}";
                Console.WriteLine(licensedString);
                licensedString = $"   Days until Expiration: {LicenseHandler.Instance.LicenseExpirationDays}";
                Console.WriteLine(licensedString);
            }

            Assert.IsTrue(LicenseHandler.Instance.IsVersionSupported());
            Assert.IsFalse(LicenseHandler.Instance.IsEvaluation);
            Assert.IsFalse(LicenseHandler.Instance.IsExpired);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.Client);
        }

        /// <summary>
        /// Verify UA Server .NET (All Features) license.
        /// </summary>
        [Test]
        public void ServerLicenseCheck()
        {
            LicenseHandler.SimulateServicePatch = false;
            const string licenseData =
                    @"<License>
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>Server</Type>
    <Version>7.0.0</Version>
    <PublishedDate>Wed, 01 Jul 2026</PublishedDate>
  </Product>
  <License>
    <Level>SingleDeveloper</Level>
    <ExpirationDate />
  </License>
  <Support>
    <Level>None</Level>
    <ExpirationDate />
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess""></Feature>
    <Feature name=""AlarmsConditions""></Feature>
    <Feature name=""HistoricalAccess""></Feature>
    <Feature name=""AllFeatures""></Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGIAkIA+ATcM6licqOP/SzQk5XWJE4wbBv0qdsrenrPi9j900mggruNAQoL+BmvLSni9vGSjjRtdn5cay2sjkjZmaooTKACQgF+T/77WKYTo9uS65hPVsfaZyBjy1tdiAknerBIwKR7Y5/jBf/lNNVvXP61PMjKDAqNYY+1JqJRX8uIAmsA7QBeeA==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Server, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.IsVersionSupported());
            Assert.IsFalse(LicenseHandler.Instance.IsEvaluation);
            Assert.IsFalse(LicenseHandler.Instance.IsExpired);
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
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>ClientAndServer</Type>
    <Version>7.0.0</Version>
    <PublishedDate>Wed, 01 Jul 2026</PublishedDate>
  </Product>
  <License>
    <Level>SingleDeveloper</Level>
    <ExpirationDate />
  </License>
  <Support>
    <Level>None</Level>
    <ExpirationDate />
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">yes</Feature>
    <Feature name=""HistoricalAccess"">yes</Feature>
    <Feature name=""AllFeatures"">yes</Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGGAkEzzCZCKI3ZPqtGZXsRqWv0YwKbaquqoPPrLuAoLJr592IZFZQGzfdAb/JoHKmAosyeBuHuOqt9AGVrBmtrEzVsAwJBYsXg7q3zZADKLM0yrAcwrhkZD+Hu7M6snZFoorYGyQLREDKA1ZEZ0TjL2p44hnNMnx5/JiVZ9WmDqz3223Zy3po=</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.ClientAndServer, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.IsVersionSupported());
            Assert.IsFalse(LicenseHandler.Instance.IsEvaluation);
            Assert.IsFalse(LicenseHandler.Instance.IsExpired);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.ClientAndServer);
        }
        #endregion Test Methods (Valid New License Check)

        #region Test Methods (Valid Client Product Features Check)
        /// <summary>
        /// Verify UA Client .NET (All Features) license
        /// </summary>
        [Test]
        public void ClientAllFeatureCheck()
        {
            const string licenseData =
                    @"<License>
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>Client</Type>
    <Version>7.0.0</Version>
    <PublishedDate>Wed, 01 Jul 2026</PublishedDate>
  </Product>
  <License>
    <Level>SingleDeveloper</Level>
    <ExpirationDate />
  </License>
  <Support>
    <Level>None</Level>
    <ExpirationDate />
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">yes</Feature>
    <Feature name=""HistoricalAccess"">yes</Feature>
    <Feature name=""AllFeatures"">yes</Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGIAkIBN3sviBPz4lED1OZ9LDQNCz2VFtjr2xOiUdkKRnII/U+GiUf8rbFU/9dyb2nlr80LzHgvDPP1Rpdr+icye+fRdjkCQgExKfUxL3Y/xNCxulApTcPYbv5LnBNwCD9JLsGW25PlISRyS/DzIN/9P+eG1CwAf37i25soJGJOufX56q663PcEhw==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.IsVersionSupported());
            Assert.IsFalse(LicenseHandler.Instance.IsEvaluation);
            Assert.IsFalse(LicenseHandler.Instance.IsExpired);
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
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>Client</Type>
    <Version>7.0.0</Version>
    <PublishedDate>Wed, 01 Jul 2026</PublishedDate>
  </Product>
  <License>
    <Level>SingleDeveloper</Level>
    <ExpirationDate />
  </License>
  <Support>
    <Level>None</Level>
    <ExpirationDate />
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">no</Feature>
    <Feature name=""HistoricalAccess"">no</Feature>
    <Feature name=""AllFeatures"">no</Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGHAkIB4doYblP2Zkllyn/KoCgN1i2lDF9S4FtxBFxfOK7XVQlxTOoNZn3YHvEvRtlG8iLa5YW380XProOh0JFvTPZ04mgCQTmH7B8s2SEwsCbgRsbvNiJpHMZ9FfSLLiJdOvmrdz3Xxbr9hY5EQ8vswU46NfK9EqpcYGXWEwAqoqtgZZR7vd0r</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.IsVersionSupported());
            Assert.IsFalse(LicenseHandler.Instance.IsEvaluation);
            Assert.IsFalse(LicenseHandler.Instance.IsExpired);
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
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>Client</Type>
    <Version>7.0.0</Version>
    <PublishedDate>Wed, 01 Jul 2026</PublishedDate>
  </Product>
  <License>
    <Level>SingleDeveloper</Level>
    <ExpirationDate />
  </License>
  <Support>
    <Level>None</Level>
    <ExpirationDate />
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">yes</Feature>
    <Feature name=""HistoricalAccess"">no</Feature>
    <Feature name=""AllFeatures"">no</Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGIAkIBKaplMUxg5sJWVhLgLfVnLDQfTfchbZv5kMeLexJrbQmZJshR9Ocx1/+JzvMJz8YpM+q9Cx1dfuIk3rhAs4uD+E0CQgFGOTnZSjWUtPZv1Oa58mpaENCRKUNi3uokUmiKnrd7QJHRMtDvRP1P7pDN4k/Ga0sWS1DP1v8RM4OnmOZHvIui9Q==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.IsVersionSupported());
            Assert.IsFalse(LicenseHandler.Instance.IsEvaluation);
            Assert.IsFalse(LicenseHandler.Instance.IsExpired);
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
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>Client</Type>
    <Version>7.0.0</Version>
    <PublishedDate>Wed, 01 Jul 2026</PublishedDate>
  </Product>
  <License>
    <Level>SingleDeveloper</Level>
    <ExpirationDate />
  </License>
  <Support>
    <Level>None</Level>
    <ExpirationDate />
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">yes</Feature>
    <Feature name=""HistoricalAccess"">yes</Feature>
    <Feature name=""AllFeatures""></Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGGAkEOasZ6UkuLqLy7mUfiWSlRRgYm/F5pv6lCnygGrmwPPyiah2ef4Vda4ICiRhOtY7dE2MlAYZjhyuvXtvzk06iU0wJBVpbAhiLnXJRFpUS10Khk7halodHlcPGqSp+jlJdTdBxVE+tgF0j9cmJf5nRwakPAAzbCTGZ5cTwW92yMyb6nEso=</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Client, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.IsVersionSupported());
            Assert.IsFalse(LicenseHandler.Instance.IsEvaluation);
            Assert.IsFalse(LicenseHandler.Instance.IsExpired);
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
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>Server</Type>
    <Version>7.0.0</Version>
    <PublishedDate>Wed, 01 Jul 2026</PublishedDate>
  </Product>
  <License>
    <Level>SingleDeveloper</Level>
    <ExpirationDate />
  </License>
  <Support>
    <Level>None</Level>
    <ExpirationDate />
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">yes</Feature>
    <Feature name=""HistoricalAccess"">yes</Feature>
    <Feature name=""AllFeatures"">yes</Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGHAkIBtfE8yR4a8lOqep8EmaHkwz17GfeMcNLxsomFE3KV2NxORLXJMXbLtb2tvM7pZCO5+Ksut6lmQ1NLvRja09qLRhwCQVsm6H37Ri95CJHHQaSiccmZLL8IFGMzPY1XUvC52RCp3b2zl9JsW33GuWz4VBGc5RIDGiiZpkutrI6bn/IqTS+T</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Server, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.IsVersionSupported());
            Assert.IsFalse(LicenseHandler.Instance.IsEvaluation);
            Assert.IsFalse(LicenseHandler.Instance.IsExpired);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.Server);
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.DataAccess));
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AlarmsConditions));
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.HistoricalAccess));
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
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>Server</Type>
    <Version>7.0.0</Version>
    <PublishedDate>Wed, 01 Jul 2026</PublishedDate>
  </Product>
  <License>
    <Level>SingleDeveloper</Level>
    <ExpirationDate />
  </License>
  <Support>
    <Level>None</Level>
    <ExpirationDate />
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions""></Feature>
    <Feature name=""HistoricalAccess""></Feature>
    <Feature name=""AllFeatures""></Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGHAkF413j5wWhPnY/ujGB7B2hTF9nPJKskq+t+Mv2S0B4o2g9lSaxv7FIAWpA+4tdlyVw/UoS9Uv7GImEzBd92mLHfDQJCAPZrgBxk4cw+Q+mixVcJSFGQ3fK2NAY0G4Da7MIHd8K0Q+7x5O/hhfqmd1i7wAAVlYwsKgU7S5z3OOamv1vuq4J0</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Server, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.IsVersionSupported());
            Assert.IsFalse(LicenseHandler.Instance.IsEvaluation);
            Assert.IsFalse(LicenseHandler.Instance.IsExpired);
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
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>Server</Type>
    <Version>7.0.0</Version>
    <PublishedDate>Wed, 01 Jul 2026</PublishedDate>
  </Product>
  <License>
    <Level>SingleDeveloper</Level>
    <ExpirationDate />
  </License>
  <Support>
    <Level>None</Level>
    <ExpirationDate />
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">yes</Feature>
    <Feature name=""HistoricalAccess""></Feature>
    <Feature name=""AllFeatures""></Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGIAkIBXDY0pnvv5zRccIlMutZxTMkMNXq9d6flfz/aOaXKTdeWKjGkl6ef7YFWTjeVGCxQXyO5iM7VCBq8FzjOXJt22Y4CQgGqmxYWxvMFuQHeAjhac6o4YbhPA/xifj7fv79CFG/qjdBZa1uvjyx17u977HNDXxqPOZN6LKC6wTxZPjAqHzlvSw==</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Server, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.IsVersionSupported());
            Assert.IsFalse(LicenseHandler.Instance.IsEvaluation);
            Assert.IsFalse(LicenseHandler.Instance.IsExpired);
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
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <Product>
    <Name>OPC UA Solutions</Name>
    <Type>Server</Type>
    <Version>7.0.0</Version>
    <PublishedDate>Wed, 01 Jul 2026</PublishedDate>
  </Product>
  <License>
    <Level>SingleDeveloper</Level>
    <ExpirationDate />
  </License>
  <Support>
    <Level>None</Level>
    <ExpirationDate />
  </Support>
  <Customer>
    <Name></Name>
    <Company></Company>
    <Domain></Domain>
  </Customer>
  <Features>
    <Feature name=""DataAccess"">yes</Feature>
    <Feature name=""AlarmsConditions"">yes</Feature>
    <Feature name=""HistoricalAccess"">yes</Feature>
    <Feature name=""AllFeatures""></Feature>
  </Features>
  <LicenseAttributes>
    <Attribute name=""Assembly Identity""></Attribute>
    <Attribute name=""Product Identity"">55ed232b0ff14fb9ace6f836d8867b39718ced5281f95d0d6289e701325efbfc</Attribute>
  </LicenseAttributes>
  <Signature>MIGHAkIBN3guYpd4TJNXCwG2FkBjSZX6OV4uGBDmeR8kfnbXvMo8nmTfmL6SvHJr2Gcf5nMMGoGL21IOJNxlPjCQKv2+UX4CQTZyvnQo4p9wXJ+aXQ8YIyh96bCITWXSH8PphPOhOiaM7zlfNDg37EkXSlJiBfhcmhC0skK4ML47Y6FndXnh3bBD</Signature>
</License>";
            bool licensed = LicenseHandler.Instance.Validate(ProductType.Server, licenseData);

            if (LicenseHandler.Instance.IsServicePatch)
            {
                NUnit.Framework.Assert.Ignore("Test only works for non-service patches. Omitting fixture.");
            }
            Assert.IsTrue(licensed);
            Assert.IsTrue(LicenseHandler.Instance.IsVersionSupported());
            Assert.IsFalse(LicenseHandler.Instance.IsEvaluation);
            Assert.IsFalse(LicenseHandler.Instance.IsExpired);
            Assert.IsTrue(LicenseHandler.Instance.LicensedType == ProductType.Server);
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.DataAccess));
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AlarmsConditions));
            Assert.IsTrue(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.HistoricalAccess));
            Assert.IsFalse(LicenseHandler.Instance.LicensedFeatures.HasFlag(ProductFeature.AllFeatures));
        }
        #endregion Test Methods (Valid Server Product Features Check)
    }
}
