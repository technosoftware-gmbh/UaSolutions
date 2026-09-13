#region Copyright (c) 2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com
//
// The Software is subject to the Technosoftware GmbH Source Code License Agreement,
// which can be found here:
// https://technosoftware.com/documents/Source_License_Agreement.pdf
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
#endregion Using Directives

namespace Technosoftware.UaUtilities.Tests
{
    /// <summary>
    /// Guards the rule that no licence used by the tests is accepted by a shipped
    /// package. See Fixtures/README.md.
    /// </summary>
    [TestFixture]
    [Category("Licensing")]
    public class LicenseFixtureIsolationTests
    {
        /// <summary>
        /// A licence minted with the fixture key must validate against the fixture key.
        /// </summary>
        [Test]
        public void FixtureLicenseIsValidAgainstTheFixtureKey()
        {
            string xml = MintFixtureLicense();

            List<IValidationFailure> failures = License.Load(xml)
                .Validate()
                .Signature(FixtureKeys.PublicKey)
                .AssertValidLicense()
                .ToList();

            Assert.That(failures, Is.Empty);
        }

        /// <summary>
        /// The same licence must be rejected by the key that ships in the product. This is
        /// what makes the fixture corpus worthless to anyone who reads it.
        /// </summary>
        [Test]
        public void FixtureLicenseIsRejectedByTheProductionKey()
        {
            string xml = MintFixtureLicense();

            List<IValidationFailure> failures = License.Load(xml)
                .Validate()
                .Signature(LicenseHandler.PublicKey)
                .AssertValidLicense()
                .ToList();

            Assert.That(failures, Is.Not.Empty, "A fixture licence must never validate against the production key.");
            Assert.That(failures.First(), Is.TypeOf<InvalidSignatureValidationFailure>());
        }

        /// <summary>
        /// The fixture key pair must not be the production key.
        /// </summary>
        [Test]
        public void FixtureKeyIsNotTheProductionKey()
        {
            Assert.That(FixtureKeys.PublicKey, Is.Not.EqualTo(LicenseHandler.PublicKey));
        }

        private static string MintFixtureLicense()
        {
            return License.New()
                .WithUniqueIdentifier(Guid.NewGuid())
                .As(LicenseType.Trial)
                .As(ProductType.Client)
                .WithProduct("OPC UA Solutions", "Client", "6.0.0", "Fri, 25 Apr 2025")
                .WithLicenseInfo("Trial", "Tue, 31 Dec 2030")
                .WithSupport("None", "Tue, 31 Dec 2030")
                .LicensedTo("Fixture User", "Fixture GmbH", "fixture.invalid")
                .CreateAndSignWithPrivateKey(FixtureKeys.PrivateKey, FixtureKeys.PassPhrase)
                .ToString();
        }
    }
}
