using DistributedInventory.Domain.Entities;
using DistributedInventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Events
{
    public record class InventoryCountProductsRemovedEvent : InventoryCountDomainEvent, IDomainEvent
    {
        public List<Product> Products { get; }

        public InventoryCountProductsRemovedEvent(string inventoryCountLocationId, IEnumerable<Product> products)
            : base(Guid.NewGuid(), inventoryCountLocationId, DomainEventTypes.ProductsRemoved) {
            Products = products.ToList();
        }
    }
}
