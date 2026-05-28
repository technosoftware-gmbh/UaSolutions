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
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaConfiguration
{
    /// <summary>
    /// Snapshot and restore interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUaSnapshotRestore<T>
    {
        /// <summary>
        /// Restore
        /// </summary>
        /// <param name="state"></param>
        void Restore(T state);

        /// <summary>
        /// Get state to serialize
        /// </summary>
        /// <returns></returns>
        T Snapshot();
    }
}
