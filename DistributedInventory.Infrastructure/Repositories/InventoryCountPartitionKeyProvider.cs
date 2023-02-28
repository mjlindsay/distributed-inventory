using DistributedInventory.Domain.Entities;
using DistributedInventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Infrastructure.Repositories
{
    public class InventoryCountPartitionKeyProvider : IInventoryCountPartitionKeyProvider
    {
        public string GetPartitionKey(InventoryCount inventoryCount) {
            return inventoryCount.Location.LocationId;
        }

        public string GetPartitionKey(string id) {
            return id;
        }
    }
}
