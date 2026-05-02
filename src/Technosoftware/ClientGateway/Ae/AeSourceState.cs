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

#endregion Using Directives

namespace Technosoftware.ClientGateway.Ae
{
    /// <summary>
    /// A object which maps a segment to a UA object.
    /// </summary>
    internal partial class AeSourceState : BaseObjectState
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AeSourceState"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="areaId">The area id.</param>
        /// <param name="qualifiedName">The qualified name for the source.</param>
        /// <param name="name">The name of the source.</param>
        /// <param name="namespaceIndex">Index of the namespace.</param>
        public AeSourceState(
            ISystemContext context,
            string areaId,
            string qualifiedName,
            string name,
            ushort namespaceIndex)
            :
                base(null)
        {
            m_areaId = areaId;
            m_qualifiedName = qualifiedName;

            this.TypeDefinitionId = Opc.Ua.ObjectTypeIds.BaseObjectType;
            this.NodeId = AeModelUtils.ConstructIdForSource(m_areaId, name, namespaceIndex);
            this.BrowseName = new QualifiedName(name, namespaceIndex);
            this.DisplayName = this.BrowseName.Name;
            this.Description = null;
            this.WriteMask = 0;
            this.UserWriteMask = 0;
            this.EventNotifier = EventNotifiers.None;

            this.AddReference(ReferenceTypeIds.HasNotifier, true, AeModelUtils.ConstructIdForArea(m_areaId, namespaceIndex));
        }
        #endregion Constructors

        #region Public Properties
        /// <summary>
        /// Gets the qualified name for the source.
        /// </summary>
        /// <value>The qualified name for the source.</value>
        public string QualifiedName
        {
            get { return m_qualifiedName; }
        }
        #endregion Public Properties

        #region Private Fields
        private string m_areaId;
        private string m_qualifiedName;
        #endregion Private Fields
    }
}
