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
using System.Globalization;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// Defines assembly build date information for an assembly manifest.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    internal sealed class AssemblyBuildDateAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyBuildDateAttribute"/> class
        /// with the specified build date.
        /// </summary>
        /// <param name="buildDate">The build date of the assembly.</param>
        public AssemblyBuildDateAttribute(DateTime buildDate)
        {
            BuildDate = buildDate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyBuildDateAttribute"/> class
        /// with the specified build date string.
        /// </summary>
        /// <param name="buildDateString">The build date of the assembly.</param>
        public AssemblyBuildDateAttribute(string buildDateString)
        {
            BuildDate = DateTime.Parse(buildDateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            BuildDateString = buildDateString;
        }

        /// <summary>
        /// Gets the assembly build date.
        /// </summary>
        public DateTime BuildDate { get; }
        public string BuildDateString { get; }
    }
}
