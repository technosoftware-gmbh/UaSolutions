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

namespace Technosoftware.Tests
{
    /// <summary>
    /// Helpers for reading values out of the built-in structs in tests.
    /// </summary>
    internal static class DataValueExtensions
    {
        /// <summary>
        /// The boxed value of a data value, exactly as the removed
        /// <c>DataValue.Value</c> property returned it.
        /// </summary>
        /// <remarks>
        /// The parameterless <c>AsBoxedObject</c> boxes an array as
        /// <c>ArrayOf&lt;T&gt;</c> rather than <see cref="System.Array"/>, so
        /// the legacy behaviour has to be asked for by name. Note that neither
        /// boxes a scalar byte string as a <c>byte[]</c> or a structure as its
        /// encodeable - use <c>WrappedValue.TryGetValue</c> or
        /// <c>WrappedValue.TryGetStructure</c> for those.
        /// </remarks>
        public static object BoxedValue(this DataValue value)
        {
            return value.WrappedValue.AsBoxedObject(Variant.BoxingBehavior.Legacy);
        }
    }
}
