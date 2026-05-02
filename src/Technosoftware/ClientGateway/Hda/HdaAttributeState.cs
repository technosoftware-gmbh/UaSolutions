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

using Opc.Ua;

using Technosoftware.ClientGateway.Da;
using Technosoftware.Common;
using Technosoftware.Rcw;

#endregion Using Directives

namespace Technosoftware.ClientGateway.Hda
{
    /// <summary>
    /// A object which maps a COM DA item to a UA variable.
    /// </summary>
    internal partial class HdaAttributeState : PropertyState
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DaItemState"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="itemId">The item id.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="namespaceIndex">Index of the namespace.</param>
        internal HdaAttributeState(
            ComHdaClientConfiguration configuration,
            string itemId,
            HdaAttribute attribute,
            ushort namespaceIndex)
        :
            base(null)
        {
            m_itemId = itemId;
            m_attribute = attribute;

            this.NodeId = HdaModelUtils.ConstructIdForHdaItemAttribute(itemId, attribute.Id, namespaceIndex);
            this.SymbolicName = attribute.Id.ToString();
            this.Description = attribute.Description;
            this.AccessLevel = AccessLevels.CurrentRead;
            this.UserAccessLevel = AccessLevels.CurrentRead;
            this.MinimumSamplingInterval = MinimumSamplingIntervals.Indeterminate;
            this.Historizing = false;
            this.ReferenceTypeId = Opc.Ua.ReferenceTypeIds.HasProperty;
            this.TypeDefinitionId = Opc.Ua.VariableTypeIds.PropertyType;
            this.Value = null;
            this.StatusCode = StatusCodes.BadWaitingForInitialData;

            bool isConfigItem = false;

            // handle built-in properties.
            switch (attribute.Id)
            {
                default:
                {
                    bool isArray = false;
                    this.BrowseName = new QualifiedName(this.SymbolicName, namespaceIndex);
                    this.DisplayName = attribute.Name;
                    this.DataType = ComUtils.GetDataTypeId(attribute.DataType, out isArray);
                    this.ValueRank = (isArray) ? ValueRanks.OneDimension : ValueRanks.Scalar;
                    break;
                }

                case Constants.OPCHDA_ENG_UNITS:
                {
                    this.BrowseName = Opc.Ua.BrowseNames.EngineeringUnits;
                    this.DisplayName = this.BrowseName.Name;
                    this.DataType = Opc.Ua.DataTypeIds.EUInformation;
                    this.ValueRank = ValueRanks.Scalar;
                    break;
                }

                case Constants.OPCHDA_NORMAL_MAXIMUM:
                {
                    this.BrowseName = Opc.Ua.BrowseNames.EURange;
                    this.DisplayName = this.BrowseName.Name;
                    this.DataType = Opc.Ua.DataTypeIds.Range;
                    this.ValueRank = ValueRanks.Scalar;
                    break;
                }

                case Constants.OPCHDA_HIGH_ENTRY_LIMIT:
                {
                    this.BrowseName = Opc.Ua.BrowseNames.InstrumentRange;
                    this.DisplayName = this.BrowseName.Name;
                    this.DataType = Opc.Ua.DataTypeIds.Range;
                    this.ValueRank = ValueRanks.Scalar;
                    break;
                }

                case Constants.OPCHDA_STEPPED:
                {
                    this.BrowseName = Opc.Ua.BrowseNames.Stepped;
                    this.DisplayName = this.BrowseName.Name;
                    this.DataType = Opc.Ua.DataTypeIds.Boolean;
                    this.ValueRank = ValueRanks.Scalar;
                    isConfigItem = true;
                    break;
                }

                case Constants.OPCHDA_DERIVE_EQUATION:
                {
                    this.BrowseName = Opc.Ua.BrowseNames.Definition;
                    this.DisplayName = this.BrowseName.Name;
                    this.DataType = Opc.Ua.DataTypeIds.String;
                    this.ValueRank = ValueRanks.Scalar;
                    isConfigItem = true;
                    break;
                }

                case Constants.OPCHDA_MIN_TIME_INT:
                {
                    this.BrowseName = Opc.Ua.BrowseNames.MinTimeInterval;
                    this.DisplayName = this.BrowseName.Name;
                    this.DataType = Opc.Ua.DataTypeIds.Duration;
                    this.ValueRank = ValueRanks.Scalar;
                    isConfigItem = true;
                    break;
                }

                case Constants.OPCHDA_MAX_TIME_INT:
                {
                    this.BrowseName = Opc.Ua.BrowseNames.MaxTimeInterval;
                    this.DisplayName = this.BrowseName.Name;
                    this.DataType = Opc.Ua.DataTypeIds.Duration;
                    this.ValueRank = ValueRanks.Scalar;
                    isConfigItem = true;
                    break;
                }

                case Constants.OPCHDA_EXCEPTION_DEV:
                {
                    this.BrowseName = Opc.Ua.BrowseNames.ExceptionDeviation;
                    this.DisplayName = this.BrowseName.Name;
                    this.DataType = Opc.Ua.DataTypeIds.Double;
                    this.ValueRank = ValueRanks.Scalar;
                    isConfigItem = true;
                    break;
                }

                case Constants.OPCHDA_EXCEPTION_DEV_TYPE:
                {
                    this.BrowseName = Opc.Ua.BrowseNames.ExceptionDeviationFormat;
                    this.DisplayName = this.BrowseName.Name;
                    this.DataType = Opc.Ua.DataTypeIds.ExceptionDeviationFormat;
                    this.ValueRank = ValueRanks.Scalar;
                    isConfigItem = true;
                    break;
                }
            }

            // set the parent id.
            NodeId parentId = null;

            if (isConfigItem)
            {
                parentId = HdaParsedNodeId.Construct(HdaModelUtils.HdaItemConfiguration, itemId, null, namespaceIndex);
            }
            else
            {
                parentId = HdaModelUtils.ConstructIdForHdaItem(itemId, namespaceIndex);
            }

            this.AddReference(ReferenceTypeIds.HasProperty, true, parentId);
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
        /// Gets the attribute.
        /// </summary>
        /// <value>The attribute.</value>
        public HdaAttribute Attribute
        {
            get
            {
                return m_attribute;
            }
        }
        #endregion Public Interface

        #region Private Fields
        private string m_itemId;
        private HdaAttribute m_attribute;
        #endregion Private Fields
    }
}
