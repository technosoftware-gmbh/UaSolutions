#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using Microsoft.Extensions.Logging;
using Opc.Ua;
#endregion Using Directives

namespace SampleCompany.NodeManagers.Alarms
{
    public class NonExclusiveLevelHolder : NonExclusiveLimitHolder
    {
        public NonExclusiveLevelHolder(
            AlarmNodeManager alarmNodeManager,
            FolderState parent,
            SourceController trigger,
            string name,
            SupportedAlarmConditionType alarmConditionType,
            Type controllerType,
            int interval,
            bool optional = true,
            double maxShelveTime = AlarmDefines.NORMAL_MAX_TIME_SHELVED,
            bool create = true)
            : base(
                alarmNodeManager.ServerData.Telemetry.CreateLogger<NonExclusiveLevelHolder>(),
                alarmNodeManager,
                parent,
                trigger,
                name,
                alarmConditionType,
                controllerType,
                interval,
                optional,
                maxShelveTime,
                false)
        {
            if (create)
            {
                Initialize(ObjectTypes.NonExclusiveLevelAlarmType, name, maxShelveTime);
            }
        }

        public new void Initialize(
            uint alarmTypeIdentifier,
            string name,
            double maxTimeShelved = AlarmDefines.NORMAL_MAX_TIME_SHELVED)
        {
            // Create an alarm and trigger name - Create a base method for creating the trigger, just provide the name

            m_alarm ??= new NonExclusiveLevelAlarmState(m_parent);

            // Call the base class to set parameters
            base.Initialize(alarmTypeIdentifier, name, maxTimeShelved);
            m_logger.LogTrace(
                "NonExclusiveLevelHolder alarm typedefinition {TypeDefinitionId}",
                m_alarm.TypeDefinitionId);
        }

        public override void SetBranching(bool value)
        {
            m_supportsBranching = value;
        }
    }
}
