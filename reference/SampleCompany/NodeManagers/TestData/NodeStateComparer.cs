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
using System.Collections.Generic;
using Opc.Ua;
#endregion Using Directives

namespace SampleCompany.NodeManagers.TestData
{
    /// <summary>
    /// Helper which implements IEqualityComparer for Linq queries.
    /// </summary>
    public class NodeStateComparer : IEqualityComparer<NodeState>
    {
        /// <inheritdoc/>
        public bool Equals(NodeState x, NodeState y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return x.NodeId == y.NodeId;
        }

        /// <inheritdoc/>
        public int GetHashCode(NodeState obj)
        {
            if (obj is null)
            {
                return 0;
            }

            return obj.NodeId.GetHashCode();
        }
    }
}
