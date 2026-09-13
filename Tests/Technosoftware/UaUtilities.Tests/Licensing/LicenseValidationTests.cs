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
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Technosoftware.UaUtilities;
#endregion Using Directives

namespace Technosoftware.UaUtilities.Tests
{
    [TestFixture]
    public class LicenseValidationTests
    {
        [Test]
#pragma warning restore RCS0056 // A line is too long
#pragma warning restore RCS0056 // A line is too long
        public void Can_Validate_Expired_ExpirationDate()
        {
            // Equivalent of the licence this test used to carry, re-minted with the
            // throw-away fixture key: an expired Trial for OPC UA Solutions Client 4.0.0.
            // See Fixtures/README.md.
            const string licenseData = @"<License>
  <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
  <License>
    <Level>Trial</Level>
    <ExpirationDate>Wed, 30 Apr 2025</ExpirationDate>
  </License>
  <Product>
    <Type>Client</Type>
    <Name>OPC UA Solutions</Name>
    <Version>4.0.0</Version>
    <PublishedDate>Fri, 25 Apr 2025</PublishedDate>
  </Product>
  <Support>
    <Level>None</Level>
    <ExpirationDate>Wed, 31 Dec 2025</ExpirationDate>
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
  <Signature>MIGHAkEcpOj7XqTUZi0ex0Qhk50ui/20DKe6gmoPfNvU+Hdirn6gHRH1IRWGgScn1ssx1gzxzrhiJCHJvcNWefsZTVrkUwJCAMdicI9W4czmsaIApVLNNoLJk+sAPWRIa182lYZooI6wUR5Eg997btZZQyGRsxiCicwk2EoveO751No1pEhZ5Skv</Signature>
</License>";
            var license = License.Load(licenseData);

            var validationResults = license
                .Validate()
                .LicenseExpirationDate()
                .AssertValidLicense().ToList();

            Assert.That(validationResults, Is.Not.Null);
            Assert.That(validationResults.Count, Is.EqualTo(1));
            Assert.That(validationResults.FirstOrDefault(), Is.TypeOf<LicenseExpiredValidationFailure>());
        }

        [Test]
        public void Do_Not_Crash_On_Invalid_Data()
        {
            const string publicKey = "1234";
            const string licenseData =
                "<license expiration='2013-06-30T00:00:00.0000000' type='Trial'><name>John Doe</name></license>";

            var license = License.Load(licenseData);

            var validationResults = license
                .Validate()
                .LicenseExpirationDate()
                .And()
                .Signature(publicKey)
                .AssertValidLicense().ToList();

            Assert.That(validationResults, Is.Not.Null);
            Assert.That(validationResults.Count, Is.EqualTo(1));
            Assert.That(validationResults.FirstOrDefault(), Is.TypeOf<InvalidSignatureValidationFailure>());
        }

        [Test]
        public void Test_ValidationChainBuilder_ValidationFailure_List()
        {
            var keyGenerator = KeyGenerator.Create();
            KeyPair keyPair = keyGenerator.GenerateKeyPair();
            string publicKey = keyPair.ToPublicKeyString();

            const string invalidLicense = @"<License>
  <Signature>WFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFhYWFg=</Signature>
</License>";

            var licenseToVerify = License.Load(invalidLicense);

            IEnumerable<IValidationFailure> validationFailures = licenseToVerify
                .Validate()
                .Signature(publicKey)
                .AssertValidLicense();

            int count = (from IValidationFailure v in validationFailures
                         select v).Count();

            Assert.That(count, Is.EqualTo(1));
            Assert.That(validationFailures.ToArray().Length, Is.EqualTo(1));
            Assert.That(validationFailures.ToArray().Length, Is.EqualTo(1));
        }
    }
}
