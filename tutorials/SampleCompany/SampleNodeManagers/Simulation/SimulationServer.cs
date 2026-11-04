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
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Opc.Ua.Security.Certificates;
using Technosoftware.UaServer;
#endregion Using Directives

namespace SampleCompany.NodeManagers.Simulation
{
    /// <summary>
    /// Implements a basic OPC UA Server.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each server instance must have one instance of a StandardServer object which is
    /// responsible for reading the configuration file, creating the endpoints and dispatching
    /// incoming requests to the appropriate handler.
    /// </para>
    /// <para>
    /// This sub-class specifies non-configurable metadata such as Product Name and initializes
    /// the SimulationServerNodeManager which provides access to the data exposed by the Server.
    /// </para>
    /// </remarks>
    public class SimulationServer : UaStandardServer
    {
        #region Constructors
        /// <summary>
        /// Initializes the server with the telemetry context UaStandardServer
        /// requires in 2.0.
        /// </summary>
        public SimulationServer(ITelemetryContext telemetry)
            : base(telemetry)
        {
        }
        #endregion Constructors

        #region Properties
        public ITokenValidator TokenValidator { get; set; }
        #endregion Properties

        #region Overridden Methods
        /// <summary>
        /// Creates the node managers for the server.
        /// </summary>
        /// <remarks>
        /// This method allows the sub-class create any additional node managers which it uses. The SDK
        /// always creates a CoreNodeManager which handles the built-in nodes defined by the specification.
        /// Any additional NodeManagers are expected to handle application specific nodes.
        /// </remarks>
        protected override MasterNodeManager CreateMasterNodeManager(
            IUaServerData server,
            ApplicationConfiguration configuration)
        {
            m_logger.LogInformation(
                Utils.TraceMasks.StartStop,
                "Creating the Simulation Server Node Manager.");

            IList<IUaNodeManager> nodeManagers;
            var asyncNodeManagers = new List<IUaStandardAsyncNodeManager>();

            nodeManagers =
            [
                // create the custom node manager.
                new SimulationServerNodeManager(
                    server,
                    configuration)
            ];

            foreach (IUaNodeManagerFactory nodeManagerFactory in NodeManagerFactories)
            {
                nodeManagers.Add(nodeManagerFactory.Create(server, configuration));
            }

            foreach (IUaAsyncNodeManagerFactory nodeManagerFactory in AsyncNodeManagerFactories)
            {
                asyncNodeManagers.Add(nodeManagerFactory.CreateAsync(server, configuration).AsTask().GetAwaiter().GetResult());
            }

            return new MasterNodeManager(server, configuration, null, asyncNodeManagers, nodeManagers);
        }

        protected override IUaMonitoredItemQueueFactory CreateMonitoredItemQueueFactory(
            IUaServerData server,
            ApplicationConfiguration configuration)
        {
            return new MonitoredItemQueueFactory(server.Telemetry);
        }

        /// <summary>
        /// Creates the subscriptionStore for the server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>Returns a subscriptionStore for a server, the return type is <seealso cref="IUaSubscriptionStore"/>.</returns>
        protected override IUaSubscriptionStore CreateSubscriptionStore(
            IUaServerData server,
            ApplicationConfiguration configuration)
        {
            return null;
        }

        /// <summary>
        /// Loads the non-configurable properties for the application.
        /// </summary>
        /// <remarks>
        /// These properties are exposed by the server but cannot be changed by administrators.
        /// </remarks>
        protected override ServerProperties LoadServerProperties()
        {
            return new ServerProperties
            {
                ManufacturerName = "Technosoftware GmbH",
                ProductName = "Technosoftware OPC UA Sample Server",
                ProductUri = "http://technosoftware.com/SampleServer/v1.04",
                SoftwareVersion = Utils.GetAssemblySoftwareVersion(),
                BuildNumber = Utils.GetAssemblyBuildNumber(),
                BuildDate = Utils.GetAssemblyTimestamp()
            };
        }

