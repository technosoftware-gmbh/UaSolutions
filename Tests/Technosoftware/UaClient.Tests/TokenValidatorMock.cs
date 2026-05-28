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
using Opc.Ua;
using SampleCompany.NodeManagers.Reference;
#endregion Using Directives

namespace Technosoftware.UaClient.Tests
{
    public class TokenValidatorMock : ITokenValidator
    {
        public IssuedIdentityToken LastIssuedToken { get; set; }

        public IUserIdentity ValidateToken(IssuedIdentityToken issuedToken)
        {
            LastIssuedToken = issuedToken;

            return new UserIdentity(issuedToken);
        }
    }
}
