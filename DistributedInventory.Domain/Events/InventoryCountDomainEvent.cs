using DistributedInventory.Domain.Entities;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Events
{
    public record class InventoryCountDomainEvent : UniqueEntity, IDomainEvent
    {
        public string InventoryCountLocationId { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DomainEventTypes Action { get; }

        private DateTime CreatedAt { get; }

        public InventoryCountDomainEvent(Guid id, string inventoryCountLocationId, DomainEventTypes action) {
            Id = id;
            InventoryCountLocationId = inventoryCountLocationId;
            Action = action;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
