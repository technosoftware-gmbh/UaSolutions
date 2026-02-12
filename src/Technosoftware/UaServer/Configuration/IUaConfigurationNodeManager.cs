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
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// The Server Configuration Node Manager.
    /// </summary>
    public interface IUaConfigurationNodeManager : IUaStandardNodeManager
    {
        /// <summary>
        /// Gets or creates the <see cref="NamespaceMetadataState"/> node for the specified NamespaceUri.
        /// </summary>
        NamespaceMetadataState CreateNamespaceMetadataState(string namespaceUri);

        /// <summary>
        /// Creates the configuration node for the server.
        /// </summary>
        void CreateServerConfiguration(UaServerContext systemContext, ApplicationConfiguration configuration);

        /// <summary>
        /// Gets and returns the <see cref="NamespaceMetadataState"/> node associated with the specified NamespaceUri
        /// </summary>
        NamespaceMetadataState GetNamespaceMetadataState(string namespaceUri);

        /// <summary>
        /// Determine if the impersonated user has admin access.
        /// </summary>
        /// <exception cref="ServiceResultException"/>
        /// <seealso cref="StatusCodes.BadUserAccessDenied"/>
        void HasApplicationSecureAdminAccess(ISystemContext context);

        /// <summary>
        /// Determine if the impersonated user has admin access.
        /// </summary>
        /// <exception cref="ServiceResultException"/>
        /// <seealso cref="StatusCodes.BadUserAccessDenied"/>
        void HasApplicationSecureAdminAccess(ISystemContext context, CertificateStoreIdentifier trustedStore);
    }
}
