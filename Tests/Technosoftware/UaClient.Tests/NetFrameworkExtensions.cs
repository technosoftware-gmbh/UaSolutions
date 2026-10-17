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
using System.Threading.Tasks;
#endregion Using Directives

namespace Technosoftware.UaClient
{
    /// <summary>
    /// .NET Framework 4.8 stand-ins for the .NET APIs the tests use.
    /// </summary>
    internal static class TestNetFrameworkExtensions
    {
        extension(Guid guid)
        {
            /// <summary>
            /// Writes the guid's bytes to the span.
            /// </summary>
            public bool TryWriteBytes(Span<byte> destination)
            {
                byte[] bytes = guid.ToByteArray();
                if (destination.Length < bytes.Length)
                {
                    return false;
                }
                bytes.CopyTo(destination);
                return true;
            }
        }

        extension(BitConverter)
        {
            /// <summary>
            /// Writes the value's bytes to the span.
            /// </summary>
            public static bool TryWriteBytes(Span<byte> destination, ulong value)
            {
                byte[] bytes = BitConverter.GetBytes(value);
                if (destination.Length < bytes.Length)
                {
                    return false;
                }
                bytes.CopyTo(destination);
                return true;
            }

            /// <summary>
            /// Reads an unsigned 64-bit integer from the span.
            /// </summary>
            public static ulong ToUInt64(ReadOnlySpan<byte> value)
            {
                return BitConverter.ToUInt64(value.ToArray(), 0);
            }
        }

        extension(ValueTask)
        {
            /// <summary>
            /// Creates a <see cref="ValueTask{TResult}"/> that has completed with the exception.
            /// </summary>
            public static ValueTask<TResult> FromException<TResult>(Exception exception)
            {
                return new ValueTask<TResult>(Task.FromException<TResult>(exception));
            }
        }
    }
}

#endif
