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

#endregion Using Directives

namespace Technosoftware.ClientGateway
{
    /// <summary>
    /// An interface to an object that parses item ids.
    /// </summary>
    public interface IItemIdParser
    {
        /// <summary>
        /// Parses the specified item id.
        /// </summary>
        /// <param name="server">The COM server that provided the item id.</param>
        /// <param name="configuration">The COM wrapper configuration.</param>
        /// <param name="itemId">The item id to parse.</param>
        /// <param name="browseName">The name of the item.</param>
        /// <returns>True if the item id could be parsed.</returns>
        bool Parse(
            ComObject server,
            ComClientConfiguration configuration,
            string itemId,
            out string browseName);
    }

    /// <summary>
    /// The default item id parser that uses the settings in the configuration.
    /// </summary>
    internal class ComItemIdParser : IItemIdParser
    {
        #region IItemIdParser Members
        /// <summary>
        /// Parses the specified item id.
        /// </summary>
        /// <param name="server">The COM server that provided the item id.</param>
        /// <param name="configuration">The COM wrapper configuration.</param>
        /// <param name="itemId">The item id to parse.</param>
        /// <param name="browseName">The name of the item.</param>
        /// <returns>True if the item id could be parsed.</returns>
        public bool Parse(ComObject server, ComClientConfiguration configuration, string itemId, out string browseName)
        {
            browseName = null;

            if (configuration == null || itemId == null)
            {
                return false;
            }

            if (String.IsNullOrEmpty(configuration.SeperatorChars))
            {
                return false;
            }

            for (int ii = 0; ii < configuration.SeperatorChars.Length; ii++)
            {
                int index = itemId.LastIndexOf(configuration.SeperatorChars[ii]);

                if (index >= 0)
                {
                    browseName = itemId.Substring(index + 1);
                    return true;
                }
            }

            return false;
        }
        #endregion IItemIdParser Members
    }
}
