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
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// A software license
    /// </summary>
    public class License
    {
        #region Constants
        private const string kProductFeatureNameVersion = "Version";
        private const string kAttributeNameProductIdentity = "Product Identity";
        private const string kAttributeNameAssemblyIdentity = "Assembly Identity";
        #endregion Constants

        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="License"/> class.
        /// </summary>
        internal License()
        {
            m_xmlData = new XElement("License");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="License"/> class
        /// with the specified content.
        /// </summary>
        /// <remarks>This constructor is only used for loading from XML.</remarks>
        /// <param name="xmlData">The initial content of this <see cref="License"/>.</param>
        internal License(XElement xmlData)
        {
            m_xmlData = xmlData;
        }
        #endregion Constructors, Destructor, Initialization

        #region Properties
        /// <summary>
        /// Gets or sets the unique identifier of this <see cref="License"/>.
        /// </summary>
        public Guid Id
        {
            get => new(GetTag("Id") ?? Guid.Empty.ToString());
            set
            {
                if (!IsSigned)
                {
                    SetTag("Id", value.ToString());
                }
            }
        }

        /// <summary>
        /// Gets or set the <see cref="LicenseLevel"/> of this <see cref="License"/>.
        /// </summary>
        public LicenseType LicenseLevel
        {
            get
            {
                if (LicenseInfo == null)
                {
                    string s = GetTag("Type");
                    if (string.IsNullOrEmpty(s))
                    {
                        return LicenseType.Trial;
                    }
                    return LicenseType.CompanySite;
                }
                Enum.TryParse(LicenseInfo.Level ?? nameof(LicenseType.Trial), false, out LicenseType result);
                return result;
            }
            set
            {
                if (!IsSigned)
                {
                    LicenseInfo.Level = value.ToString();
                }
            }
        }

        /// <summary>
        /// Gets or set the <see cref="ProductType"/> of this <see cref="License"/>.
        /// </summary>
        public ProductType ProductType
        {
            get
            {
                if (Product == null)
                {
                    return ProductType.ClientAndServer;
                }
                Enum.TryParse(Product.Type ?? nameof(ProductType.ClientAndServer), false, out ProductType result);
                return result;
            }
            set
            {
                if (!IsSigned)
                {
                    Product.Type = value.ToString();
                }
            }
        }

        /// <summary>
        /// The product name.
        /// </summary>
        public string ProductName
        {
            get
            {
                if (Product == null)
                {
                    return "OPC UA Solutions";
                }
                return Product.Name;
            }
            set
            {
                if (!IsSigned)
                {
                    Product.Name = value;
                }
            }
        }

        /// <summary>
        /// The version of the product.
        /// </summary>
        public Version ProductVersion
        {
            get
            {
                LicenseAttributes features;
                if (OldProductFeatures != null)
                {
                    features = OldProductFeatures;
                    IsNewLicenseType = false;
                    string s = features.Get(kProductFeatureNameVersion);
                    if (!string.IsNullOrEmpty(s))
                    {
                        return new Version(s);
                    }
                }
                else if (LicensedFeatures != null)
                {
                    IsNewLicenseType = true;
                }
                else
                {
                    return null;
                }
                if (Product != null && !string.IsNullOrEmpty(Product.Version))
                {
                    return new Version(Product.Version);
                }
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the expiration date of this <see cref="License"/>.
        /// Use this property to set the expiration date for a license.
        /// </summary>
        public DateTime LicenseExpirationDate
        {
            get
            {
                if (LicenseInfo == null || string.IsNullOrEmpty(LicenseInfo.ExpirationDate))
                {
                    return DateTime.ParseExact(
                        DateTime.MaxValue.ToUniversalTime().ToString("ddd, dd MMM yyyy", CultureInfo.InvariantCulture)
                        , "ddd, dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }
                return DateTime.ParseExact(
                    LicenseInfo.ExpirationDate ??
                    DateTime.MaxValue.ToUniversalTime().ToString("ddd, dd MMM yyyy", CultureInfo.InvariantCulture)
                    , "ddd, dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }
            set
            {
                if (!IsSigned)
                {
                    LicenseInfo.ExpirationDate = value.ToString("ddd, dd MMM yyyy", CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of supported included in this <see cref="License"/>.
        /// </summary>
        public SupportLevel SupportLevel
        {
            get
            {
                if (SupportInfo == null)
                {
                    return SupportLevel.None;
                }
                Enum.TryParse(SupportInfo.Level ?? nameof(SupportLevel.None), false, out SupportLevel result);
                return result;
            }

            set
            {
                if (!IsSigned)
                {
                    SupportInfo.Level = value.ToString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the expiration date of the support for this <see cref="License"/>.
        /// Use this property to set the expiration date of support and subscription
        /// updates for a license
        /// </summary>
        public DateTime SupportExpirationDate
        {
            get
            {
                if (SupportInfo == null || string.IsNullOrEmpty(SupportInfo.ExpirationDate))
                {
                    return DateTime.ParseExact(
                        DateTime.MaxValue.ToUniversalTime().ToString("ddd, dd MMM yyyy", CultureInfo.InvariantCulture)
                        , "ddd, dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                }
                return DateTime.ParseExact(
                    SupportInfo.ExpirationDate ??
                    DateTime.MaxValue.ToUniversalTime().ToString("ddd, dd MMM yyyy", CultureInfo.InvariantCulture)
                    , "ddd, dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }
            set
            {
                if (!IsSigned)
                {
                    SupportInfo.ExpirationDate = value.ToString("ddd, dd MMM yyyy", CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>
        /// The date the product was published.
        /// </summary>
        public DateTime? PublishedDate
        {
            get
            {
                if (!string.IsNullOrEmpty(Product.PublishedDate))
                {
                    IsNewLicenseType = true;
                    return DateTime.Parse(Product.PublishedDate, CultureInfo.InvariantCulture);
                }
                IsNewLicenseType = false;
                return null;
            }
        }

        /// <summary>
        /// The product identity.
        /// </summary>
        public string ProductIdentity => AdditionalAttributes.Get(kAttributeNameProductIdentity);

        /// <summary>
        /// The product identity.
        /// </summary>
        public string AssemblyIdentity => AdditionalAttributes.Get(kAttributeNameAssemblyIdentity);

        /// <summary>
        /// Whether the license was issued before 1-OCT-2025 (V4.0.4) or not.
        /// </summary>
        public bool IsNewLicenseType { get; private set; }

        /// <summary>
        /// Gets or sets the product features of this <see cref="License"/>.
        /// </summary>
        internal LicenseAttributes OldProductFeatures
        {
            get
            {
                XElement xmlElement = m_xmlData.Element("ProductFeatures");

                if (!IsSigned && xmlElement == null)
                {
                    m_xmlData.Add(new XElement("ProductFeatures"));
                    xmlElement = m_xmlData.Element("ProductFeatures");
                }
                else if (IsSigned && xmlElement == null)
                {
                    return null;
                }

                return new LicenseAttributes(xmlElement, "Feature");
            }
        }

        /// <summary>
        /// Gets or sets the licensed features of this <see cref="License"/>.
        /// </summary>
        public LicenseAttributes LicensedFeatures
        {
            get
            {
                XElement xmlElement = m_xmlData.Element("Features");

                if (!IsSigned && xmlElement == null)
                {
                    m_xmlData.Add(new XElement("Features"));
                    xmlElement = m_xmlData.Element("Features");
                }
                else if (IsSigned && xmlElement == null)
                {
                    xmlElement = m_xmlData.Element("ProductFeatures");
                    return new LicenseAttributes(xmlElement, "ProductFeatures");
                }
                return new LicenseAttributes(xmlElement, "Feature");
            }
        }

        /// <summary>
        /// Gets the <see cref="LicenseInfo"/> of this <see cref="License"/>.
        /// </summary>
        public LicenseInfo LicenseInfo
        {
            get
            {
                XElement xmlElement = m_xmlData.Element("License");

                if (!IsSigned && xmlElement == null)
                {
                    m_xmlData.Add(new XElement("License"));
                    xmlElement = m_xmlData.Element("License");
                }
                else if (IsSigned && xmlElement == null)
                {
                    return null;
                }

                return new LicenseInfo(xmlElement);
            }
        }

        /// <summary>
        /// Gets the <see cref="SupportLevel"/> of this <see cref="License"/>.
        /// </summary>
        public Support SupportInfo
        {
            get
            {
                XElement xmlElement = m_xmlData.Element("Support");

                if (!IsSigned && xmlElement == null)
                {
                    m_xmlData.Add(new XElement("Support"));
                    xmlElement = m_xmlData.Element("Support");
                }
                else if (IsSigned && xmlElement == null)
                {
                    return null;
                }

                return new Support(xmlElement);
            }
        }

        /// <summary>
        /// Gets the <see cref="Customer"/> of this <see cref="License"/>.
        /// </summary>
        public Customer Customer
        {
            get
            {
                XElement xmlElement = m_xmlData.Element("Customer");

                if (!IsSigned && xmlElement == null)
                {
                    m_xmlData.Add(new XElement("Customer"));
                    xmlElement = m_xmlData.Element("Customer");
                }
                else if (IsSigned && xmlElement == null)
                {
                    return null;
                }

                return new Customer(xmlElement);
            }
        }

        /// <summary>
        /// Gets the <see cref="Product"/> of this <see cref="License"/>.
        /// </summary>
        public Product Product
        {
            get
            {
                XElement xmlElement = m_xmlData.Element("Product");

                if (!IsSigned && xmlElement == null)
                {
                    m_xmlData.Add(new XElement("Product"));
                    xmlElement = m_xmlData.Element("Product");
                }
                else if (IsSigned && xmlElement == null)
                {
                    return null;
                }

                return new Product(xmlElement);
            }
        }

        /// <summary>
        /// Gets or sets the additional attributes of this <see cref="License"/>.
        /// </summary>
        public LicenseAttributes AdditionalAttributes
        {
            get
            {
                XElement xmlElement = m_xmlData.Element("LicenseAttributes");

                if (!IsSigned && xmlElement == null)
                {
                    m_xmlData.Add(new XElement("LicenseAttributes"));
                    xmlElement = m_xmlData.Element("LicenseAttributes");
                }
                else if (IsSigned && xmlElement == null)
                {
                    IsNewLicenseType = false;
                    return null;
                }
                IsNewLicenseType = true;
                return new LicenseAttributes(xmlElement, "Attribute");
            }
        }

        /// <summary>
        /// Gets the digital signature of this m_license.
        /// </summary>
        /// <remarks>Use the <see cref="Sign"/> method to compute a signature.</remarks>
        public string Signature => GetTag("Signature");
        #endregion Properties

        #region Public Methods
        /// <summary>
        /// Compute a signature and sign this <see cref="License"/> with the provided key.
        /// </summary>
        /// <param name="privateKey">The private key in xml string format to compute the signature.</param>
        /// <param name="passPhrase">The pass phrase to decrypt the private key.</param>
        public void Sign(string privateKey, string passPhrase)
        {
            XElement signTag = m_xmlData.Element("Signature") ?? new XElement("Signature");

            try
            {
                if (signTag.Parent != null)
                {
                    signTag.Remove();
                }

                byte[] documentToSign = Encoding.UTF8.GetBytes(m_xmlData.ToString(SaveOptions.DisableFormatting));

                var signer = Signer.Create();
                byte[] signature = signer.Sign(documentToSign, privateKey, passPhrase);
                signTag.Value = Convert.ToBase64String(signature);
            }
            finally
            {
                m_xmlData.Add(signTag);
            }
        }

        /// <summary>
        /// Determines whether the <see cref="Signature"/> property verifies for the specified key.
        /// </summary>
        /// <param name="publicKey">The public key in xml string format to verify the <see cref="Signature"/>.</param>
        /// <returns>true if the <see cref="Signature"/> verifies; otherwise false.</returns>
        public bool VerifySignature(string publicKey)
        {
            // Because we'll be manipulating the m_xmlData, make sure to do it on a deep clone of the m_xmlData.
            // Otherwise other thread accessing m_xmlData at the same time may see the "changed" (and invalid!)
            // copy of m_xmlData.
            var xmlDataClone = new XElement(m_xmlData);
            XElement signTag = xmlDataClone.Element("Signature");

            if (signTag == null)
            {
                return false;
            }

            try
            {
                signTag.Remove();

                byte[] documentToSign = Encoding.UTF8.GetBytes(xmlDataClone.ToString(SaveOptions.DisableFormatting));

                var signer = Signer.Create();
                return signer.VerifySignature(documentToSign, Convert.FromBase64String(signTag.Value), publicKey);
            }
            finally
            {
                xmlDataClone.Add(signTag);
            }
        }

        /// <summary>
        /// Create a new <see cref="License"/> using the <see cref="ILicenseBuilder"/>
        /// fluent api.
        /// </summary>
        /// <returns>An instance of the <see cref="ILicenseBuilder"/> class.</returns>
        public static ILicenseBuilder New()
        {
            return new LicenseBuilder();
        }

        /// <summary>
        /// Loads a <see cref="License"/> from a string that contains XML.
        /// </summary>
        /// <param name="xmlString">A <see cref="string"/> that contains XML.</param>
        /// <returns>A <see cref="License"/> populated from the <see cref="string"/> that contains XML.</returns>
        public static License Load(string xmlString)
        {
            return new License(XElement.Parse(xmlString, LoadOptions.None));
        }

        /// <summary>
        /// Loads a <see cref="License"/> by using the specified <see cref="Stream"/>
        /// that contains the XML.
        /// </summary>
        /// <param name="stream">A <see cref="Stream"/> that contains the XML.</param>
        /// <returns>A <see cref="License"/> populated from the <see cref="Stream"/> that contains XML.</returns>
        public static License Load(Stream stream)
        {
            return new License(XElement.Load(stream, LoadOptions.None));
        }

        /// <summary>
        /// Loads a <see cref="License"/> by using the specified <see cref="TextReader"/>
        /// that contains the XML.
        /// </summary>
        /// <param name="reader">A <see cref="TextReader"/> that contains the XML.</param>
        /// <returns>A <see cref="License"/> populated from the <see cref="TextReader"/> that contains XML.</returns>
        public static License Load(TextReader reader)
        {
            return new License(XElement.Load(reader, LoadOptions.None));
        }

        /// <summary>
        /// Loads a <see cref="License"/> by using the specified <see cref="XmlReader"/>
        /// that contains the XML.
        /// </summary>
        /// <param name="reader">A <see cref="XmlReader"/> that contains the XML.</param>
        /// <returns>A <see cref="License"/> populated from the <see cref="TextReader"/> that contains XML.</returns>
        public static License Load(XmlReader reader)
        {
            return new License(XElement.Load(reader, LoadOptions.None));
        }

        /// <summary>
        /// Serialize this <see cref="License"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">A <see cref="Stream"/> that the
        /// <see cref="License"/> will be written to.</param>
        public void Save(Stream stream)
        {
            m_xmlData.Save(stream);
        }

        /// <summary>
        /// Serialize this <see cref="License"/> to a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="textWriter">A <see cref="TextWriter"/> that the
        /// <see cref="License"/> will be written to.</param>
        public void Save(TextWriter textWriter)
        {
            m_xmlData.Save(textWriter);
        }

        /// <summary>
        /// Serialize this <see cref="License"/> to a <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="xmlWriter">A <see cref="XmlWriter"/> that the
        /// <see cref="License"/> will be written to.</param>
        public void Save(XmlWriter xmlWriter)
        {
            m_xmlData.Save(xmlWriter);
        }

        /// <summary>
        /// Returns the indented XML for this <see cref="License"/>.
        /// </summary>
        /// <returns>A string containing the indented XML.</returns>
        public override string ToString()
        {
            return m_xmlData.ToString();
        }
        #endregion Public Methods

        #region Private Properties
        /// <summary>
        /// Gets a value indicating whether this <see cref="License"/> is already signed.
        /// </summary>
        private bool IsSigned => !string.IsNullOrEmpty(Signature);
        #endregion Private Properties

        #region Private Methods
        private void SetTag(string name, string value)
        {
            XElement element = m_xmlData.Element(name);

            if (element == null)
            {
                element = new XElement(name);
                m_xmlData.Add(element);
            }

            if (value != null)
            {
                element.Value = value;
            }
        }

        private string GetTag(string name)
        {
            XElement element = m_xmlData.Element(name);
            return element?.Value;
        }
        #endregion Private Methods

        #region Private Fields
        private readonly XElement m_xmlData;
        #endregion Private Fields
    }
}
