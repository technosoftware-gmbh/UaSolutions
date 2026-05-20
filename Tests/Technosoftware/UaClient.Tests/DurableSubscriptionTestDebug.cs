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
using System.Threading.Tasks;
using NUnit.Framework;
#endregion Using Directives

namespace Technosoftware.UaClient.Tests
{
    [TestFixture]
    [Explicit]
    public class DurableSubscriptionTestDebug
    {
        internal const int LoopCount = 100;

        [Test]
        [Order(200)]
        [TestCase(false, false, TestName = "Validate Session Close")]
        [TestCase(true, false, TestName = "Validate Transfer")]
        [TestCase(true, true, TestName = "Restart of Server")]
        public async Task TransferLoopTestAsync(bool setSubscriptionDurable, bool restartServer)
        {
            var test = new DurableSubscriptionTest();
            await test.OneTimeSetUpAsync().ConfigureAwait(false);
            for (int i = 0; i < LoopCount; i++)
            {
                await test.SetUpAsync().ConfigureAwait(false);
                await test.TestSessionTransferAsync(setSubscriptionDurable, restartServer).ConfigureAwait(false);
                await test.TearDownAsync().ConfigureAwait(false);
                TestContext.Out.WriteLine("===========================================");
                TestContext.Out.WriteLine("===========================================");
                TestContext.Out.WriteLine($"Completed {i}th iteration.");
                TestContext.Out.WriteLine("===========================================");
                TestContext.Out.WriteLine("===========================================");
            }
            await test.OneTimeTearDownAsync().ConfigureAwait(false);
        }
    }
}
