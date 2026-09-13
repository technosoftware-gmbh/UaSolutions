#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on https://github.com/junian/Standard.Licensing. 
// The complete license agreement for that can be found in this directore in the LICENSE.txt file.
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// Defines the product type of the product of a <see cref="License"/>
    /// </summary>
    public enum ProductType
    {
        /// <summary>
        /// Client
        /// </summary>
        Client = 1,

        /// <summary>
        /// Server
        /// </summary>
        Server = 2,

        /// <summary>
        /// Client and Server
        /// </summary>
        ClientAndServer = 3,

        /// <summary>
        /// Client Gateway
        /// </summary>
        ClientGateway = 4
    }
}
