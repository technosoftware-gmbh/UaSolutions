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
using System;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// A node manager the diagnostic information exposed by the server.
    /// </summary>
    public interface IUaDiagnosticsNodeManager : IUaStandardNodeManager, INodeIdFactory
    {
        /// <summary>
        /// True if diagnostics are currently enabled.
        /// </summary>
        bool DiagnosticsEnabled { get; }

        /// <summary>
        /// Adds an aggregate function to the server capabilities object.
        /// </summary>
        void AddAggregateFunction(NodeId aggregateId, string aggregateName, bool isHistorical);

        /// <summary>
        /// Adds a modelling rule to the server capabilities object.
        /// </summary>
        void AddModellingRule(NodeId modellingRuleId, string modellingRuleName);

        /// <summary>
        /// Creates the diagnostics node for the server.
        /// </summary>
        void CreateServerDiagnostics(
            UaServerContext systemContext,
            ServerDiagnosticsSummaryDataType diagnostics,
            NodeValueSimpleEventHandler updateCallback);

        /// <summary>
        /// Creates the diagnostics node for a session.
        /// </summary>
        NodeId CreateSessionDiagnostics(
            UaServerContext systemContext,
            SessionDiagnosticsDataType diagnostics,
            NodeValueSimpleEventHandler updateCallback,
            SessionSecurityDiagnosticsDataType securityDiagnostics,
            NodeValueSimpleEventHandler updateSecurityCallback);

        /// <summary>
        /// Creates the diagnostics node for a subscription.
        /// </summary>
        NodeId CreateSubscriptionDiagnostics(
            UaServerContext systemContext,
            SubscriptionDiagnosticsDataType diagnostics,
            NodeValueSimpleEventHandler updateCallback);

        /// <summary>
        /// Delete the diagnostics node for a session.
        /// </summary>
        void DeleteSessionDiagnostics(UaServerContext systemContext, NodeId nodeId);

        /// <summary>
        /// Delete the diagnostics node for a subscription.
        /// </summary>
        void DeleteSubscriptionDiagnostics(UaServerContext systemContext, NodeId nodeId);

        /// <summary>
        /// Finds the specified and checks if it is of the expected type.
        /// </summary>
        /// <returns>Returns null if not found or not of the correct type.</returns>
        NodeState FindPredefinedNode(NodeId nodeId, Type expectedType);

        /// <summary>
        /// Force out of band diagnostics update after a change of diagnostics variables.
        /// </summary>
        void ForceDiagnosticsScan();

        /// <summary>
        /// Gets the default history capabilities object.
        /// </summary>
        HistoryServerCapabilitiesState GetDefaultHistoryCapabilities();

        /// <summary>
        /// Sets the flag controlling whether diagnostics is enabled for the server.
        /// </summary>
        void SetDiagnosticsEnabled(UaServerContext context, bool enabled);

        /// <summary>
        /// Updates the Server object EventNotifier based on history capabilities.
        /// </summary>
        /// <remarks>
        /// This method can be overridden to customize the Server EventNotifier based on
        /// history capabilities settings.
        /// </remarks>
        void UpdateServerEventNotifier();
    }
}
