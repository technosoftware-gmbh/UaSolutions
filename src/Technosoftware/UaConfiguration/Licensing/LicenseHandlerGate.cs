#region Copyright (c) 2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com
//
// The Software is subject to the Technosoftware GmbH MIT License, which can
// be found here:
// https://technosoftware.com/license/mit/
//
// The Software is based on the OPC Foundation UA Stack and the OPC Foundation
// MIT License. The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2026 Technosoftware GmbH. All rights reserved

// This file is the ONLY place in the source tree that is compiled conditionally.
// It is included when the libraries are built with -p:Licensed=true, which is how
// the shipped NuGet packages are produced. Everything else calls IUaLicenseGate
// unconditionally, so no other file needs to know that licensing exists.
//
// The adapter lives here rather than in the licensing library because that library
// sits below this one in the dependency order; implementing the interface there
// would create a cycle.

#if TECHNOSOFTWARE_LICENSED

#region Using Directives
using System;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.Logging;
using Technosoftware.UaUtilities;
#endregion Using Directives

namespace Technosoftware.UaConfiguration
{
    /// <summary>
    /// The license gate backed by the Technosoftware licensing library.
    /// </summary>
    internal sealed class LicenseHandlerGate : IUaLicenseGate
    {
        #region Constructors, Destructor, Initialization
        /// <summary>
        /// Initializes the gate.
        /// </summary>
        /// <param name="logger">The logger used when reporting license failures.</param>
        internal LicenseHandlerGate(ILogger logger)
        {
            m_logger = logger;
        }
        #endregion Constructors, Destructor, Initialization

        #region IUaLicenseGate Members
        /// <inheritdoc/>
        public bool TryApplyLicense(string licenseData)
        {
            return LicenseHandler.Instance.Validate(null, licenseData);
        }

        /// <inheritdoc/>
        public void RequireFeature(UaProduct product, UaFeature feature)
        {
            LicenseHandler.Instance.ValidateFeatures(
                (ProductType)(int)product,
                (ProductFeature)(uint)feature,
                m_logger);
        }

        /// <inheritdoc/>
        public void WriteLicenseInfo(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            LicenseHandler handler = LicenseHandler.Instance;
            CultureInfo culture = CultureInfo.CurrentCulture;

            if (handler.IsLicensed)
            {
                writer.WriteLine(string.Format(culture, "   Customer             : {0}", handler.Customer.Name));
                writer.WriteLine(string.Format(culture, "   Days until Expiration: {0}", handler.LicenseExpirationDays));
            }

            writer.WriteLine(string.Format(culture, "   Licensed Product     : {0}", handler.LicensedProduct));
            writer.WriteLine(string.Format(culture, "   Licensed Type        : {0}", handler.LicensedType));
            writer.WriteLine(string.Format(culture, "   Licensed Features    : {0}", handler.LicensedFeatures));

            if (handler.IsEvaluation)
            {
                writer.WriteLine(string.Format(culture, "   Evaluation expires at: {0}", handler.LicenseExpirationDate));
                writer.WriteLine(string.Format(culture, "   Days until Expiration: {0}", handler.LicenseExpirationDays));
            }

            writer.WriteLine(string.Format(culture, "   Support Included     : {0}", handler.Support));

            if (handler.Support != SupportLevel.None)
            {
                writer.WriteLine(string.Format(culture, "   Support expire at    : {0}", handler.SupportExpirationDate));
                writer.WriteLine(string.Format(culture, "   Days until Expiration: {0}", handler.SupportExpirationDays));
            }

            if (handler.IsEvaluation)
            {
                writer.WriteLine(string.Format(culture, "   Evaluation Period    : {0} minutes.", handler.EvaluationPeriod));
            }

            if (!handler.IsLicensed && !handler.IsEvaluation)
            {
                writer.WriteLine("ERROR: No valid license applied.");
            }
        }
        #endregion IUaLicenseGate Members

        #region Private Fields
        private readonly ILogger m_logger;
        #endregion Private Fields
    }
}

#endif
