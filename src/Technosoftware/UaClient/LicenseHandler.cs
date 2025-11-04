#region Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is subject to the Technosoftware GmbH Software License 
// Agreement, which can be found here:
// https://technosoftware.com/documents/Source_License_Agreement.pdf
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using Opc.Ua;

using Technosoftware.UaUtilities.Licensing;
#endregion

namespace Technosoftware.UaClient
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
        public static bool Validate(string serialNumber)
        {
            return CheckLicense(Technosoftware.UaUtilities.Licensing.ApplicationType.Client, serialNumber);
        }
        #endregion
    }
}
