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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using SampleCompany.NodeManagers.Simulation;
using Technosoftware.UaServer;
#endregion Using Directives

namespace SampleCompany.NodeManagers
{
    /// <summary>
    /// Helpers to find node managers implemented in this library.
    /// </summary>
    public static class NodeManagerUtils
    {
        /// <summary>
        /// Add all available node manager factories to the server.
        /// </summary>
        public static void AddDefaultNodeManagers(UaStandardServer server)
        {
            foreach (IUaNodeManagerFactory nodeManagerFactory in NodeManagerFactories)
            {
                server.AddNodeManager(nodeManagerFactory);
            }
        }

        /// <summary>
        /// The property with available node manager factories.
        /// </summary>
        public static ReadOnlyList<IUaNodeManagerFactory> NodeManagerFactories
        {
            get
            {
                s_nodeManagerFactories ??= GetNodeManagerFactories();
                return new ReadOnlyList<IUaNodeManagerFactory>(s_nodeManagerFactories);
            }
        }

        /// <summary>
        /// Helper to determine the INodeManagerFactory by reflection.
        /// </summary>
        private static IUaNodeManagerFactory IsINodeManagerFactoryType(Type type)
        {
            System.Reflection.TypeInfo nodeManagerTypeInfo = type.GetTypeInfo();
            if (nodeManagerTypeInfo.IsAbstract ||
                !typeof(IUaNodeManagerFactory).IsAssignableFrom(type))
            {
                return null;
            }
            return Activator.CreateInstance(type) as IUaNodeManagerFactory;
        }

        /// <summary>
        /// Enumerates all node manager factories.
        /// </summary>
        private static List<IUaNodeManagerFactory> GetNodeManagerFactories()
        {
            Assembly assembly = typeof(NodeManagerUtils).Assembly;
            IEnumerable<IUaNodeManagerFactory> nodeManagerFactories = assembly
                .GetExportedTypes()
                .Select(IsINodeManagerFactoryType)
                .Where(type => type != null);
            return [.. nodeManagerFactories];
        }

        private static IList<IUaNodeManagerFactory> s_nodeManagerFactories;
    }
}
