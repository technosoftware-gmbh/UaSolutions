#region Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;

using Opc.Ua;
using Opc.Ua.Test;
using Range = Opc.Ua.Range;

using Technosoftware.UaServer;
#endregion

namespace Technosoftware.UaBaseServer
{
    /// <summary>
    /// A base implementation of the IUaNodeManager interface.
    /// </summary>
    /// <remarks>
    /// This node manager is a base class used in multiple samples. It implements the IUaNodeManager
    /// interface and allows sub-classes to override only the methods that they need.
    /// </remarks>
    public class UaBaseNodeManager : UaStandardNodeManager
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Initializes the node manager.
        /// </summary>
        /// <param name="opcServer">The UA uaServerData implementing the IUaServer interface.</param>
        /// <param name="opcServerPlugin">The UA Server plugin implementing the IUaServerPlugin interface.</param>
        /// <param name="uaServerData">The uaServerData data implementing the IUaServerData interface.</param>
        /// <param name="configuration">The used application configuration.</param>
        /// <param name="namespaceUris">Array of namespaces that are used by the application.</param>
        public UaBaseNodeManager(
            IUaServer opcServer,
            IUaServerPlugin opcServerPlugin,
            IUaServerData uaServerData,
            ApplicationConfiguration configuration,
            params string[] namespaceUris)
            : this(uaServerData, configuration, namespaceUris)
        {
            // Save generic uaServerData and plugin interface objects
            OpcServer = opcServer;
            opcServerPlugin_ = opcServerPlugin;
        }

        /// <summary>
        /// Initializes the node manager.
        /// </summary>
        /// <param name="uaServerData">The uaServerData data implementing the IUaServerData interface.</param>
        /// <param name="configuration">The used application configuration.</param>
        /// <param name="namespaceUris">Array of namespaces that are used by the application.</param>
        protected UaBaseNodeManager(
            IUaServerData uaServerData,
            ApplicationConfiguration configuration,
            params string[] namespaceUris)
        :
            base(uaServerData, configuration, namespaceUris)
        {
        }
        #endregion

        #region INodeIdFactory Members
        /// <summary>
        /// Creates the NodeId for the specified node.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="node">The node.</param>
        /// <returns>The new NodeId.</returns>
        public override NodeId Create(ISystemContext context, NodeState node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (node is BaseInstanceState instance && instance.Parent != null)
            {
                if (instance.Parent.NodeId.Identifier is string id)
                {
                    return new NodeId(id + "_" + instance.SymbolicName, instance.Parent.NodeId.NamespaceIndex);
                }
            }

            return node.NodeId;
        }
        #endregion

        #region Type Definitions
        /// <summary>
        ///     <para>Creates a new variable type.</para>
        /// </summary>
        /// <param name="parent">The parent NodeState object the new variable type will be created in.</param>
        /// <param name="externalReferences">
        ///   <para>The externalReferences is an out parameter that allows the generic server to link to nodes.</para>
        /// </param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="dataType">
        ///     The data type of the new variable type, e.g. <see cref="BuiltInType.SByte" />. See
        ///     <see cref="BuiltInType" /> for all possible types
        /// </param>
        /// <param name="valueRank">
        ///     The value rank of the new variable type, e.g. <see cref="ValueRanks.Scalar" />. See
        ///     <see cref="ValueRanks" /> for all possible value ranks.
        /// </param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.</para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <returns>The created <see cref="BaseVariableTypeState" /></returns>
        protected internal virtual BaseVariableTypeState CreateBaseVariableTypeState(NodeState parent,
            IDictionary<NodeId, IList<IReference>> externalReferences, string browseName, LocalizedText displayName,
            LocalizedText description, BuiltInType dataType, int valueRank,
            AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = [];
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = [];
            }

            var baseDataVariableTypeState = new BaseDataVariableTypeState
            {
                SymbolicName = displayName.ToString(),
                SuperTypeId = VariableTypeIds.BaseDataVariableType,
                NodeId = new NodeId(browseName, NamespaceIndex),
                BrowseName = new QualifiedName(browseName, NamespaceIndex),
                DisplayName = displayName,
                Description = description,
                WriteMask = writeMask,
                UserWriteMask = userWriteMask,
                RolePermissions = rolePermissions,
                UserRolePermissions = userRolePermissions,
                IsAbstract = false,
                DataType = (uint)dataType,
                ValueRank = valueRank,
                Value = null
            };

            if (externalReferences != null)
            {
                if (!externalReferences.TryGetValue(VariableTypeIds.BaseDataVariableType, out var references))
                {
                    externalReferences[VariableTypeIds.BaseDataVariableType] = references = [];
                }
                references.Add(new NodeStateReference(ReferenceTypes.HasSubtype, false, baseDataVariableTypeState.NodeId));
            }

            if (parent != null)
            {
                parent.AddReference(ReferenceTypes.Organizes, false, baseDataVariableTypeState.NodeId);
                baseDataVariableTypeState.AddReference(ReferenceTypes.Organizes, true, parent.NodeId);
            }

