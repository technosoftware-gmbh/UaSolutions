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
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Technosoftware.UaServer;

#endregion Using Directives

namespace SampleCompany.NodeManagers.SampleDataTypes
{
    /// <summary>
    /// The node manager factory for test data.
    /// </summary>
    public class SampleDataTypesNodeManagerFactory : IUaNodeManagerFactory
    {
        /// <inheritdoc/>
        public IUaNodeManager Create(IUaServerData server, ApplicationConfiguration configuration)
        {
            return new SampleDataTypesNodeManager(server, configuration, [.. NamespacesUris]);
        }

        /// <inheritdoc/>
        public StringCollection NamespacesUris
            => [Namespaces.SampleDataTypes, Namespaces.SampleDataTypes + "/Instance"];
    }

    /// <summary>
    /// A node manager for a server that exposes several variables.
    /// </summary>
    public class SampleDataTypesNodeManager : UaStandardNodeManager
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Initializes the node manager.
        /// </summary>
        public SampleDataTypesNodeManager(
            IUaServerData server,
            ApplicationConfiguration configuration,
            string[] namespaceUris)
            : base(server, configuration, server.Telemetry.CreateLogger<SampleDataTypesNodeManager>())
        {
            // update the namespaces.
            NamespaceUris = namespaceUris;

            ServerData.Factory.AddEncodeableTypes(
                typeof(SampleDataTypesNodeManager)
                    .Assembly.GetExportedTypes()
                    .Where(t => t.FullName
                        .StartsWith(typeof(SampleDataTypesNodeManager).Namespace, StringComparison.Ordinal)));

            m_dynamicNodes = new List<BaseDataVariableState>();
        }
        #endregion Constructors, Destructor, Initialization

