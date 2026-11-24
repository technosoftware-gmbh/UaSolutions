#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on https://github.com/junian/Standard.Licensing. 
// The complete license agreement for that can be found in this directore in the LICENSE.txt file.
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using static System.String;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// Manages the license to enable the different product versions.
    /// </summary>
    public sealed class LicenseHandler
    {
        #region Constants
        private const string kProductId = "OPC UA Solutions";

        /// <summary>
        /// The first major version that accepts licenses for its own major version only.
        /// </summary>
        private const int kFirstSameMajorOnlyVersion = 7;

        private const string kMessageLicenseInvalidProductIdentity = "License is not associated with this product.";
        private const string kMessageLicenseResolve = "Please contact our Support at technosoftware.com.";

        private const string kProductFeature_Name_Product = "Product";
        private const string kProductFeature_Name_Version = "Version";
        private const string kProductFeature_Name_PublishDate = "Publish Date";

        /// <summary>
        /// License Validation Parameters String for the OPC UA Solutions .NET
        /// </summary>
        internal const string PublicKey =
            "MIGbMBAGByqGSM49AgEGBSuBBAAjA4GGAAQBHYen40AkVbB/sfuwuN1M8bNacAlvr3vH30s+H243HGr4rFsgLEVrZUgswDZVWQVpieFXZxmnIUm5FaDiem3ETRUA+aBerUJN/cNz1GbRh0bw3W5KIp7etYUkS+VgK4kNAgGkRODYmdi0OSYzpskfugg12LEUNMOmuI7ozcHm8cgC1rg=";

        /// <summary>
        /// The public key used before the key rotation. Licences issued to customers under
        /// that key are still theirs, so signature validation accepts either key. Retire
        /// this together with the product versions those licences were issued for; until
        /// then, removing it would lock those customers out.
        /// </summary>
        internal const string PublicKeyV3 =
            "MIGbMBAGByqGSM49AgEGBSuBBAAjA4GGAAQBRd0y7wsTBu49OWBqXTcxkrr8uUuJbuWLcPqw123Ovs4ANiOup1Sjq2jSsZopqpymvxiiqiQITst7HxCKP4MSMSYB3Slm0EPUhrD/FRJ46RU5er366BrBRDJX1IbbJoSsXtLJdwkTi/EjJmslh3F3fDdbTgpH3tb/bwQ1ieGfrPSaVEw=";

        /// <summary>
        /// The public key licences are validated against.
        /// </summary>
        /// <remarks>
        /// In a normal build this is always <see cref="PublicKey"/> and nothing can change
        /// it. A build with the LICENSE_TEST_SEAM constant additionally exposes
        /// <c>PublicKeyOverride</c>, so that test fixtures can be signed with a throw-away
        /// key instead of the production one. That override does not exist in the assembly
        /// shipped to customers - not internal, not private, absent. See project doc 13.
        /// </remarks>
        internal static string ActivePublicKey
        {
#if LICENSE_TEST_SEAM
            get => s_publicKeyOverride ?? PublicKey;
#else
            get => PublicKey;
#endif
        }

#if LICENSE_TEST_SEAM
        /// <summary>
        /// Test-only seam: overrides <see cref="ActivePublicKey"/>. Set it to <c>null</c> to
        /// go back to the production key. Never present in a shipped build.
        /// </summary>
        internal static string PublicKeyOverride
        {
            get => s_publicKeyOverride;
            set => s_publicKeyOverride = value;
        }

        private static string s_publicKeyOverride;
#endif

        internal const string TrialKey =
#pragma warning disable RCS1266 // Use raw string literal
                                       @"<License>
                                          <Id>5d019606-c120-42da-9975-ac5a94e02d07</Id>
                                          <Product>
                                            <Name>OPC UA Solutions</Name>
                                            <Type>ClientAndServer</Type>
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
                                            <Attribute name=""Product Identity"">a8482b9cc1d5cf8519bb80e0c979315703643f74ced77435eae31a6208b9c738</Attribute>
                                            <Attribute name=""Assembly Identity""></Attribute>
                                          </LicenseAttributes>
                                          <Signature>MIGHAkEqasMWOapfccOYgamqV3vplI2Zqf8AvuUYwPxmUD1kVHnJjArpfpd+6+BuHAJjDUPttYiDUmB2zb2c7PJooWm/7wJCANd3jHy8q17tKg2/nsyDyfCYO9qNgvE0ORWP2qZ4u+P3a+FHgEGNQPHG5HmmzMMkFP5Dmb2MIphTVK4T+eoNIsvt</Signature>
                                        </License>";
#pragma warning restore RCS1266 // Use raw string literal
        #endregion Constants

        #region Constructors, Initialization
        /// <summary>
        /// Private constructor.
        /// </summary>
        private LicenseHandler()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (!IsLicenseLoaded)
            {
                LoadLicense(TrialKey);
                if (IsEvaluation && s_baseTimer == null)
                {
                    s_baseTimer = new EvaluationTimer();
                    EvaluationTimer.Start(EvaluationPeriod);
                }
            }
        }
        #endregion Constructors, Initialization

        #region Instance
        /// <summary>
        /// Public Singleton Instance getter.
        /// </summary>
        public static LicenseHandler Instance => s_lazyInstance.Value;
        #endregion Instance

        #region Public Properties
        /// <summary>
        /// Gets the <see cref="Customer"/> of this <see cref="License"/>.
        /// </summary>
        public Customer Customer
        {
            get
            {
                if (s_licenseLoaded != null)
                {
                    return s_licenseLoaded.Customer;
                }
                return null;
            }
        }

        /// <summary>
        /// The type of the license, either Trial, Standard, Developer or Company.
        /// </summary>
        public LicenseType LicensedProductType
        {
            get
            {
                if (s_licenseLoaded != null)
                {
                    return s_licenseLoaded.LicenseLevel;
                }
                return LicenseType.Trial;
            }
        }

        /// <summary>
        /// The type of the license, either Trial, Standard, Developer or Company.
        /// </summary>
        public ProductType LicensedType { get; private set; } = ProductType.ClientAndServer;

        /// <summary>
        /// The product the license is for.
        /// </summary>
        public string LicensedProduct
        {
            get
            {
                if (s_licenseLoaded != null)
                {
                    return s_licenseLoaded.ProductName;
                }
                return Empty;
            }
        }

        /// <summary>
        /// The version of the product the license is for.
        /// </summary>
        public Version LicenseVersion
        {
            get
            {
                if (s_licenseLoaded != null && s_licenseLoaded.ProductVersion != null)
                {
                    return s_licenseLoaded.ProductVersion;
                }
                return ProductVersion;
            }
        }

        /// <summary>
        /// The license expiration date in UTC time.
        /// </summary>
        public DateTime LicenseExpirationDate
        {
            get
            {
                if (s_licenseLoaded != null && s_licenseLoaded.LicenseInfo != null)
                {
                    return s_licenseLoaded.LicenseExpirationDate;
                }
                return DateTime.MaxValue;
            }
        }

        /// <summary>
        /// The number of days remaining until the license expires.
        /// </summary>
        public int LicenseExpirationDays => Convert.ToInt32(LicenseExpirationDate.Subtract(DateTime.UtcNow.Date).TotalDays);

        /// <summary>
        /// The type of support included in the license.
        /// </summary>
        public SupportLevel Support
        {
            get
            {
                if (s_licenseLoaded != null)
                {
                    return s_licenseLoaded.SupportLevel;
                }
                return SupportLevel.None;
            }
        }

        /// <summary>
        /// The support expiration date in UTC time.
        /// </summary>
        public DateTime SupportExpirationDate
        {
            get
            {
                if (s_licenseLoaded != null)
                {
                    return s_licenseLoaded.SupportExpirationDate;
                }
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// The number of days remaining until the support expires.
        /// </summary>
        public int SupportExpirationDays => Convert.ToInt32(SupportExpirationDate.Subtract(DateTime.UtcNow.Date).TotalDays);

        /// <summary>
        /// The date the product was published.
        /// </summary>
        public DateTime? PublishDate
        {
            get
            {
                if (s_licenseLoaded != null)
                {
                    return s_licenseLoaded.PublishedDate;
                }
                return null;
            }
        }

        /// <summary>
        /// The date the product was published.
        /// </summary>
        public string ProductIdentity
        {
            get
            {
                if (s_licenseLoaded != null)
                {
                    return s_licenseLoaded.ProductIdentity;
                }
                return Empty;
            }
        }

        /// <summary>
        /// Whether the license was issued before 1-OCT-2025 (V4.0.4) or not.
        /// </summary>
        public bool IsNewLicenseType
        {
            get
            {
                if (s_licenseLoaded != null)
                {
                    return s_licenseLoaded.IsNewLicenseType;
                }
                return false;
            }
        }

        /// <summary>
        /// The product features included in this license.
        /// </summary>
        public Dictionary<string, string> OldProductFeatures { get; } = [];

        /// <summary>
        /// The product features included in this license.
        /// </summary>
        public Dictionary<string, string> ProductFeatures { get; } = [];

        /// <summary>
        /// The additional license attributes included in this license.
        /// </summary>
        public Dictionary<string, string> LicenseAttributes { get; } = [];

        /// <summary>
        /// The evaluation period in mutes per application start.
        /// </summary>
        public int EvaluationPeriod
        {
            get
            {
                if (s_licenseLoaded != null)
                {
                    s_runtimeMinutes = 120;
                }
                return s_runtimeMinutes;
            }
        }

        /// <summary>
        /// The name of the licensee.
        /// </summary>
        public string Name { get; } = Empty;

        /// <summary>
        /// The domain or email address of the licensee.
        /// </summary>
        public string Domain { get; } = Empty;

        /// <summary>
        /// The company of the licensee.
        /// </summary>
        public string Company { get; } = Empty;

        /// <summary>
        /// Whether the license is locked to a specific assembly (instance of the product).
        /// </summary>
        public bool IsLockedToAssembly { get; }

        /// <summary>
        /// Returns whether the product is a licensed product.
        /// </summary>
        /// <returns>Returns true if the product is licensed; false if it is used in evaluation mode or license is expired.</returns>
        public bool IsLicensed => IsVersionSupported() &&
            !IsExpired &&
            !IsEvaluation;

        /// <summary>
        /// Returns whether the product is an evaluation version.
        /// </summary>
        /// <returns>Returns true if the product is an evaluation; false if it is a product or license is expired.</returns>
        public bool IsEvaluation => (LicensedProductType == LicenseType.Trial) &&
            IsVersionSupported() &&
            !IsExpired;

        /// <summary>
        /// Indicates whether the evaluation runtime period (120 minutes) is over and a restart is required or not.
        /// </summary>
        public bool IsRestartRequired => s_baseTimer != null && s_baseTimer.IsExpired;

        /// <summary>
        /// Indicates whether the evaluation period is used and a product license is required or not.
        /// </summary>
        public bool IsExpired
        {
            get
            {
                if (LicenseExpirationDate.Date != DateTime.MaxValue.Date && LicenseExpirationDate.Date < DateTime.UtcNow)
                {
                    return true;
                }
                else if (
                    IsServicePatch &&
                    SupportExpirationDate.Date != DateTime.MaxValue.Date &&
                    SupportExpirationDate.Date < DateTime.UtcNow)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Whether the product is a service patch (buil NOT 0) or not.
        /// </summary>
        public bool IsServicePatch => ProductVersion.Build != 0;

        /// <summary>
        /// Returns the ProductVersion of the product.
        /// </summary>
        public Version ProductVersion
        {
            get
            {
                var version = new Version(0, 0, 0, 0);
                try
                {
                    Assembly assembly = typeof(LicenseHandler).Assembly;

                    var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

                    // Convert to LicenseVersion
                    version = new Version(
                        versionInfo.FileMajorPart,
                        versionInfo.FileMinorPart,
                        versionInfo.FileBuildPart,
                        versionInfo.FilePrivatePart
                    );
                    if (SimulateServicePatch && version.Build == 0)
                    {
                        version = new Version(
                            versionInfo.FileMajorPart,
                            versionInfo.FileMinorPart,
                            1,
                            versionInfo.FilePrivatePart
                        );
                    }
                }
                catch (Exception)
                {
                    return version;
                }
                return version;
            }
        }

        /// <summary>
        /// Returns the licensed OPC UA Features.
        /// </summary>
        public ProductFeature LicensedFeatures { get; internal set; } = ProductFeature.AllFeatures;
        #endregion Public Properties

        #region Private Properties
        private bool IsLicenseLoaded { get; set; }
        #endregion Private Properties

        #region Public Methods
        /// <summary>
        /// Validate the license.
        /// </summary>
        /// <param name="applicationType">The product license required for the application.</param>
        /// <param name="serialNumber">Serial Number</param>
        public bool Validate(ProductType? applicationType, string serialNumber)
        {
            bool isLicensed;

            if (!LoadLicense(serialNumber))
            {
                return false;
            }

            isLicensed = IsLicenseValid(applicationType, out _);

            if (IsEvaluation && s_baseTimer == null)
            {
                s_baseTimer = new EvaluationTimer();
                EvaluationTimer.Start(EvaluationPeriod);
            }
            else if (isLicensed && s_baseTimer != null)
            {
                EvaluationTimer.Stop();
                s_baseTimer = null;
            }
            return isLicensed;
        }

        /// <summary>
        /// Core Feature validation
        /// </summary>
        /// <param name="productType">The product license required for the application.</param>
        /// <param name="requiredProductFeature">The feature the license must support.</param>
        /// <param name="logger">The logger to be used.</param>
        /// <exception cref="NotSupportedException">Throws an exception if license is not valid</exception>
        public void ValidateFeatures(
            ProductType productType,
            ProductFeature requiredProductFeature,
            ILogger logger)
        {
            IsLicenseValid(productType);

            if (IsLicenseLoaded && !IsEvaluation && !IsProductTypeCovered(LicensedType, productType))
            {
                logger?.LogInformation(
                    "Used LicensedProduct = {LicensedProduct}, LicensedType = {LicensedType}, required = {ProductType}.",
                    LicensedProduct,
                    LicensedType,
                    productType);
                throw new NotSupportedException(
                    $"A {LicensedType} license does not cover a {productType}. You can't use this feature.");
            }

            if (IsRestartRequired)
            {
                logger?.LogInformation(
                    "Used LicensedProduct = {LicensedProduct}, Features = {LicensedFeatures}, ProductVersion = {ProductVersion}.",
                    LicensedProduct,
                    LicensedFeatures,
                    ProductVersion);
                throw new NotSupportedException("Evaluation time expired! You need to restart the application.");
            }
            if (IsExpired)
            {
                logger?.LogInformation(
                    "Used LicensedProduct = {LicensedProduct}, Features = {LicensedFeatures}, ProductVersion = {ProductVersion}.",
                    LicensedProduct,
                    LicensedFeatures,
                    ProductVersion);
                throw new NotSupportedException("License required! You can't use this feature.");
            }

            if (requiredProductFeature != ProductFeature.None &&
                LicensedFeatures != ProductFeature.AllFeatures &&
                (requiredProductFeature & LicensedFeatures) != requiredProductFeature)
            {
                string message =
                    $"Feature {requiredProductFeature} required but only {LicensedFeatures} licensed! You can't use this feature.";
                logger?.LogInformation(
                        "Feature {RequiredProductFeature} required but only {LicensedFeatures} licensed! You can't use this feature.",
                        requiredProductFeature,
                        LicensedFeatures);

                throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// Checks if a support contract is available and valid.
        /// </summary>
        /// <returns>true if the support contract is valid; false if no support contract exists or is expired.</returns>
        public bool IsSupportContractValid()
        {
            return IsSupportContractValid(Support, SupportExpirationDate, PublishDate);
        }

        /// <summary>
        /// Checks if the license supports the current used version.
        /// </summary>
        /// <returns>true if the license supports this version; false if not.</returns>
        public bool IsVersionSupported()
        {
            return IsVersionSupported(LicenseVersion, ProductVersion, IsNewLicenseType, IsSupportContractValid);
        }

        /// <summary>
        /// Checks a licence, without applying it, against a product version and type. Use it
        /// to verify a licence before it is handed to a customer.
        /// </summary>
        /// <remarks>
        /// Nothing about the running application changes: the licence in use, the evaluation
        /// timer and every property of <see cref="Instance"/> stay as they are. The rules are
        /// the ones <see cref="Validate"/> applies at run time for that version, including those
        /// of earlier major versions, so the check is meaningful for any released version.
        /// </remarks>
        /// <param name="licenseData">The licence XML as issued.</param>
        /// <param name="productVersion">
        /// The released product version, e.g. 7.0.1. Only major and minor are relevant: the
        /// patch number is carried outside the field the licence check reads (see common.props).
        /// </param>
        /// <param name="productType">
        /// The product the licence must cover, e.g. <see cref="ProductType.Server"/>, or
        /// <c>null</c> to accept any product type.
        /// </param>
        /// <returns>The result, with the reasons if the licence is not valid.</returns>
        public static LicenseCheckResult CheckLicense(string licenseData, Version productVersion, ProductType? productType)
        {
            if (productVersion == null)
            {
                throw new ArgumentNullException(nameof(productVersion));
            }

            var releasedVersion = new Version(productVersion.Major, Math.Max(productVersion.Minor, 0), 0);
            List<string> problems = [];

            License license = null;
            string verifiedKey = null;
            try
            {
                if (!IsNullOrEmpty(licenseData))
                {
                    license = License.Load(licenseData);
                    if (license.VerifySignature(ActivePublicKey))
                    {
                        verifiedKey = ActivePublicKey;
                    }
                    else if (license.VerifySignature(PublicKeyV3))
                    {
                        verifiedKey = PublicKeyV3;
                    }
                }
            }
            catch (Exception e) when (e is not OutOfMemoryException)
            {
                license = null;
            }

            if (license == null)
            {
                problems.Add("The licence data could not be read.");
                return new LicenseCheckResult(releasedVersion, productType, null, problems);
            }
            if (verifiedKey == null)
            {
                problems.Add("The signature is not valid: the licence was not issued by Technosoftware or was changed after it was issued.");
                return new LicenseCheckResult(releasedVersion, productType, license, problems);
            }

            // Reading ProductVersion is also what sets License.IsNewLicenseType, so read them
            // in this order, the same order IsVersionSupported() reads them at run time.
            Version licenseVersion = license.ProductVersion;
            bool isNewLicenseType = license.IsNewLicenseType;
            DateTime? publishedDate = license.Product != null ? license.PublishedDate : null;

            if (isNewLicenseType &&
                SecureHash.ComputeSHA256Hash(CreateProductIdentity(kProductId, verifiedKey)) != license.ProductIdentity)
            {
                problems.Add(kMessageLicenseInvalidProductIdentity);
            }

            if (!IsVersionSupported(
                licenseVersion,
                releasedVersion,
                isNewLicenseType,
                () => IsSupportContractValid(license.SupportLevel, license.SupportExpirationDate, publishedDate)))
            {
                problems.Add(string.Format(
                    CultureInfo.InvariantCulture,
                    "A licence for version {0} does not unlock version {1}.{2}.",
                    licenseVersion?.ToString() ?? "(none)",
                    releasedVersion.Major,
                    releasedVersion.Minor));
            }

            DateTime expirationDate = license.LicenseExpirationDate;
            if (expirationDate.Date != DateTime.MaxValue.Date && expirationDate.Date < DateTime.UtcNow)
            {
                problems.Add(string.Format(
                    CultureInfo.InvariantCulture,
                    "The licence expired on {0:yyyy-MM-dd}.",
                    expirationDate));
            }

            if (license.LicenseLevel == LicenseType.Trial)
            {
                problems.Add("This is a Trial licence: the libraries only run in evaluation mode with it.");
            }

            ProductType licensedType = license.Product != null && !IsNullOrEmpty(license.Product.Name)
                ? license.ProductType
                : ProductType.ClientAndServer;
            if (productType.HasValue && !IsProductTypeCovered(licensedType, productType.Value))
            {
                problems.Add(string.Format(
                    CultureInfo.InvariantCulture,
                    "A {0} licence does not cover a {1}.",
                    licensedType,
                    productType.Value));
            }

            return new LicenseCheckResult(releasedVersion, productType, license, problems);
        }

        /// <summary>
        /// Checks wether the used feature is supported by the license provided.
        /// </summary>
        /// <param name="applicationType">The product license required for the application.</param>
        /// <param name="requiredProductFeature">The feature the license must support.</param>
        /// <returns>Returns true if the feature is supported by the provided license; false if it is not supported or the license is expired.</returns>
        /// <returns>True if the license qualifies for the requested application and edition or if the evaluation period is still running; otherwise False.</returns>
        public bool IsFeatureSupported(ProductType? applicationType, ProductFeature requiredProductFeature = ProductFeature.None)
        {
            if (!IsLicenseLoaded)
            {
                return false;
            }

            if (IsEvaluation)
            {
                return true;
            }
            if (IsLicenseValid(applicationType) && (LicensedFeatures.HasFlag(ProductFeature.AllFeatures) || LicensedFeatures.HasFlag(requiredProductFeature)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if the licensed product provided through ValidateLicense qualifies for the given application type and license edition.
        /// </summary>
        /// <param name="applicationType">The product license required for the application.</param>
        /// <returns>True if the license qualifies for the requested application and edition or if the evaluation period is still running; otherwise False.</returns>
        public bool IsLicenseValid(ProductType? applicationType = null)
        {
            if (!IsLicenseLoaded)
            {
                return false;
            }

            if (IsLicenseLoaded && !IsLicensed)
            {
                return false;
            }

            if (applicationType.HasValue && !IsProductTypeCovered(LicensedType, applicationType.Value))
            {
                return false;
            }
            return Check();
        }
        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// The version rules: whether a licence for <paramref name="licenseVersion"/> unlocks
        /// <paramref name="productVersion"/>.
        /// </summary>
        /// <remarks>
        /// One rule set for every major version, so that <see cref="CheckLicense"/> can check a
        /// licence against any released version and every branch carries the same code:
        /// <list type="bullet">
        /// <item>From 7.0 on, only a licence for the same major version is accepted. Old-format
        /// licences and licences for any other major version are rejected.</item>
        /// <item>6.x keeps the rules it shipped with: old-format licences unlock x.y.0 builds, and
        /// a licence for an earlier major version unlocks it with a valid support contract.</item>
        /// <item>Within the major version, the licensed minor version is the lowest one unlocked.</item>
        /// </list>
        /// </remarks>
        private static bool IsVersionSupported(
            Version licenseVersion,
            Version productVersion,
            bool isNewLicenseType,
            Func<bool> isSupportContractValid)
        {
            if (licenseVersion == null || productVersion == null)
            {
                return false;
            }

            if (!isNewLicenseType)
            {
                // Old license types were only ever issued for versions 3 and 4. Version 6 still
                // accepts them for builds that are not service patches; version 7 never does.
                return productVersion.Major is >= 3 and < kFirstSameMajorOnlyVersion &&
                    productVersion.Build == 0;
            }

            if (licenseVersion.Major == productVersion.Major &&
                licenseVersion.Minor == productVersion.Minor &&
                licenseVersion.Build == productVersion.Build)
            {
                // Exact version match
                return true;
            }

            if (licenseVersion.Major != productVersion.Major)
            {
                if (productVersion.Major >= kFirstSameMajorOnlyVersion)
                {
                    // From 7.0 on, a license unlocks its own major version only. Customers
                    // holding a valid support contract when a new major version is released
                    // are issued a new license for it.
                    return false;
                }
                if (licenseVersion.Major != 0 && licenseVersion.Major < productVersion.Major)
                {
                    // Version 6 accepts a license for an earlier major version if it came
                    // with a valid support contract.
                    return isSupportContractValid();
                }
            }

            if (productVersion.Build != 0 && !isSupportContractValid())
            {
                // Service Patches are only allowed if support contract is valid
                return false;
            }
            return licenseVersion.Major != productVersion.Major || licenseVersion.Minor <= productVersion.Minor;
        }

        /// <summary>
        /// Whether a support contract was included that was still running when the licensed
        /// version was published.
        /// </summary>
        private static bool IsSupportContractValid(SupportLevel support, DateTime supportExpirationDate, DateTime? publishDate)
        {
            if (support == SupportLevel.None)
            {
                return false;
            }
            return !(supportExpirationDate < publishDate);
        }

        /// <summary>
        /// Whether a license issued for <paramref name="licensedType"/> covers an application
        /// that requires <paramref name="requiredType"/>.
        /// </summary>
        /// <remarks>
        /// A Client license never unlocks a server. A Client Gateway license covers the
        /// OPC UA Server part the gateway is built on.
        /// </remarks>
        internal static bool IsProductTypeCovered(ProductType licensedType, ProductType requiredType)
        {
            if (licensedType == ProductType.ClientAndServer || licensedType == requiredType)
            {
                return true;
            }
            return requiredType == ProductType.Server && licensedType == ProductType.ClientGateway;
        }

        /// <summary>
        /// The identity of the product and public key passed to the ValidateLicense() method.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="keyPublic"></param>
        /// <returns></returns>
        private static string CreateProductIdentity(string productId, string keyPublic)
        {
            return productId + " " + keyPublic;
        }

        /// <summary>
        /// Check the product license if it is an old license version.
        /// </summary>
        /// <returns>True if the license is a valid old license version; otherwise False.</returns>
        private bool IsOldProductLicense()
        {
            bool isOldLicense = false;

            if (s_licenseLoaded.OldProductFeatures != null)
            {
                isOldLicense = true;

                OldProductFeatures.Clear();
                foreach (KeyValuePair<string, string> feature in s_licenseLoaded.OldProductFeatures.GetAll())
                {
                    // Skip the reserved feature names we already handle specifically
                    if (feature.Key is not kProductFeature_Name_Product and
                         not kProductFeature_Name_Version and
                         not kProductFeature_Name_PublishDate)
                    {
                        OldProductFeatures.Add(feature.Key, feature.Value);
                    }
                }
                string productUaClientNet = OldProductFeatures.TryGetValue("UA Client .NET", out string value) ? value : Empty;
                string productUaClient = OldProductFeatures.TryGetValue("UaClient", out value) ? value : Empty;

                if (productUaClientNet.Equals("yes", StringComparison.Ordinal) ||
                    productUaClient.Equals("yes", StringComparison.Ordinal))
                {
                    LicensedType = ProductType.Client;
                }

                string productUaServerNet = OldProductFeatures.TryGetValue("UA Server .NET", out value) ? value : Empty;
                string productUaServer = OldProductFeatures.TryGetValue("UaServer", out value) ? value : Empty;

                if (productUaServerNet.Equals("yes", StringComparison.Ordinal) ||
                    productUaServer.Equals("yes", StringComparison.Ordinal))
                {
                    LicensedType = ProductType.Server;
                }

                string productUaBundleNet = ProductFeatures.TryGetValue("UA Bundle .NET", out value) ? value : Empty;

                if (productUaBundleNet.Equals("yes", StringComparison.Ordinal))
                {
                    LicensedType = ProductType.ClientAndServer;
                }
            }
            return isOldLicense;
        }

        /// <summary>
        /// Validate the product license.
        /// </summary>
        /// <param name="applicationType">The requested product license</param>
        private bool ValidateProductLicense(ProductType? applicationType = null)
        {
            bool isLicensed = false;

            if (!IsVersionSupported())
            {
                return false;
            }

            if (LicensedProductType == LicenseType.Trial &&
                LicenseExpirationDate.Date != DateTime.MaxValue.Date &&
                LicenseExpirationDate.Date < DateTime.UtcNow)
            {
                return false;
            }

            if (IsExpired)
            {
                return false;
            }

            if (IsNewLicenseType)
            {
                isLicensed = true;
            }
            else
            {
                string productUaClientNet = OldProductFeatures.TryGetValue("UA Client .NET", out string value) ? value : Empty;
                string productUaClient = OldProductFeatures.TryGetValue("UaClient", out value) ? value : Empty;

                if (productUaClientNet.Equals("yes", StringComparison.Ordinal) ||
                    productUaClient.Equals("yes", StringComparison.Ordinal))
                {
                    LicensedType = ProductType.Client;
                    isLicensed = true;
                }

                string productUaServerNet = OldProductFeatures.TryGetValue("UA Server .NET", out value) ? value : Empty;
                string productUaServer = OldProductFeatures.TryGetValue("UaServer", out value) ? value : Empty;

                if (productUaServerNet.Equals("yes", StringComparison.Ordinal) ||
                    productUaServer.Equals("yes", StringComparison.Ordinal))
                {
                    LicensedType = ProductType.Server;
                    isLicensed = true;
                }

                string productUaBundleNet = OldProductFeatures.TryGetValue("UA Bundle .NET", out value) ? value : Empty;

                if (productUaBundleNet.Equals("yes", StringComparison.Ordinal))
                {
                    LicensedType = ProductType.ClientAndServer;
                    isLicensed = true;
                }

                if (applicationType != LicensedType)
                {
                    LicensedType = ProductType.ClientAndServer;
                    isLicensed = false;
                }
            }
            return isLicensed;
        }

        /// <summary>
        /// Set the product features..
        /// </summary>
        private void SetProductFeature()
        {
            LicensedFeatures = ProductFeature.None;

            Dictionary<string, string> features;
            if (OldProductFeatures != null && OldProductFeatures.Count > 0)
            {
                features = OldProductFeatures;
            }
            else if (ProductFeatures != null)
            {
                features = ProductFeatures;
            }
            else
            {
                return;
            }

            string featureDataAccess = features.TryGetValue("DataAccess", out string value) ? value : Empty;
            string featureAlarmsConditions = features.TryGetValue("AlarmsConditions", out value) ? value : Empty;
            string featureHistoricalAccess = features.TryGetValue("HistoricalAccess", out value) ? value : Empty;
            string featureAllFeatures = features.TryGetValue("AllFeatures", out value) ? value : Empty;

            if (featureDataAccess.Equals("yes", StringComparison.Ordinal))
            {
                LicensedFeatures |= ProductFeature.DataAccess;
            }

            if (featureAlarmsConditions.Equals("yes", StringComparison.Ordinal))
            {
                LicensedFeatures |= ProductFeature.AlarmsConditions;
            }
            if (featureHistoricalAccess.Equals("yes", StringComparison.Ordinal))
            {
                LicensedFeatures |= ProductFeature.HistoricalAccess;
            }
            if (featureAllFeatures.Equals("yes", StringComparison.Ordinal))
            {
                LicensedFeatures |= ProductFeature.AllFeatures;
            }
            if (LicensedFeatures == ProductFeature.None)
            {
                LicensedFeatures = ProductFeature.AllFeatures;
            }
        }

        private bool Check()
        {
            if (IsLicensed)
            {
                return true;
            }
            if (IsExpired)
            {
                return false;
            }
            if (s_baseTimer != null && s_baseTimer.IsExpired)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Load the passed license.
        /// </summary>
        /// <param name="licenseKey">Path to the calling assembly associated with the license file.</param>
        /// <returns>True if the license was loaded successfully; otherwise False.</returns>
        private bool LoadLicense(string licenseKey)
        {
            s_licenseLoaded = null;
            IsLicenseLoaded = false;
            ProductFeatures.Clear();
            LicensedType = ProductType.ClientAndServer;

            lock (s_lock)
            {
                // The built-in evaluation licence reaches this method two ways: as an empty
                // key from an application that supplied none, and explicitly from Initialize().
                bool usedBuiltInTrial = IsNullOrEmpty(licenseKey) || Equals(licenseKey, TrialKey);
                if (usedBuiltInTrial)
                {
                    licenseKey = TrialKey;
                }

                try
                {
                    string xmlLicense = licenseKey;
                    if (IsNullOrEmpty(xmlLicense))
                    {
                        xmlLicense = TrialKey;
                        usedBuiltInTrial = true;
                    }
                    var license = License.Load(xmlLicense);

                    // The signature is what actually binds a license to Technosoftware.
                    // Without this check the Product Identity proves nothing: it is a hash
                    // of two values that both ship inside this assembly, so anyone could
                    // compute it and mint a license with a key of their own. Refuse the
                    // license here rather than further down, so that IsLicensed and every
                    // other property agree with the verdict.
                    // The built-in evaluation licence is a constant of this assembly and is
                    // always signed with the product key, so it is verified against that key
                    // rather than whatever the test seam has substituted. Otherwise enabling
                    // the seam would silently disable evaluation mode.
                    string expectedKey = usedBuiltInTrial ? PublicKey : ActivePublicKey;

                    s_verifiedPublicKey = null;
                    if (license.VerifySignature(expectedKey))
                    {
                        s_verifiedPublicKey = expectedKey;
                    }
                    else if (license.VerifySignature(PublicKeyV3))
                    {
                        // Issued before the key rotation; still a valid customer licence.
                        s_verifiedPublicKey = PublicKeyV3;
                    }

                    if (s_verifiedPublicKey == null)
                    {
                        s_licenseLoaded = null;
                        IsLicenseLoaded = false;
                        return false;
                    }

                    s_licenseLoaded = license;

                    // Load custom product features
                    ProductFeatures.Clear();
                    if (license.LicensedFeatures != null)
                    {
                        foreach (KeyValuePair<string, string> feature in license.LicensedFeatures.GetAll())
                        {
                            // Skip the reserved feature names we already handle specifically
                            if (feature.Key is not kProductFeature_Name_Product and
                                 not kProductFeature_Name_Version and
                                 not kProductFeature_Name_PublishDate)
                            {
                                ProductFeatures.Add(feature.Key, feature.Value);
                            }
                        }
                    }
                    if (s_licenseLoaded.Product != null && !IsNullOrEmpty(s_licenseLoaded.Product.Name))
                    {
                        LicensedType = license.ProductType;
                    }
                    else if (!IsOldProductLicense())
                    {
                        s_licenseLoaded = null;
                        IsLicenseLoaded = false;
                        return false;
                    }

                    SetProductFeature();
                    IsLicenseLoaded = true;
                    return true;
                }
                catch (FileNotFoundException)
                {
                    s_licenseLoaded = null;
                    IsLicenseLoaded = false;
                    return false;
                }
                catch (Exception)
                {
                    s_licenseLoaded = null;
                    IsLicenseLoaded = false;
                    return false;
                }
            }
        }

        /// <summary>
        /// Validate the passed license file.
        /// If the license is valid, it loads the license information into their corresponding properties.
        /// All exceptions are caught.
        /// </summary>
        /// <param name="applicationType">Theproduct the license should be used for</param>
        /// <param name="messages">Output parameter to hold messages, especially if the license is invalid.</param>
        /// <returns>True if the license is valid, otherwise false.</returns>
        private bool IsLicenseValid(ProductType? applicationType, out string messages)
        {
            messages = Empty;
            List<IValidationFailure> validationFailures = [];

            if (IsNewLicenseType)
            {
                // Required for new license type
                string identityProductCaller =
                    SecureHash.ComputeSHA256Hash(CreateProductIdentity(kProductId, s_verifiedPublicKey ?? ActivePublicKey));
                if (identityProductCaller != ProductIdentity)
                {
                    validationFailures.Add(
                        new GeneralValidationFailure
                        {
                            Message = kMessageLicenseInvalidProductIdentity,
                            HowToResolve = kMessageLicenseResolve
                        }
                    );
                }
            }

            // There may be other validation failures from earlier.
            if (validationFailures.Count > 0)
            {
                List<string> errorMessages = [];
                foreach (IValidationFailure failure in validationFailures)
                {
                    errorMessages.Add($"{failure.GetType().Name}: {failure.Message}{Environment.NewLine}{failure.HowToResolve}");
                }
                messages = Join(Environment.NewLine, errorMessages);
                return false;
            }
            if (ValidateProductLicense(applicationType))
            {
                messages = Empty;
                return true;
            }

            messages = Empty;
            return false;
        }
        #endregion Private Methods

        #region Internal Fields
        internal static bool SimulateServicePatch;
        #endregion Internal Fields

        #region Private Fields
        private static readonly Lazy<LicenseHandler> s_lazyInstance = new(() => new LicenseHandler());
        private static EvaluationTimer s_baseTimer;
        private static int s_runtimeMinutes = 5;
        private static License s_licenseLoaded;
        private static string s_verifiedPublicKey;
        private static readonly object s_lock = new();
        #endregion Private Fields
    }
}
