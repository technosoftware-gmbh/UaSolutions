#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on https://github.com/junian/Standard.Licensing. 
// The complete license agreement for that can be found in this directore in the LICENSE.txt file.
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// Represents a <see cref="License"/> expired failure of a <see cref="ILicenseValidator"/>.
    /// </summary>
    public class LicenseExpiredValidationFailure : IValidationFailure
    {
        /// <summary>
        /// Gets or sets a message that describes the validation failure.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a message that describes how to recover from the validation failure.
        /// </summary>
        public string HowToResolve { get; set; }
    }
}
