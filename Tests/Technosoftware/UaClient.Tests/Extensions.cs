#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using Opc.Ua;
#endregion Using Directives

namespace Technosoftware.UaClient.Tests
{
    public static class Extensions
    {
        public static bool HasArgsOfType(
            this CallMethodRequestCollection requests,
            params Type[] argTypes)
        {
            if (requests.Count != 1 || requests[0].InputArguments.Count != argTypes.Length)
            {
                return false;
            }
            for (int i = 0; i < argTypes.Length; i++)
            {
                if (requests[0].InputArguments[i].Value.GetType() != argTypes[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static CallResponse ToResponse(
            this List<object> outputArguments, StatusCode response = default, StatusCode result = default)
        {
            return new CallResponse
            {
                ResponseHeader = new ResponseHeader
                {
                    ServiceResult = response
                },
                Results =
                [
                    new CallMethodResult
                    {
                        StatusCode = result,
                        OutputArguments = outputArguments == null ?
                            null :
                            [.. outputArguments.Select(o => new Variant(o))]
                    }
                ]
            };
        }
    }
}
