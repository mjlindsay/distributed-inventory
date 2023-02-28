using DistributedInventory.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Infrastructure.Context
{
    public class DataObject<T> : IDataObject<T> where T : UniqueEntity {

        #region Properties
        [JsonProperty]
        public string Id { get; private init; }

        [JsonProperty]
        public string PartitionKey { get; private init; }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public EntityType Type { get; private init; }

        public T Data { get; set; }

        [JsonProperty("_etag")]
        public string ETag { get; set; }

        [JsonProperty("ttl")]
        public int TimeToLive { get; private init; }

        [JsonIgnore]
        public EntityState EntityState { get; set; }
        #endregion

        public DataObject(
            string id,
            string partitionKey,
            EntityType entityType,
            string etag,
            int timeToLive,
            EntityState entityState,
            T entityObject) {

            Id = id;
            PartitionKey = partitionKey;
            Type = entityType;
            ETag = etag;
            TimeToLive = timeToLive;
            EntityState = entityState;
            Data = entityObject;
        }
    }
}
