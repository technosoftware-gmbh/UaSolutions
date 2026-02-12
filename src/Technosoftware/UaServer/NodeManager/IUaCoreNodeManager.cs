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
using System.Collections.Generic;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// The default node manager for the server.
    /// </summary>
    /// <remarks>
    /// Every Server has one instance of this NodeManager.
    /// It stores objects that implement ILocalNode and indexes them by NodeId.
    /// </remarks>
    public interface IUaCoreNodeManager : IUaNodeManager
    {
        /// <summary>
        /// Imports the nodes from a dictionary of NodeState objects.
        /// </summary>
        void ImportNodes(ISystemContext context, IEnumerable<NodeState> predefinedNodes);

        /// <summary>
        /// Imports the nodes from a dictionary of NodeState objects.
        /// </summary>
        void ImportNodes(
            ISystemContext context,
            IEnumerable<NodeState> predefinedNodes,
            bool isInternal);
    }
}
