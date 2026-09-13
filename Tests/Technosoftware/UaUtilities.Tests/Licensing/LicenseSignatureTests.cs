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
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using NUnit.Framework;
using Technosoftware.UaUtilities;
#endregion Using Directives

namespace Technosoftware.UaUtilities.Tests
{
    [TestFixture]
    public class LicenseSignatureTests
    {
        private string passPhrase;
        private string privateKey;
        private string publicKey;

        [SetUp]
        public void Init()
        {
            passPhrase = Guid.NewGuid().ToString();
            var keyGenerator = KeyGenerator.Create();
            KeyPair keyPair = keyGenerator.GenerateKeyPair();
            privateKey = keyPair.ToEncryptedPrivateKeyString(passPhrase);
            publicKey = keyPair.ToPublicKeyString();
        }

        private static DateTime ConvertToRfc1123(DateTime dateTime)
        {
            return DateTime.ParseExact(
                dateTime.ToString("ddd, dd MMM yyyy", CultureInfo.InvariantCulture)
                , "ddd, dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }

        [Test]
        public void Can_Generate_And_Validate_Signature_With_Empty_License()
        {
            License license = License.New()
                                 .CreateAndSignWithPrivateKey(privateKey, passPhrase);

            Assert.That(license, Is.Not.Null);
            Assert.That(license.Signature, Is.Not.Null);

            // validate xml
            var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
            Assert.That(xmlElement.HasElements, Is.True);

            // validate default values when not set
            Assert.That(license.Id, Is.EqualTo(Guid.Empty));
            Assert.That(license.LicenseLevel, Is.EqualTo(LicenseType.Trial));
            Assert.That(license.Customer, Is.Null);
            Assert.That(license.LicenseExpirationDate, Is.EqualTo(ConvertToRfc1123(DateTime.MaxValue)));

            // verify signature
            Assert.That(license.VerifySignature(publicKey), Is.True);
        }

        [Test]
        public void Can_Generate_And_Validate_Signature_With_Standard_License()
        {
            var licenseId = Guid.NewGuid();
            const string customerName = "Max Mustermann";
            const string customerCompany = "Mustermann TLD";
            const string customerDomain = "mustermann.tld";
            DateTime expirationDate = DateTime.Now.AddYears(1);
            var productFeatures = new Dictionary<string, string>
                                      {
                                          {"Sales Module", "yes"},
                                          {"Purchase Module", "yes"},
                                          {"Maximum Transactions", "10000"}
                                      };

            License license = License.New()
                                 .WithUniqueIdentifier(licenseId)
                                 .As(LicenseType.CompanySite)
                                 .LicensedTo(customerName, customerCompany, customerDomain)
                                 .WithProductFeatures(productFeatures)
                                 .LicenseExpiresAt(expirationDate)
                                 .CreateAndSignWithPrivateKey(privateKey, passPhrase);

            Assert.That(license, Is.Not.Null);
            Assert.That(license.Signature, Is.Not.Null);

            // validate xml
            var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
            Assert.That(xmlElement.HasElements, Is.True);

            // validate default values when not set
            Assert.That(license.Id, Is.EqualTo(licenseId));
            Assert.That(license.LicenseLevel, Is.EqualTo(LicenseType.CompanySite));
            Assert.That(license.LicensedFeatures, Is.Not.Null);
            Assert.That(license.LicensedFeatures.GetAll(), Is.EquivalentTo(productFeatures));
            Assert.That(license.Customer, Is.Not.Null);
            Assert.That(license.Customer.Name, Is.EqualTo(customerName));
            Assert.That(license.Customer.Domain, Is.EqualTo(customerDomain));
            Assert.That(license.LicenseExpirationDate, Is.EqualTo(ConvertToRfc1123(expirationDate)));

            // verify signature
            Assert.That(license.VerifySignature(publicKey), Is.True);
        }

#if SUPPORT
        [Test]
        public void Can_Detect_Hacked_License()
        {
            var licenseId = Guid.NewGuid();
            var customerName = "Max Mustermann";
            var customerEmail = "max@mustermann.tld";
            var expirationDate = DateTime.Now.AddYears(1);
            var productFeatures = new Dictionary<string, string>
                                      {
                                          {"Sales Module", "yes"},
                                          {"Purchase Module", "yes"},
                                          {"Maximum Transactions", "10000"}
                                      };

            var license = License.New()
                                 .WithUniqueIdentifier(licenseId)
                                 .As(LicenseType.Standard)
                                 .WithProductFeatures(productFeatures)
                                 .LicensedTo(customerName, customerEmail)
                                 .LicenseExpiresAt(expirationDate)
                                 .CreateAndSignWithPrivateKey(privateKey, passPhrase);

            Assert.That(license, Is.Not.Null);
            Assert.That(license.Signature, Is.Not.Null);

            // verify signature
            Assert.That(license.VerifySignature(publicKey), Is.True);

            // validate xml
            var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
            Assert.That(xmlElement.HasElements, Is.True);

            // manipulate xml
            Assert.That(xmlElement.Element("Purchase Module"), Is.Not.Null);
            xmlElement.Element("Purchase Module").Value = "no"; // now we want to have 11 licenses

            // load license from manipulated xml
            var hackedLicense = License.Load(xmlElement.ToString());

            // validate default values when not set
            Assert.That(hackedLicense.Id, Is.EqualTo(licenseId));
            Assert.That(hackedLicense.Type, Is.EqualTo(LicenseType.Standard));
            Assert.That(hackedLicense.ProductFeatures, Is.Not.Null);
            Assert.That(hackedLicense.ProductFeatures.GetAll(), Is.EquivalentTo(productFeatures));
            Assert.That(hackedLicense.Customer, Is.Not.Null);
            Assert.That(hackedLicense.Customer.Name, Is.EqualTo(customerName));
            Assert.That(hackedLicense.Customer.Email, Is.EqualTo(customerEmail));
            Assert.That(hackedLicense.LicenseExpiration, Is.EqualTo(ConvertToRfc1123(expirationDate)));

            // verify signature
            Assert.That(hackedLicense.VerifySignature(publicKey), Is.False);
        }
#endif
    }
}
