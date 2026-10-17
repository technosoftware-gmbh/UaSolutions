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
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// .NET Framework 4.8 stand-ins for the .NET APIs this library uses, so the call
    /// sites stay identical across all target frameworks.
    /// </summary>
    internal static class NetFrameworkExtensions
    {
        extension(Enum)
        {
            /// <summary>
            /// Retrieves the name of the constant that has the specified value.
            /// </summary>
            public static string GetName<TEnum>(TEnum value)
                where TEnum : struct, Enum
            {
                return Enum.GetName(typeof(TEnum), value);
            }
        }

        extension(Convert)
        {
            /// <summary>
            /// Converts the bytes to their base-64 string representation.
            /// </summary>
            public static string ToBase64String(ReadOnlySpan<byte> bytes)
            {
                return Convert.ToBase64String(bytes.ToArray());
            }
        }

        extension(RandomNumberGenerator)
        {
            /// <summary>
            /// Fills the span with cryptographically strong random bytes.
            /// </summary>
            public static void Fill(Span<byte> data)
            {
                byte[] buffer = new byte[data.Length];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(buffer);
                }
                buffer.CopyTo(data);
            }
        }

        extension(Rfc2898DeriveBytes)
        {
            /// <summary>
            /// Derives a key of the length of <paramref name="destination"/> with PBKDF2.
            /// </summary>
            public static void Pbkdf2(
                ReadOnlySpan<byte> password,
                ReadOnlySpan<byte> salt,
                Span<byte> destination,
                int iterations,
                HashAlgorithmName hashAlgorithm)
            {
                Rfc2898DeriveBytes.Pbkdf2(password, salt.ToArray(), iterations, hashAlgorithm, destination.Length)
                    .CopyTo(destination);
            }

            /// <summary>
            /// Derives a key of <paramref name="outputLength"/> bytes with PBKDF2.
            /// </summary>
            [SuppressMessage(
                "Security",
                "CA5379:Do Not Use Weak Key Derivation Function Algorithm",
                Justification = "Forwards the caller's algorithm, as the .NET method it stands in for does.")]
            public static byte[] Pbkdf2(
                ReadOnlySpan<byte> password,
                byte[] salt,
                int iterations,
                HashAlgorithmName hashAlgorithm,
                int outputLength)
            {
                using (var pbkdf2 = new Rfc2898DeriveBytes(password.ToArray(), salt, iterations, hashAlgorithm))
                {
                    return pbkdf2.GetBytes(outputLength);
                }
            }
        }
    }
}
#endif
