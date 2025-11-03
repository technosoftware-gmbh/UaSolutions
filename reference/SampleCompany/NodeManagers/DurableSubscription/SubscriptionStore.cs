#region Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on the OPC Foundation MIT License. 
// The complete license agreement for that can be found here:
// http://opcfoundation.org/License/MIT/1.00/
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2025 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opc.Ua;
using Technosoftware.UaServer;
using Technosoftware.UaServer.Subscriptions;
#endregion Using Directives

namespace SampleCompany.NodeManagers.DurableSubscription
{
    public class SubscriptionStore : IUaSubscriptionStore
    {
        private static readonly JsonSerializerSettings s_settings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            Converters = { new ExtensionObjectConverter(), new NumericRangeConverter() }
        };

        private static readonly string s_storage_path = Path.Combine(
            Environment.CurrentDirectory,
            "Durable Subscriptions");

        private const string kFilename = "subscriptionsStore.txt";
        private readonly DurableMonitoredItemQueueFactory m_durableMonitoredItemQueueFactory;

        public SubscriptionStore(IUaServerData server)
        {
            m_durableMonitoredItemQueueFactory = server
                .MonitoredItemQueueFactory as DurableMonitoredItemQueueFactory;
        }

        public bool StoreSubscriptions(IEnumerable<IUaStoredSubscription> subscriptions)
        {
            try
            {
                string result = JsonConvert.SerializeObject(subscriptions, s_settings);

                if (!Directory.Exists(s_storage_path))
                {
                    Directory.CreateDirectory(s_storage_path);
                }

                File.WriteAllText(Path.Combine(s_storage_path, kFilename), result);

                if (m_durableMonitoredItemQueueFactory != null)
                {
                    IEnumerable<uint> ids = subscriptions.SelectMany(
                        s => s.MonitoredItems.Select(m => m.Id));
                    m_durableMonitoredItemQueueFactory.PersistQueues(ids, s_storage_path);
                }
                return true;
            }
            catch (Exception ex)
            {
                Opc.Ua.Utils.LogWarning(ex, "Failed to store subscriptions");
            }
            return false;
        }

        public RestoreSubscriptionResult RestoreSubscriptions()
        {
            string filePath = Path.Combine(s_storage_path, kFilename);
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    List<IUaStoredSubscription> result = JsonConvert
                        .DeserializeObject<List<IUaStoredSubscription>>(
                            json,
                            s_settings);

                    File.Delete(filePath);

                    return new RestoreSubscriptionResult(true, result);
                }
            }
            catch (Exception ex)
            {
                Opc.Ua.Utils.LogWarning(ex, "Failed to restore subscriptions");
            }

            return new RestoreSubscriptionResult(false, null);
        }

        public class ExtensionObjectConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(ExtensionObject);
            }

            public override object ReadJson(
                JsonReader reader,
                Type objectType,
                object existingValue,
                JsonSerializer serializer)
            {
                var jo = JObject.Load(reader);
                object body = jo["Body"].ToObject<object>(serializer);
                ExpandedNodeId typeId = jo["TypeId"].ToObject<ExpandedNodeId>(serializer);
                return new ExtensionObject { Body = body, TypeId = typeId };
            }

            public override void WriteJson(
                JsonWriter writer,
                object value,
                JsonSerializer serializer)
            {
                var extensionObject = (ExtensionObject)value;
                var jo = new JObject
                {
                    ["Body"] = JToken.FromObject(extensionObject.Body, serializer),
                    ["TypeId"] = JToken.FromObject(extensionObject.TypeId, serializer)
                };
                jo.WriteTo(writer);
            }
        }

        public class NumericRangeConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(NumericRange);
            }

            public override object ReadJson(
                JsonReader reader,
                Type objectType,
                object existingValue,
                JsonSerializer serializer)
            {
                var jo = JObject.Load(reader);
                int begin = jo["Begin"].ToObject<int>(serializer);
                int end = jo["End"].ToObject<int>(serializer);
                return new NumericRange(begin, end);
            }

            public override void WriteJson(
                JsonWriter writer,
                object value,
                JsonSerializer serializer)
            {
                var extensionObject = (NumericRange)value;
                var jo = new JObject
                {
                    ["Begin"] = JToken.FromObject(extensionObject.Begin, serializer),
                    ["End"] = JToken.FromObject(extensionObject.End, serializer)
                };
                jo.WriteTo(writer);
            }
        }

        public IUaDataChangeMonitoredItemQueue RestoreDataChangeMonitoredItemQueue(
            uint monitoredItemId)
        {
            return m_durableMonitoredItemQueueFactory?.RestoreDataChangeQueue(
                monitoredItemId,
                s_storage_path);
        }

        public IUaEventMonitoredItemQueue RestoreEventMonitoredItemQueue(uint monitoredItemId)
        {
            return m_durableMonitoredItemQueueFactory?.RestoreEventQueue(
                monitoredItemId,
                s_storage_path);
        }

        public void OnSubscriptionRestoreComplete(Dictionary<uint, uint[]> createdSubscriptions)
        {
            string filePath = Path.Combine(s_storage_path, kFilename);

            //remove old file
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    Opc.Ua.Utils.LogWarning(ex, "Failed to cleanup files for stored subscsription");
                }
            }
            //remove old batches & queues
            if (m_durableMonitoredItemQueueFactory != null)
            {
                IEnumerable<uint> ids = createdSubscriptions.SelectMany(s => s.Value);
                m_durableMonitoredItemQueueFactory.CleanStoredQueues(s_storage_path, ids);
            }
        }
    }
}
