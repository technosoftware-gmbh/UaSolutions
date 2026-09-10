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
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Opc.Ua.Security.Certificates;
using System.Security.Cryptography;
using System.Diagnostics;
using Opc.Ua;
using Opc.Ua.Redaction;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// The Server Configuration Node Manager.
    /// </summary>
    public class ConfigurationNodeManager : DiagnosticsNodeManager, IUaCallAsyncNodeManager, IUaConfigurationNodeManager
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Initializes the configuration and diagnostics manager.
        /// </summary>
        public ConfigurationNodeManager(
            IUaServerData server,
            ApplicationConfiguration configuration)
            : this(server, configuration, server.Telemetry.CreateLogger<ConfigurationNodeManager>())
        {
        }

        /// <summary>
        /// Initializes the configuration and diagnostics manager.
        /// </summary>
        public ConfigurationNodeManager(
            IUaServerData server,
            ApplicationConfiguration configuration,
            ILogger logger)
            : base(server, configuration, logger)
        {
            string rejectedStorePath = configuration.SecurityConfiguration.RejectedCertificateStore?
                .StorePath;
            if (!string.IsNullOrEmpty(rejectedStorePath))
            {
                m_rejectedStore = new CertificateStoreIdentifier(rejectedStorePath);
            }
            m_certificateGroups = [];
            m_configuration = configuration;
            // TODO: configure cert groups in configuration
            var defaultApplicationGroup = new ServerCertificateGroup
            {
                NodeId = ObjectIds.ServerConfiguration_CertificateGroups_DefaultApplicationGroup,
                BrowseName = BrowseNames.DefaultApplicationGroup,
                CertificateTypes = [],
                ApplicationCertificates = [],
                IssuerStore = new CertificateStoreIdentifier(
                    configuration.SecurityConfiguration.TrustedIssuerCertificates.StorePath
                ),
                TrustedStore = new CertificateStoreIdentifier(
                    configuration.SecurityConfiguration.TrustedPeerCertificates.StorePath)
            };
            m_certificateGroups.Add(defaultApplicationGroup);

            if (configuration.SecurityConfiguration.UserIssuerCertificates != null &&
                configuration.SecurityConfiguration.TrustedUserCertificates != null)
            {
                var defaultUserGroup = new ServerCertificateGroup
                {
                    NodeId = ObjectIds.ServerConfiguration_CertificateGroups_DefaultUserTokenGroup,
                    BrowseName = BrowseNames.DefaultUserTokenGroup,
                    CertificateTypes = [],
                    ApplicationCertificates = [],
                    IssuerStore = new CertificateStoreIdentifier(
                        configuration.SecurityConfiguration.UserIssuerCertificates.StorePath
                    ),
                    TrustedStore = new CertificateStoreIdentifier(
                        configuration.SecurityConfiguration.TrustedUserCertificates.StorePath)
                };

                m_certificateGroups.Add(defaultUserGroup);
            }
            ServerCertificateGroup defaultHttpsGroup = null;
            if (configuration.SecurityConfiguration.HttpsIssuerCertificates != null &&
                configuration.SecurityConfiguration.TrustedHttpsCertificates != null)
            {
                defaultHttpsGroup = new ServerCertificateGroup
                {
                    NodeId = ObjectIds.ServerConfiguration_CertificateGroups_DefaultHttpsGroup,
                    BrowseName = BrowseNames.DefaultHttpsGroup,
                    CertificateTypes = [],
                    ApplicationCertificates = [],
                    IssuerStore = new CertificateStoreIdentifier(
                        configuration.SecurityConfiguration.HttpsIssuerCertificates.StorePath
                    ),
                    TrustedStore = new CertificateStoreIdentifier(
                        configuration.SecurityConfiguration.TrustedHttpsCertificates.StorePath)
                };

                m_certificateGroups.Add(defaultHttpsGroup);
            }

            // For each certificate in ApplicationCertificates, add the certificate type to ServerConfiguration_CertificateGroups_DefaultApplicationGroup
            // under the CertificateTypes field.
            foreach (CertificateIdentifier cert in configuration.SecurityConfiguration
                .ApplicationCertificates)
            {
                defaultApplicationGroup.CertificateTypes =
                [
                    .. defaultApplicationGroup.CertificateTypes,
                    .. new NodeId[] { cert.CertificateType }
                ];
                defaultApplicationGroup.ApplicationCertificates.Add(cert);

                if (cert.CertificateType == ObjectTypeIds.HttpsCertificateType &&
                    defaultHttpsGroup != null)
                {
                    defaultHttpsGroup.CertificateTypes =
                    [
                        .. defaultHttpsGroup.CertificateTypes,
                        .. new NodeId[] { cert.CertificateType }
                    ];
                    defaultHttpsGroup.ApplicationCertificates.Add(cert);
                }
            }
        }
        #endregion Constructors, Destructor, Initialization

        #region IUaNodeManager Members
        /// <summary>
        /// Replaces the generic node with a node specific to the model.
        /// </summary>
        protected override NodeState AddBehaviourToPredefinedNode(
            ISystemContext context,
            NodeState predefinedNode)
        {
            if (predefinedNode is BaseObjectState passiveNode)
            {
                NodeId typeId = passiveNode.TypeDefinitionId;
                if (IsNodeIdInNamespace(typeId) && typeId.IdType == IdType.Numeric)
                {
                    switch ((uint)typeId.Identifier)
                    {
                        case ObjectTypes.ServerConfigurationType:
                        {
                            var activeNode = new ServerConfigurationState(passiveNode.Parent);

                            activeNode.GetCertificates = new GetCertificatesMethodState(activeNode);

                            activeNode.Create(context, passiveNode);

                            m_serverConfigurationNode = activeNode;

                            // replace the node in the parent.
                            if (passiveNode.Parent != null)
                            {
                                passiveNode.Parent.ReplaceChild(context, activeNode);
                            }
                            else
                            {
                                NodeState serverNode = FindNodeInAddressSpace(ObjectIds.Server);
                                serverNode?.ReplaceChild(context, activeNode);
                            }
                            // remove the reference to server node because it is set as parent
                            activeNode.RemoveReference(
                                ReferenceTypeIds.HasComponent,
                                true,
                                ObjectIds.Server);

                            return activeNode;
                        }
                        case ObjectTypes.CertificateGroupFolderType:
                        {
                            // The standard address space carries
                            // CertificateGroupFolderType instances under several
                            // types. Only the server's own certificate groups
                            // folder is managed here; the others keep the
                            // structure they were built with.
                            if (passiveNode.NodeId !=
                                ObjectIds.ServerConfiguration_CertificateGroups)
                            {
                                break;
                            }

                            // The node arrives fully built from the generated
                            // model, so it is taken as it is rather than being
                            // recreated. DefaultApplicationGroup is mandatory;
                            // the HTTPS and user token groups are optional and
                            // exist only once they are added, which is what
                            // replaces the 1.5 code that deleted the groups the
                            // configuration did not ask for.
                            var activeNode = (CertificateGroupFolderState)passiveNode;

                            ServerCertificateGroup applicationGroup = m_certificateGroups
                                .FirstOrDefault(group =>
                                    group.BrowseName == BrowseNames.DefaultApplicationGroup);
                            if (applicationGroup != null)
                            {
                                applicationGroup.Node = activeNode.DefaultApplicationGroup;
                            }

                            ServerCertificateGroup httpsGroup = m_certificateGroups
                                .FirstOrDefault(group =>
                                    group.BrowseName == BrowseNames.DefaultHttpsGroup);
                            if (httpsGroup != null)
                            {
                                activeNode.AddDefaultHttpsGroup(context);
                                httpsGroup.Node = activeNode.DefaultHttpsGroup;
                            }

                            ServerCertificateGroup userTokenGroup = m_certificateGroups
                                .FirstOrDefault(group =>
                                    group.BrowseName == BrowseNames.DefaultUserTokenGroup);
                            if (userTokenGroup != null)
                            {
                                activeNode.AddDefaultUserTokenGroup(context);
                                userTokenGroup.Node = activeNode.DefaultUserTokenGroup;
                            }

                            return activeNode;
                        }
                    }
                }
            }
            return base.AddBehaviourToPredefinedNode(context, predefinedNode);
        }
        #endregion IUaNodeManager Members

        #region Public methods
        ///<inheritdoc/>
        public void CreateServerConfiguration(
            UaServerContext systemContext,
            ApplicationConfiguration configuration)
        {
            // setup server configuration node
            m_serverConfigurationNode.ServerCapabilities.Value =
            [
                .. configuration.ServerConfiguration.ServerCapabilities
            ];
            m_serverConfigurationNode.ServerCapabilities.ValueRank = ValueRanks.OneDimension;
            m_serverConfigurationNode.ServerCapabilities.ArrayDimensions
                = [0];
            m_serverConfigurationNode.SupportedPrivateKeyFormats.Value =
            [
                .. configuration.ServerConfiguration.SupportedPrivateKeyFormats
            ];
            m_serverConfigurationNode.SupportedPrivateKeyFormats.ValueRank = ValueRanks
                .OneDimension;
            m_serverConfigurationNode.SupportedPrivateKeyFormats.ArrayDimensions
                = [0];
            m_serverConfigurationNode.MaxTrustListSize.Value = (uint)configuration
                .ServerConfiguration
                .MaxTrustListSize;
            m_serverConfigurationNode.MulticastDnsEnabled.Value = configuration.ServerConfiguration
                .MultiCastDnsEnabled;

            m_serverConfigurationNode.UpdateCertificate.OnCallAsync
                = new UpdateCertificateMethodStateMethodAsyncCallHandler(
                UpdateCertificateAsync);
            m_serverConfigurationNode.CreateSigningRequest.OnCallAsync =
                new CreateSigningRequestMethodStateMethodAsyncCallHandler(CreateSigningRequestAsync);
            m_serverConfigurationNode.ApplyChanges.OnCallMethod2
                = new GenericMethodCalledEventHandler2(ApplyChanges);
            m_serverConfigurationNode.GetRejectedList.OnCall
                = new GetRejectedListMethodStateMethodCallHandler(
                GetRejectedList);
            m_serverConfigurationNode.GetCertificates.OnCall
                = new GetCertificatesMethodStateMethodCallHandler(
                GetCertificates);
            m_serverConfigurationNode.ClearChangeMasks(systemContext, true);

            // setup certificate group trust list handlers
            foreach (ServerCertificateGroup certGroup in m_certificateGroups)
            {
                certGroup.Node.CertificateTypes.Value = certGroup.CertificateTypes;
                certGroup.Node.TrustList.Handle = new TrustList(
                    certGroup.Node.TrustList,
                    certGroup.TrustedStore,
                    certGroup.IssuerStore,
                    new TrustList.SecureAccess(HasApplicationSecureAdminAccess),
                    new TrustList.SecureAccess(HasApplicationSecureAdminAccess),
                    ServerData.Telemetry,
                    m_configuration.ServerConfiguration.MaxTrustListSize);
                certGroup.Node.ClearChangeMasks(systemContext, true);
            }

            // find ServerNamespaces node and subscribe to StateChanged

            if (FindPredefinedNode<NamespacesState>(ObjectIds.Server_Namespaces)
                is NamespacesState serverNamespacesNode)
            {
                serverNamespacesNode.StateChanged += ServerNamespacesChanged;
            }
        }

        ///<inheritdoc/>
        public NamespaceMetadataState GetNamespaceMetadataState(string namespaceUri)
        {
            if (namespaceUri == null)
            {
                return null;
            }

            if (m_namespaceMetadataStates.TryGetValue(
                namespaceUri,
                out NamespaceMetadataState value))
            {
                return value;
            }

            NamespaceMetadataState namespaceMetadataState = FindNamespaceMetadataState(
                namespaceUri);

            lock (Lock)
            {
                // remember the result for faster access.
                m_namespaceMetadataStates[namespaceUri] = namespaceMetadataState;
            }

            return namespaceMetadataState;
        }

        /// <inheritdoc/>
        public NamespaceMetadataState CreateNamespaceMetadataState(string namespaceUri)
        {
            NamespaceMetadataState namespaceMetadataState = FindNamespaceMetadataState(
                namespaceUri);

            if (namespaceMetadataState == null)
            {
                // find ServerNamespaces node
                if (FindPredefinedNode<NamespacesState>(ObjectIds.Server_Namespaces)
                    is not NamespacesState serverNamespacesNode)
                {
                    m_logger.LogError(
                        "Cannot create NamespaceMetadataState for namespace '{NamespaceUri}'.",
                        namespaceUri);
                    return null;
                }

                // create the NamespaceMetadata node
                namespaceMetadataState = new NamespaceMetadataState(serverNamespacesNode)
                {
                    BrowseName = new QualifiedName(namespaceUri, NamespaceIndex)
                };
                namespaceMetadataState.Create(
                    SystemContext,
                    default,
                    namespaceMetadataState.BrowseName,
                    default,
                    true);
                namespaceMetadataState.DisplayName = new LocalizedText(namespaceUri);
                namespaceMetadataState.SymbolicName = namespaceUri;
                namespaceMetadataState.NamespaceUri.Value = namespaceUri;

                // add node as child of ServerNamespaces and in predefined nodes
                serverNamespacesNode.AddChild(namespaceMetadataState);
                serverNamespacesNode.ClearChangeMasks(ServerData.DefaultSystemContext, true);
                AddPredefinedNode(SystemContext, namespaceMetadataState);
            }

            return namespaceMetadataState;
        }

        /// <inheritdoc/>
        public void HasApplicationSecureAdminAccess(ISystemContext context)
        {
            HasApplicationSecureAdminAccess(context, null);
        }

        /// <inheritdoc/>
        public void HasApplicationSecureAdminAccess(
            ISystemContext context,
            CertificateStoreIdentifier trustedStore)
        {
            if (context is SessionSystemContext { OperationContext: UaServerOperationContext operationContext })
            {
                if (operationContext.ChannelContext?.EndpointDescription?.SecurityMode !=
                    MessageSecurityMode.SignAndEncrypt)
                {
                    throw new ServiceResultException(
                        StatusCodes.BadUserAccessDenied,
                        "Access to this item is only allowed with MessageSecurityMode SignAndEncrypt.");
                }
                IUserIdentity identity = operationContext.UserIdentity;
                // allow access to system configuration only with Role SecurityAdmin
                if (identity == null ||
                    identity.TokenType == UserTokenType.Anonymous ||
                    !identity.GrantedRoleIds.Contains(ObjectIds.WellKnownRole_SecurityAdmin))
                {
                    throw new ServiceResultException(
                        StatusCodes.BadUserAccessDenied,
                        "Security Admin Role required to access this item.");
                }
            }
        }
        #endregion Public methods

        #region Private Methods
        private async ValueTask<UpdateCertificateMethodStateResult> UpdateCertificateAsync(
            ISystemContext context,
            MethodState method,
            NodeId objectId,
            NodeId certificateGroupId,
            NodeId certificateTypeId,
            ByteString certificate,
            ArrayOf<ByteString> issuerCertificates,
            string privateKeyFormat,
            ByteString privateKey,
            CancellationToken ct)
        {
            bool applyChangesRequired = false;
            HasApplicationSecureAdminAccess(context);

            ArrayOf<Variant> inputArguments =
            [
                Variant.From(certificateGroupId),
                Variant.From(certificateTypeId),
                Variant.From(certificate),
                Variant.From(issuerCertificates),
                Variant.From(privateKeyFormat),
                Variant.From(privateKey)
            ];
            Certificate newCert = null;
            Certificate certWithPrivateKey = null;

            ServerData.ReportCertificateUpdateRequestedAuditEvent(
                context,
                objectId,
                method,
                inputArguments,
                m_logger);
            try
            {
                if (certificate.IsEmpty)
                {
                    throw new ArgumentException(
                        "The certificate is empty.",
                        nameof(certificate));
                }

                privateKeyFormat = privateKeyFormat?.ToUpperInvariant();
                if (privateKeyFormat is not null and not "PEM" and not "PFX" and not "")
                {
                    throw new ServiceResultException(
                        StatusCodes.BadNotSupported,
                        $"The private key format {privateKeyFormat} is not supported.");
                }

                ServerCertificateGroup certificateGroup = VerifyGroupAndTypeId(
                    certificateGroupId,
                    certificateTypeId);
                certificateGroup.UpdateCertificate = null;

                try
                {
                    newCert = DefaultCertificateFactory.Instance.CreateFromRawData(certificate);
                }
                catch
                {
                    throw new ServiceResultException(
                        StatusCodes.BadCertificateInvalid,
                        "Certificate data is invalid.");
                }

                // validate certificate type of new certificate
                if (!CertificateIdentifier.ValidateCertificateType(newCert, certificateTypeId))
                {
                    throw new ServiceResultException(
                        StatusCodes.BadCertificateInvalid,
                        "Certificate type of new certificate doesn't match the provided certificate type.");
                }

                // identify the existing certificate to be updated
                // it should be of the same type and same subject name as the new certificate
                // CertificateIdentifier.Certificate is gone in 2.0 - resolving
                // an identifier is asynchronous and needs a registry - so the
                // fallback for a subject that changed mid-rotation matches on
                // the certificate type alone, as upstream does, rather than by
                // reading each identifier's certificate for its application URI.
                CertificateIdentifier existingCertIdentifier =
                    (
                        certificateGroup.ApplicationCertificates.FirstOrDefault(cert =>
                            X509Utils.CompareDistinguishedName(cert.SubjectName, newCert.Subject) &&
                            cert.CertificateType == certificateTypeId)
                        ?? certificateGroup.ApplicationCertificates.FirstOrDefault(cert =>
                            cert.CertificateType == certificateTypeId))
                    ?? throw new ServiceResultException(
                        StatusCodes.BadInvalidArgument,
                        "No existing certificate found for the specified certificate type and subject name.");

                var newIssuerCollection = new CertificateCollection();

                try
                {
                    // build issuer chain
                    foreach (ByteString issuerRawCert in issuerCertificates)
                    {
                        using var issuer = Certificate.FromRawData(issuerRawCert);
                        newIssuerCollection.Add(issuer);
                    }
                }
                catch
                {
                    throw new ServiceResultException(
                        StatusCodes.BadCertificateInvalid,
                        "Issuer certificate data is invalid.");
                }

                // self signed
                bool selfSigned = X509Utils.IsSelfSigned(newCert);
                if (selfSigned && newIssuerCollection.Count != 0)
                {
                    throw new ServiceResultException(
                        StatusCodes.BadCertificateInvalid,
                        "Issuer list not empty for self signed certificate.");
                }

                if (!selfSigned)
                {
                    try
                    {
                        await ValidatePushCertificateAndIssuerChainAsync(
                            newCert,
                            newIssuerCollection,
                            m_configuration.SecurityConfiguration,
                            ServerData.Telemetry,
                            ct).ConfigureAwait(false);
                    }
                    catch (ServiceResultException)
                    {
                        // the validator already reports the specific status
                        // code; do not flatten it to BadSecurityChecksFailed.
                        throw;
                    }
                    catch (Exception ex)
                    {
                        m_logger.LogError(
                            Utils.TraceMasks.Security,
                            ex,
                            "Failed to verify integrity of the new certificate {Certificate} and the issuer list.",
                            Redact.Create(newCert));
                        throw new ServiceResultException(
                            StatusCodes.BadSecurityChecksFailed,
                            "Failed to verify integrity of the new certificate and the issuer list.",
                            ex);
                    }
                }

                var updateCertificate = new UpdateCertificateData
                {
                    IssuerCollection = newIssuerCollection,
                    SessionId = (context as ISessionSystemContext)?.SessionId ?? default
                };
                try
                {
                    ICertificatePasswordProvider passwordProvider = m_configuration
                        .SecurityConfiguration
                        .CertificatePasswordProvider;
                    switch (privateKeyFormat)
                    {
                        case null:
                        case "":
                            for (int attempt = 0; ; attempt++)
                            {
                                Certificate exportableKey;
                                // use the new generated private key if one exists and matches the provided public key
                                if (certificateGroup.TemporaryApplicationCertificate != null &&
                                    X509Utils.VerifyKeyPair(
                                        newCert,
                                        certificateGroup.TemporaryApplicationCertificate))
                                {
                                    exportableKey = X509Utils.CreateCopyWithPrivateKey(
                                        certificateGroup.TemporaryApplicationCertificate,
                                        false);
                                }
                                else
                                {
                                    certWithPrivateKey = await CertificateIdentifierResolver
                                        .LoadPrivateKeyAsync(
                                            existingCertIdentifier,
                                            passwordProvider,
                                            m_configuration.ApplicationUri,
                                            ServerData.Telemetry,
                                            ct)
                                        .ConfigureAwait(false);
                                    if (certWithPrivateKey == null)
                                    {
                                        throw new ServiceResultException(
                                            StatusCodes.BadSecurityChecksFailed,
                                            "A private key was not found");
                                    }
                                    exportableKey = X509Utils.CreateCopyWithPrivateKey(
                                        certWithPrivateKey,
                                        false);
                                }

                                updateCertificate.CertificateWithPrivateKey =
                                    s_certificateFactory.CreateWithPrivateKey(
                                        newCert,
                                        exportableKey);
                                try
                                {
                                    await UpdateCertificateInternalAsync(
                                        certificateGroup,
                                        existingCertIdentifier,
                                        updateCertificate, ct).ConfigureAwait(false);
                                    break;
                                }
                                catch (Exception ex) when (ShouldRetry(attempt, ex))
                                {
                                    m_logger.LogDebug(
                                        Utils.TraceMasks.Security,
                                        ex,
                                        "Failed to update certificate {Certificate}. Retrying...",
                                        Redact.Create(newCert));
                                }
                            }
                            break;
                        case "PFX":
                            for (int attempt = 0; ; attempt++)
                            {
                                certWithPrivateKey = X509Utils.CreateCertificateFromPKCS12(
                                    privateKey.ToArray(),
                                    passwordProvider?.GetPassword(existingCertIdentifier),
                                    false);

                                updateCertificate.CertificateWithPrivateKey =
                                    s_certificateFactory.CreateWithPrivateKey(
                                        newCert,
                                        certWithPrivateKey);
                                try
                                {
                                    await UpdateCertificateInternalAsync(
                                        certificateGroup,
                                        existingCertIdentifier,
                                        updateCertificate, ct).ConfigureAwait(false);
                                    break;
                                }
                                catch (Exception ex) when (ShouldRetry(attempt, ex))
                                {
                                    m_logger.LogDebug(
                                        Utils.TraceMasks.Security,
                                        ex,
                                        "Failed to update certificate {Certificate} with PFX private key. Retrying...",
                                        Redact.Create(newCert));
                                }
                            }
                            break;
                        case "PEM":
                            for (int attempt = 0; ; attempt++)
                            {
                                updateCertificate.CertificateWithPrivateKey =
                                s_certificateFactory.CreateWithPEMPrivateKey(
                                    newCert,
                                    privateKey.ToArray(),
                                    passwordProvider?.GetPassword(existingCertIdentifier));
                                try
                                {
                                    await UpdateCertificateInternalAsync(
                                        certificateGroup,
                                        existingCertIdentifier,
                                        updateCertificate, ct).ConfigureAwait(false);
                                    break;
                                }
                                catch (Exception ex) when (ShouldRetry(attempt, ex))
                                {
                                    m_logger.LogDebug(
                                        Utils.TraceMasks.Security,
                                        ex,
                                        "Failed to update certificate {Certificate} with PEM private key. Retrying...",
                                        Redact.Create(newCert));
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex) when (ex is not ServiceResultException)
                {
                    throw new ServiceResultException(
                        StatusCodes.BadSecurityChecksFailed,
                        "Failed to verify integrity of the new certificate and the private key.", ex);
                }
                finally
                {
                    // dispose temporary new private key as it is no longer needed
                    certificateGroup.TemporaryApplicationCertificate?.Dispose();
                    certificateGroup.TemporaryApplicationCertificate = null;
                }

                certificateGroup.UpdateCertificate = updateCertificate;
                applyChangesRequired = true;
            }
            catch (Exception e)
            {
                // report the failure of UpdateCertificate via an audit event
                ServerData.ReportCertificateUpdatedAuditEvent(
                    context,
                    objectId,
                    method,
                    inputArguments,
                    certificateGroupId,
                    certificateTypeId,
                    m_logger,
                    e);
                // Raise audit certificate event
                ServerData.ReportAuditCertificateEvent(newCert, e, m_logger);
                throw;
            }

            return new UpdateCertificateMethodStateResult
            {
                ServiceResult = ServiceResult.Good,
                ApplyChangesRequired = applyChangesRequired
            };

            static bool ShouldRetry(int attempt, Exception ex)
            {
                if (ex is ServiceResultException sre && sre.StatusCode == StatusCodes.BadConfigurationError)
                {
                    return false;
                }
                const int maxAttempts = 3;
                return attempt < maxAttempts;
            }

            // Handle the store update
            async Task UpdateCertificateInternalAsync(
                ServerCertificateGroup certificateGroup,
                CertificateIdentifier existingCertIdentifier,
                UpdateCertificateData updateCertificate,
                CancellationToken ct)
            {
                try
                {
                    using (ICertificateStore appStore = CertificateIdentifierResolver
                        .OpenStore(existingCertIdentifier, ServerData.Telemetry))
                    {
                        if (appStore == null)
                        {
                            throw ServiceResultException.ConfigurationError(
                                "Failed to open application certificate store.");
                        }

                        m_logger.LogInformation(
                            Utils.TraceMasks.Security,
                            "Delete application certificate {Thumbprint}",
                            existingCertIdentifier.Thumbprint);
                        await appStore.DeleteAsync(
                            existingCertIdentifier.Thumbprint,
                            ct)
                            .ConfigureAwait(false);
                        ICertificatePasswordProvider passwordProvider = m_configuration
                            .SecurityConfiguration
                            .CertificatePasswordProvider;
                        m_logger.LogInformation(
                            Utils.TraceMasks.Security,
                            "Add new application certificate {Certificate}",
                            Redact.Create(updateCertificate.CertificateWithPrivateKey));
                        Debug.Assert(updateCertificate.CertificateWithPrivateKey.HasPrivateKey);
                        await appStore.AddAsync(
                            updateCertificate.CertificateWithPrivateKey,
                            passwordProvider?.GetPassword(existingCertIdentifier),
                            ct)
                            .ConfigureAwait(false);
                        // keep only track of cert without private key
                        Certificate certOnly = DefaultCertificateFactory.Instance.CreateFromRawData(
                            updateCertificate.CertificateWithPrivateKey.RawData);
                        updateCertificate.CertificateWithPrivateKey.Dispose();
                        updateCertificate.CertificateWithPrivateKey = certOnly;
                        // the identifier no longer caches a resolved
                        // certificate; the store is the single source of truth
                        // and callers resolve when they need one.
                    }

                    ICertificateStore issuerStore = certificateGroup.IssuerStore.OpenStore(ServerData.Telemetry);
                    try
                    {
                        if (issuerStore == null)
                        {
                            throw ServiceResultException.ConfigurationError(
                                "Failed to open issuer certificate store.");
                        }

                        foreach (Certificate issuer in updateCertificate.IssuerCollection)
                        {
                            try
                            {
                                m_logger.LogInformation(
                                    Utils.TraceMasks.Security,
                                    "Add new issuer certificate {Certificate}",
                                    Redact.Create(issuer));
                                await issuerStore.AddAsync(issuer, ct: ct).ConfigureAwait(false);
                            }
                            catch (ArgumentException)
                            {
                                // ignore error if issuer cert already exists
                            }
                        }
                    }
                    finally
                    {
                        issuerStore?.Close();
                    }

                    ServerData.ReportCertificateUpdatedAuditEvent(
                        context,
                        objectId,
                        method,
                        inputArguments,
                        certificateGroupId,
                        certificateTypeId,
                        m_logger);
                }
                catch (Exception ex)
                {
                    m_logger.LogError(
                        Utils.TraceMasks.Security,
                        ex,
                        "Failed to update certificate {Certificate}.",
                        Redact.Create(newCert));
                    throw new ServiceResultException(
                        StatusCodes.BadSecurityChecksFailed,
                        "Failed to update certificate.",
                        ex);
                }
            }
        }

        private async ValueTask<CreateSigningRequestMethodStateResult> CreateSigningRequestAsync(
            ISystemContext context,
            MethodState method,
            NodeId objectId,
            NodeId certificateGroupId,
            NodeId certificateTypeId,
            string subjectName,
            bool regeneratePrivateKey,
            ByteString nonce,
            CancellationToken cancellationToken)
        {
            HasApplicationSecureAdminAccess(context);

            ServerCertificateGroup certificateGroup = VerifyGroupAndTypeId(
                certificateGroupId,
                certificateTypeId);

            // identify the existing certificate for which to CreateSigningRequest
            // it should be of the same type
            CertificateIdentifier existingCertIdentifier = certificateGroup.ApplicationCertificates
                .FirstOrDefault(
                    cert => cert.CertificateType == certificateTypeId);

            if (string.IsNullOrEmpty(subjectName))
            {
                subjectName = existingCertIdentifier.Certificate.Subject;
            }

            certificateGroup.TemporaryApplicationCertificate?.Dispose();
            certificateGroup.TemporaryApplicationCertificate = null;

            Certificate certWithPrivateKey;
            if (regeneratePrivateKey)
            {
                // the configured identifier is metadata only in 2.0; the
                // currently-active certificate comes from the manager's
                // registry, and its domains seed the temporary certificate.
                using CertificateEntry currentEntry =
                    (m_configuration.CertificateManager as ICertificateRegistry)?
                        .AcquireApplicationCertificateByType(certificateTypeId);
                Certificate currentCert = currentEntry?.Certificate;

                ArrayOf<string> domainNames = currentCert != null
                    ? X509Utils.GetDomainsFromCertificate(currentCert)
                    : default;

                certWithPrivateKey = GenerateTemporaryApplicationCertificate(
                    certificateTypeId,
                    certificateGroup,
                    subjectName,
                    domainNames);
            }
            else
            {
                ICertificatePasswordProvider passwordProvider = m_configuration
                    .SecurityConfiguration
                    .CertificatePasswordProvider;
                certWithPrivateKey = await CertificateIdentifierResolver
                    .LoadPrivateKeyAsync(
                        existingCertIdentifier,
                        passwordProvider,
                        m_configuration.ApplicationUri,
                        ServerData.Telemetry,
                        cancellationToken)
                    .ConfigureAwait(false);

                if (certWithPrivateKey == null)
                {
                    throw ServiceResultException.Create(StatusCodes.BadInternalError, "Failed to load private key");
                }
            }

            m_logger.LogInformation(
                Utils.TraceMasks.Security,
                "Create signing request {Certificate}",
                Redact.Create(certWithPrivateKey));
            byte[] certificateRequest = s_certificateFactory.CreateSigningRequest(
                certWithPrivateKey,
                X509Utils.GetDomainsFromCertificate(certWithPrivateKey).ToArray());

            return new CreateSigningRequestMethodStateResult
            {
                ServiceResult = ServiceResult.Good,
                CertificateRequest = certificateRequest.ToByteString()
            };
        }

        private Certificate GenerateTemporaryApplicationCertificate(
            NodeId certificateTypeId,
            ServerCertificateGroup certificateGroup,
            string subjectName,
            ArrayOf<string> domainNames)
        {
            Certificate certificate;

            // CreateApplicationCertificate does not default the domain
            // names the way the removed CertificateFactory.CreateCertificate
            // did, and this path is reached with none of them when the
            // current certificate could not be resolved. Without the
            // fallback the signing request would carry no DNS entry in its
            // subject alternative name at all.
            if (domainNames.IsEmpty)
            {
                domainNames = [Utils.GetHostName()];
            }

            ICertificateBuilder certificateBuilder = s_certificateFactory
                .CreateApplicationCertificate(m_configuration.ApplicationUri, m_configuration.ApplicationName, subjectName, [.. domainNames])
                .SetNotBefore(DateTime.Today.AddDays(-1))
                .SetNotAfter(DateTime.Today.AddDays(14));

            if (certificateTypeId.IsNull ||
                certificateTypeId == ObjectTypeIds.ApplicationCertificateType ||
                certificateTypeId == ObjectTypeIds.RsaMinApplicationCertificateType ||
                certificateTypeId == ObjectTypeIds.RsaSha256ApplicationCertificateType)
            {
                certificate = certificateBuilder.SetRSAKeySize(CertificateFactory.DefaultKeySize)
                    .CreateForRSA();
            }
            else
            {
                ECCurve? curve =
                    CryptoUtils.GetCurveFromCertificateTypeId(certificateTypeId)
                    ?? throw new ServiceResultException(
                        StatusCodes.BadNotSupported,
                        "The Ecc certificate type is not supported.");
                certificate = certificateBuilder.SetECCurve(curve.Value).CreateForECDsa();
            }

            certificateGroup.TemporaryApplicationCertificate = certificate;

            return certificate;
        }

        private ServiceResult ApplyChanges(
            ISystemContext context,
            MethodState method,
            NodeId objectId,
            ArrayOf<Variant> inputArguments,
            List<Variant> outputArguments)
        {
            HasApplicationSecureAdminAccess(context);

            bool disconnectSessions = false;

            foreach (ServerCertificateGroup certificateGroup in m_certificateGroups)
            {
                try
                {
                    UpdateCertificateData updateCertificate = certificateGroup.UpdateCertificate;
                    if (updateCertificate != null)
                    {
                        disconnectSessions = true;
                        m_logger.LogInformation(
                            Utils.TraceMasks.Security,
                            "Apply Changes for certificate {Certificate}",
                            Redact.Create(updateCertificate.CertificateWithPrivateKey));
                    }
                }
                finally
                {
                    certificateGroup.UpdateCertificate = null;
                }
            }

            if (disconnectSessions)
            {
                // When a Server Certificate or TrustList changes active SecureChannels
                // are not immediately affected. This ensures the caller of ApplyChanges
                // can get a response to the Method call. Once the Method response is
                // returned the Server shall force existing SecureChannels affected by
                // the changes to renegotiate and use the new Server Certificate
                // and/or TrustLists.

                // TODO: This needs fixing, the 1 second might or might not work to give
                // Time to the client to receive the response.  Also, this needs to cut
                // all channels and reevaluate sessions, this needs to be implemented in
                // Transport side presumably.

                _ = Task.Run(async () =>
                {
                    m_logger.LogInformation(
                        Utils.TraceMasks.Security,
                        "----- Apply Changes of application certificate starts in 1 second...");

                    // give the client some time to receive the response
                    // before the certificate update may disconnect all sessions
                    await Task.Delay(1000).ConfigureAwait(false);

                    try
                    {
                        m_logger.LogInformation(
                            Utils.TraceMasks.Security,
                            "----- Apply Changes for application certificate update running...");

                        // CertificateValidator.UpdateCertificateAsync is gone
                        // in 2.0; the manager reloads the application
                        // certificate snapshot from the configuration instead.
                        await m_configuration
                            .CertificateManager.ReloadApplicationCertificatesAsync(
                                m_configuration.SecurityConfiguration,
                                m_configuration.ApplicationUri)
                            .ConfigureAwait(false);

                        m_logger.LogInformation(
                            Utils.TraceMasks.Security,
                            "----- Apply Changes for application certificate update completed.");
                    }
                    catch (Exception ex)
                    {
                        m_logger.LogCritical(
                            ex,
                            "----- Apply Changes for application certificate update failed: " +
                            "Error updating application instance certificates. " +
                            "Server could be in faulted state.");

                        // Throws to nowhere since no one is listening ... // throw;
                    }
                });
            }

            return StatusCodes.Good;
        }

        private ServiceResult GetRejectedList(
            ISystemContext context,
            MethodState method,
            NodeId objectId,
            ref ArrayOf<ByteString> certificates)
        {
            HasApplicationSecureAdminAccess(context);

            // No rejected store configured
            if (m_rejectedStore == null)
            {
                certificates = [];
                return StatusCodes.Good;
            }

            ICertificateStore store = m_rejectedStore.OpenStore(ServerData.Telemetry);
            try
            {
                if (store != null)
                {
                    using CertificateCollection collection = store.EnumerateAsync().Result;
                    var rawList = new List<ByteString>();
                    foreach (Certificate cert in collection)
                    {
                        rawList.Add(cert.RawData.ToByteString());
                    }
                    certificates = rawList.ToArrayOf();
                }
            }
            finally
            {
                store?.Close();
            }

            return StatusCodes.Good;
        }

        private ServiceResult GetCertificates(
            ISystemContext context,
            MethodState method,
            NodeId objectId,
            NodeId certificateGroupId,
            ref ArrayOf<NodeId> certificateTypeIds,
            ref ArrayOf<ByteString> certificates)
        {
            HasApplicationSecureAdminAccess(context);

            ServerCertificateGroup certificateGroup =
                m_certificateGroups.FirstOrDefault(
                    group => Utils.IsEqual(group.NodeId, certificateGroupId))
                ?? throw new ServiceResultException(
                    StatusCodes.BadInvalidArgument,
                    "Certificate group invalid.");

            certificateTypeIds = certificateGroup.CertificateTypes;
            certificates = certificateGroup.ApplicationCertificates
                .Select(s => s.Certificate?.RawData.ToByteString() ?? default)
                .ToArrayOf();

            return ServiceResult.Good;
        }

        private ServerCertificateGroup VerifyGroupAndTypeId(
            NodeId certificateGroupId,
            NodeId certificateTypeId)
        {
            // verify typeid must be set
            if (certificateTypeId.IsNull)
            {
                throw new ServiceResultException(
                    StatusCodes.BadInvalidArgument,
                    "Certificate type not specified.");
            }

            // verify requested certificate group
            if (certificateGroupId.IsNull)
            {
                certificateGroupId = ObjectIds
                    .ServerConfiguration_CertificateGroups_DefaultApplicationGroup;
            }

            ServerCertificateGroup certificateGroup =
                m_certificateGroups.FirstOrDefault(
                    group => Utils.IsEqual(group.NodeId, certificateGroupId))
                ?? throw new ServiceResultException(
                    StatusCodes.BadInvalidArgument,
                    "Certificate group invalid.");

            // verify certificate type
            bool foundCertType = certificateGroup.CertificateTypes
                .Any(t => Utils.IsEqual(t, certificateTypeId));
            if (!foundCertType)
            {
                throw new ServiceResultException(
                    StatusCodes.BadInvalidArgument,
                    "Certificate type not valid for certificate group.");
            }

            return certificateGroup;
        }

        /// <summary>
        /// Finds the <see cref="NamespaceMetadataState"/> node for the specified NamespaceUri.
        /// </summary>
        private NamespaceMetadataState FindNamespaceMetadataState(string namespaceUri)
        {
            try
            {
                // find ServerNamespaces node
                if (FindPredefinedNode<NamespacesState>(ObjectIds.Server_Namespaces)
                    is not NamespacesState serverNamespacesNode)
                {
                    m_logger.LogError("Cannot find ObjectIds.Server_Namespaces node.");
                    return null;
                }

                IList<BaseInstanceState> serverNamespacesChildren = [];
                serverNamespacesNode.GetChildren(SystemContext, serverNamespacesChildren);

                foreach (BaseInstanceState namespacesReference in serverNamespacesChildren)
                {
                    // Find NamespaceMetadata node of NamespaceUri in Namespaces children
                    if (namespacesReference is not NamespaceMetadataState namespaceMetadata)
                    {
                        continue;
                    }

                    if (namespaceMetadata.NamespaceUri.Value == namespaceUri)
                    {
                        return namespaceMetadata;
                    }
                }

                IList<IReference> serverNamespacesReferencs = [];
                serverNamespacesNode.GetReferences(SystemContext, serverNamespacesReferencs);

                foreach (IReference serverNamespacesReference in serverNamespacesReferencs)
                {
                    if (!serverNamespacesReference.IsInverse)
                    {
                        // Find NamespaceMetadata node of NamespaceUri in Namespaces references
                        var nameSpaceNodeId = ExpandedNodeId.ToNodeId(
                            serverNamespacesReference.TargetId,
                            ServerData.NamespaceUris);
                        if (FindNodeInAddressSpace(
                            nameSpaceNodeId) is not NamespaceMetadataState namespaceMetadata)
                        {
                            continue;
                        }

                        if (namespaceMetadata.NamespaceUri.Value == namespaceUri)
                        {
                            return namespaceMetadata;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                m_logger.LogError(
                    ex,
                    "Error searching NamespaceMetadata for namespaceUri {NamespaceUri}.",
                    namespaceUri);
                return null;
            }
        }

        /// <summary>
        /// Clear NamespaceMetadata nodes cache in case nodes are added or deleted
        /// </summary>
        private void ServerNamespacesChanged(
            ISystemContext context,
            NodeState node,
            NodeStateChangeMasks changes)
        {
            if ((changes & NodeStateChangeMasks.Children) != 0 ||
                (changes & NodeStateChangeMasks.References) != 0)
            {
                try
                {
                    lock (Lock)
                    {
                        m_namespaceMetadataStates.Clear();
                    }
                }
                catch
                {
                    // ignore errors
                }
            }
        }

        /// <summary>
        /// Verifies the integrity of a pushed certificate against the issuer
        /// chain that was pushed with it.
        /// </summary>
        /// <remarks>
        /// The chain is validated as a whole rather than by building a private
        /// trust list from the issuers, which is what the 1.5 code did with
        /// <c>CertificateValidator.Update</c>. Certificate download is switched
        /// off so a pushed certificate cannot make the server fetch a URL, and
        /// BadCertificateUntrusted is accepted, since at this point the
        /// certificate is by definition not yet trusted - every other error,
        /// including signature integrity and key size, still fails.
        /// </remarks>
        internal static async Task ValidatePushCertificateAndIssuerChainAsync(
            Certificate newCertificate,
            CertificateCollection issuerCertificates,
            SecurityConfiguration securityConfiguration,
            ITelemetryContext telemetry,
            CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(newCertificate);
            ArgumentNullException.ThrowIfNull(issuerCertificates);
            ArgumentNullException.ThrowIfNull(securityConfiguration);
            ArgumentNullException.ThrowIfNull(telemetry);

            using CertificateCollection validationChain = issuerCertificates.AddRef();
            validationChain.Insert(0, newCertificate);

            using CertificateManager validator = CertificateManagerFactory.Create(
                securityConfiguration,
                telemetry);

            var options = new Opc.Ua.Security.Certificates.CertificateValidationOptions
            {
                AllowCertificateDownload = false,
                UrlRetrievalTimeout = TimeSpan.FromMilliseconds(1),
                AcceptError = static (_, serviceResult) =>
                    serviceResult.StatusCode == StatusCodes.BadCertificateUntrusted
            };

            Opc.Ua.CertificateValidationResult validationResult = await validator
                .ValidateAsync(validationChain, trustList: null, options: options, ct)
                .ConfigureAwait(false);

            validationResult.ThrowIfInvalid();
        }
        #endregion Private Methods

        #region Private Fields
        private class UpdateCertificateData
        {
            public NodeId SessionId { get; set; }
            public Certificate CertificateWithPrivateKey { get; set; }
            public CertificateCollection IssuerCollection { get; set; }
        }

        private class ServerCertificateGroup
        {
            public string BrowseName { get; set; }
            public NodeId NodeId { get; set; }
            public CertificateGroupState Node { get; set; }
            public NodeId[] CertificateTypes { get; set; }
            public List<CertificateIdentifier> ApplicationCertificates { get; set; }
            public CertificateStoreIdentifier IssuerStore { get; set; }
            public CertificateStoreIdentifier TrustedStore { get; set; }
            public UpdateCertificateData UpdateCertificate { get; set; }
            public Certificate TemporaryApplicationCertificate { get; set; }
        }

        private ServerConfigurationState m_serverConfigurationNode;
        private static readonly ICertificateFactory s_certificateFactory = DefaultCertificateFactory.Instance;
        private readonly ApplicationConfiguration m_configuration;
        private readonly List<ServerCertificateGroup> m_certificateGroups;
        private readonly CertificateStoreIdentifier m_rejectedStore;
        private readonly Dictionary<string, NamespaceMetadataState> m_namespaceMetadataStates = [];
        #endregion Private Fields
    }
}
