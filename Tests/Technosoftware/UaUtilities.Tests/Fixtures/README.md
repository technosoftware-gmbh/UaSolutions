# Licence fixtures

Everything in this folder exists to keep **production-signed licences out of the test
suite**.

## Why

The fixtures used to carry licences signed with the production key. Those licences work
against the packages customers install — anyone holding the strings could licence the
product. They also came within one `git commit` of landing in the public repository
during the September 2026 repository split.

## The rule

**No licence in any fixture may be signed with the production key.**

`FixtureKeys.cs` holds a throw-away key pair generated for this purpose. Licences signed
with it validate against `FixtureKeys.PublicKey` and are rejected by every shipped
package. `LicenseFixtureIsolationTests` asserts exactly that, so a fixture that drifts
back to the production key fails the build rather than passing quietly.

The private key is committed deliberately. It is worthless: it can only mint licences
that no customer package accepts.

## Re-signing or adding a fixture

Mint it with the public API — no internals needed:

```csharp
License.New()
    .WithUniqueIdentifier(Guid.NewGuid())
    .As(LicenseType.Trial)
    .As(ProductType.Client)
    .WithProduct("OPC UA Solutions", "Client", "6.0.0", "Fri, 25 Apr 2025")
    .WithLicenseInfo("Trial", "Wed, 30 Apr 2030")
    .WithSupport("None", "Wed, 31 Dec 2030")
    .LicensedTo("Fixture User", "Fixture GmbH", "fixture.invalid")
    .CreateAndSignWithPrivateKey(FixtureKeys.PrivateKey, FixtureKeys.PassPhrase);
```

If the key pair itself ever has to be regenerated, generate a new throw-away pair with
`KeyGenerator.Create().GenerateKeyPair()` and re-mint the corpus. Never substitute the
production key pair.

## Still to do

`OldLicenseTests` and `NewLicenseTests` are still production-signed. They validate
through `LicenseHandler.Instance.Validate(...)`, which uses the key baked into the
assembly, so re-signing them needs a build-flavour seam rather than a key parameter.
Project doc 13 has the design.
