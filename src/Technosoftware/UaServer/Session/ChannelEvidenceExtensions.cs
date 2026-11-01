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
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Conversions of the read-only channel evidence in <see cref="SecureChannelContext"/>
    /// for APIs that take arrays.
    /// </summary>
    internal static class ChannelEvidenceExtensions
    {
        /// <summary>
        /// Returns a copy of the bytes, or <c>null</c> for a null byte string, matching
        /// the nullable arrays the signature helpers expect.
        /// </summary>
        /// <param name="value">The byte string to convert.</param>
        /// <returns>A copy of the bytes, or <c>null</c> if the byte string is null.</returns>
        public static byte[] ToArrayOrNull(this ByteString value)
        {
            return value.IsNull ? null : value.ToArray();
        }
    }
}