        /// <summary>
        /// Creates the resource manager for the server.
        /// </summary>
        protected override ResourceManager CreateResourceManager(
            IUaServerData server,
            ApplicationConfiguration configuration)
        {
            var resourceManager = new ResourceManager(configuration);

            foreach (
                System.Reflection.FieldInfo field in typeof(StatusCodes).GetFields(
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                uint? id = field.GetValue(typeof(StatusCodes)) as uint?;

                if (id != null)
                {
                    resourceManager.Add(id.Value, "en-US", field.Name);
                }
            }

            return resourceManager;
        }

        /// <summary>
        /// Initializes the server before it starts up.
        /// </summary>
        /// <remarks>
        /// This method is called before any startup processing occurs. The sub-class may update the
        /// configuration object or do any other application specific startup tasks.
        /// </remarks>
        protected override void OnServerStarting(ApplicationConfiguration configuration)
        {
            base.OnServerStarting(configuration);

            m_logger.LogInformation(Utils.TraceMasks.StartStop, "The server is starting.");

            // it is up to the application to decide how to validate user identity tokens.
            // this function creates validator for X509 identity tokens.
            CreateUserIdentityValidators(configuration);
        }

        /// <summary>
        /// Called after the server has been started.
        /// </summary>
        protected override void OnServerStarted(IUaServerData server)
        {
            base.OnServerStarted(server);

            // request notifications when the user identity is changed. all valid users are accepted by default.
            server.SessionManager.ImpersonateUser
                += OnImpersonateUser;

            try
            {
                ServerInternal.UpdateServerStatus(
                    status =>
                    // allow a faster sampling interval for CurrentTime node.
                        status.Variable.CurrentTime.MinimumSamplingInterval = 250);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Override some of the default user token policies for some endpoints.
        /// </summary>
        /// <remarks>
        /// Sample to show how to override default user token policies.
        /// </remarks>
        public override ArrayOf<UserTokenPolicy> GetUserTokenPolicies(
            ApplicationConfiguration configuration,
            EndpointDescription description)
        {
            ArrayOf<UserTokenPolicy> policies = base.GetUserTokenPolicies(
                configuration,
                description);

            // sample how to modify default user token policies
            if (description.SecurityPolicyUri == SecurityPolicies.Aes256_Sha256_RsaPss &&
                description.SecurityMode == MessageSecurityMode.SignAndEncrypt)
            {
                return policies.Filter(u => u.TokenType != UserTokenType.Certificate);
            }
            else if (description.SecurityPolicyUri == SecurityPolicies.Aes128_Sha256_RsaOaep &&
                description.SecurityMode == MessageSecurityMode.Sign)
            {
                return policies.Filter(u => u.TokenType != UserTokenType.Anonymous);
            }
            else if (description.SecurityPolicyUri == SecurityPolicies.Aes128_Sha256_RsaOaep &&
                description.SecurityMode == MessageSecurityMode.SignAndEncrypt)
            {
                return policies.Filter(u => u.TokenType != UserTokenType.UserName);
            }
            return policies;
        }
        #endregion Overridden Methods

        #region User Validation Functions
        /// <summary>
        /// Creates the objects used to validate the user identity tokens supported by the server.
        /// </summary>
        private void CreateUserIdentityValidators(ApplicationConfiguration configuration)
        {
            for (int ii = 0; ii < configuration.ServerConfiguration.UserTokenPolicies.Count; ii++)
            {
                UserTokenPolicy policy = configuration.ServerConfiguration.UserTokenPolicies[ii];

                // create a validator for a certificate token policy.
                if (policy.TokenType == UserTokenType.Certificate)
                {
                    // check if user certificate trust lists are specified in configuration.
                    if (configuration.SecurityConfiguration.TrustedUserCertificates != null &&
                        configuration.SecurityConfiguration.UserIssuerCertificates != null)
                    {
                        // 2.0 has no separate CertificateValidator to build and no
                        // GetChannelValidator: the server's own CertificateManager
                        // already holds the user trust lists from the
                        // SecurityConfiguration, and a user certificate is validated
                        // by naming TrustListIdentifier.Users at the call.
                        m_validateUserCertificates = true;
                    }
                }
            }
        }

        /// <summary>
        /// Called when a client tries to change its user identity.
        /// </summary>
        /// <exception cref="ServiceResultException"></exception>
        private void OnImpersonateUser(object sender, ImpersonateUserEventArgs args)
        {
            IUaSession session = (IUaSession)sender;

            // check for a user name token.

            if (args.NewIdentity is UserNameIdentityToken userNameToken)
            {
                args.Identity = VerifyPassword(
                    userNameToken,
                    args.NewIdentityTokenHandler);

                m_logger.LogInformation(
                    Utils.TraceMasks.Security,
                    "Username Token Accepted: {Identity}",
                    args.Identity?.DisplayName);

                return;
            }

            // check for x509 user token.

            if (args.NewIdentity is X509IdentityToken x509Token)
            {
                VerifyX509IdentityToken(x509Token);
                // set AuthenticatedUser role for accepted certificate authentication
                args.Identity = new RoleBasedIdentity(
                    new UserIdentity(x509Token),
                    [Role.AuthenticatedUser]);
                m_logger.LogInformation(
                    Utils.TraceMasks.Security,
                    "X509 Token Accepted: {Identity}",
                    args.Identity?.DisplayName);

                return;
            }

            // check for issued identity token.
            if (args.NewIdentity is IssuedIdentityToken)
            {
                args.Identity = VerifyIssuedToken(
                    args.NewIdentityTokenHandler as IssuedIdentityTokenHandler);

                // set AuthenticatedUser role for accepted identity token.
                // GrantedRoleIds is an immutable ArrayOf in 2.0, so the role is
                // granted by wrapping the identity rather than appending to it.
                if (args.Identity != null)
                {
                    args.Identity = new RoleBasedIdentity(
                        args.Identity,
                        [Role.AuthenticatedUser]);
                }

                return;
            }

            // check for anonymous token.
            if (args.NewIdentity is AnonymousIdentityToken or null)
            {
                // allow anonymous authentication and set Anonymous role for this authentication
                args.Identity = new RoleBasedIdentity(new UserIdentity(), [Role.Anonymous]);
                return;
            }

            // unsupported identity token type.
            throw ServiceResultException.Create(
                StatusCodes.BadIdentityTokenInvalid,
                "Not supported user token type: {0}.",
                args.NewIdentity);
        }

        /// <summary>
        /// Validates the password for a username token.
        /// </summary>
        /// <exception cref="ServiceResultException"></exception>
        private IUserIdentity VerifyPassword(
            UserNameIdentityToken userNameToken,
            IUserIdentityTokenHandler tokenHandler)
        {
            string userName = userNameToken.UserName;

            // 2.0 leaves UserNameIdentityToken.Password as it arrived on the
            // wire and keeps the decrypted secret on the handler, so the
            // plaintext is read from there.
            byte[] password = (tokenHandler as UserNameIdentityTokenHandler)
                ?.DecryptedPassword;
            if (string.IsNullOrEmpty(userName))
            {
                // an empty username is not accepted.
                throw ServiceResultException.Create(
                    StatusCodes.BadIdentityTokenInvalid,
                    "Security token is not a valid username token. An empty username is not accepted.");
            }

            if (Utils.Utf8IsNullOrEmpty(password))
            {
                // an empty password is not accepted.
                throw ServiceResultException.Create(
                    StatusCodes.BadIdentityTokenRejected,
                    "Security token is not a valid username token. An empty password is not accepted.");
            }

            // User with permission to configure server
            if (userName == "sysadmin" && Utils.IsEqual(password, "demo"u8))
            {
                return new SystemConfigurationIdentity(
                    new UserIdentity(userNameToken));
            }

            // standard users for CTT verification
            if (!((userName == "user1" && Utils.IsEqual(password, "password"u8)) ||
                (userName == "user2" && Utils.IsEqual(password, "password1"u8))))
            {
                // construct translation object with default text.
                var info = new TranslationInfo(
                    "InvalidPassword",
                    "en-US",
                    "Invalid username or password.",
                    userName);

                // create an exception with a vendor defined sub-code.
                throw new ServiceResultException(
                    new ServiceResult(
                    LoadServerProperties().ProductUri,
                        new StatusCode(StatusCodes.BadUserAccessDenied.Code, "InvalidPassword"),
                    new LocalizedText(info)));
            }
            return new RoleBasedIdentity(
                new UserIdentity(userNameToken),
                [Role.AuthenticatedUser]);
        }

        /// <summary>
        /// Verifies that a certificate user token is trusted.
        /// </summary>
        /// <exception cref="ServiceResultException"></exception>
        private void VerifyX509IdentityToken(X509IdentityToken token)
        {
            using Certificate certificate = Certificate.FromRawData(token.CertificateData);
            try
            {
                Opc.Ua.CertificateValidationResult result = CertificateManager
                    .ValidateAsync(
                        certificate,
                        m_validateUserCertificates
                            ? TrustListIdentifier.Users
                            : TrustListIdentifier.Peers)
                    .GetAwaiter()
                    .GetResult();

                result.ThrowIfInvalid();
            }
            catch (Exception e)
            {
                TranslationInfo info;
                StatusCode result = StatusCodes.BadIdentityTokenRejected;
                if (e is ServiceResultException se &&
                    se.StatusCode == StatusCodes.BadCertificateUseNotAllowed)
                {
                    info = new TranslationInfo(
                        "InvalidCertificate",
                        "en-US",
                        "'{0}' is an invalid user certificate.",
                        certificate.Subject);

                    result = StatusCodes.BadIdentityTokenInvalid;
                }
                else
                {
                    // construct translation object with default text.
                    info = new TranslationInfo(
                        "UntrustedCertificate",
                        "en-US",
                        "'{0}' is not a trusted user certificate.",
                        certificate.Subject);
                }

                // create an exception with a vendor defined sub-code.
                throw new ServiceResultException(
                    new ServiceResult(
                        LoadServerProperties().ProductUri,
                        new StatusCode(result.Code, info.Key),
                        new LocalizedText(info)));
            }
        }

        private IUserIdentity VerifyIssuedToken(IssuedIdentityTokenHandler issuedTokenHandler)
        {
            if (TokenValidator == null)
            {
                m_logger.LogWarning(Utils.TraceMasks.Security, "No TokenValidator is specified.");
                return null;
            }
            try
            {
                // 2.0 keeps the issued token's type - and its decrypted
                // data - on the handler rather than on the token itself.
                if (issuedTokenHandler?.IssuedTokenType == IssuedTokenType.JWT)
                {
                    m_logger.LogDebug(Utils.TraceMasks.Security, "VerifyIssuedToken: ValidateToken");
                    return TokenValidator.ValidateToken(issuedTokenHandler);
                }

                return null;
            }
            catch (Exception e)
            {
                TranslationInfo info;
                StatusCode result = StatusCodes.BadIdentityTokenRejected;
                if (e is ServiceResultException se &&
                    se.StatusCode == StatusCodes.BadIdentityTokenInvalid)
                {
                    info = new TranslationInfo(
                        "IssuedTokenInvalid",
                        "en-US",
                        "token is an invalid issued token.");
                    result = StatusCodes.BadIdentityTokenInvalid;
                }
                else // Rejected
                {
                    // construct translation object with default text.
                    info = new TranslationInfo(
                        "IssuedTokenRejected",
                        "en-US",
                        "token is rejected.");
                }

                m_logger.LogWarning(
                    Utils.TraceMasks.Security,
                    "VerifyIssuedToken: Throw ServiceResultException 0x{Result:x}", result);

                throw new ServiceResultException(
                    new ServiceResult(
                        LoadServerProperties().ProductUri,
                        new StatusCode(result.Code, info.Key),
                    new LocalizedText(info)));
            }
        }
        #endregion User Validation Functions

        #region Private Fields
        private bool m_validateUserCertificates;
        #endregion Private Fields
    }
}
