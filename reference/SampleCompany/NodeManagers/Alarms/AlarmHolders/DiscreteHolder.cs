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
using System.Globalization;
using Opc.Ua;
#endregion Using Directives

namespace SampleCompany.NodeManagers.Alarms
{
    public class DiscreteHolder : AlarmConditionTypeHolder
    {
        public DiscreteHolder(
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
            Utils.LogTrace("{0} Discrete Constructor Optional = {1}", name, optional);
            if (create)
            {
                Initialize(ObjectTypes.DiscreteAlarmType, name, maxShelveTime);
            }
        }

        public new void Initialize(
            uint alarmTypeIdentifier,
            string name,
            double maxTimeShelved = AlarmDefines.NORMAL_MAX_TIME_SHELVED)
        {
            m_analog = false;

            m_alarm ??= new DiscreteAlarmState(m_parent);

            // Call the base class to set parameters
            base.Initialize(alarmTypeIdentifier, name, maxTimeShelved);
        }

        #region Overrides
        public override void SetValue(string message = "")
        {
            bool active = m_alarmController.IsBooleanActive();
            int value = m_alarmController.GetValue();

            if (message.Length == 0)
            {
                message =
                    "Discrete Alarm analog value = " +
                    value.ToString(CultureInfo.InvariantCulture) +
                    ", active = " +
                    active.ToString();
            }

            base.SetValue(message);
        }
        #endregion Overrides
    }
}
