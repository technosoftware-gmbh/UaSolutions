#region Copyright (c) 2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com
//
// The Software is subject to the Technosoftware GmbH MIT License, which can
// be found here:
// https://technosoftware.com/license/mit/
//
// The Software is based on the OPC Foundation UA Stack and the OPC Foundation
// MIT License. The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System.Threading.Tasks;
#endregion Using Directives

namespace Technosoftware.UaConfiguration
{
    /// <summary>
    /// Generic Message Dialog for issues during loading of an application
    /// </summary>
    public abstract class IUaApplicationMessageDlg
    {
        /// <summary>
        /// Defines the message to show and if the user is asked for acceptance or not.
        /// </summary>
        /// <param name="text">The text of the message.</param>
        /// <param name="ask">If the application should ask the user.</param>
        public abstract void Message(string text, bool ask = false);

        /// <summary>
        /// Asynchronous version of showing the message
        /// </summary>
        /// <returns>True if user answered yes; otherwise false.</returns>
        public abstract Task<bool> ShowAsync();
    }
}
