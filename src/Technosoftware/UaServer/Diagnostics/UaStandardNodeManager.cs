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

#region Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

using Opc.Ua;
using Opc.Ua.Test;
using Range = Opc.Ua.Range;

using Technosoftware.UaServer.Aggregates;
using Technosoftware.UaServer.NodeManager;
using Technosoftware.UaServer.Diagnostics;
using Technosoftware.UaServer.Subscriptions;
#endregion

namespace Technosoftware.UaServer
{
    /// <summary>
    /// A base implementation of the IUaStandardNodeManager interface.
    /// </summary>
    /// <remarks>
    /// This node manager is a base class used in multiple samples. It implements the IUaStandardNodeManager
    /// interface and allows sub-classes to override only the methods that they need.
    /// </remarks>
    public class UaStandardNodeManager : IUaStandardNodeManager, INodeIdFactory, IDisposable
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Initializes the node manager.
        /// </summary>
        /// <param name="server">The server data implementing the IUaServerData interface.</param>
        /// <param name="namespaceUris">Array of namespaces that are used by the application.</param>
        protected UaStandardNodeManager(
            IUaServerData server,
            params string[] namespaceUris)
        :
            this(server, (ApplicationConfiguration)null, namespaceUris)
        {
        }

        /// <summary>
        /// Initializes the node manager.
        /// </summary>
        /// <param name="server">The server data implementing the IUaServerData interface.</param>
        /// <param name="configuration">The used application configuration.</param>
        /// <param name="namespaceUris">Array of namespaces that are used by the application.</param>
        protected UaStandardNodeManager(
            IUaServerData server,
            ApplicationConfiguration configuration,
            params string[] namespaceUris)
        {
            // set defaults.
            m_maxQueueSize = 1000;
            m_maxDurableQueueSize = 200000; //default value in deprecated Conformance Unit Subscription Durable StorageLevel High

            if (configuration?.ServerConfiguration != null)
            {
                m_maxQueueSize = (uint)configuration.ServerConfiguration.MaxNotificationQueueSize;
                m_maxDurableQueueSize = (uint)configuration.ServerConfiguration.MaxDurableNotificationQueueSize;
            }

            // save a reference to the UA server instance that owns the node manager.
            m_server = server;

            // all operations require information about the system
            m_systemContext = m_server.DefaultSystemContext.Copy();

            // the node id factory assigns new node ids to new nodes.
            // the strategy used by a NodeManager depends on what kind of information it provides.
            m_systemContext.NodeIdFactory = this;

            // add the uris to the server's namespace table and cache the indexes.
            ushort[] namespaceIndexes = Array.Empty<ushort>();
            if (namespaceUris != null)
            {
                namespaceIndexes = new ushort[namespaceUris.Length];

                for (int ii = 0; ii < namespaceUris.Length; ii++)
                {
                    namespaceIndexes[ii] = m_server.NamespaceUris.GetIndexOrAppend(namespaceUris[ii]);
                }
            }

            // add the table of namespaces that are used by the NodeManager.
            m_namespaceUris = namespaceUris;
            m_namespaceIndexes = namespaceIndexes;

            // create the table of monitored nodes.
            // these are created by the node manager whenever a client subscribe to an attribute of the node.
            m_monitoredNodes = new NodeIdDictionary<UaMonitoredNode>();

            m_predefinedNodes = new NodeIdDictionary<NodeState>();
        }
        #endregion

