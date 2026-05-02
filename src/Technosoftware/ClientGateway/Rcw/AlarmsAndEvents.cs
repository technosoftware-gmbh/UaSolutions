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
    [GuidAttribute("58E13251-AC87-11d1-84D5-00608CB8A7E9")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface CATID_OPCAEServer10 { }

    /// <exclude />
    public enum OPCAEBROWSEDIRECTION
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCAE_BROWSE_UP = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCAE_BROWSE_DOWN,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCAE_BROWSE_TO
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCAEBROWSETYPE
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_AREA = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPC_SOURCE
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
    public enum OPCEVENTSERVERSTATE
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCAE_STATUS_RUNNING = 1,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCAE_STATUS_FAILED,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCAE_STATUS_NOCONFIG,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCAE_STATUS_SUSPENDED,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCAE_STATUS_TEST,
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        OPCAE_STATUS_COMM_FAULT
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct ONEVENTSTRUCT
    {
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wChangeMask;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wNewState;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szSource;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public System.Runtime.InteropServices.ComTypes.FILETIME ftTime;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szMessage;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwEventType;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwEventCategory;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwSeverity;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szConditionName;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szSubconditionName;
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
        public int bAckRequired;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public System.Runtime.InteropServices.ComTypes.FILETIME ftActiveTime;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwCookie;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwNumEventAttrs;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pEventAttributes;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szActorID;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct OPCEVENTSERVERSTATUS
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
        public OPCEVENTSERVERSTATE dwServerState;
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
    public struct OPCCONDITIONSTATE
    {
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wState;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wReserved1;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szActiveSubCondition;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szASCDefinition;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwASCSeverity;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szASCDescription;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wQuality;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I2)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public short wReserved2;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAckTime;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public System.Runtime.InteropServices.ComTypes.FILETIME ftSubCondLastActive;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public System.Runtime.InteropServices.ComTypes.FILETIME ftCondLastActive;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public System.Runtime.InteropServices.ComTypes.FILETIME ftCondLastInactive;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szAcknowledgerID;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.LPWStr)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public string szComment;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        [MarshalAs(UnmanagedType.I4)]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwNumSCs;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pszSCNames;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pszSCDefinitions;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pdwSCSeverities;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pszSCDescriptions;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public int dwNumEventAttrs;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pEventAttributes;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public IntPtr pErrors;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("65168851-5783-11D1-84A0-00608CB8A7E9")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCEventServer
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetStatus(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            out IntPtr ppEventServerStatus);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void CreateEventSubscription(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int bActive,
            [MarshalAs(UnmanagedType.I4)]
            int dwBufferTime,
            [MarshalAs(UnmanagedType.I4)]
            int dwMaxSize,
            [MarshalAs(UnmanagedType.I4)]
            int hClientSubscription,
            ref Guid riid,
            [Out][MarshalAs(UnmanagedType.IUnknown, IidParameterIndex=4)]
            out object ppUnk,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwRevisedBufferTime,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwRevisedMaxSize);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QueryAvailableFilters(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwFilterMask);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QueryEventCategories(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwEventType,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppdwEventCategories,
            [Out]
            out IntPtr ppszEventCategoryDescs);

        [PreserveSig]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        int QueryConditionNames(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwEventCategory,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppszConditionNames);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QuerySubConditionNames(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szConditionName,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppszSubConditionNames);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QuerySourceConditions(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szSource,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppszConditionNames);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QueryEventAttributes(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwEventCategory,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppdwAttrIDs,
            [Out]
            out IntPtr ppszAttrDescs,
            [Out]
            out IntPtr ppvtAttrTypes);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void TranslateToItemIDs(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szSource,
            [MarshalAs(UnmanagedType.I4)]
            int dwEventCategory,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szConditionName,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szSubconditionName,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=4)]
            int[] pdwAssocAttrIDs,
            out IntPtr ppszAttrItemIDs,
            out IntPtr ppszNodeNames,
            out IntPtr ppCLSIDs);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetConditionState(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szSource,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szConditionName,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumEventAttrs,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] pdwAttributeIDs,
            [Out]
            out IntPtr ppConditionState);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void EnableConditionByArea(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumAreas,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszAreas);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void EnableConditionBySource(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumSources,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszSources);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void DisableConditionByArea(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumAreas,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszAreas);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void DisableConditionBySource(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumSources,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszSources);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void AckCondition(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szAcknowledgerID,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szComment,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszSource,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] szConditionName,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            System.Runtime.InteropServices.ComTypes.FILETIME[] pftActiveTime,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] pdwCookie,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void CreateAreaBrowser(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            ref Guid riid,
            [Out][MarshalAs(UnmanagedType.IUnknown, IidParameterIndex=0)]
            out object ppUnk);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("65168855-5783-11D1-84A0-00608CB8A7E9")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCEventSubscriptionMgt
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetFilter(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwEventType,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumCategories,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] pdwEventCategories,
            [MarshalAs(UnmanagedType.I4)]
            int dwLowSeverity,
            [MarshalAs(UnmanagedType.I4)]
            int dwHighSeverity,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumAreas,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=5)]
            string[] pszAreaList,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumSources,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=7)]
            string[] pszSourceList);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetFilter(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwEventType,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwNumCategories,
            [Out]
            out IntPtr ppdwEventCategories,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwLowSeverity,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwHighSeverity,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwNumAreas,
            [Out]
            out IntPtr ppszAreaList,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwNumSources,
            [Out]
            out IntPtr ppszSourceList);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SelectReturnedAttributes(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwEventCategory,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] dwAttributeIDs);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetReturnedAttributes(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwEventCategory,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppdwAttributeIDs);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Refresh(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwConnection);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void CancelRefresh(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwConnection);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetState(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pbActive,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwBufferTime,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwMaxSize,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int phClientSubscription);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetState(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            IntPtr pbActive,
            IntPtr pdwBufferTime,
            IntPtr pdwMaxSize,
            [MarshalAs(UnmanagedType.I4)]
            int hClientSubscription,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwRevisedBufferTime,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwRevisedMaxSize);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("65168857-5783-11D1-84A0-00608CB8A7E9")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCEventAreaBrowser
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void ChangeBrowsePosition(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            OPCAEBROWSEDIRECTION dwBrowseDirection,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szString);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void BrowseOPCAreas(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            OPCAEBROWSETYPE dwBrowseFilterType,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szFilterCriteria,
            [Out]
            out Technosoftware.Rcw.IEnumString ppIEnumString);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetQualifiedAreaName(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szAreaName,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string pszQualifiedAreaName);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetQualifiedSourceName(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szSourceName,
            [Out][MarshalAs(UnmanagedType.LPWStr)]
            out string pszQualifiedSourceName);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("6516885F-5783-11D1-84A0-00608CB8A7E9")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCEventSink
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void OnEvent(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int hClientSubscription,
            [MarshalAs(UnmanagedType.I4)]
            int bRefresh,
            [MarshalAs(UnmanagedType.I4)]
            int bLastRefresh,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=3)]
            ONEVENTSTRUCT[] pEvents);
    }

    /// <exclude />
	[ComImport]
    [GuidAttribute("71BBE88E-9564-4bcd-BCFC-71C558D94F2D")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCEventServer2 // : IOPCEventServer
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetStatus(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            out IntPtr ppEventServerStatus);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void CreateEventSubscription(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int bActive,
            [MarshalAs(UnmanagedType.I4)]
            int dwBufferTime,
            [MarshalAs(UnmanagedType.I4)]
            int dwMaxSize,
            [MarshalAs(UnmanagedType.I4)]
            int hClientSubscription,
            ref Guid riid,
            [Out][MarshalAs(UnmanagedType.IUnknown, IidParameterIndex=4)]
            out object ppUnk,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwRevisedBufferTime,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwRevisedMaxSize);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QueryAvailableFilters(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwFilterMask);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QueryEventCategories(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwEventType,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppdwEventCategories,
            [Out]
            out IntPtr ppszEventCategoryDescs);

        [PreserveSig]
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        int QueryConditionNames(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwEventCategory,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppszConditionNames);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QuerySubConditionNames(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szConditionName,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppszSubConditionNames);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QuerySourceConditions(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szSource,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppszConditionNames);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void QueryEventAttributes(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwEventCategory,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppdwAttrIDs,
            [Out]
            out IntPtr ppszAttrDescs,
            [Out]
            out IntPtr ppvtAttrTypes);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void TranslateToItemIDs(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szSource,
            [MarshalAs(UnmanagedType.I4)]
            int dwEventCategory,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szConditionName,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szSubconditionName,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=4)]
            int[] pdwAssocAttrIDs,
            out IntPtr ppszAttrItemIDs,
            out IntPtr ppszNodeNames,
            out IntPtr ppCLSIDs);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetConditionState(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.LPWStr)]
            string szSource,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szConditionName,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumEventAttrs,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=2)]
            int[] pdwAttributeIDs,
            [Out]
            out IntPtr ppConditionState);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void EnableConditionByArea(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumAreas,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszAreas);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void EnableConditionBySource(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumSources,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszSources);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void DisableConditionByArea(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumAreas,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszAreas);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void DisableConditionBySource(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumSources,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszSources);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void AckCondition(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szAcknowledgerID,
            [MarshalAs(UnmanagedType.LPWStr)]
            string szComment,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszSource,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] szConditionName,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStruct, SizeParamIndex=0)]
            System.Runtime.InteropServices.ComTypes.FILETIME[] pftActiveTime,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=0)]
            int[] pdwCookie,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void CreateAreaBrowser(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            ref Guid riid,
            [Out][MarshalAs(UnmanagedType.IUnknown, IidParameterIndex=0)]
            out object ppUnk);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void EnableConditionByArea2(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumAreas,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszAreas,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void EnableConditionBySource2(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumSources,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszSources,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void DisableConditionByArea2(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumAreas,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszAreas,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void DisableConditionBySource2(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumSources,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszSources,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetEnableStateByArea(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumAreas,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszAreas,
            [Out]
            out IntPtr pbEnabled,
            [Out]
            out IntPtr pbEffectivelyEnabled,
            [Out]
            out IntPtr ppErrors);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetEnableStateBySource(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwNumSources,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=0)]
            string[] pszSources,
            out IntPtr pbEnabled,
            out IntPtr pbEffectivelyEnabled,
            out IntPtr ppErrors);
    };

    /// <exclude />
	[ComImport]
    [GuidAttribute("94C955DC-3684-4ccb-AFAB-F898CE19AAC3")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOPCEventSubscriptionMgt2 // : IOPCEventSubscriptionMgt
    {
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetFilter(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwEventType,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumCategories,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] pdwEventCategories,
            [MarshalAs(UnmanagedType.I4)]
            int dwLowSeverity,
            [MarshalAs(UnmanagedType.I4)]
            int dwHighSeverity,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumAreas,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=5)]
            string[] pszAreaList,
            [MarshalAs(UnmanagedType.I4)]
            int dwNumSources,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPWStr, SizeParamIndex=7)]
            string[] pszSourceList);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetFilter(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwEventType,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwNumCategories,
            [Out]
            out IntPtr ppdwEventCategories,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwLowSeverity,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwHighSeverity,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwNumAreas,
            [Out]
            out IntPtr ppszAreaList,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwNumSources,
            [Out]
            out IntPtr ppszSourceList);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SelectReturnedAttributes(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwEventCategory,
            [MarshalAs(UnmanagedType.I4)]
            int dwCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I4, SizeParamIndex=1)]
            int[] dwAttributeIDs);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetReturnedAttributes(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwEventCategory,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwCount,
            [Out]
            out IntPtr ppdwAttributeIDs);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void Refresh(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwConnection);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void CancelRefresh(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [MarshalAs(UnmanagedType.I4)]
            int dwConnection);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void GetState(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pbActive,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwBufferTime,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwMaxSize,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int phClientSubscription);

