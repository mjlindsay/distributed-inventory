using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DistributedInventory.Domain.Events
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DomainEventTypes
    {
        Created = 10,
        Deleted = 20,
        StatusUpdated = 30,
        ProductsAdded = 40,
        ProductsUpdated = 50,
        ProductsRemoved = 60,
    }
}
