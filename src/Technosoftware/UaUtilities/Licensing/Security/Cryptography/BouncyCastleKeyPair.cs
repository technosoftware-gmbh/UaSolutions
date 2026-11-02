#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on https://github.com/junian/Standard.Licensing. 
// The complete license agreement for that can be found in this directore in the LICENSE.txt file.
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

#if NETFRAMEWORK

#region Using Directives
using System;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// Key pair for .NET Framework. Encodes keys exactly as <c>NativeKeyPair</c> does:
    /// PKCS#8 encrypted with PKCS#12 3-key TripleDES/SHA-1, and SubjectPublicKeyInfo.
    /// </summary>
    internal class BouncyCastleKeyPair : KeyPair
    {
        private const int kIterations = 10;
        private const int kSaltSize = 8;
        private readonly AsymmetricCipherKeyPair m_keyPair;

        /// <inheritdoc/>
        public BouncyCastleKeyPair(AsymmetricCipherKeyPair keyPair)
        {
            m_keyPair = keyPair ?? throw new ArgumentNullException(nameof(keyPair));
        }

        /// <summary>
        /// Gets the encrypted and DER encoded private key.
        /// </summary>
        /// <param name="passPhrase">The pass phrase to encrypt the private key.</param>
        /// <returns>The encrypted private key.</returns>
        public override string ToEncryptedPrivateKeyString(string passPhrase)
        {
            byte[] salt = SecureRandom.GetNextBytes(new SecureRandom(), kSaltSize);
            EncryptedPrivateKeyInfo info = EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(
                PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc,
                passPhrase.ToCharArray(),
                salt,
                kIterations,
                m_keyPair.Private);
            return Convert.ToBase64String(info.GetEncoded());
        }

        /// <summary>
        /// Gets the DER encoded public key.
        /// </summary>
        /// <returns>The public key.</returns>
        public override string ToPublicKeyString()
        {
            return Convert.ToBase64String(
                SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(m_keyPair.Public).GetEncoded());
        }
    }
}
#endif
