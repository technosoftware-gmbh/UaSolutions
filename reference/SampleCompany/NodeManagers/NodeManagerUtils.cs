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
using Opc.Ua;
using SampleCompany.NodeManagers.Reference;
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
        /// Applies custom settings to reference servers for CTT run.
        /// </summary>
        public static void ApplyCTTMode(TextWriter output, UaStandardServer server)
        {
            var methodsToCall = new CallMethodRequestCollection();
            int index = server.CurrentInstance.NamespaceUris.GetIndex(Alarms.Namespaces.Alarms);
            if (index > 0)
            {
                try
                {
                    methodsToCall.Add(
                        // Start the Alarms with infinite runtime
                        new CallMethodRequest
                        {
                            MethodId = new NodeId("Alarms.Start", (ushort)index),
                            ObjectId = new NodeId("Alarms", (ushort)index),
                            InputArguments = [new Variant(uint.MaxValue)]
                        });
                    var requestHeader = new RequestHeader
                    {
                        Timestamp = DateTime.UtcNow,
                        TimeoutHint = 10000
                    };
                    var context = new UaServerOperationContext(requestHeader, RequestType.Call);
                    server.CurrentInstance.NodeManager.Call(
                        context,
                        methodsToCall,
                        out CallMethodResultCollection results,
                        out DiagnosticInfoCollection diagnosticInfos);
                    foreach (CallMethodResult result in results)
                    {
                        if (ServiceResult.IsBad(result.StatusCode))
                        {
                            Opc.Ua.Utils.LogError("Error calling method {0}.", result.StatusCode);
                        }
                    }
                    output.WriteLine("The Alarms for CTT mode are active.");
                    return;
                }
                catch (Exception ex)
                {
                    Opc.Ua.Utils.LogError(ex, "Failed to start alarms for CTT.");
                }
            }
            output.WriteLine(
                "The alarms could not be enabled for CTT, the namespace does not exist.");
        }

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
        /// Add all available node manager factories to the server.
        /// </summary>
        public static void UseSamplingGroupsInReferenceNodeManager(
            ReferenceServer server)
        {
            server.UseSamplingGroupsInReferenceNodeManager = true;
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

        #region Private Fields
        private static IList<IUaNodeManagerFactory> s_nodeManagerFactories;
        #endregion Private Fields
    }
}
