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

namespace SampleCompany.NodeManagers.Boiler
{
    public partial class BoilerStateMachineState
    {
        /// <summary>
        /// Initializes the object as a collection of counters which change value on read.
        /// </summary>
        protected override void OnAfterCreate(ISystemContext context, NodeState node)
        {
            base.OnAfterCreate(context, node);

            Start.OnCallMethod = OnStart;
            Start.OnReadExecutable = IsStartExecutable;
            Start.OnReadUserExecutable = IsStartUserExecutable;

            Suspend.OnCallMethod = OnSuspend;
            Suspend.OnReadExecutable = IsSuspendExecutable;
            Suspend.OnReadUserExecutable = IsSuspendUserExecutable;

            Resume.OnCallMethod = OnResume;
            Resume.OnReadExecutable = IsResumeExecutable;
            Resume.OnReadUserExecutable = IsResumeUserExecutable;

            Halt.OnCallMethod = OnHalt;
            Halt.OnReadExecutable = IsHaltExecutable;
            Halt.OnReadUserExecutable = IsHaltUserExecutable;

            Reset.OnCallMethod = OnReset;
            Reset.OnReadExecutable = IsResetExecutable;
            Reset.OnReadUserExecutable = IsResetUserExecutable;
        }
    }
}
