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
using System.IO;
using Opc.Ua;
#endregion Using Directives

namespace SampleCompany.ReferenceServer
{
    /// <summary>
    /// A certificate store type that stores certificates in a directory.
    /// </summary>
    /// <remarks>
    /// Registering a store type is what this sample demonstrates: the type is
    /// added to <c>CertificateStoreType</c> and any store path the type claims
    /// is then opened through <see cref="CreateStore"/>.
    /// </remarks>
    public class CustomDirectoryCertificateStoreType : ICertificateStoreType
    {
        /// <summary>
        /// A directory certificate store.
        /// </summary>
        public const string StoreName = "CustomDirectory";

        /// <inheritdoc/>
        public bool SupportsStorePath(string storePath)
        {
            if (string.IsNullOrEmpty(storePath))
            {
                return false;
            }
            // check for directory path.
            return Directory.Exists(storePath) ||
                Directory.Exists(Utils.ReplaceSpecialFolderNames(storePath));
        }

        /// <inheritdoc/>
        public ICertificateStore CreateStore(ITelemetryContext telemetry)
        {
            return new CustomDirectoryCertificateStore(telemetry);
        }
    }

    /// <summary>
    /// Provides access to a simple file based certificate store.
    /// </summary>
    /// <remarks>
    /// This used to carry a copy of the stack's own directory store. It now
    /// derives from it instead, so the sample shows the part that is actually
    /// custom - the store type above - and the storage behaviour tracks the
    /// stack rather than a snapshot of it. Override the members that need to
    /// behave differently.
    /// </remarks>
    public class CustomDirectoryCertificateStore : DirectoryCertificateStore
    {
        /// <summary>
        /// Initializes a store for a directory path.
        /// </summary>
        public CustomDirectoryCertificateStore(ITelemetryContext telemetry)
            : base(telemetry)
        {
        }

        /// <summary>
        /// Initializes a store with a directory path.
        /// </summary>
        public CustomDirectoryCertificateStore(bool noSubDirs, ITelemetryContext telemetry)
            : base(noSubDirs, telemetry)
        {
        }
    }
}
