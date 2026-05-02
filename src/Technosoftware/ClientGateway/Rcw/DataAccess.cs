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
    [GuidAttribute("63D5F430-CFE4-11d1-B2C8-0060083BA1FB")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface CATID_OPCDAServer10 { }

    /// <exclude />
	[ComImport]
    [GuidAttribute("63D5F432-CFE4-11d1-B2C8-0060083BA1FB")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface CATID_OPCDAServer20 { }

    /// <exclude />
	[ComImport]
    [GuidAttribute("CC603642-66D7-48f1-B69A-B625E73652D7")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface CATID_OPCDAServer30 { }

    /// <exclude />
	[ComImport]
    [GuidAttribute("3098EDA4-A006-48b2-A27F-247453959408")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface CATID_XMLDAServer10 { }

    /// <exclude />
    public enum OPCDATASOURCE
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_DS_CACHE = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_DS_DEVICE
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCBROWSETYPE
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_BRANCH = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_LEAF,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_FLAT
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCNAMESPACETYPE
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_NS_HIERARCHIAL = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_NS_FLAT
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCBROWSEDIRECTION
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_BROWSE_UP = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_BROWSE_DOWN,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_BROWSE_TO
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCEUTYPE
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_NOENUM = 0,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_ANALOG,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_ENUMERATED
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCSERVERSTATE
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_STATUS_RUNNING = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_STATUS_FAILED,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_STATUS_NOCONFIG,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_STATUS_SUSPENDED,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_STATUS_TEST,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_STATUS_COMM_FAULT
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCENUMSCOPE
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_ENUM_PRIVATE_CONNECTIONS = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_ENUM_PUBLIC_CONNECTIONS,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_ENUM_ALL_CONNECTIONS,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_ENUM_PRIVATE,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_ENUM_PUBLIC,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_ENUM_ALL
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCGROUPHEADER
    {
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwSize;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwItemCount;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hClientGroup;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwTransactionID;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hrStatus;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCITEMHEADER1
    {
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hClient;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwValueOffset;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wQuality;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wReserved;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public System.Runtime.InteropServices.ComTypes.FILETIME ftTimeStampItem;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCITEMHEADER2
    {
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hClient;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwValueOffset;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wQuality;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wReserved;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCGROUPHEADERWRITE
    {
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwItemCount;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hClientGroup;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwTransactionID;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hrStatus;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCITEMHEADERWRITE
    {
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hClient;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwError;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCITEMSTATE
    {
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hClient;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public System.Runtime.InteropServices.ComTypes.FILETIME ftTimeStamp;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wQuality;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wReserved;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.Struct)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public object vDataValue;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCSERVERSTATUS
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public System.Runtime.InteropServices.ComTypes.FILETIME ftStartTime;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public System.Runtime.InteropServices.ComTypes.FILETIME ftCurrentTime;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public System.Runtime.InteropServices.ComTypes.FILETIME ftLastUpdateTime;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public OPCSERVERSTATE dwServerState;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwGroupCount;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwBandWidth;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wMajorVersion;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wMinorVersion;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wBuildNumber;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wReserved;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szVendorInfo;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCITEMDEF
    {
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szAccessPath;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szItemID;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int bActive;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hClient;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwBlobSize;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pBlob;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short vtRequestedDataType;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wReserved;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    };

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCITEMATTRIBUTES
    {
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szAccessPath;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szItemID;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int bActive;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hClient;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hServer;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwAccessRights;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwBlobSize;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pBlob;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short vtRequestedDataType;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short vtCanonicalDataType;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public OPCEUTYPE dwEUType;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.Struct)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public object vEUInfo;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCITEMRESULT
    {
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hServer;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short vtCanonicalDataType;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wReserved;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwAccessRights;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwBlobSize;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pBlob;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCITEMPROPERTY
    {
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short vtDataType;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wReserved;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwPropertyID;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szItemID;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szDescription;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.Struct)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public object vValue;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hrErrorID;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwReserved;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCITEMPROPERTIES
    {
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int hrErrorID;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwNumProperties;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pItemProperties;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwReserved;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCBROWSEELEMENT
    {
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szName;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szItemID;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwFlagValue;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwReserved;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public OPCITEMPROPERTIES ItemProperties;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCITEMVQT
    {
        [MarshalAs(UnmanagedType.Struct)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public object vDataValue;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int bQualitySpecified;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wQuality;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wReserved;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int bTimeStampSpecified;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwReserved;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public System.Runtime.InteropServices.ComTypes.FILETIME ftTimeStamp;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCBROWSEFILTER
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_BROWSE_FILTER_ALL = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_BROWSE_FILTER_BRANCHES,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_BROWSE_FILTER_ITEMS,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("39c13a4d-011e-11d0-9675-0020afd8adb3")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCServer
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void AddGroup(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szName,
            [MarshalAs(UnmanagedType.I4)]
            int bActive,
            [MarshalAs(UnmanagedType.I4)]
            int dwRequestedUpdateRate,
            [MarshalAs(UnmanagedType.I4)]
            int hClientGroup,
            IntPtr pTimeBias,
            IntPtr pPercentDeadband,
            [MarshalAs(UnmanagedType.I4)]
            int dwLCID,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int phServerGroup,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pRevisedUpdateRate,
            ref Guid riid,
            [Out][MarshalAs(UnmanagedType.IUnknown, IidParameterIndex=9)]
            out object ppUnk);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetErrorString(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwError,
            [MarshalAs(UnmanagedType.I4)]
            int dwLocale,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string ppString);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetGroupByName(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szName,
            ref Guid riid,
            [Out][MarshalAs(UnmanagedType.IUnknown, IidParameterIndex=1)]
            out object ppUnk);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetStatus(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out IntPtr ppServerStatus);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void RemoveGroup(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int hServerGroup,
            [MarshalAs(UnmanagedType.I4)]
            int bForce);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void CreateGroupEnumerator(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            OPCENUMSCOPE dwScope,
            ref Guid riid,
            [Out][MarshalAs(UnmanagedType.IUnknown, IidParameterIndex=1)]
            out object ppUnk);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("39c13a4e-011e-11d0-9675-0020afd8adb3")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCServerPublicGroups
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetPublicGroupByName(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szName,
            ref Guid riid,
            [Out][MarshalAs(UnmanagedType.IUnknown, IidParameterIndex=1)]
            out object ppUnk);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void RemovePublicGroup(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int hServerGroup,
            [MarshalAs(UnmanagedType.I4)]
            int bForce);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("39c13a4f-011e-11d0-9675-0020afd8adb3")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCBrowseServerAddressSpace
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QueryOrganization(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out]
            out OPCNAMESPACETYPE pNameSpaceType);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ChangeBrowsePosition(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            OPCBROWSEDIRECTION dwBrowseDirection,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szString);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void BrowseOPCItemIDs(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            OPCBROWSETYPE dwBrowseFilterType,
            [MarshalAs(UnmanagedType.LPWStr)]
            string  szFilterCriteria,
            [MarshalAs(UnmanagedType.I2)]
            short vtDataTypeFilter,
            [MarshalAs(UnmanagedType.I4)]
            int dwAccessRightsFilter,
            [Out]
            out Technosoftware.Rcw.IEnumString ppIEnumString);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetItemID(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szItemDataID,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string szItemID);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void BrowseAccessPaths(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szItemID,
            [Out]
            out Technosoftware.Rcw.IEnumString pIEnumString);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("39c13a50-011e-11d0-9675-0020afd8adb3")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCGroupStateMgt
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetState(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pUpdateRate,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pActive,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string ppName,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pTimeBias,
            [Out][MarshalAs(UnmanagedType.R4)]
            out float pPercentDeadband,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pLCID,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int phClientGroup,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int phServerGroup);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetState(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            IntPtr pRequestedUpdateRate,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pRevisedUpdateRate,
            IntPtr pActive,
            IntPtr pTimeBias,
            IntPtr pPercentDeadband,
            IntPtr pLCID,
            IntPtr phClientGroup);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetName(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szName);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void CloneGroup(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szName,
            ref Guid riid,
            [Out][MarshalAs(UnmanagedType.IUnknown, IidParameterIndex=1)]
            out object ppUnk);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("39c13a51-011e-11d0-9675-0020afd8adb3")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCPublicGroupStateMgt
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetState(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pPublic);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void MoveToPublic();
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("39c13a52-011e-11d0-9675-0020afd8adb3")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCSyncIO
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Read(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            OPCDATASOURCE dwSource,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] phServer,
            [Out]
            out IntPtr ppItemValues,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Write(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)]
            object[] pItemValues,
            [Out]
            out IntPtr ppErrors);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("39c13a53-011e-11d0-9675-0020afd8adb3")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCAsyncIO
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Read(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwConnection,
            OPCDATASOURCE dwSource,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] phServer,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pTransactionID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Write(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwConnection,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=1)]
            object[] pItemValues,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pTransactionID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Refresh(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwConnection,
            OPCDATASOURCE dwSource,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pTransactionID);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Cancel(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("39c13a54-011e-11d0-9675-0020afd8adb3")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCItemMgt
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void AddItems(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            OPCITEMDEF[] pItemArray,
            [Out]
            out IntPtr ppAddResults,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ValidateItems(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            OPCITEMDEF[] pItemArray,
            [MarshalAs(UnmanagedType.I4)]
            int bBlobUpdate,
            [Out]
            out IntPtr ppValidationResults,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void RemoveItems(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetActiveState(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.I4)]
            int  bActive,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetClientHandles(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phClient,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetDatatypes(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I2, SizeParamIndex=0)]
            short[] pRequestedDatatypes,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void CreateEnumerator(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            ref Guid riid,
            [Out][MarshalAs(UnmanagedType.IUnknown, IidParameterIndex=0)]
            out object ppUnk);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("39c13a55-011e-11d0-9675-0020afd8adb3")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumOPCItemAttributes
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Next(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int celt,
            [Out]
            out IntPtr ppItemArray,
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
            out IEnumOPCItemAttributes ppEnumItemAttributes);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("39c13a70-011e-11d0-9675-0020afd8adb3")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCDataCallback
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void OnDataChange(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransid,
            [MarshalAs(UnmanagedType.I4)]
            int hGroup,
            [MarshalAs(UnmanagedType.I4)]
            int hrMasterquality,
            [MarshalAs(UnmanagedType.I4)]
            int hrMastererror,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=4)]
            int[] phClientItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=4)]
            object[] pvValues,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I2, SizeParamIndex=4)]
            short[] pwQualities,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=4)]
            System.Runtime.InteropServices.ComTypes.FILETIME[] pftTimeStamps,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=4)]
            int[] pErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void OnReadComplete(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransid,
            [MarshalAs(UnmanagedType.I4)]
            int hGroup,
            [MarshalAs(UnmanagedType.I4)]
            int hrMasterquality,
            [MarshalAs(UnmanagedType.I4)]
            int hrMastererror,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=4)]
            int[] phClientItems,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=4)]
            object[] pvValues,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I2, SizeParamIndex=4)]
            short[] pwQualities,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=4)]
            System.Runtime.InteropServices.ComTypes.FILETIME[] pftTimeStamps,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=4)]
            int[] pErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void OnWriteComplete(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransid,
            [MarshalAs(UnmanagedType.I4)]
            int hGroup,
            [MarshalAs(UnmanagedType.I4)]
            int hrMastererr,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=3)]
            int[] pClienthandles,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=3)]
            int[] pErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void OnCancelComplete(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwTransid,
            [MarshalAs(UnmanagedType.I4)]
            int hGroup);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("39c13a71-011e-11d0-9675-0020afd8adb3")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCAsyncIO2
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Read(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Write(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)]
            object[] pItemValues,
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Refresh2(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            OPCDATASOURCE dwSource,
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCancelID);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Cancel2(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCancelID);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetEnable(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int bEnable);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetEnable(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pbEnable);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("39c13a72-011e-11d0-9675-0020afd8adb3")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCItemProperties
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QueryAvailableProperties(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szItemID,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppPropertyIDs,
            [Out]
            out IntPtr ppDescriptions,
            [Out]
            out IntPtr ppvtDataTypes);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetItemProperties(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szItemID,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] pdwPropertyIDs,
            [Out]
            out IntPtr ppvData,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void LookupItemIDs(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szItemID,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] pdwPropertyIDs,
            [Out]
            out IntPtr ppszNewItemIDs,
            [Out]
            out IntPtr ppErrors);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("5946DA93-8B39-4ec8-AB3D-AA73DF5BC86F")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCItemDeadbandMgt
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetItemDeadband(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.R4, SizeParamIndex=0)]
            float[] pPercentDeadband,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetItemDeadband(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [Out]
            out IntPtr ppPercentDeadband,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ClearItemDeadband(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [Out]
            out IntPtr ppErrors);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("3E22D313-F08B-41a5-86C8-95E95CB49FFC")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCItemSamplingMgt
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetItemSamplingRate(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] pdwRequestedSamplingRate,
            [Out]
            out IntPtr ppdwRevisedSamplingRate,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetItemSamplingRate(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [Out]
            out IntPtr ppdwSamplingRate,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ClearItemSamplingRate(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetItemBufferEnable(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] pbEnable,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetItemBufferEnable(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [Out]
            out IntPtr ppbEnable,
            [Out]
            out IntPtr ppErrors);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("39227004-A18F-4b57-8B0A-5235670F4468")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCBrowse
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetProperties(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwItemCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszItemIDs,
            [MarshalAs(UnmanagedType.I4)]
            int bReturnPropertyValues,
            [MarshalAs(UnmanagedType.I4)]
            int dwPropertyCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=3)]
            int[] dwPropertyIDs,
            [Out]
            out IntPtr ppItemProperties);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Browse(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szItemID,
            ref IntPtr pszContinuationPoint,
            [MarshalAs(UnmanagedType.I4)]
            int dwMaxElementsReturned,
            OPCBROWSEFILTER dwBrowseFilter,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szElementNameFilter,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szVendorFilter,
            [MarshalAs(UnmanagedType.I4)]
            int bReturnAllProperties,
            [MarshalAs(UnmanagedType.I4)]
            int bReturnPropertyValues,
            [MarshalAs(UnmanagedType.I4)]
            int dwPropertyCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=8)]
            int[] pdwPropertyIDs,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pbMoreElements,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppBrowseElements);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("85C0B427-2893-4cbc-BD78-E5FC5146F08F")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCItemIO
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Read(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszItemIDs,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] pdwMaxAge,
            [Out]
            out IntPtr ppvValues,
            [Out]
            out IntPtr ppwQualities,
            [Out]
            out IntPtr ppftTimeStamps,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void WriteVQT(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszItemIDs,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            OPCITEMVQT[] pItemVQT,
            [Out]
            out IntPtr ppErrors);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("730F5F0F-55B1-4c81-9E18-FF8A0904E1FA")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCSyncIO2 // : IOPCSyncIO
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Read(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            OPCDATASOURCE dwSource,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] phServer,
            [Out]
            out IntPtr ppItemValues,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Write(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)]
            object[] pItemValues,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReadMaxAge(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] pdwMaxAge,
            [Out]
            out IntPtr ppvValues,
            [Out]
            out IntPtr ppwQualities,
            [Out]
            out IntPtr ppftTimeStamps,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void WriteVQT(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            OPCITEMVQT[] pItemVQT,
            [Out]
            out IntPtr ppErrors);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("0967B97B-36EF-423e-B6F8-6BFF1E40D39D")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCAsyncIO3 // : IOPCAsyncIO2
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Read(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Write(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct, SizeParamIndex=0)]
            object[] pItemValues,
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Refresh2(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            OPCDATASOURCE dwSource,
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCancelID);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Cancel2(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCancelID);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetEnable(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int bEnable);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetEnable(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pbEnable);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ReadMaxAge(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] pdwMaxAge,
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [Out]
            [MarshalAs(UnmanagedType.I4)]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void WriteVQT(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] phServer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            OPCITEMVQT[] pItemVQT,
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [Out]
            [MarshalAs(UnmanagedType.I4)]
            out int pdwCancelID,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void RefreshMaxAge(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwMaxAge,
            [MarshalAs(UnmanagedType.I4)]
            int dwTransactionID,
            [Out]
            [MarshalAs(UnmanagedType.I4)]
            out int pdwCancelID);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("8E368666-D72E-4f78-87ED-647611C61C9F")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCGroupStateMgt2 // : IOPCGroupStateMgt
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetState(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pUpdateRate,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pActive,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string ppName,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pTimeBias,
            [Out][MarshalAs(UnmanagedType.R4)]
            out float pPercentDeadband,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pLCID,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int phClientGroup,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int phServerGroup);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetState(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            IntPtr pRequestedUpdateRate,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pRevisedUpdateRate,
            IntPtr pActive,
            IntPtr pTimeBias,
            IntPtr pPercentDeadband,
            IntPtr pLCID,
            IntPtr phClientGroup);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetName(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szName);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void CloneGroup(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szName,
            ref Guid riid,
            [Out][MarshalAs(UnmanagedType.IUnknown, IidParameterIndex=1)]
            out object ppUnk);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetKeepAlive(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwKeepAliveTime,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwRevisedKeepAliveTime);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetKeepAlive(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwKeepAliveTime);
    }

    /// <exclude />
	public static partial class Constants
    {
        // category description strings.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_CATEGORY_DESCRIPTION_DA10 = "OPC Data Access Servers Version 1.0";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_CATEGORY_DESCRIPTION_DA20 = "OPC Data Access Servers Version 2.0";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_CATEGORY_DESCRIPTION_DA30 = "OPC Data Access Servers Version 3.0";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_CATEGORY_DESCRIPTION_XMLDA10 = "OPC XML Data Access Servers Version 1.0";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // values for access rights mask.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_READABLE = 0x01;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_WRITEABLE = 0x02;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // values for browse element flags.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_BROWSE_HASCHILDREN = 0x01;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_BROWSE_ISITEM = 0x02;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // well known complex type description systems.   
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_TYPE_SYSTEM_OPCBINARY = "OPCBinary";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_TYPE_SYSTEM_XMLSCHEMA = "XMLSchema";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // complex data consitency window values.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_CONSISTENCY_WINDOW_UNKNOWN = "Unknown";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_CONSISTENCY_WINDOW_NOT_CONSISTENT = "Not Consistent";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // complex data write behavoir values.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_WRITE_BEHAVIOR_BEST_EFFORT = "Best Effort";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_WRITE_BEHAVIOR_ALL_OR_NOTHING = "All or Nothing";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	public static class Qualities
    {
        // Values for fields in the quality word
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_MASK = 0xC0;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_STATUS_MASK = 0xFC;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_LIMIT_MASK = 0x03;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // Values for QUALITY_MASK bit field
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_BAD = 0x00;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_UNCERTAIN = 0x40;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_GOOD = 0xC0;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // STATUS_MASK Values for Quality = BAD
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_CONFIG_ERROR = 0x04;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_NOT_CONNECTED = 0x08;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_DEVICE_FAILURE = 0x0c;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_SENSOR_FAILURE = 0x10;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_LAST_KNOWN = 0x14;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_COMM_FAILURE = 0x18;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_OUT_OF_SERVICE = 0x1C;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_WAITING_FOR_INITIAL_DATA = 0x20;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // STATUS_MASK Values for Quality = UNCERTAIN
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_LAST_USABLE = 0x44;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_SENSOR_CAL = 0x50;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_EGU_EXCEEDED = 0x54;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_SUB_NORMAL = 0x58;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // STATUS_MASK Values for Quality = GOOD
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_QUALITY_LOCAL_OVERRIDE = 0xD8;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // Values for Limit Bitfield 
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_LIMIT_OK = 0x00;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_LIMIT_LOW = 0x01;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_LIMIT_HIGH = 0x02;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const short OPC_LIMIT_CONST = 0x03;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    //==========================================================================
    // Properties

    /// <exclude />
    public static class Properties
    {
        // property ids.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_DATATYPE = 1;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_VALUE = 2;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_QUALITY = 3;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_TIMESTAMP = 4;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_ACCESS_RIGHTS = 5;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_SCAN_RATE = 6;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_EU_TYPE = 7;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_EU_INFO = 8;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_EU_UNITS = 100;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_DESCRIPTION = 101;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_HIGH_EU = 102;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_LOW_EU = 103;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_HIGH_IR = 104;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_LOW_IR = 105;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_CLOSE_LABEL = 106;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_OPEN_LABEL = 107;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_TIMEZONE = 108;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_CONDITION_STATUS = 300;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_ALARM_QUICK_HELP = 301;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_ALARM_AREA_LIST = 302;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_PRIMARY_ALARM_AREA = 303;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_CONDITION_LOGIC = 304;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_LIMIT_EXCEEDED = 305;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_DEADBAND = 306;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_HIHI_LIMIT = 307;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_HI_LIMIT = 308;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_LO_LIMIT = 309;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_LOLO_LIMIT = 310;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_CHANGE_RATE_LIMIT = 311;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_DEVIATION_LIMIT = 312;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_SOUND_FILE = 313;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // complex data properties.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_TYPE_SYSTEM_ID = 600;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_DICTIONARY_ID = 601;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_TYPE_ID = 602;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_DICTIONARY = 603;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_TYPE_DESCRIPTION = 604;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_CONSISTENCY_WINDOW = 605;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_WRITE_BEHAVIOR = 606;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_UNCONVERTED_ITEM_ID = 607;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_UNFILTERED_ITEM_ID = 608;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int OPC_PROPERTY_DATA_FILTER_VALUE = 609;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // property descriptions.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_DATATYPE = "Item Canonical Data Type";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_VALUE = "Item Value";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_QUALITY = "Item Quality";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_TIMESTAMP = "Item Timestamp";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_ACCESS_RIGHTS = "Item Access Rights";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_SCAN_RATE = "Server Scan Rate";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_EU_TYPE = "Item EU Type";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_EU_INFO = "Item EU Info";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_EU_UNITS = "EU Units";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_DESCRIPTION = "Item Description";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_HIGH_EU = "High EU";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_LOW_EU = "Low EU";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_HIGH_IR = "High Instrument Range";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_LOW_IR = "Low Instrument Range";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_CLOSE_LABEL = "Contact Close Label";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_OPEN_LABEL = "Contact Open Label";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_TIMEZONE = "Item Timezone";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_CONDITION_STATUS = "Condition Status";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_ALARM_QUICK_HELP = "Alarm Quick Help";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_ALARM_AREA_LIST = "Alarm Area List";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_PRIMARY_ALARM_AREA = "Primary Alarm Area";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_CONDITION_LOGIC = "Condition Logic";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_LIMIT_EXCEEDED = "Limit Exceeded";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_DEADBAND = "Deadband";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_HIHI_LIMIT = "HiHi Limit";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_HI_LIMIT = "Hi Limit";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_LO_LIMIT = "Lo Limit";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_LOLO_LIMIT = "LoLo Limit";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_CHANGE_RATE_LIMIT = "Rate of Change Limit";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_DEVIATION_LIMIT = "Deviation Limit";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_SOUND_FILE = "Sound File";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // complex data properties.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_TYPE_SYSTEM_ID = "Type System ID";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_DICTIONARY_ID = "Dictionary ID";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_TYPE_ID = "Type ID";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_DICTIONARY = "Dictionary";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_TYPE_DESCRIPTION = "Type Description";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_CONSISTENCY_WINDOW = "Consistency Window";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_WRITE_BEHAVIOR = "Write Behavior";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_UNCONVERTED_ITEM_ID = "Unconverted Item ID";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_UNFILTERED_ITEM_ID = "Unfiltered Item ID";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_PROPERTY_DESC_DATA_FILTER_VALUE = "Data Filter Value";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }
}
