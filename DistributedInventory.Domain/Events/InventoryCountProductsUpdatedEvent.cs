using DistributedInventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Events
{
    public record class InventoryCountProductsUpdatedEvent : InventoryCountDomainEvent, IDomainEvent
    {
        public List<Product> Products { get; }

        public InventoryCountProductsUpdatedEvent(string inventoryCountLocationId, IEnumerable<Product> products)
            : base(Guid.NewGuid(), inventoryCountLocationId, DomainEventTypes.ProductsUpdated) {
            Products = products.ToList();
        }
    }
}
