#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on https://github.com/junian/Standard.Licensing. 
// The complete license agreement for that can be found in this directore in the LICENSE.txt file.
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// Represents a dictionary of m_license attributes.
    /// </summary>
    public class LicenseAttributes
    {
        /// <summary>
        /// The XML Data
        /// </summary>
        protected readonly XElement xmlData;

        /// <summary>
        /// The child name
        /// </summary>
        protected readonly XName ChildName;

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseAttributes"/> class.
        /// </summary>
        internal LicenseAttributes(XElement xmlData, XName childName)
        {
            this.xmlData = xmlData ?? new XElement("null");
            ChildName = childName;
        }

        /// <summary>
        /// Adds a new element with the specified key and value
        /// to the collection.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value of the element.</param>
        public virtual void Add(string key, string value)
        {
            SetChildTag(key, value);
        }

        /// <summary>
        /// Adds all new element into the collection.
        /// </summary>
        /// <param name="features">The dictionary of elements.</param>
        public virtual void AddAll(IDictionary<string, string> features)
        {
            foreach (KeyValuePair<string, string> feature in features)
            {
                Add(feature.Key, feature.Value);
            }
        }

        /// <summary>
        /// Removes a element with the specified key
        /// from the collection.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        public virtual void Remove(string key)
        {
            XElement element =
                xmlData.Elements(ChildName)
                    .FirstOrDefault(e => e.Attribute("name") != null && e.Attribute("name").Value == key);

            if (element != null)
            {
                element.Remove();
            }
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        public virtual void RemoveAll()
        {
            xmlData.RemoveAll();
        }

        /// <summary>
        /// Gets the value of a element with the
        /// specified key.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <returns>The value of the element if available; otherwise null.</returns>
        public virtual string Get(string key)
        {
            return GetChildTag(key);
        }

        /// <summary>
        /// Gets all elements.
        /// </summary>
        /// <returns>A dictionary of all elements in this collection.</returns>
        public virtual IDictionary<string, string> GetAll()
        {
            return xmlData.Elements(ChildName).ToDictionary(e => e.Attribute("name").Value, e => e.Value);
        }

        /// <summary>
        /// Determines whether the specified element is in
        /// this collection.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <returns>true if the collection contains this element; otherwise false.</returns>
        public virtual bool Contains(string key)
        {
            return xmlData.Elements(ChildName).Any(e => e.Attribute("name") != null && e.Attribute("name").Value == key);
        }

        /// <summary>
        /// Determines whether all specified elements are in
        /// this collection.
        /// </summary>
        /// <param name="keys">The list of keys of the elements.</param>
        /// <returns>true if the collection contains all specified elements; otherwise false.</returns>
        public virtual bool ContainsAll(string[] keys)
        {
            return xmlData.Elements(ChildName).All(e => e.Attribute("name") != null && keys.Contains(e.Attribute("name").Value));
        }

        /// <summary>
        /// Set the tag value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected virtual void SetTag(string name, string value)
        {
            XElement element = xmlData.Element(name);

            if (element == null)
            {
                element = new XElement(name);
                xmlData.Add(element);
            }

            if (value != null)
            {
                element.Value = value;
            }
        }

        /// <summary>
        /// Set the child tag value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected virtual void SetChildTag(string name, string value)
        {
            XElement element =
                xmlData.Elements(ChildName)
                    .FirstOrDefault(e => e.Attribute("name") != null && e.Attribute("name").Value == name);

            if (element == null)
            {
                element = new XElement(ChildName);
                element.Add(new XAttribute("name", name));
                xmlData.Add(element);
            }

            if (value != null)
            {
                element.Value = value;
            }
        }

        /// <summary>
        /// Get the tag value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual string GetTag(string name)
        {
            XElement element = xmlData.Element(name);
            return element != null ? element.Value : null;
        }

        /// <summary>
        /// Get the child tag value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual string GetChildTag(string name)
        {
            XElement element =
                xmlData.Elements(ChildName)
                    .FirstOrDefault(e => e.Attribute("name") != null && e.Attribute("name").Value == name);

            return element != null ? element.Value : null;
        }
    }
}