        #region IDisposable Members
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
                lock (Lock)
                {
                    foreach (NodeState node in m_predefinedNodes.Values)
                    {
                        Utils.SilentDispose(node);
                    }

                    m_predefinedNodes.Clear();
                }
            }
        }
        #endregion

        #region INodeIdFactory Members
        /// <summary>
        /// Creates the NodeId for the specified node.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="node">The node.</param>
        /// <returns>The new NodeId.</returns>
        public virtual NodeId Create(ISystemContext context, NodeState node)
        {
            return node.NodeId;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Acquires the lock on the node manager.
        /// </summary>
        public object Lock
        {
            get { return m_lock; }
        }

        /// <summary>
        /// Gets the server that the node manager belongs to.
        /// </summary>
        public IUaServerData ServerData
        {
            get { return m_server; }
        }

        /// <summary>
        /// The default context to use.
        /// </summary>
        public UaServerContext SystemContext
        {
            get { return m_systemContext; }
        }

        /// <summary>
        /// Gets the default index for the node manager's namespace.
        /// </summary>
        public ushort NamespaceIndex
        {
            get { return m_namespaceIndexes[0]; }
        }

        /// <summary>
        /// Gets the namespace indexes owned by the node manager.
        /// </summary>
        /// <value>The namespace indexes.</value>
        public IReadOnlyList<ushort> NamespaceIndexes
        {
            get { return m_namespaceIndexes; }
        }

        /// <summary>
        /// Gets or sets the maximum size of a monitored item queue.
        /// </summary>
        /// <value>The maximum size of a monitored item queue.</value>
        public uint MaxQueueSize
        {
            get { return m_maxQueueSize; }
            set { m_maxQueueSize = value; }
        }

        /// <summary>
        /// Gets or sets the maximum size of a durable monitored item queue.
        /// </summary>
        /// <value>The maximum size of a durable monitored item queue.</value>
        public uint MaxDurableQueueSize
        {
            get { return m_maxDurableQueueSize; }
            set { m_maxDurableQueueSize = value; }
        }

        /// <summary>
        /// The root for the alias assigned to the node manager.
        /// </summary>
        public string AliasRoot
        {
            get { return m_aliasRoot; }
            set { m_aliasRoot = value; }
        }
        #endregion

        #region Protected Members
        /// <summary>
        /// The predefined nodes managed by the node manager.
        /// </summary>
        protected NodeIdDictionary<NodeState> PredefinedNodes => m_predefinedNodes;

        /// <summary>
        /// The root notifiers for the node manager.
        /// </summary>
        protected List<NodeState> RootNotifiers
        {
            get { return m_rootNotifiers; }
        }

        /// <summary>
        /// Gets the table of nodes being monitored.
        /// </summary>
        protected NodeIdDictionary<UaMonitoredNode> MonitoredNodes => m_monitoredNodes;

        /// <summary>
        /// Sets the namespaces supported by the NodeManager.
        /// </summary>
        /// <param name="namespaceUris">The namespace uris.</param>
        protected void SetNamespaces(params string[] namespaceUris)
        {
            // add the uris to the server's namespace table and cache the indexes.
            var namespaceIndexes = new ushort[namespaceUris.Length];

            for (int ii = 0; ii < namespaceUris.Length; ii++)
            {
                namespaceIndexes[ii] = m_server.NamespaceUris.GetIndexOrAppend(namespaceUris[ii]);
            }

            // create the immutable table of namespaces that are used by the NodeManager.
            m_namespaceUris = namespaceUris;
            m_namespaceIndexes = namespaceIndexes;
        }

        /// <summary>
        /// Sets the namespace indexes supported by the NodeManager.
        /// </summary>
        protected void SetNamespaceIndexes(ushort[] namespaceIndexes)
        {
            var namespaceUris = new string[namespaceIndexes.Length];

            for (int ii = 0; ii < namespaceIndexes.Length; ii++)
            {
                namespaceUris[ii] = m_server.NamespaceUris.GetString(namespaceIndexes[ii]);
            }

            // create the immutable table of namespaces that are used by the NodeManager.
            m_namespaceUris = namespaceUris;
            m_namespaceIndexes = namespaceIndexes;
        }

        /// <summary>
        /// Returns true if the namespace for the node id is one of the namespaces managed by the node manager.
        /// </summary>
        /// <remarks>
        /// It is thread safe to call this method outside the node manager lock.
        /// </remarks>
        /// <param name="nodeId">The node id to check.</param>
        /// <returns>True if the namespace is one of the nodes.</returns>
        protected virtual bool IsNodeIdInNamespace(NodeId nodeId)
        {
            // nulls are never a valid node.
            if (NodeId.IsNull(nodeId))
            {
                return false;
            }

            // quickly exclude nodes that not in the namespace.
            return m_namespaceIndexes.Contains(nodeId.NamespaceIndex);
        }

        /// <summary>
        /// Returns the node if the handle refers to a node managed by this manager.
        /// </summary>
        /// <remarks>
        /// It is thread safe to call this method outside the node manager lock.
        /// </remarks>
        /// <param name="managerHandle">The handle to check.</param>
        /// <returns>Non-null if the handle belongs to the node manager.</returns>
        protected virtual UaNodeHandle IsHandleInNamespace(object managerHandle)
        {
            UaNodeHandle source = managerHandle as UaNodeHandle;

            if (source == null)
            {
                return null;
            }

            if (!IsNodeIdInNamespace(source.NodeId))
            {
                return null;
            }

            return source;
        }

        /// <summary>
        /// Returns the state object for the specified node if it exists.
        /// </summary>
        public NodeState Find(NodeId nodeId)
        {
            NodeState node = null;
            if (m_predefinedNodes.TryGetValue(nodeId, out node) == true)
            {
                return node;
            }

            return null;
        }

        /// <summary>
        /// Creates a new instance and assigns unique identifiers to all children.
        /// </summary>
        /// <param name="context">The operation context.</param>
        /// <param name="parentId">An optional parent identifier.</param>
        /// <param name="referenceTypeId">The reference type from the parent.</param>
        /// <param name="browseName">The browse name.</param>
        /// <param name="instance">The instance to create.</param>
        /// <returns>The new node id.</returns>
        public NodeId CreateNode(
            UaServerContext context,
            NodeId parentId,
            NodeId referenceTypeId,
            QualifiedName browseName,
            BaseInstanceState instance)
        {
            UaServerContext contextToUse = m_systemContext.Copy(context);

            lock (Lock)
            {
                instance.ReferenceTypeId = referenceTypeId;

                NodeState parent = null;

                if (parentId != null)
                {
                    if (!m_predefinedNodes.TryGetValue(parentId, out parent))
                    {
                        throw ServiceResultException.Create(
                            StatusCodes.BadNodeIdUnknown,
                            "Cannot find parent with id: {0}",
                            parentId);
                    }

                    parent.AddChild(instance);
                }

                instance.Create(contextToUse, null, browseName, null, true);
                AddPredefinedNode(contextToUse, instance);

                return instance.NodeId;
            }
        }

        /// <summary>
        /// Deletes a node and all of its children.
        /// </summary>
        public bool DeleteNode(
            UaServerContext context,
            NodeId nodeId)
        {
            UaServerContext contextToUse = m_systemContext.Copy(context);

            List<LocalReference> referencesToRemove = new List<LocalReference>();

            NodeState node = null;
            if (m_predefinedNodes.TryGetValue(nodeId, out node) != true)
            {
                return false;
            }

            RemovePredefinedNode(contextToUse, node, referencesToRemove);
            RemoveRootNotifier(node);

            if (referencesToRemove.Count > 0)
            {
                ServerData.NodeManager.RemoveReferences(referencesToRemove);
            }

            return true;
        }

        /// <summary>
        /// Searches the node id in all node managers
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public NodeState FindNodeInAddressSpace(NodeId nodeId)
        {
            if (nodeId == null)
            {
                return null;
            }
            // search node id in all node managers
            foreach (IUaNodeManager nodeManager in ServerData.NodeManager.NodeManagers)
            {
                UaNodeHandle handle = nodeManager.GetManagerHandle(nodeId) as UaNodeHandle;
                if (handle == null)
                {
                    continue;
                }
                return handle.Node;
            }
            return null;
        }
        #endregion

        #region IUaNodeManager Members
        /// <summary>
        /// Returns the namespaces used by the node manager.
        /// </summary>
        /// <remarks>
        /// All NodeIds exposed by the node manager must be qualified by a namespace URI. This property
        /// returns the URIs used by the node manager. In this example all NodeIds use a single URI.
        /// </remarks>
        public virtual IEnumerable<string> NamespaceUris
        {
            get
            {
                return m_namespaceUris;
            }

            protected set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                List<string> namespaceUris = new List<string>(value);
                SetNamespaces(namespaceUris.ToArray());
            }
        }

        /// <summary>
        /// Does any initialization required before the address space can be used.
        /// </summary>
        /// <param name="externalReferences">
        /// The externalReferences is an out parameter that allows the node manager to link to nodes
        /// in other node managers. For example, the 'Objects' node is managed by the CoreNodeManager and
        /// should have a reference to the root folder node(s) exposed by this node manager.
        /// </param>
        public virtual void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            LoadPredefinedNodes(m_systemContext, externalReferences);
        }

        #region CreateAddressSpace Support Functions
        /// <summary>
        /// Loads a node set from a file or resource and adds them to the set of predefined nodes.
        /// </summary>
        /// <param name="context">The UA server implementation of the ISystemContext interface.</param>
        /// <param name="resourcePath">The resource path.</param>
        /// <param name="assembly">The assembly containing the resource.</param>
        /// <param name="externalReferences"></param>
        public virtual void LoadPredefinedNodes(
            ISystemContext context,
            Assembly assembly,
            string resourcePath,
            IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            // load the predefined nodes from an XML document.
            NodeStateCollection predefinedNodes = new NodeStateCollection();
            predefinedNodes.LoadFromResource(context, resourcePath, assembly, true);

            // add the predefined nodes to the node manager.
            for (int ii = 0; ii < predefinedNodes.Count; ii++)
            {
                AddPredefinedNode(context, predefinedNodes[ii]);
            }

            // ensure the reverse references exist.
            AddReverseReferences(externalReferences);
        }

        /// <summary>
        /// Loads a node set from a file or resource and adds them to the set of predefined nodes.
        /// </summary>
        /// <param name="context">The UA server implementation of the ISystemContext interface.</param>
        protected virtual NodeStateCollection LoadPredefinedNodes(ISystemContext context)
        {
            return new NodeStateCollection();
        }

        /// <summary>
        /// Loads a node set from a file or resource and adds them to the set of predefined nodes.
        /// </summary>
        /// <param name="context">The UA server implementation of the ISystemContext interface.</param>
        /// <param name="externalReferences">The externalReferences is an out parameter that allows the generic server to link to nodes.</param>
        protected virtual void LoadPredefinedNodes(
            ISystemContext context,
            IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            // load the predefined nodes from an XML document.
            NodeStateCollection predefinedNodes = LoadPredefinedNodes(context);

            // add the predefined nodes to the node manager.
            for (int ii = 0; ii < predefinedNodes.Count; ii++)
            {
                AddPredefinedNode(context, predefinedNodes[ii]);
            }

            // ensure the reverse references exist.
            AddReverseReferences(externalReferences);
        }

        /// <summary>
        /// Replaces the generic node with a node specific to the model.
        /// </summary>
        /// <param name="context">The UA server implementation of the ISystemContext interface.</param>
        /// <param name="predefinedNode">The predefined node.</param>
        protected virtual NodeState AddBehaviourToPredefinedNode(ISystemContext context, NodeState predefinedNode)
        {
            BaseObjectState passiveNode = predefinedNode as BaseObjectState;

            if (passiveNode == null)
            {
                return predefinedNode;
            }

            return predefinedNode;
        }

        /// <summary>
        /// Recursively indexes the node and its children and add them to the predefined nodes.
        /// </summary>
        /// <param name="context">The UA server implementation of the ISystemContext interface.</param>
        /// <param name="node">The node to add as predefined node.</param>
        public virtual void AddPredefinedNode(ISystemContext context, NodeState node)
        {
            // assign a default value to any variable in namespace 0
            if (node is BaseVariableState nodeStateVar)
            {
                if (nodeStateVar.NodeId.NamespaceIndex == 0 && nodeStateVar.Value == null)
                {
                    nodeStateVar.Value = Opc.Ua.TypeInfo.GetDefaultValue(nodeStateVar.DataType,
                        nodeStateVar.ValueRank,
                        ServerData.TypeTree);
                }
            }

            NodeState activeNode = AddBehaviourToPredefinedNode(context, node);
            m_predefinedNodes.AddOrUpdate(activeNode.NodeId, activeNode, (key, _) => activeNode);

            BaseTypeState type = activeNode as BaseTypeState;

            if (type != null)
            {
                AddTypesToTypeTree(type);
            }
            lock (Lock)
            {
                // update the root notifiers.
                if (m_rootNotifiers != null)
                {
                    for (int ii = 0; ii < m_rootNotifiers.Count; ii++)
                    {
                        if (m_rootNotifiers[ii].NodeId == activeNode.NodeId)
                        {
                            m_rootNotifiers[ii] = activeNode;

                            // need to prevent recursion with the server object.
                            if (activeNode.NodeId != ObjectIds.Server)
                            {
                                activeNode.OnReportEvent = OnReportEvent;

                                if (!activeNode.ReferenceExists(ReferenceTypeIds.HasNotifier, true, ObjectIds.Server))
                                {
                                    activeNode.AddReference(ReferenceTypeIds.HasNotifier, true, ObjectIds.Server);
                                }
                            }

                            break;
                        }
                    }
                }
            }

            List<BaseInstanceState> children = new List<BaseInstanceState>();
            activeNode.GetChildren(context, children);

            for (int ii = 0; ii < children.Count; ii++)
            {
                AddPredefinedNode(context, children[ii]);
            }
        }

        /// <summary>
        /// Recursively indexes the node and its children and removes  them from the predefined nodes.
        /// </summary>
        /// <param name="context">The UA server implementation of the ISystemContext interface.</param>
        /// <param name="node">The node to remove from the predefined nodes.</param>
        /// <param name="referencesToRemove">The references to remove.</param>
        protected virtual void RemovePredefinedNode(
            ISystemContext context,
            NodeState node,
            List<LocalReference> referencesToRemove)
        {
            if (m_predefinedNodes.TryRemove(node.NodeId, out _) != true)
            {
                return;
            }
            node.UpdateChangeMasks(NodeStateChangeMasks.Deleted);
            node.ClearChangeMasks(context, false);
            OnNodeRemoved(node);

            // remove from the parent.
            if (node is BaseInstanceState instance && instance.Parent != null)
            {
                instance.Parent.RemoveChild(instance);
            }

            // remove children.
            List<BaseInstanceState> children = new List<BaseInstanceState>();
            node.GetChildren(context, children);

            for (int ii = 0; ii < children.Count; ii++)
            {
                node.RemoveChild(children[ii]);
            }

            for (int ii = 0; ii < children.Count; ii++)
            {
                RemovePredefinedNode(context, children[ii], referencesToRemove);
            }

            // remove from type table.
            BaseTypeState type = node as BaseTypeState;

            if (type != null)
            {
                m_server.TypeTree.Remove(type.NodeId);
            }

            // remove inverse references.
            List<IReference> references = new List<IReference>();
            node.GetReferences(context, references);

            for (int ii = 0; ii < references.Count; ii++)
            {
                IReference reference = references[ii];

                if (reference.TargetId.IsAbsolute)
                {
                    continue;
                }

                LocalReference referenceToRemove = new LocalReference(
                    (NodeId)reference.TargetId,
                    reference.ReferenceTypeId,
                    !reference.IsInverse,
                    node.NodeId);

                referencesToRemove.Add(referenceToRemove);
            }
        }

        /// <summary>
        /// Called after a node has been deleted.
        /// </summary>
        /// <param name="node">The removed node.</param>
        protected virtual void OnNodeRemoved(NodeState node)
        {
            // overridden by the sub-class.
        }

        /// <summary>
        /// Ensures that all reverse references exist.
        /// </summary>
        /// <param name="externalReferences">A list of references to add to external targets.</param>
        public virtual void AddReverseReferences(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            foreach (NodeState source in m_predefinedNodes.Values)
            {
                IList<IReference> references = new List<IReference>();
                source.GetReferences(SystemContext, references);

                for (int ii = 0; ii < references.Count; ii++)
                {
                    IReference reference = references[ii];

                    // nothing to do with external nodes.
                    if (reference.TargetId == null || reference.TargetId.IsAbsolute)
                    {
                        continue;
                    }

                    // no need to add HasSubtype references since these are handled via the type table.
                    if (reference.ReferenceTypeId == ReferenceTypeIds.HasSubtype)
                    {
                        continue;
                    }

                    NodeId targetId = (NodeId)reference.TargetId;

                    // check for data type encoding references.
                    if (reference.IsInverse && reference.ReferenceTypeId == ReferenceTypeIds.HasEncoding)
                    {
                        ServerData.TypeTree.AddEncoding(targetId, source.NodeId);
                    }

                    // add inverse reference to internal targets.
                    NodeState target = null;

                    if (m_predefinedNodes.TryGetValue(targetId, out target))
                    {
                        if (!target.ReferenceExists(reference.ReferenceTypeId, !reference.IsInverse, source.NodeId))
                        {
                            target.AddReference(reference.ReferenceTypeId, !reference.IsInverse, source.NodeId);
                        }

                        continue;
                    }

                    // check for inverse references to external notifiers.
                    if (reference.IsInverse && reference.ReferenceTypeId == ReferenceTypeIds.HasNotifier)
                    {
                        AddRootNotifier(source);
                    }

                    // nothing more to do for references to nodes managed by this manager.
                    if (IsNodeIdInNamespace(targetId))
                    {
                        continue;
                    }

                    // add external reference.
                    AddExternalReference(
                        targetId,
                        reference.ReferenceTypeId,
                        !reference.IsInverse,
                        source.NodeId,
                        externalReferences);
                }
            }
        }

        /// <summary>
        /// Adds an external reference to the dictionary.
        /// </summary>
        /// <param name="sourceId">The ID of the source node.</param>
        /// <param name="referenceTypeId">The ID of the reference type.</param>
        /// <param name="isInverse">Is the reference an inverse reference?</param>
        /// <param name="targetId">The ID of the target node.</param>
        /// <param name="externalReferences">The externalReferences is an out parameter that allows the generic server to link to nodes.</param>
        protected void AddExternalReference(
            NodeId sourceId,
            NodeId referenceTypeId,
            bool isInverse,
            NodeId targetId,
            IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            // get list of references to external nodes.
            IList<IReference> referencesToAdd = null;

            if (!externalReferences.TryGetValue(sourceId, out referencesToAdd))
            {
                externalReferences[sourceId] = referencesToAdd = new List<IReference>();
            }

            // add reserve reference from external node.
            ReferenceNode referenceToAdd = new ReferenceNode
            {
                ReferenceTypeId = referenceTypeId,
                IsInverse = isInverse,
                TargetId = targetId
            };

            referencesToAdd.Add(referenceToAdd);
        }

        /// <summary>
        /// Recursively adds the types to the type tree.
        /// </summary>
        /// <param name="type">The type to add.</param>
        protected void AddTypesToTypeTree(BaseTypeState type)
        {
            if (!NodeId.IsNull(type.SuperTypeId))
            {
                if (!ServerData.TypeTree.IsKnown(type.SuperTypeId))
                {
                    AddTypesToTypeTree(type.SuperTypeId);
                }
            }

            if (type.NodeClass != NodeClass.ReferenceType)
            {
                ServerData.TypeTree.AddSubtype(type.NodeId, type.SuperTypeId);
            }
            else
            {
                ServerData.TypeTree.AddReferenceSubtype(type.NodeId, type.SuperTypeId, type.BrowseName);
            }
        }

        /// <summary>
        /// Recursively adds the types to the type tree.
        /// </summary>
        /// <param name="typeId">The node ID of the type to add.</param>
        protected void AddTypesToTypeTree(NodeId typeId)
        {
            if (!m_predefinedNodes.TryGetValue(typeId, out NodeState node))
            {
                return;
            }


            if (!(node is BaseTypeState type))
            {
                return;
            }

            AddTypesToTypeTree(type);
        }

        /// <summary>
        /// Finds the specified and checks if it is of the expected type.
        /// </summary>
        /// <param name="nodeId">The node to find.</param>
        /// <param name="expectedType">The expected type of the node.</param>
        /// <returns>Returns null if not found or not of the correct type.</returns>
        public NodeState FindPredefinedNode(NodeId nodeId, Type expectedType)
        {
            if (nodeId == null)
            {
                return null;
            }

            if (!m_predefinedNodes.TryGetValue(nodeId, out NodeState node))
            {
                return null;
            }

            if (expectedType != null)
            {
                if (!expectedType.IsInstanceOfType(node))
                {
                    return null;
                }
            }

            return node;
        }
        #endregion

        /// <summary>
        /// Frees any resources allocated for the address space.
        /// </summary>
        public virtual void DeleteAddressSpace()
        {
            var nodes = m_predefinedNodes.Values.ToArray();
            m_predefinedNodes.Clear();

            foreach (NodeState node in nodes)
            {
                Utils.SilentDispose(node);
            }
        }

        /// <summary>
        /// Returns an unique handle for the node.
        /// </summary>
        /// <param name="nodeId">The node to get the handle for.</param>
        /// <returns>A node handle, null if the node manager does not recognize the node id.</returns>
        /// <remarks>
        /// This must efficiently determine whether the node belongs to the node manager. If it does belong to
        /// NodeManager it should return a handle that does not require the NodeId to be validated again when
        /// the handle is passed into other methods such as 'Read' or 'Write'.
        /// </remarks>
        public virtual object GetManagerHandle(NodeId nodeId)
        {
            return GetManagerHandle(m_systemContext, nodeId, null);
        }

        /// <summary>
        /// Returns an unique handle for the node.
        /// </summary>
        /// <remarks>
        /// It is thread safe to call this method outside the node manager lock.
        /// </remarks>
        /// <param name="context">The UA server implementation of the ISystemContext interface.</param>
        /// <param name="nodeId">The node to get the handle for.</param>
        /// <param name="cache">The list of nodes to check.</param>
        /// <returns>A node handle, null if the node manager does not recognize the node id.</returns>
        protected virtual UaNodeHandle GetManagerHandle(UaServerContext context, NodeId nodeId, IDictionary<NodeId, NodeState> cache)
        {
            if (!IsNodeIdInNamespace(nodeId))
            {
                return null;
            }

            NodeState node = null;
            if (m_predefinedNodes.TryGetValue(nodeId, out node) == true)
            {
                var handle = new UaNodeHandle
                {
                    NodeId = nodeId,
                    Node = node,
                    Validated = true
                };

                return handle;
            }
            return null;
        }

        /// <summary>
        /// This method is used to add bi-directional references to nodes from other node managers.
        /// </summary>
        /// <remarks>
        /// The additional references are optional, however, the NodeManager should support them.
        /// </remarks>
        public virtual void AddReferences(IDictionary<NodeId, IList<IReference>> references)
        {
            lock (Lock)
            {
                foreach (KeyValuePair<NodeId, IList<IReference>> current in references)
                {
                    // get the handle.
                    UaNodeHandle source = GetManagerHandle(m_systemContext, current.Key, null);

                    // only support external references to nodes that are stored in memory.
                    if (source?.Node == null || !source.Validated)
                    {
                        continue;
                    }


                    // add reference to external target.
                    foreach (IReference reference in current.Value)
                    {
                        if (!source.Node.ReferenceExists(reference.ReferenceTypeId, reference.IsInverse, reference.TargetId))
                        {
                            source.Node.AddReference(reference.ReferenceTypeId, reference.IsInverse, reference.TargetId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to delete bi-directional references to nodes from other node managers.
        /// </summary>
        public virtual ServiceResult DeleteReference(
            object sourceHandle,
            NodeId referenceTypeId,
            bool isInverse,
            ExpandedNodeId targetId,
            bool deleteBidirectional)
        {
            // get the handle.
            UaNodeHandle source = IsHandleInNamespace(sourceHandle);

            if (source == null)
            {
                return StatusCodes.BadNodeIdUnknown;
            }

            // only support external references to nodes that are stored in memory.
            if (!source.Validated || source.Node == null)
            {
                return StatusCodes.BadNotSupported;
            }

            lock (Lock)
            {
                // only support references to Source Areas.
                source.Node.RemoveReference(referenceTypeId, isInverse, targetId);

                if (deleteBidirectional)
                {
                    // check if the target is also managed by this node manager.
                    if (!targetId.IsAbsolute)
                    {
                        UaNodeHandle target = GetManagerHandle(m_systemContext, (NodeId)targetId, null);

                        if (target != null && target.Validated && target.Node != null)
                        {
                            target.Node.RemoveReference(referenceTypeId, !isInverse, source.NodeId);
                        }
                    }
                }

                return ServiceResult.Good;
            }
        }

        /// <summary>
        /// Returns the basic metadata for the node. Returns null if the node does not exist.
        /// </summary>
        /// <remarks>
        /// This method validates any placeholder handle.
        /// </remarks>
        /// <param name="context">The UA server implementation of the IOperationContext interface.</param>
        /// <param name="targetHandle">The node to get the basic metadata from.</param>
        /// <param name="resultMask">The returned basic metadata.</param>
        public virtual UaNodeMetadata GetNodeMetadata(
            UaServerOperationContext context,
            object targetHandle,
            BrowseResultMask resultMask)
        {
            UaServerContext systemContext = m_systemContext.Copy(context);

            // check for valid handle.
            UaNodeHandle handle = IsHandleInNamespace(targetHandle);

            if (handle == null)
            {
                return null;
            }

            lock (Lock)
            {
                // validate node.
                NodeState target = ValidateNode(systemContext, handle, null);

                if (target == null)
                {
                    return null;
                }

                // read the attributes.
                List<object> values = target.ReadAttributes(
                    systemContext,
                    Attributes.WriteMask,
                    Attributes.UserWriteMask,
                    Attributes.DataType,
                    Attributes.ValueRank,
                    Attributes.ArrayDimensions,
                    Attributes.AccessLevel,
                    Attributes.UserAccessLevel,
                    Attributes.EventNotifier,
                    Attributes.Executable,
                    Attributes.UserExecutable,
                    Attributes.AccessRestrictions,
                    Attributes.RolePermissions,
                    Attributes.UserRolePermissions);

                // construct the meta-data object.
                UaNodeMetadata metadata = new UaNodeMetadata(target, target.NodeId)
                {
                    NodeClass = target.NodeClass,
                    BrowseName = target.BrowseName,
                    DisplayName = target.DisplayName
                };

                if (values[0] != null && values[1] != null)
                {
                    metadata.WriteMask = (AttributeWriteMask)(((uint)values[0]) & ((uint)values[1]));
                }

                metadata.DataType = (NodeId)values[2];

                if (values[3] != null)
                {
                    metadata.ValueRank = (int)values[3];
                }

                metadata.ArrayDimensions = (IList<uint>)values[4];

                if (values[5] != null && values[6] != null)
                {
                    metadata.AccessLevel = (byte)(((byte)values[5]) & ((byte)values[6]));
                }

                if (values[7] != null)
                {
                    metadata.EventNotifier = (byte)values[7];
                }

                if (values[8] != null && values[9] != null)
                {
                    metadata.Executable = (((bool)values[8]) && ((bool)values[9]));
                }

                if (values[10] != null)
                {
                    metadata.AccessRestrictions = (AccessRestrictionType)Enum.ToObject(typeof(AccessRestrictionType), values[10]);
                }

                if (values[11] != null)
                {
                    metadata.RolePermissions = new RolePermissionTypeCollection(ExtensionObject.ToList<RolePermissionType>(values[11]));
                }

                if (values[12] != null)
                {
                    metadata.UserRolePermissions = new RolePermissionTypeCollection(ExtensionObject.ToList<RolePermissionType>(values[12]));
                }

                SetDefaultPermissions(systemContext, target, metadata);

                // get instance references.
                if (target is BaseInstanceState instance)
                {
                    metadata.TypeDefinition = instance.TypeDefinitionId;
                    metadata.ModellingRule = instance.ModellingRuleId;
                }

                // fill in the common attributes.
                return metadata;
            }
        }

        /// <summary>
        /// Sets the AccessRestrictions, RolePermissions and UserRolePermissions values in the metadata
        /// </summary>
        /// <param name="values"></param>
        /// <param name="metadata"></param>
        private static void SetAccessAndRolePermissions(List<object> values, UaNodeMetadata metadata)
        {
            if (values[0] != null)
            {
                metadata.AccessRestrictions = (AccessRestrictionType)Enum.ToObject(typeof(AccessRestrictionType), values[0]);
            }
            if (values[1] != null)
            {
                metadata.RolePermissions = new RolePermissionTypeCollection(ExtensionObject.ToList<RolePermissionType>(values[1]));
            }
            if (values[2] != null)
            {
                metadata.UserRolePermissions = new RolePermissionTypeCollection(ExtensionObject.ToList<RolePermissionType>(values[2]));
            }
        }

        /// <summary>
        /// Reads and caches the Attributes used by the AccessRestrictions and RolePermission validation process
        /// </summary>
        /// <param name="uniqueNodesServiceAttributesCache">The cache used to save the attributes</param>
        /// <param name="systemContext">The context</param>
        /// <param name="target">The target for which the attributes are read and cached</param>
        /// <param name="key">The key representing the NodeId for which the cache is kept</param>
        /// <returns>The values of the attributes</returns>
        private static List<object> ReadAndCacheValidationAttributes(Dictionary<NodeId, List<object>> uniqueNodesServiceAttributesCache, UaServerContext systemContext, NodeState target, NodeId key)
        {
            List<object> values = ReadValidationAttributes(systemContext, target);
            uniqueNodesServiceAttributesCache[key] = values;

            return values;
        }

        /// <summary>
        /// Reads the Attributes used by the AccessRestrictions and RolePermission validation process
        /// </summary>
        /// <param name="systemContext">The context</param>
        /// <param name="target">The target for which the attributes are read and cached</param>
        /// <returns>The values of the attributes</returns>
        private static List<object> ReadValidationAttributes(UaServerContext systemContext, NodeState target)
        {
            // This is the list of attributes to be populated by GetNodeMetadata from CustomNodeManagers.
            // The are originating from services in the context of AccessRestrictions and RolePermission validation.
            // For such calls the other attributes are ignored since reading them might trigger unnecessary callbacks
            List<object> values = target.ReadAttributes(systemContext,
                                           Attributes.AccessRestrictions,
                                           Attributes.RolePermissions,
                                           Attributes.UserRolePermissions);

            return values;
        }

        /// <summary>
        /// Browses the references from a node managed by the node manager.
        /// </summary>
        /// <remarks>
        /// The continuation point is created for every browse operation and contains the browse parameters.
        /// The node manager can store its state information in the Data and Index properties.
        /// </remarks>
        public virtual void Browse(
            UaServerOperationContext context,
            ref UaContinuationPoint continuationPoint,
            IList<ReferenceDescription> references)
        {
            if (continuationPoint == null) throw new ArgumentNullException(nameof(continuationPoint));
            if (references == null) throw new ArgumentNullException(nameof(references));

            UaServerContext systemContext = m_systemContext.Copy(context);

            // check for valid view.
            ValidateViewDescription(systemContext, continuationPoint.View);

            INodeBrowser browser = null;

            // check for valid handle.
            UaNodeHandle handle = IsHandleInNamespace(continuationPoint.NodeToBrowse);

            if (handle == null)
            {
                throw new ServiceResultException(StatusCodes.BadNodeIdUnknown);
            }
            lock (Lock)
            {
                // validate node.
                NodeState source = ValidateNode(systemContext, handle, null);

                if (source == null)
                {
                    throw new ServiceResultException(StatusCodes.BadNodeIdUnknown);
                }

                // check if node is in the view.
                if (!IsNodeInView(systemContext, continuationPoint, source))
                {
                    throw new ServiceResultException(StatusCodes.BadNodeNotInView);
                }

                // check if node is accessible for the user.
                if (!IsNodeAccessibleForUser(systemContext, continuationPoint, source))
                {
                    throw new ServiceResultException(StatusCodes.BadNodeIdUnknown);
                }

                // check for previous continuation point.
                browser = continuationPoint.Data as INodeBrowser;

                // fetch list of references.
                if (browser == null)
                {
                    // create a new browser.
                    continuationPoint.Data = browser = source.CreateBrowser(
                        systemContext,
                        continuationPoint.View,
                        continuationPoint.ReferenceTypeId,
                        continuationPoint.IncludeSubtypes,
                        continuationPoint.BrowseDirection,
                        null,
                        null,
                        false);
                }
            }
            // prevent multiple access the browser object.
            lock (browser)
            {
                // apply filters to references.
                Dictionary<NodeId, NodeState> cache = new Dictionary<NodeId, NodeState>();

                for (IReference reference = browser.Next(); reference != null; reference = browser.Next())
                {
                    // validate Browse permission
                    ServiceResult serviceResult = ValidateRolePermissions(context,
                        ExpandedNodeId.ToNodeId(reference.TargetId, ServerData.NamespaceUris),
                        PermissionType.Browse);
                    if (ServiceResult.IsBad(serviceResult))
                    {
                        // ignore reference
                        continue;
                    }
                    // create the type definition reference.
                    ReferenceDescription description = GetReferenceDescription(systemContext, cache, reference, continuationPoint);

                    if (description == null)
                    {
                        continue;
                    }

                    // check if limit reached.
                    if (continuationPoint.MaxResultsToReturn != 0 && references.Count >= continuationPoint.MaxResultsToReturn)
                    {
                        browser.Push(reference);
                        return;
                    }

                    references.Add(description);
                }

                // release the continuation point if all done.
                continuationPoint.Dispose();
                continuationPoint = null;
            }
        }

        #region Browse Support Functions
        /// <summary>
        /// Validates the view description passed to a browse request (throws on error).
        /// </summary>
        protected virtual void ValidateViewDescription(UaServerContext context, ViewDescription view)
        {
            if (ViewDescription.IsDefault(view))
            {
                return;
            }

            ViewState node = (ViewState)FindPredefinedNode(view.ViewId, typeof(ViewState));

            if (node == null)
            {
                throw new ServiceResultException(StatusCodes.BadViewIdUnknown);
            }

            if (view.Timestamp != DateTime.MinValue)
            {
                throw new ServiceResultException(StatusCodes.BadViewTimestampInvalid);
            }

            if (view.ViewVersion != 0)
            {
                throw new ServiceResultException(StatusCodes.BadViewVersionInvalid);
            }
        }

        /// <summary>
        /// Checks if the node is in the view.
        /// </summary>
        protected virtual bool IsNodeInView(UaServerContext context, UaContinuationPoint continuationPoint, NodeState node)
        {
            if (continuationPoint == null || ViewDescription.IsDefault(continuationPoint.View))
            {
                return true;
            }

            return IsNodeInView(context, continuationPoint.View.ViewId, node);
        }

        /// <summary>
        /// Checks if the node is in the view.
        /// </summary>
        protected virtual bool IsNodeInView(UaServerContext context, NodeId viewId, NodeState node)
        {
            ViewState view = (ViewState)FindPredefinedNode(viewId, typeof(ViewState));

            if (view != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the reference is in the view.
        /// </summary>
        protected virtual bool IsReferenceInView(UaServerContext context, UaContinuationPoint continuationPoint, IReference reference)
        {
            return true;
        }

        /// <summary>
        /// Checks if the user is allowed to access this node.
        /// </summary>
        protected virtual bool IsNodeAccessibleForUser(UaServerContext context, UaContinuationPoint continuationPoint, NodeState node)
        {
            return true;
        }

        /// <summary>
        /// Checks if the user is allowed to access this reference.
        /// </summary>
        protected virtual bool IsReferenceAccessibleForUser(UaServerContext context, UaContinuationPoint continuationPoint, IReference reference)
        {
            return true;
        }

        /// <summary>
        /// Returns the references for the node that meets the criteria specified.
        /// </summary>
        protected virtual ReferenceDescription GetReferenceDescription(
            UaServerContext context,
            Dictionary<NodeId, NodeState> cache,
            IReference reference,
            UaContinuationPoint continuationPoint)
        {
            UaServerContext systemContext = m_systemContext.Copy(context);

            // create the type definition reference.
            ReferenceDescription description = new ReferenceDescription();

            description.NodeId = reference.TargetId;
            description.SetReferenceType(continuationPoint.ResultMask, reference.ReferenceTypeId, !reference.IsInverse);

            // check if reference is in the view.
            if (!IsReferenceInView(context, continuationPoint, reference))
            {
                return null;
            }

            // check if the user is allowed to access this reference.
            if (!IsReferenceAccessibleForUser(context, continuationPoint, reference))
            {
                return null;
            }

            // do not cache target parameters for remote nodes.
            if (reference.TargetId.IsAbsolute)
            {
                // only return remote references if no node class filter is specified.
                if (continuationPoint.NodeClassMask != 0)
                {
                    return null;
                }

                return description;
            }

            NodeState target = null;

            // check for local reference.
            NodeStateReference referenceInfo = reference as NodeStateReference;

            if (referenceInfo != null)
            {
                target = referenceInfo.Target;
            }

            // check for internal reference.
            if (target == null)
            {
                UaNodeHandle handle = GetManagerHandle(context, (NodeId)reference.TargetId, null) as UaNodeHandle;

                if (handle != null)
                {
                    target = ValidateNode(context, handle, null);
                }
            }

            // the target may be a reference to a node in another node manager. In these cases
            // the target attributes must be fetched by the caller. The Unfiltered flag tells the
            // caller to do that.
            if (target == null)
            {
                description.Unfiltered = true;
                return description;
            }

            // apply node class filter.
            if (continuationPoint.NodeClassMask != 0 && ((continuationPoint.NodeClassMask & (uint)target.NodeClass) == 0))
            {
                return null;
            }

            // check if target is in the view.
            if (!IsNodeInView(context, continuationPoint, target))
            {
                return null;
            }

            // look up the type definition.
            NodeId typeDefinition = null;

            BaseInstanceState instance = target as BaseInstanceState;

            if (instance != null)
            {
                typeDefinition = instance.TypeDefinitionId;
            }

            // set target attributes.
            description.SetTargetAttributes(
                continuationPoint.ResultMask,
                target.NodeClass,
                target.BrowseName,
                target.DisplayName,
                typeDefinition);

            return description;
        }
        #endregion

        /// <summary>
        /// Returns the target of the specified browse path fragment(s).
        /// </summary>
        /// <remarks>
        /// If reference exists but the node manager does not know the browse name it must
        /// return the NodeId as an unresolvedTargetIds. The caller will try to check the
        /// browse name.
        /// </remarks>
        public virtual void TranslateBrowsePath(
            UaServerOperationContext context,
            object sourceHandle,
            RelativePathElement relativePath,
            IList<ExpandedNodeId> targetIds,
            IList<NodeId> unresolvedTargetIds)
        {
            UaServerContext systemContext = m_systemContext.Copy(context);
            IDictionary<NodeId, NodeState> operationCache = new NodeIdDictionary<NodeState>();

            // check for valid handle.
            UaNodeHandle handle = IsHandleInNamespace(sourceHandle);

            if (handle == null)
            {
                return;
            }

            lock (Lock)
            {
                // validate node.
                NodeState source = ValidateNode(systemContext, handle, operationCache);

                if (source == null)
                {
                    return;
                }

                // get list of references that relative path.
                INodeBrowser browser = source.CreateBrowser(
                    systemContext,
                    null,
                    relativePath.ReferenceTypeId,
                    relativePath.IncludeSubtypes,
                    (relativePath.IsInverse) ? BrowseDirection.Inverse : BrowseDirection.Forward,
                    relativePath.TargetName,
                    null,
                    false);

                // check the browse names.
                try
                {
                    for (IReference reference = browser.Next(); reference != null; reference = browser.Next())
                    {
                        // ignore unknown external references.
                        if (reference.TargetId.IsAbsolute)
                        {
                            continue;
                        }

                        NodeState target = null;

                        // check for local reference.
                        NodeStateReference referenceInfo = reference as NodeStateReference;

                        if (referenceInfo != null)
                        {
                            target = referenceInfo.Target;
                        }

                        if (target == null)
                        {
                            NodeId targetId = (NodeId)reference.TargetId;

                            // the target may be a reference to a node in another node manager.
                            if (!IsNodeIdInNamespace(targetId))
                            {
                                unresolvedTargetIds.Add((NodeId)reference.TargetId);
                                continue;
                            }

                            // look up the target manually.
                            UaNodeHandle targetHandle = GetManagerHandle(systemContext, targetId, operationCache);

                            if (targetHandle == null)
                            {
                                continue;
                            }

                            // validate target.
                            target = ValidateNode(systemContext, targetHandle, operationCache);

                            if (target == null)
                            {
                                continue;
                            }
                        }

                        // check browse name.
                        if (target.BrowseName == relativePath.TargetName)
                        {
                            if (!targetIds.Contains(reference.TargetId))
                            {
                                targetIds.Add(reference.TargetId);
                            }
                        }
                    }
                }
                finally
                {
                    browser.Dispose();
                }
            }
        }

        /// <summary>
        /// Reads the value for the specified attribute.
        /// </summary>
        public virtual void Read(
            UaServerOperationContext context,
            double maxAge,
            IList<ReadValueId> nodesToRead,
            IList<DataValue> values,
            IList<ServiceResult> errors)
        {
            UaServerContext systemContext = m_systemContext.Copy(context);
            IDictionary<NodeId, NodeState> operationCache = new NodeIdDictionary<NodeState>();
            List<UaNodeHandle> nodesToValidate = new List<UaNodeHandle>();

            lock (Lock)
            {
                for (int ii = 0; ii < nodesToRead.Count; ii++)
                {
                    ReadValueId nodeToRead = nodesToRead[ii];

                    // skip items that have already been processed.
                    if (nodeToRead.Processed)
                    {
                        continue;
                    }

                    // check for valid handle.
                    UaNodeHandle handle = GetManagerHandle(systemContext, nodeToRead.NodeId, operationCache);

                    if (handle == null)
                    {
                        continue;
                    }

                    // owned by this node manager.
                    nodeToRead.Processed = true;

                    // create an initial value.
                    DataValue value = values[ii] = new DataValue();

                    value.Value = null;
                    value.ServerTimestamp = DateTime.UtcNow;
                    value.SourceTimestamp = DateTime.MinValue;
                    value.StatusCode = StatusCodes.Good;

                    // check if the node is a area in memory.
                    if (handle.Node == null)
                    {
                        errors[ii] = StatusCodes.BadNodeIdUnknown;

                        // must validate node in a separate operation
                        handle.Index = ii;
                        nodesToValidate.Add(handle);

                        continue;
                    }

                    // read the attribute value.
                    errors[ii] = handle.Node.ReadAttribute(
                        systemContext,
                        nodeToRead.AttributeId,
                        nodeToRead.ParsedIndexRange,
                        nodeToRead.DataEncoding,
                        value);
#if DEBUG
                    if (nodeToRead.AttributeId == Attributes.Value)
                    {
                        UaServerUtils.EventLog.ReadValueRange(nodeToRead.NodeId, value.WrappedValue, nodeToRead.IndexRange);
                    }
#endif
                }

                // check for nothing to do.
                if (nodesToValidate.Count == 0)
                {
                    return;
                }
            }

            // validates the nodes (reads values from the underlying data source if required).
            Read(
                systemContext,
                nodesToRead,
                values,
                errors,
                nodesToValidate,
                operationCache);
        }

        #region Read Support Functions
        /// <summary>
        /// Finds a node in the dynamic cache.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="handle">The node handle.</param>
        /// <param name="cache">The cache to search.</param>
        /// <returns>The node if found. Null otherwise.</returns>
        public virtual NodeState FindNodeInCache(
            UaServerContext context,
            UaNodeHandle handle,
            IDictionary<NodeId, NodeState> cache)
        {
            NodeState target = null;

            // not valid if no root.
            if (handle == null)
            {
                return null;
            }

            // check if previously validated.
            if (handle.Validated)
            {
                return handle.Node;
            }

            // construct id for root node.
            NodeId rootId = handle.RootId;

            if (cache != null)
            {
                // lookup component in local cache for request.
                if (cache.TryGetValue(handle.NodeId, out target))
                {
                    return target;
                }

                // lookup root in local cache for request.
                if (!String.IsNullOrEmpty(handle.ComponentPath))
                {
                    if (cache.TryGetValue(rootId, out target))
                    {
                        target = target.FindChildBySymbolicName(context, handle.ComponentPath);

                        // component exists.
                        if (target != null)
                        {
                            return target;
                        }
                    }
                }
            }

            // lookup component in shared cache.
            target = LookupNodeInComponentCache(context, handle);

            if (target != null)
            {
                return target;
            }

            return null;
        }

        /// <summary>
        /// Marks the handle as validated and saves the node in the dynamic cache.
        /// </summary>
        protected virtual NodeState ValidationComplete(
            UaServerContext context,
            UaNodeHandle handle,
            NodeState node,
            IDictionary<NodeId, NodeState> cache)
        {
            handle.Node = node;
            handle.Validated = true;

            if (cache != null && handle != null)
            {
                cache[handle.NodeId] = node;
            }

            return node;
        }

        /// <summary>
        /// Verifies that the specified node exists.
        /// </summary>
        public virtual NodeState ValidateNode(
            UaServerContext context,
            UaNodeHandle handle,
            IDictionary<NodeId, NodeState> cache)
        {
            // lookup in cache.
            NodeState target = FindNodeInCache(context, handle, cache);

            if (target != null)
            {
                handle.Node = target;
                handle.Validated = true;
                return handle.Node;
            }

            // return default.
            return handle.Node;
        }

        /// <summary>
        /// Validates the nodes and reads the values from the underlying source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="nodesToRead">The nodes to read.</param>
        /// <param name="values">The values.</param>
        /// <param name="errors">The errors.</param>
        /// <param name="nodesToValidate">The nodes to validate.</param>
        /// <param name="cache">The cache.</param>
        protected virtual void Read(
            UaServerContext context,
            IList<ReadValueId> nodesToRead,
            IList<DataValue> values,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToValidate,
            IDictionary<NodeId, NodeState> cache)
        {
            for (int ii = 0; ii < nodesToValidate.Count; ii++)
            {
                UaNodeHandle handle = nodesToValidate[ii];

                lock (Lock)
                {
                    // validate node.
                    NodeState source = ValidateNode(context, handle, cache);

                    if (source == null)
                    {
                        continue;
                    }

                    ReadValueId nodeToRead = nodesToRead[handle.Index];
                    DataValue value = values[handle.Index];

                    // update the attribute value.
                    errors[handle.Index] = source.ReadAttribute(
                        context,
                        nodeToRead.AttributeId,
                        nodeToRead.ParsedIndexRange,
                        nodeToRead.DataEncoding,
                        value);
                }
            }
        }
        #endregion

        /// <summary>
        /// Writes the value for the specified attributes.
        /// </summary>
        public virtual void Write(
            UaServerOperationContext context,
            IList<WriteValue> nodesToWrite,
            IList<ServiceResult> errors)
        {
            UaServerContext systemContext = m_systemContext.Copy(context);
            IDictionary<NodeId, NodeState> operationCache = new NodeIdDictionary<NodeState>();
            List<UaNodeHandle> nodesToValidate = new List<UaNodeHandle>();

            lock (Lock)
            {
                for (var ii = 0; ii < nodesToWrite.Count; ii++)
                {
                    WriteValue nodeToWrite = nodesToWrite[ii];

                    // skip items that have already been processed.
                    if (nodeToWrite.Processed)
                    {
                        continue;
                    }

                    // check for valid handle.
                    UaNodeHandle handle = GetManagerHandle(systemContext, nodeToWrite.NodeId, operationCache);

                    if (handle == null)
                    {
                        continue;
                    }

                    // owned by this node manager.
                    nodeToWrite.Processed = true;

                    // index range is not supported.
                    if (nodeToWrite.AttributeId != Attributes.Value)
                    {
                        if (!String.IsNullOrEmpty(nodeToWrite.IndexRange))
                        {
                            errors[ii] = StatusCodes.BadWriteNotSupported;
                            continue;
                        }
                    }

                    // check if the node is a area in memory.
                    if (handle.Node == null)
                    {
                        errors[ii] = StatusCodes.BadNodeIdUnknown;

                        // must validate node in a separate operation.
                        handle.Index = ii;
                        nodesToValidate.Add(handle);

                        continue;
                    }

                    // check if the node is AnalogItem and the values are outside the InstrumentRange.
                    AnalogItemState analogItemState = handle.Node as AnalogItemState;
                    if (analogItemState != null && analogItemState.InstrumentRange != null)
                    {
                        try
                        {
                            if (nodeToWrite.Value.Value is Array array)
                            {
                                bool isOutOfRange = false;
                                foreach (var arrayValue in array)
                                {
                                    double newValue = Convert.ToDouble(arrayValue, CultureInfo.InvariantCulture);
                                    if (newValue > analogItemState.InstrumentRange.Value.High ||
                                        newValue < analogItemState.InstrumentRange.Value.Low)
                                    {
                                        isOutOfRange = true;
                                        break;
                                    }
                                }
                                if (isOutOfRange)
                                {
                                    errors[ii] = StatusCodes.BadOutOfRange;
                                    continue;
                                }
                            }
                            else
                            {
                                double newValue = Convert.ToDouble(nodeToWrite.Value.Value, CultureInfo.InvariantCulture);

                                if (newValue > analogItemState.InstrumentRange.Value.High ||
                                    newValue < analogItemState.InstrumentRange.Value.Low)
                                {
                                    errors[ii] = StatusCodes.BadOutOfRange;
                                    continue;
                                }
                            }
                        }
                        catch
                        {
                            //skip the InstrumentRange check if the transformation isn't possible.
                        }

                    }

#if DEBUG
                    UaServerUtils.EventLog.WriteValueRange(nodeToWrite.NodeId, nodeToWrite.Value.WrappedValue, nodeToWrite.IndexRange);
#endif
                    PropertyState propertyState = handle.Node as PropertyState;
                    object previousPropertyValue = null;

                    if (propertyState != null)
                    {
                        ExtensionObject extension = propertyState.Value as ExtensionObject;
                        if (extension != null)
                        {
                            previousPropertyValue = extension.Body;
                        }
                        else
                        {
                            previousPropertyValue = propertyState.Value;
                        }
                    }

                    DataValue oldValue = null;

                    if (ServerData?.Auditing == true)
                    {
                        //current server supports auditing 
                        oldValue = new DataValue();
                        // read the old value for the purpose of auditing
                        handle.Node.ReadAttribute(systemContext, nodeToWrite.AttributeId, nodeToWrite.ParsedIndexRange, null, oldValue);
                    }

                    // write the attribute value.
                    errors[ii] = handle.Node.WriteAttribute(
                        systemContext,
                        nodeToWrite.AttributeId,
                        nodeToWrite.ParsedIndexRange,
                        nodeToWrite.Value);

                    // report the write value audit event 
                    ServerData.ReportAuditWriteUpdateEvent(systemContext, nodeToWrite, oldValue?.Value, errors[ii]?.StatusCode ?? StatusCodes.Good);

                    if (!ServiceResult.IsGood(errors[ii]))
                    {
                        continue;
                    }

                    if (propertyState != null)
                    {
                        object propertyValue;
                        ExtensionObject extension = nodeToWrite.Value.Value as ExtensionObject;

                        if (extension != null)
                        {
                            propertyValue = extension.Body;
                        }
                        else
                        {
                            propertyValue = nodeToWrite.Value.Value;
                        }

                        CheckIfSemanticsHaveChanged(systemContext, propertyState, propertyValue, previousPropertyValue);
                    }

                    // updates to source finished - report changes to monitored items.
                    handle.Node.ClearChangeMasks(systemContext, true);
                }

                // check for nothing to do.
                if (nodesToValidate.Count == 0)
                {
                    return;
                }
            }

            // validates the nodes and writes the value to the underlying system.
            Write(
                systemContext,
                nodesToWrite,
                errors,
                nodesToValidate,
                operationCache);
        }

        private void CheckIfSemanticsHaveChanged(UaServerContext systemContext, PropertyState property, object newPropertyValue, object previousPropertyValue)
        {
            // check if the changed property is one that can trigger semantic changes
            string propertyName = property.BrowseName.Name;

            if (propertyName != BrowseNames.EURange &&
                propertyName != BrowseNames.InstrumentRange &&
                propertyName != BrowseNames.EngineeringUnits &&
                propertyName != BrowseNames.Title &&
                propertyName != BrowseNames.AxisDefinition &&
                propertyName != BrowseNames.FalseState &&
                propertyName != BrowseNames.TrueState &&
                propertyName != BrowseNames.EnumStrings &&
                propertyName != BrowseNames.XAxisDefinition &&
                propertyName != BrowseNames.YAxisDefinition &&
                propertyName != BrowseNames.ZAxisDefinition)
            {
                return;
            }

            //look for the Parent and its monitoring items
            foreach (UaMonitoredNode monitoredNode in MonitoredNodes.Values)
            {
                var propertyState = monitoredNode.Node.FindChild(systemContext, property.BrowseName);

                if (propertyState != null && property != null && propertyState.NodeId == property.NodeId && !Utils.IsEqual(newPropertyValue, previousPropertyValue))
                {
                    foreach (var monitoredItem in monitoredNode.DataChangeMonitoredItems)
                    {
                        if (monitoredItem.AttributeId == Attributes.Value)
                        {
                            NodeState node = monitoredNode.Node;

                            if ((node is AnalogItemState && (propertyName == BrowseNames.EURange || propertyName == BrowseNames.EngineeringUnits)) ||
                                (node is TwoStateDiscreteState && (propertyName == BrowseNames.FalseState || propertyName == BrowseNames.TrueState)) ||
                                (node is MultiStateDiscreteState && (propertyName == BrowseNames.EnumStrings)) ||
                                (node is ArrayItemState && (propertyName == BrowseNames.InstrumentRange || propertyName == BrowseNames.EURange || propertyName == BrowseNames.EngineeringUnits || propertyName == BrowseNames.Title)) ||
                                ((node is YArrayItemState || node is XYArrayItemState) && (propertyName == BrowseNames.InstrumentRange || propertyName == BrowseNames.EURange || propertyName == BrowseNames.EngineeringUnits || propertyName == BrowseNames.Title || propertyName == BrowseNames.XAxisDefinition)) ||
                                (node is ImageItemState && (propertyName == BrowseNames.InstrumentRange || propertyName == BrowseNames.EURange || propertyName == BrowseNames.EngineeringUnits || propertyName == BrowseNames.Title || propertyName == BrowseNames.XAxisDefinition || propertyName == BrowseNames.YAxisDefinition)) ||
                                (node is CubeItemState && (propertyName == BrowseNames.InstrumentRange || propertyName == BrowseNames.EURange || propertyName == BrowseNames.EngineeringUnits || propertyName == BrowseNames.Title || propertyName == BrowseNames.XAxisDefinition || propertyName == BrowseNames.YAxisDefinition || propertyName == BrowseNames.ZAxisDefinition)) ||
                                (node is NDimensionArrayItemState && (propertyName == BrowseNames.InstrumentRange || propertyName == BrowseNames.EURange || propertyName == BrowseNames.EngineeringUnits || propertyName == BrowseNames.Title || propertyName == BrowseNames.AxisDefinition)))
                            {
                                monitoredItem.SetSemanticsChanged();

                                DataValue value = new DataValue();
                                value.ServerTimestamp = DateTime.UtcNow;

                                monitoredNode.Node.ReadAttribute(systemContext, Attributes.Value, monitoredItem.IndexRange, null, value);

                                monitoredItem.QueueValue(value, ServiceResult.Good, true);
                            }
                        }
                    }
                }
            }
        }

        #region Write Support Functions
        /// <summary>
        /// Validates the nodes and writes the value to the underlying system.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="nodesToWrite">The nodes to write.</param>
        /// <param name="errors">The errors.</param>
        /// <param name="nodesToValidate">The nodes to validate.</param>
        /// <param name="cache">The cache.</param>
        protected virtual void Write(
            UaServerContext context,
            IList<WriteValue> nodesToWrite,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToValidate,
            IDictionary<NodeId, NodeState> cache)
        {
            // validates the nodes (reads values from the underlying data source if required).
            for (int ii = 0; ii < nodesToValidate.Count; ii++)
            {
                UaNodeHandle handle = nodesToValidate[ii];

                lock (Lock)
                {
                    // validate node.
                    NodeState source = ValidateNode(context, handle, cache);

                    if (source == null)
                    {
                        continue;
                    }

                    WriteValue nodeToWrite = nodesToWrite[handle.Index];

                    // write the attribute value.
                    errors[handle.Index] = source.WriteAttribute(
                        context,
                        nodeToWrite.AttributeId,
                        nodeToWrite.ParsedIndexRange,
                        nodeToWrite.Value);

                    // updates to source finished - report changes to monitored items.
                    source.ClearChangeMasks(context, false);
                }
            }
        }
        #endregion

        /// <summary>
        /// Reads the history for the specified nodes.
        /// </summary>
        public virtual void HistoryRead(
            UaServerOperationContext context,
            HistoryReadDetails details,
            TimestampsToReturn timestampsToReturn,
            bool releaseContinuationPoints,
            IList<HistoryReadValueId> nodesToRead,
            IList<HistoryReadResult> results,
            IList<ServiceResult> errors)
        {
            UaServerContext systemContext = m_systemContext.Copy(context);
            IDictionary<NodeId, NodeState> operationCache = new NodeIdDictionary<NodeState>();
            var nodesToProcess = new List<UaNodeHandle>();

            lock (Lock)
            {
                for (int ii = 0; ii < nodesToRead.Count; ii++)
                {
                    HistoryReadValueId nodeToRead = nodesToRead[ii];

                    // skip items that have already been processed.
                    if (nodeToRead.Processed)
                    {
                        continue;
                    }

                    // check for valid handle.
                    UaNodeHandle handle = GetManagerHandle(systemContext, nodeToRead.NodeId, operationCache);

                    if (handle == null)
                    {
                        continue;
                    }

                    // owned by this node manager.
                    nodeToRead.Processed = true;

                    // create an initial result.
                    HistoryReadResult result = results[ii] = new HistoryReadResult();

                    result.HistoryData = null;
                    result.ContinuationPoint = null;
                    result.StatusCode = StatusCodes.Good;

                    // check if the node is a area in memory.
                    if (handle.Node == null)
                    {
                        errors[ii] = StatusCodes.BadNodeIdUnknown;

                        // must validate node in a separate operation
                        handle.Index = ii;
                        nodesToProcess.Add(handle);

                        continue;
                    }

                    errors[ii] = StatusCodes.BadHistoryOperationUnsupported;

                    // check for data history variable.
                    BaseVariableState variable = handle.Node as BaseVariableState;

                    if (variable != null)
                    {
                        if ((variable.AccessLevel & AccessLevels.HistoryRead) != 0)
                        {
                            handle.Index = ii;
                            nodesToProcess.Add(handle);
                            continue;
                        }
                    }

                    // check for event history object.
                    BaseObjectState notifier = handle.Node as BaseObjectState;

                    if (notifier != null)
                    {
                        if ((notifier.EventNotifier & EventNotifiers.HistoryRead) != 0)
                        {
                            handle.Index = ii;
                            nodesToProcess.Add(handle);
                            continue;
                        }
                    }
                }

                // check for nothing to do.
                if (nodesToProcess.Count == 0)
                {
                    return;
                }
            }

            // validates the nodes (reads values from the underlying data source if required).
            HistoryRead(
                systemContext,
                details,
                timestampsToReturn,
                releaseContinuationPoints,
                nodesToRead,
                results,
                errors,
                nodesToProcess,
                operationCache);
        }

        #region HistoryRead Support Functions
        /// <summary>
        /// Releases the continuation points.
        /// </summary>
        protected virtual void HistoryReleaseContinuationPoints(
            UaServerContext context,
            IList<HistoryReadValueId> nodesToRead,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToProcess,
            IDictionary<NodeId, NodeState> cache)
        {
            for (int ii = 0; ii < nodesToProcess.Count; ii++)
            {
                UaNodeHandle handle = nodesToProcess[ii];

                // validate node.
                NodeState source = ValidateNode(context, handle, cache);

                if (source == null)
                {
                    continue;
                }

                errors[handle.Index] = StatusCodes.BadContinuationPointInvalid;
            }
        }

        /// <summary>
        /// Reads raw history data.
        /// </summary>
        protected virtual void HistoryReadRawModified(
            UaServerContext context,
            ReadRawModifiedDetails details,
            TimestampsToReturn timestampsToReturn,
            IList<HistoryReadValueId> nodesToRead,
            IList<HistoryReadResult> results,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToProcess,
            IDictionary<NodeId, NodeState> cache)
        {
            for (int ii = 0; ii < nodesToProcess.Count; ii++)
            {
                UaNodeHandle handle = nodesToProcess[ii];

                // validate node.
                NodeState source = ValidateNode(context, handle, cache);

                if (source == null)
                {
                    continue;
                }

                errors[handle.Index] = StatusCodes.BadHistoryOperationUnsupported;
            }
        }

        /// <summary>
        /// Reads processed history data.
        /// </summary>
        protected virtual void HistoryReadProcessed(
            UaServerContext context,
            ReadProcessedDetails details,
            TimestampsToReturn timestampsToReturn,
            IList<HistoryReadValueId> nodesToRead,
            IList<HistoryReadResult> results,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToProcess,
            IDictionary<NodeId, NodeState> cache)
        {
            for (int ii = 0; ii < nodesToProcess.Count; ii++)
            {
                UaNodeHandle handle = nodesToProcess[ii];

                // validate node.
                NodeState source = ValidateNode(context, handle, cache);

                if (source == null)
                {
                    continue;
                }

                errors[handle.Index] = StatusCodes.BadHistoryOperationUnsupported;
            }
        }

        /// <summary>
        /// Reads history data at specified times.
        /// </summary>
        protected virtual void HistoryReadAtTime(
            UaServerContext context,
            ReadAtTimeDetails details,
            TimestampsToReturn timestampsToReturn,
            IList<HistoryReadValueId> nodesToRead,
            IList<HistoryReadResult> results,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToProcess,
            IDictionary<NodeId, NodeState> cache)
        {
            for (int ii = 0; ii < nodesToProcess.Count; ii++)
            {
                UaNodeHandle handle = nodesToProcess[ii];

                // validate node.
                NodeState source = ValidateNode(context, handle, cache);

                if (source == null)
                {
                    continue;
                }

                errors[handle.Index] = StatusCodes.BadHistoryOperationUnsupported;
            }
        }

        /// <summary>
        /// Reads history events.
        /// </summary>
        protected virtual void HistoryReadEvents(
            UaServerContext context,
            ReadEventDetails details,
            TimestampsToReturn timestampsToReturn,
            IList<HistoryReadValueId> nodesToRead,
            IList<HistoryReadResult> results,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToProcess,
            IDictionary<NodeId, NodeState> cache)
        {
            for (int ii = 0; ii < nodesToProcess.Count; ii++)
            {
                UaNodeHandle handle = nodesToProcess[ii];

                // validate node.
                NodeState source = ValidateNode(context, handle, cache);

                if (source == null)
                {
                    continue;
                }

                errors[handle.Index] = StatusCodes.BadHistoryOperationUnsupported;
            }
        }

        /// <summary>
        /// Validates the nodes and reads the values from the underlying source.
        /// </summary>
        protected virtual void HistoryRead(
            UaServerContext context,
            HistoryReadDetails details,
            TimestampsToReturn timestampsToReturn,
            bool releaseContinuationPoints,
            IList<HistoryReadValueId> nodesToRead,
            IList<HistoryReadResult> results,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToProcess,
            IDictionary<NodeId, NodeState> cache)
        {
            // check if continuation points are being released.
            if (releaseContinuationPoints)
            {
                HistoryReleaseContinuationPoints(
                    context,
                    nodesToRead,
                    errors,
                    nodesToProcess,
                    cache);

                return;
            }

            // check timestamps to return.
            if (timestampsToReturn < TimestampsToReturn.Source || timestampsToReturn > TimestampsToReturn.Neither)
            {
                throw new ServiceResultException(StatusCodes.BadTimestampsToReturnInvalid);
            }

            // handle raw data request.
            ReadRawModifiedDetails readRawModifiedDetails = details as ReadRawModifiedDetails;

            if (readRawModifiedDetails != null)
            {
                // at least one must be provided.
                if (readRawModifiedDetails.StartTime == DateTime.MinValue && readRawModifiedDetails.EndTime == DateTime.MinValue)
                {
                    throw new ServiceResultException(StatusCodes.BadInvalidTimestampArgument);
                }

                // if one is null the num values must be provided.
                if (readRawModifiedDetails.StartTime == DateTime.MinValue || readRawModifiedDetails.EndTime == DateTime.MinValue)
                {
                    if (readRawModifiedDetails.NumValuesPerNode == 0)
                    {
                        throw new ServiceResultException(StatusCodes.BadInvalidTimestampArgument);
                    }
                }

                HistoryReadRawModified(
                    context,
                    readRawModifiedDetails,
                    timestampsToReturn,
                    nodesToRead,
                    results,
                    errors,
                    nodesToProcess,
                    cache);

                return;
            }

            // handle processed data request.
            ReadProcessedDetails readProcessedDetails = details as ReadProcessedDetails;

            if (readProcessedDetails != null)
            {
                // check the list of aggregates.
                if (readProcessedDetails.AggregateType == null || readProcessedDetails.AggregateType.Count != nodesToRead.Count)
                {
                    throw new ServiceResultException(StatusCodes.BadAggregateListMismatch);
                }

                // check start/end time.
                if (readProcessedDetails.StartTime == DateTime.MinValue || readProcessedDetails.EndTime == DateTime.MinValue)
                {
                    throw new ServiceResultException(StatusCodes.BadInvalidTimestampArgument);
                }

                HistoryReadProcessed(
                    context,
                    readProcessedDetails,
                    timestampsToReturn,
                    nodesToRead,
                    results,
                    errors,
                    nodesToProcess,
                    cache);

                return;
            }

            // handle raw data at time request.
            ReadAtTimeDetails readAtTimeDetails = details as ReadAtTimeDetails;

            if (readAtTimeDetails != null)
            {
                HistoryReadAtTime(
                    context,
                    readAtTimeDetails,
                    timestampsToReturn,
                    nodesToRead,
                    results,
                    errors,
                    nodesToProcess,
                    cache);

                return;
            }

            // handle read events request.
            ReadEventDetails readEventDetails = details as ReadEventDetails;

            if (readEventDetails != null)
            {
                // check start/end time and max values.
                if (readEventDetails.NumValuesPerNode == 0)
                {
                    if (readEventDetails.StartTime == DateTime.MinValue || readEventDetails.EndTime == DateTime.MinValue)
                    {
                        throw new ServiceResultException(StatusCodes.BadInvalidTimestampArgument);
                    }
                }
                else
                {
                    if (readEventDetails.StartTime == DateTime.MinValue && readEventDetails.EndTime == DateTime.MinValue)
                    {
                        throw new ServiceResultException(StatusCodes.BadInvalidTimestampArgument);
                    }
                }

                // validate the event filter.
                EventFilter.Result result = readEventDetails.Filter.Validate(new FilterContext(m_server.NamespaceUris, m_server.TypeTree, context));

                if (ServiceResult.IsBad(result.Status))
                {
                    throw new ServiceResultException(result.Status);
                }

                // read the event history.
                HistoryReadEvents(
                    context,
                    readEventDetails,
                    timestampsToReturn,
                    nodesToRead,
                    results,
                    errors,
                    nodesToProcess,
                    cache);

                return;
            }
        }
        #endregion

        /// <summary>
        /// Updates the history for the specified nodes.
        /// </summary>
        public virtual void HistoryUpdate(
            UaServerOperationContext context,
            Type detailsType,
            IList<HistoryUpdateDetails> nodesToUpdate,
            IList<HistoryUpdateResult> results,
            IList<ServiceResult> errors)
        {
            UaServerContext systemContext = SystemContext.Copy(context);
            IDictionary<NodeId, NodeState> operationCache = new NodeIdDictionary<NodeState>();
            var nodesToProcess = new List<UaNodeHandle>();

            lock (Lock)
            {
                for (int ii = 0; ii < nodesToUpdate.Count; ii++)
                {
                    HistoryUpdateDetails nodeToUpdate = nodesToUpdate[ii];

                    // skip items that have already been processed.
                    if (nodeToUpdate.Processed)
                    {
                        continue;
                    }

                    // check for valid handle.
                    UaNodeHandle handle = GetManagerHandle(systemContext, nodeToUpdate.NodeId, operationCache);

                    if (handle == null)
                    {
                        continue;
                    }

                    // owned by this node manager.
                    nodeToUpdate.Processed = true;

                    // create an initial result.
                    HistoryUpdateResult result = results[ii] = new HistoryUpdateResult();
                    result.StatusCode = StatusCodes.Good;

                    // check if the node is a area in memory.
                    if (handle.Node == null)
                    {
                        errors[ii] = StatusCodes.BadNodeIdUnknown;

                        // must validate node in a separate operation
                        handle.Index = ii;
                        nodesToProcess.Add(handle);
                        continue;
                    }

                    errors[ii] = StatusCodes.BadHistoryOperationUnsupported;

                    // check for data history variable.
                    BaseVariableState variable = handle.Node as BaseVariableState;

                    if (variable != null)
                    {
                        if ((variable.AccessLevel & AccessLevels.HistoryWrite) != 0)
                        {
                            handle.Index = ii;
                            nodesToProcess.Add(handle);
                            continue;
                        }
                    }

                    // check for event history object.
                    BaseObjectState notifier = handle.Node as BaseObjectState;

                    if (notifier != null)
                    {
                        if ((notifier.EventNotifier & EventNotifiers.HistoryWrite) != 0)
                        {
                            handle.Index = ii;
                            nodesToProcess.Add(handle);
                            continue;
                        }
                    }
                }

                // check for nothing to do.
                if (nodesToProcess.Count == 0)
                {
                    return;
                }
            }

            // validates the nodes and updates.
            HistoryUpdate(
                systemContext,
                detailsType,
                nodesToUpdate,
                results,
                errors,
                nodesToProcess,
                operationCache);
        }

        #region HistoryUpdate Support Functions
        /// <summary>
        /// Validates the nodes and updates the history.
        /// </summary>
        protected virtual void HistoryUpdate(
            UaServerContext context,
            Type detailsType,
            IList<HistoryUpdateDetails> nodesToUpdate,
            IList<HistoryUpdateResult> results,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToProcess,
            IDictionary<NodeId, NodeState> cache)
        {
            // handle update data request.
            if (detailsType == typeof(UpdateDataDetails))
            {
                UpdateDataDetails[] details = new UpdateDataDetails[nodesToUpdate.Count];

                for (int ii = 0; ii < details.Length; ii++)
                {
                    details[ii] = (UpdateDataDetails)nodesToUpdate[ii];
                }

                HistoryUpdateData(
                    context,
                    details,
                    results,
                    errors,
                    nodesToProcess,
                    cache);

                return;
            }

            // handle update structure data request.
            if (detailsType == typeof(UpdateStructureDataDetails))
            {
                UpdateStructureDataDetails[] details = new UpdateStructureDataDetails[nodesToUpdate.Count];

                for (int ii = 0; ii < details.Length; ii++)
                {
                    details[ii] = (UpdateStructureDataDetails)nodesToUpdate[ii];
                }

                HistoryUpdateStructureData(
                    context,
                    details,
                    results,
                    errors,
                    nodesToProcess,
                    cache);

                return;
            }

            // handle update events request.
            if (detailsType == typeof(UpdateEventDetails))
            {
                UpdateEventDetails[] details = new UpdateEventDetails[nodesToUpdate.Count];

                for (int ii = 0; ii < details.Length; ii++)
                {
                    details[ii] = (UpdateEventDetails)nodesToUpdate[ii];
                }

                HistoryUpdateEvents(
                    context,
                    details,
                    results,
                    errors,
                    nodesToProcess,
                    cache);

                return;
            }

            // handle delete raw data request.
            if (detailsType == typeof(DeleteRawModifiedDetails))
            {
                DeleteRawModifiedDetails[] details = new DeleteRawModifiedDetails[nodesToUpdate.Count];

                for (int ii = 0; ii < details.Length; ii++)
                {
                    details[ii] = (DeleteRawModifiedDetails)nodesToUpdate[ii];
                }

                HistoryDeleteRawModified(
                    context,
                    details,
                    results,
                    errors,
                    nodesToProcess,
                    cache);

                return;
            }

            // handle delete at time request.
            if (detailsType == typeof(DeleteAtTimeDetails))
            {
                DeleteAtTimeDetails[] details = new DeleteAtTimeDetails[nodesToUpdate.Count];

                for (int ii = 0; ii < details.Length; ii++)
                {
                    details[ii] = (DeleteAtTimeDetails)nodesToUpdate[ii];
                }

                HistoryDeleteAtTime(
                    context,
                    details,
                    results,
                    errors,
                    nodesToProcess,
                    cache);

                return;
            }

            // handle delete at time request.
            if (detailsType == typeof(DeleteEventDetails))
            {
                DeleteEventDetails[] details = new DeleteEventDetails[nodesToUpdate.Count];

                for (int ii = 0; ii < details.Length; ii++)
                {
                    details[ii] = (DeleteEventDetails)nodesToUpdate[ii];
                }

                HistoryDeleteEvents(
                    context,
                    details,
                    results,
                    errors,
                    nodesToProcess,
                    cache);

                return;
            }
        }

        /// <summary>
        /// Updates the data history for one or more nodes.
        /// </summary>
        protected virtual void HistoryUpdateData(
            UaServerContext context,
            IList<UpdateDataDetails> nodesToUpdate,
            IList<HistoryUpdateResult> results,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToProcess,
            IDictionary<NodeId, NodeState> cache)
        {
            for (int ii = 0; ii < nodesToProcess.Count; ii++)
            {
                UaNodeHandle handle = nodesToProcess[ii];

                // validate node.
                NodeState source = ValidateNode(context, handle, cache);

                if (source == null)
                {
                    continue;
                }

                errors[handle.Index] = StatusCodes.BadHistoryOperationUnsupported;
            }
        }

        /// <summary>
        /// Updates the structured data history for one or more nodes.
        /// </summary>
        protected virtual void HistoryUpdateStructureData(
            UaServerContext context,
            IList<UpdateStructureDataDetails> nodesToUpdate,
            IList<HistoryUpdateResult> results,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToProcess,
            IDictionary<NodeId, NodeState> cache)
        {
            for (int ii = 0; ii < nodesToProcess.Count; ii++)
            {
                UaNodeHandle handle = nodesToProcess[ii];

                // validate node.
                NodeState source = ValidateNode(context, handle, cache);

                if (source == null)
                {
                    continue;
                }

                errors[handle.Index] = StatusCodes.BadHistoryOperationUnsupported;
            }
        }

        /// <summary>
        /// Updates the event history for one or more nodes.
        /// </summary>
        protected virtual void HistoryUpdateEvents(
            UaServerContext context,
            IList<UpdateEventDetails> nodesToUpdate,
            IList<HistoryUpdateResult> results,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToProcess,
            IDictionary<NodeId, NodeState> cache)
        {
            for (int ii = 0; ii < nodesToProcess.Count; ii++)
            {
                UaNodeHandle handle = nodesToProcess[ii];

                // validate node.
                NodeState source = ValidateNode(context, handle, cache);

                if (source == null)
                {
                    continue;
                }

                errors[handle.Index] = StatusCodes.BadHistoryOperationUnsupported;
            }
        }

        /// <summary>
        /// Deletes the data history for one or more nodes.
        /// </summary>
        protected virtual void HistoryDeleteRawModified(
            UaServerContext context,
            IList<DeleteRawModifiedDetails> nodesToUpdate,
            IList<HistoryUpdateResult> results,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToProcess,
            IDictionary<NodeId, NodeState> cache)
        {
            for (int ii = 0; ii < nodesToProcess.Count; ii++)
            {
                UaNodeHandle handle = nodesToProcess[ii];

                // validate node.
                NodeState source = ValidateNode(context, handle, cache);

                if (source == null)
                {
                    continue;
                }

                errors[handle.Index] = StatusCodes.BadHistoryOperationUnsupported;
            }
        }

        /// <summary>
        /// Deletes the data history for one or more nodes.
        /// </summary>
        protected virtual void HistoryDeleteAtTime(
            UaServerContext context,
            IList<DeleteAtTimeDetails> nodesToUpdate,
            IList<HistoryUpdateResult> results,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToProcess,
            IDictionary<NodeId, NodeState> cache)
        {
            for (int ii = 0; ii < nodesToProcess.Count; ii++)
            {
                UaNodeHandle handle = nodesToProcess[ii];

                // validate node.
                NodeState source = ValidateNode(context, handle, cache);

                if (source == null)
                {
                    continue;
                }

                errors[handle.Index] = StatusCodes.BadHistoryOperationUnsupported;
            }
        }

        /// <summary>
        /// Deletes the event history for one or more nodes.
        /// </summary>
        protected virtual void HistoryDeleteEvents(
            UaServerContext context,
            IList<DeleteEventDetails> nodesToUpdate,
            IList<HistoryUpdateResult> results,
            IList<ServiceResult> errors,
            List<UaNodeHandle> nodesToProcess,
            IDictionary<NodeId, NodeState> cache)
        {
            for (int ii = 0; ii < nodesToProcess.Count; ii++)
            {
                UaNodeHandle handle = nodesToProcess[ii];

                // validate node.
                NodeState source = ValidateNode(context, handle, cache);

                if (source == null)
                {
                    continue;
                }

                errors[handle.Index] = StatusCodes.BadHistoryOperationUnsupported;
            }
        }
        #endregion

        /// <summary>
        /// Calls a method on the specified nodes.
        /// </summary>
        public virtual void Call(
            UaServerOperationContext context,
            IList<CallMethodRequest> methodsToCall,
            IList<CallMethodResult> results,
            IList<ServiceResult> errors)
        {
            UaServerContext systemContext = SystemContext.Copy(context);
            IDictionary<NodeId, NodeState> operationCache = new NodeIdDictionary<NodeState>();

            for (int ii = 0; ii < methodsToCall.Count; ii++)
            {
                CallMethodRequest methodToCall = methodsToCall[ii];

                // skip items that have already been processed.
                if (methodToCall.Processed)
                {
                    continue;
                }

                MethodState method = null;

                // check for valid handle.
                UaNodeHandle handle = GetManagerHandle(systemContext, methodToCall.ObjectId, operationCache);

                if (handle == null)
                {
                    continue;
                }

                lock (Lock)
                {
                    // owned by this node manager.
                    methodToCall.Processed = true;

                    // validate the source node.
                    NodeState source = ValidateNode(systemContext, handle, operationCache);

                    if (source == null)
                    {
                        errors[ii] = StatusCodes.BadNodeIdUnknown;
                        continue;
                    }

                    // find the method.
                    method = source.FindMethod(systemContext, methodToCall.MethodId);

                    if (method == null)
                    {
                        // check for loose coupling.
                        if (source.ReferenceExists(ReferenceTypeIds.HasComponent, false, methodToCall.MethodId))
                        {
                            method = (MethodState)FindPredefinedNode(methodToCall.MethodId, typeof(MethodState));
                        }

                        if (method == null)
                        {
                            errors[ii] = StatusCodes.BadMethodInvalid;
                            continue;
                        }
                    }

                    // validate the role permissions for method to be executed,
                    // it may be a different MethodState that does not have the MethodId specified in the method call
                    errors[ii] = ValidateRolePermissions(context,
                        method.NodeId,
                        PermissionType.Call);

                    if (ServiceResult.IsBad(errors[ii]))
                    {
                        continue;
                    }
                }

                // call the method.
                CallMethodResult result = results[ii] = new CallMethodResult();

                errors[ii] = Call(
                    systemContext,
                    methodToCall,
                    method,
                    result);
            }
        }

        /// <summary>
        /// Calls a method on an object.
        /// </summary>
        protected virtual ServiceResult Call(
            ISystemContext context,
            CallMethodRequest methodToCall,
            MethodState method,
            CallMethodResult result)
        {
            UaServerContext systemContext = context as UaServerContext;
            List<ServiceResult> argumentErrors = new List<ServiceResult>();
            VariantCollection outputArguments = new VariantCollection();

            ServiceResult callResult = method.Call(
                context,
                methodToCall.ObjectId,
                methodToCall.InputArguments,
                argumentErrors,
                outputArguments);

            if (ServiceResult.IsBad(callResult))
            {
                return callResult;
            }

            // check for argument errors.
            bool argumentsValid = true;

            for (int jj = 0; jj < argumentErrors.Count; jj++)
            {
                ServiceResult argumentError = argumentErrors[jj];

                if (argumentError != null)
                {
                    result.InputArgumentResults.Add(argumentError.StatusCode);

                    if (ServiceResult.IsBad(argumentError))
                    {
                        argumentsValid = false;
                    }
                }
                else
                {
                    result.InputArgumentResults.Add(StatusCodes.Good);
                }

                // only fill in diagnostic info if it is requested.
                if (systemContext.OperationContext != null)
                {
                    if ((systemContext.OperationContext.DiagnosticsMask & DiagnosticsMasks.OperationAll) != 0)
                    {
                        if (ServiceResult.IsBad(argumentError))
                        {
                            argumentsValid = false;
                            result.InputArgumentDiagnosticInfos.Add(new DiagnosticInfo(argumentError, systemContext.OperationContext.DiagnosticsMask, false, systemContext.OperationContext.StringTable));
                        }
                        else
                        {
                            result.InputArgumentDiagnosticInfos.Add(null);
                        }
                    }
                }
            }

            // check for validation errors.
            if (!argumentsValid)
            {
                result.StatusCode = StatusCodes.BadInvalidArgument;
                return result.StatusCode;
            }

            // do not return diagnostics if there are no errors.
            result.InputArgumentDiagnosticInfos.Clear();

            // return output arguments.
            result.OutputArguments = outputArguments;

            // return the actual result of the original call
            return callResult;
        }


        /// <summary>
        /// Subscribes or unsubscribes to events produced by the specified source.
        /// </summary>
        /// <remarks>
        /// This method is called when a event subscription is created or deletes. The node manager
        /// must  start/stop reporting events for the specified object and all objects below it in
        /// the notifier hierarchy.
        /// </remarks>
        public virtual ServiceResult SubscribeToEvents(
            UaServerOperationContext context,
            object sourceId,
            uint subscriptionId,
            IUaEventMonitoredItem monitoredItem,
            bool unsubscribe)
        {
            UaServerContext systemContext = SystemContext.Copy(context);

            // check for valid handle.
            UaNodeHandle handle = IsHandleInNamespace(sourceId);

            if (handle == null)
            {
                return StatusCodes.BadNodeIdInvalid;
            }

            lock (Lock)
            {
                // check for valid node.
                NodeState source = ValidateNode(systemContext, handle, null);

                if (source == null)
                {
                    return StatusCodes.BadNodeIdUnknown;
                }

                // subscribe to events.
                return SubscribeToEvents(systemContext, source, monitoredItem, unsubscribe);
            }
        }

        /// <summary>
        /// Subscribes or unsubscribes to events produced by all event sources.
        /// </summary>
        /// <remarks>
        /// This method is called when a event subscription is created or deleted. The node
        /// manager must start/stop reporting events for all objects that it manages.
        /// </remarks>
        public virtual ServiceResult SubscribeToAllEvents(
            UaServerOperationContext context,
            uint subscriptionId,
            IUaEventMonitoredItem monitoredItem,
            bool unsubscribe)
        {
            UaServerContext systemContext = SystemContext.Copy(context);

            lock (Lock)
            {
                // A client has subscribed to the Server object which means all events produced
                // by this manager must be reported. This is done by incrementing the monitoring
                // reference count for all root notifiers.
                if (m_rootNotifiers != null)
                {
                    for (int ii = 0; ii < m_rootNotifiers.Count; ii++)
                    {
                        SubscribeToEvents(systemContext, m_rootNotifiers[ii], monitoredItem, unsubscribe);
                    }
                }

                return ServiceResult.Good;
            }
        }

        #region SubscribeToEvents Support Functions
        /// <summary>
        /// Adds a root notifier.
        /// </summary>
        /// <param name="notifier">The notifier.</param>
        /// <remarks>
        /// A root notifier is a notifier owned by the NodeManager that is not the target of a
        /// HasNotifier reference. These nodes need to be linked directly to the Server object.
        /// </remarks>
        public virtual void AddRootNotifier(NodeState notifier)
        {
            lock (Lock)
            {
                if (m_rootNotifiers == null)
                {
                    m_rootNotifiers = new List<NodeState>();
                }

                bool mustAdd = true;

                for (int ii = 0; ii < m_rootNotifiers.Count; ii++)
                {
                    if (Object.ReferenceEquals(notifier, m_rootNotifiers[ii]))
                    {
                        return;
                    }

                    if (m_rootNotifiers[ii].NodeId == notifier.NodeId)
                    {
                        m_rootNotifiers[ii] = notifier;
                        mustAdd = false;
                        break;
                    }
                }

                if (mustAdd)
                {
                    m_rootNotifiers.Add(notifier);
                }

                // need to prevent recursion with the server object.
                if (notifier.NodeId != ObjectIds.Server)
                {
                    notifier.OnReportEvent = OnReportEvent;

                    if (!notifier.ReferenceExists(ReferenceTypeIds.HasNotifier, true, ObjectIds.Server))
                    {
                        notifier.AddReference(ReferenceTypeIds.HasNotifier, true, ObjectIds.Server);
                    }
                }

                // subscribe to existing events.
                if (m_server.EventManager != null)
                {
                    IList<IUaEventMonitoredItem> monitoredItems = m_server.EventManager.GetMonitoredItems();

                    for (int ii = 0; ii < monitoredItems.Count; ii++)
                    {
                        if (monitoredItems[ii].MonitoringAllEvents)
                        {
                            SubscribeToEvents(
                            SystemContext,
                            notifier,
                            monitoredItems[ii],
                            true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes a root notifier previously added with AddRootNotifier.
        /// </summary>
        /// <param name="notifier">The notifier.</param>
        public virtual void RemoveRootNotifier(NodeState notifier)
        {
            lock (Lock)
            {
                if (m_rootNotifiers != null)
                {
                    for (int ii = 0; ii < m_rootNotifiers.Count; ii++)
                    {
                        if (Object.ReferenceEquals(notifier, m_rootNotifiers[ii]))
                        {
                            notifier.OnReportEvent = null;
                            notifier.RemoveReference(ReferenceTypeIds.HasNotifier, true, ObjectIds.Server);
                            m_rootNotifiers.RemoveAt(ii);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reports an event for a root notifier.
        /// </summary>
        protected virtual void OnReportEvent(
            ISystemContext context,
            NodeState node,
            IFilterTarget e)
        {
            ServerData.ReportEvent(context, e);
        }

        /// <summary>
        /// Subscribes to events.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="source">The source.</param>
        /// <param name="monitoredItem">The monitored item.</param>
        /// <param name="unsubscribe">if set to <c>true</c> [unsubscribe].</param>
        /// <returns>Any error code.</returns>
        protected virtual ServiceResult SubscribeToEvents(
            UaServerContext context,
            NodeState source,
            IUaEventMonitoredItem monitoredItem,
            bool unsubscribe)
        {
            UaMonitoredNode monitoredNode;

            // handle unsubscribe.
            if (unsubscribe)
            {
                // check for existing monitored node.
                if (!MonitoredNodes.TryGetValue(source.NodeId, out monitoredNode))
                {
                    return StatusCodes.BadNodeIdUnknown;
                }

                monitoredNode.Remove(monitoredItem);

                // check if node is no longer being monitored.
                if (!monitoredNode.HasMonitoredItems)
                {
                    MonitoredNodes.Remove(source.NodeId);
                }

                // update flag.
                source.SetAreEventsMonitored(context, !unsubscribe, true);

                // call subclass.
                OnSubscribeToEvents(context, monitoredNode, unsubscribe);

                // all done.
                return ServiceResult.Good;
            }

            // only objects or views can be subscribed to.
            if (!(source is BaseObjectState instance) || (instance.EventNotifier & EventNotifiers.SubscribeToEvents) == 0)
            {
                if (!(source is ViewState view) || (view.EventNotifier & EventNotifiers.SubscribeToEvents) == 0)
                {
                    return StatusCodes.BadNotSupported;
                }
            }

            // check for existing monitored node.
            if (!MonitoredNodes.TryGetValue(source.NodeId, out monitoredNode))
            {
                MonitoredNodes[source.NodeId] = monitoredNode = new UaMonitoredNode(this, source);
            }

            if (monitoredNode.EventMonitoredItems != null)
            {
                // remove existing monitored items with the same Id prior to insertion in order to avoid duplicates
                // this is necessary since the SubscribeToEvents method is called also from ModifyMonitoredItemsForEvents
                monitoredNode.EventMonitoredItems.RemoveAll(e => e.Id == monitoredItem.Id);
            }

            // this links the node to specified monitored item and ensures all events
            // reported by the node are added to the monitored item's queue.
            monitoredNode.Add(monitoredItem);

            // This call recursively updates a reference count all nodes in the notifier
            // hierarchy below the area. Sources with a reference count of 0 do not have
            // any active subscriptions so they do not need to report events.
            source.SetAreEventsMonitored(context, !unsubscribe, true);

            // signal update.
            OnSubscribeToEvents(context, monitoredNode, unsubscribe);

            // all done.
            return ServiceResult.Good;
        }

        /// <summary>
        /// Called after subscribing/unsubscribing to events.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="monitoredNode">The monitored node.</param>
        /// <param name="unsubscribe">if set to <c>true</c> unsubscribing.</param>
        protected virtual void OnSubscribeToEvents(
            UaServerContext context,
            UaMonitoredNode monitoredNode,
            bool unsubscribe)
        {
            // defined by the sub-class
        }
        #endregion

        /// <summary>
        /// Tells the node manager to refresh any conditions associated with the specified monitored items.
        /// </summary>
        /// <remarks>
        /// This method is called when the condition refresh method is called for a subscription.
        /// The node manager must create a refresh event for each condition monitored by the subscription.
        /// </remarks>
        public virtual ServiceResult ConditionRefresh(
            UaServerOperationContext context,
            IList<IUaEventMonitoredItem> monitoredItems)
        {
            UaServerContext systemContext = SystemContext.Copy(context);

            for (int ii = 0; ii < monitoredItems.Count; ii++)
            {
                // the IUaEventMonitoredItem should always be MonitoredItems since they are created by the MasterNodeManager.
                UaMonitoredItem monitoredItem = monitoredItems[ii] as UaMonitoredItem;

                if (monitoredItem == null)
                {
                    continue;
                }

                List<IFilterTarget> events = new List<IFilterTarget>();
                List<NodeState> nodesToRefresh = new List<NodeState>();

                lock (Lock)
                {
                    // check for server subscription.
                    if (monitoredItem.NodeId == ObjectIds.Server)
                    {
                        if (m_rootNotifiers != null)
                        {
                            nodesToRefresh.AddRange(m_rootNotifiers);
                        }
                    }
                    else
                    {
                        // check for existing monitored node.
                        UaMonitoredNode monitoredNode = null;

                        if (!MonitoredNodes.TryGetValue(monitoredItem.NodeId, out monitoredNode))
                        {
                            continue;
                        }

                        // get the refresh events.
                        nodesToRefresh.Add(monitoredNode.Node);
                    }
                }

                // block and wait for the refresh.
                for (int jj = 0; jj < nodesToRefresh.Count; jj++)
                {
                    nodesToRefresh[jj].ConditionRefresh(systemContext, events, true);
                }

                // queue the events.
                for (int jj = 0; jj < events.Count; jj++)
                {
                    // verify if the event can be received by the current monitored item
                    var result = ValidateEventRolePermissions(monitoredItem, events[jj]);
                    if (ServiceResult.IsBad(result))
                    {
                        continue;
                    }
                    monitoredItem.QueueEvent(events[jj]);
                }
            }

            // all done.
            return ServiceResult.Good;
        }

        /// <summary>
        /// Restore a set of monitored items after a restart.
        /// </summary>
        public virtual void RestoreMonitoredItems(
            IList<IUaStoredMonitoredItem> itemsToRestore,
            IList<IUaMonitoredItem> monitoredItems,
            IUserIdentity savedOwnerIdentity)
        {
            if (itemsToRestore == null) throw new ArgumentNullException(nameof(itemsToRestore));
            if (monitoredItems == null) throw new ArgumentNullException(nameof(monitoredItems));

            if (m_server.IsRunning)
            {
                throw new InvalidOperationException("Subscription restore can only occur on startup");
            }

            UaServerContext systemContext = m_systemContext.Copy();
            IDictionary<NodeId, NodeState> operationCache = new NodeIdDictionary<NodeState>();
            List<UaNodeHandle> nodesToValidate = new List<UaNodeHandle>();

            for (int ii = 0; ii < itemsToRestore.Count; ii++)
            {
                IUaStoredMonitoredItem itemToCreate = itemsToRestore[ii];

                // skip items that have already been processed.
                if (itemToCreate.IsRestored)
                {
                    continue;
                }

                // check for valid handle.
                UaNodeHandle handle = GetManagerHandle(systemContext, itemToCreate.NodeId, operationCache);

                if (handle == null)
                {
                    continue;
                }

                // owned by this node manager.
                itemToCreate.IsRestored = true;

                handle.Index = ii;
                nodesToValidate.Add(handle);
            }

            // check for nothing to do.
            if (nodesToValidate.Count == 0)
            {
                return;
            }

            // validates the nodes (reads values from the underlying data source if required).
            for (int ii = 0; ii < nodesToValidate.Count; ii++)
            {
                UaNodeHandle handle = nodesToValidate[ii];

                bool success = false;
                IUaMonitoredItem monitoredItem = null;

                lock (Lock)
                {
                    // validate node.
                    NodeState source = ValidateNode(systemContext, handle, operationCache);

                    if (source == null)
                    {
                        continue;
                    }

                    IUaStoredMonitoredItem itemToCreate = itemsToRestore[handle.Index];

                    // create monitored item.
                    success = RestoreMonitoredItem(
                        systemContext,
                        handle,
                        itemToCreate,
                        out monitoredItem);
                }

                if (!success)
                {
                    continue;
                }

                // save the monitored item.
                monitoredItems[handle.Index] = monitoredItem;
            }

            // do any post processing.
            OnCreateMonitoredItemsComplete(systemContext, monitoredItems);
        }

        /// <summary>
        /// Restore a single monitored Item after a restart
        /// </summary>
        /// <returns>true if sucesfully restored</returns>
        protected virtual bool RestoreMonitoredItem(
            UaServerContext context,
            UaNodeHandle handle,
            IUaStoredMonitoredItem storedMonitoredItem,
            out IUaMonitoredItem monitoredItem)
        {
            monitoredItem = null;

            // validate attribute.
            if (!Attributes.IsValid(handle.Node.NodeClass, storedMonitoredItem.AttributeId))
            {
                return false;
            }

            // check if the node is already being monitored.
            UaMonitoredNode monitoredNode = null;

            if (!m_monitoredNodes.TryGetValue(handle.Node.NodeId, out monitoredNode))
            {
                NodeState cachedNode = AddNodeToComponentCache(context, handle, handle.Node);
                m_monitoredNodes[handle.Node.NodeId] = monitoredNode = new UaMonitoredNode(this, cachedNode);
            }

            handle.Node = monitoredNode.Node;
            handle.MonitoredNode = monitoredNode;

            // put an upper limit on queue size.
            storedMonitoredItem.QueueSize = SubscriptionManager.CalculateRevisedQueueSize(storedMonitoredItem.IsDurable, storedMonitoredItem.QueueSize, m_maxQueueSize, m_maxDurableQueueSize);

            // create the item.
            UaMonitoredItem datachangeItem = new UaMonitoredItem(
                ServerData,
                this,
                handle,
                storedMonitoredItem);

            // update monitored item list.
            monitoredItem = datachangeItem;

            // save the monitored item.
            monitoredNode.Add(datachangeItem);

            // report change.
            OnMonitoredItemCreated(context, handle, datachangeItem);

            return true;
        }

        /// <summary>
        /// Creates a new set of monitored items for a set of variables.
        /// </summary>
        /// <remarks>
        /// This method only handles data change subscriptions. Event subscriptions are created by the SDK.
        /// </remarks>
        public virtual void CreateMonitoredItems(
            UaServerOperationContext context,
            uint subscriptionId,
            double publishingInterval,
            TimestampsToReturn timestampsToReturn,
            IList<MonitoredItemCreateRequest> itemsToCreate,
            IList<ServiceResult> errors,
            IList<MonitoringFilterResult> filterErrors,
            IList<IUaMonitoredItem> monitoredItems,
            bool createDurable,
            ref long globalIdCounter)
        {
            UaServerContext systemContext = m_systemContext.Copy(context);
            IDictionary<NodeId, NodeState> operationCache = new NodeIdDictionary<NodeState>();
            List<UaNodeHandle> nodesToValidate = new List<UaNodeHandle>();
            List<IUaMonitoredItem> createdItems = new List<IUaMonitoredItem>();

            for (int ii = 0; ii < itemsToCreate.Count; ii++)
            {
                MonitoredItemCreateRequest itemToCreate = itemsToCreate[ii];

                // skip items that have already been processed.
                if (itemToCreate.Processed)
                {
                    continue;
                }

                ReadValueId itemToMonitor = itemToCreate.ItemToMonitor;

                // check for valid handle.
                UaNodeHandle handle = GetManagerHandle(systemContext, itemToMonitor.NodeId, operationCache);

                if (handle == null)
                {
                    continue;
                }

                // owned by this node manager.
                itemToCreate.Processed = true;

                // must validate node in a separate operation.
                errors[ii] = StatusCodes.BadNodeIdUnknown;

                handle.Index = ii;
                nodesToValidate.Add(handle);
            }

            // check for nothing to do.
            if (nodesToValidate.Count == 0)
            {
                return;
            }

            // validates the nodes (reads values from the underlying data source if required).
            for (int ii = 0; ii < nodesToValidate.Count; ii++)
            {
                UaNodeHandle handle = nodesToValidate[ii];

                MonitoringFilterResult filterResult = null;
                IUaMonitoredItem monitoredItem = null;

                lock (Lock)
                {
                    // validate node.
                    NodeState source = ValidateNode(systemContext, handle, operationCache);

                    if (source == null)
                    {
                        continue;
                    }

                    MonitoredItemCreateRequest itemToCreate = itemsToCreate[handle.Index];

                    // create monitored item.
                    errors[handle.Index] = CreateMonitoredItem(
                        systemContext,
                        handle,
                        subscriptionId,
                        publishingInterval,
                        context.DiagnosticsMask,
                        timestampsToReturn,
                        itemToCreate,
                        createDurable,
                        ref globalIdCounter,
                        out filterResult,
                        out monitoredItem);
                }

                // save any filter error details.
                filterErrors[handle.Index] = filterResult;

                if (ServiceResult.IsBad(errors[handle.Index]))
                {
                    continue;
                }

                // save the monitored item.
                monitoredItems[handle.Index] = monitoredItem;
                createdItems.Add(monitoredItem);
            }

            // do any post processing.
            OnCreateMonitoredItemsComplete(systemContext, createdItems);
        }

        #region CreateMonitoredItem Support Functions
        /// <summary>
        /// Called when a batch of monitored items has been created.
        /// </summary>
        protected virtual void OnCreateMonitoredItemsComplete(UaServerContext context, IList<IUaMonitoredItem> monitoredItems)
        {
            // defined by the sub-class
        }

        /// <summary>
        /// Creates a new set of monitored items for a set of variables.
        /// </summary>
        /// <remarks>
        /// This method only handles data change subscriptions. Event subscriptions are created by the SDK.
        /// </remarks>
        protected virtual ServiceResult CreateMonitoredItem(
            UaServerContext context,
            UaNodeHandle handle,
            uint subscriptionId,
            double publishingInterval,
            DiagnosticsMasks diagnosticsMasks,
            TimestampsToReturn timestampsToReturn,
            MonitoredItemCreateRequest itemToCreate,
            bool createDurable,
            ref long globalIdCounter,
            out MonitoringFilterResult filterResult,
            out IUaMonitoredItem monitoredItem)
        {
            filterResult = null;
            monitoredItem = null;

            // validate parameters.
            MonitoringParameters parameters = itemToCreate.RequestedParameters;

            // validate attribute.
            if (!Attributes.IsValid(handle.Node.NodeClass, itemToCreate.ItemToMonitor.AttributeId))
            {
                return StatusCodes.BadAttributeIdInvalid;
            }

            // check if the node is already being monitored.
            UaMonitoredNode monitoredNode = null;

            if (!m_monitoredNodes.TryGetValue(handle.Node.NodeId, out monitoredNode))
            {
                NodeState cachedNode = AddNodeToComponentCache(context, handle, handle.Node);
                m_monitoredNodes[handle.Node.NodeId] = monitoredNode = new UaMonitoredNode(this, cachedNode);
            }

            handle.Node = monitoredNode.Node;
            handle.MonitoredNode = monitoredNode;

            // create a globally unique identifier.
            uint monitoredItemId = Utils.IncrementIdentifier(ref globalIdCounter);

            // determine the sampling interval.
            double samplingInterval = itemToCreate.RequestedParameters.SamplingInterval;

            if (samplingInterval < 0)
            {
                samplingInterval = publishingInterval;
            }

            // ensure minimum sampling interval is not exceeded.
            if (itemToCreate.ItemToMonitor.AttributeId == Attributes.Value)
            {
                BaseVariableState variable = handle.Node as BaseVariableState;

                if (variable != null && samplingInterval < variable.MinimumSamplingInterval)
                {
                    samplingInterval = variable.MinimumSamplingInterval;
                }
            }

            // put a large upper limit on sampling.
            if (samplingInterval == Double.MaxValue)
            {
                samplingInterval = 365 * 24 * 3600 * 1000.0;
            }

            // put an upper limit on queue size.
            uint revisedQueueSize = SubscriptionManager.CalculateRevisedQueueSize(createDurable, itemToCreate.RequestedParameters.QueueSize, m_maxQueueSize, m_maxDurableQueueSize);

            // validate the monitoring filter.
            Opc.Ua.Range euRange = null;
            MonitoringFilter filterToUse = null;

            ServiceResult error = ValidateMonitoringFilter(
                context,
                handle,
                itemToCreate.ItemToMonitor.AttributeId,
                samplingInterval,
                revisedQueueSize,
                parameters.Filter,
                out filterToUse,
                out euRange,
                out filterResult);

            if (ServiceResult.IsBad(error))
            {
                return error;
            }

            // create the item.
            UaMonitoredItem datachangeItem = new UaMonitoredItem(
                ServerData,
                this,
                handle,
                subscriptionId,
                monitoredItemId,
                itemToCreate.ItemToMonitor,
                diagnosticsMasks,
                timestampsToReturn,
                itemToCreate.MonitoringMode,
                itemToCreate.RequestedParameters.ClientHandle,
                filterToUse,
                filterToUse,
                euRange,
                samplingInterval,
                revisedQueueSize,
                itemToCreate.RequestedParameters.DiscardOldest,
                0,
                createDurable);

            // report the initial value.
            error = ReadInitialValue(context, handle, datachangeItem);
            if (ServiceResult.IsBad(error))
            {
                if (error.StatusCode == StatusCodes.BadAttributeIdInvalid ||
                    error.StatusCode == StatusCodes.BadDataEncodingInvalid ||
                    error.StatusCode == StatusCodes.BadDataEncodingUnsupported)
                {
                    return error;
                }
                error = StatusCodes.Good;
            }

            // update monitored item list.
            monitoredItem = datachangeItem;

            // save the monitored item.
            monitoredNode.Add(datachangeItem);

            // report change.
            OnMonitoredItemCreated(context, handle, datachangeItem);

            return error;
        }

        /// <summary>
        /// Reads the initial value for a monitored item.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="handle">The item handle.</param>
        /// <param name="monitoredItem">The monitored item.</param>
        protected virtual ServiceResult ReadInitialValue(
            ISystemContext context,
            UaNodeHandle handle,
            IUaDataChangeMonitoredItem2 monitoredItem)
        {
            DataValue initialValue = new DataValue
            {
                Value = null,
                ServerTimestamp = DateTime.UtcNow,
                SourceTimestamp = DateTime.MinValue,
                StatusCode = StatusCodes.BadWaitingForInitialData
            };

            ServiceResult error = handle.Node.ReadAttribute(
                context,
                monitoredItem.AttributeId,
                monitoredItem.IndexRange,
                monitoredItem.DataEncoding,
                initialValue);

            monitoredItem.QueueValue(initialValue, error, true);

            return error;
        }

        /// <summary>
        /// Called after creating a MonitoredItem.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="handle">The handle for the node.</param>
        /// <param name="monitoredItem">The monitored item.</param>
        protected virtual void OnMonitoredItemCreated(
            UaServerContext context,
            UaNodeHandle handle,
            UaMonitoredItem monitoredItem)
        {
            // overridden by the sub-class.
        }

        /// <summary>
        /// Validates Role permissions for the specified NodeId
        /// </summary>
        /// <param name="operationContext"></param>
        /// <param name="nodeId"></param>
        /// <param name="requestedPermission"></param>
        /// <returns></returns>
        public virtual ServiceResult ValidateRolePermissions(UaServerOperationContext operationContext, NodeId nodeId, PermissionType requestedPermission)
        {
            if (requestedPermission == PermissionType.None)
            {
                // no permission is required hence the validation passes.
                return StatusCodes.Good;
            }

            IUaNodeManager nodeManager = null;
            object nodeHandle = ServerData.NodeManager.GetManagerHandle(nodeId, out nodeManager);
            if (nodeHandle == null || nodeManager == null)
            {
                // ignore unknown nodes.
                return StatusCodes.Good;
            }

            UaNodeMetadata nodeMetadata = nodeManager.GetNodeMetadata(operationContext, nodeHandle, BrowseResultMask.All);

            return MasterNodeManager.ValidateRolePermissions(operationContext, nodeMetadata, requestedPermission);
        }

        /// <summary>
        /// Validates if the specified event monitored item has enough permissions to receive the specified event
        /// </summary>
        /// <returns></returns>
        public ServiceResult ValidateEventRolePermissions(IUaEventMonitoredItem monitoredItem, IFilterTarget filterTarget)
        {
            NodeId eventTypeId = null;
            NodeId sourceNodeId = null;
            BaseEventState baseEventState = filterTarget as BaseEventState;

            if (baseEventState == null && filterTarget is InstanceStateSnapshot snapshot)
            {
                // try to get the event instance from snapshot object
                baseEventState = snapshot.Handle as BaseEventState;
            }

            if (baseEventState != null)
            {
                eventTypeId = baseEventState.EventType?.Value;
                sourceNodeId = baseEventState.SourceNode?.Value;
            }

            UaServerOperationContext operationContext = new UaServerOperationContext(monitoredItem);

            // validate the event type id permissions as specified
            ServiceResult result = ValidateRolePermissions(operationContext, eventTypeId, PermissionType.ReceiveEvents);

            if (ServiceResult.IsBad(result))
            {
                return result;
            }

            // validate the source node id permissions as specified
            return ValidateRolePermissions(operationContext, sourceNodeId, PermissionType.ReceiveEvents);
        }

        /// <summary>
        /// Validates the monitoring filter specified by the client.
        /// </summary>
        protected virtual StatusCode ValidateMonitoringFilter(
            UaServerContext context,
            UaNodeHandle handle,
            uint attributeId,
            double samplingInterval,
            uint queueSize,
            ExtensionObject filter,
            out MonitoringFilter filterToUse,
            out Opc.Ua.Range range,
            out MonitoringFilterResult result)
        {
            range = null;
            filterToUse = null;
            result = null;

            // nothing to do if the filter is not specified.
            if (ExtensionObject.IsNull(filter))
            {
                return StatusCodes.Good;
            }

            // extension objects wrap any data structure. must check that the client provided the correct structure.
            DataChangeFilter deadbandFilter = ExtensionObject.ToEncodeable(filter) as DataChangeFilter;

            if (deadbandFilter == null)
            {
                AggregateFilter aggregateFilter = ExtensionObject.ToEncodeable(filter) as AggregateFilter;

                if (aggregateFilter == null || attributeId != Attributes.Value)
                {
                    return StatusCodes.BadFilterNotAllowed;
                }

                if (!ServerData.AggregateManager.IsSupported(aggregateFilter.AggregateType))
                {
                    return StatusCodes.BadAggregateNotSupported;
                }

                ServerAggregateFilter revisedFilter = new ServerAggregateFilter();
                revisedFilter.AggregateType = aggregateFilter.AggregateType;
                revisedFilter.StartTime = aggregateFilter.StartTime;
                revisedFilter.ProcessingInterval = aggregateFilter.ProcessingInterval;
                revisedFilter.AggregateConfiguration = aggregateFilter.AggregateConfiguration;
                revisedFilter.Stepped = false;

                StatusCode error = ReviseAggregateFilter(context, handle, samplingInterval, queueSize, revisedFilter);

                if (StatusCode.IsBad(error))
                {
                    return error;
                }

                AggregateFilterResult aggregateFilterResult = new AggregateFilterResult();
                aggregateFilterResult.RevisedProcessingInterval = aggregateFilter.ProcessingInterval;
                aggregateFilterResult.RevisedStartTime = aggregateFilter.StartTime;
                aggregateFilterResult.RevisedAggregateConfiguration = aggregateFilter.AggregateConfiguration;

                filterToUse = revisedFilter;
                result = aggregateFilterResult;
                return StatusCodes.Good;
            }

            // deadband filters only allowed for variable values.
            if (attributeId != Attributes.Value)
            {
                return StatusCodes.BadFilterNotAllowed;
            }

            BaseVariableState variable = handle.Node as BaseVariableState;

            if (variable == null)
            {
                return StatusCodes.BadFilterNotAllowed;
            }

            // check for status filter.
            if (deadbandFilter.DeadbandType == (uint)DeadbandType.None)
            {
                filterToUse = deadbandFilter;
                return StatusCodes.Good;
            }

            // deadband filters can only be used for numeric values.
            if (!ServerData.TypeTree.IsTypeOf(variable.DataType, DataTypeIds.Number))
            {
                return StatusCodes.BadFilterNotAllowed;
            }

            // nothing more to do for absolute filters.
            if (deadbandFilter.DeadbandType == (uint)DeadbandType.Absolute)
            {
                filterToUse = deadbandFilter;
                return StatusCodes.Good;
            }

            // need to look up the EU range if a percent filter is requested.
            if (deadbandFilter.DeadbandType == (uint)DeadbandType.Percent)
            {
                PropertyState property = handle.Node.FindChild(context, Opc.Ua.BrowseNames.EURange) as PropertyState;

                if (property == null)
                {
                    return StatusCodes.BadMonitoredItemFilterUnsupported;
                }

                range = property.Value as Opc.Ua.Range;

                if (range == null)
                {
                    return StatusCodes.BadMonitoredItemFilterUnsupported;
                }

                filterToUse = deadbandFilter;

                return StatusCodes.Good;
            }

            // no other type of filter supported.
            return StatusCodes.BadFilterNotAllowed;
        }

        /// <summary>
        /// Revises an aggregate filter (may require knowledge of the variable being used).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="handle">The handle.</param>
        /// <param name="samplingInterval">The sampling interval for the monitored item.</param>
        /// <param name="queueSize">The queue size for the monitored item.</param>
        /// <param name="filterToUse">The filter to revise.</param>
        /// <returns>Good if the </returns>
        protected virtual StatusCode ReviseAggregateFilter(
            UaServerContext context,
            UaNodeHandle handle,
            double samplingInterval,
            uint queueSize,
            ServerAggregateFilter filterToUse)
        {
            if (filterToUse.ProcessingInterval < samplingInterval)
            {
                filterToUse.ProcessingInterval = samplingInterval;
            }

            if (filterToUse.ProcessingInterval < ServerData.AggregateManager.MinimumProcessingInterval)
            {
                filterToUse.ProcessingInterval = ServerData.AggregateManager.MinimumProcessingInterval;
            }

            DateTime earliestStartTime = DateTime.UtcNow.AddMilliseconds(-(queueSize - 1) * filterToUse.ProcessingInterval);

            if (earliestStartTime > filterToUse.StartTime)
            {
                filterToUse.StartTime = earliestStartTime;
            }

            if (filterToUse.AggregateConfiguration.UseServerCapabilitiesDefaults)
            {
                filterToUse.AggregateConfiguration = ServerData.AggregateManager.GetDefaultConfiguration(null);
            }

            return StatusCodes.Good;
        }
        #endregion

        /// <summary>
        /// Modifies the parameters for a set of monitored items.
        /// </summary>
        public virtual void ModifyMonitoredItems(
            UaServerOperationContext context,
            TimestampsToReturn timestampsToReturn,
            IList<IUaMonitoredItem> monitoredItems,
            IList<MonitoredItemModifyRequest> itemsToModify,
            IList<ServiceResult> errors,
            IList<MonitoringFilterResult> filterErrors)
        {
            UaServerContext systemContext = m_systemContext.Copy(context);
            var nodesInNamespace = new List<(int, UaNodeHandle)>(monitoredItems.Count);

            for (var ii = 0; ii < monitoredItems.Count; ii++)
            {
                MonitoredItemModifyRequest itemToModify = itemsToModify[ii];

                // skip items that have already been processed.
                if (itemToModify.Processed || monitoredItems[ii] == null)
                {
                    continue;
                }

                // check handle.
                UaNodeHandle handle = IsHandleInNamespace(monitoredItems[ii].ManagerHandle);

                if (handle == null)
                {
                    continue;
                }

                nodesInNamespace.Add((ii, handle));
            }

            if (nodesInNamespace.Count == 0)
            {
                return;
            }

            var modifiedItems = new List<IUaMonitoredItem>();

            lock (Lock)
            {
                foreach (var nodeInNamespace in nodesInNamespace)
                {
                    int ii = nodeInNamespace.Item1;
                    var handle = nodeInNamespace.Item2;
                    MonitoredItemModifyRequest itemToModify = itemsToModify[ii];

                    // owned by this node manager.
                    itemToModify.Processed = true;

                    // modify the monitored item.
                    MonitoringFilterResult filterResult = null;

                    errors[ii] = ModifyMonitoredItem(
                        systemContext,
                        context.DiagnosticsMask,
                        timestampsToReturn,
                        monitoredItems[ii],
                        itemToModify,
                        handle,
                    out filterResult);

                    // save any filter error details.
                    filterErrors[ii] = filterResult;

                    // save the modified item.
                    if (ServiceResult.IsGood(errors[ii]))
                    {
                        modifiedItems.Add(monitoredItems[ii]);
                    }
                }
            }

            // do any post processing.
            OnModifyMonitoredItemsComplete(systemContext, modifiedItems);
        }

        #region ModifyMonitoredItem Support Functions
        /// <summary>
        /// Called when a batch of monitored items has been modified.
        /// </summary>
        protected virtual void OnModifyMonitoredItemsComplete(UaServerContext context, IList<IUaMonitoredItem> monitoredItems)
        {
            // defined by the sub-class
        }

        /// <summary>
        /// Modifies the parameters for a monitored item.
        /// </summary>
        protected virtual ServiceResult ModifyMonitoredItem(
            UaServerContext context,
            DiagnosticsMasks diagnosticsMasks,
            TimestampsToReturn timestampsToReturn,
            IUaMonitoredItem monitoredItem,
            MonitoredItemModifyRequest itemToModify,
            UaNodeHandle handle,
            out MonitoringFilterResult filterResult)
        {
            filterResult = null;

            // check for valid monitored item.
            UaMonitoredItem datachangeItem = monitoredItem as UaMonitoredItem;

            // validate parameters.
            MonitoringParameters parameters = itemToModify.RequestedParameters;

            double previousSamplingInterval = datachangeItem.SamplingInterval;

            // check if the variable needs to be sampled.
            double samplingInterval = itemToModify.RequestedParameters.SamplingInterval;

            if (samplingInterval < 0)
            {
                samplingInterval = previousSamplingInterval;
            }

            // ensure minimum sampling interval is not exceeded.
            if (datachangeItem.AttributeId == Attributes.Value)
            {
                BaseVariableState variable = handle.Node as BaseVariableState;

                if (variable != null && samplingInterval < variable.MinimumSamplingInterval)
                {
                    samplingInterval = variable.MinimumSamplingInterval;
                }
            }

            // put a large upper limit on sampling.
            if (samplingInterval == Double.MaxValue)
            {
                samplingInterval = 365 * 24 * 3600 * 1000.0;
            }

            // put an upper limit on queue size.
            uint revisedQueueSize = SubscriptionManager.CalculateRevisedQueueSize(monitoredItem.IsDurable, itemToModify.RequestedParameters.QueueSize, m_maxQueueSize, m_maxDurableQueueSize);

            // validate the monitoring filter.
            Opc.Ua.Range euRange = null;
            MonitoringFilter filterToUse = null;

            ServiceResult error = ValidateMonitoringFilter(
                context,
                handle,
                datachangeItem.AttributeId,
                samplingInterval,
                revisedQueueSize,
                parameters.Filter,
                out filterToUse,
                out euRange,
                out filterResult);

            if (ServiceResult.IsBad(error))
            {
                return error;
            }

            // modify the monitored item parameters.
            error = datachangeItem.ModifyAttributes(
                diagnosticsMasks,
                timestampsToReturn,
                itemToModify.RequestedParameters.ClientHandle,
                filterToUse,
                filterToUse,
                euRange,
                samplingInterval,
                revisedQueueSize,
                itemToModify.RequestedParameters.DiscardOldest);

            // report change.
            if (ServiceResult.IsGood(error))
            {
                OnMonitoredItemModified(context, handle, datachangeItem);
            }

            return error;
        }

        /// <summary>
        /// Called after modifying a MonitoredItem.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="handle">The handle for the node.</param>
        /// <param name="monitoredItem">The monitored item.</param>
        protected virtual void OnMonitoredItemModified(
            UaServerContext context,
            UaNodeHandle handle,
            UaMonitoredItem monitoredItem)
        {
            // overridden by the sub-class.
        }
        #endregion

        /// <summary>
        /// Deletes a set of monitored items.
        /// </summary>
        public virtual void DeleteMonitoredItems(
            UaServerOperationContext context,
            IList<IUaMonitoredItem> monitoredItems,
            IList<bool> processedItems,
            IList<ServiceResult> errors)
        {
            UaServerContext systemContext = m_systemContext.Copy(context);
            var nodesInNamespace = new List<(int, UaNodeHandle)>(monitoredItems.Count);

            for (int ii = 0; ii < monitoredItems.Count; ii++)
            {
                // skip items that have already been processed.
                if (processedItems[ii] || monitoredItems[ii] == null)
                {
                    continue;
                }

                // check handle.
                UaNodeHandle handle = IsHandleInNamespace(monitoredItems[ii].ManagerHandle);

                if (handle == null)
                {
                    continue;
                }

                nodesInNamespace.Add((ii, handle));
            }

            if (nodesInNamespace.Count == 0)
            {
                return;
            }

            var deletedItems = new List<IUaMonitoredItem>();

            lock (Lock)
            {
                foreach (var nodeInNamespace in nodesInNamespace)
                {
                    int ii = nodeInNamespace.Item1;
                    var handle = nodeInNamespace.Item2;

                    // owned by this node manager.
                    processedItems[ii] = true;

                    errors[ii] = DeleteMonitoredItem(
                        systemContext,
                        monitoredItems[ii],
                        handle);

                    // save the modified item.
                    if (ServiceResult.IsGood(errors[ii]))
                    {
                        deletedItems.Add(monitoredItems[ii]);
                        RemoveNodeFromComponentCache(systemContext, handle);
                    }
                }
            }

            // do any post processing.
            OnDeleteMonitoredItemsComplete(systemContext, deletedItems);
        }

        #region DeleteMonitoredItems Support Functions
        /// <summary>
        /// Called when a batch of monitored items has been modified.
        /// </summary>
        protected virtual void OnDeleteMonitoredItemsComplete(UaServerContext context, IList<IUaMonitoredItem> monitoredItems)
        {
            // defined by the sub-class
        }

        /// <summary>
        /// Deletes a monitored item.
        /// </summary>
        protected virtual ServiceResult DeleteMonitoredItem(
            UaServerContext context,
            IUaMonitoredItem monitoredItem,
            UaNodeHandle handle)
        {
            // check for valid monitored item.
            UaMonitoredItem datachangeItem = monitoredItem as UaMonitoredItem;

            // check if the node is already being monitored.
            UaMonitoredNode monitoredNode = null;

            if (m_monitoredNodes.TryGetValue(handle.NodeId, out monitoredNode))
            {
                monitoredNode.Remove(datachangeItem);

                // check if node is no longer being monitored.
                if (!monitoredNode.HasMonitoredItems)
                {
                    m_monitoredNodes.Remove(handle.NodeId);
                }
            }

            // report change.
            OnMonitoredItemDeleted(context, handle, datachangeItem);

            return ServiceResult.Good;
        }

        /// <summary>
        /// Called after deleting a MonitoredItem.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="handle">The handle for the node.</param>
        /// <param name="monitoredItem">The monitored item.</param>
        protected virtual void OnMonitoredItemDeleted(
            UaServerContext context,
            UaNodeHandle handle,
            UaMonitoredItem monitoredItem)
        {
            // overridden by the sub-class.
        }
        #endregion

        #region TransferMonitoredItems Support Functions
        /// <summary>
        /// Transfers a set of monitored items.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sendInitialValues">Whether the subscription should send initial values after transfer.</param>
        /// <param name="monitoredItems">The set of monitoring items to update.</param>
        /// <param name="processedItems">The list of bool with items that were already processed.</param>
        /// <param name="errors">Any errors.</param>
        public virtual void TransferMonitoredItems(
            UaServerOperationContext context,
            bool sendInitialValues,
            IList<IUaMonitoredItem> monitoredItems,
            IList<bool> processedItems,
            IList<ServiceResult> errors)
        {
            UaServerContext systemContext = m_systemContext.Copy(context);
            IList<IUaMonitoredItem> transferredItems = new List<IUaMonitoredItem>();
            lock (Lock)
            {
                for (int ii = 0; ii < monitoredItems.Count; ii++)
                {
                    // skip items that have already been processed.
                    if (processedItems[ii] || monitoredItems[ii] == null)
                    {
                        continue;
                    }

                    // check handle.
                    UaNodeHandle handle = IsHandleInNamespace(monitoredItems[ii].ManagerHandle);
                    if (handle == null)
                    {
                        continue;
                    }

                    // owned by this node manager.
                    processedItems[ii] = true;
                    transferredItems.Add(monitoredItems[ii]);
                    if (sendInitialValues)
                    {
                        monitoredItems[ii].SetupResendDataTrigger();
                    }
                    errors[ii] = StatusCodes.Good;
                }
            }

            // do any post processing.
            OnMonitoredItemsTransferred(systemContext, transferredItems);
        }

        /// <summary>
        /// Called after transfer of MonitoredItems.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="monitoredItems">The transferred monitored items.</param>
        protected virtual void OnMonitoredItemsTransferred(
            UaServerContext context,
            IList<IUaMonitoredItem> monitoredItems
            )
        {
            // defined by the sub-class
        }
        #endregion

        /// <summary>
        /// Changes the monitoring mode for a set of monitored items.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="monitoringMode">The monitoring mode.</param>
        /// <param name="monitoredItems">The set of monitoring items to update.</param>
        /// <param name="processedItems">Flags indicating which items have been processed.</param>
        /// <param name="errors">Any errors.</param>
        public virtual void SetMonitoringMode(
            UaServerOperationContext context,
            MonitoringMode monitoringMode,
            IList<IUaMonitoredItem> monitoredItems,
            IList<bool> processedItems,
            IList<ServiceResult> errors)
        {
            UaServerContext systemContext = m_systemContext.Copy(context);
            var nodesInNamespace = new List<(int, UaNodeHandle)>(monitoredItems.Count);

                for (var ii = 0; ii < monitoredItems.Count; ii++)
                {
                    // skip items that have already been processed.
                    if (processedItems[ii] || monitoredItems[ii] == null)
                    {
                        continue;
                    }

                    // check handle.
                    UaNodeHandle handle = IsHandleInNamespace(monitoredItems[ii].ManagerHandle);

                    if (handle == null)
                    {
                        continue;
                    }

                    nodesInNamespace.Add((ii, handle));
                }

                if (nodesInNamespace.Count == 0)
                {
                    return;
                }

                var changedItems = new List<IUaMonitoredItem>();

                lock (Lock)
                {
                    foreach (var nodeInNamespace in nodesInNamespace)
                    {
                        int ii = nodeInNamespace.Item1;
                        var handle = nodeInNamespace.Item2;

                        // indicate whether it was processed or not.
                        processedItems[ii] = true;

                        // update monitoring mode.
                        errors[ii] = SetMonitoringMode(
                            systemContext,
                            monitoredItems[ii],
                            monitoringMode,
                            handle);

                        // save the modified item.
                        if (ServiceResult.IsGood(errors[ii]))
                        {
                            changedItems.Add(monitoredItems[ii]);
                        }
                    }
                }

                // do any post processing.
                OnSetMonitoringModeComplete(systemContext, changedItems);
            }

        #region SetMonitoringMode Support Functions
            /// <summary>
            /// Called when a batch of monitored items has their monitoring mode changed.
            /// </summary>
        protected virtual void OnSetMonitoringModeComplete(UaServerContext context, IList<IUaMonitoredItem> monitoredItems)
        {
            // defined by the sub-class
        }

        /// <summary>
        /// Changes the monitoring mode for an item.
        /// </summary>
        protected virtual ServiceResult SetMonitoringMode(
            UaServerContext context,
            IUaMonitoredItem monitoredItem,
            MonitoringMode monitoringMode,
            UaNodeHandle handle)
        {
            // check for valid monitored item.
            UaMonitoredItem datachangeItem = monitoredItem as UaMonitoredItem;

            // update monitoring mode.
            MonitoringMode previousMode = datachangeItem.SetMonitoringMode(monitoringMode);

            // must send the latest value after enabling a disabled item.
            if (monitoringMode == MonitoringMode.Reporting && previousMode == MonitoringMode.Disabled)
            {
                handle.MonitoredNode.QueueValue(context, handle.Node, datachangeItem);
            }

            // report change.
            if (previousMode != monitoringMode)
            {
                OnMonitoringModeChanged(
                    context,
                    handle,
                    datachangeItem,
                    previousMode,
                    monitoringMode);
            }

            return ServiceResult.Good;
        }

        /// <summary>
        /// Called after changing the MonitoringMode for a MonitoredItem.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="handle">The handle for the node.</param>
        /// <param name="monitoredItem">The monitored item.</param>
        /// <param name="previousMode">The previous monitoring mode.</param>
        /// <param name="monitoringMode">The current monitoring mode.</param>
        protected virtual void OnMonitoringModeChanged(
            UaServerContext context,
            UaNodeHandle handle,
            UaMonitoredItem monitoredItem,
            MonitoringMode previousMode,
            MonitoringMode monitoringMode)
        {
            // overridden by the sub-class.
        }
        #endregion
        #endregion

        #region IUaNodeManager Members
        /// <summary>
        /// Called when a session is closed.
        /// </summary>
        /// <param name="context">The UA server implementation of the ISystemContext interface.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="deleteSubscriptions">if set to <c>true</c> subscriptions are to be deleted.</param>
        public virtual void SessionClosing(UaServerOperationContext context, NodeId sessionId, bool deleteSubscriptions)
        {
        }

        /// <summary>
        /// Returns true if a node is in a view.
        /// </summary>
        /// <param name="context">The UA server implementation of the ISystemContext interface.</param>
        /// <param name="viewId">The view identifier.</param>
        /// <param name="nodeHandle">The node to check.</param>
        public virtual bool IsNodeInView(UaServerOperationContext context, NodeId viewId, object nodeHandle)
        {
            var handle = nodeHandle as UaNodeHandle;

            if (handle == null)
            {
                return false;
            }

            if (handle.Node != null)
            {
                return IsNodeInView(context, viewId, handle.Node);
            }

            return false;
        }

        /// <summary>
        /// Returns the metadata containing the AccessRestrictions, RolePermissions and UserRolePermissions for the node.
        /// Returns null if the node does not exist.
        /// </summary>
        /// <remarks>
        /// This method validates any placeholder handle.
        /// </remarks>
        public virtual UaNodeMetadata GetPermissionMetadata(
            UaServerOperationContext context,
            object targetHandle,
            BrowseResultMask resultMask,
            Dictionary<NodeId, List<object>> uniqueNodesServiceAttributesCache,
            bool permissionsOnly)
        {
            UaServerContext systemContext = m_systemContext.Copy(context);

            // check for valid handle.
            UaNodeHandle handle = IsHandleInNamespace(targetHandle);

            if (handle == null)
            {
                return null;
            }

            lock (Lock)
            {
                // validate node.
                NodeState target = ValidateNode(systemContext, handle, null);

                if (target == null)
                {
                    return null;
                }

                List<object> values = null;

                // construct the meta-data object.
                UaNodeMetadata metadata = new UaNodeMetadata(target, target.NodeId);

                // Treat the case of calls originating from the optimized services that use the cache (Read, Browse and Call services)
                if (uniqueNodesServiceAttributesCache != null)
                {
                    NodeId key = handle.NodeId;
                    if (uniqueNodesServiceAttributesCache.ContainsKey(key))
                    {
                        if (uniqueNodesServiceAttributesCache[key].Count == 0)
                        {
                            values = ReadAndCacheValidationAttributes(uniqueNodesServiceAttributesCache, systemContext, target, key);
                        }
                        else
                        {
                            // Retrieve value from cache
                            values = uniqueNodesServiceAttributesCache[key];
                        }
                    }
                    else
                    {
                        values = ReadAndCacheValidationAttributes(uniqueNodesServiceAttributesCache, systemContext, target, key);
                    }

                    SetAccessAndRolePermissions(values, metadata);
                }// All other calls that do not use the cache
                else if (permissionsOnly == true)
                {
                    values = ReadValidationAttributes(systemContext, target);
                    SetAccessAndRolePermissions(values, metadata);
                }

                SetDefaultPermissions(systemContext, target, metadata);

                return metadata;
            }
        }


        /// <summary>
        /// Set the metadata default permission values for DefaultAccessRestrictions, DefaultRolePermissions and DefaultUserRolePermissions
        /// </summary>
        /// <param name="systemContext"></param>
        /// <param name="target"></param>
        /// <param name="metadata"></param>
        private void SetDefaultPermissions(UaServerContext systemContext, NodeState target, UaNodeMetadata metadata)
        {
            // check if NamespaceMetadata is defined for NamespaceUri
            string namespaceUri = ServerData.NamespaceUris.GetString(target.NodeId.NamespaceIndex);
            NamespaceMetadataState namespaceMetadataState = ServerData.NodeManager.ConfigurationNodeManager.GetNamespaceMetadataState(namespaceUri);

            if (namespaceMetadataState != null)
            {
                List<object> namespaceMetadataValues;

                if (namespaceMetadataState.DefaultAccessRestrictions != null)
                {
                    // get DefaultAccessRestrictions for Namespace
                    namespaceMetadataValues = namespaceMetadataState.DefaultAccessRestrictions.ReadAttributes(systemContext, Attributes.Value);

                    if (namespaceMetadataValues[0] != null)
                    {
                        metadata.DefaultAccessRestrictions = (AccessRestrictionType)Enum.ToObject(typeof(AccessRestrictionType), namespaceMetadataValues[0]);
                    }
                }

                if (namespaceMetadataState.DefaultRolePermissions != null)
                {
                    // get DefaultRolePermissions for Namespace
                    namespaceMetadataValues = namespaceMetadataState.DefaultRolePermissions.ReadAttributes(systemContext, Attributes.Value);

                    if (namespaceMetadataValues[0] != null)
                    {
                        metadata.DefaultRolePermissions = new RolePermissionTypeCollection(ExtensionObject.ToList<RolePermissionType>(namespaceMetadataValues[0]));
                    }
                }

                if (namespaceMetadataState.DefaultUserRolePermissions != null)
                {
                    // get DefaultUserRolePermissions for Namespace
                    namespaceMetadataValues = namespaceMetadataState.DefaultUserRolePermissions.ReadAttributes(systemContext, Attributes.Value);

                    if (namespaceMetadataValues[0] != null)
                    {
                        metadata.DefaultUserRolePermissions = new RolePermissionTypeCollection(ExtensionObject.ToList<RolePermissionType>(namespaceMetadataValues[0]));
                    }
                }
            }
        }
        #endregion


        #region ComponentCache Functions
        /// <summary>
        /// Stores a reference count for entries in the component cache.
        /// </summary>
        private class CacheEntry
        {
            public int RefCount;
            public NodeState Entry;
        }

        /// <summary>
        /// Looks up a component in cache.
        /// </summary>
        protected NodeState LookupNodeInComponentCache(ISystemContext context, UaNodeHandle handle)
        {
            lock (Lock)
            {
                if (m_componentCache == null)
                {
                    return null;
                }

                CacheEntry entry = null;

                if (!String.IsNullOrEmpty(handle.ComponentPath))
                {
                    if (m_componentCache.TryGetValue(handle.RootId, out entry))
                    {
                        return entry.Entry.FindChildBySymbolicName(context, handle.ComponentPath);
                    }
                }
                else
                {
                    if (m_componentCache.TryGetValue(handle.NodeId, out entry))
                    {
                        return entry.Entry;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Removes a reference to a component in the cache.
        /// </summary>
        protected void RemoveNodeFromComponentCache(ISystemContext context, UaNodeHandle handle)
        {
            lock (Lock)
            {
                if (handle == null)
                {
                    return;
                }

                if (m_componentCache != null)
                {
                    NodeId nodeId = handle.NodeId;

                    if (!String.IsNullOrEmpty(handle.ComponentPath))
                    {
                        nodeId = handle.RootId;
                    }

                    CacheEntry entry = null;

                    if (m_componentCache.TryGetValue(nodeId, out entry))
                    {
                        entry.RefCount--;

                        if (entry.RefCount == 0)
                        {
                            m_componentCache.Remove(nodeId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds a node to the component cache.
        /// </summary>
        protected NodeState AddNodeToComponentCache(ISystemContext context, UaNodeHandle handle, NodeState node)
        {
            lock (Lock)
            {
                if (handle == null)
                {
                    return node;
                }

                if (m_componentCache == null)
                {
                    m_componentCache = new NodeIdDictionary<CacheEntry>();
                }

                // check if a component is actually specified.
                if (!String.IsNullOrEmpty(handle.ComponentPath))
                {
                    CacheEntry entry = null;

                    if (m_componentCache.TryGetValue(handle.RootId, out entry))
                    {
                        entry.RefCount++;

                        if (!String.IsNullOrEmpty(handle.ComponentPath))
                        {
                            return entry.Entry.FindChildBySymbolicName(context, handle.ComponentPath);
                        }

                        return entry.Entry;
                    }

                    NodeState root = node.GetHierarchyRoot();

                    if (root != null)
                    {
                        entry = new CacheEntry();
                        entry.RefCount = 1;
                        entry.Entry = root;
                        m_componentCache.Add(handle.RootId, entry);
                    }
                }

                // simply add the node to the cache.
                else
                {
                    CacheEntry entry = null;

                    if (m_componentCache.TryGetValue(handle.NodeId, out entry))
                    {
                        entry.RefCount++;
                        return entry.Entry;
                    }

                    entry = new CacheEntry();
                    entry.RefCount = 1;
                    entry.Entry = node;
                    m_componentCache.Add(handle.NodeId, entry);
                }

                return node;
            }
        }

        #endregion

        #region Protected Members
        /// <summary>
        /// A list of references
        /// </summary>
        protected IList<IReference> References { get; set; }
        #endregion

        #region Create Node Classes
        /// <summary>
        /// <para>Creates a new folder.</para>
        /// <para>Folders are used to organize the AddressSpace into a hierarchy of Nodes. They represent the root Node of a subtree, and have no other semantics associated
        /// with them.</para>
        /// </summary>
        /// <param name="parent">The parent NodeState object the new folder will be created in.</param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.<br /></para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <returns>The created folder object which can be used in further calls to <see cref="CreateFolderState" />.</returns>
        public FolderState CreateFolderState(NodeState parent, string browseName, LocalizedText displayName,
            LocalizedText description, AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = new RolePermissionTypeCollection();
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = new RolePermissionTypeCollection();
            }

            var folderState = new FolderState(parent) {
                SymbolicName = displayName.ToString(),
                ReferenceTypeId = ReferenceTypes.Organizes,
                TypeDefinitionId = ObjectTypeIds.FolderType,
                NodeId = new NodeId(browseName, NamespaceIndex),
                BrowseName = new QualifiedName(browseName, NamespaceIndex),
                DisplayName = displayName,
                Description = description,
                WriteMask = writeMask,
                UserWriteMask = userWriteMask,
                RolePermissions = rolePermissions,
                UserRolePermissions = userRolePermissions,
                EventNotifier = EventNotifiers.None
            };

            if (parent != null)
            {
                parent.AddChild(folderState);
            }
            else
            {
                folderState.AddReference(ReferenceTypes.Organizes, true, ObjectIds.ObjectsFolder);
                References.Add(new NodeStateReference(ReferenceTypes.Organizes, false, folderState.NodeId));
                folderState.EventNotifier = EventNotifiers.SubscribeToEvents;
                AddRootNotifier(folderState);
            }

            return folderState;
        }

        /// <summary>
        ///   <para>Creates a new Object NodeClass.</para>
        ///   <para>Objects are used to represent systems, system components, real-world objects and software objects. Objects are defined using the BaseObjectState class.</para>
        /// </summary>
        /// <param name="parent">The parent NodeState object the new Object NodeClass will be created in.</param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.<br /></para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.<br /></para>
        /// </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <returns>The created Object NodeClass.</returns>
        public BaseObjectState CreateBaseObjectState(NodeState parent, string browseName,
            LocalizedText displayName, LocalizedText description,
            AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = new RolePermissionTypeCollection();
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = new RolePermissionTypeCollection();
            }

            var baseObjectState = new BaseObjectState(parent) {
                SymbolicName = displayName.ToString(),
                ReferenceTypeId = ReferenceTypes.Organizes,
                TypeDefinitionId = ObjectTypeIds.BaseObjectType,
                NodeId = new NodeId(browseName, NamespaceIndex),
                BrowseName = new QualifiedName(browseName, NamespaceIndex),
                DisplayName = displayName,
                Description = description,
                WriteMask = writeMask,
                UserWriteMask = userWriteMask,
                RolePermissions = rolePermissions,
                UserRolePermissions = userRolePermissions,
                EventNotifier = EventNotifiers.None
            };

            parent?.AddChild(baseObjectState);

            return baseObjectState;
        }

        /// <summary>
        ///   <para>Creates a new Property</para>
        ///   <para>Properties are used to define the characteristics of Nodes. Properties are defined using the Variable NodeClass. However, they restrict their use.</para>
        ///   <para>Properties are the leaf of any hierarchy; therefore they shall not be the SourceNode of any hierarchical References. This includes the HasComponent or
        /// HasProperty Reference, that is, Properties do not contain Properties and cannot expose their complex structure. However, they may be the SourceNode of any
        /// non-hierarchical References.</para>
        ///   <para>The HasTypeDefinition Reference points to the VariableType of the Property. Since Properties are uniquely identified by their BrowseName, all Properties
        /// shall point to the PropertyType defined in OPC 10000-5.</para>
        ///   <para>Properties shall always be defined in the context of another Node and shall be the TargetNode of at least one HasProperty Reference. To distinguish them
        /// from DataVariables, they shall not be the TargetNode of any HasComponent Reference. Thus, a HasProperty Reference pointing to a Variable Node defines this Node
        /// as a Property.</para>
        ///   <para>The BrowseName of a Property is always unique in the context of a Node. It is not permitted for a Node to refer to two Variables using HasProperty
        /// References having the same BrowseName.</para>
        /// </summary>
        /// <param name="parent">The parent NodeState object the new Property will be created in.</param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="dataType">
        ///     The data type of the new variable, e.g. <see cref="BuiltInType.SByte" />. See <see cref="BuiltInType" /> for all possible types
        /// </param>
        /// <param name="valueRank">
        ///     The value rank of the new variable, e.g. <see cref="ValueRanks.Scalar" />. See <see cref="ValueRanks" /> for all possible value ranks.
        /// </param>
        /// <param name="accessLevel">
        ///     The access level of the new variable, e.g. <see cref="AccessLevels.CurrentRead" />. See <see cref="AccessLevels" /> for all possible access levels.
        /// </param>
        /// <param name="initialValue">The initial value. If null a default value is used as initial value.</param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.</para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <returns>The created property object.</returns>
        public PropertyState CreatePropertyState(NodeState parent, string browseName,
            LocalizedText displayName, LocalizedText description, BuiltInType dataType, int valueRank, byte accessLevel,
            object initialValue, AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = new RolePermissionTypeCollection();
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = new RolePermissionTypeCollection();
            }

            var propertyState = new PropertyState(parent) {
                SymbolicName = displayName.ToString(),
                TypeDefinitionId = VariableTypeIds.PropertyType,
                ReferenceTypeId = ReferenceTypeIds.HasProperty,
                NodeId = new NodeId(browseName, NamespaceIndex),
                BrowseName = new QualifiedName(browseName, NamespaceIndex),
                DisplayName = displayName,
                Description = description,
                WriteMask = writeMask,
                UserWriteMask = userWriteMask,
                RolePermissions = rolePermissions,
                UserRolePermissions = userRolePermissions,
                DataType = (uint)dataType,
                ValueRank = valueRank,
                AccessLevel = accessLevel,
                UserAccessLevel = accessLevel,
                Historizing = false
            };

            if (initialValue != null)
            {
                propertyState.Value = initialValue;
            }

            parent?.AddChild(propertyState);

            return propertyState;
        }

        /// <summary>
        ///   <para>Creates a new View NodeClass.</para>
        ///   <para>Underlying systems are often large, and Clients often have an interest in only a specific subset of the data. They do not need, or want, to be burdened with
        /// viewing Nodes in the AddressSpace for which they have no interest.</para>
        ///   <para>To address this problem, this standard defines the concept of a View. Each View defines a subset of the Nodes in the AddressSpace. The entire AddressSpace
        /// is the default View. Each Node in a View may contain only a subset of its References, as defined by the creator of the View. The View Node acts as the root for
        /// the Nodes in the View. Views are defined using the View NodeClass.</para>
        ///   <para>All Nodes contained in a View shall be accessible starting from the View Node when browsing in the context of the View. It is not expected that all
        /// containing Nodes can be browsed directly from the View Node but rather browsed from other Nodes contained in the View.</para>
        ///   <para>A View Node may not only be used as additional entry point into the AddressSpace but as a construct to organize the AddressSpace and thus as the only entry
        /// point into a subset of the AddressSpace. Therefore, Clients shall not ignore View Nodes when exposing the AddressSpace. Simple Clients that do not deal with
        /// Views for filtering purposes can, for example, handle a View Node like an Object of type FolderType.<br /></para>
        /// </summary>
        /// <param name="parent">The parent NodeState object the new View NodeClass will be created in.</param>
        /// <param name="externalReferences">
        ///   <para>The externalReferences is an out parameter that allows the generic server to link to nodes.</para>
        /// </param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.</para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <returns>The created <see cref="ViewState" /></returns>
        protected ViewState CreateViewState(NodeState parent,
            IDictionary<NodeId, IList<IReference>> externalReferences, string browseName, LocalizedText displayName,
            LocalizedText description, AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = new RolePermissionTypeCollection();
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = new RolePermissionTypeCollection();
            }

            var viewState = new ViewState {
                SymbolicName = displayName.ToString(),
                NodeId = new NodeId(browseName, NamespaceIndex),
                BrowseName = new QualifiedName(browseName, NamespaceIndex),
                DisplayName = displayName,
                Description = description,
                WriteMask = writeMask,
                UserWriteMask = userWriteMask,
                RolePermissions = rolePermissions,
                UserRolePermissions = userRolePermissions,
                ContainsNoLoops = true
            };

            if (externalReferences != null)
            {
                if (!externalReferences.TryGetValue(ObjectIds.ViewsFolder, out IList<IReference> references))
                {
                    externalReferences[ObjectIds.ViewsFolder] = references = new List<IReference>();
                }
                viewState.AddReference(ReferenceTypeIds.Organizes, true, ObjectIds.ViewsFolder);
                references.Add(new NodeStateReference(ReferenceTypeIds.Organizes, false, viewState.NodeId));
            }

            if (parent != null)
            {
                parent.AddReference(ReferenceTypes.Organizes, false, viewState.NodeId);
                viewState.AddReference(ReferenceTypes.Organizes, true, parent.NodeId);
            }

            AddPredefinedNode(SystemContext, viewState);
            return viewState;
        }

        /// <summary>Creates a new DataVariable NodeClass.</summary>
        /// <param name="parent">The parent NodeState object the new DataVariable NodeClass will be created in.</param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="dataType">
        ///     The data type of the new variable, e.g. <see cref="BuiltInType.SByte" />. See
        ///     <see cref="BuiltInType" /> for all possible types
        /// </param>
        /// <param name="valueRank">
        ///     The value rank of the new variable, e.g. <see cref="ValueRanks.Scalar" />. See
        ///     <see cref="ValueRanks" /> for all possible value ranks.
        /// </param>
        /// <param name="accessLevel">
        ///     The access level of the new variable, e.g. <see cref="AccessLevels.CurrentRead" />. See
        ///     <see cref="AccessLevels" /> for all possible access levels.
        /// </param>
        /// <param name="initialValue">The initial value. If null a default value is used as initial value.</param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.</para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <returns>The created <see cref="BaseDataVariableState" /></returns>
        public BaseDataVariableState CreateBaseDataVariableState(NodeState parent, string browseName,
            LocalizedText displayName, LocalizedText description, BuiltInType dataType, int valueRank, byte accessLevel,
            object initialValue, AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = new RolePermissionTypeCollection();
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = new RolePermissionTypeCollection();
            }

            var baseDataVariableTypeState = new BaseDataVariableState(parent) {
                SymbolicName = displayName.ToString(),
                ReferenceTypeId = ReferenceTypes.Organizes,
                TypeDefinitionId = VariableTypeIds.BaseDataVariableType,
                NodeId = new NodeId(browseName, NamespaceIndex),
                BrowseName = new QualifiedName(browseName, NamespaceIndex),
                DisplayName = displayName,
                Description = description,
                WriteMask = writeMask,
                UserWriteMask = userWriteMask,
                RolePermissions = rolePermissions,
                UserRolePermissions = userRolePermissions,
                DataType = (uint)dataType,
                ValueRank = valueRank,
                AccessLevel = accessLevel,
                UserAccessLevel = accessLevel,
                Historizing = false
            };

            baseDataVariableTypeState.Value = initialValue ?? GetNewValue(baseDataVariableTypeState);
            baseDataVariableTypeState.StatusCode = StatusCodes.Good;
            baseDataVariableTypeState.Timestamp = DateTime.UtcNow;

            if (valueRank == ValueRanks.OneDimension)
            {
                baseDataVariableTypeState.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0 });
            }
            else if (valueRank == ValueRanks.TwoDimensions)
            {
                baseDataVariableTypeState.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0, 0 });
            }

            parent?.AddChild(baseDataVariableTypeState);

            return baseDataVariableTypeState;
        }

        /// <summary>Creates a new DataVariable NodeClass.</summary>
        /// <param name="parent">The parent NodeState object the new DataVariable NodeClass will be created in.</param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="dataType">
        ///     The Node Id of the node used as data type of the new variable.
        /// </param>
        /// <param name="valueRank">
        ///     The value rank of the new variable, e.g. <see cref="ValueRanks.Scalar" />. See
        ///     <see cref="ValueRanks" /> for all possible value ranks.
        /// </param>
        /// <param name="accessLevel">
        ///     The access level of the new variable, e.g. <see cref="AccessLevels.CurrentRead" />. See
        ///     <see cref="AccessLevels" /> for all possible access levels.
        /// </param>
        /// <param name="initialValue">The initial value. If null a default value is used as initial value.</param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.</para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <returns>The created <see cref="BaseDataVariableState" /></returns>
        public BaseDataVariableState CreateBaseDataVariableState(NodeState parent, string browseName,
            LocalizedText displayName, LocalizedText description, NodeId dataType, int valueRank, byte accessLevel,
            object initialValue, AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = new RolePermissionTypeCollection();
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = new RolePermissionTypeCollection();
            }

            var baseDataVariableTypeState = new BaseDataVariableState(parent) {
                SymbolicName = displayName.ToString(),
                ReferenceTypeId = ReferenceTypes.Organizes,
                TypeDefinitionId = VariableTypeIds.BaseDataVariableType,
                NodeId = new NodeId(browseName, NamespaceIndex),
                BrowseName = new QualifiedName(browseName, NamespaceIndex),
                DisplayName = displayName,
                Description = description,
                WriteMask = writeMask,
                UserWriteMask = userWriteMask,
                RolePermissions = rolePermissions,
                UserRolePermissions = userRolePermissions,
                DataType = dataType,
                ValueRank = valueRank,
                AccessLevel = accessLevel,
                UserAccessLevel = accessLevel,
                Historizing = false
            };

            baseDataVariableTypeState.Value = initialValue ?? GetNewValue(baseDataVariableTypeState);
            baseDataVariableTypeState.StatusCode = StatusCodes.Good;
            baseDataVariableTypeState.Timestamp = DateTime.UtcNow;

            if (valueRank == ValueRanks.OneDimension)
            {
                baseDataVariableTypeState.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0 });
            }
            else if (valueRank == ValueRanks.TwoDimensions)
            {
                baseDataVariableTypeState.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0, 0 });
            }

            parent?.AddChild(baseDataVariableTypeState);

            return baseDataVariableTypeState;
        }

        /// <summary>Creates a new DataVariable NodeClass.</summary>
        /// <param name="parent">The parent NodeState object the new DataVariable NodeClass will be created in.</param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="dataType">
        ///     The Expanded Node Id of the node used as data type of the new variable.
        /// </param>
        /// <param name="valueRank">
        ///     The value rank of the new variable, e.g. <see cref="ValueRanks.Scalar" />. See
        ///     <see cref="ValueRanks" /> for all possible value ranks.
        /// </param>
        /// <param name="accessLevel">
        ///     The access level of the new variable, e.g. <see cref="AccessLevels.CurrentRead" />. See
        ///     <see cref="AccessLevels" /> for all possible access levels.
        /// </param>
        /// <param name="initialValue">The initial value. If null a default value is used as initial value.</param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.</para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <returns>The created <see cref="BaseDataVariableState" /></returns>
        public BaseDataVariableState CreateBaseDataVariableState(NodeState parent, string browseName,
            LocalizedText displayName, LocalizedText description, ExpandedNodeId dataType, int valueRank,
            byte accessLevel, object initialValue, AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = new RolePermissionTypeCollection();
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = new RolePermissionTypeCollection();
            }

            var baseDataVariableTypeState = new BaseDataVariableState(parent) {
                SymbolicName = displayName.ToString(),
                ReferenceTypeId = ReferenceTypes.Organizes,
                TypeDefinitionId = VariableTypeIds.BaseDataVariableType,
                NodeId = new NodeId(browseName, NamespaceIndex),
                BrowseName = new QualifiedName(browseName, NamespaceIndex),
                DisplayName = displayName,
                Description = description,
                WriteMask = writeMask,
                UserWriteMask = userWriteMask,
                RolePermissions = rolePermissions,
                UserRolePermissions = userRolePermissions,
                DataType = (NodeId)dataType,
                ValueRank = valueRank,
                AccessLevel = accessLevel,
                UserAccessLevel = accessLevel,
                Historizing = false
            };

            baseDataVariableTypeState.Value = initialValue ?? GetNewValue(baseDataVariableTypeState);
            baseDataVariableTypeState.StatusCode = StatusCodes.Good;
            baseDataVariableTypeState.Timestamp = DateTime.UtcNow;

            if (valueRank == ValueRanks.OneDimension)
            {
                baseDataVariableTypeState.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0 });
            }
            else if (valueRank == ValueRanks.TwoDimensions)
            {
                baseDataVariableTypeState.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0, 0 });
            }

            parent?.AddChild(baseDataVariableTypeState);

            return baseDataVariableTypeState;
        }
        #endregion

        #region DataAccess Server Facet related Methods
        /// <summary>Creates a new DataItem variable.</summary>
        /// <param name="parent">The parent NodeState object the new folder will be created in.</param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="initialValue">The initial value. If null a default value is used as initial value.</param>
        /// <param name="dataType">
        ///     The data type of the new variable, e.g. <see cref="BuiltInType.SByte" />. See
        ///     <see cref="BuiltInType" /> for all possible types
        /// </param>
        /// <param name="valueRank">
        ///     The value rank of the new variable, e.g. <see cref="ValueRanks.Scalar" />. See
        ///     <see cref="ValueRanks" /> for all possible value ranks.
        /// </param>
        /// <param name="accessLevel">
        ///     The access level of the new variable, e.g. <see cref="AccessLevels.CurrentRead" />. See
        ///     <see cref="AccessLevels" /> for all possible access levels.
        /// </param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.<br /></para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="definition">Definition is a vendor-specific, human readable string that specifies how the value of this DataItem is calculated. Definition is non-localized and will often contain an equation that can be parsed by certain clients, e.g. Definition::= "(TempA � 25) + TempB� </param>
        /// <param name="valuePrecision">
        ///     <para>
        ///         The optional valuePrecision Specifies the maximum precision that the server can maintain for the item based on restrictions in the target
        ///         environment. If null is specified the property ValuePrecision will not be created.
        ///         The precision can be used for the following DataTypes:
        ///     </para>
        ///     <list type="bullet">
        ///         <item>For Float and Double values it specifies the number of digits after the decimal place.</item>
        ///         <item>
        ///             For DateTime values it indicates the minimum time difference in nanoseconds. E.g., a precision of
        ///             20000000 defines a precision of 20 milliseconds.
        ///         </item>
        ///     </list>
        /// </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <returns>The created <see cref="DataItemState" /></returns>
        public DataItemState CreateDataItemState(NodeState parent, string browseName,
            LocalizedText displayName,
            LocalizedText description, BuiltInType dataType, int valueRank, byte accessLevel, object initialValue,
            AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            string definition = null, double? valuePrecision = null,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = new RolePermissionTypeCollection();
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = new RolePermissionTypeCollection();
            }

            var variable = new DataItemState(parent);

            if (definition != null)
            {
                variable.Definition = new PropertyState<string>(variable);
            }

            if (valuePrecision != null)
            {
                variable.ValuePrecision = new PropertyState<double>(variable);
            }

            variable.Create(
                SystemContext,
                null,
                variable.BrowseName,
                null,
                true);

            variable.SymbolicName = displayName.ToString();
            variable.ReferenceTypeId = ReferenceTypes.Organizes;
            variable.TypeDefinitionId = VariableTypeIds.BaseDataVariableType;
            variable.NodeId = new NodeId(browseName, NamespaceIndex);
            variable.BrowseName = new QualifiedName(browseName, NamespaceIndex);
            variable.DisplayName = displayName;
            variable.Description = description;
            variable.WriteMask = writeMask;
            variable.UserWriteMask = userWriteMask;
            variable.RolePermissions = rolePermissions;
            variable.UserRolePermissions = userRolePermissions;
            variable.DataType = (uint)dataType;
            variable.ValueRank = valueRank;
            variable.AccessLevel = accessLevel;
            variable.UserAccessLevel = accessLevel;
            variable.Historizing = false;

            variable.Value = initialValue ??
                             Opc.Ua.TypeInfo.GetDefaultValue((uint)dataType, valueRank, ServerData.TypeTree);
            variable.StatusCode = StatusCodes.Good;
            variable.Timestamp = DateTime.UtcNow;

            switch (valueRank)
            {
                case ValueRanks.OneDimension:
                    variable.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0 });
                    break;
                case ValueRanks.TwoDimensions:
                    variable.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0, 0 });
                    break;
                default:
                    break;
            }

            if (definition != null)
            {
                variable.Definition.Value = definition;
                variable.ValuePrecision.AccessLevel = accessLevel;
                variable.ValuePrecision.UserAccessLevel = accessLevel;
            }

            if (valuePrecision != null)
            {
                variable.ValuePrecision.Value = (double)valuePrecision;
                variable.ValuePrecision.AccessLevel = accessLevel;
                variable.ValuePrecision.UserAccessLevel = accessLevel;
            }

            parent?.AddChild(variable);

            return variable;
        }

        /// <summary>Creates a new AnalogItem variable.</summary>
        /// <param name="parent">The parent NodeState object the new folder will be created in.</param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="dataType">
        ///     The data type of the new variable, e.g. <see cref="BuiltInType.SByte" />. See
        ///     <see cref="BuiltInType" /> for all possible types
        /// </param>
        /// <param name="valueRank">
        ///     The value rank of the new variable, e.g. <see cref="ValueRanks.Scalar" />. See
        ///     <see cref="ValueRanks" /> for all possible value ranks.
        /// </param>
        /// <param name="accessLevel">
        ///     The access level of the new variable, e.g. <see cref="AccessLevels.CurrentRead" />. See
        ///     <see cref="AccessLevels" /> for all possible access levels.
        /// </param>
        /// <param name="initialValue">The initial value. If null a default value is used as initial value.</param>
        /// <param name="euRange">
        ///     <para>
        ///         The engineering unit range defines the value <see cref="Range" /> likely to be obtained in normal operation. It
        ///         is intended for such use as automatically
        ///         scaling a bar graph display.
        ///     </para>
        ///     <para>
        ///         Sensor or instrument failure or deactivation can result in a returned item value which is actually outside
        ///         this range.
        ///     </para>
        /// </param>
        /// <param name="engineeringUnit">The optional engineering unit specifies the units for the item value</param>
        /// <param name="instrumentRange">
        ///     The optional instrument range defines the value <see cref="Range" /> that can be returned by the
        ///     instrument.
        /// </param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.<br /></para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="definition">Definition is a vendor-specific, human readable string that specifies how the value of this DataItem is calculated. Definition is non-localized and will often contain an equation that can be parsed by certain clients, e.g. Definition::= "(TempA � 25) + TempB� </param>
        /// <param name="valuePrecision">
        ///     <para>
        ///         The optional valuePrecision Specifies the maximum precision that the server can maintain for the item based on restrictions in the target
        ///         environment. If null is specified the property ValuePrecision will not be created.
        ///         The precision can be used for the following DataTypes:
        ///     </para>
        ///     <list type="bullet">
        ///         <item>For Float and Double values it specifies the number of digits after the decimal place.</item>
        ///         <item>
        ///             For DateTime values it indicates the minimum time difference in nanoseconds. E.g., a precision of
        ///             20000000 defines a precision of 20 milliseconds.
        ///         </item>
        ///     </list>
        /// </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <returns>The created <see cref="AnalogItemState" /></returns>
        public AnalogItemState CreateAnalogItemState(NodeState parent, string browseName,
            LocalizedText displayName,
            LocalizedText description, BuiltInType dataType, int valueRank, byte accessLevel, object initialValue,
            Range euRange, EUInformation engineeringUnit = null, Range instrumentRange = null,
            AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            string definition = null, double? valuePrecision = null,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null)
        {
            return CreateAnalogItemState(parent, browseName, displayName, description, (uint)dataType, valueRank,
                accessLevel, initialValue, euRange, engineeringUnit, instrumentRange, writeMask, userWriteMask,
                definition, valuePrecision, rolePermissions, userRolePermissions);
        }

        /// <summary>Creates a new AnalogItem variable.</summary>
        /// <param name="parent">The parent NodeState object the new folder will be created in.</param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="dataType">
        ///     The Node Id of the node used as data type of the new variable.
        /// </param>
        /// <param name="valueRank">
        ///     The value rank of the new variable, e.g. <see cref="ValueRanks.Scalar" />. See
        ///     <see cref="ValueRanks" /> for all possible value ranks.
        /// </param>
        /// <param name="accessLevel">
        ///     The access level of the new variable, e.g. <see cref="AccessLevels.CurrentRead" />. See
        ///     <see cref="AccessLevels" /> for all possible access levels.
        /// </param>
        /// <param name="initialValue">The initial value. If null a default value is used as initial value.</param>
        /// <param name="euRange">
        ///     <para>
        ///         The engineering unit range defines the value <see cref="Range" /> likely to be obtained in normal operation. It
        ///         is intended for such use as automatically
        ///         scaling a bar graph display.
        ///     </para>
        ///     <para>
        ///         Sensor or instrument failure or deactivation can result in a returned item value which is actually outside
        ///         this range.
        ///     </para>
        /// </param>
        /// <param name="engineeringUnit">The optional engineering unit specifies the units for the item value</param>
        /// <param name="instrumentRange">
        ///     The optional instrument range defines the value <see cref="Range" /> that can be returned by the
        ///     instrument.
        /// </param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.<br /></para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="definition">Definition is a vendor-specific, human readable string that specifies how the value of this DataItem is calculated. Definition is non-localized and will often contain an equation that can be parsed by certain clients, e.g. Definition::= "(TempA � 25) + TempB� </param>
        /// <param name="valuePrecision">
        ///     <para>
        ///         The optional valuePrecision Specifies the maximum precision that the server can maintain for the item based on restrictions in the target
        ///         environment. If null is specified the property ValuePrecision will not be created.
        ///         The precision can be used for the following DataTypes:
        ///     </para>
        ///     <list type="bullet">
        ///         <item>For Float and Double values it specifies the number of digits after the decimal place.</item>
        ///         <item>
        ///             For DateTime values it indicates the minimum time difference in nanoseconds. E.g., a precision of
        ///             20000000 defines a precision of 20 milliseconds.
        ///         </item>
        ///     </list>
        /// </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <returns>The created <see cref="AnalogItemState" /></returns>
        protected AnalogItemState CreateAnalogItemState(NodeState parent, string browseName,
            LocalizedText displayName,
            LocalizedText description, NodeId dataType, int valueRank, byte accessLevel, object initialValue,
            Range euRange, EUInformation engineeringUnit = null, Range instrumentRange = null,
            AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            string definition = null, double? valuePrecision = null,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = new RolePermissionTypeCollection();
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = new RolePermissionTypeCollection();
            }

            var variable = new AnalogItemState(parent) {
                BrowseName = new QualifiedName(browseName, NamespaceIndex)
            };

            if (engineeringUnit != null)
            {
                variable.EngineeringUnits = new PropertyState<EUInformation>(variable);
            }

            if (instrumentRange != null)
            {
                variable.InstrumentRange = new PropertyState<Range>(variable);
            }

            if (definition != null)
            {
                variable.Definition = new PropertyState<string>(variable);
            }

            if (valuePrecision != null)
            {
                variable.ValuePrecision = new PropertyState<double>(variable);
            }

            variable.Create(
                SystemContext,
                new NodeId(browseName, NamespaceIndex),
                variable.BrowseName,
                null,
                true);

            if (engineeringUnit != null)
            {
                variable.EngineeringUnits.Value = engineeringUnit;
                variable.EngineeringUnits.AccessLevel = accessLevel;
                variable.EngineeringUnits.UserAccessLevel = accessLevel;
            }

            if (instrumentRange != null)
            {
                variable.InstrumentRange.Value = instrumentRange;
                variable.InstrumentRange.AccessLevel = accessLevel;
                variable.InstrumentRange.UserAccessLevel = accessLevel;
            }

            if (definition != null)
            {
                variable.Definition.Value = definition;
                variable.ValuePrecision.AccessLevel = accessLevel;
                variable.ValuePrecision.UserAccessLevel = accessLevel;
            }

            if (valuePrecision != null)
            {
                variable.ValuePrecision.Value = (double)valuePrecision;
                variable.ValuePrecision.AccessLevel = accessLevel;
                variable.ValuePrecision.UserAccessLevel = accessLevel;
            }

            variable.NodeId = new NodeId(browseName, NamespaceIndex);
            variable.SymbolicName = displayName.ToString();
            variable.DisplayName = displayName;
            variable.Description = description;
            variable.WriteMask = writeMask;
            variable.UserWriteMask = userWriteMask;
            variable.RolePermissions = rolePermissions;
            variable.UserRolePermissions = userRolePermissions;
            variable.ReferenceTypeId = ReferenceTypes.Organizes;
            variable.DataType = dataType;
            variable.ValueRank = valueRank;
            variable.AccessLevel = accessLevel;
            variable.UserAccessLevel = accessLevel;
            variable.Historizing = false;

            if (valueRank == ValueRanks.OneDimension)
            {
                variable.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0 });
            }
            else if (valueRank == ValueRanks.TwoDimensions)
            {
                variable.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0, 0 });
            }

            variable.EURange.Value = euRange ?? new Range(100, 0);
            variable.EURange.AccessLevel = accessLevel;
            variable.EURange.UserAccessLevel = accessLevel;

            variable.Value = initialValue ?? Opc.Ua.TypeInfo.GetDefaultValue(dataType, valueRank, ServerData.TypeTree);

            variable.StatusCode = StatusCodes.Good;
            variable.Timestamp = DateTime.UtcNow;

            parent?.AddChild(variable);

            return variable;
        }

        /// <summary>Creates a new two state variable.</summary>
        /// <param name="parent">The parent NodeState object the new folder will be created in.</param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="accessLevel">
        ///     The access level of the new variable, e.g. <see cref="AccessLevels.CurrentRead" />. See
        ///     <see cref="AccessLevels" /> for all possible access levels.
        /// </param>
        /// <param name="initialValue">The initial value. If null a default value is used as initial value.</param>
        /// <param name="trueState">
        ///     Defines the string to be associated with this variable when it is TRUE. This is typically used for a contact when
        ///     it is in the closed (non-zero)
        ///     state.
        /// </param>
        /// <param name="falseState">
        ///     Defines the string to be associated with this variable when it is FALSE. This is typically
        ///     used for a contact when it is in the open(zero) state.
        /// </param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.<br /></para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="definition">Definition is a vendor-specific, human readable string that specifies how the value of this DataItem is calculated. Definition is non-localized and will often contain an equation that can be parsed by certain clients, e.g. Definition::= "(TempA � 25) + TempB� </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <returns>The created <see cref="TwoStateDiscreteState" /></returns>
        public TwoStateDiscreteState CreateTwoStateDiscreteState(NodeState parent, string browseName,
            LocalizedText displayName,
            LocalizedText description, byte accessLevel, bool initialValue, string trueState, string falseState,
            AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            string definition = null,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = new RolePermissionTypeCollection();
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = new RolePermissionTypeCollection();
            }

            var variable = new TwoStateDiscreteState(parent);

            if (definition != null)
            {
                variable.Definition = new PropertyState<string>(variable);
            }

            variable.Create(
                SystemContext,
                new NodeId(browseName, NamespaceIndex),
                new QualifiedName(browseName, NamespaceIndex),
                displayName,
                true);

            if (definition != null)
            {
                variable.Definition.Value = definition;
                variable.ValuePrecision.AccessLevel = accessLevel;
                variable.ValuePrecision.UserAccessLevel = accessLevel;
            }

            variable.SymbolicName = displayName.ToString();
            variable.ReferenceTypeId = ReferenceTypes.Organizes;
            variable.DataType = DataTypeIds.Boolean;
            variable.ValueRank = ValueRanks.Scalar;
            variable.Description = description;
            variable.WriteMask = writeMask;
            variable.UserWriteMask = userWriteMask;
            variable.RolePermissions = rolePermissions;
            variable.UserRolePermissions = userRolePermissions;
            variable.AccessLevel = accessLevel;
            variable.UserAccessLevel = accessLevel;
            variable.Historizing = false;
            variable.Value = initialValue;

            variable.StatusCode = StatusCodes.Good;
            variable.Timestamp = DateTime.UtcNow;

            variable.TrueState.Value = trueState;
            variable.TrueState.AccessLevel = accessLevel;
            variable.TrueState.UserAccessLevel = accessLevel;

            variable.FalseState.Value = falseState;
            variable.FalseState.AccessLevel = accessLevel;
            variable.FalseState.UserAccessLevel = accessLevel;

            parent?.AddChild(variable);

            return variable;
        }

        /// <summary>Creates a new multi state variable.</summary>
        /// <param name="parent">The parent NodeState object the new folder will be created in.</param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="initialValue">The initial value. If null a default value is used as initial value.</param>
        /// <param name="accessLevel">
        ///     The access level of the new variable, e.g. <see cref="AccessLevels.CurrentRead" />. See
        ///     <see cref="AccessLevels" /> for all possible access levels.
        /// </param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.<br /></para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="definition">Definition is a vendor-specific, human readable string that specifies how the value of this DataItem is calculated. Definition is non-localized and will often contain an equation that can be parsed by certain clients, e.g. Definition::= "(TempA � 25) + TempB� </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <param name="values">The possible values the multi-state variable can have.</param>
        /// <returns>The created <see cref="MultiStateDiscreteState" /></returns>
        public MultiStateDiscreteState CreateMultiStateDiscreteState(NodeState parent, string browseName,
            LocalizedText displayName,
            LocalizedText description, byte accessLevel, object initialValue,
            AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            string definition = null,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null, params LocalizedText[] values)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = new RolePermissionTypeCollection();
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = new RolePermissionTypeCollection();
            }

            var variable = new MultiStateDiscreteState(parent);

            if (definition != null)
            {
                variable.Definition = new PropertyState<string>(variable);
            }

            variable.Create(
                SystemContext,
                new NodeId(browseName, NamespaceIndex),
                new QualifiedName(browseName, NamespaceIndex),
                displayName,
                true);

            if (definition != null)
            {
                variable.Definition.Value = definition;
                variable.ValuePrecision.AccessLevel = accessLevel;
                variable.ValuePrecision.UserAccessLevel = accessLevel;
            }

            variable.SymbolicName = displayName.ToString();
            variable.ReferenceTypeId = ReferenceTypes.Organizes;
            variable.DataType = DataTypeIds.UInt32;
            variable.ValueRank = ValueRanks.Scalar;
            variable.Description = description;
            variable.WriteMask = writeMask;
            variable.UserWriteMask = userWriteMask;
            variable.RolePermissions = rolePermissions;
            variable.UserRolePermissions = userRolePermissions;
            variable.AccessLevel = accessLevel;
            variable.UserAccessLevel = accessLevel;
            variable.Historizing = false;

            variable.Value = initialValue ?? (uint)0;

            variable.StatusCode = StatusCodes.Good;
            variable.Timestamp = DateTime.UtcNow;

            var strings = new LocalizedText[values.Length];
            for (var ii = 0; ii < strings.Length; ii++)
            {
                strings[ii] = values[ii];
            }

            variable.EnumStrings.Value = strings;
            variable.EnumStrings.AccessLevel = accessLevel;
            variable.EnumStrings.UserAccessLevel = accessLevel;

            parent?.AddChild(variable);

            return variable;
        }

        /// <summary>Creates a new multi state value variable.</summary>
        /// <param name="parent">The parent NodeState object the new folder will be created in.</param>
        /// <param name="browseName">Nodes have a BrowseName Attribute that is used as a non-localized human-readable name when browsing the AddressSpace to create paths out of BrowseNames. The
        /// TranslateBrowsePathsToNodeIds Service defined in OPC 10000-4 can be used to follow a path constructed of BrowseNames, e.g. /Static/Simple Types</param>
        /// <param name="displayName">
        ///   <para>The DisplayName Attribute contains the localized name of the Node, e.g. Simple Types. Clients should use this Attribute if they want to display the name of
        /// the Node to the user. They should not use the BrowseName for this purpose.</para>
        ///   <para>The string part of the DisplayName is restricted to 512 characters.</para>
        /// </param>
        /// <param name="description">The optional Description Attribute shall explain the meaning of the Node in a localized text using the same mechanisms for localization as described for the
        /// DisplayName.</param>
        /// <param name="dataType">
        ///     The Node Id of the node used as data type of the new variable.
        /// </param>
        /// <param name="initialValue">The initial value. If null a default value is used as initial value.</param>
        /// <param name="accessLevel">
        ///     The access level of the new variable, e.g. <see cref="AccessLevels.CurrentRead" />. See
        ///     <see cref="AccessLevels" /> for all possible access levels.
        /// </param>
        /// <param name="writeMask">
        ///   <para>The optional WriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node. The WriteMask Attribute does not take any user
        /// access rights into account, that is, although an Attribute is writable this may be restricted to a certain user/user group.</para>
        ///   <para>If the OPC UA Server does not have the ability to get the WriteMask information for a specific Attribute from the underlying system, it should state that it
        /// is writable. If a write operation is called on the Attribute, the Server should transfer this request and return the corresponding StatusCode if such a request
        /// is rejected. StatusCodes are defined in OPC 10000-4.<br /></para>
        /// </param>
        /// <param name="userWriteMask">
        ///   <para>The optional UserWriteMask Attribute exposes the possibilities of a client to write the Attributes of the Node taking user access rights into account. It
        /// uses the AttributeWriteMask DataType which is defined in 0.</para>
        ///   <para>The UserWriteMask Attribute can only further restrict the WriteMask Attribute, when it is set to not writable in the general case that applies for every
        /// user.</para>
        ///   <para>Clients cannot assume an Attribute can be written based on the UserWriteMask Attribute.It is possible that the Server may return an access denied error due
        /// to some server specific change which was not reflected in the state of this Attribute at the time the Client accessed it.</para>
        /// </param>
        /// <param name="definition">Definition is a vendor-specific, human readable string that specifies how the value of this DataItem is calculated. Definition is non-localized and will often contain an equation that can be parsed by certain clients, e.g. Definition::= "(TempA � 25) + TempB� </param>
        /// <param name="rolePermissions">The optional RolePermissions Attribute specifies the Permissions that apply to a Node for all Roles which have access to the Node.</param>
        /// <param name="userRolePermissions">The optional UserRolePermissions Attribute specifies the Permissions that apply to a Node for all Roles granted to current Session.</param>
        /// <param name="enumNames">The possible values the multi-state variable can have.</param>
        /// <returns>The created <see cref="MultiStateDiscreteState" /></returns>
        protected MultiStateValueDiscreteState CreateMultiStateValueDiscreteState(NodeState parent,
            string browseName, LocalizedText displayName,
            LocalizedText description, NodeId dataType, byte accessLevel, object initialValue,
            AttributeWriteMask writeMask = AttributeWriteMask.None,
            AttributeWriteMask userWriteMask = AttributeWriteMask.None,
            string definition = null,
            RolePermissionTypeCollection rolePermissions = null,
            RolePermissionTypeCollection userRolePermissions = null, params LocalizedText[] enumNames)
        {
            if (displayName == null)
            {
                displayName = new LocalizedText("");
            }

            if (description == null)
            {
                description = new LocalizedText("");
            }

            if (rolePermissions == null)
            {
                rolePermissions = new RolePermissionTypeCollection();
            }

            if (userRolePermissions == null)
            {
                userRolePermissions = new RolePermissionTypeCollection();
            }

            var variable = new MultiStateValueDiscreteState(parent);

            if (definition != null)
            {
                variable.Definition = new PropertyState<string>(variable);
            }

            variable.Create(
                SystemContext,
                new NodeId(browseName, NamespaceIndex),
                new QualifiedName(browseName, NamespaceIndex),
                displayName,
                true);

            if (definition != null)
            {
                variable.Definition.Value = definition;
                variable.ValuePrecision.AccessLevel = accessLevel;
                variable.ValuePrecision.UserAccessLevel = accessLevel;
            }

            variable.SymbolicName = displayName.ToString();
            variable.ReferenceTypeId = ReferenceTypes.Organizes;
            variable.DataType = dataType ?? DataTypeIds.UInt32;
            variable.ValueRank = ValueRanks.Scalar;
            variable.Description = description;
            variable.WriteMask = writeMask;
            variable.UserWriteMask = userWriteMask;
            variable.RolePermissions = rolePermissions;
            variable.UserRolePermissions = userRolePermissions;
            variable.AccessLevel = accessLevel;
            variable.UserAccessLevel = accessLevel;
            variable.Historizing = false;

            variable.Value = initialValue ?? (uint)0;

            variable.StatusCode = StatusCodes.Good;
            variable.Timestamp = DateTime.UtcNow;

            // there are two enumerations for this type:
            // EnumStrings = the string representations for enumerated values
            // ValueAsText = the actual enumerated value

            // set the enumerated strings
            var strings = new LocalizedText[enumNames.Length];
            for (var ii = 0; ii < strings.Length; ii++)
            {
                strings[ii] = enumNames[ii];
            }

            // set the enumerated values
            if (enumNames != null)
            {
                var values = new EnumValueType[enumNames.Length];
                for (var ii = 0; ii < values.Length; ii++)
                {
                    values[ii] = new EnumValueType {
                        Value = ii,
                        Description = strings[ii],
                        DisplayName = strings[ii]
                    };
                }

                variable.EnumValues.Value = values;
            }

            variable.EnumValues.AccessLevel = accessLevel;
            variable.EnumValues.UserAccessLevel = accessLevel;
            variable.ValueAsText.Value = variable.EnumValues.Value[0].DisplayName;

            parent?.AddChild(variable);

            return variable;
        }
        #endregion

        #region Method related functions
        /// <summary>
        ///     <para>Creates a new method.</para>
        ///     <para>
        ///         Nodes of the type Method represent a method, that is, something that is called by a client and returns a
        ///         result.
        ///     </para>
        /// </summary>
        /// <param name="parent">The parent object the new method will be created in.</param>
        /// <param name="path">
        ///     The unique path name for the variable in the server's address space.
        /// </param>
        /// <param name="name">
        ///     The name of the new method, e.g. <font color="#A31515" size="2" face="Consolas">Method1</font>
        /// </param>
        /// <param name="callingMethod">The method which will be called if the method is executed.</param>
        /// <returns>The created method object.</returns>
        public MethodState CreateMethodState(NodeState parent, string path, string name,
            GenericMethodCalledEventHandler2 callingMethod = null)
        {
            var method = new MethodState(parent) {
                SymbolicName = name,
                ReferenceTypeId = ReferenceTypeIds.HasComponent,
                NodeId = new NodeId(path, NamespaceIndex),
                BrowseName = new QualifiedName(path, NamespaceIndex),
                DisplayName = new LocalizedText("en", name),
                WriteMask = AttributeWriteMask.None,
                UserWriteMask = AttributeWriteMask.None,
                Executable = true,
                UserExecutable = true
            };

            parent?.AddChild(method);

            if (callingMethod != null)
            {
                method.OnCallMethod2 = callingMethod;
            }

            return method;
        }

        /// <summary>Creates a new argument.</summary>
        /// <param name="name">
        ///     The name of the new argument, e.g.
        ///     <font color="#A31515" size="2" face="Consolas">Initial State</font>
        /// </param>
        /// <param name="description">
        ///     The description of the new argument, e.g.
        ///     <font color="#A31515" size="2" face="Consolas">The initialize state for the process.</font>
        /// </param>
        /// <param name="dataType">
        ///     The data type of the new argument, e.g. <see cref="BuiltInType.SByte" />. See
        ///     <see cref="BuiltInType" /> for all possible types
        /// </param>
        /// <param name="valueRank">
        ///     The value rank of the new argument, e.g. <see cref="ValueRanks.Scalar" />. See
        ///     <see cref="ValueRanks" /> for all possible value ranks.
        /// </param>
        /// <returns>The created argument</returns>
        public Argument CreateArgument(string name, string description, BuiltInType dataType, int valueRank)
        {
            var argument = new Argument { Name = name, Description = description, DataType = (uint)dataType, ValueRank = valueRank };

            return argument;
        }

        /// <summary>Adds the input arguments to a method.</summary>
        /// <param name="parent">The method object.</param>
        /// <param name="inputArguments">The input arguments.</param>
        /// <returns>A <see cref="StatusCode" /> code with the result of the operation.</returns>
        public StatusCode AddInputArguments(MethodState parent, Argument[] inputArguments)
        {
            if (parent != null)
            {
                parent.InputArguments = new PropertyState<Argument[]>(parent) {
                    NodeId = new NodeId(parent.BrowseName.Name + "InArgs", NamespaceIndex),
                    BrowseName = BrowseNames.InputArguments
                };
                parent.InputArguments.DisplayName = parent.InputArguments.BrowseName.Name;
                parent.InputArguments.TypeDefinitionId = VariableTypeIds.PropertyType;
                parent.InputArguments.ReferenceTypeId = ReferenceTypeIds.HasProperty;
                parent.InputArguments.DataType = DataTypeIds.Argument;
                parent.InputArguments.ValueRank = ValueRanks.OneDimension;
                parent.InputArguments.Value = inputArguments;

                return StatusCodes.Good;
            }

            return StatusCodes.Bad;
        }

        /// <summary>Adds the output arguments to a method.</summary>
        /// <param name="parent">The method object.</param>
        /// <param name="outputArguments">The output arguments.</param>
        /// <returns>A <see cref="StatusCode" /> code with the result of the operation.</returns>
        public StatusCode AddOutputArguments(MethodState parent, params Argument[] outputArguments)
        {
            if (parent != null)
            {
                parent.OutputArguments = new PropertyState<Argument[]>(parent) {
                    NodeId = new NodeId(parent.BrowseName.Name + "OutArgs", NamespaceIndex),
                    BrowseName = BrowseNames.OutputArguments
                };
                parent.OutputArguments.DisplayName = parent.OutputArguments.BrowseName.Name;
                parent.OutputArguments.TypeDefinitionId = VariableTypeIds.PropertyType;
                parent.OutputArguments.ReferenceTypeId = ReferenceTypeIds.HasProperty;
                parent.OutputArguments.DataType = DataTypeIds.Argument;
                parent.OutputArguments.ValueRank = ValueRanks.OneDimension;
                parent.OutputArguments.Value = outputArguments;

                return StatusCodes.Good;
            }

            return StatusCodes.Bad;
        }
        #endregion

        #region Random value generator
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="boundaryValueFrequency"></param>
        protected void ResetRandomGenerator(int seed, int boundaryValueFrequency = 0)
        {
            randomSource_ = new RandomSource(seed);
            generator_ = new DataGenerator(randomSource_) {
                BoundaryValueFrequency = boundaryValueFrequency
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        protected object GetNewValue(BaseVariableState variable)
        {
            Debug.Assert(generator_ != null, "Need a random generator!");

            object value = null;
            var retryCount = 0;

            while (value == null && retryCount < 10)
            {
                value = generator_.GetRandom(variable.DataType, variable.ValueRank, new uint[] { 10 }, ServerData.TypeTree);
                // skip Variant Null
                if (value is Variant variant)
                {
                    if (variant.Value == null)
                    {
                        value = null;
                    }
                }
                retryCount++;
            }

            return value;
        }
        #endregion
        #region Private Fields
        private readonly object m_lock = new object();
        private IUaServerData m_server;
        private UaServerContext m_systemContext;
        private IReadOnlyList<string> m_namespaceUris;
        private IReadOnlyList<ushort> m_namespaceIndexes;
        private NodeIdDictionary<UaMonitoredNode> m_monitoredNodes;
        private NodeIdDictionary<CacheEntry> m_componentCache;
        private NodeIdDictionary<NodeState> m_predefinedNodes;
        private List<NodeState> m_rootNotifiers;
        private uint m_maxQueueSize;
        private uint m_maxDurableQueueSize;
        private string m_aliasRoot;
        private RandomSource randomSource_;
        private DataGenerator generator_;
        #endregion
    }
}
