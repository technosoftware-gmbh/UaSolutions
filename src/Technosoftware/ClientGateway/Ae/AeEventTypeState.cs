#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
// Web: http://www.technosoftware.com
//
// The Software is based on the OPC Foundation’s software and is subject to 
// the OPC Foundation MIT License 1.00, which can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//
// The Software is subject to the Technosoftware GmbH Software License Agreement,
// which can be found here:
// https://technosoftware.com/license-agreement/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved


#region Using Directives

using System;

using Opc.Ua;

using Technosoftware.Common;
using Technosoftware.Rcw;

#endregion Using Directives

namespace Technosoftware.ClientGateway.Ae
{
    /// <summary>
    /// Stores information about an AE event type in the server address space.
    /// </summary>
    internal class AeEventTypeState : BaseObjectTypeState
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AeEventTypeState"/> class.
        /// </summary>
        public AeEventTypeState(EventType eventType, ushort namespaceIndex)
        {
            m_eventType = eventType;

            // create the name for the event type.
            string name = eventType.Description;

            if (!String.IsNullOrEmpty(eventType.ConditionName))
            {
                name = eventType.ConditionName;
            }

            if (!name.EndsWith("Type"))
            {
                if (eventType.EventTypeId == Technosoftware.Rcw.Constants.CONDITION_EVENT)
                {
                    name += "AlarmType";
                }
                else
                {
                    name += "EventType";
                }
            }

            // the attributes.
            this.NodeId = AeParsedNodeId.Construct(eventType, null, namespaceIndex);
            this.BrowseName = new QualifiedName(name, namespaceIndex);
            this.DisplayName = eventType.Description;
            this.IsAbstract = false;
            this.SuperTypeId = AeParsedNodeId.Construct(eventType.EventTypeMapping, namespaceIndex);

            // add the attributes as properties.
            if (eventType.Attributes != null)
            {
                for (int ii = 0; ii < eventType.Attributes.Count; ii++)
                {
                    string propertyName = eventType.Attributes[ii].Description;

                    if (AeTypeCache.IsKnownName(propertyName, "ACK COMMENT"))
                    {
                        continue;
                    }

                    PropertyState property = new PropertyState(this);

                    property.SymbolicName = propertyName;
                    property.ReferenceTypeId = Opc.Ua.ReferenceTypeIds.HasProperty;
                    property.TypeDefinitionId = Opc.Ua.VariableTypeIds.PropertyType;
                    property.ModellingRuleId = Opc.Ua.ObjectIds.ModellingRule_Optional;
                    property.NodeId = AeParsedNodeId.Construct(eventType, propertyName, namespaceIndex);
                    property.BrowseName = new QualifiedName(propertyName, namespaceIndex);
                    property.DisplayName = property.BrowseName.Name;
                    property.AccessLevel = AccessLevels.None;
                    property.UserAccessLevel = AccessLevels.None;
                    property.MinimumSamplingInterval = MinimumSamplingIntervals.Indeterminate;
                    property.Historizing = false;

                    bool isArray = false;
                    property.DataType = ComUtils.GetDataTypeId(eventType.Attributes[ii].VarType, out isArray);
                    property.ValueRank = (isArray) ? ValueRanks.OneDimension : ValueRanks.Scalar;

                    this.AddChild(property);
                }
            }
        }
        #endregion Constructors

        #region Public Members
        /// <summary>
        /// Gets the event type metadata.
        /// </summary>
        public EventType EventType
        {
            get { return m_eventType; }
        }
        #endregion Public Members

        BaseEventState ConstructEvent(ONEVENTSTRUCT e)
        {
            return null;
        }

        #region Private Fields
        private EventType m_eventType;
        #endregion Private Fields
    }

    /// <summary>
    /// Stores information about an abstract event type that groups AE events in the type hierarchy.
    /// </summary>
    internal class AeEventTypeMappingState : BaseObjectTypeState
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AeEventTypeMappingState"/> class.
        /// </summary>
        public AeEventTypeMappingState(EventTypeMapping eventType, ushort namespaceIndex)
        {
            m_eventType = eventType;

            // create the name for the event type.
            string name = "COMAE" + eventType.ToString();

            // the attributes.
            this.NodeId = AeParsedNodeId.Construct(eventType, namespaceIndex);
            this.BrowseName = new QualifiedName(name, namespaceIndex);
            this.DisplayName = this.BrowseName.Name;
            this.IsAbstract = true;

            // set the supertype.
            switch (eventType)
            {
                case EventTypeMapping.AlarmConditionType:
                { SuperTypeId = Opc.Ua.ObjectTypeIds.AlarmConditionType; break; }
                case EventTypeMapping.AuditEventType:
                { SuperTypeId = Opc.Ua.ObjectTypeIds.AuditEventType; break; }
                case EventTypeMapping.BaseEventType:
                { SuperTypeId = Opc.Ua.ObjectTypeIds.BaseEventType; break; }
                case EventTypeMapping.DeviceFailureEventType:
                { SuperTypeId = Opc.Ua.ObjectTypeIds.DeviceFailureEventType; break; }
                case EventTypeMapping.DiscreteAlarmType:
                { SuperTypeId = Opc.Ua.ObjectTypeIds.DiscreteAlarmType; break; }
                case EventTypeMapping.NonExclusiveDeviationAlarmType:
                { SuperTypeId = Opc.Ua.ObjectTypeIds.NonExclusiveDeviationAlarmType; break; }
                case EventTypeMapping.ExclusiveLevelAlarmType:
                { SuperTypeId = Opc.Ua.ObjectTypeIds.ExclusiveLevelAlarmType; break; }
                case EventTypeMapping.LimitAlarmType:
                { SuperTypeId = Opc.Ua.ObjectTypeIds.LimitAlarmType; break; }
                case EventTypeMapping.NonExclusiveLevelAlarmType:
                { SuperTypeId = Opc.Ua.ObjectTypeIds.NonExclusiveLevelAlarmType; break; }
                case EventTypeMapping.OffNormalAlarmType:
                { SuperTypeId = Opc.Ua.ObjectTypeIds.OffNormalAlarmType; break; }
                case EventTypeMapping.SystemEventType:
                { SuperTypeId = Opc.Ua.ObjectTypeIds.SystemEventType; break; }
                case EventTypeMapping.TripAlarmType:
                { SuperTypeId = Opc.Ua.ObjectTypeIds.TripAlarmType; break; }
                case EventTypeMapping.ConditionClassType:
                { SuperTypeId = Opc.Ua.ObjectTypeIds.BaseConditionClassType; break; }
            }
        }
        #endregion Constructors

        #region Public Members
        /// <summary>
        /// Gets the event type metadata.
        /// </summary>
        public EventTypeMapping EventType
        {
            get { return m_eventType; }
        }
        #endregion Public Members

        #region Private Fields
        private EventTypeMapping m_eventType;
        #endregion Private Fields
    }
}
