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
using System.Collections.Generic;
using System.Globalization;
using Opc.Ua;
using Range = Opc.Ua.Range;
#endregion Using Directives

namespace SampleCompany.NodeManagers.TestData
{
    public partial class TestDataObjectState
    {
        /// <summary>
        /// Initializes the object as a collection of counters which change value on read.
        /// </summary>
        protected override void OnAfterCreate(ISystemContext context, NodeState node)
        {
            base.OnAfterCreate(context, node);

            GenerateValues.OnCall = OnGenerateValues;
        }

        /// <summary>
        /// Initialzies the variable as a counter.
        /// </summary>
        protected void InitializeVariable(
            ISystemContext context,
            BaseVariableState variable,
            uint numericId)
        {
            variable.NumericId = numericId;

            // provide an implementation that produces a random value on each read.
            if (SimulationActive.Value)
            {
                variable.OnReadValue = DoDeviceRead;
            }

            // set a valid initial value.

            if (context.SystemHandle is TestDataSystem system)
            {
                GenerateValue(system, variable);
            }

            // allow writes if the simulation is not active.
            if (!SimulationActive.Value)
            {
                variable.AccessLevel = variable.UserAccessLevel = AccessLevels.CurrentReadOrWrite;

                var children = new List<BaseInstanceState>();
                variable.GetChildren(context, children);
                foreach (BaseInstanceState child in children)
                {
                    if (child is BaseVariableState variableChild)
                    {
                        variableChild.AccessLevel = variableChild.UserAccessLevel = AccessLevels
                            .CurrentReadOrWrite;
                    }
                }
            }

            // set the EU range.

            if (variable.FindChild(
                context,
                Opc.Ua.BrowseNames.EURange) is BaseVariableState euRange)
            {
                if (context.TypeTable.IsTypeOf(variable.DataType, Opc.Ua.DataTypeIds.UInteger))
                {
                    euRange.Value = new Range(250, 50);
                }
                else
                {
                    euRange.Value = new Range(100, -100);
                }
                variable.OnSimpleWriteValue = OnWriteAnalogValue;
            }
        }

        /// <summary>
        /// Validates a written value.
        /// </summary>
        public ServiceResult OnWriteAnalogValue(
            ISystemContext context,
            NodeState node,
            ref object value)
        {
            try
            {
                if (node.FindChild(
                    context,
                    Opc.Ua.BrowseNames.EURange) is not BaseVariableState euRange)
                {
                    return ServiceResult.Good;
                }

                if (euRange.Value is not Range range)
                {
                    return ServiceResult.Good;
                }

                if (value is Array array)
                {
                    for (int ii = 0; ii < array.Length; ii++)
                    {
                        object element = array.GetValue(ii);

                        if (typeof(Variant).IsInstanceOfType(element))
                        {
                            element = ((Variant)element).Value;
                        }

                        double elementNumber = Convert.ToDouble(
                            element,
                            CultureInfo.InvariantCulture);

                        if (elementNumber > range.High || elementNumber < range.Low)
                        {
                            return StatusCodes.BadOutOfRange;
                        }
                    }

                    return ServiceResult.Good;
                }

                double number = Convert.ToDouble(value, CultureInfo.InvariantCulture);

                if (number > range.High || number < range.Low)
                {
                    return StatusCodes.BadOutOfRange;
                }

                return ServiceResult.Good;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Generates a new value for the variable.
        /// </summary>
        protected void GenerateValue(TestDataSystem system, BaseVariableState variable)
        {
            variable.Value = system.ReadValue(variable);
            variable.Timestamp = DateTime.UtcNow;
            variable.StatusCode = StatusCodes.Good;
        }

        /// <summary>
        /// Handles the generate values method.
        /// </summary>
        protected virtual ServiceResult OnGenerateValues(
            ISystemContext context,
            MethodState method,
            NodeId objectId,
            uint count)
        {
            ClearChangeMasks(context, true);

            if (AreEventsMonitored)
            {
                var e = new GenerateValuesEventState(null);

                var message = new TranslationInfo(
                    "GenerateValuesEventType",
                    "en-US",
                    "New values generated for test source '{0}'.",
                    DisplayName);

                e.Initialize(context, this, EventSeverity.MediumLow, new LocalizedText(message));

                e.Iterations = new PropertyState<uint>(e) { Value = count };

                e.NewValueCount = new PropertyState<uint>(e) { Value = 10 };

                ReportEvent(context, e);
            }

#if CONDITION_SAMPLES
            this.CycleComplete.RequestAcknowledgement(context, (ushort)EventSeverity.Low);
#endif

            return ServiceResult.Good;
        }

        /// <summary>
        /// Generates a new value each time the value is read.
        /// </summary>
        private ServiceResult DoDeviceRead(
            ISystemContext context,
            NodeState node,
            NumericRange indexRange,
            QualifiedName dataEncoding,
            ref object value,
            ref StatusCode statusCode,
            ref DateTime timestamp)
        {
            if (node is not BaseVariableState variable)
            {
                return ServiceResult.Good;
            }

            if (!SimulationActive.Value)
            {
                return ServiceResult.Good;
            }

            if (context.SystemHandle is not TestDataSystem system)
            {
                return StatusCodes.BadOutOfService;
            }

            try
            {
                value = system.ReadValue(variable);

                statusCode = StatusCodes.Good;
                timestamp = DateTime.UtcNow;

                ServiceResult error = BaseVariableState.ApplyIndexRangeAndDataEncoding(
                    context,
                    indexRange,
                    dataEncoding,
                    ref value);

                if (ServiceResult.IsBad(error))
                {
                    statusCode = error.StatusCode;
                }

                return ServiceResult.Good;
            }
            catch (Exception e)
            {
                return new ServiceResult(e);
            }
        }
    }
}
