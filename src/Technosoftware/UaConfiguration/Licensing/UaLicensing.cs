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
#endregion Using Directives

namespace Technosoftware.UaConfiguration
{
    /// <summary>
    /// Provides access to the license gate used by the libraries.
    /// </summary>
    public static class UaLicensing
    {
        /// <summary>
        /// The license gate. Defaults to <see cref="NullLicenseGate"/>; the licensed build
        /// replaces it when an <c>ApplicationInstance</c> is created.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if set to <c>null</c>.</exception>
        public static IUaLicenseGate Gate
        {
            get => s_gate;
            set => s_gate = value ?? throw new ArgumentNullException(nameof(value));
        }

        private static IUaLicenseGate s_gate = NullLicenseGate.Instance;
    }
}
