#region Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved

#region Using Directives

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// Second part of the <see cref="UaStandardNodeManager"/>
    /// Implementation of the <see cref="IUaCallAsyncNodeManager"/> interface for custom node management.
    /// The interface is not implmented by default to ensure consistent behaviour in existing NodeManagers, but can be implemented in a derived class.
    /// </summary>
    public partial class UaStandardNodeManager
    {
        /// <summary>
        /// Asycnhronously calls a method defined on an object.
        /// </summary>
        public virtual ValueTask CallAsync(
            UaServerOperationContext context,
            IList<CallMethodRequest> methodsToCall,
            IList<CallMethodResult> results,
            IList<ServiceResult> errors,
            CancellationToken cancellationToken = default)
        {
            return CallInternalAsync(
                context,
                methodsToCall,
                results,
                errors,
                sync: false,
                cancellationToken);
        }
    }
}
