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
using System.Collections.Generic;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.ClientGateway.Ae
{
    /// <summary>
    /// A object which maps a segment to a UA object.
    /// </summary>
    /// <exclude />
    internal class AeAreaState : FolderState
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AeAreaState"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="qualifiedName">The qualified name for the area.</param>
        /// <param name="name">The name of the area.</param>
        /// <param name="namespaceIndex">Index of the namespace.</param>
        /// <param name="telemetry"></param>
        public AeAreaState(
            ISystemContext context,
            string qualifiedName,
            string name,
            ushort namespaceIndex,
            ITelemetryContext telemetry)
        :
            base(null)
        {
            m_qualifiedName = qualifiedName;

            this.SymbolicName = name;
            this.TypeDefinitionId = Opc.Ua.ObjectTypeIds.FolderType;
            this.NodeId = AeModelUtils.ConstructIdForArea(qualifiedName, namespaceIndex);
            this.BrowseName = new QualifiedName(name, namespaceIndex);
            this.DisplayName = this.BrowseName.Name;
            this.Description = null;
            this.WriteMask = 0;
            this.UserWriteMask = 0;
            this.EventNotifier = EventNotifiers.SubscribeToEvents;
            m_telemetry = telemetry;
        }
        #endregion Constructors

        #region Public Properties
        /// <summary>
        /// Gets the qualified name for the area.
        /// </summary>
        /// <value>The qualified name for the area.</value>
        public string QualifiedName
        {
            get { return m_qualifiedName; }
        }
        #endregion Public Properties

        #region Overridden Methods
        /// <summary>
        /// Creates a browser that finds the references to the branch.
        /// </summary>
        /// <param name="context">The system context to use.</param>
        /// <param name="view">The view which may restrict the set of references/nodes found.</param>
        /// <param name="referenceType">The type of references being followed.</param>
        /// <param name="includeSubtypes">Whether subtypes of the reference type are followed.</param>
        /// <param name="browseDirection">Which way the references are being followed.</param>
        /// <param name="browseName">The browse name of a specific target (used when translating browse paths).</param>
        /// <param name="additionalReferences">Any additional references that should be included.</param>
        /// <param name="internalOnly">If true the browser should not making blocking calls to external systems.</param>
        /// <returns>The browse object (must be disposed).</returns>
        public override INodeBrowser CreateBrowser(
            ISystemContext context,
            ViewDescription view,
            NodeId referenceType,
            bool includeSubtypes,
            BrowseDirection browseDirection,
            QualifiedName browseName,
            IEnumerable<IReference> additionalReferences,
            bool internalOnly)
        {
            NodeBrowser browser = new AeAreaBrower(
                context,
                view,
                referenceType,
                includeSubtypes,
                browseDirection,
                browseName,
                additionalReferences,
                internalOnly,
                m_qualifiedName,
                this.NodeId.NamespaceIndex,
                m_telemetry);

            PopulateBrowser(context, browser);

            return browser;
        }
        #endregion Overridden Methods

        #region Private Fields
        private string m_qualifiedName;
        private readonly ITelemetryContext m_telemetry;
        #endregion Private Fields
    }
}
