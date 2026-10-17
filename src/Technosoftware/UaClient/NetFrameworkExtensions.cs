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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BitFaster.Caching;
#endregion Using Directives

namespace Technosoftware.UaClient
{
    /// <summary>
    /// .NET Framework 4.8 stand-ins for the .NET APIs this library uses, so the call
    /// sites stay identical across all target frameworks.
    /// </summary>
    internal static class NetFrameworkExtensions
    {
        extension(ValueTask)
        {
            /// <summary>
            /// Creates a <see cref="ValueTask{TResult}"/> that has completed with the result.
            /// </summary>
            public static ValueTask<TResult> FromResult<TResult>(TResult result)
            {
                return new ValueTask<TResult>(result);
            }
        }

        /// <summary>
        /// Gets the value for a key, or adds it using the factory and its argument.
        /// BitFaster.Caching only provides this overload on .NET Core 3.0 and later.
        /// The factory may return a subtype of <typeparamref name="TValue"/>, as the
        /// node cache's factories do, which the instance method on .NET accepts too.
        /// </summary>
        public static ValueTask<TValue> GetOrAddAsync<TKey, TValue, TArg, TResult>(
            this IAsyncCache<TKey, TValue> cache,
            TKey key,
            Func<TKey, TArg, Task<TResult>> valueFactory,
            TArg factoryArgument)
            where TResult : TValue
        {
            return cache.GetOrAddAsync(
                key,
                async k => await valueFactory(k, factoryArgument).ConfigureAwait(false));
        }

        /// <summary>
        /// Removes the object at the beginning of the queue, if there is one.
        /// </summary>
        public static bool TryDequeue<T>(this Queue<T> queue, [MaybeNullWhen(false)] out T result)
        {
            if (queue.Count == 0)
            {
                result = default;
                return false;
            }
            result = queue.Dequeue();
            return true;
        }

        /// <summary>
        /// Writes the buffer to the stream.
        /// </summary>
        public static Task WriteAsync(this Stream stream, byte[] buffer, CancellationToken ct)
        {
            return stream.WriteAsync(buffer, 0, buffer.Length, ct);
        }

        /// <summary>
        /// Disposes the stream.
        /// </summary>
        public static ValueTask DisposeAsync(this Stream stream)
        {
            stream.Dispose();
            return default;
        }
    }
}
#endif
