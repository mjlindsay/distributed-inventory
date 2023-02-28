using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedInventory.Domain.Models;

namespace DistributedInventory.Domain.Entities
{
    public interface IInventoryCountRepository
    {
        public void Create(InventoryCount inventoryCount);

        public Task<OCCEntity<InventoryCount>> ReadAsync(Guid id, string etag);

        public Task DeleteAsync(Guid id, string etag);

        public Task<BulkOCCEntityResult<InventoryCount>> ReadAllAsync(int pageSize, string continuationToken);

        public Task<BulkOCCEntityResult<InventoryCount>> ReadAllFromLocationAsync(
            string locationId,
            int pageSize,
            string continuationToken);

        public void Update(InventoryCount inventoryCount, string etag);
    }
}
