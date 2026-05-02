#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
// Web: http://www.technosoftware.com
//
// The Software is based on the OPC Foundation’s software and is subject to 
// the OPC Foundation MIT License 1.00, which can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//
// The Software is subject to the Technosoftware GmbH Software License Agreement,
// which can be found here:
// https://technosoftware.com/license-agreement/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved


#region Using Directives

using Opc.Ua;

using Technosoftware.UaServer;
using Technosoftware.Rcw;

#endregion Using Directives

namespace Technosoftware.ClientGateway.Ae
{
    /// <summary>
    /// A object which represents a COM AE condition.
    /// </summary>
    internal partial class AeConditionState : AlarmConditionState
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AeConditionState"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="handle">The handle.</param>
        /// <param name="acknowledgeMethod">The acknowledge method.</param>
        public AeConditionState(ISystemContext context, UaNodeHandle handle, AddCommentMethodState acknowledgeMethod)
        :
            base(null)
        {
            AeParsedNodeId parsedNodeId = (AeParsedNodeId)handle.ParsedNodeId;

            this.NodeId = handle.NodeId;

            this.TypeDefinitionId = AeParsedNodeId.Construct(
                Constants.CONDITION_EVENT,
                parsedNodeId.CategoryId,
                parsedNodeId.ConditionName,
                parsedNodeId.NamespaceIndex);

            this.Acknowledge = acknowledgeMethod;
            this.AddChild(acknowledgeMethod);
        }
        #endregion Constructors

        #region Public Properties
        #endregion Public Properties

        #region Private Fields
        #endregion Private Fields
    }
}
