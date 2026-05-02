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

namespace Technosoftware.ClientGateway.Da
{
    /// <summary>
    /// Stores information an element in the DA server address space.
    /// </summary>
    internal class DaElement
    {
        #region Public Members
        /// <summary>
        /// Initializes a new instance of the <see cref="DaElement"/> class.
        /// </summary>
        public DaElement()
        {
        }
        #endregion Public Members

        #region Public Members
        /// <summary>
        /// Gets or sets the item id.
        /// </summary>
        /// <value>The item id.</value>
        public string ItemId
        {
            get { return m_itemId; }
            set { m_itemId = value; }
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
        /// Gets or sets the parent item id.
        /// </summary>
        /// <value>The parent item id.</value>
        public string ParentId
        {
            get { return m_parentId; }
            set { m_parentId = value; }
        }

        /// <summary>
        /// Gets or sets the type of the element.
        /// </summary>
        /// <value>The type of the element.</value>
        public DaElementType ElementType
        {
            get { return m_elementType; }
            set { m_elementType = value; }
        }

        /// <summary>
        /// Gets or sets the properties available for the element.
        /// </summary>
        /// <value>The available properties.</value>
        public DaProperty[] Properties
        {
            get { return m_properties; }
            set { m_properties = value; }
        }

        /// <summary>
        /// Gets or sets the COM data type for the value.
        /// </summary>
        /// <value>The COM data type for the value.</value>
        public short DataType
        {
            get { return m_dataType; }
            set { m_dataType = value; }
        }

        /// <summary>
        /// Gets or sets the access rights for the item.
        /// </summary>
        /// <value>The access rights.</value>
        public int AccessRights
        {
            get { return m_accessRights; }
            set { m_accessRights = value; }
        }

        /// <summary>
        /// Gets or sets the scan rate.
        /// </summary>
        /// <value>The scan rate.</value>
        public float ScanRate
        {
            get { return m_scanRate; }
            set { m_scanRate = value; }
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
        /// Gets or sets the engineering units.
        /// </summary>
        /// <value>The engineering units.</value>
        public string EngineeringUnits
        {
            get { return m_engineeringUnits; }
            set { m_engineeringUnits = value; }
        }

        /// <summary>
        /// Gets or sets EUType forthe item.
        /// </summary>
        /// <value>The EUType for the item.</value>
        public int EuType
        {
            get { return m_euType; }
            set { m_euType = value; }
        }

        /// <summary>
        /// Gets or sets the EU information for the item.
        /// </summary>
        /// <value>The EU information for the item.</value>
        public string[] EuInfo
        {
            get { return m_euInfo; }
            set { m_euInfo = value; }
        }

        /// <summary>
        /// Gets or sets the high EU value.
        /// </summary>
        /// <value>The high EU value.</value>
        public double HighEU
        {
            get { return m_highEU; }
            set { m_highEU = value; }
        }

        /// <summary>
        /// Gets or sets the low EU value.
        /// </summary>
        /// <value>The low EU value.</value>
        public double LowEU
        {
            get { return m_lowEU; }
            set { m_lowEU = value; }
        }

        /// <summary>
        /// Gets or sets the high IR value.
        /// </summary>
        /// <value>The high IR value.</value>
        public double HighIR
        {
            get { return m_highIR; }
            set { m_highIR = value; }
        }

        /// <summary>
        /// Gets or sets the low IR value.
        /// </summary>
        /// <value>The low IR value.</value>
        public double LowIR
        {
            get { return m_lowIR; }
            set { m_lowIR = value; }
        }

        /// <summary>
        /// Gets or sets the open label.
        /// </summary>
        /// <value>The open label.</value>
        public string OpenLabel
        {
            get { return m_openLabel; }
            set { m_openLabel = value; }
        }

        /// <summary>
        /// Gets or sets the close label.
        /// </summary>
        /// <value>The close label.</value>
        public string CloseLabel
        {
            get { return m_closeLabel; }
            set { m_closeLabel = value; }
        }

        /// <summary>
        /// Gets or sets the time zone for the item.
        /// </summary>
        /// <value>The time zone.</value>
        public int? TimeZone
        {
            get { return m_timeZone; }
            set { m_timeZone = value; }
        }

        /// <summary>
        /// Returns true if the element supports the specified property.
        /// </summary>
        /// <param name="propertyId">The property id.</param>
        /// <returns>Rrue if the element supports the specified property.</returns>
        public bool SupportsProperty(int propertyId)
        {
            if (m_properties != null)
            {
                for (int ii = 0; ii < m_properties.Length; ii++)
                {
                    if (propertyId == m_properties[ii].PropertyId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion Public Members

        #region Private Methods
        #endregion Private Methods

        #region Private Fields
        private string m_itemId;
        private string m_name;
        private string m_parentId;
        private DaElementType m_elementType;
        private DaProperty[] m_properties;
        private short m_dataType;
        private int m_accessRights;
        private float m_scanRate;
        private string m_description;
        private string m_engineeringUnits;
        private int m_euType;
        private string[] m_euInfo;
        private double m_highEU;
        private double m_lowEU;
        private double m_highIR;
        private double m_lowIR;
        private string m_openLabel;
        private string m_closeLabel;
        private int? m_timeZone;
        #endregion Private Fields
    }

    /// <summary>
    /// The possible type of elements.
    /// </summary>
    public enum DaElementType
    {
        /// <summary>
        /// Unknown element type.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// A branch
        /// </summary>
        Branch = 1,

        /// <summary>
        /// An item. 
        /// </summary>
        Item = 2,

        /// <summary>
        /// An analog item.
        /// </summary>
        AnalogItem = 3,

        /// <summary>
        /// An enumerated item.
        /// </summary>
        EnumeratedItem = 4,

        /// <summary>
        /// An digital item.
        /// </summary>
        DigitalItem = 5
    }
}
