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

using Opc.Ua;

using Technosoftware.Common;

#endregion Using Directives

namespace Technosoftware.ClientGateway.Da
{
    /// <summary>
    /// A object which maps a COM DA item to a UA variable.
    /// </summary>
    internal partial class DaPropertyState : PropertyState
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DaPropertyState"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="itemId">The item id.</param>
        /// <param name="property">The property.</param>
        /// <param name="namespaceIndex">Index of the namespace.</param>
        public DaPropertyState(
            ISystemContext context,
            string itemId,
            DaProperty property,
            ushort namespaceIndex)
        :
            base(null)
        {
            this.TypeDefinitionId = Opc.Ua.VariableTypeIds.DataItemType;
            this.Description = null;
            this.WriteMask = 0;
            this.UserWriteMask = 0;

            if (property != null)
            {
                Initialize(context, itemId, property, namespaceIndex);
            }
        }
        #endregion Constructors

        #region Public Interface
        /// <summary>
        /// Gets the item id.
        /// </summary>
        /// <value>The item id.</value>
        public string ItemId
        {
            get
            {
                return m_itemId;
            }
        }

        /// <summary>
        /// Gets the property id.
        /// </summary>
        /// <value>The property id.</value>
        public int PropertyId
        {
            get
            {
                if (m_property != null)
                {
                    return m_property.PropertyId;
                }

                return -1;
            }
        }

        /// <summary>
        /// Gets the property description.
        /// </summary>
        /// <value>The property description.</value>
        public DaProperty Property
        {
            get { return m_property; }
        }

        /// <summary>
        /// Initializes the node from the element.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="itemId">The item id.</param>
        /// <param name="property">The property.</param>
        /// <param name="namespaceIndex">Index of the namespace.</param>
        public void Initialize(ISystemContext context, string itemId, DaProperty property, ushort namespaceIndex)
        {
            m_itemId = itemId;
            m_property = property;

            if (property == null)
            {
                return;
            }

            this.NodeId = DaModelUtils.ConstructIdForDaElement(m_itemId, property.PropertyId, namespaceIndex);
            this.BrowseName = new QualifiedName(property.Name, namespaceIndex);
            this.DisplayName = new LocalizedText(property.Name);
            this.TypeDefinitionId = Opc.Ua.VariableTypeIds.PropertyType;
            this.Value = null;
            this.StatusCode = StatusCodes.BadWaitingForInitialData;
            this.Timestamp = DateTime.UtcNow;

            bool isArray = false;
            this.DataType = ComUtils.GetDataTypeId(property.DataType, out isArray);
            this.ValueRank = (isArray) ? ValueRanks.OneOrMoreDimensions : ValueRanks.Scalar;
            this.ArrayDimensions = null;

            // assume that properties with item ids are writeable. the server may still reject the write.
            if (String.IsNullOrEmpty(property.ItemId))
            {
                this.AccessLevel = AccessLevels.CurrentRead;
            }
            else
            {
                this.AccessLevel = AccessLevels.CurrentReadOrWrite;
            }

            this.UserAccessLevel = this.AccessLevel;
            this.MinimumSamplingInterval = MinimumSamplingIntervals.Indeterminate;
            this.Historizing = false;

            // add a reference to the parent node.
            NodeId parentNodeId = DaModelUtils.ConstructIdForDaElement(itemId, -1, namespaceIndex);
            this.AddReference(ReferenceTypeIds.HasProperty, true, parentNodeId);
        }
        #endregion Public Interface

        #region Private Fields
        private string m_itemId;
        private DaProperty m_property;
        #endregion Private Fields
    }
}
