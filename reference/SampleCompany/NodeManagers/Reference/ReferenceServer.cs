#region Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Opc.Ua;
using Technosoftware.UaServer;
using Technosoftware.UaServer.Configuration;
using Technosoftware.UaServer.NodeManager;
using Technosoftware.UaServer.Subscriptions;
#endregion Using Directives

namespace SampleCompany.NodeManagers.Reference
{
    /// <summary>
    /// Implements a basic OPC UA Server.
    /// </summary>
    /// <remarks>
    /// Each server instance must have one instance of a StandardServer object which is
    /// responsible for reading the configuration file, creating the endpoints and dispatching
    /// incoming requests to the appropriate handler.
    ///
    /// This sub-class specifies non-configurable metadata such as Product Name and initializes
    /// the SimulationServerNodeManager which provides access to the data exposed by the Server.
    /// </remarks>
    public class ReferenceServer : UaStandardServer
    {
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
            Utils.LogInfo(
                Utils.TraceMasks.StartStop,
                "Creating the Simulation Server Node Manager.");

            IList<IUaNodeManager> nodeManagers =
            [
            // create the custom node manager.
                new ReferenceServerNodeManager(
                    server,
                    configuration)
            ];

            foreach (IUaNodeManagerFactory nodeManagerFactory in NodeManagerFactories)
            {
                nodeManagers.Add(nodeManagerFactory.Create(server, configuration));
            }

            // create master node manager.
            return new MasterNodeManager(server, configuration, null, [.. nodeManagers]);
        }

        protected override IUaMonitoredItemQueueFactory CreateMonitoredItemQueueFactory(
            IUaServerData server,
            ApplicationConfiguration configuration)
        {
            if (configuration?.ServerConfiguration?.DurableSubscriptionsEnabled == true)
            {
                return new DurableSubscription.DurableMonitoredItemQueueFactory();
            }
            return new MonitoredItemQueueFactory();
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
            if (configuration?.ServerConfiguration?.DurableSubscriptionsEnabled == true)
            {
                return new DurableSubscription.SubscriptionStore(server);
            }
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
                ProductName = "Technosoftware OPC UA Reference Server",
                ProductUri = "http://technosoftware.com/ReferenceServer/v1.04",
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
            var resourceManager = new ResourceManager(server, configuration);

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
            Utils.LogInfo(Utils.TraceMasks.StartStop, "The server is starting.");

            base.OnServerStarting(configuration);

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
            server.SessionManager.ImpersonateUserEvent
                += OnImpersonateUser;

            try
            {
                lock (server.Status.Lock)
                {
                    // allow a faster sampling interval for CurrentTime node.
                    server.Status.Variable.CurrentTime.MinimumSamplingInterval = 250;
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Override some of the default user token policies for some endpoints.
        /// </summary>
        /// <remarks>
        /// Sample to show how to override default user token policies.
        /// </remarks>
        public override UserTokenPolicyCollection GetUserTokenPolicies(
            ApplicationConfiguration configuration,
            EndpointDescription description)
        {
            UserTokenPolicyCollection policies = base.GetUserTokenPolicies(
                configuration,
                description);

            // sample how to modify default user token policies
            if (description.SecurityPolicyUri == SecurityPolicies.Aes256_Sha256_RsaPss &&
                description.SecurityMode == MessageSecurityMode.SignAndEncrypt)
            {
                return [.. policies.Where(u => u.TokenType != UserTokenType.Certificate)];
            }
            else if (description.SecurityPolicyUri == SecurityPolicies.Aes128_Sha256_RsaOaep &&
                description.SecurityMode == MessageSecurityMode.Sign)
            {
                return [.. policies.Where(u => u.TokenType != UserTokenType.Anonymous)];
            }
            else if (description.SecurityPolicyUri == SecurityPolicies.Aes128_Sha256_RsaOaep &&
                description.SecurityMode == MessageSecurityMode.SignAndEncrypt)
            {
                return [.. policies.Where(u => u.TokenType != UserTokenType.UserName)];
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
                        var certificateValidator = new CertificateValidator();
                        certificateValidator.UpdateAsync(configuration.SecurityConfiguration)
                            .Wait();
                        certificateValidator.Update(
                            configuration.SecurityConfiguration.UserIssuerCertificates,
                            configuration.SecurityConfiguration.TrustedUserCertificates,
                            configuration.SecurityConfiguration.RejectedCertificateStore);

                        // set custom validator for user certificates.
                        m_userCertificateValidator = certificateValidator.GetChannelValidator();
                    }
                }
            }
        }

        /// <summary>
        /// Called when a client tries to change its user identity.
        /// </summary>
        /// <exception cref="ServiceResultException"></exception>
        private void OnImpersonateUser(object sender, UaImpersonateUserEventArgs args)
        {
            // check for a user name token.

            if (args.NewIdentity is UserNameIdentityToken userNameToken)
            {
                args.Identity = VerifyPassword(userNameToken);

                Utils.LogInfo(
                    Utils.TraceMasks.Security,
                    "Username Token Accepted: {0}",
                    args.Identity?.DisplayName);

                return;
            }

            // check for x509 user token.

            if (args.NewIdentity is X509IdentityToken x509Token)
            {
                VerifyUserTokenCertificate(x509Token.Certificate);
                // set AuthenticatedUser role for accepted certificate authentication
                args.Identity = new RoleBasedIdentity(
                    new UserIdentity(x509Token),
                    [Role.AuthenticatedUser]);
                Utils.LogInfo(
                    Utils.TraceMasks.Security,
                    "X509 Token Accepted: {0}",
                    args.Identity?.DisplayName);

                return;
            }

            // check for issued identity token.
            if (args.NewIdentity is IssuedIdentityToken issuedToken)
            {
                args.Identity = VerifyIssuedToken(issuedToken);

                // set AuthenticatedUser role for accepted identity token
                args.Identity.GrantedRoleIds.Add(ObjectIds.WellKnownRole_AuthenticatedUser);

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
        private IUserIdentity VerifyPassword(UserNameIdentityToken userNameToken)
        {
            string userName = userNameToken.UserName;
            string password = userNameToken.DecryptedPassword;
            if (string.IsNullOrEmpty(userName))
            {
                // an empty username is not accepted.
                throw ServiceResultException.Create(
                    StatusCodes.BadIdentityTokenInvalid,
                    "Security token is not a valid username token. An empty username is not accepted.");
            }

            if (string.IsNullOrEmpty(password))
            {
                // an empty password is not accepted.
                throw ServiceResultException.Create(
                    StatusCodes.BadIdentityTokenRejected,
                    "Security token is not a valid username token. An empty password is not accepted.");
            }

            // User with permission to configure server
            if (userName == "sysadmin" && password == "demo")
            {
                return new SystemConfigurationIdentity(new UserIdentity(userNameToken));
            }

            // standard users for CTT verification
            if (!((userName == "user1" && password == "password") ||
                (userName == "user2" && password == "password1")))
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
                    StatusCodes.BadUserAccessDenied,
                    "InvalidPassword",
                    LoadServerProperties().ProductUri,
                    new LocalizedText(info)));
            }
            return new RoleBasedIdentity(new UserIdentity(userNameToken), [Role.AuthenticatedUser]);
        }

