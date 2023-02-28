using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Entities
{
    public abstract record class UniqueEntity : IUniqueEntity
    {
        [JsonProperty]
        public Guid Id { get; init; }
    }
}
