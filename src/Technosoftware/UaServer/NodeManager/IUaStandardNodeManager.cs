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
using System.Threading;
using System.Threading.Tasks;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// An interface to an object that manages a set of nodes in the address space.
    /// </summary>
    public interface IUaStandardNodeManager : IUaNodeManager
    {
        /// <summary>
        /// Called when the session is closed.
        /// </summary>
        void SessionClosing(UaServerOperationContext context, NodeId sessionId, bool deleteSubscriptions);

        /// <summary>
        /// Returns true if the node is in the view.
        /// </summary>
        bool IsNodeInView(UaServerOperationContext context, NodeId viewId, object nodeHandle);

        /// <summary>
        /// Returns the metadata needed for validating permissions, associated with the node with
        /// the option to optimize services by using a cache.
        /// </summary>
        /// <remarks>
        /// Returns null if the node does not exist.
        /// It should return null in case the implementation wishes to handover the task to the parent IUaNodeManager.GetNodeMetadata
        /// </remarks>
        UaNodeMetadata GetPermissionMetadata(
            UaServerOperationContext context,
            object targetHandle,
            BrowseResultMask resultMask,
            Dictionary<NodeId, List<object>> uniqueNodesServiceAttributesCache,
            bool permissionsOnly);
    }
}
