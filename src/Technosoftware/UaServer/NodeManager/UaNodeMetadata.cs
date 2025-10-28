#region Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is subject to the Technosoftware GmbH Software License 
// Agreement, which can be found here:
// https://technosoftware.com/documents/Source_License_Agreement.pdf
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using System.Collections.Generic;

using Opc.Ua;
#endregion

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Stores metadata required to process requests related to a node.
    /// </summary>
    public class UaNodeMetadata
    {
        #region Constructors
        /// <summary>
        /// Initializes the object with its handle and NodeId.
        /// </summary>
        public UaNodeMetadata(object handle, NodeId nodeId)
        {
            m_handle = handle;
            m_nodeId = nodeId;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// The handle assigned by the NodeManager that owns the Node.
        /// </summary>
        public object Handle
        {
            get { return m_handle; }
        }

        /// <summary>
        /// The canonical NodeId for the Node.
        /// </summary>
        public NodeId NodeId
        {
            get { return m_nodeId; }
        }

        /// <summary>
        /// The NodeClass for the Node.
        /// </summary>
        public NodeClass NodeClass
        {
            get { return m_nodeClass; }
            set { m_nodeClass = value; }
        }

        /// <summary>
        /// The BrowseName for the Node.
        /// </summary>
        public QualifiedName BrowseName
        {
            get { return m_browseName; }
            set { m_browseName = value; }
        }

        /// <summary>
        /// The DisplayName for the Node.
        /// </summary>
        public LocalizedText DisplayName
        {
            get { return m_displayName; }
            set { m_displayName = value; }
        }

        /// <summary>
        /// The type definition for the Node (if one exists).
        /// </summary>
        public ExpandedNodeId TypeDefinition
        {
            get { return m_typeDefinition; }
            set { m_typeDefinition = value; }
        }

        /// <summary>
        /// The modelling for the Node (if one exists).
        /// </summary>
        public NodeId ModellingRule
        {
            get { return m_modellingRule; }
            set { m_modellingRule = value; }
        }

        /// <summary>
        /// Specifies which attributes are writeable.
        /// </summary>
        public AttributeWriteMask WriteMask
        {
            get { return m_writeMask; }
            set { m_writeMask = value; }
        }

        /// <summary>
        /// Whether the Node can be used with event subscriptions or for historical event queries.
        /// </summary>
        public byte EventNotifier
        {
            get { return m_eventNotifier; }
            set { m_eventNotifier = value; }
        }

        /// <summary>
        /// Whether the Node can be use to read or write current or historical values.
        /// </summary>
        public byte AccessLevel
        {
            get { return m_accessLevel; }
            set { m_accessLevel = value; }
        }

        /// <summary>
        /// Whether the Node is a Method that can be executed.
        /// </summary>
        public bool Executable
        {
            get { return m_executable; }
            set { m_executable = value; }
        }

        /// <summary>
        /// The DataType of the Value attribute for Variable or VariableType nodes.
        /// </summary>
        public NodeId DataType
        {
            get { return m_dataType; }
            set { m_dataType = value; }
        }

        /// <summary>
        /// The ValueRank for the Value attribute for Variable or VariableType nodes.
        /// </summary>
        public int ValueRank
        {
            get { return m_valueRank; }
            set { m_valueRank = value; }
        }

        /// <summary>
        /// The ArrayDimensions for the Value attribute for Variable or VariableType nodes.
        /// </summary>
        public IList<uint> ArrayDimensions
        {
            get { return m_arrayDimensions; }
            set { m_arrayDimensions = value; }
        }

        /// <summary>
        /// Specifies the AccessRestrictions that apply to a Node.
        /// </summary>
        public AccessRestrictionType AccessRestrictions
        {
            get { return m_accessRestrictions; }
            set { m_accessRestrictions = value; }
        }

        /// <summary>
        /// The value reflects the DefaultAccessRestrictions Property of the NamespaceMetadata Object for the Namespace
        /// to which the Node belongs.
        /// </summary>
        public AccessRestrictionType DefaultAccessRestrictions
        {
            get { return m_defaultAccessRestrictions; }
            set { m_defaultAccessRestrictions = value; }
        }

        /// <summary>
        /// The RolePermissions for the Node.
        /// Specifies the Permissions that apply to a Node for all Roles which have access to the Node.
        /// </summary>
        public RolePermissionTypeCollection RolePermissions
        {
            get { return m_rolePermissions; }
            set { m_rolePermissions = value; }
        }

        /// <summary>
        /// The DefaultRolePermissions of the Node's name-space meta-data
        /// The value reflects the DefaultRolePermissions Property from the NamespaceMetadata Object associated with the Node.
        /// </summary>
        public RolePermissionTypeCollection DefaultRolePermissions
        {
            get { return m_defaultRolePermissions; }
            set { m_defaultRolePermissions = value; }
        }

        /// <summary>
        /// The UserRolePermissions of the Node.
        /// Specifies the Permissions that apply to a Node for all Roles granted to current Session.
        /// </summary>
        public RolePermissionTypeCollection UserRolePermissions
        {
            get { return m_userRolePermissions; }
            set { m_userRolePermissions = value; }
        }

        /// <summary>
        /// The DefaultUserRolePermissions of the Node.
        /// The value reflects the DefaultUserRolePermissions Property from the NamespaceMetadata Object associated with the Node.
        /// </summary>
        public RolePermissionTypeCollection DefaultUserRolePermissions
        {
            get { return m_defaultUserRolePermissions; }
            set { m_defaultUserRolePermissions = value; }
        }
        #endregion

        #region Private Fields
        private object m_handle;
        private NodeId m_nodeId;
        private NodeClass m_nodeClass;
        private QualifiedName m_browseName;
        private LocalizedText m_displayName;
        private ExpandedNodeId m_typeDefinition;
        private NodeId m_modellingRule;
        private AttributeWriteMask m_writeMask;
        private byte m_eventNotifier;
        private byte m_accessLevel;
        private bool m_executable;
        private NodeId m_dataType;
        private int m_valueRank;
        private IList<uint> m_arrayDimensions;
        private AccessRestrictionType m_accessRestrictions;
        private AccessRestrictionType m_defaultAccessRestrictions;
        private RolePermissionTypeCollection m_rolePermissions;
        private RolePermissionTypeCollection m_defaultRolePermissions;
        private RolePermissionTypeCollection m_userRolePermissions;
        private RolePermissionTypeCollection m_defaultUserRolePermissions;
        #endregion
    }
}