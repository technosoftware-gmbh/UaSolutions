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
using System.Runtime.Serialization;
#endregion Using Directives

namespace SampleCompany.NodeManagers.MemoryBuffer
{
    /// <summary>
    /// Stores the configuration the test node manager
    /// </summary>
    [DataContract(Namespace = Namespaces.MemoryBuffer)]
    public class MemoryBufferConfiguration
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public MemoryBufferConfiguration()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the object during deserialization.
        /// </summary>
        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        /// <summary>
        /// Sets private members to default values.
        /// </summary>
        private void Initialize()
        {
            Buffers = null;
        }

        /// <summary>
        /// The buffers exposed by the memory
        /// </summary>
        [DataMember(Order = 1)]
        public MemoryBufferInstanceCollection Buffers { get; set; }
    }

    /// <summary>
    /// Stores the configuration for a memory buffer instance.
    /// </summary>
    [DataContract(Namespace = Namespaces.MemoryBuffer)]
    public class MemoryBufferInstance
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public MemoryBufferInstance()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the object during deserialization.
        /// </summary>
        [OnDeserializing]
        private void Initialize(StreamingContext context)
        {
            Initialize();
        }

        /// <summary>
        /// Sets private members to default values.
        /// </summary>
        private void Initialize()
        {
            Name = null;
            TagCount = 0;
            DataType = null;
        }

        /// <summary>
        /// The browse name for the instance.
        /// </summary>
        [DataMember(Order = 1)]
        public string Name { get; set; }

        /// <summary>
        /// The number of tags in the buffer.
        /// </summary>
        [DataMember(Order = 2)]
        public int TagCount { get; set; }

        /// <summary>
        /// The data type of the tags in the buffer.
        /// </summary>
        [DataMember(Order = 3)]
        public string DataType { get; set; }
    }

    /// <summary>
    /// A collection of MemoryBufferInstances.
    /// </summary>
    [CollectionDataContract(
        Name = "ListOfMemoryBufferInstance",
        Namespace = Namespaces.MemoryBuffer,
        ItemName = "MemoryBufferInstance"
    )]
    public class MemoryBufferInstanceCollection : List<MemoryBufferInstance>;
}
