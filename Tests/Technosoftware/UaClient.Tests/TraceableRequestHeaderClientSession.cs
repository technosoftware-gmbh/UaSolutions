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
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaClient
{
    /// <summary>
    /// A subclass of a session to override some implementations from CleintBase
    /// </summary>
    public class TraceableRequestHeaderClientSession : Session
    {
        /// <inheritdoc/>
        public TraceableRequestHeaderClientSession(
            ITransportChannel channel,
            ApplicationConfiguration configuration,
            ConfiguredEndpoint endpoint,
            X509Certificate2 clientCertificate,
            EndpointDescriptionCollection availableEndpoints = null,
            StringCollection discoveryProfileUris = null)
            : base(
                channel,
                configuration,
                endpoint,
                clientCertificate,
                null,
                availableEndpoints,
                discoveryProfileUris)
        {
        }

        /// <inheritdoc/>
        public TraceableRequestHeaderClientSession(
            ITransportChannel channel,
            Session template,
            bool copyEventHandlers)
            : base(channel, template, copyEventHandlers)
        {
        }

        /// <summary>
        /// Populates AdditionalParameters with details from the ActivityContext
        /// </summary>
        public static void InjectTraceIntoAdditionalParameters(
            ActivityContext context,
            out AdditionalParametersType traceData)
        {
            // https://reference.opcfoundation.org/Core/Part26/v105/docs/5.5.4
            Span<byte> spanId = stackalloc byte[8];
            Span<byte> traceId = stackalloc byte[16];
            context.SpanId.CopyTo(spanId);
            context.TraceId.CopyTo(traceId);
            var spanContextParameter = new KeyValuePair
            {
                Key = "SpanContext",
                Value = new Variant(new SpanContextDataType
                {
#if NET8_0_OR_GREATER
                    SpanId = BitConverter.ToUInt64(spanId),
                    TraceId = (Uuid)new Guid(traceId)
#else
                    SpanId = BitConverter.ToUInt64(spanId.ToArray(), 0),
                    TraceId = (Uuid)new Guid(traceId.ToArray())
#endif
                })
            };
            traceData = new AdditionalParametersType();
            traceData.Parameters.Add(spanContextParameter);
        }

        ///<inheritdoc/>
        protected override void UpdateRequestHeader(IServiceRequest request, bool useDefaults)
        {
            base.UpdateRequestHeader(request, useDefaults);

            if (Activity.Current != null)
            {
                InjectTraceIntoAdditionalParameters(
                    Activity.Current.Context,
                    out AdditionalParametersType traceData);

                if (request.RequestHeader.AdditionalHeader == null)
                {
                    request.RequestHeader.AdditionalHeader = new ExtensionObject(traceData);
                }
                else if (request.RequestHeader.AdditionalHeader
                    .Body is AdditionalParametersType existingParameters)
                {
                    // Merge the trace data into the existing parameters.
                    existingParameters.Parameters.AddRange(traceData.Parameters);
                }
            }
        }
    }
}
