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

using System;

using Opc.Ua;

#endregion Using Directives

namespace Technosoftware.ClientGateway.Hda
{
    /// <summary>
    /// Stores the history of an HDA item.
    /// </summary>
    /// <exclude />
    internal class HdaItemHistoryData
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HdaItemHistoryData"/> class.
        /// </summary>
        public HdaItemHistoryData()
        {
        }
        #endregion Constructors

        #region Public Members
        /// <summary>
        /// Gets or sets the server handle.
        /// </summary>
        /// <value>The server handle.</value>
        public int ServerHandle
        {
            get { return m_serverHandle; }
            set { m_serverHandle = value; }
        }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        public int Error
        {
            get { return m_error; }
            set { m_error = value; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object[] Values
        {
            get { return m_values; }
            set { m_values = value; }
        }

        /// <summary>
        /// Gets or sets the qualities.
        /// </summary>
        /// <value>The qualities.</value>
        public int[] Qualities
        {
            get { return m_qualities; }
            set { m_qualities = value; }
        }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        public DateTime[] Timestamps
        {
            get { return m_timestamps; }
            set { m_timestamps = value; }
        }

        /// <summary>
        /// Gets or sets the modifications.
        /// </summary>
        /// <value>The modifications.</value>
        public ModificationInfo[] Modifications
        {
            get { return m_modifications; }
            set { m_modifications = value; }
        }
        #endregion Public Members

        #region Private Fields
        private int m_serverHandle;
        private object[] m_values;
        private int[] m_qualities;
        private DateTime[] m_timestamps;
        private ModificationInfo[] m_modifications;
        private int m_error;
        #endregion Private Fields
    }
}
