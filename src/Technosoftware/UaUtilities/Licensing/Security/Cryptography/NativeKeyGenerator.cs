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
using System.Security.Cryptography;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    /// <inheritdoc/>
    internal class NativeKeyGenerator : KeyGenerator
    {
        /// <inheritdoc/>
        public override KeyPair GenerateKeyPair()
        {
            return new NativeKeyPair(ECDsa.Create());
        }
    }
}
