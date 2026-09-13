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
    /// Interface for the fluent validation syntax.
    /// </summary>
    public interface IValidationChainCondition : IFluentInterface
    {
        /// <summary>
        /// Adds a when predicate to the current validator.
        /// </summary>
        /// <param name="predicate">The predicate that defines the conditions.</param>
        /// <returns>An instance of <see cref="ICompleteValidationChain"/>.</returns>
        ICompleteValidationChain When(Predicate<License> predicate);
    }
}
