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
    /// Interface for the fluent validation syntax.
    /// </summary>
    public interface IAddAdditionalValidationChain : IFluentInterface
    {
        /// <summary>
        /// Adds an additional validation chain.
        /// </summary>
        /// <returns>An instance of <see cref="IStartValidationChain"/>.</returns>
        IStartValidationChain And();
    }
}
