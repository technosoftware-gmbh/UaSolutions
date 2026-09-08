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
    <Version>6.0.0</Version>
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
  <Signature>MIGIAkIBTHoExbDfTnmBmt6L6TtSH++OHPB9e4q8L3vDih1p9vGsMuti02LorFuyXJy1hPW4Ej7gONHprGf7Go++BgUHZ5UCQgDf1nS9AaK65D2UjoLTD6L3RaUNwP86Z66t/2sLJNywE2x+2QMHVyoeBEGqPX7GluDsp/rkmuIJiYXqmWg5YEbSWg==</Signature>
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
    <Version>6.0.0</Version>
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
  <Signature>MIGIAkIA0+0xNQsqUlhP++GGZRzW7UDaEoZ7LYdpwyrV/EB0GzAXmINDqkqtAd/6bw0sk3lJOFkMAr3aAaDIIE4fgfVRUKkCQgDehgUfl5o5EheSVNJAO3cU4tJEOppaNtlqH+jh7G7Rbj2Xa6e3lensFBVMBjpNTc6fA21awpaAm5wsX2VCUNHlhQ==</Signature>
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
    <Version>6.0.0</Version>
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
  <Signature>MIGIAkIBH4lafSLopWaUS9VYzEuEjTwWnvdvuaR3gR+mQks1DY7XPuoXVyfAKymsfW8ZDMzYAaxP7CIKvmQUPjAdnCMnoIICQgCP+rXa3rjAocGJ7dxfwIF2Uy/kcgqY4nULkDaoo6KsqD0Fj59b+IDc3bvpoOZhhLJPSbshOka653KhCD1ZP27SFQ==</Signature>
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
    <Version>6.0.0</Version>
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
  <Signature>MIGIAkIBfOtM8zWTTXkjXsRu/kBUgexTvMmS40lvb5fQRkfiddJnLab1M9Im7+ck3kr1H9KUZ5pRCy+nNSV7EP7hWQAiTe4CQgELo1oVe/T8fWJwxLL9Oa+fYVMzRgDCsf3nlJrdfEKcJc/HkAonw65u24AF2p/+z08Rrz+EuHea9Av0lr5PtN6/kA==</Signature>
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
    <Version>6.0.0</Version>
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
  <Signature>MIGHAkFm9C9poW+zHCiVAMdCpfhYgKGrHL2wR+9c7sluaBQYKIcC5hYlyj3t+Wl/1WQJwNmnfbOfKiQCg7JIsSWGz1LEaAJCAR6i2kVrQAtkpoE6pN0hjSCRkIIiiB2LbI/tosF5qgL7cdLS/WxLPbOehxdiIqcw7fjh+rNSZ/4cWpZ3xxn8D4RE</Signature>
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
    <Version>6.0.0</Version>
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
  <Signature>MIGIAkIBB5g40JMCOm3Kf5FrVKnq+OlrJ2/h3DE1SzPuZO8c/9E3kZKsZ1Y64Crv1a3F6VTDsXG+maggq9cstBHxpNncDtgCQgDruV/4HAPmLZdeEVLbveVU2yw9T948Xhg613A2B/OemvIwghvnYQBTF63GZxlfx2fudkqtX8lvE4atvdQQ0qEbAQ==</Signature>
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
    <Version>6.0.0</Version>
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
  <Signature>MIGIAkIB1UDkCD2k9aTztDqzY3MqdTkMC3xLDiAaRD8H7UbNUUHoTsP9axekAVnlWb5vZTMrUQWYOHiVdFb5gJb1anrocUsCQgCDXVJZaumtpJ/djUSm2lf7R3AnwB9dQS3m+TKlWi696MpUNPShFDvaxNWi3frFza+wUVlYZPRfJnB9JS/UgPYHcQ==</Signature>
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
    <Version>6.0.0</Version>
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
  <Signature>MIGHAkIAlXlqLNdKHxeUiDV1rVvbSJeTJbs7/bcO4FJ1UakbWkmCZ0dvV4ShWBe6kQFJYFSvnZWz8YP1HRxAIp2XDn9suyYCQRxOs3ws9ZxM4hfMuB996H5b7NVXG6XHyItmU0kD9GrlzksICrXsk1/p3Z+cCf/UaLwzaE3L2Wy0BOCXYhh/ANre</Signature>
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
    <Version>6.0.0</Version>
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
  <Signature>MIGIAkIAjcR0p1HMNqrh1Uq23713tQc+el+QLPFkF7e56OtNH72kOIM0tcer+UKVIit+uRhXjbRizYXm4oxMBw8fg+2WDj8CQgF1NMbuEhcC8/EaNsqojF6Gi9h5jSiwPW/T8m4E7ZHnY3WsV9EBfEEaQ/HX2PkMVyrYhWMSF+hxE0hwECTw6foqiA==</Signature>
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
    <Version>6.0.0</Version>
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
  <Signature>MIGHAkEE+B04hgizBmdAOukjCaDWTGHsFD+hMe/wXXA47Ahxd3AU701iK+BSZ9HcINIoQvOZBnScBcQCcJnFJBiSKatTKAJCAPwAF3ZfvxQ4EgBdSPKlZf08Sazib+CrMLop3OKBBnLQEAgqnuxz6LWUbm88wf+w4weS7Whvy0fd+jzTxmKHDnGV</Signature>
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
    <Version>6.0.0</Version>
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
  <Signature>MIGIAkIByPAbI58wIiGO3vbS4aEk/2Lplq38w6Ie8lpEvznNcHsDYmgMo36xzK+xdvYrrJpNWXASb0zgDufdCk2ak3zhC+ICQgGcocSWJxvEN+NmWpyg8Rwsq730Ed6pbTDu0AX14GybX0N3JGXAzyvQT9ouhgKYnAGk7Ioxkg5LVDT0cI1yx4IAvw==</Signature>
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
    <Version>6.0.0</Version>
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
  <Signature>MIGHAkF3uTWtwIu22LrVn00xUEoQdzzUnAT7B8xMFjPjOOth3O3evNfPw+xIGbN9TjJitPw2y3Hc+X+iKKNgqJk68AS6GwJCAfs97ykmXQ5U4CEeFCMhI1VKsxAWql22rVUHk546lHgQxHOOQCSXd/ryx/aP+7Xp18XE1ZglUSmzVB2c0XFY6h1Y</Signature>
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
