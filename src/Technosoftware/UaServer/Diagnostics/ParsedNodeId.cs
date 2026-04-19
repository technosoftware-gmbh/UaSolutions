#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Text;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
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
    public class ParsedNodeId
    {
        #region Public Interface
        /// <summary>
        /// The namespace index that qualified the NodeId.
        /// </summary>
        public ushort NamespaceIndex { get; set; }

        /// <summary>
        /// The identifier for the root of the NodeId.
        /// </summary>
        public string RootId { get; set; }

        /// <summary>
        /// The type of root node.
        /// </summary>
        public int RootType { get; set; }

        /// <summary>
        /// The relative path to the component identified by the NodeId.
        /// </summary>
        public string ComponentPath { get; set; }

        /// <summary>
        /// Parses the specified node identifier.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>The parsed node identifier. Null if the identifier cannot be parsed.</returns>
        public static ParsedNodeId Parse(NodeId nodeId)
        {
            // can only parse non-null string node identifiers.
            if (NodeId.IsNull(nodeId))
            {
                return null;
            }

            string identifier = nodeId.Identifier as string;

            if (string.IsNullOrEmpty(identifier))
            {
                return null;
            }

            var parsedNodeId = new ParsedNodeId
            {
                NamespaceIndex = nodeId.NamespaceIndex,

                // extract the type of identifier.
                RootType = 0
            };

            int start = 0;

            for (int ii = 0; ii < identifier.Length; ii++)
            {
                if (!char.IsDigit(identifier[ii]))
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
            var buffer = new StringBuilder();

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

            if (end < identifier.Length)
            {
                parsedNodeId.ComponentPath = identifier[end..];
            }

            return parsedNodeId;
        }

        /// <summary>
        /// Constructs a node identifier from the component pieces.
        /// </summary>
        public static NodeId Construct(
            int rootType,
            string rootId,
            ushort namespaceIndex,
            params string[] componentNames)
        {
            var pnd = new ParsedNodeId
            {
                RootType = rootType,
                RootId = rootId,
                NamespaceIndex = namespaceIndex
            };

            if (componentNames != null)
            {
                var path = new StringBuilder();

                for (int ii = 0; ii < componentNames.Length; ii++)
                {
                    if (path.Length > 0)
                    {
                        path.Append('/');
                    }

                    path.Append(componentNames[ii]);
                }

                pnd.ComponentPath = path.ToString();
            }

            return pnd.Construct(null);
        }

        /// <summary>
        /// Constructs a node identifier.
        /// </summary>
        /// <returns>The node identifier.</returns>
        public NodeId Construct()
        {
            return Construct(null);
        }

        /// <summary>
        /// Constructs a node identifier for a component with the specified name.
        /// </summary>
        /// <returns>The node identifier.</returns>
        public NodeId Construct(string componentName)
        {
            var buffer = new StringBuilder();

            // add the root type.
            buffer.Append(RootType)
                .Append(':');

            // add the root identifier.
            if (RootId != null)
            {
                for (int ii = 0; ii < RootId.Length; ii++)
                {
                    char ch = RootId[ii];

                    // escape any special characters.
                    if (ch is '&' or '?')
                    {
                        buffer.Append('&');
                    }

                    buffer.Append(ch);
                }
            }

            // add the component path.
            if (!string.IsNullOrEmpty(ComponentPath))
            {
                buffer.Append('?')
                    .Append(ComponentPath);
            }

            // add the component name.
            if (!string.IsNullOrEmpty(componentName))
            {
                if (string.IsNullOrEmpty(ComponentPath))
                {
                    buffer.Append('?');
                }
                else
                {
                    buffer.Append('/');
                }

                buffer.Append(componentName);
            }

            // construct the node id with the namespace index provided.
            return new NodeId(buffer.ToString(), NamespaceIndex);
        }

        /// <summary>
        /// Constructs the node identifier for a component.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="namespaceIndex">Index of the namespace.</param>
        /// <returns>The node identifier for a component.</returns>
        public static NodeId CreateIdForComponent(NodeState component, ushort namespaceIndex)
        {
            if (component == null)
            {
                return default;
            }

            // components must be instances with a parent.

            if (component is not BaseInstanceState instance || instance.Parent == null)
            {
                return component.NodeId;
            }

            // parent must have a string identifier.
            if (instance.Parent.NodeId.Identifier is not string parentId)
            {
                return default;
            }

            var buffer = new StringBuilder();
            buffer.Append(parentId);

            // check if the parent is another component.
            int index = parentId.IndexOf('?', StringComparison.Ordinal);

            if (index < 0)
            {
                buffer.Append('?');
            }
            else
            {
                buffer.Append('/');
            }

            buffer.Append(component.SymbolicName);

            // return the node identifier.
            return new NodeId(buffer.ToString(), namespaceIndex);
        }

        /// <summary>
        /// Extracts a number from the string.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="start">The start. Set the first non-digit character.</param>
        /// <returns>The number</returns>
        protected static uint ExtractNumber(string identifier, ref int start)
        {
            uint number = 0;

            for (int ii = start; ii < identifier.Length; ii++)
            {
                if (!Char.IsDigit(identifier[ii]))
                {
                    start = ii;
                    return number;
                }

                number *= 10;
                number += (byte)(identifier[ii] - '0');
            }

            start = identifier.Length;
            return number;
        }

        /// <summary>
        /// Escapes and appends a string.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="text">The text.</param>
        /// <param name="specialChars">The special chars.</param>
        protected static void EscapeAndAppendString(StringBuilder buffer, string text, params char[] specialChars)
        {
            // add the root identifier.
            if (text != null)
            {
                for (int ii = 0; ii < text.Length; ii++)
                {
                    char ch = text[ii];

                    // escape any special characters.
                    for (int jj = 0; jj < specialChars.Length; jj++)
                    {
                        if (specialChars[jj] == ch)
                        {
                            buffer.Append(specialChars[0]);
                        }
                    }

                    buffer.Append(ch);
                }
            }
        }

        /// <summary>
        /// Extracts the and unescapes a string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="start">The start.</param>
        /// <param name="specialChars">The special chars.</param>
        /// <returns></returns>
        protected static string ExtractAndUnescapeString(string text, ref int start, params char[] specialChars)
        {
            StringBuilder buffer = new StringBuilder();

            int index = start;
            bool escaped = false;

            while (index < text.Length)
            {
                char ch = text[index++];

                if (!escaped)
                {
                    // skip any escape character but keep the one after it.
                    if (ch == specialChars[0])
                    {
                        escaped = true;
                        continue;
                    }

                    // terminate on any special char other than the escape char.
                    for (int jj = 1; jj < specialChars.Length; jj++)
                    {
                        if (specialChars[jj] == ch)
                        {
                            start = index-1;
                            return buffer.ToString();
                        }
                    }
                }

                buffer.Append(ch);
                escaped = false;
            }

            start = text.Length;
            return buffer.ToString();
        }
        #endregion
    }
}
