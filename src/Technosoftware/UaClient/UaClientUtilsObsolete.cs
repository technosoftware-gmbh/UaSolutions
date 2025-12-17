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

#nullable disable

#region Using Directives
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaClient
{
    /// <summary>
    /// Defines numerous re-useable utility functions for clients.
    /// </summary>
    public static partial class UaClientUtils
    {
        /// <summary>
        /// Discovers the servers on the local machine.
        /// </summary>
        [Obsolete("Use DiscoverServersAsync instead.")]
        public static IList<string> DiscoverServers(
            ApplicationConfiguration configuration)
        {
            return DiscoverServersAsync(
                configuration,
                DefaultDiscoverTimeout,
                null).AsTask().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Discovers the servers on the local machine.
        /// </summary>
        [Obsolete("Use DiscoverServersAsync instead.")]
        public static IList<string> DiscoverServers(
            ApplicationConfiguration configuration,
            int discoverTimeout)
        {
            return DiscoverServersAsync(
                configuration,
                discoverTimeout,
                null).AsTask().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Finds the endpoint that best matches the current settings.
        /// </summary>
        [Obsolete("Use SelectEndpointAsync instead.")]
        public static EndpointDescription SelectEndpoint(
            ApplicationConfiguration application,
            ITransportWaitingConnection connection,
            bool useSecurity)
        {
            return SelectEndpointAsync(
                application,
                connection,
                useSecurity,
                DefaultDiscoverTimeout,
                null).AsTask().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Finds the endpoint that best matches the current settings.
        /// </summary>
        [Obsolete("Use SelectEndpointAsync instead.")]
        public static EndpointDescription SelectEndpoint(
            ApplicationConfiguration application,
            ITransportWaitingConnection connection,
            bool useSecurity,
            int discoverTimeout)
        {
            return SelectEndpointAsync(
                application,
                connection,
                useSecurity,
                discoverTimeout,
                null).AsTask().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Finds the endpoint that best matches the current settings.
        /// </summary>
        [Obsolete("Use SelectEndpointAsync instead.")]
        public static EndpointDescription SelectEndpoint(
            ApplicationConfiguration application,
            string discoveryUrl,
            bool useSecurity)
        {
            return SelectEndpointAsync(
                application,
                discoveryUrl,
                useSecurity,
                DefaultDiscoverTimeout,
                null).AsTask().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Finds the endpoint that best matches the current settings.
        /// </summary>
        [Obsolete("Use SelectEndpointAsync instead.")]
        public static EndpointDescription SelectEndpoint(
            ApplicationConfiguration application,
            string discoveryUrl,
            bool useSecurity,
            int discoverTimeout)
        {
            return SelectEndpointAsync(
                application,
                discoveryUrl,
                useSecurity,
                discoverTimeout,
                null).AsTask().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Finds the endpoint that best matches the current settings.
        /// </summary>
        [Obsolete("Use SelectEndpointAsync with ITelemetryContext parameter instead.")]
        public static ValueTask<EndpointDescription> SelectEndpointAsync(
            ApplicationConfiguration application,
            ITransportWaitingConnection connection,
            bool useSecurity,
            CancellationToken ct = default)
        {
            return SelectEndpointAsync(
                application,
                connection,
                useSecurity,
                DefaultDiscoverTimeout,
                null,
                ct);
        }

        /// <summary>
        /// Finds the endpoint that best matches the current settings.
        /// </summary>
        [Obsolete("Use SelectEndpointAsync with ITelemetryContext parameter instead.")]
        public static ValueTask<EndpointDescription> SelectEndpointAsync(
            ApplicationConfiguration application,
            ITransportWaitingConnection connection,
            bool useSecurity,
            int discoverTimeout,
            CancellationToken ct = default)
        {
            return SelectEndpointAsync(
                application,
                connection,
                useSecurity,
                discoverTimeout,
                null,
                ct);
        }

        /// <summary>
        /// Selects the endpoint that best matches the current settings.
        /// </summary>
        /// <param name="application"></param>
        /// <param name="discoveryUrl"></param>
        /// <param name="useSecurity"></param>
        /// <param name="discoverTimeout"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [Obsolete("Use SelectEndpointAsync with ITelemetryContext parameter instead.")]
        public static ValueTask<EndpointDescription> SelectEndpointAsync(
            ApplicationConfiguration application,
            string discoveryUrl,
            bool useSecurity,
            int discoverTimeout,
            CancellationToken ct = default)
        {
            return SelectEndpointAsync(
                application,
                discoveryUrl,
                useSecurity,
                discoverTimeout,
                null,
                ct);
        }
    }
}
