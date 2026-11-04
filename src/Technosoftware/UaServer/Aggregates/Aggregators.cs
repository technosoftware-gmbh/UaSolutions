#region Copyright (c) 2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com
//
// The Software is subject to the Technosoftware GmbH MIT License, which can
// be found here:
// https://technosoftware.com/license/mit/
//
// The Software is based on the OPC Foundation UA Stack and the OPC Foundation
// MIT License. The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Creates a new instance of an aggregate factory.
    /// </summary>
    public delegate IUaAggregateCalculator AggregatorFactory(
        NodeId aggregateId,
        DateTime startTime,
        DateTime endTime,
        double processingInterval,
        bool stepped,
        AggregateConfiguration configuration,
        ITelemetryContext telemetry);

    /// <summary>
    /// The set of built-in aggregate factories.
    /// </summary>
    public static class Aggregators
    {
        /// <summary>
        /// Stores the mapping for a aggregate id to the calculator.
        /// </summary>
        private class FactoryMapping
        {
            public NodeId AggregateId { get; set; }

            public QualifiedName AggregateName { get; set; }

            public Type Calculator { get; set; }
        }

        /// <summary>
        /// Mapping for all of the standard aggregates.
        /// </summary>
        private static readonly FactoryMapping[] s_mappings =
        [
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_Interpolative,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_Interpolative),
                Calculator = typeof(AggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_Average,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_Average),
                Calculator = typeof(AverageAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_TimeAverage,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_TimeAverage),
                Calculator = typeof(AverageAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_TimeAverage2,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_TimeAverage2),
                Calculator = typeof(AverageAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_Total,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_Total),
                Calculator = typeof(AverageAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_Total2,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_Total2),
                Calculator = typeof(AverageAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_Minimum,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_Minimum),
                Calculator = typeof(MinMaxAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_Maximum,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_Maximum),
                Calculator = typeof(MinMaxAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_MinimumActualTime,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_MinimumActualTime),
                Calculator = typeof(MinMaxAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_MaximumActualTime,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_MaximumActualTime),
                Calculator = typeof(MinMaxAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_Range,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_Range),
                Calculator = typeof(MinMaxAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_Minimum2,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_Minimum2),
                Calculator = typeof(MinMaxAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_Maximum2,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_Maximum2),
                Calculator = typeof(MinMaxAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_MinimumActualTime2,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_MinimumActualTime2),
                Calculator = typeof(MinMaxAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_MaximumActualTime2,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_MaximumActualTime2),
                Calculator = typeof(MinMaxAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_Range2,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_Range2),
                Calculator = typeof(MinMaxAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_Count,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_Count),
                Calculator = typeof(CountAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_AnnotationCount,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_AnnotationCount),
                Calculator = typeof(CountAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_DurationInStateZero,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_DurationInStateZero),
                Calculator = typeof(CountAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_DurationInStateNonZero,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_DurationInStateNonZero),
                Calculator = typeof(CountAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_NumberOfTransitions,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_NumberOfTransitions),
                Calculator = typeof(CountAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_Start,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_Start),
                Calculator = typeof(StartEndAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_End,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_End),
                Calculator = typeof(StartEndAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_Delta,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_Delta),
                Calculator = typeof(StartEndAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_StartBound,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_StartBound),
                Calculator = typeof(StartEndAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_EndBound,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_EndBound),
                Calculator = typeof(StartEndAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_DeltaBounds,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_DeltaBounds),
                Calculator = typeof(StartEndAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_DurationGood,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_DurationGood),
                Calculator = typeof(StatusAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_DurationBad,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_DurationBad),
                Calculator = typeof(StatusAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_PercentGood,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_PercentGood),
                Calculator = typeof(StatusAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_PercentBad,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_PercentBad),
                Calculator = typeof(StatusAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_WorstQuality,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_WorstQuality),
                Calculator = typeof(StatusAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_WorstQuality2,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_WorstQuality2),
                Calculator = typeof(StatusAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_StandardDeviationPopulation,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_StandardDeviationPopulation),
                Calculator = typeof(StdDevAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_VariancePopulation,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_VariancePopulation),
                Calculator = typeof(StdDevAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_StandardDeviationSample,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_StandardDeviationSample),
                Calculator = typeof(StdDevAggregateCalculator)
            },
            new FactoryMapping
            {
                AggregateId = ObjectIds.AggregateFunction_VarianceSample,
                AggregateName = new QualifiedName(BrowseNames.AggregateFunction_VarianceSample),
                Calculator = typeof(StdDevAggregateCalculator)
            }
        ];

        /// <summary>
        /// Returns the name for a standard aggregates.
        /// </summary>
        public static QualifiedName GetNameForStandardAggregate(NodeId aggregateId)
        {
            for (int ii = 0; ii < s_mappings.Length; ii++)
            {
                if (s_mappings[ii].AggregateId == aggregateId)
                {
                    return s_mappings[ii].AggregateName;
                }
            }

            return default;
        }

        /// <summary>
        /// Returns the id for a standard aggregates.
        /// </summary>
        public static NodeId GetIdForStandardAggregate(QualifiedName aggregateName)
        {
            for (int ii = 0; ii < s_mappings.Length; ii++)
            {
                if (s_mappings[ii].AggregateName == aggregateName)
                {
                    return s_mappings[ii].AggregateId;
                }
            }

            return default;
        }

        /// <summary>
        /// Creates a calculator for one of the standard aggregates.
        /// </summary>
        public static IUaAggregateCalculator CreateStandardCalculator(
            NodeId aggregateId,
            DateTime startTime,
            DateTime endTime,
            double processingInterval,
            bool stepped,
            AggregateConfiguration configuration,
            ITelemetryContext telemetry)
        {
            for (int ii = 0; ii < s_mappings.Length; ii++)
            {
                if (s_mappings[ii].AggregateId == aggregateId)
                {
                    return (IUaAggregateCalculator)
                        Activator.CreateInstance(
                            s_mappings[ii].Calculator,
                            aggregateId,
                            startTime,
                            endTime,
                            processingInterval,
                            stepped,
                            configuration,
                            telemetry);
                }
            }

            return null;
        }
    }
}
