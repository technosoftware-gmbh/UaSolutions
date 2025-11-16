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
using System;
using System.Collections.Generic;
using System.Xml;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// The well known roles in a server.
    /// https://reference.opcfoundation.org/Core/Part3/v105/docs/4.9.2
    /// </summary>
    public class Role : IEquatable<Role>
    {
        /// <summary>
        /// The Role is allowed to browse and read non-security related Nodes only in the ServerData Object and all type Nodes.
        /// </summary>
        public static Role Anonymous { get; } =
            new Role(ObjectIds.WellKnownRole_Anonymous, BrowseNames.WellKnownRole_Anonymous);

        /// <summary>
        /// The Role is allowed to browse and read non-security related Nodes.
        /// </summary>
        public static Role AuthenticatedUser { get; } =
            new Role(
                ObjectIds.WellKnownRole_AuthenticatedUser,
                BrowseNames.WellKnownRole_AuthenticatedUser);

        /// <summary>
        /// The Role is allowed to browse, read live data, read historical data/events or subscribe to data/events.
        /// </summary>
        public static Role Observer { get; } =
            new Role(ObjectIds.WellKnownRole_Observer, BrowseNames.WellKnownRole_Observer);

        /// <summary>
        /// The Role is allowed to browse, read live data, read historical data/events or subscribe to data/events.
        /// In addition, the Session is allowed to write some live data and call some Methods.
        /// </summary>
        public static Role Operator { get; } =
            new Role(ObjectIds.WellKnownRole_Operator, BrowseNames.WellKnownRole_Operator);

        /// <summary>
        /// The Role is allowed to browse, read/write configuration data, read historical data/events, call Methods or subscribe to data/events.
        /// </summary>
        public static Role Engineer { get; } =
            new Role(ObjectIds.WellKnownRole_Engineer, BrowseNames.WellKnownRole_Engineer);

        /// <summary>
        /// The Role is allowed to browse, read live data, read historical data/events, call Methods or subscribe to data/events.
        /// </summary>
        public static Role Supervisor { get; } =
            new Role(ObjectIds.WellKnownRole_Supervisor, BrowseNames.WellKnownRole_Supervisor);

        /// <summary>
        /// The Role is allowed to change the non-security related configuration settings.
        /// </summary>
        public static Role ConfigureAdmin { get; } =
            new Role(
                ObjectIds.WellKnownRole_ConfigureAdmin,
                BrowseNames.WellKnownRole_ConfigureAdmin);

        /// <summary>
        /// The Role is allowed to change security related settings.
        /// </summary>
        public static Role SecurityAdmin { get; } =
            new Role(
                ObjectIds.WellKnownRole_SecurityAdmin,
                BrowseNames.WellKnownRole_SecurityAdmin);

        /// <summary>
        /// Constructor for new Role
        /// </summary>
        /// <param name="roleId">NodeId of the Role, used for WellKnownRoles</param>
        /// <param name="name">Name of the Role</param>
        public Role(NodeId roleId, string name)
        {
            RoleId = roleId;
            Name = name;
        }

        /// <summary>
        /// the NodeId of the role
        /// </summary>
        public NodeId RoleId { get; }

        /// <summary>
        /// the name of the role
        /// </summary>
        public string Name { get; }

        /// <inheritdoc/>
        public bool Equals(Role other)
        {
            if (other is null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (GetType() != other.GetType())
            {
                return false;
            }
            return (Name == other.Name) && (RoleId == other.RoleId);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals(obj as Role);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, RoleId);
        }

        /// <inheritdoc/>
        public static bool operator ==(Role lhs, Role rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        /// <inheritdoc/>
        public static bool operator !=(Role lhs, Role rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// returns the name of the role
        /// </summary>
        /// <returns>the name of the role</returns>
        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// The role based identity for a server.
    /// </summary>
    public class RoleBasedIdentity : IUserIdentity
    {
        private readonly IUserIdentity m_identity;

        /// <summary>
        /// Initialize the role based identity.
        /// </summary>
        public RoleBasedIdentity(IUserIdentity identity, IEnumerable<Role> roles)
        {
            m_identity = identity;
            Roles = roles;
            foreach (Role role in roles)
            {
                if (!(role.RoleId?.IsNullNodeId ?? true))
                {
                    GrantedRoleIds.Add(role.RoleId);
                }
            }
        }

        /// <inheritdoc/>
        public NodeIdCollection GrantedRoleIds => m_identity.GrantedRoleIds;

        /// <summary>
        /// The role in the context of a server.
        /// </summary>
        public IEnumerable<Role> Roles { get; }

        /// <inheritdoc/>
        public string DisplayName => m_identity.DisplayName;

        /// <inheritdoc/>
        public string PolicyId => m_identity.PolicyId;

        /// <inheritdoc/>
        public UserTokenType TokenType => m_identity.TokenType;

        /// <inheritdoc/>
        public XmlQualifiedName IssuedTokenType => m_identity.IssuedTokenType;

        /// <inheritdoc/>
        public bool SupportsSignatures => m_identity.SupportsSignatures;

        /// <inheritdoc/>
        public UserIdentityToken GetIdentityToken()
        {
            return m_identity.GetIdentityToken();
        }
    }
}