#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        void SetState(
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
            IntPtr pbActive,
            IntPtr pdwBufferTime,
            IntPtr pdwMaxSize,
            [MarshalAs(UnmanagedType.I4)]
            int hClientSubscription,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwRevisedBufferTime,
            [Out][MarshalAs(UnmanagedType.I4)]
            out int pdwRevisedMaxSize);

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
        // category description string.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const string OPC_CATEGORY_DESCRIPTION_AE10 = "OPC Alarm & Event Server Version 1.0";
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // state bit masks.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int CONDITION_ENABLED = 0x0001;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int CONDITION_ACTIVE = 0x0002;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int CONDITION_ACKED = 0x0004;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // bit masks for change mask.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int CHANGE_ACTIVE_STATE = 0x0001;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int CHANGE_ACK_STATE = 0x0002;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int CHANGE_ENABLE_STATE = 0x0004;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int CHANGE_QUALITY = 0x0008;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int CHANGE_SEVERITY = 0x0010;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int CHANGE_SUBCONDITION = 0x0020;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int CHANGE_MESSAGE = 0x0040;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int CHANGE_ATTRIBUTE = 0x0080;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // event type.
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int SIMPLE_EVENT = 0x0001;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int TRACKING_EVENT = 0x0002;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int CONDITION_EVENT = 0x0004;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int ALL_EVENTS = 0x0007;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element

        // bit masks for QueryAvailableFilters().
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int FILTER_BY_EVENT = 0x0001;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int FILTER_BY_CATEGORY = 0x0002;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int FILTER_BY_SEVERITY = 0x0004;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int FILTER_BY_AREA = 0x0008;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
#pragma warning disable CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
        public const int FILTER_BY_SOURCE = 0x0010;
#pragma warning restore CS1591 // Fehledes XML-Kommentar für öffentlich sichtbaren Typ oder Element
    }
}
