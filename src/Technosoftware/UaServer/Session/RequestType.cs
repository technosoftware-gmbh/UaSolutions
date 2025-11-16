#region Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is subject to the Technosoftware GmbH Software License 
// Agreement, which can be found here:
// https://technosoftware.com/documents/Source_License_Agreement.pdf
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaServer
{
    /// <summary>
    /// The set of all service request types (used for collecting diagnostics and checking permissions).
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// The request type is not known.
        /// </summary>
        Unknown,

        /// <summary>
        /// <see cref="IDiscoveryServer.FindServers" />
        /// </summary>
        FindServers,

        /// <summary>
        /// <see cref="IDiscoveryServer.GetEndpoints" />
        /// </summary>
        GetEndpoints,

        /// <summary>
        /// <see cref="ISessionServer.CreateSession" />
        /// </summary>
        CreateSession,

        /// <summary>
        /// <see cref="ISessionServer.ActivateSession" />
        /// </summary>
        ActivateSession,

        /// <summary>
        /// <see cref="ISessionServer.CloseSession" />
        /// </summary>
        CloseSession,

        /// <summary>
        /// <see cref="ISessionServer.Cancel" />
        /// </summary>
        Cancel,

        /// <summary>
        /// <see cref="ISessionServer.Read" />
        /// </summary>
        Read,

        /// <summary>
        /// <see cref="ISessionServer.HistoryRead" />
        /// </summary>
        HistoryRead,

        /// <summary>
        /// <see cref="ISessionServer.Write" />
        /// </summary>
        Write,

        /// <summary>
        /// <see cref="ISessionServer.HistoryUpdate" />
        /// </summary>
        HistoryUpdate,

        /// <summary>
        /// <see cref="ISessionServer.Call" />
        /// </summary>
        Call,

        /// <summary>
        /// <see cref="ISessionServer.CreateMonitoredItems" />
        /// </summary>
        CreateMonitoredItems,

        /// <summary>
        /// <see cref="ISessionServer.ModifyMonitoredItems" />
        /// </summary>
        ModifyMonitoredItems,

        /// <summary>
        /// <see cref="ISessionServer.SetMonitoringMode" />
        /// </summary>
        SetMonitoringMode,

        /// <summary>
        /// <see cref="ISessionServer.SetTriggering" />
        /// </summary>
        SetTriggering,

        /// <summary>
        /// <see cref="ISessionServer.DeleteMonitoredItems" />
        /// </summary>
        DeleteMonitoredItems,

        /// <summary>
        /// <see cref="ISessionServer.CreateSubscription" />
        /// </summary>
        CreateSubscription,

        /// <summary>
        /// <see cref="ISessionServer.ModifySubscription" />
        /// </summary>
        ModifySubscription,

        /// <summary>
        /// <see cref="ISessionServer.SetPublishingMode" />
        /// </summary>
        SetPublishingMode,

        /// <summary>
        /// <see cref="ISessionServer.Publish" />
        /// </summary>
        Publish,

        /// <summary>
        /// <see cref="ISessionServer.Republish" />
        /// </summary>
        Republish,

        /// <summary>
        /// <see cref="ISessionServer.TransferSubscriptions" />
        /// </summary>
        TransferSubscriptions,

        /// <summary>
        /// <see cref="ISessionServer.DeleteSubscriptions" />
        /// </summary>
        DeleteSubscriptions,

        /// <summary>
        /// <see cref="ISessionServer.AddNodes" />
        /// </summary>
        AddNodes,

        /// <summary>
        /// <see cref="ISessionServer.AddReferences" />
        /// </summary>
        AddReferences,

        /// <summary>
        /// <see cref="ISessionServer.DeleteNodes" />
        /// </summary>
        DeleteNodes,

        /// <summary>
        /// <see cref="ISessionServer.DeleteReferences" />
        /// </summary>
        DeleteReferences,

        /// <summary>
        /// <see cref="ISessionServer.Browse" />
        /// </summary>
        Browse,

        /// <summary>
        /// <see cref="ISessionServer.BrowseNext" />
        /// </summary>
        BrowseNext,

        /// <summary>
        /// <see cref="ISessionServer.TranslateBrowsePathsToNodeIds" />
        /// </summary>
        TranslateBrowsePathsToNodeIds,

        /// <summary>
        /// <see cref="ISessionServer.QueryFirst" />
        /// </summary>
        QueryFirst,

        /// <summary>
        /// <see cref="ISessionServer.QueryNext" />
        /// </summary>
        QueryNext,

        /// <summary>
        /// <see cref="ISessionServer.RegisterNodes" />
        /// </summary>
        RegisterNodes,

        /// <summary>
        /// <see cref="ISessionServer.UnregisterNodes" />
        /// </summary>
        UnregisterNodes
    }
}
