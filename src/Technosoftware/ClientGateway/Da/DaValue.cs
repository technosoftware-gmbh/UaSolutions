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

namespace Technosoftware.ClientGateway.Da
{
    /// <summary>
    /// Stores information an element in the DA server address space.
    /// </summary>
    internal class DaValue
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DaValue"/> class.
        /// </summary>
        public DaValue()
        {
        }
        #endregion Constructors

        #region Public Members
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
        /// Gets or sets the quality.
        /// </summary>
        /// <value>The quality.</value>
        public short Quality
        {
            get { return m_quality; }
            set { m_quality = value; }
        }

        /// <summary>
        /// Gets or sets the COM error.
        /// </summary>
        /// <value>The COM error.</value>
        public int Error
        {
            get { return m_error; }
            set { m_error = value; }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of value to return.</typeparam>
        /// <returns>The value if no error and a valid value exists. The default value for the type otherwise.</returns>
        public T GetValue<T>()
        {
            if (m_error < 0)
            {
                return default(T);
            }

            if (typeof(T).IsInstanceOfType(m_value))
            {
                return (T)m_value;
            }

            return default(T);
        }
        #endregion Public Members

        #region Private Fields
        private object m_value;
        private short m_quality;
        private DateTime m_timestamp;
        private int m_error;
        #endregion Private Fields
    }
}
