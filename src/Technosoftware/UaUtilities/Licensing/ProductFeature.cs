#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on https://github.com/junian/Standard.Licensing. 
// The complete license agreement for that can be found in this directore in the LICENSE.txt file.
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using static System.String;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// The possible products.
    /// </summary>
    [Flags]
    public enum ProductFeature : uint
    {
        /// <summary>
        /// Basic OPC UA Features enabled
        /// </summary>
        None = 0,

        /// <summary>
        /// OPC UA DataAccess enabled
        /// </summary>
        DataAccess = 1,

        /// <summary>
        /// OPC UA Alarms and Conditions enabled
        /// </summary>
        AlarmsConditions = 2,

        /// <summary>
        /// OPC UA Historical Access and Historical Events enabled
        /// </summary>
        HistoricalAccess = 4,

        /// <summary>
        /// OPC UA PubSub enabled
        /// </summary>
        PubSub = 8,

        /// <summary>
        /// All supported OPC UA Features enabled
        /// </summary>
        AllFeatures = 16
    }
}
