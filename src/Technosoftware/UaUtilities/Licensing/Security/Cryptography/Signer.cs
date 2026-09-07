#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on https://github.com/junian/Standard.Licensing. 
// The complete license agreement for that can be found in this directore in the LICENSE.txt file.
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

namespace Technosoftware.UaUtilities
{
    /// <summary>
    ///
    /// </summary>
    internal abstract class Signer
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static Signer Create()
        {
            return new NativeSigner();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="documentToSign"></param>
        /// <param name="privateKey"></param>
        /// <param name="passPhrase"></param>
        /// <returns></returns>
        public abstract byte[] Sign(byte[] documentToSign, string privateKey, string passPhrase);

        /// <summary>
        ///
        /// </summary>
        /// <param name="documentToSign"></param>
        /// <param name="signature"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public abstract bool VerifySignature(byte[] documentToSign, byte[] signature, string publicKey);
    }
}
