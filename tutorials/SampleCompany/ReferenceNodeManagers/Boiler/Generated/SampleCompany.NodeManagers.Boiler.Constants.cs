/* ========================================================================
 * Copyright (c) 2005-2024 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Runtime.Serialization;
using Opc.Ua;

#pragma warning disable 1591

namespace SampleCompany.NodeManagers.Boiler
{
    #region Method Identifiers
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public static partial class Methods
    {
        public const uint BoilerStateMachineType_Start = 1118;

        public const uint BoilerStateMachineType_Suspend = 1119;

        public const uint BoilerStateMachineType_Resume = 1120;

        public const uint BoilerStateMachineType_Halt = 1121;

        public const uint BoilerStateMachineType_Reset = 1122;

        public const uint BoilerType_Simulation_Start = 1233;

        public const uint BoilerType_Simulation_Suspend = 1234;

        public const uint BoilerType_Simulation_Resume = 1235;

        public const uint BoilerType_Simulation_Halt = 1236;

        public const uint BoilerType_Simulation_Reset = 1237;

        public const uint Boilers_Boiler1_Simulation_Start = 1317;

        public const uint Boilers_Boiler1_Simulation_Suspend = 1318;

        public const uint Boilers_Boiler1_Simulation_Resume = 1319;

        public const uint Boilers_Boiler1_Simulation_Halt = 1320;

        public const uint Boilers_Boiler1_Simulation_Reset = 1321;
    }
    #endregion

    #region Object Identifiers
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public static partial class Objects
    {
        public const uint BoilerInputPipeType_FlowTransmitter1 = 1125;

        public const uint BoilerInputPipeType_Valve = 1132;

        public const uint BoilerDrumType_LevelIndicator = 1140;

        public const uint BoilerOutputPipeType_FlowTransmitter2 = 1148;

        public const uint BoilerType_InputPipe = 1156;

        public const uint BoilerType_InputPipe_FlowTransmitter1 = 1157;

        public const uint BoilerType_InputPipe_Valve = 1164;

        public const uint BoilerType_Drum = 1171;

        public const uint BoilerType_Drum_LevelIndicator = 1172;

        public const uint BoilerType_OutputPipe = 1179;

        public const uint BoilerType_OutputPipe_FlowTransmitter2 = 1180;

        public const uint BoilerType_FlowController = 1187;

        public const uint BoilerType_LevelController = 1191;

        public const uint BoilerType_CustomController = 1195;

        public const uint BoilerType_Simulation = 1201;

        public const uint Boilers = 1238;

        public const uint Boilers_Boiler1 = 1239;

        public const uint Boilers_Boiler1_InputPipe = 1240;

        public const uint Boilers_Boiler1_InputPipe_FlowTransmitter1 = 1241;

        public const uint Boilers_Boiler1_InputPipe_Valve = 1248;

        public const uint Boilers_Boiler1_Drum = 1255;

        public const uint Boilers_Boiler1_Drum_LevelIndicator = 1256;

        public const uint Boilers_Boiler1_OutputPipe = 1263;

        public const uint Boilers_Boiler1_OutputPipe_FlowTransmitter2 = 1264;

        public const uint Boilers_Boiler1_FlowController = 1271;

        public const uint Boilers_Boiler1_LevelController = 1275;

        public const uint Boilers_Boiler1_CustomController = 1279;

        public const uint Boilers_Boiler1_Simulation = 1285;
    }
    #endregion

    #region ObjectType Identifiers
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public static partial class ObjectTypes
    {
        public const uint GenericControllerType = 1004;

        public const uint GenericSensorType = 1008;

        public const uint GenericActuatorType = 1015;

        public const uint CustomControllerType = 1022;

        public const uint ValveType = 1028;

        public const uint LevelControllerType = 1035;

        public const uint FlowControllerType = 1039;

        public const uint LevelIndicatorType = 1043;

        public const uint FlowTransmitterType = 1050;

        public const uint BoilerStateMachineType = 1057;

        public const uint BoilerInputPipeType = 1124;

        public const uint BoilerDrumType = 1139;

        public const uint BoilerOutputPipeType = 1147;

        public const uint BoilerType = 1155;
    }
    #endregion

    #region ReferenceType Identifiers
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public static partial class ReferenceTypes
    {
        public const uint FlowTo = 1001;

        public const uint HotFlowTo = 1002;

        public const uint SignalTo = 1003;
    }
    #endregion

    #region Variable Identifiers
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public static partial class Variables
    {
        public const uint GenericControllerType_Measurement = 1005;

        public const uint GenericControllerType_SetPoint = 1006;

        public const uint GenericControllerType_ControlOut = 1007;

        public const uint GenericSensorType_Output = 1009;

        public const uint GenericSensorType_Output_EURange = 1013;

        public const uint GenericActuatorType_Input = 1016;

        public const uint GenericActuatorType_Input_EURange = 1020;

        public const uint CustomControllerType_Input1 = 1023;

        public const uint CustomControllerType_Input2 = 1024;

        public const uint CustomControllerType_Input3 = 1025;

        public const uint CustomControllerType_ControlOut = 1026;

        public const uint CustomControllerType_DescriptionX = 1027;

        public const uint BoilerStateMachineType_Halted_StateNumber = 1093;

        public const uint BoilerStateMachineType_Suspended_StateNumber = 1099;

        public const uint BoilerStateMachineType_HaltedToReady_TransitionNumber = 1101;

        public const uint BoilerStateMachineType_SuspendedToRunning_TransitionNumber = 1111;

        public const uint BoilerStateMachineType_SuspendedToHalted_TransitionNumber = 1113;

        public const uint BoilerStateMachineType_SuspendedToReady_TransitionNumber = 1115;

        public const uint BoilerStateMachineType_UpdateRate = 1123;

        public const uint BoilerInputPipeType_FlowTransmitter1_Output = 1126;

        public const uint BoilerInputPipeType_FlowTransmitter1_Output_EURange = 1130;

        public const uint BoilerInputPipeType_Valve_Input = 1133;

        public const uint BoilerInputPipeType_Valve_Input_EURange = 1137;

        public const uint BoilerDrumType_LevelIndicator_Output = 1141;

        public const uint BoilerDrumType_LevelIndicator_Output_EURange = 1145;

        public const uint BoilerOutputPipeType_FlowTransmitter2_Output = 1149;

        public const uint BoilerOutputPipeType_FlowTransmitter2_Output_EURange = 1153;

        public const uint BoilerType_InputPipe_FlowTransmitter1_Output = 1158;

        public const uint BoilerType_InputPipe_FlowTransmitter1_Output_EURange = 1162;

        public const uint BoilerType_InputPipe_Valve_Input = 1165;

        public const uint BoilerType_InputPipe_Valve_Input_EURange = 1169;

        public const uint BoilerType_Drum_LevelIndicator_Output = 1173;

        public const uint BoilerType_Drum_LevelIndicator_Output_EURange = 1177;

        public const uint BoilerType_OutputPipe_FlowTransmitter2_Output = 1181;

        public const uint BoilerType_OutputPipe_FlowTransmitter2_Output_EURange = 1185;

        public const uint BoilerType_FlowController_Measurement = 1188;

        public const uint BoilerType_FlowController_SetPoint = 1189;

        public const uint BoilerType_FlowController_ControlOut = 1190;

        public const uint BoilerType_LevelController_Measurement = 1192;

        public const uint BoilerType_LevelController_SetPoint = 1193;

        public const uint BoilerType_LevelController_ControlOut = 1194;

        public const uint BoilerType_CustomController_Input1 = 1196;

        public const uint BoilerType_CustomController_Input2 = 1197;

        public const uint BoilerType_CustomController_Input3 = 1198;

        public const uint BoilerType_CustomController_ControlOut = 1199;

        public const uint BoilerType_CustomController_DescriptionX = 1200;

        public const uint BoilerType_Simulation_CurrentState = 1202;

        public const uint BoilerType_Simulation_CurrentState_Id = 1203;

        public const uint BoilerType_Simulation_CurrentState_Number = 1205;

        public const uint BoilerType_Simulation_LastTransition = 1207;

        public const uint BoilerType_Simulation_LastTransition_Id = 1208;

        public const uint BoilerType_Simulation_LastTransition_Number = 1210;

        public const uint BoilerType_Simulation_LastTransition_TransitionTime = 1211;

        public const uint BoilerType_Simulation_Deletable = 1215;

        public const uint BoilerType_Simulation_AutoDelete = 1216;

        public const uint BoilerType_Simulation_RecycleCount = 1217;

        public const uint BoilerType_Simulation_ProgramDiagnostic_CreateSessionId = 1219;

        public const uint BoilerType_Simulation_ProgramDiagnostic_CreateClientName = 1220;

        public const uint BoilerType_Simulation_ProgramDiagnostic_InvocationCreationTime = 1221;

        public const uint BoilerType_Simulation_ProgramDiagnostic_LastTransitionTime = 1222;

        public const uint BoilerType_Simulation_ProgramDiagnostic_LastMethodCall = 1223;

        public const uint BoilerType_Simulation_ProgramDiagnostic_LastMethodSessionId = 1224;

        public const uint BoilerType_Simulation_ProgramDiagnostic_LastMethodInputArguments = 1225;

        public const uint BoilerType_Simulation_ProgramDiagnostic_LastMethodOutputArguments = 1226;

        public const uint BoilerType_Simulation_ProgramDiagnostic_LastMethodInputValues = 1227;

        public const uint BoilerType_Simulation_ProgramDiagnostic_LastMethodOutputValues = 1228;

        public const uint BoilerType_Simulation_ProgramDiagnostic_LastMethodCallTime = 1229;

        public const uint BoilerType_Simulation_ProgramDiagnostic_LastMethodReturnStatus = 1230;

        public const uint BoilerType_Simulation_UpdateRate = 1232;

        public const uint Boilers_Boiler1_InputPipe_FlowTransmitter1_Output = 1242;

        public const uint Boilers_Boiler1_InputPipe_FlowTransmitter1_Output_EURange = 1246;

        public const uint Boilers_Boiler1_InputPipe_Valve_Input = 1249;

        public const uint Boilers_Boiler1_InputPipe_Valve_Input_EURange = 1253;

        public const uint Boilers_Boiler1_Drum_LevelIndicator_Output = 1257;

        public const uint Boilers_Boiler1_Drum_LevelIndicator_Output_EURange = 1261;

        public const uint Boilers_Boiler1_OutputPipe_FlowTransmitter2_Output = 1265;

        public const uint Boilers_Boiler1_OutputPipe_FlowTransmitter2_Output_EURange = 1269;

        public const uint Boilers_Boiler1_FlowController_Measurement = 1272;

        public const uint Boilers_Boiler1_FlowController_SetPoint = 1273;

        public const uint Boilers_Boiler1_FlowController_ControlOut = 1274;

        public const uint Boilers_Boiler1_LevelController_Measurement = 1276;

        public const uint Boilers_Boiler1_LevelController_SetPoint = 1277;

        public const uint Boilers_Boiler1_LevelController_ControlOut = 1278;

        public const uint Boilers_Boiler1_CustomController_Input1 = 1280;

        public const uint Boilers_Boiler1_CustomController_Input2 = 1281;

        public const uint Boilers_Boiler1_CustomController_Input3 = 1282;

        public const uint Boilers_Boiler1_CustomController_ControlOut = 1283;

        public const uint Boilers_Boiler1_CustomController_DescriptionX = 1284;

        public const uint Boilers_Boiler1_Simulation_CurrentState = 1286;

        public const uint Boilers_Boiler1_Simulation_CurrentState_Id = 1287;

        public const uint Boilers_Boiler1_Simulation_CurrentState_Number = 1289;

        public const uint Boilers_Boiler1_Simulation_LastTransition = 1291;

        public const uint Boilers_Boiler1_Simulation_LastTransition_Id = 1292;

        public const uint Boilers_Boiler1_Simulation_LastTransition_Number = 1294;

        public const uint Boilers_Boiler1_Simulation_LastTransition_TransitionTime = 1295;

        public const uint Boilers_Boiler1_Simulation_Deletable = 1299;

        public const uint Boilers_Boiler1_Simulation_AutoDelete = 1300;

        public const uint Boilers_Boiler1_Simulation_RecycleCount = 1301;

        public const uint Boilers_Boiler1_Simulation_ProgramDiagnostic_CreateSessionId = 1303;

        public const uint Boilers_Boiler1_Simulation_ProgramDiagnostic_CreateClientName = 1304;

        public const uint Boilers_Boiler1_Simulation_ProgramDiagnostic_InvocationCreationTime = 1305;

        public const uint Boilers_Boiler1_Simulation_ProgramDiagnostic_LastTransitionTime = 1306;

        public const uint Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodCall = 1307;

        public const uint Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodSessionId = 1308;

        public const uint Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodInputArguments = 1309;

        public const uint Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodOutputArguments = 1310;

        public const uint Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodInputValues = 1311;

        public const uint Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodOutputValues = 1312;

        public const uint Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodCallTime = 1313;

        public const uint Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodReturnStatus = 1314;

        public const uint Boilers_Boiler1_Simulation_UpdateRate = 1316;
    }
    #endregion

    #region Method Node Identifiers
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public static partial class MethodIds
    {
        public static readonly ExpandedNodeId BoilerStateMachineType_Start = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.BoilerStateMachineType_Start, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerStateMachineType_Suspend = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.BoilerStateMachineType_Suspend, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerStateMachineType_Resume = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.BoilerStateMachineType_Resume, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerStateMachineType_Halt = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.BoilerStateMachineType_Halt, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerStateMachineType_Reset = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.BoilerStateMachineType_Reset, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_Start = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.BoilerType_Simulation_Start, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_Suspend = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.BoilerType_Simulation_Suspend, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_Resume = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.BoilerType_Simulation_Resume, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_Halt = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.BoilerType_Simulation_Halt, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_Reset = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.BoilerType_Simulation_Reset, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_Start = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.Boilers_Boiler1_Simulation_Start, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_Suspend = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.Boilers_Boiler1_Simulation_Suspend, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_Resume = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.Boilers_Boiler1_Simulation_Resume, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_Halt = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.Boilers_Boiler1_Simulation_Halt, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_Reset = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Methods.Boilers_Boiler1_Simulation_Reset, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);
    }
    #endregion

    #region Object Node Identifiers
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public static partial class ObjectIds
    {
        public static readonly ExpandedNodeId BoilerInputPipeType_FlowTransmitter1 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerInputPipeType_FlowTransmitter1, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerInputPipeType_Valve = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerInputPipeType_Valve, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerDrumType_LevelIndicator = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerDrumType_LevelIndicator, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerOutputPipeType_FlowTransmitter2 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerOutputPipeType_FlowTransmitter2, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_InputPipe = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerType_InputPipe, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_InputPipe_FlowTransmitter1 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerType_InputPipe_FlowTransmitter1, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_InputPipe_Valve = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerType_InputPipe_Valve, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Drum = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerType_Drum, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Drum_LevelIndicator = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerType_Drum_LevelIndicator, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_OutputPipe = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerType_OutputPipe, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_OutputPipe_FlowTransmitter2 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerType_OutputPipe_FlowTransmitter2, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_FlowController = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerType_FlowController, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_LevelController = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerType_LevelController, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_CustomController = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerType_CustomController, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.BoilerType_Simulation, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.Boilers, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.Boilers_Boiler1, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_InputPipe = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.Boilers_Boiler1_InputPipe, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_InputPipe_FlowTransmitter1 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.Boilers_Boiler1_InputPipe_FlowTransmitter1, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_InputPipe_Valve = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.Boilers_Boiler1_InputPipe_Valve, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Drum = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.Boilers_Boiler1_Drum, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Drum_LevelIndicator = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.Boilers_Boiler1_Drum_LevelIndicator, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_OutputPipe = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.Boilers_Boiler1_OutputPipe, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_OutputPipe_FlowTransmitter2 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.Boilers_Boiler1_OutputPipe_FlowTransmitter2, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_FlowController = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.Boilers_Boiler1_FlowController, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_LevelController = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.Boilers_Boiler1_LevelController, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_CustomController = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.Boilers_Boiler1_CustomController, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Objects.Boilers_Boiler1_Simulation, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);
    }
    #endregion

    #region ObjectType Node Identifiers
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public static partial class ObjectTypeIds
    {
        public static readonly ExpandedNodeId GenericControllerType = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ObjectTypes.GenericControllerType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId GenericSensorType = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ObjectTypes.GenericSensorType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId GenericActuatorType = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ObjectTypes.GenericActuatorType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId CustomControllerType = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ObjectTypes.CustomControllerType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId ValveType = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ObjectTypes.ValveType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId LevelControllerType = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ObjectTypes.LevelControllerType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId FlowControllerType = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ObjectTypes.FlowControllerType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId LevelIndicatorType = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ObjectTypes.LevelIndicatorType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId FlowTransmitterType = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ObjectTypes.FlowTransmitterType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerStateMachineType = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ObjectTypes.BoilerStateMachineType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerInputPipeType = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ObjectTypes.BoilerInputPipeType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerDrumType = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ObjectTypes.BoilerDrumType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerOutputPipeType = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ObjectTypes.BoilerOutputPipeType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ObjectTypes.BoilerType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);
    }
    #endregion

    #region ReferenceType Node Identifiers
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public static partial class ReferenceTypeIds
    {
        public static readonly ExpandedNodeId FlowTo = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ReferenceTypes.FlowTo, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId HotFlowTo = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ReferenceTypes.HotFlowTo, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId SignalTo = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.ReferenceTypes.SignalTo, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);
    }
    #endregion

    #region Variable Node Identifiers
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public static partial class VariableIds
    {
        public static readonly ExpandedNodeId GenericControllerType_Measurement = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.GenericControllerType_Measurement, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId GenericControllerType_SetPoint = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.GenericControllerType_SetPoint, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId GenericControllerType_ControlOut = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.GenericControllerType_ControlOut, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId GenericSensorType_Output = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.GenericSensorType_Output, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId GenericSensorType_Output_EURange = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.GenericSensorType_Output_EURange, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId GenericActuatorType_Input = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.GenericActuatorType_Input, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId GenericActuatorType_Input_EURange = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.GenericActuatorType_Input_EURange, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId CustomControllerType_Input1 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.CustomControllerType_Input1, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId CustomControllerType_Input2 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.CustomControllerType_Input2, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId CustomControllerType_Input3 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.CustomControllerType_Input3, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId CustomControllerType_ControlOut = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.CustomControllerType_ControlOut, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId CustomControllerType_DescriptionX = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.CustomControllerType_DescriptionX, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerStateMachineType_Halted_StateNumber = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerStateMachineType_Halted_StateNumber, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerStateMachineType_Suspended_StateNumber = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerStateMachineType_Suspended_StateNumber, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerStateMachineType_HaltedToReady_TransitionNumber = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerStateMachineType_HaltedToReady_TransitionNumber, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerStateMachineType_SuspendedToRunning_TransitionNumber = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerStateMachineType_SuspendedToRunning_TransitionNumber, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerStateMachineType_SuspendedToHalted_TransitionNumber = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerStateMachineType_SuspendedToHalted_TransitionNumber, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerStateMachineType_SuspendedToReady_TransitionNumber = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerStateMachineType_SuspendedToReady_TransitionNumber, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerStateMachineType_UpdateRate = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerStateMachineType_UpdateRate, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerInputPipeType_FlowTransmitter1_Output = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerInputPipeType_FlowTransmitter1_Output, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerInputPipeType_FlowTransmitter1_Output_EURange = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerInputPipeType_FlowTransmitter1_Output_EURange, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerInputPipeType_Valve_Input = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerInputPipeType_Valve_Input, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerInputPipeType_Valve_Input_EURange = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerInputPipeType_Valve_Input_EURange, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerDrumType_LevelIndicator_Output = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerDrumType_LevelIndicator_Output, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerDrumType_LevelIndicator_Output_EURange = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerDrumType_LevelIndicator_Output_EURange, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerOutputPipeType_FlowTransmitter2_Output = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerOutputPipeType_FlowTransmitter2_Output, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerOutputPipeType_FlowTransmitter2_Output_EURange = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerOutputPipeType_FlowTransmitter2_Output_EURange, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_InputPipe_FlowTransmitter1_Output = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_InputPipe_FlowTransmitter1_Output, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_InputPipe_FlowTransmitter1_Output_EURange = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_InputPipe_FlowTransmitter1_Output_EURange, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_InputPipe_Valve_Input = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_InputPipe_Valve_Input, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_InputPipe_Valve_Input_EURange = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_InputPipe_Valve_Input_EURange, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Drum_LevelIndicator_Output = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Drum_LevelIndicator_Output, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Drum_LevelIndicator_Output_EURange = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Drum_LevelIndicator_Output_EURange, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_OutputPipe_FlowTransmitter2_Output = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_OutputPipe_FlowTransmitter2_Output, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_OutputPipe_FlowTransmitter2_Output_EURange = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_OutputPipe_FlowTransmitter2_Output_EURange, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_FlowController_Measurement = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_FlowController_Measurement, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_FlowController_SetPoint = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_FlowController_SetPoint, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_FlowController_ControlOut = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_FlowController_ControlOut, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_LevelController_Measurement = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_LevelController_Measurement, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_LevelController_SetPoint = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_LevelController_SetPoint, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_LevelController_ControlOut = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_LevelController_ControlOut, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_CustomController_Input1 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_CustomController_Input1, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_CustomController_Input2 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_CustomController_Input2, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_CustomController_Input3 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_CustomController_Input3, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_CustomController_ControlOut = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_CustomController_ControlOut, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_CustomController_DescriptionX = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_CustomController_DescriptionX, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_CurrentState = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_CurrentState, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_CurrentState_Id = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_CurrentState_Id, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_CurrentState_Number = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_CurrentState_Number, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_LastTransition = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_LastTransition, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_LastTransition_Id = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_LastTransition_Id, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_LastTransition_Number = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_LastTransition_Number, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_LastTransition_TransitionTime = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_LastTransition_TransitionTime, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_Deletable = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_Deletable, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_AutoDelete = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_AutoDelete, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_RecycleCount = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_RecycleCount, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_ProgramDiagnostic_CreateSessionId = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_ProgramDiagnostic_CreateSessionId, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_ProgramDiagnostic_CreateClientName = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_ProgramDiagnostic_CreateClientName, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_ProgramDiagnostic_InvocationCreationTime = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_ProgramDiagnostic_InvocationCreationTime, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_ProgramDiagnostic_LastTransitionTime = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_ProgramDiagnostic_LastTransitionTime, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_ProgramDiagnostic_LastMethodCall = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_ProgramDiagnostic_LastMethodCall, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_ProgramDiagnostic_LastMethodSessionId = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_ProgramDiagnostic_LastMethodSessionId, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_ProgramDiagnostic_LastMethodInputArguments = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_ProgramDiagnostic_LastMethodInputArguments, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_ProgramDiagnostic_LastMethodOutputArguments = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_ProgramDiagnostic_LastMethodOutputArguments, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_ProgramDiagnostic_LastMethodInputValues = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_ProgramDiagnostic_LastMethodInputValues, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_ProgramDiagnostic_LastMethodOutputValues = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_ProgramDiagnostic_LastMethodOutputValues, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_ProgramDiagnostic_LastMethodCallTime = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_ProgramDiagnostic_LastMethodCallTime, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_ProgramDiagnostic_LastMethodReturnStatus = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_ProgramDiagnostic_LastMethodReturnStatus, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId BoilerType_Simulation_UpdateRate = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.BoilerType_Simulation_UpdateRate, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_InputPipe_FlowTransmitter1_Output = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_InputPipe_FlowTransmitter1_Output, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_InputPipe_FlowTransmitter1_Output_EURange = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_InputPipe_FlowTransmitter1_Output_EURange, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_InputPipe_Valve_Input = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_InputPipe_Valve_Input, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_InputPipe_Valve_Input_EURange = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_InputPipe_Valve_Input_EURange, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Drum_LevelIndicator_Output = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Drum_LevelIndicator_Output, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Drum_LevelIndicator_Output_EURange = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Drum_LevelIndicator_Output_EURange, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_OutputPipe_FlowTransmitter2_Output = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_OutputPipe_FlowTransmitter2_Output, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_OutputPipe_FlowTransmitter2_Output_EURange = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_OutputPipe_FlowTransmitter2_Output_EURange, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_FlowController_Measurement = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_FlowController_Measurement, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_FlowController_SetPoint = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_FlowController_SetPoint, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_FlowController_ControlOut = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_FlowController_ControlOut, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_LevelController_Measurement = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_LevelController_Measurement, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_LevelController_SetPoint = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_LevelController_SetPoint, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_LevelController_ControlOut = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_LevelController_ControlOut, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_CustomController_Input1 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_CustomController_Input1, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_CustomController_Input2 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_CustomController_Input2, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_CustomController_Input3 = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_CustomController_Input3, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_CustomController_ControlOut = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_CustomController_ControlOut, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_CustomController_DescriptionX = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_CustomController_DescriptionX, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_CurrentState = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_CurrentState, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_CurrentState_Id = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_CurrentState_Id, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_CurrentState_Number = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_CurrentState_Number, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_LastTransition = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_LastTransition, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_LastTransition_Id = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_LastTransition_Id, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_LastTransition_Number = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_LastTransition_Number, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_LastTransition_TransitionTime = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_LastTransition_TransitionTime, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_Deletable = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_Deletable, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_AutoDelete = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_AutoDelete, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_RecycleCount = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_RecycleCount, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_ProgramDiagnostic_CreateSessionId = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_ProgramDiagnostic_CreateSessionId, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_ProgramDiagnostic_CreateClientName = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_ProgramDiagnostic_CreateClientName, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_ProgramDiagnostic_InvocationCreationTime = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_ProgramDiagnostic_InvocationCreationTime, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_ProgramDiagnostic_LastTransitionTime = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_ProgramDiagnostic_LastTransitionTime, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodCall = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodCall, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodSessionId = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodSessionId, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodInputArguments = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodInputArguments, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodOutputArguments = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodOutputArguments, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodInputValues = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodInputValues, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodOutputValues = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodOutputValues, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodCallTime = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodCallTime, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodReturnStatus = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_ProgramDiagnostic_LastMethodReturnStatus, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);

        public static readonly ExpandedNodeId Boilers_Boiler1_Simulation_UpdateRate = new ExpandedNodeId(SampleCompany.NodeManagers.Boiler.Variables.Boilers_Boiler1_Simulation_UpdateRate, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler);
    }
    #endregion

    #region BrowseName Declarations
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public static partial class BrowseNames
    {
        public const string Boiler1 = "Boiler #1";

        public const string BoilerDrumType = "BoilerDrumType";

        public const string BoilerInputPipeType = "BoilerInputPipeType";

        public const string BoilerOutputPipeType = "BoilerOutputPipeType";

        public const string Boilers = "Boilers";

        public const string BoilerStateMachineType = "BoilerStateMachineType";

        public const string BoilerType = "BoilerType";

        public const string ControlOut = "ControlOut";

        public const string CustomController = "CCX001";

        public const string CustomControllerType = "CustomControllerType";

        public const string DescriptionX = "Description";

        public const string Drum = "DrumX001";

        public const string FlowController = "FCX001";

        public const string FlowControllerType = "FlowControllerType";

        public const string FlowTo = "FlowTo";

        public const string FlowTransmitter1 = "FTX001";

        public const string FlowTransmitter2 = "FTX002";

        public const string FlowTransmitterType = "FlowTransmitterType";

        public const string GenericActuatorType = "GenericActuatorType";

        public const string GenericControllerType = "GenericControllerType";

        public const string GenericSensorType = "GenericSensorType";

        public const string Halt = "Halt";

        public const string HotFlowTo = "HotFlowTo";

        public const string Input = "Input";

        public const string Input1 = "Input1";

        public const string Input2 = "Input2";

        public const string Input3 = "Input3";

        public const string InputPipe = "PipeX001";

        public const string LevelController = "LCX001";

        public const string LevelControllerType = "LevelControllerType";

        public const string LevelIndicator = "LIX001";

        public const string LevelIndicatorType = "LevelIndicatorType";

        public const string Measurement = "Measurement";

        public const string Output = "Output";

        public const string OutputPipe = "PipeX002";

        public const string Reset = "Reset";

        public const string Resume = "Resume";

        public const string SetPoint = "SetPoint";

        public const string SignalTo = "SignalTo";

        public const string Simulation = "Simulation";

        public const string Start = "Start";

        public const string Suspend = "Suspend";

        public const string UpdateRate = "UpdateRate";

        public const string Valve = "ValveX001";

        public const string ValveType = "ValveType";
    }
    #endregion

    #region Namespace Declarations
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public static partial class Namespaces
    {
        /// <summary>
        /// The URI for the OpcUa namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUa = "http://opcfoundation.org/UA/";

        /// <summary>
        /// The URI for the OpcUaXsd namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUaXsd = "http://opcfoundation.org/UA/2008/02/Types.xsd";

        /// <summary>
        /// The URI for the Boiler namespace (.NET code namespace is 'SampleCompany.NodeManagers.Boiler').
        /// </summary>
        public const string Boiler = "http://samplecompany.com/SampleServer/NodeManagers/Boiler";
    }
    #endregion
}