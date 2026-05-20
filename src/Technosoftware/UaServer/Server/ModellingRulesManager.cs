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
using System.Threading;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// An object that manages modelling rules supported by the server.
    /// </summary>
    public class ModellingRulesManager : IDisposable
    {
        /// <summary>
        /// Initializes the manager.
        /// </summary>
        public ModellingRulesManager(IUaServerData server)
        {
            m_server = server;
            m_modellingRules = [];
        }

        /// <summary>
        /// Frees any unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// An overrideable version of the Dispose.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TBD
            }
        }

        /// <summary>
        /// Checks if the modelling rule is supported by the server.
        /// </summary>
        /// <param name="modellingRuleId">The id of the modelling rule.</param>
        /// <returns>True if the modelling rule is supported.</returns>
        public bool IsSupported(NodeId modellingRuleId)
        {
            if (NodeId.IsNull(modellingRuleId))
            {
                return false;
            }

            lock (m_lock)
            {
                return m_modellingRules.ContainsKey(modellingRuleId);
            }
        }

        /// <summary>
        /// Registers a modelling rule.
        /// </summary>
        /// <param name="modellingRuleId">The id of the modelling rule.</param>
        /// <param name="modellingRuleName">The name of the modelling rule.</param>
        public void RegisterModellingRule(
            NodeId modellingRuleId,
            string modellingRuleName)
        {
            lock (m_lock)
            {
                m_modellingRules[modellingRuleId] = modellingRuleName;
            }

            m_server?.DiagnosticsNodeManager.AddModellingRule(modellingRuleId, modellingRuleName);
        }

        /// <summary>
        /// Unregisters a modelling rule.
        /// </summary>
        /// <param name="modellingRuleId">The id of the modelling rule.</param>
        public void UnregisterModellingRule(NodeId modellingRuleId)
        {
            lock (m_lock)
            {
                m_modellingRules.Remove(modellingRuleId);
            }
        }

        private readonly Lock m_lock = new();
        private readonly IUaServerData m_server;
        private readonly NodeIdDictionary<string> m_modellingRules;
    }
}
