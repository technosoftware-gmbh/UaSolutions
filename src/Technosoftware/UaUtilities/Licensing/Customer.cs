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
using System.Xml.Linq;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// The customer of a <see cref="License"/>.
    /// </summary>
    public class Customer : LicenseAttributes
    {
        internal Customer(XElement xmlData)
            : base(xmlData, "Customer")
        {
        }

        /// <summary>
        /// Gets or sets the Name of this <see cref="Customer"/>.
        /// </summary>
        public string Name
        {
            get => GetTag("Name"); set => SetTag("Name", value);
        }

        /// <summary>
        /// Gets or sets the Company of this <see cref="Customer"/>.
        /// </summary>
        public string Company
        {
            get => GetTag("Company"); set => SetTag("Company", value);
        }

        /// <summary>
        /// Gets or sets the domain address of this <see cref="Customer"/>.
        /// </summary>
        public string Domain
        {
            get => GetTag("Domain"); set => SetTag("Domain", value);
        }

    }
}
