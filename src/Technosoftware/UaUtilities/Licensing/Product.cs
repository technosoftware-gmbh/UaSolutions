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
using System.Xml.Linq;
using static Technosoftware.UaUtilities.LicenseHandler;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// The product of a <see cref="License"/>.
    /// </summary>
    public class Product : LicenseAttributes
    {
        internal Product(XElement xmlData)
            : base(xmlData, "Product")
        {
        }

        /// <summary>
        /// Gets or sets the Name of this <see cref="Product"/>.
        /// </summary>
        public string Name
        {
            get => GetTag("Name"); set => SetTag("Name", value);
        }

        /// <summary>
        /// Gets or sets the type of this <see cref="Product"/>.
        /// </summary>
        public string Type
        {
            get => GetTag("Type"); set => SetTag("Type", value);
        }

        /// <summary>
        /// The version of the product.
        /// </summary>
        public string Version
        {
            get => GetTag("Version"); set => SetTag("Version", value);
        }

        /// <summary>
        /// Gets or sets the publish date of this <see cref="Product"/>.
        /// </summary>
        public string PublishedDate
        {
            get => GetTag("PublishedDate"); set => SetTag("PublishedDate", value);
        }
    }
}
