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
    internal class NativeSigner : Signer
    {
        /// <inheritdoc/>
        public override byte[] Sign(byte[] documentToSign, string privateKey, string passPhrase)
        {
            var ecdsa = ECDsa.Create();
            ecdsa.ImportEncryptedPkcs8PrivateKey(passPhrase, Convert.FromBase64String(privateKey), out int _);
            return ecdsa.SignData(documentToSign, HashAlgorithmName.SHA512, DSASignatureFormat.Rfc3279DerSequence);
        }

        /// <inheritdoc/>
        public override bool VerifySignature(byte[] documentToSign, byte[] signature, string publicKey)
        {
            var ecdsa = ECDsa.Create();
            ecdsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out int read);
            return ecdsa.VerifyData(documentToSign, signature, HashAlgorithmName.SHA512, DSASignatureFormat.Rfc3279DerSequence);
        }
    }
}
