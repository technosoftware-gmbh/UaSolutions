#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    ///     Manages the license to enable the different product versions.
    /// </summary>
    public class LicenseHandler : Technosoftware.UaUtilities.Licensing.LicenseHandler
    {
        #region Public Methods
        /// <summary>
        /// Validate the license.
        /// </summary>
        /// <param name="serialNumber">Serial Number</param>
        /// <returns>True if the license is a valid license; false otherwise</returns>
        public static bool Validate(string serialNumber)
        {
            return CheckLicense(Technosoftware.UaUtilities.Licensing.ApplicationType.Server, serialNumber);
        }
        #endregion Public Methods
    }
}
