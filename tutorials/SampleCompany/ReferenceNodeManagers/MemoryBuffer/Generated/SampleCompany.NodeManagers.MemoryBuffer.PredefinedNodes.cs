/* ========================================================================
 * Copyright (c) 2005-2024 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using System;
using System.Text;
using System.IO;
using Opc.Ua;

#pragma warning disable 1591

namespace SampleCompany.NodeManagers.MemoryBuffer
{
    #region _ClassName_ Declarations
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute()]
    public static partial class PredefinedNodes
    {
        #region PredefinedNodes Declarations
        // <summary/>
        public static NodeStateCollection Load(ISystemContext context)
        {
            byte[] initializationBuffer = Convert.FromBase64String(
               "AQAAAD8AAABodHRwOi8vc2FtcGxlY29tcGFueS5jb20vU2FtcGxlU2VydmVyL05vZGVNYW5hZ2Vycy9N" +
               "ZW1vcnlCdWZmZXL/////AwAAABRgABAQAAAAAQANAAAATWVtb3J5VGFnVHlwZQEBAQAAPwAY/////wAA" +
               "AAAEYAAQCAAAAAEAEAAAAE1lbW9yeUJ1ZmZlclR5cGUBAQIAADr/////AgAAABVgqQsCAAAAAQAMAAAA" +
               "U3RhcnRBZGRyZXNzAQEDAAAuAEQATgMAAAAHAAAAAAAH/////wEB/////wAAAAAVYKkLAgAAAAEACwAA" +
               "AFNpemVJbkJ5dGVzAQEEAAAuAEQATgQAAAAHABAAAAAH/////wEB/////wAAAAAEYIAKAQAAAAEADQAA" +
               "AE1lbW9yeUJ1ZmZlcnMBAQUAAC8APQUAAAABAAAAACMBAFUAAAAA"
            );
            using (MemoryStream stream = new MemoryStream(initializationBuffer))
            {
                NodeStateCollection predefinedNodes = new NodeStateCollection();
                predefinedNodes.LoadFromBinary(context, stream, true);
                return predefinedNodes;
            }
        }
        #endregion
    }
    #endregion
}