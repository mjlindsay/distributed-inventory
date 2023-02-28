using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Entities
{
    public record class OCCEntity<T> where T : DomainEntity
    {
        public T Entity { get; init; }

        [JsonProperty("_etag")]
        string ETag { get; init; }

        public OCCEntity(T entity, string etag)
        {
            Entity = entity;
            this.ETag = etag;
        }
    }
}
