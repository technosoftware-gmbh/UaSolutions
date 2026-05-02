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
using System.Text;

using Opc.Ua;

using Technosoftware.Common.Client;
using Technosoftware.UaServer;

#endregion Using Directives

namespace Technosoftware.ClientGateway.Da
{
    /// <summary>
    /// Stores the elements of a NodeId after it is parsed.
    /// </summary>
    /// <remarks>
    /// The NodeIds used by the samples are strings with an optional path appended.
    /// The RootType identifies the type of Root Node. The RootId is the unique identifier
    /// for the Root Node. The ComponentPath is constructed from the SymbolicNames
    /// of one or more children of the Root Node. 
    /// </remarks>
    internal class DaParsedNodeId : ParsedNodeId
    {
        #region Public Interface
        /// <summary>
        /// The identifier for the property identifier.
        /// </summary>
        public int PropertyId
        {
            get { return m_propertyId; }
            set { m_propertyId = value; }
        }

        /// <summary>
        /// Parses the specified node identifier.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>The parsed node identifier. Null if the identifier cannot be parsed.</returns>
        public static new DaParsedNodeId Parse(NodeId nodeId)
        {
            // can only parse non-null string node identifiers.
            if (NodeId.IsNull(nodeId))
            {
                return null;
            }

            string identifier = nodeId.Identifier as string;

            if (String.IsNullOrEmpty(identifier))
            {
                return null;
            }

            DaParsedNodeId parsedNodeId = new DaParsedNodeId();
            parsedNodeId.NamespaceIndex = nodeId.NamespaceIndex;

            // extract the type of identifier.
            parsedNodeId.RootType = 0;

            int start = 0;

            for (int ii = 0; ii < identifier.Length; ii++)
            {
                if (!Char.IsDigit(identifier[ii]))
                {
                    start = ii;
                    break;
                }

                parsedNodeId.RootType *= 10;
                parsedNodeId.RootType += (byte)(identifier[ii] - '0');
            }

            if (start >= identifier.Length || identifier[start] != ':')
            {
                return null;
            }

            // extract any component path.
            StringBuilder buffer = new StringBuilder();

            int index = start + 1;
            int end = identifier.Length;

            bool escaped = false;

            while (index < end)
            {
                char ch = identifier[index++];

                // skip any escape character but keep the one after it.
                if (ch == '&')
                {
                    escaped = true;
                    continue;
                }

                if (!escaped && ch == '?')
                {
                    end = index;
                    break;
                }

                buffer.Append(ch);
                escaped = false;
            }

            // extract any component.
            parsedNodeId.RootId = buffer.ToString();
            parsedNodeId.ComponentPath = null;

            if (parsedNodeId.RootType == DaModelUtils.DaProperty)
            {
                // must have the property id.
                if (end >= identifier.Length)
                {
                    return null;
                }

                // extract the property id.
                for (int ii = end; ii < identifier.Length; ii++)
                {
                    end++;

                    if (!Char.IsDigit(identifier[ii]))
                    {
                        // check for terminator.
                        if (identifier[ii] != ':')
                        {
                            return null;
                        }

                        break;
                    }

                    parsedNodeId.PropertyId *= 10;
                    parsedNodeId.PropertyId += (byte)(identifier[ii] - '0');
                }
            }

            // extract the component path.
            if (end < identifier.Length)
            {
                parsedNodeId.ComponentPath = identifier.Substring(end);
            }

            return parsedNodeId;
        }

        /// <summary>
        /// Constructs a node identifier.
        /// </summary>
        /// <returns>The node identifier.</returns>
        public new NodeId Construct()
        {
            StringBuilder buffer = new StringBuilder();

            // add the root type.
            buffer.Append(RootType);
            buffer.Append(':');

            // add the root identifier.
            if (this.RootId != null)
            {
                for (int ii = 0; ii < this.RootId.Length; ii++)
                {
                    char ch = this.RootId[ii];

                    // escape any special characters.
                    if (ch == '&' || ch == '?')
                    {
                        buffer.Append('&');
                    }

                    buffer.Append(ch);
                }
            }

            // add property id.
            if (this.RootType == DaModelUtils.DaProperty)
            {
                buffer.Append('?');
                buffer.Append(this.PropertyId);

                // add the component path.
                if (!String.IsNullOrEmpty(this.ComponentPath))
                {
                    buffer.Append(':');
                    buffer.Append(this.ComponentPath);
                }
            }
            else
            {
                // add the component path.
                if (!String.IsNullOrEmpty(this.ComponentPath))
                {
                    buffer.Append('?');
                    buffer.Append(this.ComponentPath);
                }
            }

            // construct the node id with the namespace index provided.
            return new NodeId(buffer.ToString(), this.NamespaceIndex);
        }
        #endregion Public Interface

        #region Private Fields
        private int m_propertyId;
        #endregion Private Fields
    }
}
