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
using Opc.Ua;
#endregion Using Directives

namespace SampleCompany.NodeManagers.TestData
{
    public partial class ScalarStructureVariableState : ITestDataSystemValuesGenerator
    {
        /// <summary>
        /// Initializes the object as a collection of counters which change value on read.
        /// </summary>
        protected override void OnAfterCreate(ISystemContext context, NodeState node)
        {
            base.OnAfterCreate(context, node);

            InitializeVariable(context, BooleanValue);
            InitializeVariable(context, SByteValue);
            InitializeVariable(context, ByteValue);
            InitializeVariable(context, Int16Value);
            InitializeVariable(context, UInt16Value);
            InitializeVariable(context, Int32Value);
            InitializeVariable(context, UInt32Value);
            InitializeVariable(context, Int64Value);
            InitializeVariable(context, UInt64Value);
            InitializeVariable(context, FloatValue);
            InitializeVariable(context, DoubleValue);
            InitializeVariable(context, StringValue);
            InitializeVariable(context, DateTimeValue);
            InitializeVariable(context, GuidValue);
            InitializeVariable(context, ByteStringValue);
            InitializeVariable(context, XmlElementValue);
            InitializeVariable(context, NodeIdValue);
            InitializeVariable(context, ExpandedNodeIdValue);
            InitializeVariable(context, QualifiedNameValue);
            InitializeVariable(context, LocalizedTextValue);
            InitializeVariable(context, StatusCodeValue);
            InitializeVariable(context, VariantValue);
            InitializeVariable(context, EnumerationValue);
            InitializeVariable(context, StructureValue);
            InitializeVariable(context, NumberValue);
            InitializeVariable(context, IntegerValue);
            InitializeVariable(context, UIntegerValue);
        }

        /// <summary>
        /// Initializes the variable.
        /// </summary>
        protected void InitializeVariable(ISystemContext context, BaseVariableState variable)
        {
            // set a valid initial value.
            _ = context.SystemHandle as TestDataSystem;

            // copy access level to childs
            variable.AccessLevel = AccessLevel;
            variable.UserAccessLevel = UserAccessLevel;
        }

        public virtual StatusCode OnGenerateValues(ISystemContext context)
        {
            if (context.SystemHandle is not TestDataSystem system)
            {
                return StatusCodes.BadOutOfService;
            }

            byte accessLevel = AccessLevel;
            byte userAccessLevel = UserAccessLevel;
            AccessLevel = UserAccessLevel = AccessLevels.CurrentReadOrWrite;

            // generate structure values here
            ServiceResult result = WriteValueAttribute(
                context,
                NumericRange.Empty,
                system.ReadValue(this),
                StatusCodes.Good,
                DateTime.UtcNow);

            AccessLevel = accessLevel;
            UserAccessLevel = userAccessLevel;

            ClearChangeMasks(context, true);

            return result.StatusCode;
        }
    }
}
