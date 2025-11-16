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
using Opc.Ua;
#endregion Using Directives

namespace SampleCompany.NodeManagers.TestData
{
    public partial class TestSystemConditionState
    {
        #region Initialization
        /// <summary>
        /// Initializes the object as a collection of counters which change value on read.
        /// </summary>
        protected override void OnAfterCreate(ISystemContext context, NodeState node)
        {
            base.OnAfterCreate(context, node);
            MonitoredNodeCount.OnSimpleReadValue = OnReadMonitoredNodeCount;
        }
        #endregion Initialization

        #region Protected Methods
        /// <summary>
        /// Reads the value for the MonitoredNodeCount.
        /// </summary>
        protected virtual ServiceResult OnReadMonitoredNodeCount(
            ISystemContext context,
            NodeState node,
            ref object value)
        {
            if (context?.SystemHandle is not TestDataSystem system)
            {
                return StatusCodes.BadOutOfService;
            }

            value = system.MonitoredNodeCount;
            return ServiceResult.Good;
        }
        #endregion Protected Methods
    }
}
