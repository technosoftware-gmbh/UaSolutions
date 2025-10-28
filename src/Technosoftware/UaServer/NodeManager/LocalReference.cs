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

#endregion

namespace Technosoftware.UaServer.NodeManager
{
    /// <summary>
    /// Stores a reference between NodeManagers that is needs to be created or deleted.
    /// </summary>
    public class LocalReference
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Initializes the reference.
        /// </summary>
        public LocalReference(
            NodeId sourceId,
            NodeId referenceTypeId,
            bool isInverse,
            NodeId targetId)
        {
            m_sourceId = sourceId;
            m_referenceTypeId = referenceTypeId;
            m_isInverse = isInverse;
            m_targetId = targetId;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// The source of the reference.
        /// </summary>
        public NodeId SourceId
        {
            get { return m_sourceId; }
        }

        /// <summary>
        /// The type of reference.
        /// </summary>
        public NodeId ReferenceTypeId
        {
            get { return m_referenceTypeId; }
        }

        /// <summary>
        /// True if the reference is an inverse reference.
        /// </summary>
        public bool IsInverse
        {
            get { return m_isInverse; }
        }

        /// <summary>
        /// The target of the reference.
        /// </summary>
        public NodeId TargetId
        {
            get { return m_targetId; }
        }
        #endregion

        #region Private Fields
        private NodeId m_sourceId;
        private NodeId m_referenceTypeId;
        private bool m_isInverse;
        private NodeId m_targetId;
        #endregion
    }
}