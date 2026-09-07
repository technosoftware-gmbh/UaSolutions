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
    /// Defines the type of a <see cref="License"/>
    /// </summary>
    public enum LicenseType
    {
        /// <summary>
        /// No valid license
        /// </summary>
        None = 0,

        /// <summary>
        /// For evaluation use only
        /// Included:
        /// - Can be used with the version it was issued for, e.g. 4.1.0.
        /// - Expires after 30 days.
        /// - Runtime limited to 2 hours per session.
        /// </summary>
        Trial = 1,

        /// <summary>
        /// Single Developer License
        /// Included:
        /// - Any application developed with the solutions can be delivered to an unlimited number of customers (no royalties)
        /// - Can be used by one developers, see <see cref="LicenseHandler.Name"/> and <see cref="LicenseHandler.Domain"/>
        /// - Free minor updates within the same major version (e.g. 4.x.0)
        /// </summary>
        SingleDeveloper = 2,

        /// <summary>
        /// Company Site License
        /// Included:
        /// - Any application developed with the solutions can be delivered to an unlimited number of customers (no royalties)
        /// - Software may be used by multiple developers on a single site of the Licensee’s organization.
        /// - Free minor updates within the same major version (e.g. 4.x.0)
        /// </summary>
        CompanySite = 3,

        /// <summary>
        /// Company-Wide Global License
        /// Included:
        /// - Any application developed with the solutions can be delivered to an unlimited number of customers (no royalties)
        /// - Software may be used by multiple developers of the Licensee’s organization (legal subject), see <see cref="LicenseHandler.Company"/> and <see cref="LicenseHandler.Domain"/>
        /// - Free minor updates within the same major version (e.g. 4.x.0)
        /// </summary>
        CompanyWideGlobal = 4
    }
}
