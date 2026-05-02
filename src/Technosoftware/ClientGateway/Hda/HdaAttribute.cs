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



#endregion Using Directives

namespace Technosoftware.ClientGateway.Hda
{
    /// <summary>
    /// Stores information about an HDA attribute.
    /// </summary>
    internal class HdaAttribute
    {
        #region Public Members
        /// <summary>
        /// Initializes a new instance of the <see cref="HdaAttribute"/> class.
        /// </summary>
        public HdaAttribute()
        {
        }
        #endregion Public Members

        #region Public Members
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public uint Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public short DataType
        {
            get { return m_dataType; }
            set { m_dataType = value; }
        }
        #endregion Public Members

        #region Private Fields
        private uint m_id;
        private string m_name;
        private string m_description;
        private short m_dataType;
        #endregion Private Fields
    }
}
