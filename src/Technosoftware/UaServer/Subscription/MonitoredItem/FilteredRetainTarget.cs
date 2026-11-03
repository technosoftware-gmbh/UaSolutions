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
using System;
using System.Collections.Generic;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Presents a condition to one client's select clauses with <c>Retain</c> forced to
    /// <c>false</c>.
    /// </summary>
    /// <remarks>
    /// OPC UA Part 9, 5.5.2 requires the trailing event a condition produces on its way out
    /// of a client's where clause to carry a client specific <c>Retain = false</c>, whatever
    /// the server itself retains. The filter target behind that event is shared by every
    /// monitored item the condition is reported to, so the override cannot be written into
    /// it; this wrapper applies it while the fields for a single item are read and leaves
    /// the shared target untouched.
    /// </remarks>
    internal sealed class FilteredRetainTarget : IFilterTarget
    {
        private readonly IFilterTarget m_target;

        /// <summary>
        /// Wraps <paramref name="target"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is <c>null</c>.</exception>
        public FilteredRetainTarget(IFilterTarget target)
        {
            m_target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <inheritdoc/>
        public bool IsTypeOf(IFilterContext context, NodeId typeDefinitionId)
        {
            return m_target.IsTypeOf(context, typeDefinitionId);
        }

        /// <inheritdoc/>
        public object GetAttributeValue(
            IFilterContext context,
            NodeId typeDefinitionId,
            IList<QualifiedName> relativePath,
            uint attributeId,
            NumericRange indexRange)
        {
            object value = m_target.GetAttributeValue(
                context,
                typeDefinitionId,
                relativePath,
                attributeId,
                indexRange);

            // only a Retain the target actually resolves is overridden; a clause the type
            // check or the browse path rejects stays null so the field list keeps its shape.
            if (value is bool &&
                attributeId == Attributes.Value &&
                relativePath != null &&
                relativePath.Count == 1 &&
                relativePath[0] != null &&
                relativePath[0].NamespaceIndex == 0 &&
                relativePath[0].Name == BrowseNames.Retain)
            {
                return false;
            }

            return value;
        }
    }
}
