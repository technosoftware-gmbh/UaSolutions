#region Copyright (c) 2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com
//
// The Software is subject to the Technosoftware GmbH MIT License, which can
// be found here:
// https://technosoftware.com/license/mit/
//
// The Software is based on the OPC Foundation UA Stack and the OPC Foundation
// MIT License. The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.IO;
#endregion Using Directives

namespace Technosoftware.UaConfiguration
{
    /// <summary>
    /// The seam between the libraries and the licensing implementation.
    /// </summary>
    /// <remarks>
    /// The libraries call this interface unconditionally. In a build without licensing
    /// the implementation is <see cref="NullLicenseGate"/> and every member is a no-op;
    /// in the licensed build <c>ApplicationInstance</c> installs an implementation backed
    /// by the licensing library. This is what keeps conditional compilation out of the
    /// call sites.
    /// </remarks>
    public interface IUaLicenseGate
    {
        /// <summary>
        /// Applies license data supplied by the application.
        /// </summary>
        /// <param name="licenseData">The license data, or an empty string to run unlicensed.</param>
        /// <returns>
        /// <c>true</c> if the build requires no license, or if a valid license was applied;
        /// otherwise <c>false</c>.
        /// </returns>
        bool TryApplyLicense(string licenseData);

        /// <summary>
        /// Throws if the given feature is not covered by the applied license.
        /// </summary>
        /// <param name="product">The product the license is required for.</param>
        /// <param name="feature">The feature the license must grant.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown if the feature is not licensed, or the evaluation period has expired.
        /// </exception>
        void RequireFeature(UaProduct product, UaFeature feature);

        /// <summary>
        /// Writes license and support information to the given writer.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        void WriteLicenseInfo(TextWriter writer);
    }
}
