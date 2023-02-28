using DistributedInventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Entities
{
    public interface IInventoryCountPartitionKeyProvider
    {
        public string GetPartitionKey(InventoryCount inventoryCount);

        public string GetPartitionKey(string id);
    }
}
