#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
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
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved


#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

#endregion Using Directives

namespace Technosoftware.Rcw
{
    /// <exclude />
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct CONNECTDATA
    {
        [MarshalAs(UnmanagedType.IUnknown)]
        object pUnk;
        [MarshalAs(UnmanagedType.I4)]
        int dwCookie;
    }

    /// <exclude />
    [ComImport]
    [GuidAttribute("B196B287-BAB4-101A-B69C-00AA00341D07")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumConnections
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void RemoteNext(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int cConnections,
            [Out]
            IntPtr rgcd,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pcFetched);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Skip(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int cConnections);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Reset();
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Clone(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out IEnumConnections ppEnum);
    }

    /// <exclude />
    [ComImport]
    [GuidAttribute("B196B286-BAB4-101A-B69C-00AA00341D07")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IConnectionPoint
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetConnectionInterface(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out Guid pIID);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetConnectionPointContainer(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out IConnectionPointContainer ppCPC);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Advise(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.IUnknown)]
            object pUnkSink,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCookie);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Unadvise(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCookie);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void EnumConnections(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out IEnumConnections ppEnum);
    }

    /// <exclude />
    [ComImport]
    [GuidAttribute("B196B285-BAB4-101A-B69C-00AA00341D07")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumConnectionPoints
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void RemoteNext(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int cConnections,
            [Out]
            IntPtr ppCP,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pcFetched);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Skip(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int cConnections);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Reset();
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Clone(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out IEnumConnectionPoints ppEnum);
    }

    /// <exclude />
    [ComImport]
    [GuidAttribute("B196B284-BAB4-101A-B69C-00AA00341D07")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IConnectionPointContainer
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void EnumConnectionPoints(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out IEnumConnectionPoints ppEnum);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void FindConnectionPoint(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            ref Guid riid,
            [Out]
            out IConnectionPoint ppCP);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("F31DFDE1-07B6-11d2-B2D8-0060083BA1FB")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCShutdown
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ShutdownRequest(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szReason);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("F31DFDE2-07B6-11d2-B2D8-0060083BA1FB")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCCommon
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetLocaleID(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwLcid);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetLocaleID(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwLcid);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QueryAvailableLocaleIDs(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr pdwLcid);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetErrorString(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwError,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out String ppString);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetClientName(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            String szName);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("13486D50-4821-11D2-A494-3CB306C10000")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCServerList
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void EnumClassesOfCategories(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int cImplemented,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            Guid[] rgcatidImpl,
            [MarshalAs(UnmanagedType.I4)]
            int cRequired,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=2)]
            Guid[] rgcatidReq,
            [Out][MarshalAs(UnmanagedType.IUnknown)]
            out object ppenumClsid);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetClassDetails(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            ref Guid clsid,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string ppszProgID,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string ppszUserType);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void CLSIDFromProgID(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szProgId,
            [Out]
            out Guid clsid);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("55C382C8-21C7-4e88-96C1-BECFB1E3F483")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCEnumGUID
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Next(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int celt,
            [Out]
            IntPtr rgelt,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pceltFetched);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Skip(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int celt);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Reset();
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Clone(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out IOPCEnumGUID ppenum);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("0002E000-0000-0000-C000-000000000046")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumGUID
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Next(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int celt,
            [Out]
            IntPtr rgelt,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pceltFetched);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Skip(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int celt);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Reset();
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Clone(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out IEnumGUID ppenum);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("00000100-0000-0000-C000-000000000046")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumUnknown
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void RemoteNext(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int celt,
            [Out]
            IntPtr rgelt,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pceltFetched);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Skip(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int celt);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Reset();
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Clone(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out IEnumUnknown ppenum);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("00000101-0000-0000-C000-000000000046")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumString
    {
        [PreserveSig]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        int RemoteNext(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int celt,
            IntPtr rgelt,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pceltFetched);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Skip(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int celt);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Reset();
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Clone(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out IEnumString ppenum);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("9DD0B56C-AD9E-43ee-8305-487F3188BF7A")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCServerList2
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void EnumClassesOfCategories(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int cImplemented,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            Guid[] rgcatidImpl,
            [MarshalAs(UnmanagedType.I4)]
            int cRequired,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            Guid[] rgcatidReq,
            [Out]
            out IOPCEnumGUID ppenumClsid);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetClassDetails(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            ref Guid clsid,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string ppszProgID,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string ppszUserType,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string ppszVerIndProgID);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void CLSIDFromProgID(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szProgId,
            [Out]
            out Guid clsid);
    }
}
