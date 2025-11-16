#region Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is subject to the Technosoftware GmbH Software License 
// Agreement, which can be found here:
// https://technosoftware.com/documents/Source_License_Agreement.pdf
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved

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
    public class CountAggregateCalculator : AggregateCalculator
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
        public CountAggregateCalculator(
            NodeId aggregateId,
            DateTime startTime,
            DateTime endTime,
            double processingInterval,
            bool stepped,
            AggregateConfiguration configuration)
            : base(aggregateId, startTime, endTime, processingInterval, stepped, configuration)
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

            if (id != null)
            {
                switch (id.Value)
                {
                    case Objects.AggregateFunction_Count:
                        return ComputeCount(slice);
                    case Objects.AggregateFunction_AnnotationCount:
                        return ComputeAnnotationCount(slice);
                    case Objects.AggregateFunction_DurationInStateZero:
                        return ComputeDurationInState(slice, false);
                    case Objects.AggregateFunction_DurationInStateNonZero:
                        return ComputeDurationInState(slice, true);
                    case Objects.AggregateFunction_NumberOfTransitions:
                        return ComputeNumberOfTransitions(slice);
                }
            }

            return base.ComputeValue(slice);
        }

        /// <summary>
        /// Calculates the Count aggregate for the timeslice.
        /// </summary>
        protected DataValue ComputeCount(TimeSlice slice)
        {
            // get the values in the slice.
            List<DataValue> values = GetValues(slice);

            // check for empty slice.
            if (values == null)
            {
                return GetNoDataValue(slice);
            }

            // count the values.
            int count = 0;

            for (int ii = 0; ii < values.Count; ii++)
            {
                if (StatusCode.IsGood(values[ii].StatusCode))
                {
                    count++;
                }
            }

            // set the timestamp and status.
            var value = new DataValue
            {
                WrappedValue = new Variant(count, TypeInfo.Scalars.Int32),
                SourceTimestamp = GetTimestamp(slice),
                ServerTimestamp = GetTimestamp(slice)
            };
            value.StatusCode = GetValueBasedStatusCode(slice, values, value.StatusCode);

            if (!StatusCode.IsBad(value.StatusCode))
            {
                // set aggregate bits fon non Bad values
                value.StatusCode = value.StatusCode.SetAggregateBits(AggregateBits.Calculated);
            }
            // return result.
            return value;
        }

        /// <summary>
        /// Calculates the AnnotationCount aggregate for the timeslice.
        /// </summary>
        protected DataValue ComputeAnnotationCount(TimeSlice slice)
        {
            // get the values in the slice.
            List<DataValue> values = GetValues(slice);

            // check for empty slice.
            if (values == null)
            {
                return GetNoDataValue(slice);
            }

            // count the values.
            int count = 0;

            for (int ii = 0; ii < values.Count; ii++)
            {
                count++;
            }

            // set the timestamp and status.
            var value = new DataValue
            {
                WrappedValue = new Variant(count, TypeInfo.Scalars.Int32),
                SourceTimestamp = GetTimestamp(slice),
                ServerTimestamp = GetTimestamp(slice)
            };
            value.StatusCode = value.StatusCode.SetAggregateBits(AggregateBits.Calculated);

            // return result.
            return value;
        }

        /// <summary>
        /// Calculates the DurationInStateZero and DurationInStateNonZero aggregates for the timeslice.
        /// </summary>
        protected DataValue ComputeDurationInState(TimeSlice slice, bool isNonZero)
        {
            // get the values in the slice.
            List<DataValue> values = GetValuesWithSimpleBounds(slice);

            // check for empty slice.
            if (values == null)
            {
                return GetNoDataValue(slice);
            }

            // get the regions.
            List<SubRegion> regions = GetRegionsInValueSet(values, false, true);

            double duration = 0;

            for (int ii = 0; ii < regions.Count; ii++)
            {
                if (StatusCode.IsNotGood(regions[ii].StatusCode))
                {
                    continue;
                }

                if (isNonZero)
                {
                    if (regions[ii].StartValue != 0)
                    {
                        duration += regions[ii].Duration;
                    }
                }
                else if (regions[ii].StartValue == 0)
                {
                    duration += regions[ii].Duration;
                }
            }

            // set the timestamp and status.
            var value = new DataValue
            {
                WrappedValue = new Variant(duration, TypeInfo.Scalars.Double),
                SourceTimestamp = GetTimestamp(slice),
                ServerTimestamp = GetTimestamp(slice)
            };
            value.StatusCode = GetTimeBasedStatusCode(regions, value.StatusCode);
            value.StatusCode = value.StatusCode.SetAggregateBits(AggregateBits.Calculated);

            // return result.
            return value;
        }

        /// <summary>
        /// Calculates the Count aggregate for the timeslice.
        /// </summary>
        protected DataValue ComputeNumberOfTransitions(TimeSlice slice)
        {
            // get the values in the slice.
            List<DataValue> values = GetValues(slice);

            // check for empty slice.
            if (values == null)
            {
                return GetNoDataValue(slice);
            }

            // determine whether a transition occurs at the StartTime
            double lastValue = double.NaN;

            if (slice.EarlyBound != null && StatusCode.IsGood(slice.EarlyBound.Value.StatusCode))
            {
                try
                {
                    lastValue = CastToDouble(slice.EarlyBound.Value);
                }
                catch (Exception)
                {
                    lastValue = double.NaN;
                }
            }

            // count the transitions.
            int count = 0;

            for (int ii = 0; ii < values.Count; ii++)
            {
                if (!IsGood(values[ii]))
                {
                    continue;
                }

                double nextValue;
                try
                {
                    nextValue = CastToDouble(values[ii]);
                }
                catch (Exception)
                {
                    continue;
                }

                if (!double.IsNaN(lastValue) && lastValue != nextValue)
                {
                    count++;
                }

                lastValue = nextValue;
            }

            // set the timestamp and status.
            var value = new DataValue
            {
                WrappedValue = new Variant(count, TypeInfo.Scalars.Int32),
                SourceTimestamp = GetTimestamp(slice),
                ServerTimestamp = GetTimestamp(slice)
            };
            value.StatusCode = value.StatusCode.SetAggregateBits(AggregateBits.Calculated);
            value.StatusCode = GetValueBasedStatusCode(slice, values, value.StatusCode);

            // return result.
            return value;
        }
        #endregion Overridden Methods
    }
}
