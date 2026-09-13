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
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// Fluent api to create and sign a new <see cref="License"/>.
    /// </summary>
    public interface ILicenseBuilder : IFluentInterface
    {
        /// <summary>
        /// Sets the unique identifier of the <see cref="License"/>.
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="License"/>.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder WithUniqueIdentifier(Guid id);

        /// <summary>
        /// Sets the <see cref="LicenseType"/> of the <see cref="License"/>.
        /// </summary>
        /// <param name="type">The <see cref="LicenseType"/> of the <see cref="License"/>.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder As(LicenseType type);

        /// <summary>
        /// Sets the <see cref="ProductType"/> of the <see cref="License"/>.
        /// </summary>
        /// <param name="type">The <see cref="ProductType"/> of the <see cref="License"/>.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder As(ProductType type);

        /// <summary>
        /// Sets the <see cref="Customer">license holder</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="name">The name of the license holder.</param>
        /// <param name="company">The company of the license holder.</param>
        /// <param name="domain">The domain of the license holder.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder LicensedTo(string name, string company, string domain);

        /// <summary>
        /// Sets the <see cref="Customer">license holder</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="name">The name of the license holder.</param>
        /// <param name="company">The company of the license holder.</param>
        /// <param name="domain">The email of the license holder.</param>
        /// <param name="configureCustomer">A delegate to configure the license holder.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder LicensedTo(string name, string company, string domain, Action<Customer> configureCustomer);

        /// <summary>
        /// Sets the <see cref="Customer">license holder</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="configureCustomer">A delegate to configure the license holder.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder LicensedTo(Action<Customer> configureCustomer);

        /// <summary>
        /// Sets the expiration date of the <see cref="License"/>.
        /// </summary>
        /// <param name="date">The expiration date of the <see cref="License"/>.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder LicenseExpiresAt(DateTime date);

        /// <summary>
        /// Sets the expiration date of the <see cref="License"/>.
        /// </summary>
        /// <param name="date">The expiration date of the <see cref="License"/>.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder SupportExpiresAt(DateTime date);

        /// <summary>
        /// Sets the <see cref="LicenseInfo">product</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="level">The level of the license.</param>
        /// <param name="expirationDate">The date the license expires.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder WithLicenseInfo(string level, string expirationDate);

        /// <summary>
        /// Sets the <see cref="LicenseInfo">product</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="configureLicenseInfo">A delegate to configure the license.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder WithLicenseInfo(Action<LicenseInfo> configureLicenseInfo);

        /// <summary>
        /// Sets the <see cref="Support">support</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="level">The level of the support.</param>
        /// <param name="expirationDate">The date the support expires.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder WithSupport(string level, string expirationDate);

        /// <summary>
        /// Sets the <see cref="Support">support</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="configureSupport">A delegate to configure the support.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder WithSupport(Action<Support> configureSupport);

        /// <summary>
        /// Sets the <see cref="Product">product</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <param name="type">The type of the product.</param>
        /// <param name="version">The version of the product.</param>
        /// <param name="publishedDate">The date the product was published.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder WithProduct(string name, string type, string version, string publishedDate );

        /// <summary>
        /// Sets the <see cref="Product">product</see> of the <see cref="License"/>.
        /// </summary>
        /// <param name="configureProduct">A delegate to configure the product.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder WithProduct(Action<Product> configureProduct);

        /// <summary>
        /// Sets the licensed product features of the <see cref="License"/>.
        /// </summary>
        /// <param name="productFeatures">The licensed product features of the <see cref="License"/>.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder WithProductFeatures(IDictionary<string, string> productFeatures);

        /// <summary>
        /// Sets the licensed product features of the <see cref="License"/>.
        /// </summary>
        /// <param name="configureProductFeatures">A delegate to configure the product features.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder WithProductFeatures(Action<LicenseAttributes> configureProductFeatures);

        /// <summary>
        /// Sets the licensed additional attributes of the <see cref="License"/>.
        /// </summary>
        /// <param name="additionalAttributes">The additional attributes of the <see cref="License"/>.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder WithAdditionalAttributes(IDictionary<string, string> additionalAttributes);

        /// <summary>
        /// Sets the licensed additional attributes of the <see cref="License"/>.
        /// </summary>
        /// <param name="configureAdditionalAttributes">A delegate to configure the additional attributes.</param>
        /// <returns>The <see cref="ILicenseBuilder"/>.</returns>
        ILicenseBuilder WithAdditionalAttributes(Action<LicenseAttributes> configureAdditionalAttributes);

        /// <summary>
        /// Create and sign a new <see cref="License"/> with the specified
        /// private encryption key.
        /// </summary>
        /// <param name="privateKey">The private encryption key for the signature.</param>
        /// <param name="passPhrase">The pass phrase to decrypt the private key.</param>
        /// <returns>The signed <see cref="License"/>.</returns>
        License CreateAndSignWithPrivateKey(string privateKey, string passPhrase);
    }
}
