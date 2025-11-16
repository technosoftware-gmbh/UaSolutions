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
using Opc.Ua;
#endregion Using Directives

#pragma warning disable CS0219

namespace SampleCompany.NodeManagers.Alarms
{
    internal class ExclusiveLimitHolder : LimitAlarmTypeHolder
    {
        public ExclusiveLimitHolder(
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
                Initialize(ObjectTypes.ExclusiveLimitAlarmType, name, maxShelveTime);
            }
        }

        public new void Initialize(
            uint alarmTypeIdentifier,
            string name,
            double maxTimeShelved = AlarmDefines.NORMAL_MAX_TIME_SHELVED)
        {
            // Create an alarm and trigger name - Create a base method for creating the trigger, just provide the name

            m_alarm ??= new ExclusiveLimitAlarmState(m_parent);

            ExclusiveLimitAlarmState alarm = GetAlarm();

            // Call the base class to set parameters
            base.Initialize(alarmTypeIdentifier, name, maxTimeShelved);

            alarm.SetLimitState(SystemContext, LimitAlarmStates.Inactive);
        }

        public override void SetValue(string message = "")
        {
            ExclusiveLimitAlarmState alarm = GetAlarm();
            int newSeverity = GetSeverity();
            int currentSeverity = alarm.Severity.Value;

            if (newSeverity != currentSeverity)
            {
                LimitAlarmStates state = LimitAlarmStates.Inactive;

                if (newSeverity == AlarmDefines.HIGHHIGH_SEVERITY)
                {
                    state = LimitAlarmStates.HighHigh;
                }
                else if (newSeverity == AlarmDefines.HIGH_SEVERITY)
                {
                    state = LimitAlarmStates.High;
                }
                else if (newSeverity == AlarmDefines.LOW_SEVERITY)
                {
                    state = LimitAlarmStates.Low;
                }
                else if (newSeverity == AlarmDefines.LOWLOW_SEVERITY)
                {
                    state = LimitAlarmStates.LowLow;
                }

                alarm.SetLimitState(SystemContext, state);
            }

            base.SetValue(message);
        }

        private ExclusiveLimitAlarmState GetAlarm()
        {
            return (ExclusiveLimitAlarmState)m_alarm;
        }
    }
}
