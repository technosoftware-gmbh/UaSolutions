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
using System;
#endregion Using Directives

namespace Technosoftware.UaConfiguration
{
    /// <summary>
    /// The features a license can grant. Mirrors the feature flags used by the
    /// licensing library so that the license gate can be referenced from libraries
    /// that do not depend on it.
    /// </summary>
    [Flags]
    public enum UaFeature : uint
    {
        /// <summary>
        /// Basic OPC UA features only.
        /// </summary>
        None = 0,

        /// <summary>
        /// OPC UA Data Access.
        /// </summary>
        DataAccess = 1,

        /// <summary>
        /// OPC UA Alarms and Conditions.
        /// </summary>
        AlarmsConditions = 2,

        /// <summary>
        /// OPC UA Historical Access and Historical Events.
        /// </summary>
        HistoricalAccess = 4,

        /// <summary>
        /// OPC UA PubSub.
        /// </summary>
        PubSub = 8,

        /// <summary>
        /// All supported OPC UA features.
        /// </summary>
        AllFeatures = 16
    }
}
