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
using System;
using System.Collections.Generic;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Calculates the value of an aggregate.
    /// </summary>
    public class StartEndAggregateCalculator : AggregateCalculator
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Initializes the aggregate calculator.
        /// </summary>
        /// <param name="aggregateId">The aggregate function to apply.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="processingInterval">The processing interval.</param>
        /// <param name="stepped">Whether to use stepped interpolation.</param>
        /// <param name="configuration">The aggregate configuration.</param>
        /// <param name="telemetry">The telemetry context to use to create obvservability instruments</param>
        public StartEndAggregateCalculator(
            NodeId aggregateId,
            DateTime startTime,
            DateTime endTime,
            double processingInterval,
            bool stepped,
            AggregateConfiguration configuration,
            ITelemetryContext telemetry)
            : base(aggregateId, startTime, endTime, processingInterval, stepped, configuration, telemetry)
        {
            SetPartialBit = true;
        }
        #endregion Constructors, Destructor, Initialization

        #region Overridden Methods
        /// <summary>
        /// Computes the value for the timeslice.
        /// </summary>
        protected override DataValue ComputeValue(TimeSlice slice)
        {
            uint? id = AggregateId.Identifier as uint?;

            if (id == null)
            {
                return base.ComputeValue(slice);
            }
            switch (id.Value)
            {
                case Objects.AggregateFunction_Start:
                    return ComputeStartEnd(slice, false);
                case Objects.AggregateFunction_End:
                    return ComputeStartEnd(slice, true);
                case Objects.AggregateFunction_Delta:
                    return ComputeDelta(slice);
                case Objects.AggregateFunction_StartBound:
                    return ComputeStartEnd2(slice, false);
                case Objects.AggregateFunction_EndBound:
                    return ComputeStartEnd2(slice, true);
                case Objects.AggregateFunction_DeltaBounds:
                    return ComputeDelta2(slice);
                default:
                    return base.ComputeValue(slice);
            }
        }
        #endregion Overridden Methods

        #region Protected Methods
        /// <summary>
        /// Calculate the Start and End aggregates for the timeslice.
        /// </summary>
        protected DataValue ComputeStartEnd(TimeSlice slice, bool returnEnd)
        {
            // get the values in the slice.
            List<DataValue> values = GetValues(slice);

            // check for empty slice.
            if (values == null || values.Count == 0)
            {
                return GetNoDataValue(slice);
            }

            // return start value.
            if (!returnEnd)
            {
                return values[0];
            }

            return values[^1];
        }

        /// <summary>
        /// Calculates the Delta aggregate for the timeslice.
        /// </summary>
        protected DataValue ComputeDelta(TimeSlice slice)
        {
            // get the values in the slice.
            List<DataValue> values = GetValues(slice);

            // check for empty slice.
            if (values == null || values.Count == 0)
            {
                return GetNoDataValue(slice);
            }

            double startValue = double.NaN;
            TypeInfo originalType = null;
            bool badDataSkipped = false;

            for (int ii = 0; ii < values.Count; ii++)
            {
                // find start value.
                DataValue start = values[ii];

                if (IsGood(start))
                {
                    try
                    {
                        startValue = CastToDouble(start);
                        originalType = start.WrappedValue.TypeInfo;
                        break;
                    }
                    catch (Exception)
                    {
                        startValue = double.NaN;
                    }
                }

                badDataSkipped = true;
            }

            double endValue = double.NaN;

            for (int ii = values.Count - 1; ii >= 0; ii--)
            {
                // find end value.
                DataValue end = values[ii];

                if (IsGood(end))
                {
                    try
                    {
                        endValue = CastToDouble(end);
                        break;
                    }
                    catch (Exception)
                    {
                        endValue = double.NaN;
                    }

                    break;
                }

                badDataSkipped = true;
            }

            // check if no good data.
            if (double.IsNaN(startValue) || double.IsNaN(endValue))
            {
                return GetNoDataValue(slice);
            }

            var value = new DataValue
            {
                SourceTimestamp = GetTimestamp(slice),
                ServerTimestamp = GetTimestamp(slice)
            };

            // set status code.
            if (badDataSkipped)
            {
                value.StatusCode = StatusCodes.UncertainDataSubNormal;
            }

            value.StatusCode = value.StatusCode.SetAggregateBits(AggregateBits.Calculated);

            // calculate delta.
            double delta = endValue - startValue;

            if (originalType != null && originalType.BuiltInType != BuiltInType.Double)
            {
                object delta2 = TypeInfo.Cast(
                    delta,
                    TypeInfo.Scalars.Double,
                    originalType.BuiltInType);
                value.WrappedValue = new Variant(delta2, originalType);
            }
            else
            {
                value.WrappedValue = new Variant(delta, TypeInfo.Scalars.Double);
            }

            // return result.
            return value;
        }

        /// <summary>
        /// Calculate the Start2 and End2 aggregates for the timeslice.
        /// </summary>
        protected DataValue ComputeStartEnd2(TimeSlice slice, bool returnEnd)
        {
            // get the values in the slice.
            List<DataValue> values = GetValuesWithSimpleBounds(slice);

            // check for empty slice.
            if (values == null || values.Count == 0)
            {
                return GetNoDataValue(slice);
            }

            DataValue value;

            // return start bound.
            if ((!returnEnd && !TimeFlowsBackward) || (returnEnd && TimeFlowsBackward))
            {
                value = values[0];
            }
            // return end bound.
            else
            {
                value = values[^1];
            }

            if (!IsGood(value))
            {
                value.StatusCode = StatusCodes.BadNoData;
            }

            if (returnEnd)
            {
                value.SourceTimestamp = GetTimestamp(slice);
                value.ServerTimestamp = GetTimestamp(slice);

                if (StatusCode.IsNotBad(value.StatusCode))
                {
                    value.StatusCode = value.StatusCode.SetAggregateBits(AggregateBits.Calculated);
                }
            }

            return value;
        }

        /// <summary>
        /// Calculates the Delta2 aggregate for the timeslice.
        /// </summary>
        protected DataValue ComputeDelta2(TimeSlice slice)
        {
            // get the values in the slice.
            List<DataValue> values = GetValuesWithSimpleBounds(slice);

            // check for empty slice.
            if (values == null || values.Count == 0)
            {
                return GetNoDataValue(slice);
            }

            DataValue start = values[0];
            DataValue end = values[^1];

            // check for bad bounds.
            if (!IsGood(start) || !IsGood(end))
            {
                return GetNoDataValue(slice);
            }

            TypeInfo originalType = null;

            // convert to doubles.
            double startValue;
            try
            {
                startValue = CastToDouble(start);
                originalType = start.WrappedValue.TypeInfo;
            }
            catch (Exception)
            {
                startValue = double.NaN;
            }

            double endValue;
            try
            {
                endValue = CastToDouble(end);
            }
            catch (Exception)
            {
                endValue = double.NaN;
            }

            // check for bad bounds.
            if (double.IsNaN(startValue) || double.IsNaN(endValue))
            {
                return GetNoDataValue(slice);
            }

            var value = new DataValue
            {
                SourceTimestamp = GetTimestamp(slice),
                ServerTimestamp = GetTimestamp(slice)
            };

            if (!IsGood(start) || !IsGood(end))
            {
                value.StatusCode = StatusCodes.UncertainDataSubNormal;
            }

            value.StatusCode = value.StatusCode.SetAggregateBits(AggregateBits.Calculated);

            // calculate delta.
            double delta = endValue - startValue;

            if (originalType != null && originalType.BuiltInType != BuiltInType.Double)
            {
                object delta2 = TypeInfo.Cast(
                    delta,
                    TypeInfo.Scalars.Double,
                    originalType.BuiltInType);
                value.WrappedValue = new Variant(delta2, originalType);
            }
            else
            {
                value.WrappedValue = new Variant(delta, TypeInfo.Scalars.Double);
            }

            // return result.
            return value;
        }
        #endregion Protected Methods
    }
}
