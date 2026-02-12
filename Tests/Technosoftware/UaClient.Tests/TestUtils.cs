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
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Opc.Ua.Security.Certificates;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
#endregion Using Directives

namespace Opc.Ua.Tests
{
    /// <summary>
    /// The interface to initialize an asset.
    /// </summary>
    public interface IAsset
    {
        void Initialize(byte[] blob, string path);
    }

    /// <summary>
    /// Create a collection of test assets.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AssetCollection<T> : List<T>
        where T : IAsset, new()
    {
        public AssetCollection()
        {
        }

        public AssetCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public AssetCollection(int capacity)
            : base(capacity)
        {
        }

        public static AssetCollection<T> ToAssetCollection(T[] values)
        {
            return values != null ? [.. values] : [];
        }

        public static AssetCollection<T> CreateFromFiles(IEnumerable<string> filelist)
        {
            var result = new AssetCollection<T>();
            foreach (string file in filelist)
            {
                result.Add(file);
            }
            return result;
        }

        public void Add(string path)
        {
            byte[] blob = File.ReadAllBytes(path);
            var asset = new T();
            asset.Initialize(blob, path);
            Add(asset);
        }
    }

    /// <summary>
    /// Test helpers.
    /// </summary>
    public static class TestUtils
    {
        public static string[] EnumerateTestAssets(string searchPattern)
        {
            string assetsPath = Utils.GetAbsoluteDirectoryPath("Assets", true, false, false);
            if (assetsPath != null)
            {
                return [.. Directory.EnumerateFiles(assetsPath, searchPattern)];
            }
            return [];
        }

        public static void ValidateSelSignedBasicConstraints(X509Certificate2 certificate)
        {
            X509BasicConstraintsExtension basicConstraintsExtension =
                certificate.Extensions.FindExtension<X509BasicConstraintsExtension>();
            Assert.NotNull(basicConstraintsExtension);
            Assert.False(basicConstraintsExtension.CertificateAuthority);
            Assert.True(basicConstraintsExtension.Critical);
            Assert.False(basicConstraintsExtension.HasPathLengthConstraint);
        }
    }
}
