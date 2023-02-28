using DistributedInventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Events
{
    public record class InventoryCountStatusUpdatedEvent : InventoryCountDomainEvent, IDomainEvent
    {

        InventoryCountStatus Status { get; }

        public InventoryCountStatusUpdatedEvent(string inventoryCountLocationId, InventoryCountStatus status)
            : base(Guid.NewGuid(), inventoryCountLocationId, DomainEventTypes.StatusUpdated) {
            Status = status;
        }
    }
}