        /// <summary>
        /// Verifies that a certificate user token is trusted.
        /// </summary>
        /// <exception cref="ServiceResultException"></exception>
        private void VerifyUserTokenCertificate(X509Certificate2 certificate)
        {
            try
            {
                if (m_userCertificateValidator != null)
                {
                    m_userCertificateValidator.Validate(certificate);
                }
                else
                {
                    CertificateValidator.Validate(certificate);
                }
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
                    result,
                    info.Key,
                    LoadServerProperties().ProductUri,
                    new LocalizedText(info)));
            }
        }

        private IUserIdentity VerifyIssuedToken(IssuedIdentityToken issuedToken)
        {
            if (TokenValidator == null)
            {
                Utils.LogWarning(Utils.TraceMasks.Security, "No TokenValidator is specified.");
                return null;
            }
            try
            {
                if (issuedToken.IssuedTokenType == IssuedTokenType.JWT)
                {
                    Utils.LogDebug(Utils.TraceMasks.Security, "VerifyIssuedToken: ValidateToken");
                    return TokenValidator.ValidateToken(issuedToken);
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

                Utils.LogWarning(
                    Utils.TraceMasks.Security,
                    "VerifyIssuedToken: Throw ServiceResultException 0x{result:x}");
                throw new ServiceResultException(
                    new ServiceResult(
                    result,
                    info.Key,
                        LoadServerProperties().ProductUri,
                    new LocalizedText(info)));
            }
        }
        #endregion User Validation Functions

        #region Private Fields
        private ICertificateValidator m_userCertificateValidator;
        #endregion Private Fields
    }
}
