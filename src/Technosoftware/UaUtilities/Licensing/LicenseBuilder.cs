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
using System.Collections.Generic;
using System.ComponentModel;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// Implementation of the <see cref="ILicenseBuilder"/>, a fluent api
    /// to create new licenses.
    /// </summary>
    internal class LicenseBuilder : ILicenseBuilder
    {
        private readonly License m_license;

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseBuilder"/> class.
        /// </summary>
        public LicenseBuilder()
        {
            m_license = new License();
        }

        /// <summary>
        /// Sets the unique identifier of the <see cref="License"/>.
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="License"/>.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder WithUniqueIdentifier(Guid id)
        {
            m_license.Id = id;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="LicenseType"/> of the <see cref="License"/>.
        /// </summary>
        /// <param name="type">The <see cref="LicenseType"/> of the <see cref="License"/>.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder As(LicenseType type)
        {
            m_license.LicenseLevel = type;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="ProductType"/> of the <see cref="License"/>.
        /// </summary>
        /// <param name="type">The <see cref="ProductType"/> of the <see cref="License"/>.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder As(ProductType type)
        {
            m_license.ProductType = type;
            return this;
        }

        /// <summary>
        /// Sets the expiration date of the <see cref="License"/>.
        /// </summary>
        /// <param name="date">The expiration date of the <see cref="License"/>.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder LicenseExpiresAt(DateTime date)
        {
            m_license.LicenseExpirationDate = date.ToUniversalTime();
            return this;
        }

        /// <summary>
        /// Sets the expiration date of the <see cref="License"/>.
        /// </summary>
        /// <param name="date">The expiration date of the <see cref="License"/>.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder SupportExpiresAt(DateTime date)
        {
            m_license.SupportExpirationDate = date.ToUniversalTime();
            return this;
        }

        /// <param name="name">The name of the license holder.</param>
        /// <param name="company">The company of the license holder.</param>
        /// <param name="domain">The domain of the license holder.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder LicensedTo(string name, string company, string domain)
        {
            m_license.Customer.Name = name;
            m_license.Customer.Company = company;
            m_license.Customer.Domain = domain;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="LicenseInfo">product</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="level">The level of the license.</param>
        /// <param name="expirationDate">The date the license expires.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder WithLicenseInfo(string level, string expirationDate)
        {
            m_license.LicenseInfo.Level = level;
            m_license.LicenseInfo.ExpirationDate = expirationDate;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="LicenseInfo">product</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="configureLicenseInfo">A delegate to configure the license.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder WithLicenseInfo(Action<LicenseInfo> configureLicenseInfo)
        {
            configureLicenseInfo(m_license.LicenseInfo);
            return this;
        }

        /// <summary>
        /// Sets the <see cref="Support">support</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="level">The level of the support.</param>
        /// <param name="expirationDate">The date the support expires.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder WithSupport(string level, string expirationDate)
        {
            m_license.SupportInfo.Level = level;
            m_license.SupportInfo.ExpirationDate = expirationDate;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="Support">support</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="configureSupport">A delegate to configure the support.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder WithSupport(Action<Support> configureSupport)
        {
            configureSupport(m_license.SupportInfo);
            return this;
        }

        /// <summary>
        /// Sets the <see cref="Customer">m_license holder</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="name">The name of the m_license holder.</param>
        /// <param name="company">The company of the license holder.</param>
        /// <param name="domain">The domain of the m_license holder.</param>
        /// <param name="configureCustomer">A delegate to configure the m_license holder.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder LicensedTo(string name, string company, string domain, Action<Customer> configureCustomer)
        {
            m_license.Customer.Name = name;
            m_license.Customer.Company = company;
            m_license.Customer.Domain = domain;
            configureCustomer(m_license.Customer);
            return this;
        }

        /// <summary>
        /// Sets the <see cref="Customer">license holder</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="configureCustomer">A delegate to configure the license holder.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder LicensedTo(Action<Customer> configureCustomer)
        {
            configureCustomer(m_license.Customer);
            return this;
        }

        /// <summary>
        /// Sets the <see cref="Product">product</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <param name="type">The type of the product.</param>
        /// <param name="version">The version of the product.</param>
        /// <param name="publishedDate">The date the product was published.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder WithProduct(string name, string type, string version, string publishedDate)
        {
            m_license.Product.Name = name;
            m_license.Product.Type = type;
            m_license.Product.Version = version;
            m_license.Product.PublishedDate = publishedDate;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="Product">product</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="configureProduct">A delegate to configure the product.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder WithProduct(Action<Product> configureProduct)
        {
            configureProduct(m_license.Product);
            return this;
        }

        /// <summary>
        /// Sets the licensed product features of the <see cref="License"/>.
        /// </summary>
        /// <param name="productFeatures">The licensed product features of the <see cref="License"/>.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder WithProductFeatures(IDictionary<string, string> productFeatures)
        {
            m_license.LicensedFeatures.AddAll(productFeatures);
            return this;
        }

        /// <summary>
        /// Sets the licensed product features of the <see cref="License"/>.
        /// </summary>
        /// <param name="configureProductFeatures">A delegate to configure the product features.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder WithProductFeatures(Action<LicenseAttributes> configureProductFeatures)
        {
            configureProductFeatures(m_license.LicensedFeatures);
            return this;
        }

        /// <summary>
        /// Sets the licensed additional attributes of the <see cref="License"/>.
        /// </summary>
        /// <param name="additionalAttributes">The additional attributes of the <see cref="License"/>.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder WithAdditionalAttributes(IDictionary<string, string> additionalAttributes)
        {
            m_license.AdditionalAttributes.AddAll(additionalAttributes);
            return this;
        }

        /// <summary>
        /// Sets the licensed additional attributes of the <see cref="License"/>.
        /// </summary>
        /// <param name="configureAdditionalAttributes">A delegate to configure the additional attributes.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        public ILicenseBuilder WithAdditionalAttributes(Action<LicenseAttributes> configureAdditionalAttributes)
        {
            configureAdditionalAttributes(m_license.AdditionalAttributes);
            return this;
        }

        /// <summary>
        /// Create and sign a new <see cref="License"/> with the specified
        /// private encryption key.
        /// </summary>
        /// <param name="privateKey">The private encryption key for the signature.</param>
        /// <param name="passPhrase">The pass phrase to decrypt the private key.</param>
        /// <returns>The signed <see cref="License"/>.</returns>
        public License CreateAndSignWithPrivateKey(string privateKey, string passPhrase)
        {
            m_license.Sign(privateKey, passPhrase);
            return m_license;
        }
    }
}
