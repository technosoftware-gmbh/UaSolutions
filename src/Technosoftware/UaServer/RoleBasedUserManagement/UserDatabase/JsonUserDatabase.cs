#region Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is subject to the Technosoftware GmbH Software License 
// Agreement, which can be found here:
// https://technosoftware.com/documents/Source_License_Agreement.pdf
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2011-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Logging;
using Opc.Ua;
#endregion

namespace Technosoftware.UaServer.UserDatabase
{
    /// <summary>
    /// A user database with JSON storage.
    /// </summary>
    /// <remarks>
    /// This db is used for testing, not for production.
    /// </remarks>
    public class JsonUserDatabase : LinqUserDatabase
    {
        /// <summary>
        /// Create a JSON database.
        /// </summary>
        public JsonUserDatabase(string fileName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// Load the JSON application database.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> is <c>null</c>.</exception>
        public static IUaUserDatabase Load(string fileName, ITelemetryContext telemetry)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            try
            {
                if (File.Exists(fileName))
                {
                    byte[] utf8Json = File.ReadAllBytes(fileName);
                    var utf8JsonReader = new Utf8JsonReader(utf8Json, s_allowTrailingCommasInJsonReader);
                    JsonUserDatabase db = JsonSerializer.Deserialize<JsonUserDatabase>(ref utf8JsonReader, s_exchangeJsonSerializerOptions);
                    db.FileName = fileName;
                    return db;
                }
            }
            catch
            {
                ILogger logger = telemetry.CreateLogger<JsonUserDatabase>();
                logger.LogWarning("User database {FileName} was not found.", fileName);
            }
            return new JsonUserDatabase(fileName);
        }

        /// <summary>
        /// Save the complete database.
        /// </summary>
        protected override void Save()
        {
            byte[] utf8Json = JsonSerializer.SerializeToUtf8Bytes(
                this,
                s_exchangeJsonSerializerOptions);
            File.WriteAllBytes(FileName, utf8Json);
        }

        /// <summary>
        /// Get or set the filename.
        /// </summary>
        [IgnoreDataMember]
        [JsonIgnore]
        public string FileName { get; private set; }

        private static readonly JsonSerializerOptions s_exchangeJsonSerializerOptions = new()
        {
            AllowTrailingCommas = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            TypeInfoResolver = new SystemRuntimeSerializationAttributeResolver()
        };

        private static readonly JsonReaderOptions s_allowTrailingCommasInJsonReader = new()
        {
            AllowTrailingCommas = true
        };

        private class SystemRuntimeSerializationAttributeResolver : DefaultJsonTypeInfoResolver
        {
            public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
            {
                JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

                if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object &&
                    type.GetCustomAttribute<DataContractAttribute>() is not null)
                {
                    jsonTypeInfo.Properties.Clear();

                    foreach ((PropertyInfo propertyInfo, DataMemberAttribute attr) in type
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .Select((prop) => (prop, prop.GetCustomAttribute<DataMemberAttribute>()))
                        .Where((x) => x.Item2 != null)
                        .OrderBy((x) => x.Item2!.Order))
                    {
                        JsonPropertyInfo jsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(
                            propertyInfo.PropertyType,
                            attr.Name ?? propertyInfo.Name);
                        jsonPropertyInfo.Get =
                            propertyInfo.CanRead
                            ? propertyInfo.GetValue
                            : null;

                        jsonPropertyInfo.Set = propertyInfo.CanWrite
                            ? propertyInfo.SetValue
                            : null;

                        jsonTypeInfo.Properties.Add(jsonPropertyInfo);
                    }
                }

                return jsonTypeInfo;
            }
        }
    }
}
