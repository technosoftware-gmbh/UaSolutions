#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
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
using System.Collections;
using System.Reflection;
#endregion Using Directives

namespace Technosoftware.ClientGateway
{
    /// <summary>
    /// Defines constants for standard data types.
    /// </summary>
    /// <exclude />
    internal class BuiltInTypes
    {
        /// <remarks/>
        public static Type SBYTE = typeof(sbyte);

        /// <remarks/>
        public static Type BYTE = typeof(byte);

        /// <remarks/>
        public static Type SHORT = typeof(short);

        /// <remarks/>
        public static Type USHORT = typeof(ushort);

        /// <remarks/>
        public static Type INT = typeof(int);

        /// <remarks/>
        public static Type UINT = typeof(uint);

        /// <remarks/>
        public static Type LONG = typeof(long);

        /// <remarks/>
        public static Type ULONG = typeof(ulong);

        /// <remarks/>
        public static Type FLOAT = typeof(float);

        /// <remarks/>
        public static Type DOUBLE = typeof(double);

        /// <remarks/>
        public static Type DECIMAL = typeof(decimal);

        /// <remarks/>
        public static Type BOOLEAN = typeof(bool);

        /// <remarks/>
        public static Type DATETIME = typeof(DateTime);

        /// <remarks/>
        public static Type DURATION = typeof(TimeSpan);

        /// <remarks/>
        public static Type STRING = typeof(string);

        /// <remarks/>
        public static Type ANY_TYPE = typeof(object);

        /// <remarks/>
        public static Type BINARY = typeof(byte[]);

        /// <remarks/>
        public static Type ARRAY_SHORT = typeof(short[]);

        /// <remarks/>
        public static Type ARRAY_USHORT = typeof(ushort[]);

        /// <remarks/>
        public static Type ARRAY_INT = typeof(int[]);

        /// <remarks/>
        public static Type ARRAY_UINT = typeof(uint[]);

        /// <remarks/>
        public static Type ARRAY_LONG = typeof(long[]);

        /// <remarks/>
        public static Type ARRAY_ULONG = typeof(ulong[]);

        /// <remarks/>
        public static Type ARRAY_FLOAT = typeof(float[]);

        /// <remarks/>
        public static Type ARRAY_DOUBLE = typeof(double[]);

        /// <remarks/>
        public static Type ARRAY_DECIMAL = typeof(decimal[]);

        /// <remarks/>
        public static Type ARRAY_BOOLEAN = typeof(bool[]);

        /// <remarks/>
        public static Type ARRAY_DATETIME = typeof(DateTime[]);

        /// <remarks/>
        public static Type ARRAY_STRING = typeof(string[]);

        /// <remarks/>
        public static Type ARRAY_ANY_TYPE = typeof(object[]);

        /// <remarks/>
        public static Type ILLEGAL_TYPE = typeof(BuiltInTypes);

        /// <summary>
        /// Returns an array of all well-known types.
        /// </summary>
        public static Type[] Enumerate()
        {
            var values = new ArrayList();

            FieldInfo[] fields = typeof(BuiltInTypes).GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo field in fields)
            {
                values.Add(field.GetValue(typeof(Type)));
            }

            return (Type[])values.ToArray(typeof(Type));
        }
    }
}
