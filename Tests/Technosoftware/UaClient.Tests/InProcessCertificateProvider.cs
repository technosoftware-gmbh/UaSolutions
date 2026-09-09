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

#region Using Directives
using System;
using System.Threading;
using System.Threading.Tasks;
using Opc.Ua;
using Opc.Ua.Security.Certificates;
#endregion Using Directives

namespace Technosoftware.UaClient.Tests
{
    /// <summary>
    /// Test-only <see cref="ICertificateProvider"/> that wraps a single
    /// in-memory <see cref="Certificate"/>.
    /// </summary>
    /// <remarks>
    /// Used by the tests that used to construct
    /// <c>new UserIdentity(certificate)</c> directly and now need to feed a
    /// private-key certificate into the <see cref="X509IdentityTokenHandler"/>
    /// constructor without persisting it to a directory store. Adopted from
    /// the OPC Foundation test framework.
    /// </remarks>
    public sealed class InProcessCertificateProvider : ICertificateProvider, IDisposable
    {
        /// <summary>
        /// Initializes the provider with the certificate it hands out.
        /// </summary>
        public InProcessCertificateProvider(Certificate cert)
        {
            if (cert == null)
            {
                throw new ArgumentNullException(nameof(cert));
            }
            m_cert = cert.AddRef();
        }

        /// <inheritdoc/>
        public Certificate TryGetPrivateKeyCertificate(string thumbprint)
        {
            Certificate cert = m_cert;
            return cert != null &&
                string.Equals(cert.Thumbprint, thumbprint, StringComparison.OrdinalIgnoreCase)
                ? cert.AddRef()
                : null;
        }

        /// <inheritdoc/>
        public ValueTask<Certificate> GetPrivateKeyCertificateAsync(
            CertificateIdentifier identifier,
            ICertificatePasswordProvider passwordProvider = null,
            string applicationUri = null,
            CancellationToken ct = default)
        {
            Certificate cert = m_cert;
            return cert == null
                ? new ValueTask<Certificate>((Certificate)null)
                : new ValueTask<Certificate>(cert.AddRef());
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            m_cert?.Dispose();
            m_cert = null;
        }

        private Certificate m_cert;
    }
}