        #region IDisposable Members
        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!m_disposed)
            {
                lock (m_lockDisposable)
                {
                    // If disposing equals true, dispose all managed
                    // and unmanaged resources.
                    if (disposing)
                    {
                        // Dispose managed resources.
                        m_simulationTimer?.Dispose();
                    }

                    // Call the appropriate methods to clean up
                    // unmanaged resources here.
                    // If disposing is false,
                    // only the following code is executed.

                    // Disposing has been done.
                    m_disposed = true;
                }
            }
            base.Dispose(disposing);
        }
        #endregion IDisposable Members

        #region INodeIdFactory Members
        #endregion INodeIdFactory Members

        #region Overridden Methods
        /// <summary>
        /// Loads a node set from a file or resource and add them to the set of predefined nodes.
        /// </summary>
        protected override NodeStateCollection LoadPredefinedNodes(ISystemContext context)
        {
            var predefinedNodes = new NodeStateCollection();
            predefinedNodes.LoadFromBinaryResource(
                context,
                "SampleCompany.NodeManagers.SampleDataTypes.Generated.SampleCompany.NodeManagers.SampleDataTypes.PredefinedNodes.uanodes",
                GetType().GetTypeInfo().Assembly,
                true);
            return predefinedNodes;
        }
        #endregion Overridden Methods

        #region IUaNodeManager Methods
        /// <summary>
        /// Does any initialization required before the address space can be used.
        /// </summary>
        /// <remarks>
        /// The externalReferences is an out parameter that allows the node manager to link to nodes
        /// in other node managers. For example, the 'Objects' node is managed by the CoreNodeManager and
        /// should have a reference to the root folder node(s) exposed by this node manager.  
        /// </remarks>
        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            lock (Lock)
            {
                if (!externalReferences.TryGetValue(Opc.Ua.ObjectIds.ObjectsFolder, out IList<IReference> references))
                {
                    externalReferences[Opc.Ua.ObjectIds.ObjectsFolder] = References = new List<IReference>();
                }
                else
                {
                    References = references;
                }

                LoadPredefinedNodes(SystemContext, externalReferences);

                // Create the root folder for all nodes of this server
                m_root = CreateFolderState(null, "My Data", new LocalizedText("en", "My Data"),
                    new LocalizedText("en", "Root folder of the Sample Server. All nodes must be placed under this root."));

                try
                {
                    #region Static
                    ResetRandomGenerator(1);
                    FolderState staticFolder = CreateFolderState(m_root, "Static", "Static", "A folder with a sample static variable.");
                    const string scalarStatic = "Static_";
                    CreateBaseDataVariableState(
                        staticFolder,
                        scalarStatic + "String",
                        "String",
                        null,
                        Opc.Ua.DataTypeIds.String,
                        ValueRanks.Scalar,
                        AccessLevels.CurrentReadOrWrite,
                        null);
                    #endregion Static

                    #region Simulation
                    FolderState simulationFolder = CreateFolderState(m_root, "Simulation", "Simulation", "A folder with simulated variables.");
                    const string simulation = "Simulation_";

                    BaseDataVariableState simulatedVariable = CreateDynamicVariable(
                        simulationFolder,
                        simulation + "Double",
                        "Double",
                        "A simulated variable of type Double. If Enabled is true this value changes based on the defined Interval.",
                        Opc.Ua.DataTypeIds.Double,
                        ValueRanks.Scalar,
                        AccessLevels.CurrentReadOrWrite,
                        null);

                    BaseDataVariableState intervalVariable = CreateBaseDataVariableState(
                        simulationFolder,
                        simulation + "Interval",
                        "Interval",
                        "The Interval used for changing the simulated values.",
                        Opc.Ua.DataTypeIds.UInt16,
                        ValueRanks.Scalar,
                        AccessLevels.CurrentReadOrWrite,
                        m_simulationInterval);
                    intervalVariable.OnSimpleWriteValue = OnWriteInterval;

                    BaseDataVariableState enabledVariable = CreateBaseDataVariableState(
                        simulationFolder,
                        simulation + "Enabled",
                        "Enabled",
                        "Specifies whether the simulation is enabled (true) or disabled (false).",
                        Opc.Ua.DataTypeIds.Boolean,
                        ValueRanks.Scalar,
                        AccessLevels.CurrentReadOrWrite,
                        m_simulationEnabled);
                    enabledVariable.OnSimpleWriteValue = OnWriteEnabled;
                    #endregion Simulation

                    #region Devices
                    FolderState devices = CreateFolderState(m_root, "Devices", "Devices", null);
                    var symbolicName = $"Controler #1";
                    var displayName = symbolicName;
                    var controller = new GenericControllerState(devices);

                    var nodeId = new NodeId(symbolicName, devices.NodeId.NamespaceIndex);
                    controller.Create(SystemContext, nodeId, symbolicName, displayName, true);

                    controller.AddReference(ReferenceTypeIds.Organizes, true, devices.NodeId);
                    devices.AddReference(ReferenceTypeIds.Organizes, false, controller.NodeId);
                    AddPredefinedNode(SystemContext, controller);
                    #endregion Devices

                    #region Plant
                    FolderState plantFolder = CreateFolderState(m_root, "Plant", "Plant", null);

                    // Create an instance for machine 1
                    symbolicName = $"Machine #1";
                    displayName = symbolicName;
                    m_machine1 = new MachineState(plantFolder);

                    nodeId = new NodeId(symbolicName, plantFolder.NodeId.NamespaceIndex);
                    m_machine1.Create(SystemContext, nodeId, symbolicName, displayName, true);
                    // Initialize the property value of MachineData
                    m_machine1.MachineData.Value = new MachineDataType {
                        MachineName = displayName,
                        Manufacturer = "SampleCompany",
                        SerialNumber = "SN 1079",
                        MachineState = MachineStateDataType.Inactive
                    };

                    m_machine1.AddReference(ReferenceTypeIds.Organizes, true, plantFolder.NodeId);
                    plantFolder.AddReference(ReferenceTypeIds.Organizes, false, m_machine1.NodeId);
                    AddPredefinedNode(SystemContext, m_machine1);

                    // Create an instance for machine 2
                    symbolicName = $"Machine #2";
                    displayName = symbolicName;
                    m_machine2 = new MachineState(plantFolder);

                    nodeId = new NodeId(symbolicName, plantFolder.NodeId.NamespaceIndex);
                    m_machine2.Create(
                        SystemContext,
                        nodeId,
                        displayName,
                        null,
                        true);
                    // Initialize the property value of MachineData
                    m_machine2.MachineData.Value = new MachineDataType {
                        MachineName = displayName,
                        Manufacturer = "Unknown",
                        SerialNumber = "SN 1312",
                        MachineState = MachineStateDataType.PrepareRemove
                    };

                    m_machine2.AddReference(ReferenceTypeIds.Organizes, true, plantFolder.NodeId);
                    plantFolder.AddReference(ReferenceTypeIds.Organizes, false, m_machine2.NodeId);
                    AddPredefinedNode(SystemContext, m_machine2);

                    // Create an instance of GetMachineDataMethodState
                    symbolicName = $"GetMachineData";
                    displayName = symbolicName;
                    var getMachineDataMethod = new GetMachineDataMethodState(plantFolder);

                    nodeId = new NodeId(symbolicName, plantFolder.NodeId.NamespaceIndex);
                    getMachineDataMethod.Create(SystemContext, nodeId, symbolicName, displayName, true);
                    getMachineDataMethod.AddReference(ReferenceTypeIds.Organizes, true, plantFolder.NodeId);
                    plantFolder.AddReference(ReferenceTypeIds.Organizes, false, getMachineDataMethod.NodeId);
                    plantFolder.AddChild(getMachineDataMethod);

                    // Add the event handler if the method is called
                    getMachineDataMethod.OnCall = OnGetMachineData;
                    AddPredefinedNode(SystemContext, getMachineDataMethod);
                    #endregion Plant
                }
                catch (Exception e)
                {
                    m_logger.LogError(e, "Error creating the SampleDataTypesServerNodeManager address space.");
                }

                AddPredefinedNode(SystemContext, m_root);

                // reset random generator and generate boundary values
                ResetRandomGenerator(100, 1);
                m_simulationTimer = new Timer(DoSimulation, null, 1000, 1000);
            }
        }
        #endregion IUaNodeManager Methods

        #region Event Handlers
        private ServiceResult OnWriteInterval(ISystemContext context, NodeState node, ref object value)
        {
            try
            {
                m_simulationInterval = (ushort)value;

                if (m_simulationEnabled)
                {
                    m_simulationTimer.Change(100, m_simulationInterval);
                }

                return ServiceResult.Good;
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Error writing Interval variable.");
                return ServiceResult.Create(e, StatusCodes.Bad, "Error writing Interval variable.");
            }
        }

        private ServiceResult OnWriteEnabled(ISystemContext context, NodeState node, ref object value)
        {
            try
            {
                m_simulationEnabled = (bool)value;

                m_simulationTimer.Change(100, m_simulationEnabled ? m_simulationInterval : 0);

                return ServiceResult.Good;
            }
            catch (Exception e)
            {
                m_logger.LogTrace(e, "Error writing Enabled variable.");
                return ServiceResult.Create(e, StatusCodes.Bad, "Error writing Enabled variable.");
            }
        }

        private ServiceResult OnGetMachineData(ISystemContext context, MethodState method, NodeId objectid, string machineName, ref MachineDataType machinedata)
        {
            machinedata = new MachineDataType { MachineName = machineName };

            if (machineName == "Machine #1")
            {
                machinedata.Manufacturer = m_machine1.MachineData.Value.Manufacturer;
                machinedata.SerialNumber = m_machine1.MachineData.Value.SerialNumber;
                machinedata.MachineState = m_machine1.MachineData.Value.MachineState;
            }
            else if (machineName == "Machine #2")
            {
                machinedata.Manufacturer = m_machine1.MachineData.Value.Manufacturer;
                machinedata.SerialNumber = m_machine1.MachineData.Value.SerialNumber;
                machinedata.MachineState = m_machine1.MachineData.Value.MachineState;
            }
            else
            {
                machinedata.Manufacturer = "Unknown";
                machinedata.SerialNumber = "Unknown";
                machinedata.MachineState = MachineStateDataType.Failed;
            }
            return ServiceResult.Good;
        }
        #endregion Event Handlers

        #region Helper Methods
        /// <summary>
        /// Creates a new variable.
        /// </summary>
        private BaseDataVariableState CreateDynamicVariable(
            NodeState parent,
            string path,
            string name,
            string description,
            NodeId dataType,
            int valueRank,
            byte accessLevel,
            object initialValue)
        {
            BaseDataVariableState variable = CreateBaseDataVariableState(parent, path, name, description, dataType, valueRank, accessLevel, initialValue);
            m_dynamicNodes.Add(variable);
            return variable;
        }

        private void DoSimulation(object state)
        {
            try
            {
                lock (Lock)
                {
                    DateTime timeStamp = DateTime.UtcNow;
                    foreach (BaseDataVariableState variable in m_dynamicNodes)
                    {
                        variable.Value = GetNewValue(variable);
                        variable.Timestamp = timeStamp;
                        variable.ClearChangeMasks(SystemContext, false);
                    }
                }
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Unexpected error doing simulation.");
            }
        }
        #endregion Helper Methods

        #region Private Fields
        // Track whether Dispose has been called.
        private bool m_disposed;
        private readonly object m_lockDisposable = new object();

        private Timer m_simulationTimer;
        private UInt16 m_simulationInterval = 1000;
        private bool m_simulationEnabled = true;
        private List<BaseDataVariableState> m_dynamicNodes;

        private MachineState m_machine1;
        private MachineState m_machine2;
        private FolderState m_root;
        #endregion Private Fields
    }
}
