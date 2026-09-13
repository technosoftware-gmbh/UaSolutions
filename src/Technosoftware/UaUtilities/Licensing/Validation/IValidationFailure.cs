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
    /// Represents a failure of a <see cref="ILicenseValidator"/>.
    /// </summary>
    public interface IValidationFailure
    {
        /// <summary>
        /// Gets or sets a message that describes the validation failure.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Gets or sets a message that describes how to recover from the validation failure.
        /// </summary>
        string HowToResolve { get; set; }
    }
}
