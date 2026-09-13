#region Copyright (c) 2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com
//
// The Software is subject to the Technosoftware GmbH Source Code License Agreement,
// which can be found here:
// https://technosoftware.com/documents/Source_License_Agreement.pdf
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2026 Technosoftware GmbH. All rights reserved

//-----------------------------------------------------------------------------
//  T H R O W - A W A Y   K E Y   P A I R   -   N O T   A   S E C R E T
//
//  This key pair exists so that licences used by the tests are signed with
//  something other than the production key. A licence signed with it is REJECTED
//  by every shipped package - that is the entire point, and there is a test that
//  asserts it (see LicenseFixtureIsolationTests).
//
//  Consequences to keep in mind:
//    * Never replace these values with the production key pair. If a fixture ever
//      needs to be re-signed, regenerate this pair - do not reach for the real one.
//    * The private key is committed on purpose. It signs nothing of value.
//    * Fixture licences must never be checked in signed with the production key.
//      See project doc 12/13 for the history of why.
//-----------------------------------------------------------------------------

namespace Technosoftware.UaUtilities.Tests
{
    /// <summary>
    /// The throw-away key pair used to sign every licence in the test fixtures.
    /// </summary>
    internal static class FixtureKeys
    {
        /// <summary>
        /// The pass phrase protecting <see cref="PrivateKey"/>.
        /// </summary>
        internal const string PassPhrase = "fixture";

        /// <summary>
        /// The fixture public key. Licences signed by this pair validate only against it.
        /// </summary>
        internal const string PublicKey =
            "MIGbMBAGByqGSM49AgEGBSuBBAAjA4GGAAQAloP0SVLIGeY0CkWEp5OqyCPupAl0HlGh9Gq5YvgsYaCgaXZ0J+ALMgyXioKymPTd"
                + "FYP+pE4pDE2Y8LHD5DPnCawAW8OasDollso0N4O5K1aMPIKcF/zPtotF2k+NTtG65qHr+bGg1if3N+WMephU/sJhHw5ZXLgjqfw9"
                + "qI4L0TDArcc=";

        /// <summary>
        /// The fixture private key, encrypted with <see cref="PassPhrase"/>. Committed on
        /// purpose: it can only mint licences that shipped packages reject.
        /// </summary>
        internal const string PrivateKey =
            "MIIBIDAjBgoqhkiG9w0BDAEDMBUEEI8WzgaiUgs8UnPloZ6+mRoCAQoEgfiVyGmXw2ljhkMWjwf+Ex+FWGFulWgND4JJVBqJrMoi"
                + "u0YdEUucngOg1LCaPM8PYG15yjid275t0LgkP0LmHwfOhpXjUbuW7AfE3z7AO9L3hZSbXS/fmwlYpyoTjWN6Sopzmclot+8q/PVj"
                + "FUEqcmbhGXYszt23NiMj593dmjmMx9+fKdMgrAlupkxnUjCcqRYzBCV9SP2o3Wn41n6c1IWy6Aw6WadtYiabbBv5kPT2gocYwcp0"
                + "HiGatn8yK5oW9VEy5RLXWo34bymbI7pql1U0BAC/oLmpkso4Of0t2jtQH9NL/t9NC3kBesdHaOwHaahZQtu7LWfiZg==";
    }
}
