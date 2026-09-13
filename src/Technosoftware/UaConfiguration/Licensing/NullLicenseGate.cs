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
    /// The license gate used by builds without licensing: nothing is required and
    /// nothing is reported.
    /// </summary>
    public sealed class NullLicenseGate : IUaLicenseGate
    {
        /// <summary>
        /// The single instance.
        /// </summary>
        public static NullLicenseGate Instance { get; } = new NullLicenseGate();

        private NullLicenseGate()
        {
        }

        /// <inheritdoc/>
        public bool TryApplyLicense(string licenseData)
        {
            return true;
        }

        /// <inheritdoc/>
        public void RequireFeature(UaProduct product, UaFeature feature)
        {
            // No license is required in this build.
        }

        /// <inheritdoc/>
        public void WriteLicenseInfo(TextWriter writer)
        {
            // Nothing to report in this build.
        }
    }
}
