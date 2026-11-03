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
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Wraps an event so that its Retain field reads as false, for the trailing event
    /// a filtered retain monitored item sends when a condition leaves its filter.
    /// </summary>
    internal sealed class FilteredRetainTarget : IFilterTarget
    {
        private readonly IFilterTarget m_target;

        /// <summary>
        /// Creates the wrapper for the event.
        /// </summary>
        /// <param name="target">The event to wrap.</param>
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
        public Variant GetAttributeValue(
            IFilterContext context,
            NodeId typeDefinitionId,
            ArrayOf<QualifiedName> relativePath,
            uint attributeId,
            NumericRange indexRange)
        {
            Variant value = m_target.GetAttributeValue(
                context,
                typeDefinitionId,
                relativePath,
                attributeId,
                indexRange);

            // only a Retain the target actually resolves is overridden; a clause the type
            // check or the browse path rejects stays null so the field list keeps its shape.
            if (value.TryGetValue(out bool _) &&
                attributeId == Attributes.Value &&
                relativePath.Count == 1 &&
                !relativePath[0].IsNull &&
                relativePath[0].NamespaceIndex == 0 &&
                relativePath[0].Name == BrowseNames.Retain)
            {
                return new Variant(false);
            }

            return value;
        }
    }
}
