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
	[ComImport]
    [GuidAttribute("7DE5B060-E089-11d2-A5E6-000086339399")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface CATID_OPCHDAServer10 { }

    /// <exclude />
    public enum OPCHDA_SERVERSTATUS
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_UP = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_DOWN,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_INDETERMINATE
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCHDA_BROWSEDIRECTION
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_BROWSE_UP = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_BROWSE_DOWN,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_BROWSE_DIRECT
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCHDA_BROWSETYPE
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_BRANCH = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_LEAF,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_FLAT,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_ITEMS
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCHDA_ANNOTATIONCAPABILITIES
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_READANNOTATIONCAP = 0x01,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_INSERTANNOTATIONCAP = 0x02
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCHDA_UPDATECAPABILITIES
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_INSERTCAP = 0x01,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_REPLACECAP = 0x02,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_INSERTREPLACECAP = 0x04,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_DELETERAWCAP = 0x08,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_DELETEATTIMECAP = 0x10
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCHDA_OPERATORCODES
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_EQUAL = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_LESS,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_LESSEQUAL,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_GREATER,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_GREATEREQUAL,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_NOTEQUAL
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCHDA_EDITTYPE
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_INSERT = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_REPLACE,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_INSERTREPLACE,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_DELETE
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCHDA_AGGREGATE
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_NOAGGREGATE = 0,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_INTERPOLATIVE,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_TOTAL,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_AVERAGE,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_TIMEAVERAGE,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_COUNT,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_STDEV,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_MINIMUMACTUALTIME,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_MINIMUM,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_MAXIMUMACTUALTIME,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_MAXIMUM,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_START,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_END,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_DELTA,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_REGSLOPE,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_REGCONST,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_REGDEV,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_VARIANCE,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_RANGE,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_DURATIONGOOD,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_DURATIONBAD,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_PERCENTGOOD,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_PERCENTBAD,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_WORSTQUALITY,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCHDA_ANNOTATIONS
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCHDA_ANNOTATION
    {
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hClient;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwNumValues;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr ftTimeStamps;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr szAnnotation;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr ftAnnotationTime;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr szUser;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCHDA_MODIFIEDITEM
    {
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hClient;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwCount;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pftTimeStamps;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pdwQualities;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pvDataValues;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pftModificationTime;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pEditType;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr szUser;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCHDA_ATTRIBUTE
    {
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hClient;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwNumValues;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwAttributeID;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr ftTimeStamps;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr vAttributeValues;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    };

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCHDA_TIME
    {
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int bString;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szTime;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public System.Runtime.InteropServices.ComTypes.FILETIME ftTime;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCHDA_ITEM
    {
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hClient;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int haAggregate;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwCount;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pftTimeStamps;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pdwQualities;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pvDataValues;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("1F1217B1-DEE0-11d2-A5E5-000086339399")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCHDA_Browser
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetEnum(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            OPCHDA_BROWSETYPE dwBrowseType,
            [Out]
            out Technosoftware.Rcw.IEnumString ppIEnumString);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ChangeBrowsePosition(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            OPCHDA_BROWSEDIRECTION dwBrowseDirection,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szString);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetItemID(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szNode,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string pszItemID);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetBranchPosition(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string pszBranchPos);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("1F1217B0-DEE0-11d2-A5E5-000086339399")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCHDA_Server
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetItemAttributes(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppdwAttrID,
            [Out]
            out IntPtr ppszAttrName,
            [Out]
            out IntPtr ppszAttrDesc,
            [Out]
            out IntPtr ppvtAttrDataType);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetAggregates(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppdwAggrID,
            [Out]
            out IntPtr ppszAggrName,
            [Out]
            out IntPtr ppszAggrDesc);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetHistorianStatus(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out OPCHDA_SERVERSTATUS pwStatus,
            [Out]
            out IntPtr pftCurrentTime,
            [Out]
            out IntPtr pftStartTime,
            [Out][MarshalAs(UnmanagedType.I2)]
            out short pwMajorVersion,
            [Out][MarshalAs(UnmanagedType.I2)]
            out short wMinorVersion,
            [Out][MarshalAs(UnmanagedType.I2)]
            out short pwBuildNumber,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwMaxReturnValues,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string  ppszStatusString,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string ppszVendorInfo);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetItemHandles(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszItemID,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phClient,
            [Out]
            out IntPtr pphServer,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReleaseItemHandles(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ValidateItemIDs(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszItemID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void CreateBrowse(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] pdwAttrID,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)]
            OPCHDA_OPERATORCODES[] pOperator,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)]
            object[]  vFilter,
            out IOPCHDA_Browser pphBrowser,
            [Out]
            out IntPtr ppErrors);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("1F1217B2-DEE0-11d2-A5E5-000086339399")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCHDA_SyncRead
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReadRaw(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            ref OPCHDA_TIME htStartTime,
            ref OPCHDA_TIME htEndTime,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumValues,
            [MarshalAs(UnmanagedType.I4)]
            int bBounds,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=4)]
            int[] phServer,
            [Out]
            out IntPtr ppItemValues,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReadProcessed(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            ref OPCHDA_TIME htStartTime,
            ref OPCHDA_TIME htEndTime,
            System.Runtime.InteropServices.ComTypes.FILETIME ftResampleInterval,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=3)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=3)]
            int[] haAggregate,
            [Out]
            out IntPtr ppItemValues,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReadAtTime(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumTimeStamps,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            System.Runtime.InteropServices.ComTypes.FILETIME[] ftTimeStamps,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] phServer,
            [Out]
            out IntPtr ppItemValues,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReadModified(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            ref OPCHDA_TIME htStartTime,
            ref OPCHDA_TIME htEndTime,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumValues,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=3)]
            int[] phServer,
            [Out]
            out IntPtr ppItemValues,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReadAttribute(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            ref OPCHDA_TIME htStartTime,
            ref OPCHDA_TIME htEndTime,
            [MarshalAs(UnmanagedType.I4)]
            int hServer,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumAttributes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=3)]
            int[] pdwAttributeIDs,
            [Out]
            out IntPtr ppAttributeValues,
            [Out]
            out IntPtr ppErrors);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("1F1217B3-DEE0-11d2-A5E5-000086339399")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCHDA_SyncUpdate
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QueryCapabilities(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out OPCHDA_UPDATECAPABILITIES pCapabilities);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Insert(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            System.Runtime.InteropServices.ComTypes.FILETIME[] ftTimeStamps,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)]
            object[] vDataValues,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] pdwQualities,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Replace(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            System.Runtime.InteropServices.ComTypes.FILETIME[] ftTimeStamps,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)]
            object[] vDataValues,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] pdwQualities,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void InsertReplace(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            System.Runtime.InteropServices.ComTypes.FILETIME[] ftTimeStamps,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)]
            object[] vDataValues,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] pdwQualities,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void DeleteRaw(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            ref OPCHDA_TIME htStartTime,
            ref OPCHDA_TIME htEndTime,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] phServer,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void DeleteAtTime(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            System.Runtime.InteropServices.ComTypes.FILETIME[] ftTimeStamps,
            [Out]
            out IntPtr ppErrors);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("1F1217B4-DEE0-11d2-A5E5-000086339399")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCHDA_SyncAnnotations
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QueryCapabilities(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out OPCHDA_ANNOTATIONCAPABILITIES pCapabilities);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Read(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            ref OPCHDA_TIME htStartTime,
            ref OPCHDA_TIME htEndTime,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] phServer,
            [Out]
            out IntPtr ppAnnotationValues,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Insert(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            System.Runtime.InteropServices.ComTypes.FILETIME[] ftTimeStamps,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            OPCHDA_ANNOTATION[] pAnnotationValues,
            [Out]
            out IntPtr ppErrors);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("1F1217B5-DEE0-11d2-A5E5-000086339399")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCHDA_AsyncRead
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReadRaw(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            ref OPCHDA_TIME htStartTime,
            ref OPCHDA_TIME htEndTime,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumValues,
            [MarshalAs(UnmanagedType.I4)]
            int bBounds,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=5)]
            int[] phServer,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void AdviseRaw(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            ref OPCHDA_TIME htStartTime,
            System.Runtime.InteropServices.ComTypes.FILETIME ftUpdateInterval,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=3)]
            int[] phServer,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReadProcessed(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            ref OPCHDA_TIME htStartTime,
            ref OPCHDA_TIME htEndTime,
            System.Runtime.InteropServices.ComTypes.FILETIME ftResampleInterval,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=4)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=4)]
            int[] haAggregate,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void AdviseProcessed(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            ref OPCHDA_TIME htStartTime,
            System.Runtime.InteropServices.ComTypes.FILETIME ftResampleInterval,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] haAggregate,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumIntervals,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReadAtTime(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumTimeStamps,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=1)]
            System.Runtime.InteropServices.ComTypes.FILETIME[]  ftTimeStamps,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=3)]
            int[] phServer,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReadModified(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            ref OPCHDA_TIME htStartTime,
            ref OPCHDA_TIME htEndTime,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumValues,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=4)]
            int[] phServer,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReadAttribute(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            ref OPCHDA_TIME htStartTime,
            ref OPCHDA_TIME htEndTime,
            [MarshalAs(UnmanagedType.I4)]
            int hServer,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumAttributes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=4)]
            int[] dwAttributeIDs,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Cancel(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
             [MarshalAs(UnmanagedType.I4)]
             int dwCancelID);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("1F1217B6-DEE0-11d2-A5E5-000086339399")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCHDA_AsyncUpdate
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QueryCapabilities(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            out OPCHDA_UPDATECAPABILITIES pCapabilities
        );

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Insert(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int  dwTransactionID,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=1)]
            System.Runtime.InteropServices.ComTypes.FILETIME[]  ftTimeStamps,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=1)]
            object[] vDataValues,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] pdwQualities,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Replace(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int  dwTransactionID,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=1)]
            System.Runtime.InteropServices.ComTypes.FILETIME[]  ftTimeStamps,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=1)]
            object[] vDataValues,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] pdwQualities,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void InsertReplace(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int  dwTransactionID,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=1)]
            System.Runtime.InteropServices.ComTypes.FILETIME[]  ftTimeStamps,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=1)]
            object[] vDataValues,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] pdwQualities,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void DeleteRaw(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            ref OPCHDA_TIME htStartTime,
            ref OPCHDA_TIME htEndTime,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=3)]
            int[] phServer,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void DeleteAtTime(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=1)]
            System.Runtime.InteropServices.ComTypes.FILETIME[] ftTimeStamps,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Cancel(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCancelID);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("1F1217B7-DEE0-11d2-A5E5-000086339399")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCHDA_AsyncAnnotations
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QueryCapabilities(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            out OPCHDA_ANNOTATIONCAPABILITIES pCapabilities);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Read(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            ref OPCHDA_TIME htStartTime,
            ref OPCHDA_TIME htEndTime,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=3)]
            int[] phServer,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Insert(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=1)]
            System.Runtime.InteropServices.ComTypes.FILETIME[] ftTimeStamps,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=1)]
            OPCHDA_ANNOTATION[] pAnnotationValues,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Cancel(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
             [MarshalAs(UnmanagedType.I4)]
             int dwCancelID);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("1F1217B8-DEE0-11d2-A5E5-000086339399")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCHDA_Playback
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReadRawWithUpdate(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            ref OPCHDA_TIME htStartTime,
            ref OPCHDA_TIME htEndTime,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumValues,
            System.Runtime.InteropServices.ComTypes.FILETIME ftUpdateDuration,
            System.Runtime.InteropServices.ComTypes.FILETIME ftUpdateInterval,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=6)]
            int[] phServer,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReadProcessedWithUpdate(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            ref OPCHDA_TIME htStartTime,
            ref OPCHDA_TIME htEndTime,
            System.Runtime.InteropServices.ComTypes.FILETIME ftResampleInterval,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumIntervals,
            System.Runtime.InteropServices.ComTypes.FILETIME ftUpdateInterval,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=6)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=6)]
            int[] haAggregate,
            [Out]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Cancel(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCancelID);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("1F1217B9-DEE0-11d2-A5E5-000086339399")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCHDA_DataCallback
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void OnDataChange(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [MarshalAs(UnmanagedType.I4)]
            int hrStatus,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=2)]
            OPCHDA_ITEM[] pItemValues,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] phrErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void OnReadComplete(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [MarshalAs(UnmanagedType.I4)]
            int hrStatus,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=2)]
            OPCHDA_ITEM[] pItemValues,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] phrErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void OnReadModifiedComplete(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [MarshalAs(UnmanagedType.I4)]
            int hrStatus,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=2)]
            OPCHDA_MODIFIEDITEM[] pItemValues,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] phrErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void OnReadAttributeComplete(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [MarshalAs(UnmanagedType.I4)]
            int hrStatus,
            [MarshalAs(UnmanagedType.I4)]
            int hClient,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=3)]
            OPCHDA_ATTRIBUTE[] pAttributeValues,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=3)]
            int[] phrErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void OnReadAnnotations(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [MarshalAs(UnmanagedType.I4)]
            int hrStatus,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=2)]
            OPCHDA_ANNOTATION[] pAnnotationValues,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] phrErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void OnInsertAnnotations(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [MarshalAs(UnmanagedType.I4)]
            int hrStatus,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] phClients,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] phrErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void OnPlayback(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [MarshalAs(UnmanagedType.I4)]
            int hrStatus,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumItems,
            IntPtr ppItemValues,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] phrErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void OnUpdateComplete(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [MarshalAs(UnmanagedType.I4)]
            int hrStatus,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] phClients,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] phrErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void OnCancelComplete(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCancelID);
    }

    /// <exclude />
	public static partial class Constants
    {
        // category description.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_CATEGORY_DESCRIPTION_HDA10 = "OPC History Data Access Servers Version 1.0";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // attribute ids.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_DATA_TYPE = 0x01;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_DESCRIPTION = 0x02;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_ENG_UNITS = 0x03;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_STEPPED = 0x04;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_ARCHIVING = 0x05;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_DERIVE_EQUATION = 0x06;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_NODE_NAME = 0x07;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_PROCESS_NAME = 0x08;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_SOURCE_NAME = 0x09;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_SOURCE_TYPE = 0x0a;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_NORMAL_MAXIMUM = 0x0b;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_NORMAL_MINIMUM = 0x0c;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_ITEMID = 0x0d;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_MAX_TIME_INT = 0x0e;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_MIN_TIME_INT = 0x0f;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_EXCEPTION_DEV = 0x10;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_EXCEPTION_DEV_TYPE = 0x11;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_HIGH_ENTRY_LIMIT = 0x12;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_LOW_ENTRY_LIMIT = 0x13;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // attribute names.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_DATA_TYPE = "Data Type";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_DESCRIPTION = "Description";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_ENG_UNITS = "Eng Units";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_STEPPED = "Stepped";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_ARCHIVING = "Archiving";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_DERIVE_EQUATION = "Derive Equation";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_NODE_NAME = "Node Name";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_PROCESS_NAME = "Process Name";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_SOURCE_NAME = "Source Name";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_SOURCE_TYPE = "Source Type";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_NORMAL_MAXIMUM = "Normal Maximum";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_NORMAL_MINIMUM = "Normal Minimum";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_ITEMID = "ItemID";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_MAX_TIME_INT = "Max Time Interval";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_MIN_TIME_INT = "Min Time Interval";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_EXCEPTION_DEV = "Exception Deviation";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_EXCEPTION_DEV_TYPE = "Exception Dev Type";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_HIGH_ENTRY_LIMIT = "High Entry Limit";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_ATTRNAME_LOW_ENTRY_LIMIT = "Low Entry Limit";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // aggregate names.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_INTERPOLATIVE = "Interpolative";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_TOTAL = "Total";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_AVERAGE = "Average";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_TIMEAVERAGE = "Time Average";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_COUNT = "Count";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_STDEV = "Standard Deviation";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_MINIMUMACTUALTIME = "Minimum Actual Time";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_MINIMUM = "Minimum";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_MAXIMUMACTUALTIME = "Maximum Actual Time";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_MAXIMUM = "Maximum";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_START = "Start";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_END = "End";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_DELTA = "Delta";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_REGSLOPE = "Regression Line Slope";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_REGCONST = "Regression Line Constant";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_REGDEV = "Regression Line Error";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_VARIANCE = "Variance";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_RANGE = "Range";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_DURATIONGOOD = "Duration Good";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_DURATIONBAD = "Duration Bad";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_PERCENTGOOD = "Percent Good";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_PERCENTBAD = "Percent Bad";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_WORSTQUALITY = "Worst Quality";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPCHDA_AGGRNAME_ANNOTATIONS = "Annotations";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // OPCHDA_QUALITY -- these are the high-order 16 bits, OPC DA Quality occupies low-order 16 bits.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_EXTRADATA = 0x00010000;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_INTERPOLATED = 0x00020000;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_RAW = 0x00040000;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_CALCULATED = 0x00080000;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_NOBOUND = 0x00100000;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_NODATA = 0x00200000;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_DATALOST = 0x00400000;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_CONVERSION = 0x00800000;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPCHDA_PARTIAL = 0x01000000;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }
}
