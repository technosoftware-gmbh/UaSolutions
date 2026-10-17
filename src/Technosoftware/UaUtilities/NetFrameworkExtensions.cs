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

#if NETFRAMEWORK

#region Using Directives
using System;
using System.IO;
using System.Security.Cryptography;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// .NET Framework 4.8 stand-ins for the .NET APIs this library uses, so the call
    /// sites stay identical across all target frameworks.
    /// </summary>
    internal static class NetFrameworkExtensions
    {
        extension(ArgumentException)
        {
            /// <summary>
            /// Throws if <paramref name="argument"/> is null, empty or white space.
            /// .NET Framework has no CallerArgumentExpression, so the parameter name is not reported.
            /// </summary>
            public static void ThrowIfNullOrWhiteSpace(string argument)
            {
                if (argument is null)
                {
                    throw new ArgumentNullException(nameof(argument));
                }
                if (string.IsNullOrWhiteSpace(argument))
                {
                    throw new ArgumentException(
                        "The value cannot be an empty string or composed entirely of whitespace.",
                        nameof(argument));
                }
            }
        }

        extension(SHA256)
        {
            /// <summary>
            /// Computes the SHA-256 hash of the data.
            /// </summary>
            public static byte[] HashData(byte[] source)
            {
                using (var sha256 = SHA256.Create())
                {
                    return sha256.ComputeHash(source);
                }
            }

            /// <summary>
            /// Computes the SHA-256 hash of the stream.
            /// </summary>
            public static byte[] HashData(Stream source)
            {
                using (var sha256 = SHA256.Create())
                {
                    return sha256.ComputeHash(source);
                }
            }
        }
    }
}
#endif
