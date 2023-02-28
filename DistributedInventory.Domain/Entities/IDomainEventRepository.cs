using DistributedInventory.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Entities
{
    public interface IDomainEventRepository
    {
        public void Create(InventoryCountDomainEvent domainEvent);
    }
}
