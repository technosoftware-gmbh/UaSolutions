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
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// Represents a <see cref="License"/> validator.
    /// </summary>
    public interface ILicenseValidator
    {
        /// <summary>
        /// Gets or sets the predicate to determine if the <see cref="License"/>
        /// is valid.
        /// </summary>
        Predicate<License> Validate { get; set; }

        /// <summary>
        /// Gets or sets the predicate to determine if the <see cref="ILicenseValidator"/>
        /// should be executed.
        /// </summary>
        Predicate<License> ValidateWhen { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IValidationFailure"/> result. The <see cref="IValidationFailure"/>
        /// will be returned to the application when the <see cref="ILicenseValidator"/> fails.
        /// </summary>
        IValidationFailure FailureResult { get; set; }
    }
}
