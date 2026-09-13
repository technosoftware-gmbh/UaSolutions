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
using System.Security.Cryptography;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <inheritdoc/>
    internal class NativeKeyPair : KeyPair
    {
        private readonly AsymmetricAlgorithm m_algorithm;

        /// <inheritdoc/>
        public NativeKeyPair(AsymmetricAlgorithm algorithm)
        {
            m_algorithm = algorithm ?? throw new ArgumentNullException(nameof(algorithm));
        }

        /// <summary>
        /// Gets the encrypted and DER encoded private key.
        /// </summary>
        /// <param name="passPhrase">The pass phrase to encrypt the private key.</param>
        /// <returns>The encrypted private key.</returns>
        public override string ToEncryptedPrivateKeyString(string passPhrase)
        {
            var data = m_algorithm
                .ExportEncryptedPkcs8PrivateKey(passPhrase, new PbeParameters(PbeEncryptionAlgorithm.TripleDes3KeyPkcs12, HashAlgorithmName.SHA1, 10));
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Gets the DER encoded public key.
        /// </summary>
        /// <returns>The public key.</returns>
        public override string ToPublicKeyString()
        {
            var data = m_algorithm.ExportSubjectPublicKeyInfo();
            return Convert.ToBase64String(data);
        }
    }
}
