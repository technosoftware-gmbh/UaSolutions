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

#endregion Using Directives

namespace Technosoftware.ClientGateway.Hda
{
    /// <summary>
    /// Stores the value of an HDA attribute.
    /// </summary>
    internal class HdaAttributeValue
    {
        #region Public Members
        /// <summary>
        /// Initializes a new instance of the <see cref="HdaAttributeValue"/> class.
        /// </summary>
        public HdaAttributeValue()
        {
        }
        #endregion Public Members

        #region Public Members
        /// <summary>
        /// Gets or sets the attribute id.
        /// </summary>
        /// <value>The attribute id.</value>
        public uint AttributeId
        {
            get { return m_attributeId; }
            set { m_attributeId = value; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        public DateTime Timestamp
        {
            get { return m_timestamp; }
            set { m_timestamp = value; }
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
        #endregion Public Members

        #region Private Fields
        private uint m_attributeId;
        private object m_value;
        private DateTime m_timestamp;
        private int m_error;
        #endregion Private Fields
    }
}
