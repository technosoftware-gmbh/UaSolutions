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
using System.Xml;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Threading;
using Opc.Ua;

#pragma warning disable 1591

namespace SampleCompany.NodeManagers.Boiler
{
    #region GenericControllerState Class
    #if (!OPCUA_EXCLUDE_GenericControllerState)
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public partial class GenericControllerState : BaseObjectState
    {
        #region Constructors
        public GenericControllerState(NodeState parent) : base(parent)
        {
        }

        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.NodeManagers.Boiler.ObjectTypes.GenericControllerType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADkAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9C" +
           "b2lsZXL/////BGCAAgEAAAABAB0AAABHZW5lcmljQ29udHJvbGxlclR5cGVJbnN0YW5jZQEB7AMBAewD" +
           "7AMAAP////8DAAAAFWCJCgIAAAABAAsAAABNZWFzdXJlbWVudAEB7QMALgBE7QMAAAAL/////wEB////" +
           "/wAAAAAVYIkKAgAAAAEACAAAAFNldFBvaW50AQHuAwAuAETuAwAAAAv/////AwP/////AAAAABVgiQoC" +
           "AAAAAQAKAAAAQ29udHJvbE91dAEB7wMALgBE7wMAAAAL/////wEB/////wAAAAA=";
        #endregion
        #endif
        #endregion

        #region Public Properties
        public PropertyState<double> Measurement
        {
            get => m_measurement;

            set
            {
                if (!Object.ReferenceEquals(m_measurement, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_measurement = value;
            }
        }

        public PropertyState<double> SetPoint
        {
            get => m_setPoint;

            set
            {
                if (!Object.ReferenceEquals(m_setPoint, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_setPoint = value;
            }
        }

        public PropertyState<double> ControlOut
        {
            get => m_controlOut;

            set
            {
                if (!Object.ReferenceEquals(m_controlOut, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_controlOut = value;
            }
        }
        #endregion

        #region Overridden Methods
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_measurement != null)
            {
                children.Add(m_measurement);
            }

            if (m_setPoint != null)
            {
                children.Add(m_setPoint);
            }

            if (m_controlOut != null)
            {
                children.Add(m_controlOut);
            }

            base.GetChildren(context, children);
        }
            
        protected override void RemoveExplicitlyDefinedChild(BaseInstanceState child)
        {
            if (Object.ReferenceEquals(m_measurement, child))
            {
                m_measurement = null;
                return;
            }

            if (Object.ReferenceEquals(m_setPoint, child))
            {
                m_setPoint = null;
                return;
            }

            if (Object.ReferenceEquals(m_controlOut, child))
            {
                m_controlOut = null;
                return;
            }

            base.RemoveExplicitlyDefinedChild(child);
        }

        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case SampleCompany.NodeManagers.Boiler.BrowseNames.Measurement:
                {
                    if (createOrReplace)
                    {
                        if (Measurement == null)
                        {
                            if (replacement == null)
                            {
                                Measurement = new PropertyState<double>(this);
                            }
                            else
                            {
                                Measurement = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = Measurement;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.SetPoint:
                {
                    if (createOrReplace)
                    {
                        if (SetPoint == null)
                        {
                            if (replacement == null)
                            {
                                SetPoint = new PropertyState<double>(this);
                            }
                            else
                            {
                                SetPoint = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = SetPoint;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.ControlOut:
                {
                    if (createOrReplace)
                    {
                        if (ControlOut == null)
                        {
                            if (replacement == null)
                            {
                                ControlOut = new PropertyState<double>(this);
                            }
                            else
                            {
                                ControlOut = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = ControlOut;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<double> m_measurement;
        private PropertyState<double> m_setPoint;
        private PropertyState<double> m_controlOut;
        #endregion
    }
    #endif
    #endregion

    #region GenericSensorState Class
    #if (!OPCUA_EXCLUDE_GenericSensorState)
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public partial class GenericSensorState : BaseObjectState
    {
        #region Constructors
        public GenericSensorState(NodeState parent) : base(parent)
        {
        }

        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.NodeManagers.Boiler.ObjectTypes.GenericSensorType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADkAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9C" +
           "b2lsZXL/////BGCAAgEAAAABABkAAABHZW5lcmljU2Vuc29yVHlwZUluc3RhbmNlAQHwAwEB8APwAwAA" +
           "/////wEAAAAVYIkKAgAAAAEABgAAAE91dHB1dAEB8QMALwEAQAnxAwAAAAv/////AQH/////AQAAABVg" +
           "iQoCAAAAAAAHAAAARVVSYW5nZQEB9QMALgBE9QMAAAEAdAP/////AQH/////AAAAAA==";
        #endregion
        #endif
        #endregion

        #region Public Properties
        public AnalogItemState<double> Output
        {
            get => m_output;

            set
            {
                if (!Object.ReferenceEquals(m_output, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_output = value;
            }
        }
        #endregion

        #region Overridden Methods
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_output != null)
            {
                children.Add(m_output);
            }

            base.GetChildren(context, children);
        }
            
        protected override void RemoveExplicitlyDefinedChild(BaseInstanceState child)
        {
            if (Object.ReferenceEquals(m_output, child))
            {
                m_output = null;
                return;
            }

            base.RemoveExplicitlyDefinedChild(child);
        }

        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case SampleCompany.NodeManagers.Boiler.BrowseNames.Output:
                {
                    if (createOrReplace)
                    {
                        if (Output == null)
                        {
                            if (replacement == null)
                            {
                                Output = new AnalogItemState<double>(this);
                            }
                            else
                            {
                                Output = (AnalogItemState<double>)replacement;
                            }
                        }
                    }

                    instance = Output;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private AnalogItemState<double> m_output;
        #endregion
    }
    #endif
    #endregion

    #region GenericActuatorState Class
    #if (!OPCUA_EXCLUDE_GenericActuatorState)
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public partial class GenericActuatorState : BaseObjectState
    {
        #region Constructors
        public GenericActuatorState(NodeState parent) : base(parent)
        {
        }

        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.NodeManagers.Boiler.ObjectTypes.GenericActuatorType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADkAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9C" +
           "b2lsZXL/////BGCAAgEAAAABABsAAABHZW5lcmljQWN0dWF0b3JUeXBlSW5zdGFuY2UBAfcDAQH3A/cD" +
           "AAD/////AQAAABVgiQoCAAAAAQAFAAAASW5wdXQBAfgDAC8BAEAJ+AMAAAAL/////wIC/////wEAAAAV" +
           "YIkKAgAAAAAABwAAAEVVUmFuZ2UBAfwDAC4ARPwDAAABAHQD/////wEB/////wAAAAA=";
        #endregion
        #endif
        #endregion

        #region Public Properties
        public AnalogItemState<double> Input
        {
            get => m_input;

            set
            {
                if (!Object.ReferenceEquals(m_input, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_input = value;
            }
        }
        #endregion

        #region Overridden Methods
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_input != null)
            {
                children.Add(m_input);
            }

            base.GetChildren(context, children);
        }
            
        protected override void RemoveExplicitlyDefinedChild(BaseInstanceState child)
        {
            if (Object.ReferenceEquals(m_input, child))
            {
                m_input = null;
                return;
            }

            base.RemoveExplicitlyDefinedChild(child);
        }

        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case SampleCompany.NodeManagers.Boiler.BrowseNames.Input:
                {
                    if (createOrReplace)
                    {
                        if (Input == null)
                        {
                            if (replacement == null)
                            {
                                Input = new AnalogItemState<double>(this);
                            }
                            else
                            {
                                Input = (AnalogItemState<double>)replacement;
                            }
                        }
                    }

                    instance = Input;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private AnalogItemState<double> m_input;
        #endregion
    }
    #endif
    #endregion

    #region CustomControllerState Class
    #if (!OPCUA_EXCLUDE_CustomControllerState)
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public partial class CustomControllerState : BaseObjectState
    {
        #region Constructors
        public CustomControllerState(NodeState parent) : base(parent)
        {
        }

        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.NodeManagers.Boiler.ObjectTypes.CustomControllerType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADkAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9C" +
           "b2lsZXL/////BGCAAgEAAAABABwAAABDdXN0b21Db250cm9sbGVyVHlwZUluc3RhbmNlAQH+AwEB/gP+" +
           "AwAA/////wUAAAAVYIkKAgAAAAEABgAAAElucHV0MQEB/wMALgBE/wMAAAAL/////wIC/////wAAAAAV" +
           "YIkKAgAAAAEABgAAAElucHV0MgEBAAQALgBEAAQAAAAL/////wIC/////wAAAAAVYIkKAgAAAAEABgAA" +
           "AElucHV0MwEBAQQALgBEAQQAAAAL/////wIC/////wAAAAAVYIkKAgAAAAEACgAAAENvbnRyb2xPdXQB" +
           "AQIEAC4ARAIEAAAAC/////8BAf////8AAAAAFWDJCgIAAAAMAAAARGVzY3JpcHRpb25YAQALAAAARGVz" +
           "Y3JpcHRpb24BAQMEAC4ARAMEAAAAFf////8BAf////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        public PropertyState<double> Input1
        {
            get => m_input1;

            set
            {
                if (!Object.ReferenceEquals(m_input1, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_input1 = value;
            }
        }

        public PropertyState<double> Input2
        {
            get => m_input2;

            set
            {
                if (!Object.ReferenceEquals(m_input2, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_input2 = value;
            }
        }

        public PropertyState<double> Input3
        {
            get => m_input3;

            set
            {
                if (!Object.ReferenceEquals(m_input3, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_input3 = value;
            }
        }

        public PropertyState<double> ControlOut
        {
            get => m_controlOut;

            set
            {
                if (!Object.ReferenceEquals(m_controlOut, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_controlOut = value;
            }
        }

        public PropertyState<LocalizedText> DescriptionX
        {
            get => m_descriptionX;

            set
            {
                if (!Object.ReferenceEquals(m_descriptionX, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_descriptionX = value;
            }
        }
        #endregion

        #region Overridden Methods
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_input1 != null)
            {
                children.Add(m_input1);
            }

            if (m_input2 != null)
            {
                children.Add(m_input2);
            }

            if (m_input3 != null)
            {
                children.Add(m_input3);
            }

            if (m_controlOut != null)
            {
                children.Add(m_controlOut);
            }

            if (m_descriptionX != null)
            {
                children.Add(m_descriptionX);
            }

            base.GetChildren(context, children);
        }
            
        protected override void RemoveExplicitlyDefinedChild(BaseInstanceState child)
        {
            if (Object.ReferenceEquals(m_input1, child))
            {
                m_input1 = null;
                return;
            }

            if (Object.ReferenceEquals(m_input2, child))
            {
                m_input2 = null;
                return;
            }

            if (Object.ReferenceEquals(m_input3, child))
            {
                m_input3 = null;
                return;
            }

            if (Object.ReferenceEquals(m_controlOut, child))
            {
                m_controlOut = null;
                return;
            }

            if (Object.ReferenceEquals(m_descriptionX, child))
            {
                m_descriptionX = null;
                return;
            }

            base.RemoveExplicitlyDefinedChild(child);
        }

        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case SampleCompany.NodeManagers.Boiler.BrowseNames.Input1:
                {
                    if (createOrReplace)
                    {
                        if (Input1 == null)
                        {
                            if (replacement == null)
                            {
                                Input1 = new PropertyState<double>(this);
                            }
                            else
                            {
                                Input1 = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = Input1;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.Input2:
                {
                    if (createOrReplace)
                    {
                        if (Input2 == null)
                        {
                            if (replacement == null)
                            {
                                Input2 = new PropertyState<double>(this);
                            }
                            else
                            {
                                Input2 = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = Input2;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.Input3:
                {
                    if (createOrReplace)
                    {
                        if (Input3 == null)
                        {
                            if (replacement == null)
                            {
                                Input3 = new PropertyState<double>(this);
                            }
                            else
                            {
                                Input3 = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = Input3;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.ControlOut:
                {
                    if (createOrReplace)
                    {
                        if (ControlOut == null)
                        {
                            if (replacement == null)
                            {
                                ControlOut = new PropertyState<double>(this);
                            }
                            else
                            {
                                ControlOut = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = ControlOut;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.DescriptionX:
                {
                    if (createOrReplace)
                    {
                        if (DescriptionX == null)
                        {
                            if (replacement == null)
                            {
                                DescriptionX = new PropertyState<LocalizedText>(this);
                            }
                            else
                            {
                                DescriptionX = (PropertyState<LocalizedText>)replacement;
                            }
                        }
                    }

                    instance = DescriptionX;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<double> m_input1;
        private PropertyState<double> m_input2;
        private PropertyState<double> m_input3;
        private PropertyState<double> m_controlOut;
        private PropertyState<LocalizedText> m_descriptionX;
        #endregion
    }
    #endif
    #endregion

    #region ValveState Class
    #if (!OPCUA_EXCLUDE_ValveState)
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public partial class ValveState : GenericActuatorState
    {
        #region Constructors
        public ValveState(NodeState parent) : base(parent)
        {
        }

        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.NodeManagers.Boiler.ObjectTypes.ValveType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADkAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9C" +
           "b2lsZXL/////BGCAAgEAAAABABEAAABWYWx2ZVR5cGVJbnN0YW5jZQEBBAQBAQQEBAQAAP////8BAAAA" +
           "FWCJCgIAAAABAAUAAABJbnB1dAIBAEFCDwAALwEAQAlBQg8AAAv/////AgL/////AQAAABVgiQoCAAAA" +
           "AAAHAAAARVVSYW5nZQIBAEVCDwAALgBERUIPAAEAdAP/////AQH/////AAAAAA==";
        #endregion
        #endif
        #endregion

        #region Public Properties
        #endregion

        #region Overridden Methods
        #endregion

        #region Private Fields
        #endregion
    }
    #endif
    #endregion

    #region LevelControllerState Class
    #if (!OPCUA_EXCLUDE_LevelControllerState)
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public partial class LevelControllerState : GenericControllerState
    {
        #region Constructors
        public LevelControllerState(NodeState parent) : base(parent)
        {
        }

        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.NodeManagers.Boiler.ObjectTypes.LevelControllerType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADkAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9C" +
           "b2lsZXL/////BGCAAgEAAAABABsAAABMZXZlbENvbnRyb2xsZXJUeXBlSW5zdGFuY2UBAQsEAQELBAsE" +
           "AAD/////AwAAABVgiQoCAAAAAQALAAAATWVhc3VyZW1lbnQCAQBHQg8AAC4AREdCDwAAC/////8BAf//" +
           "//8AAAAAFWCJCgIAAAABAAgAAABTZXRQb2ludAIBAEhCDwAALgBESEIPAAAL/////wMD/////wAAAAAV" +
           "YIkKAgAAAAEACgAAAENvbnRyb2xPdXQCAQBJQg8AAC4ARElCDwAAC/////8BAf////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        #endregion

        #region Overridden Methods
        #endregion

        #region Private Fields
        #endregion
    }
    #endif
    #endregion

    #region FlowControllerState Class
    #if (!OPCUA_EXCLUDE_FlowControllerState)
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public partial class FlowControllerState : GenericControllerState
    {
        #region Constructors
        public FlowControllerState(NodeState parent) : base(parent)
        {
        }

        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.NodeManagers.Boiler.ObjectTypes.FlowControllerType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADkAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9C" +
           "b2lsZXL/////BGCAAgEAAAABABoAAABGbG93Q29udHJvbGxlclR5cGVJbnN0YW5jZQEBDwQBAQ8EDwQA" +
           "AP////8DAAAAFWCJCgIAAAABAAsAAABNZWFzdXJlbWVudAIBAEpCDwAALgBESkIPAAAL/////wEB////" +
           "/wAAAAAVYIkKAgAAAAEACAAAAFNldFBvaW50AgEAS0IPAAAuAERLQg8AAAv/////AwP/////AAAAABVg" +
           "iQoCAAAAAQAKAAAAQ29udHJvbE91dAIBAExCDwAALgBETEIPAAAL/////wEB/////wAAAAA=";
        #endregion
        #endif
        #endregion

        #region Public Properties
        #endregion

        #region Overridden Methods
        #endregion

        #region Private Fields
        #endregion
    }
    #endif
    #endregion

    #region LevelIndicatorState Class
    #if (!OPCUA_EXCLUDE_LevelIndicatorState)
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public partial class LevelIndicatorState : GenericSensorState
    {
        #region Constructors
        public LevelIndicatorState(NodeState parent) : base(parent)
        {
        }

        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.NodeManagers.Boiler.ObjectTypes.LevelIndicatorType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADkAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9C" +
           "b2lsZXL/////BGCAAgEAAAABABoAAABMZXZlbEluZGljYXRvclR5cGVJbnN0YW5jZQEBEwQBARMEEwQA" +
           "AP////8BAAAAFWCJCgIAAAABAAYAAABPdXRwdXQCAQBNQg8AAC8BAEAJTUIPAAAL/////wEB/////wEA" +
           "AAAVYIkKAgAAAAAABwAAAEVVUmFuZ2UCAQBRQg8AAC4ARFFCDwABAHQD/////wEB/////wAAAAA=";
        #endregion
        #endif
        #endregion

        #region Public Properties
        #endregion

        #region Overridden Methods
        #endregion

        #region Private Fields
        #endregion
    }
    #endif
    #endregion

    #region FlowTransmitterState Class
    #if (!OPCUA_EXCLUDE_FlowTransmitterState)
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public partial class FlowTransmitterState : GenericSensorState
    {
        #region Constructors
        public FlowTransmitterState(NodeState parent) : base(parent)
        {
        }

        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.NodeManagers.Boiler.ObjectTypes.FlowTransmitterType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADkAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9C" +
           "b2lsZXL/////BGCAAgEAAAABABsAAABGbG93VHJhbnNtaXR0ZXJUeXBlSW5zdGFuY2UBARoEAQEaBBoE" +
           "AAD/////AQAAABVgiQoCAAAAAQAGAAAAT3V0cHV0AgEAU0IPAAAvAQBACVNCDwAAC/////8BAf////8B" +
           "AAAAFWCJCgIAAAAAAAcAAABFVVJhbmdlAgEAV0IPAAAuAERXQg8AAQB0A/////8BAf////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        #endregion

        #region Overridden Methods
        #endregion

        #region Private Fields
        #endregion
    }
    #endif
    #endregion

    #region BoilerStateMachineState Class
    #if (!OPCUA_EXCLUDE_BoilerStateMachineState)
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public partial class BoilerStateMachineState : ProgramStateMachineState
    {
        #region Constructors
        public BoilerStateMachineState(NodeState parent) : base(parent)
        {
        }

        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.NodeManagers.Boiler.ObjectTypes.BoilerStateMachineType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADkAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9C" +
           "b2lsZXL/////BGCAAgEAAAABAB4AAABCb2lsZXJTdGF0ZU1hY2hpbmVUeXBlSW5zdGFuY2UBASEEAQEh" +
           "BCEEAAD/////CwAAABVgiQoCAAAAAAAMAAAAQ3VycmVudFN0YXRlAgEAWUIPAAAvAQDICllCDwAAFf//" +
           "//8BAf////8CAAAAFWCJCgIAAAAAAAIAAABJZAIBAFpCDwAALgBEWkIPAAAR/////wEB/////wAAAAAV" +
           "YIkKAgAAAAAABgAAAE51bWJlcgIBAFxCDwAALgBEXEIPAAAH/////wEB/////wAAAAAVYIkKAgAAAAAA" +
           "DgAAAExhc3RUcmFuc2l0aW9uAgEAXkIPAAAvAQDPCl5CDwAAFf////8BAf////8DAAAAFWCJCgIAAAAA" +
           "AAIAAABJZAIBAF9CDwAALgBEX0IPAAAR/////wEB/////wAAAAAVYIkKAgAAAAAABgAAAE51bWJlcgIB" +
           "AGFCDwAALgBEYUIPAAAH/////wEB/////wAAAAAVYIkKAgAAAAAADgAAAFRyYW5zaXRpb25UaW1lAgEA" +
           "YkIPAAAuAERiQg8AAQAmAf////8BAf////8AAAAAFWCJCgIAAAAAAAkAAABEZWxldGFibGUCAQBnQg8A" +
           "AC4ARGdCDwAAAf////8BAf////8AAAAAFWCJCgIAAAAAAAoAAABBdXRvRGVsZXRlAgEAaEIPAAAuAERo" +
           "Qg8AAAH/////AQH/////AAAAABVgiQoCAAAAAAAMAAAAUmVjeWNsZUNvdW50AgEAaUIPAAAuAERpQg8A" +
           "AAb/////AQH/////AAAAACRhggoEAAAAAQAFAAAAU3RhcnQBAV4EAwAAAABLAAAAQ2F1c2VzIHRoZSBQ" +
           "cm9ncmFtIHRvIHRyYW5zaXRpb24gZnJvbSB0aGUgUmVhZHkgc3RhdGUgdG8gdGhlIFJ1bm5pbmcgc3Rh" +
           "dGUuAC8BAHoJXgQAAAEBAQAAAAA1AQIBAH9CDwAAAAAAJGGCCgQAAAABAAcAAABTdXNwZW5kAQFfBAMA" +
           "AAAATwAAAENhdXNlcyB0aGUgUHJvZ3JhbSB0byB0cmFuc2l0aW9uIGZyb20gdGhlIFJ1bm5pbmcgc3Rh" +
           "dGUgdG8gdGhlIFN1c3BlbmRlZCBzdGF0ZS4ALwEAewlfBAAAAQEBAAAAADUBAgEAhUIPAAAAAAAkYYIK" +
           "BAAAAAEABgAAAFJlc3VtZQEBYAQDAAAAAE8AAABDYXVzZXMgdGhlIFByb2dyYW0gdG8gdHJhbnNpdGlv" +
           "biBmcm9tIHRoZSBTdXNwZW5kZWQgc3RhdGUgdG8gdGhlIFJ1bm5pbmcgc3RhdGUuAC8BAHwJYAQAAAEB" +
           "AQAAAAA1AQEBVgQAAAAAJGGCCgQAAAABAAQAAABIYWx0AQFhBAMAAAAAYAAAAENhdXNlcyB0aGUgUHJv" +
           "Z3JhbSB0byB0cmFuc2l0aW9uIGZyb20gdGhlIFJlYWR5LCBSdW5uaW5nIG9yIFN1c3BlbmRlZCBzdGF0" +
           "ZSB0byB0aGUgSGFsdGVkIHN0YXRlLgAvAQB9CWEEAAABAQMAAAAANQECAQCBQg8AADUBAQFYBAA1AQIB" +
           "AIdCDwAAAAAAJGGCCgQAAAABAAUAAABSZXNldAEBYgQDAAAAAEoAAABDYXVzZXMgdGhlIFByb2dyYW0g" +
           "dG8gdHJhbnNpdGlvbiBmcm9tIHRoZSBIYWx0ZWQgc3RhdGUgdG8gdGhlIFJlYWR5IHN0YXRlLgAvAQB+" +
           "CWIEAAABAQEAAAAANQEBAUwEAAAAADVgiQoCAAAAAQAKAAAAVXBkYXRlUmF0ZQEBYwQDAAAAACYAAABU" +
           "aGUgcmF0ZSBhdCB3aGljaCB0aGUgc2ltdWxhdGlvbiBydW5zLgAuAERjBAAAAAf/////AwP/////AAAA" +
           "AA==";
        #endregion
        #endif
        #endregion

        #region Public Properties
        public PropertyState<uint> UpdateRate
        {
            get => m_updateRate;

            set
            {
                if (!Object.ReferenceEquals(m_updateRate, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_updateRate = value;
            }
        }

        public MethodState Start
        {
            get => m_startMethod;

            set
            {
                if (!Object.ReferenceEquals(m_startMethod, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_startMethod = value;
            }
        }

        public MethodState Suspend
        {
            get => m_suspendMethod;

            set
            {
                if (!Object.ReferenceEquals(m_suspendMethod, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_suspendMethod = value;
            }
        }

        public MethodState Resume
        {
            get => m_resumeMethod;

            set
            {
                if (!Object.ReferenceEquals(m_resumeMethod, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_resumeMethod = value;
            }
        }

        public MethodState Halt
        {
            get => m_haltMethod;

            set
            {
                if (!Object.ReferenceEquals(m_haltMethod, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_haltMethod = value;
            }
        }

        public MethodState Reset
        {
            get => m_resetMethod;

            set
            {
                if (!Object.ReferenceEquals(m_resetMethod, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_resetMethod = value;
            }
        }
        #endregion

        #region Overridden Methods
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_updateRate != null)
            {
                children.Add(m_updateRate);
            }

            if (m_startMethod != null)
            {
                children.Add(m_startMethod);
            }

            if (m_suspendMethod != null)
            {
                children.Add(m_suspendMethod);
            }

            if (m_resumeMethod != null)
            {
                children.Add(m_resumeMethod);
            }

            if (m_haltMethod != null)
            {
                children.Add(m_haltMethod);
            }

            if (m_resetMethod != null)
            {
                children.Add(m_resetMethod);
            }

            base.GetChildren(context, children);
        }
            
        protected override void RemoveExplicitlyDefinedChild(BaseInstanceState child)
        {
            if (Object.ReferenceEquals(m_updateRate, child))
            {
                m_updateRate = null;
                return;
            }

            if (Object.ReferenceEquals(m_startMethod, child))
            {
                m_startMethod = null;
                return;
            }

            if (Object.ReferenceEquals(m_suspendMethod, child))
            {
                m_suspendMethod = null;
                return;
            }

            if (Object.ReferenceEquals(m_resumeMethod, child))
            {
                m_resumeMethod = null;
                return;
            }

            if (Object.ReferenceEquals(m_haltMethod, child))
            {
                m_haltMethod = null;
                return;
            }

            if (Object.ReferenceEquals(m_resetMethod, child))
            {
                m_resetMethod = null;
                return;
            }

            base.RemoveExplicitlyDefinedChild(child);
        }

        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case SampleCompany.NodeManagers.Boiler.BrowseNames.UpdateRate:
                {
                    if (createOrReplace)
                    {
                        if (UpdateRate == null)
                        {
                            if (replacement == null)
                            {
                                UpdateRate = new PropertyState<uint>(this);
                            }
                            else
                            {
                                UpdateRate = (PropertyState<uint>)replacement;
                            }
                        }
                    }

                    instance = UpdateRate;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.Start:
                {
                    if (createOrReplace)
                    {
                        if (Start == null)
                        {
                            if (replacement == null)
                            {
                                Start = new MethodState(this);
                            }
                            else
                            {
                                Start = (MethodState)replacement;
                            }
                        }
                    }

                    instance = Start;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.Suspend:
                {
                    if (createOrReplace)
                    {
                        if (Suspend == null)
                        {
                            if (replacement == null)
                            {
                                Suspend = new MethodState(this);
                            }
                            else
                            {
                                Suspend = (MethodState)replacement;
                            }
                        }
                    }

                    instance = Suspend;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.Resume:
                {
                    if (createOrReplace)
                    {
                        if (Resume == null)
                        {
                            if (replacement == null)
                            {
                                Resume = new MethodState(this);
                            }
                            else
                            {
                                Resume = (MethodState)replacement;
                            }
                        }
                    }

                    instance = Resume;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.Halt:
                {
                    if (createOrReplace)
                    {
                        if (Halt == null)
                        {
                            if (replacement == null)
                            {
                                Halt = new MethodState(this);
                            }
                            else
                            {
                                Halt = (MethodState)replacement;
                            }
                        }
                    }

                    instance = Halt;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.Reset:
                {
                    if (createOrReplace)
                    {
                        if (Reset == null)
                        {
                            if (replacement == null)
                            {
                                Reset = new MethodState(this);
                            }
                            else
                            {
                                Reset = (MethodState)replacement;
                            }
                        }
                    }

                    instance = Reset;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<uint> m_updateRate;
        private MethodState m_startMethod;
        private MethodState m_suspendMethod;
        private MethodState m_resumeMethod;
        private MethodState m_haltMethod;
        private MethodState m_resetMethod;
        #endregion
    }
    #endif
    #endregion

    #region BoilerInputPipeState Class
    #if (!OPCUA_EXCLUDE_BoilerInputPipeState)
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public partial class BoilerInputPipeState : FolderState
    {
        #region Constructors
        public BoilerInputPipeState(NodeState parent) : base(parent)
        {
        }

        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.NodeManagers.Boiler.ObjectTypes.BoilerInputPipeType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADkAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9C" +
           "b2lsZXL/////BGCAAgEAAAABABsAAABCb2lsZXJJbnB1dFBpcGVUeXBlSW5zdGFuY2UBAWQEAQFkBGQE" +
           "AAABAAAAADAAAQFlBAIAAACEYMAKAQAAABAAAABGbG93VHJhbnNtaXR0ZXIxAQAGAAAARlRYMDAxAQFl" +
           "BAAvAQEaBGUEAAABAQAAAAAwAQEBZAQBAAAAFWCJCgIAAAABAAYAAABPdXRwdXQBAWYEAC8BAEAJZgQA" +
           "AAAL/////wEB/////wEAAAAVYIkKAgAAAAAABwAAAEVVUmFuZ2UBAWoEAC4ARGoEAAABAHQD/////wEB" +
           "/////wAAAACEYMAKAQAAAAUAAABWYWx2ZQEACQAAAFZhbHZlWDAwMQEBbAQALwEBBARsBAAAAf////8B" +
           "AAAAFWCJCgIAAAABAAUAAABJbnB1dAEBbQQALwEAQAltBAAAAAv/////AgL/////AQAAABVgiQoCAAAA" +
           "AAAHAAAARVVSYW5nZQEBcQQALgBEcQQAAAEAdAP/////AQH/////AAAAAA==";
        #endregion
        #endif
        #endregion

        #region Public Properties
        public FlowTransmitterState FlowTransmitter1
        {
            get => m_flowTransmitter1;

            set
            {
                if (!Object.ReferenceEquals(m_flowTransmitter1, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_flowTransmitter1 = value;
            }
        }

        public ValveState Valve
        {
            get => m_valve;

            set
            {
                if (!Object.ReferenceEquals(m_valve, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_valve = value;
            }
        }
        #endregion

        #region Overridden Methods
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_flowTransmitter1 != null)
            {
                children.Add(m_flowTransmitter1);
            }

            if (m_valve != null)
            {
                children.Add(m_valve);
            }

            base.GetChildren(context, children);
        }
            
        protected override void RemoveExplicitlyDefinedChild(BaseInstanceState child)
        {
            if (Object.ReferenceEquals(m_flowTransmitter1, child))
            {
                m_flowTransmitter1 = null;
                return;
            }

            if (Object.ReferenceEquals(m_valve, child))
            {
                m_valve = null;
                return;
            }

            base.RemoveExplicitlyDefinedChild(child);
        }

        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case SampleCompany.NodeManagers.Boiler.BrowseNames.FlowTransmitter1:
                {
                    if (createOrReplace)
                    {
                        if (FlowTransmitter1 == null)
                        {
                            if (replacement == null)
                            {
                                FlowTransmitter1 = new FlowTransmitterState(this);
                            }
                            else
                            {
                                FlowTransmitter1 = (FlowTransmitterState)replacement;
                            }
                        }
                    }

                    instance = FlowTransmitter1;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.Valve:
                {
                    if (createOrReplace)
                    {
                        if (Valve == null)
                        {
                            if (replacement == null)
                            {
                                Valve = new ValveState(this);
                            }
                            else
                            {
                                Valve = (ValveState)replacement;
                            }
                        }
                    }

                    instance = Valve;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private FlowTransmitterState m_flowTransmitter1;
        private ValveState m_valve;
        #endregion
    }
    #endif
    #endregion

    #region BoilerDrumState Class
    #if (!OPCUA_EXCLUDE_BoilerDrumState)
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public partial class BoilerDrumState : FolderState
    {
        #region Constructors
        public BoilerDrumState(NodeState parent) : base(parent)
        {
        }

        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.NodeManagers.Boiler.ObjectTypes.BoilerDrumType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADkAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9C" +
           "b2lsZXL/////BGCAAgEAAAABABYAAABCb2lsZXJEcnVtVHlwZUluc3RhbmNlAQFzBAEBcwRzBAAAAQAA" +
           "AAAwAAEBdAQBAAAAhGDACgEAAAAOAAAATGV2ZWxJbmRpY2F0b3IBAAYAAABMSVgwMDEBAXQEAC8BARME" +
           "dAQAAAEBAAAAADABAQFzBAEAAAAVYIkKAgAAAAEABgAAAE91dHB1dAEBdQQALwEAQAl1BAAAAAv/////" +
           "AQH/////AQAAABVgiQoCAAAAAAAHAAAARVVSYW5nZQEBeQQALgBEeQQAAAEAdAP/////AQH/////AAAA" +
           "AA==";
        #endregion
        #endif
        #endregion

        #region Public Properties
        public LevelIndicatorState LevelIndicator
        {
            get => m_levelIndicator;

            set
            {
                if (!Object.ReferenceEquals(m_levelIndicator, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_levelIndicator = value;
            }
        }
        #endregion

        #region Overridden Methods
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_levelIndicator != null)
            {
                children.Add(m_levelIndicator);
            }

            base.GetChildren(context, children);
        }
            
        protected override void RemoveExplicitlyDefinedChild(BaseInstanceState child)
        {
            if (Object.ReferenceEquals(m_levelIndicator, child))
            {
                m_levelIndicator = null;
                return;
            }

            base.RemoveExplicitlyDefinedChild(child);
        }

        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case SampleCompany.NodeManagers.Boiler.BrowseNames.LevelIndicator:
                {
                    if (createOrReplace)
                    {
                        if (LevelIndicator == null)
                        {
                            if (replacement == null)
                            {
                                LevelIndicator = new LevelIndicatorState(this);
                            }
                            else
                            {
                                LevelIndicator = (LevelIndicatorState)replacement;
                            }
                        }
                    }

                    instance = LevelIndicator;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private LevelIndicatorState m_levelIndicator;
        #endregion
    }
    #endif
    #endregion

    #region BoilerOutputPipeState Class
    #if (!OPCUA_EXCLUDE_BoilerOutputPipeState)
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public partial class BoilerOutputPipeState : FolderState
    {
        #region Constructors
        public BoilerOutputPipeState(NodeState parent) : base(parent)
        {
        }

        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.NodeManagers.Boiler.ObjectTypes.BoilerOutputPipeType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADkAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9C" +
           "b2lsZXL/////BGCAAgEAAAABABwAAABCb2lsZXJPdXRwdXRQaXBlVHlwZUluc3RhbmNlAQF7BAEBewR7" +
           "BAAAAQAAAAAwAAEBfAQBAAAAhGDACgEAAAAQAAAARmxvd1RyYW5zbWl0dGVyMgEABgAAAEZUWDAwMgEB" +
           "fAQALwEBGgR8BAAAAQEAAAAAMAEBAXsEAQAAABVgiQoCAAAAAQAGAAAAT3V0cHV0AQF9BAAvAQBACX0E" +
           "AAAAC/////8BAf////8BAAAAFWCJCgIAAAAAAAcAAABFVVJhbmdlAQGBBAAuAESBBAAAAQB0A/////8B" +
           "Af////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        public FlowTransmitterState FlowTransmitter2
        {
            get => m_flowTransmitter2;

            set
            {
                if (!Object.ReferenceEquals(m_flowTransmitter2, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_flowTransmitter2 = value;
            }
        }
        #endregion

        #region Overridden Methods
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_flowTransmitter2 != null)
            {
                children.Add(m_flowTransmitter2);
            }

            base.GetChildren(context, children);
        }
            
        protected override void RemoveExplicitlyDefinedChild(BaseInstanceState child)
        {
            if (Object.ReferenceEquals(m_flowTransmitter2, child))
            {
                m_flowTransmitter2 = null;
                return;
            }

            base.RemoveExplicitlyDefinedChild(child);
        }

        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case SampleCompany.NodeManagers.Boiler.BrowseNames.FlowTransmitter2:
                {
                    if (createOrReplace)
                    {
                        if (FlowTransmitter2 == null)
                        {
                            if (replacement == null)
                            {
                                FlowTransmitter2 = new FlowTransmitterState(this);
                            }
                            else
                            {
                                FlowTransmitter2 = (FlowTransmitterState)replacement;
                            }
                        }
                    }

                    instance = FlowTransmitter2;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private FlowTransmitterState m_flowTransmitter2;
        #endregion
    }
    #endif
    #endregion

    #region BoilerState Class
    #if (!OPCUA_EXCLUDE_BoilerState)
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public partial class BoilerState : BaseObjectState
    {
        #region Constructors
        public BoilerState(NodeState parent) : base(parent)
        {
        }

        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.NodeManagers.Boiler.ObjectTypes.BoilerType, SampleCompany.NodeManagers.Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAADkAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9C" +
           "b2lsZXL/////hGCAAgEAAAABABIAAABCb2lsZXJUeXBlSW5zdGFuY2UBAYMEAQGDBIMEAAABBAAAAAAw" +
           "AAEBhAQAMAABAZMEADAAAQGbBAAkAAEBsQQHAAAAhGDACgEAAAAJAAAASW5wdXRQaXBlAQAIAAAAUGlw" +
           "ZVgwMDEBAYQEAC8BAWQEhAQAAAEDAAAAADABAQGDBAAwAAEBhQQBAekDAAEBkwQCAAAAhGDACgEAAAAQ" +
           "AAAARmxvd1RyYW5zbWl0dGVyMQEABgAAAEZUWDAwMQEBhQQALwEBGgSFBAAAAQEAAAAAMAEBAYQEAQAA" +
           "ABVgiQoCAAAAAQAGAAAAT3V0cHV0AQGGBAAvAQBACYYEAAAAC/////8BAQIAAAABAesDAAEBpAQBAesD" +
           "AAEBrQQBAAAAFWCJCgIAAAAAAAcAAABFVVJhbmdlAQGKBAAuAESKBAAAAQB0A/////8BAf////8AAAAA" +
           "hGDACgEAAAAFAAAAVmFsdmUBAAkAAABWYWx2ZVgwMDEBAYwEAC8BAQQEjAQAAAH/////AQAAABVgiQoC" +
           "AAAAAQAFAAAASW5wdXQBAY0EAC8BAEAJjQQAAAAL/////wICAQAAAAEB6wMBAQGmBAEAAAAVYIkKAgAA" +
           "AAAABwAAAEVVUmFuZ2UBAZEEAC4ARJEEAAABAHQD/////wEB/////wAAAACEYMAKAQAAAAQAAABEcnVt" +
           "AQAIAAAARHJ1bVgwMDEBAZMEAC8BAXMEkwQAAAEEAAAAADABAQGDBAEB6QMBAQGEBAAwAAEBlAQBAeoD" +
           "AAEBmwQBAAAAhGDACgEAAAAOAAAATGV2ZWxJbmRpY2F0b3IBAAYAAABMSVgwMDEBAZQEAC8BARMElAQA" +
           "AAEBAAAAADABAQGTBAEAAAAVYIkKAgAAAAEABgAAAE91dHB1dAEBlQQALwEAQAmVBAAAABr/////AQEB" +
           "AAAAAQHrAwABAagEAQAAABVgiQoCAAAAAAAHAAAARVVSYW5nZQEBmQQALgBEmQQAAAEAdAP/////AQH/" +
           "////AAAAAIRgwAoBAAAACgAAAE91dHB1dFBpcGUBAAgAAABQaXBlWDAwMgEBmwQALwEBewSbBAAAAQMA" +
           "AAAAMAEBAYMEAQHqAwEBAZMEADAAAQGcBAEAAACEYMAKAQAAABAAAABGbG93VHJhbnNtaXR0ZXIyAQAG" +
           "AAAARlRYMDAyAQGcBAAvAQEaBJwEAAABAQAAAAAwAQEBmwQBAAAAFWCJCgIAAAABAAYAAABPdXRwdXQB" +
           "AZ0EAC8BAEAJnQQAAAAL/////wEBAQAAAAEB6wMAAQGuBAEAAAAVYIkKAgAAAAAABwAAAEVVUmFuZ2UB" +
           "AaEEAC4ARKEEAAABAHQD/////wEB/////wAAAAAEYMAKAQAAAA4AAABGbG93Q29udHJvbGxlcgEABgAA" +
           "AEZDWDAwMQEBowQALwEBDwSjBAAA/////wMAAAAVYIkKAgAAAAEACwAAAE1lYXN1cmVtZW50AQGkBAAu" +
           "AESkBAAAAAv/////AQEBAAAAAQHrAwEBAYYEAAAAABVgiQoCAAAAAQAIAAAAU2V0UG9pbnQBAaUEAC4A" +
           "RKUEAAAAC/////8DAwEAAAABAesDAQEBrwQAAAAAFWCJCgIAAAABAAoAAABDb250cm9sT3V0AQGmBAAu" +
           "AESmBAAAAAv/////AQEBAAAAAQHrAwABAY0EAAAAAARgwAoBAAAADwAAAExldmVsQ29udHJvbGxlcgEA" +
           "BgAAAExDWDAwMQEBpwQALwEBCwSnBAAA/////wMAAAAVYIkKAgAAAAEACwAAAE1lYXN1cmVtZW50AQGo" +
           "BAAuAESoBAAAAAv/////AQEBAAAAAQHrAwEBAZUEAAAAABVgiQoCAAAAAQAIAAAAU2V0UG9pbnQBAakE" +
           "AC4ARKkEAAAAC/////8DA/////8AAAAAFWCJCgIAAAABAAoAAABDb250cm9sT3V0AQGqBAAuAESqBAAA" +
           "AAv/////AQEBAAAAAQHrAwABAawEAAAAAARgwAoBAAAAEAAAAEN1c3RvbUNvbnRyb2xsZXIBAAYAAABD" +
           "Q1gwMDEBAasEAC8BAf4DqwQAAP////8FAAAAFWCJCgIAAAABAAYAAABJbnB1dDEBAawEAC4ARKwEAAAA" +
           "C/////8CAgEAAAABAesDAQEBqgQAAAAAFWCJCgIAAAABAAYAAABJbnB1dDIBAa0EAC4ARK0EAAAAC///" +
           "//8CAgEAAAABAesDAQEBhgQAAAAAFWCJCgIAAAABAAYAAABJbnB1dDMBAa4EAC4ARK4EAAAAC/////8C" +
           "AgEAAAABAesDAQEBnQQAAAAAFWCJCgIAAAABAAoAAABDb250cm9sT3V0AQGvBAAuAESvBAAAAAv/////" +
           "AQEBAAAAAQHrAwABAaUEAAAAABVgyQoCAAAADAAAAERlc2NyaXB0aW9uWAEACwAAAERlc2NyaXB0aW9u" +
           "AQGwBAAuAESwBAAAABX/////AQH/////AAAAAIRggAoBAAAAAQAKAAAAU2ltdWxhdGlvbgEBsQQALwEB" +
           "IQSxBAAAAQEAAAAAJAEBAYMECwAAABVgiQoCAAAAAAAMAAAAQ3VycmVudFN0YXRlAQGyBAAvAQDICrIE" +
           "AAAAFf////8BAf////8CAAAAFWCJCgIAAAAAAAIAAABJZAEBswQALgBEswQAAAAR/////wEB/////wAA" +
           "AAAVYIkKAgAAAAAABgAAAE51bWJlcgEBtQQALgBEtQQAAAAH/////wEB/////wAAAAAVYIkKAgAAAAAA" +
           "DgAAAExhc3RUcmFuc2l0aW9uAQG3BAAvAQDPCrcEAAAAFf////8BAf////8DAAAAFWCJCgIAAAAAAAIA" +
           "AABJZAEBuAQALgBEuAQAAAAR/////wEB/////wAAAAAVYIkKAgAAAAAABgAAAE51bWJlcgEBugQALgBE" +
           "ugQAAAAH/////wEB/////wAAAAAVYIkKAgAAAAAADgAAAFRyYW5zaXRpb25UaW1lAQG7BAAuAES7BAAA" +
           "AQAmAf////8BAf////8AAAAAFWCJCgIAAAAAAAkAAABEZWxldGFibGUBAb8EAC4ARL8EAAAAAf////8B" +
           "Af////8AAAAAFWCJCgIAAAAAAAoAAABBdXRvRGVsZXRlAQHABAAuAETABAAAAAH/////AQH/////AAAA" +
           "ABVgiQoCAAAAAAAMAAAAUmVjeWNsZUNvdW50AQHBBAAuAETBBAAAAAb/////AQH/////AAAAADVgiQoC" +
           "AAAAAQAKAAAAVXBkYXRlUmF0ZQEB0AQDAAAAACYAAABUaGUgcmF0ZSBhdCB3aGljaCB0aGUgc2ltdWxh" +
           "dGlvbiBydW5zLgAuAETQBAAAAAf/////AwP/////AAAAACRhggoEAAAAAQAFAAAAU3RhcnQBAdEEAwAA" +
           "AABLAAAAQ2F1c2VzIHRoZSBQcm9ncmFtIHRvIHRyYW5zaXRpb24gZnJvbSB0aGUgUmVhZHkgc3RhdGUg" +
           "dG8gdGhlIFJ1bm5pbmcgc3RhdGUuAC8BAV4E0QQAAAEB/////wAAAAAkYYIKBAAAAAEABwAAAFN1c3Bl" +
           "bmQBAdIEAwAAAABPAAAAQ2F1c2VzIHRoZSBQcm9ncmFtIHRvIHRyYW5zaXRpb24gZnJvbSB0aGUgUnVu" +
           "bmluZyBzdGF0ZSB0byB0aGUgU3VzcGVuZGVkIHN0YXRlLgAvAQFfBNIEAAABAf////8AAAAAJGGCCgQA" +
           "AAABAAYAAABSZXN1bWUBAdMEAwAAAABPAAAAQ2F1c2VzIHRoZSBQcm9ncmFtIHRvIHRyYW5zaXRpb24g" +
           "ZnJvbSB0aGUgU3VzcGVuZGVkIHN0YXRlIHRvIHRoZSBSdW5uaW5nIHN0YXRlLgAvAQFgBNMEAAABAf//" +
           "//8AAAAAJGGCCgQAAAABAAQAAABIYWx0AQHUBAMAAAAAYAAAAENhdXNlcyB0aGUgUHJvZ3JhbSB0byB0" +
           "cmFuc2l0aW9uIGZyb20gdGhlIFJlYWR5LCBSdW5uaW5nIG9yIFN1c3BlbmRlZCBzdGF0ZSB0byB0aGUg" +
           "SGFsdGVkIHN0YXRlLgAvAQFhBNQEAAABAf////8AAAAAJGGCCgQAAAABAAUAAABSZXNldAEB1QQDAAAA" +
           "AEoAAABDYXVzZXMgdGhlIFByb2dyYW0gdG8gdHJhbnNpdGlvbiBmcm9tIHRoZSBIYWx0ZWQgc3RhdGUg" +
           "dG8gdGhlIFJlYWR5IHN0YXRlLgAvAQFiBNUEAAABAf////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        public BoilerInputPipeState InputPipe
        {
            get => m_inputPipe;

            set
            {
                if (!Object.ReferenceEquals(m_inputPipe, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_inputPipe = value;
            }
        }

        public BoilerDrumState Drum
        {
            get => m_drum;

            set
            {
                if (!Object.ReferenceEquals(m_drum, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_drum = value;
            }
        }

        public BoilerOutputPipeState OutputPipe
        {
            get => m_outputPipe;

            set
            {
                if (!Object.ReferenceEquals(m_outputPipe, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_outputPipe = value;
            }
        }

        public FlowControllerState FlowController
        {
            get => m_flowController;

            set
            {
                if (!Object.ReferenceEquals(m_flowController, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_flowController = value;
            }
        }

        public LevelControllerState LevelController
        {
            get => m_levelController;

            set
            {
                if (!Object.ReferenceEquals(m_levelController, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_levelController = value;
            }
        }

        public CustomControllerState CustomController
        {
            get => m_customController;

            set
            {
                if (!Object.ReferenceEquals(m_customController, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_customController = value;
            }
        }

        public BoilerStateMachineState Simulation
        {
            get => m_simulation;

            set
            {
                if (!Object.ReferenceEquals(m_simulation, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_simulation = value;
            }
        }
        #endregion

        #region Overridden Methods
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_inputPipe != null)
            {
                children.Add(m_inputPipe);
            }

            if (m_drum != null)
            {
                children.Add(m_drum);
            }

            if (m_outputPipe != null)
            {
                children.Add(m_outputPipe);
            }

            if (m_flowController != null)
            {
                children.Add(m_flowController);
            }

            if (m_levelController != null)
            {
                children.Add(m_levelController);
            }

            if (m_customController != null)
            {
                children.Add(m_customController);
            }

            if (m_simulation != null)
            {
                children.Add(m_simulation);
            }

            base.GetChildren(context, children);
        }
            
        protected override void RemoveExplicitlyDefinedChild(BaseInstanceState child)
        {
            if (Object.ReferenceEquals(m_inputPipe, child))
            {
                m_inputPipe = null;
                return;
            }

            if (Object.ReferenceEquals(m_drum, child))
            {
                m_drum = null;
                return;
            }

            if (Object.ReferenceEquals(m_outputPipe, child))
            {
                m_outputPipe = null;
                return;
            }

            if (Object.ReferenceEquals(m_flowController, child))
            {
                m_flowController = null;
                return;
            }

            if (Object.ReferenceEquals(m_levelController, child))
            {
                m_levelController = null;
                return;
            }

            if (Object.ReferenceEquals(m_customController, child))
            {
                m_customController = null;
                return;
            }

            if (Object.ReferenceEquals(m_simulation, child))
            {
                m_simulation = null;
                return;
            }

            base.RemoveExplicitlyDefinedChild(child);
        }

        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case SampleCompany.NodeManagers.Boiler.BrowseNames.InputPipe:
                {
                    if (createOrReplace)
                    {
                        if (InputPipe == null)
                        {
                            if (replacement == null)
                            {
                                InputPipe = new BoilerInputPipeState(this);
                            }
                            else
                            {
                                InputPipe = (BoilerInputPipeState)replacement;
                            }
                        }
                    }

                    instance = InputPipe;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.Drum:
                {
                    if (createOrReplace)
                    {
                        if (Drum == null)
                        {
                            if (replacement == null)
                            {
                                Drum = new BoilerDrumState(this);
                            }
                            else
                            {
                                Drum = (BoilerDrumState)replacement;
                            }
                        }
                    }

                    instance = Drum;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.OutputPipe:
                {
                    if (createOrReplace)
                    {
                        if (OutputPipe == null)
                        {
                            if (replacement == null)
                            {
                                OutputPipe = new BoilerOutputPipeState(this);
                            }
                            else
                            {
                                OutputPipe = (BoilerOutputPipeState)replacement;
                            }
                        }
                    }

                    instance = OutputPipe;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.FlowController:
                {
                    if (createOrReplace)
                    {
                        if (FlowController == null)
                        {
                            if (replacement == null)
                            {
                                FlowController = new FlowControllerState(this);
                            }
                            else
                            {
                                FlowController = (FlowControllerState)replacement;
                            }
                        }
                    }

                    instance = FlowController;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.LevelController:
                {
                    if (createOrReplace)
                    {
                        if (LevelController == null)
                        {
                            if (replacement == null)
                            {
                                LevelController = new LevelControllerState(this);
                            }
                            else
                            {
                                LevelController = (LevelControllerState)replacement;
                            }
                        }
                    }

                    instance = LevelController;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.CustomController:
                {
                    if (createOrReplace)
                    {
                        if (CustomController == null)
                        {
                            if (replacement == null)
                            {
                                CustomController = new CustomControllerState(this);
                            }
                            else
                            {
                                CustomController = (CustomControllerState)replacement;
                            }
                        }
                    }

                    instance = CustomController;
                    break;
                }

                case SampleCompany.NodeManagers.Boiler.BrowseNames.Simulation:
                {
                    if (createOrReplace)
                    {
                        if (Simulation == null)
                        {
                            if (replacement == null)
                            {
                                Simulation = new BoilerStateMachineState(this);
                            }
                            else
                            {
                                Simulation = (BoilerStateMachineState)replacement;
                            }
                        }
                    }

                    instance = Simulation;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private BoilerInputPipeState m_inputPipe;
        private BoilerDrumState m_drum;
        private BoilerOutputPipeState m_outputPipe;
        private FlowControllerState m_flowController;
        private LevelControllerState m_levelController;
        private CustomControllerState m_customController;
        private BoilerStateMachineState m_simulation;
        #endregion
    }
    #endif
    #endregion
}