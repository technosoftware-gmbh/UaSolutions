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
    public abstract class KeyGenerator
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static KeyGenerator Create()
        {
            return new NativeKeyGenerator();
        }

        ///
        /// <summary>
        /// Generates a private/public key pair for m_license signing.
        /// </summary>
        /// <returns>An <see cref="KeyPair"/> containing the keys.</returns>
        public abstract KeyPair GenerateKeyPair();
    }
}
