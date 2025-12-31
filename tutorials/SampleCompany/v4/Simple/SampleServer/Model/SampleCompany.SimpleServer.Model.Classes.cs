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

namespace SampleCompany.SimpleServer.Model
{
    #region SystemCycleStatusEventState Class
    #if (!OPCUA_EXCLUDE_SystemCycleStatusEventState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class SystemCycleStatusEventState : SystemEventState
    {
        #region Constructors
        /// <remarks />
        public SystemCycleStatusEventState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.SimpleServer.Model.ObjectTypes.SystemCycleStatusEventType, SampleCompany.SimpleServer.Model.Namespaces.SimpleServer, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACsAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2ltcGxlU2VydmVyL01vZGVs/////wRggAIB" +
           "AAAAAQAiAAAAU3lzdGVtQ3ljbGVTdGF0dXNFdmVudFR5cGVJbnN0YW5jZQEBAgABAQIAAgAAAP////8K" +
           "AAAAFWCJCgIAAAAAAAcAAABFdmVudElkAgEAQUIPAAAuAERBQg8AAA//////AQH/////AAAAABVgiQoC" +
           "AAAAAAAJAAAARXZlbnRUeXBlAgEAQkIPAAAuAERCQg8AABH/////AQH/////AAAAABVgiQoCAAAAAAAK" +
           "AAAAU291cmNlTm9kZQIBAENCDwAALgBEQ0IPAAAR/////wEB/////wAAAAAVYIkKAgAAAAAACgAAAFNv" +
           "dXJjZU5hbWUCAQBEQg8AAC4ARERCDwAADP////8BAf////8AAAAAFWCJCgIAAAAAAAQAAABUaW1lAgEA" +
           "RUIPAAAuAERFQg8AAQAmAf////8BAf////8AAAAAFWCJCgIAAAAAAAsAAABSZWNlaXZlVGltZQIBAEZC" +
           "DwAALgBERkIPAAEAJgH/////AQH/////AAAAABVgiQoCAAAAAAAHAAAATWVzc2FnZQIBAEhCDwAALgBE" +
           "SEIPAAAV/////wEB/////wAAAAAVYIkKAgAAAAAACAAAAFNldmVyaXR5AgEASUIPAAAuAERJQg8AAAX/" +
           "////AQH/////AAAAABVgiQoCAAAAAQAHAAAAQ3ljbGVJZAEBDAAALgBEDAAAAAAM/////wEB/////wAA" +
           "AAAVYIkKAgAAAAEACwAAAEN1cnJlbnRTdGVwAQENAAAuAEQNAAAAAQEBAP////8BAf////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public PropertyState<string> CycleId
        {
            get
            {
                return m_cycleId;
            }

            set
            {
                if (!Object.ReferenceEquals(m_cycleId, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_cycleId = value;
            }
        }

        /// <remarks />
        public PropertyState<CycleStepDataType> CurrentStep
        {
            get
            {
                return m_currentStep;
            }

            set
            {
                if (!Object.ReferenceEquals(m_currentStep, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_currentStep = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_cycleId != null)
            {
                children.Add(m_cycleId);
            }

            if (m_currentStep != null)
            {
                children.Add(m_currentStep);
            }

            base.GetChildren(context, children);
        }
            
        /// <remarks />
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
                case SampleCompany.SimpleServer.Model.BrowseNames.CycleId:
                {
                    if (createOrReplace)
                    {
                        if (CycleId == null)
                        {
                            if (replacement == null)
                            {
                                CycleId = new PropertyState<string>(this);
                            }
                            else
                            {
                                CycleId = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = CycleId;
                    break;
                }

                case SampleCompany.SimpleServer.Model.BrowseNames.CurrentStep:
                {
                    if (createOrReplace)
                    {
                        if (CurrentStep == null)
                        {
                            if (replacement == null)
                            {
                                CurrentStep = new PropertyState<CycleStepDataType>(this);
                            }
                            else
                            {
                                CurrentStep = (PropertyState<CycleStepDataType>)replacement;
                            }
                        }
                    }

                    instance = CurrentStep;
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
        private PropertyState<string> m_cycleId;
        private PropertyState<CycleStepDataType> m_currentStep;
        #endregion
    }
    #endif
    #endregion

    #region SystemCycleStartedEventState Class
    #if (!OPCUA_EXCLUDE_SystemCycleStartedEventState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class SystemCycleStartedEventState : SystemCycleStatusEventState
    {
        #region Constructors
        /// <remarks />
        public SystemCycleStartedEventState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.SimpleServer.Model.ObjectTypes.SystemCycleStartedEventType, SampleCompany.SimpleServer.Model.Namespaces.SimpleServer, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACsAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2ltcGxlU2VydmVyL01vZGVs/////wRggAIB" +
           "AAAAAQAjAAAAU3lzdGVtQ3ljbGVTdGFydGVkRXZlbnRUeXBlSW5zdGFuY2UBAQ4AAQEOAA4AAAD/////" +
           "CwAAABVgiQoCAAAAAAAHAAAARXZlbnRJZAIBAEpCDwAALgBESkIPAAAP/////wEB/////wAAAAAVYIkK" +
           "AgAAAAAACQAAAEV2ZW50VHlwZQIBAEtCDwAALgBES0IPAAAR/////wEB/////wAAAAAVYIkKAgAAAAAA" +
           "CgAAAFNvdXJjZU5vZGUCAQBMQg8AAC4ARExCDwAAEf////8BAf////8AAAAAFWCJCgIAAAAAAAoAAABT" +
           "b3VyY2VOYW1lAgEATUIPAAAuAERNQg8AAAz/////AQH/////AAAAABVgiQoCAAAAAAAEAAAAVGltZQIB" +
           "AE5CDwAALgBETkIPAAEAJgH/////AQH/////AAAAABVgiQoCAAAAAAALAAAAUmVjZWl2ZVRpbWUCAQBP" +
           "Qg8AAC4ARE9CDwABACYB/////wEB/////wAAAAAVYIkKAgAAAAAABwAAAE1lc3NhZ2UCAQBRQg8AAC4A" +
           "RFFCDwAAFf////8BAf////8AAAAAFWCJCgIAAAAAAAgAAABTZXZlcml0eQIBAFJCDwAALgBEUkIPAAAF" +
           "/////wEB/////wAAAAAVYIkKAgAAAAEABwAAAEN5Y2xlSWQCAQBTQg8AAC4ARFNCDwAADP////8BAf//" +
           "//8AAAAAFWCJCgIAAAABAAsAAABDdXJyZW50U3RlcAIBAFRCDwAALgBEVEIPAAEBAQD/////AQH/////" +
           "AAAAABdgiQoCAAAAAQAFAAAAU3RlcHMBARoAAC4ARBoAAAABAQEAAQAAAAEAAAAAAAAAAQH/////AAAA" +
           "AA==";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public PropertyState<CycleStepDataType[]> Steps
        {
            get
            {
                return m_steps;
            }

            set
            {
                if (!Object.ReferenceEquals(m_steps, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_steps = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_steps != null)
            {
                children.Add(m_steps);
            }

            base.GetChildren(context, children);
        }
            
        /// <remarks />
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
                case SampleCompany.SimpleServer.Model.BrowseNames.Steps:
                {
                    if (createOrReplace)
                    {
                        if (Steps == null)
                        {
                            if (replacement == null)
                            {
                                Steps = new PropertyState<CycleStepDataType[]>(this);
                            }
                            else
                            {
                                Steps = (PropertyState<CycleStepDataType[]>)replacement;
                            }
                        }
                    }

                    instance = Steps;
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
        private PropertyState<CycleStepDataType[]> m_steps;
        #endregion
    }
    #endif
    #endregion

    #region SystemCycleAbortedEventState Class
    #if (!OPCUA_EXCLUDE_SystemCycleAbortedEventState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class SystemCycleAbortedEventState : SystemCycleStatusEventState
    {
        #region Constructors
        /// <remarks />
        public SystemCycleAbortedEventState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.SimpleServer.Model.ObjectTypes.SystemCycleAbortedEventType, SampleCompany.SimpleServer.Model.Namespaces.SimpleServer, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACsAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2ltcGxlU2VydmVyL01vZGVs/////wRggAIB" +
           "AAAAAQAjAAAAU3lzdGVtQ3ljbGVBYm9ydGVkRXZlbnRUeXBlSW5zdGFuY2UBARsAAQEbABsAAAD/////" +
           "CwAAABVgiQoCAAAAAAAHAAAARXZlbnRJZAIBAFVCDwAALgBEVUIPAAAP/////wEB/////wAAAAAVYIkK" +
           "AgAAAAAACQAAAEV2ZW50VHlwZQIBAFZCDwAALgBEVkIPAAAR/////wEB/////wAAAAAVYIkKAgAAAAAA" +
           "CgAAAFNvdXJjZU5vZGUCAQBXQg8AAC4ARFdCDwAAEf////8BAf////8AAAAAFWCJCgIAAAAAAAoAAABT" +
           "b3VyY2VOYW1lAgEAWEIPAAAuAERYQg8AAAz/////AQH/////AAAAABVgiQoCAAAAAAAEAAAAVGltZQIB" +
           "AFlCDwAALgBEWUIPAAEAJgH/////AQH/////AAAAABVgiQoCAAAAAAALAAAAUmVjZWl2ZVRpbWUCAQBa" +
           "Qg8AAC4ARFpCDwABACYB/////wEB/////wAAAAAVYIkKAgAAAAAABwAAAE1lc3NhZ2UCAQBcQg8AAC4A" +
           "RFxCDwAAFf////8BAf////8AAAAAFWCJCgIAAAAAAAgAAABTZXZlcml0eQIBAF1CDwAALgBEXUIPAAAF" +
           "/////wEB/////wAAAAAVYIkKAgAAAAEABwAAAEN5Y2xlSWQCAQBeQg8AAC4ARF5CDwAADP////8BAf//" +
           "//8AAAAAFWCJCgIAAAABAAsAAABDdXJyZW50U3RlcAIBAF9CDwAALgBEX0IPAAEBAQD/////AQH/////" +
           "AAAAABVgiQoCAAAAAQAFAAAARXJyb3IBAScAAC4ARCcAAAAAE/////8BAf////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public PropertyState<StatusCode> Error
        {
            get
            {
                return m_error;
            }

            set
            {
                if (!Object.ReferenceEquals(m_error, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_error = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_error != null)
            {
                children.Add(m_error);
            }

            base.GetChildren(context, children);
        }
            
        /// <remarks />
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
                case SampleCompany.SimpleServer.Model.BrowseNames.Error:
                {
                    if (createOrReplace)
                    {
                        if (Error == null)
                        {
                            if (replacement == null)
                            {
                                Error = new PropertyState<StatusCode>(this);
                            }
                            else
                            {
                                Error = (PropertyState<StatusCode>)replacement;
                            }
                        }
                    }

                    instance = Error;
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
        private PropertyState<StatusCode> m_error;
        #endregion
    }
    #endif
    #endregion

    #region SystemCycleFinishedEventState Class
    #if (!OPCUA_EXCLUDE_SystemCycleFinishedEventState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class SystemCycleFinishedEventState : SystemCycleStatusEventState
    {
        #region Constructors
        /// <remarks />
        public SystemCycleFinishedEventState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(SampleCompany.SimpleServer.Model.ObjectTypes.SystemCycleFinishedEventType, SampleCompany.SimpleServer.Model.Namespaces.SimpleServer, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACsAAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2ltcGxlU2VydmVyL01vZGVs/////wRggAIB" +
           "AAAAAQAkAAAAU3lzdGVtQ3ljbGVGaW5pc2hlZEV2ZW50VHlwZUluc3RhbmNlAQEoAAEBKAAoAAAA////" +
           "/woAAAAVYIkKAgAAAAAABwAAAEV2ZW50SWQCAQBgQg8AAC4ARGBCDwAAD/////8BAf////8AAAAAFWCJ" +
           "CgIAAAAAAAkAAABFdmVudFR5cGUCAQBhQg8AAC4ARGFCDwAAEf////8BAf////8AAAAAFWCJCgIAAAAA" +
           "AAoAAABTb3VyY2VOb2RlAgEAYkIPAAAuAERiQg8AABH/////AQH/////AAAAABVgiQoCAAAAAAAKAAAA" +
           "U291cmNlTmFtZQIBAGNCDwAALgBEY0IPAAAM/////wEB/////wAAAAAVYIkKAgAAAAAABAAAAFRpbWUC" +
           "AQBkQg8AAC4ARGRCDwABACYB/////wEB/////wAAAAAVYIkKAgAAAAAACwAAAFJlY2VpdmVUaW1lAgEA" +
           "ZUIPAAAuAERlQg8AAQAmAf////8BAf////8AAAAAFWCJCgIAAAAAAAcAAABNZXNzYWdlAgEAZ0IPAAAu" +
           "AERnQg8AABX/////AQH/////AAAAABVgiQoCAAAAAAAIAAAAU2V2ZXJpdHkCAQBoQg8AAC4ARGhCDwAA" +
           "Bf////8BAf////8AAAAAFWCJCgIAAAABAAcAAABDeWNsZUlkAgEAaUIPAAAuAERpQg8AAAz/////AQH/" +
           "////AAAAABVgiQoCAAAAAQALAAAAQ3VycmVudFN0ZXACAQBqQg8AAC4ARGpCDwABAQEA/////wEB////" +
           "/wAAAAA=";
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
}