            AddPredefinedNode(SystemContext, baseDataVariableTypeState);
            return baseDataVariableTypeState;
        }

        /// <summary>
        ///   <para>Creates a new ObjectType NodeClass.</para>
        ///   <para>ObjectTypes provide definitions for Objects.</para>
        /// </summary>
        /// <param name="parent">The parent NodeState object the new ObjectType NodeClass will be created in.</param>
        /// <param name="externalReferences">
        ///   <para>The externalReferences is an out parameter that allows the generic server to link to nodes.</para>
        /// </param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.</para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <returns>The created <see cref="BaseObjectTypeState" /></returns>
        protected internal virtual BaseObjectTypeState CreateBaseObjectTypeState(NodeState parent,
            IDictionary<NodeId, IList<IReference>> externalReferences, string browseName, LocalizedText displayName,
            LocalizedText description, AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = [];
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = [];
            }

            var baseObjectTypeState = new BaseObjectTypeState
            {
                SymbolicName = displayName.ToString(),
                SuperTypeId = ObjectTypeIds.BaseObjectType,
                NodeId = new NodeId(browseName, NamespaceIndex),
                BrowseName = new QualifiedName(browseName, NamespaceIndex),
                DisplayName = displayName,
                Description = description,
                WriteMask = writeMask,
                UserWriteMask = userWriteMask,
                RolePermissions = rolePermissions,
                UserRolePermissions = userRolePermissions,
                IsAbstract = false
            };

            if (externalReferences != null)
            {
                if (!externalReferences.TryGetValue(ObjectTypeIds.BaseObjectType, out var references))
                {
                    externalReferences[ObjectTypeIds.BaseObjectType] = references = [];
                }
                references.Add(new NodeStateReference(ReferenceTypes.HasSubtype, false, baseObjectTypeState.NodeId));
            }

            if (parent != null)
            {
                parent.AddReference(ReferenceTypes.Organizes, false, baseObjectTypeState.NodeId);
                baseObjectTypeState.AddReference(ReferenceTypes.Organizes, true, parent.NodeId);
            }

            AddPredefinedNode(SystemContext, baseObjectTypeState);
            return baseObjectTypeState;
        }

        /// <summary>Creates a new data type.</summary>
        /// <param name="parent">The parent NodeState object the new data type will be created in.</param>
        /// <param name="externalReferences">
        ///   <para>The externalReferences is an out parameter that allows the generic server to link to nodes.</para>
        /// </param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.</para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <returns>The created data type object</returns>
        protected internal virtual DataTypeState CreateDataTypeState(NodeState parent,
            IDictionary<NodeId, IList<IReference>> externalReferences, string browseName, LocalizedText displayName,
            LocalizedText description, AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = [];
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = [];
            }

            var type = new DataTypeState
            {
                SymbolicName = displayName.ToString(),
                SuperTypeId = DataTypeIds.Structure,
                NodeId = new NodeId(browseName, NamespaceIndex),
                BrowseName = new QualifiedName(browseName, NamespaceIndex),
                DisplayName = displayName,
                Description = description,
                WriteMask = writeMask,
                UserWriteMask = userWriteMask,
                RolePermissions = rolePermissions,
                UserRolePermissions = userRolePermissions,
                IsAbstract = false
            };

            if (externalReferences != null)
            {
                if (!externalReferences.TryGetValue(DataTypeIds.Structure, out var references))
                {
                    externalReferences[DataTypeIds.Structure] = references = [];
                }
                references.Add(new NodeStateReference(ReferenceTypeIds.HasSubtype, false, type.NodeId));
            }

            if (parent != null)
            {
                parent.AddReference(ReferenceTypes.Organizes, false, type.NodeId);
                type.AddReference(ReferenceTypes.Organizes, true, parent.NodeId);
            }

            AddPredefinedNode(SystemContext, type);
            return type;
        }

        /// <summary>
        ///     <para>Creates a new reference type.</para>
        /// </summary>
        /// <param name="parent">The parent NodeState object the new reference type will be created in.</param>
        /// <param name="externalReferences">
        ///   <para>The externalReferences is an out parameter that allows the generic server to link to nodes.</para>
        /// </param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <returns>The created <see cref="ReferenceTypeState" /></returns>
        internal ReferenceTypeState CreateReferenceTypeState(NodeState parent,
            IDictionary<NodeId, IList<IReference>> externalReferences, string browseName, LocalizedText displayName)
        {
            var type = new ReferenceTypeState
            {
                SymbolicName = displayName.ToString(),
                SuperTypeId = ReferenceTypeIds.NonHierarchicalReferences,
                NodeId = new NodeId(browseName, NamespaceIndex),
                BrowseName = new QualifiedName(browseName, NamespaceIndex),
                DisplayName = displayName,
                WriteMask = AttributeWriteMask.None,
                UserWriteMask = AttributeWriteMask.None,
                IsAbstract = false,
                Symmetric = true,
                InverseName = displayName
            };

            if (!externalReferences.TryGetValue(ReferenceTypeIds.NonHierarchicalReferences, out var references))
            {
                externalReferences[ReferenceTypeIds.NonHierarchicalReferences] = references = [];
            }

            references.Add(new NodeStateReference(ReferenceTypeIds.HasSubtype, false, type.NodeId));

            if (parent != null)
            {
                parent.AddReference(ReferenceTypes.Organizes, false, type.NodeId);
                type.AddReference(ReferenceTypes.Organizes, true, parent.NodeId);
            }

            AddPredefinedNode(SystemContext, type);
            return type;
        }
        #endregion

        #region Internal properties
        internal IUaServer OpcServer { get; }
        #endregion

        #region Private Fields
        private readonly IUaServerPlugin opcServerPlugin_;
        #endregion
    }
}
