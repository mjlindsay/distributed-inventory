using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Events
{
    public record class InventoryCountDeletedEvent : InventoryCountDomainEvent
    {
        public InventoryCountDeletedEvent(string inventoryCountLocationId)
            : base(Guid.NewGuid(), inventoryCountLocationId, DomainEventTypes.Deleted) { }
    }
}
