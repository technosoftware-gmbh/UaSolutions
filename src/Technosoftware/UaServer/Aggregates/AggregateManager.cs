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
using System.Threading;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// An object that manages aggregate factories supported by the server.
    /// </summary>
    public class AggregateManager : IDisposable
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Initilizes the manager.
        /// </summary>
        public AggregateManager(IUaServerData server)
        {
            m_server = server;
            m_factories = [];
            m_minimumProcessingInterval = 1000;
        }
        #endregion Constructors, Destructor, Initialization
        
        #region IDisposable Members
        /// <summary>
        /// Frees any unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// An overrideable version of the Dispose.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TBD
            }
        }
        #endregion IDisposable Members

        #region Public Members
        /// <summary>
        /// Checks if the aggregate is supported by the server.
        /// </summary>
        /// <param name="aggregateId">The id of the aggregate function.</param>
        /// <returns>True if the aggregate is supported.</returns>
        public bool IsSupported(NodeId aggregateId)
        {
            if (NodeId.IsNull(aggregateId))
            {
                return false;
            }

            lock (m_lock)
            {
                return m_factories.ContainsKey(aggregateId);
            }
        }

        /// <summary>
        /// The minimum processing interval for any aggregate calculation.
        /// </summary>
        public double MinimumProcessingInterval
        {
            get
            {
                lock (m_lock)
                {
                    return m_minimumProcessingInterval;
                }
            }
            set
            {
                lock (m_lock)
                {
                    m_minimumProcessingInterval = value;
                }
            }
        }

        /// <summary>
        /// Returns the default configuration for the specified variable id.
        /// </summary>
        /// <param name="variableId">The id of history data node.</param>
        /// <returns>The configuration.</returns>
        public AggregateConfiguration GetDefaultConfiguration(NodeId variableId)
        {
            lock (m_lock)
            {
                m_defaultConfiguration ??= new AggregateConfiguration
                {
                    PercentDataBad = 100,
                    PercentDataGood = 100,
                    TreatUncertainAsBad = false,
                    UseSlopedExtrapolation = false,
                    UseServerCapabilitiesDefaults = false
                };

                return m_defaultConfiguration;
            }
        }

        /// <summary>
        /// Sets the default aggregate configuration.
        /// </summary>
        /// <param name="configuration">The default aggregate configuration..</param>
        public void SetDefaultConfiguration(AggregateConfiguration configuration)
        {
            lock (m_lock)
            {
                m_defaultConfiguration = configuration;
            }
        }

        /// <summary>
        /// Creates a new aggregate calculator.
        /// </summary>
        /// <param name="aggregateId">The id of the aggregate function.</param>
        /// <param name="startTime">When to start processing.</param>
        /// <param name="endTime">When to stop processing.</param>
        /// <param name="processingInterval">The processing interval.</param>
        /// <param name="stepped">Whether stepped interpolation should be used.</param>
        /// <param name="configuration">The configuration to use.</param>
        public IUaAggregateCalculator CreateCalculator(
            NodeId aggregateId,
            DateTime startTime,
            DateTime endTime,
            double processingInterval,
            bool stepped,
            AggregateConfiguration configuration)
        {
            if (NodeId.IsNull(aggregateId))
            {
                return null;
            }

            AggregatorFactory factory = null;

            lock (m_lock)
            {
                if (!m_factories.TryGetValue(aggregateId, out factory))
                {
                    return null;
                }
            }

            if (configuration.UseServerCapabilitiesDefaults)
            {
                // ensure the configuration is initialized
                configuration = GetDefaultConfiguration(null);
            }

            return factory(
                aggregateId,
                startTime,
                endTime,
                processingInterval,
                stepped,
                configuration,
                m_server.Telemetry);
        }

        /// <summary>
        /// Registers an aggregate factory.
        /// </summary>
        /// <param name="aggregateId">The id of the aggregate function.</param>
        /// <param name="aggregateName">The id of the aggregate name.</param>
        /// <param name="factory">The factory used to create calculators.</param>
        public void RegisterFactory(
            NodeId aggregateId,
            string aggregateName,
            AggregatorFactory factory)
        {
            lock (m_lock)
            {
                m_factories[aggregateId] = factory;
            }

            m_server?.DiagnosticsNodeManager.AddAggregateFunction(aggregateId, aggregateName, true);
        }

        /// <summary>
        /// Unregisters an aggregate factory.
        /// </summary>
        /// <param name="aggregateId">The id of the aggregate function.</param>
        public void RegisterFactory(NodeId aggregateId)
        {
            lock (m_lock)
            {
                m_factories.Remove(aggregateId);
            }
        }
        #endregion Public Members

        #region Private Methods
        #endregion Private Methods

        #region Private Fields
        private readonly Lock m_lock = new();
        private readonly IUaServerData m_server;
        private AggregateConfiguration m_defaultConfiguration;
        private readonly NodeIdDictionary<AggregatorFactory> m_factories;
        private double m_minimumProcessingInterval;
        #endregion Private Fields
    }
}
