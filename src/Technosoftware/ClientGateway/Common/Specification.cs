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

namespace Technosoftware.Common
{
    /// <summary>
    ///     A description of an interface version defined by an OPC specification.
    /// </summary>
    public struct Specification
    {
        #region Specification Constants

        /// <summary>OPC Alarms&amp;Events 1.0 and OPC ALarms&amp;Events 1.1.</summary>
        public static readonly Specification Ae10 = new Specification("58E13251-AC87-11d1-84D5-00608CB8A7E9",
            "Alarms and Event 1.XX");

        /// <summary>OPC Data Access 1.0.</summary>
        public static readonly Specification Da10 = new Specification("63D5F430-CFE4-11d1-B2C8-0060083BA1FB",
            "Data Access 1.0a");

        /// <summary>OPC Data Access 2.0.</summary>
        public static readonly Specification Da20 = new Specification("63D5F432-CFE4-11d1-B2C8-0060083BA1FB",
            "Data Access 2.XX");

        /// <summary>OPC Data Access 3.0.</summary>
        public static readonly Specification Da30 = new Specification("CC603642-66D7-48f1-B69A-B625E73652D7",
            "Data Access 3.00");

        /// <summary>OPC Historical Data Access 1.0.</summary>
        public static readonly Specification Hda10 = new Specification("7DE5B060-E089-11d2-A5E6-000086339399",
            "Historical Data Access 1.XX");

        /// <summary>OPC UA 1.0.</summary>
        public static readonly Specification Ua10 = new Specification("EC10AFD8-9BC0-4828-B47E-B3D907F929B1",
            "Unified Architecture 1.0");

        #endregion Specification Constants

        #region Fields

        private string description_;
        private string id_;

        #endregion Fields

        #region Constructors, Destructor, Initialization

        /// <summary>
        ///     Initializes the object with the description and a GUID as a string.
        /// </summary>
        public Specification(string id, string description)
        {
            id_ = id;
            description_ = description;
        }

        #endregion Constructors, Destructor, Initialization

        #region Properties

        /// <summary>
        ///     The unique identifier for the interface version.
        /// </summary>
        public string Id
        {
            get { return id_; }
            internal set { id_ = value; }
        }

        /// <summary>
        ///     The human readable description for the interface version.
        /// </summary>
        public string Description
        {
            get { return description_; }
            internal set { description_ = value; }
        }

        #endregion Properties

        #region Comparison Operators

        /// <summary>
        ///     Determines if the object is equal to the specified value.
        /// </summary>
        public override bool Equals(object target)
        {
            if (target is Specification)
            {
                return (Id == ((Specification)target).Id);
            }

            return false;
        }

        /// <summary>
        ///     Converts the object to a string used for display.
        /// </summary>
        public override string ToString()
        {
            return Description;
        }

        /// <summary>
        ///     Returns a suitable hash code for the result.
        /// </summary>
        public override int GetHashCode()
        {
            return (Id != null) ? Id.GetHashCode() : base.GetHashCode();
        }

        /// <summary>
        ///     Returns true if the objects are equal.
        /// </summary>
        public static bool operator ==(Specification a, Specification b)
        {
            return a.Equals(b);
        }

        /// <summary>
        ///     Returns true if the objects are not equal.
        /// </summary>
        public static bool operator !=(Specification a, Specification b)
        {
            return !a.Equals(b);
        }

        #endregion Comparison Operators
    }
}
