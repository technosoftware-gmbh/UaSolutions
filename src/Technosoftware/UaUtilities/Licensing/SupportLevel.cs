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
    /// Defines the type of the support contract.
    /// </summary>
    public enum SupportLevel
    {
        /// <summary>
        /// No support included.
        /// Included:
        /// - Questions, Change Requests and Issues can be submitted free of charge at https://github.com/technosoftware-gmbh/UaSolutions/issues.
        /// </summary>
        None = 0,

        /// <summary>
        /// Support included.
        /// - Questions, Change Requests and Issues can be submitted free of charge at https://github.com/technosoftware-gmbh/UaSolutions/issues.
        /// - Also included is technical support via direct Email contact or remote sessions.
        /// - Access to service patches.
        /// </summary>
        Standard = 1
    }
}
