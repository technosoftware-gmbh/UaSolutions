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
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <summary>
    /// Key generator for .NET Framework. Uses NIST P-521, the curve <c>ECDsa.Create()</c>
    /// defaults to on .NET.
    /// </summary>
    internal class BouncyCastleKeyGenerator : KeyGenerator
    {
        /// <inheritdoc/>
        public override KeyPair GenerateKeyPair()
        {
            var generator = new ECKeyPairGenerator("ECDSA");
            generator.Init(new ECKeyGenerationParameters(SecObjectIdentifiers.SecP521r1, new SecureRandom()));
            return new BouncyCastleKeyPair(generator.GenerateKeyPair());
        }
    }
}
#endif
