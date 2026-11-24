#region Copyright (c) 2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com
//
// The Software is based on https://github.com/junian/Standard.Licensing.
// The complete license agreement for that can be found in this directore in the LICENSE.txt file.
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// The outcome of <see cref="LicenseHandler.CheckLicense"/>: whether a licence would
    /// unlock a given product version and type, and if not, why.
    /// </summary>
    public sealed class LicenseCheckResult
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseCheckResult"/> class.
        /// </summary>
        /// <param name="productVersion">The product version the licence was checked against.</param>
        /// <param name="productType">The product type the licence was checked against.</param>
        /// <param name="license">The parsed licence, or <c>null</c> if it could not be read.</param>
        /// <param name="problems">The reasons the licence is not valid; empty if it is.</param>
        internal LicenseCheckResult(
            Version productVersion,
            ProductType? productType,
            License license,
            IReadOnlyList<string> problems)
        {
            ProductVersion = productVersion;
            ProductType = productType;
            License = license;
            Problems = problems;
        }
        #endregion Constructors, Destructor, Initialization

        #region Public Properties
        /// <summary>
        /// Whether the licence unlocks <see cref="ProductVersion"/> for <see cref="ProductType"/>.
        /// </summary>
        public bool IsValid => Problems.Count == 0;

        /// <summary>
        /// The product version the licence was checked against.
        /// </summary>
        public Version ProductVersion { get; }

        /// <summary>
        /// The product type the licence was checked against, or <c>null</c> if any type was accepted.
        /// </summary>
        public ProductType? ProductType { get; }

        /// <summary>
        /// The parsed licence, or <c>null</c> if the licence data could not be read.
        /// </summary>
        public License License { get; }

        /// <summary>
        /// The reasons the licence is not valid, one sentence each. Empty if it is valid.
        /// </summary>
        public IReadOnlyList<string> Problems { get; }
        #endregion Public Properties
    }
}
