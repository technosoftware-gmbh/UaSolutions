#region Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved
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
#endregion Copyright (c) 2011-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System.Collections.Generic;
using Opc.Ua;
using Technosoftware.UaServer;
#endregion Using Directives

namespace Technosoftware.ClientGateway.Ae
{
    /// <summary>
    /// Browses the children of a segment.
    /// </summary>
    /// <exclude />
    internal class AeAreaBrower : NodeBrowser
    {
        #region Constructors
        /// <summary>
        /// Creates a new browser object with a set of filters.
        /// </summary>
        /// <param name="context">The system context to use.</param>
        /// <param name="view">The view which may restrict the set of references/nodes found.</param>
        /// <param name="referenceType">The type of references being followed.</param>
        /// <param name="includeSubtypes">Whether subtypes of the reference type are followed.</param>
        /// <param name="browseDirection">Which way the references are being followed.</param>
        /// <param name="browseName">The browse name of a specific target (used when translating browse paths).</param>
        /// <param name="additionalReferences">Any additional references that should be included.</param>
        /// <param name="internalOnly">If true the browser should not making blocking calls to external systems.</param>
        /// <param name="qualifiedName">Name of the qualified.</param>
        /// <param name="namespaceIndex">Index of the namespace.</param>
        /// <param name="telemetry"></param>
        public AeAreaBrower(
            ISystemContext context,
            ViewDescription view,
            NodeId referenceType,
            bool includeSubtypes,
            BrowseDirection browseDirection,
            QualifiedName browseName,
            IEnumerable<IReference> additionalReferences,
            bool internalOnly,
            string qualifiedName,
            ushort namespaceIndex,
            ITelemetryContext telemetry)
        :
            base(
                context,
                view,
                referenceType,
                includeSubtypes,
                browseDirection,
                browseName,
                additionalReferences,
                internalOnly)
        {
            m_qualifiedName = qualifiedName;
            m_namespaceIndex = namespaceIndex;
            m_stage = Stage.Begin;
            m_telemetry = telemetry;
            //m_logger = telemetry.CreateLogger<AeAreaBrower>();
        }
        #endregion Constructors

        #region Overridden Methods
        /// <summary>
        /// Returns the next reference.
        /// </summary>
        /// <returns>The next reference that meets the browse criteria.</returns>
        public override IReference Next()
        {
            lock (DataLock)
            {
                // enumerate pre-defined references.
                // always call first to ensure any pushed-back references are returned first.
                IReference reference = base.Next();

                if (reference != null)
                {
                    return reference;
                }

                // don't start browsing huge number of references when only internal references are requested.
                if (InternalOnly)
                {
                    return null;
                }

                // fetch references from the server.
                do
                {
                    // fetch next reference.
                    reference = NextChild();

                    if (reference != null)
                    {
                        return reference;
                    }

                    // go to the next stage.
                    NextStage();
                }
                while (m_stage != Stage.Done);

                // all done.
                return null;
            }
        }
        #endregion Overridden Methods

        #region Private Methods
        /// <summary>
        /// Returns the next child.
        /// </summary>
        private IReference NextChild()
        {
            // check if a specific browse name is requested.
            if (QualifiedName.IsNull(BrowseName))
            {
                return NextChild(m_stage);
            }

            // keep fetching references until a matching browse name if found.
            NodeStateReference reference;
            do
            {
                reference = NextChild(m_stage);

                if (reference != null)
                {
                    // need to let the caller look up the browse name.
                    if (reference.Target == null)
                    {
                        return reference;
                    }

                    // check for browse name match.
                    if (reference.Target.BrowseName == BrowseName)
                    {
                        return reference;
                    }
                }
            }
            while (reference != null);

            // no match - need to go onto the next stage.
            return null;
        }

        /// <summary>
        /// Returns the next child.
        /// </summary>
        private NodeStateReference NextChild(Stage stage)
        {
            // fetch children.
            if (stage == Stage.Children)
            {
                if (m_browser == null)
                {
                    return null;
                }

                BaseObjectState node = m_browser.Next(SystemContext, m_namespaceIndex);

                if (node != null)
                {
                    return new NodeStateReference(ReferenceTypeIds.HasNotifier, false, node.NodeId);
                }

                // all done.
                // return null;
            }

            // fetch child parents.
            //if (stage == Stage.Parents)
            //{
            //    return null;
            //}

            return null;
        }

        /// <summary>
        /// Initializes the next stage of browsing.
        /// </summary>
        private void NextStage()
        {
            var system = (ComAeClientManager)SystemContext.SystemHandle;
            ComAeClient client = system.SelectClient((UaServerContext)SystemContext, false);

            // determine which stage is next based on the reference types requested.
            for (Stage next = m_stage + 1; next <= Stage.Done; next++)
            {
                if (next == Stage.Children)
                {
                    if (IsRequired(ReferenceTypeIds.HasNotifier, false))
                    {
                        m_stage = next;
                        break;
                    }
                }
                else if (next == Stage.Parents)
                {
                    if (IsRequired(ReferenceTypeIds.HasNotifier, true))
                    {
                        m_stage = next;
                        break;
                    }
                }
                else if (next == Stage.Done)
                {
                    m_stage = next;
                    break;
                }
            }

            // start enumerating areas.
            if (m_stage == Stage.Children)
            {
                m_browser = new ComAeBrowserClient(client, m_qualifiedName, m_telemetry);
                //return;
            }

            // start enumerating parents.
            //if (m_stage == Stage.Parents)
            //{
            //    return;
            //}

            // all done.
        }
        #endregion Private Methods

        #region Stage Enumeration
        /// <summary>
        /// The stages available in a browse operation.
        /// </summary>
        private enum Stage
        {
            Begin,
            Children,
            Parents,
            Done
        }
        #endregion Stage Enumeration

        #region Private Fields
        private Stage m_stage;
        private readonly string m_qualifiedName;
        private readonly ushort m_namespaceIndex;
        private ComAeBrowserClient m_browser;
        private readonly ITelemetryContext m_telemetry;
        #endregion Private Fields
    }
}
