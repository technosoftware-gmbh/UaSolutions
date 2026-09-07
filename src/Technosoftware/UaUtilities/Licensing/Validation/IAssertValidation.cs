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
using System.Collections.Generic;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// Interface for the fluent validation syntax.
    /// </summary>
    public interface IAssertValidation : IFluentInterface
    {
        /// <summary>
        /// Invokes the m_license assertion.
        /// </summary>
        /// <returns>An array is <see cref="IValidationFailure"/> when the validation fails.</returns>
        IEnumerable<IValidationFailure> AssertValidLicense();
    }
}
