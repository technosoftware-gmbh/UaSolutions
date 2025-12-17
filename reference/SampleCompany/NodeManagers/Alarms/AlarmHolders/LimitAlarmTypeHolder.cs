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
using Microsoft.Extensions.Logging;
using Opc.Ua;
#endregion Using Directives

namespace SampleCompany.NodeManagers.Alarms
{
    public abstract class LimitAlarmTypeHolder : AlarmConditionTypeHolder
    {
        protected LimitAlarmTypeHolder(
            ILogger logger,
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
                logger,
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
                Initialize(ObjectTypes.LimitAlarmType, name, maxShelveTime);
            }
        }

        public new void Initialize(
            uint alarmTypeIdentifier,
            string name,
            double maxTimeShelved = AlarmDefines.NORMAL_MAX_TIME_SHELVED)
        {
            // Create an alarm and trigger name - Create a base method for creating the trigger, just provide the name

            m_alarm ??= new LimitAlarmState(m_parent);

            LimitAlarmState alarm = GetAlarm();

            alarm.HighLimit ??= new PropertyState<double>(alarm);
            alarm.HighHighLimit ??= new PropertyState<double>(alarm);
            alarm.LowLimit ??= new PropertyState<double>(alarm);
            alarm.LowLowLimit ??= new PropertyState<double>(alarm);

            if (Optional)
            {
                alarm.BaseHighLimit = new PropertyState<double>(alarm);
                alarm.BaseHighHighLimit = new PropertyState<double>(alarm);
                alarm.BaseLowLimit = new PropertyState<double>(alarm);
                alarm.BaseLowLowLimit = new PropertyState<double>(alarm);
            }

            // Call the base class to set parameters
            base.Initialize(alarmTypeIdentifier, name, maxTimeShelved);

            alarm.HighLimit.Value = AlarmDefines.HIGH_ALARM;
            alarm.HighHighLimit.Value = AlarmDefines.HIGHHIGH_ALARM;
            alarm.LowLimit.Value = AlarmDefines.LOW_ALARM;
            alarm.LowLowLimit.Value = AlarmDefines.LOWLOW_ALARM;

            if (Optional)
            {
                alarm.BaseHighLimit.Value = AlarmDefines.HIGH_ALARM;
                alarm.BaseHighHighLimit.Value = AlarmDefines.HIGHHIGH_ALARM;
                alarm.BaseLowLimit.Value = AlarmDefines.LOW_ALARM;
                alarm.BaseLowLowLimit.Value = AlarmDefines.LOWLOW_ALARM;
            }
            else
            {
                alarm.BaseHighHighLimit = null;
                alarm.BaseLowLimit = null;
                alarm.BaseLowLowLimit = null;
            }
        }

        private LimitAlarmState GetAlarm()
        {
            return (LimitAlarmState)m_alarm;
        }
    }
}
