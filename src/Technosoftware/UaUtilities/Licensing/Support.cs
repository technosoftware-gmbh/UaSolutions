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
    /// The support of a <see cref="License"/>.
    /// </summary>
    public class Support : LicenseAttributes
    {
        internal Support(XElement xmlData)
            : base(xmlData, "Support")
        {
        }

        /// <summary>
        /// Gets or sets the level of this <see cref="Support"/>.
        /// </summary>
        public string Level
        {
            get => GetTag("Level"); set => SetTag("Level", value);
        }

        /// <summary>
        /// Gets or sets the expiration date of this <see cref="Support"/>.
        /// </summary>
        public string ExpirationDate
        {
            get => GetTag("ExpirationDate"); set => SetTag("ExpirationDate", value);
        }
    }
}
