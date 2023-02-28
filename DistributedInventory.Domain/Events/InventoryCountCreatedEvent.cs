using DistributedInventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Events
{
    public record class InventoryCountCreatedEvent : InventoryCountDomainEvent
    {
        public InventoryCount InventoryCount { get; private set; }

        public InventoryCountCreatedEvent(InventoryCount count) 
            : base(Guid.NewGuid(), count.Location.LocationId, DomainEventTypes.Created) {

            InventoryCount = count;
        }
    }
}
