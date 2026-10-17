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
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// ECDSA signer for .NET Framework, where <see cref="System.Security.Cryptography.ECDsa"/> cannot
    /// import PKCS#8 or SubjectPublicKeyInfo keys. Produces and accepts the same DER encoded
    /// SHA-512 signatures as <c>NativeSigner</c>, so licences are interchangeable.
    /// </summary>
    internal class BouncyCastleSigner : Signer
    {
        private const string kAlgorithm = "SHA-512withECDSA";

        /// <inheritdoc/>
        public override byte[] Sign(byte[] documentToSign, string privateKey, string passPhrase)
        {
            AsymmetricKeyParameter key = PrivateKeyFactory.DecryptKey(
                passPhrase.ToCharArray(),
                Convert.FromBase64String(privateKey));
            ISigner signer = SignerUtilities.GetSigner(kAlgorithm);
            signer.Init(true, key);
            signer.BlockUpdate(documentToSign, 0, documentToSign.Length);
            return signer.GenerateSignature();
        }

        /// <inheritdoc/>
        public override bool VerifySignature(byte[] documentToSign, byte[] signature, string publicKey)
        {
            AsymmetricKeyParameter key = PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
            ISigner signer = SignerUtilities.GetSigner(kAlgorithm);
            signer.Init(false, key);
            signer.BlockUpdate(documentToSign, 0, documentToSign.Length);
            return signer.VerifySignature(signature);
        }
    }
}
#endif
