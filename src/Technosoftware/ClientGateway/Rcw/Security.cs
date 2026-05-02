#region Copyright (c) 2011-2017 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2017 Technosoftware GmbH. All rights reserved
// Web: http://www.technosoftware.com
//
// The Software is based on the OPC Foundation’s software and is subject to 
// the OPC Foundation MIT License 1.00, which can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//
// The Software is subject to the Technosoftware GmbH Software License Agreement,
// which can be found here:
// https://technosoftware.com/license-agreement/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2017 Technosoftware GmbH. All rights reserved


#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

#endregion Using Directives

namespace Technosoftware.Rcw
{
    /// <exclude />
    [ComImport]
    [GuidAttribute("7AA83A01-6C77-11d3-84F9-00008630A38B")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCSecurityNT
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void IsAvailableNT(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pbAvailable);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QueryMinImpersonationLevel(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwMinImpLevel);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ChangeUser();
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    };

    /// <exclude />
	[ComImport]
    [GuidAttribute("7AA83A02-6C77-11d3-84F9-00008630A38B")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCSecurityPrivate
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void IsAvailablePriv(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pbAvailable);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Logon(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szUserID,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szPassword);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Logoff();
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    };
}
