using DistributedInventory.Domain.Entities;
using DistributedInventory.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IContainerContext _context;

        public IInventoryCountRepository InventoryCountRepository { get; private init; }

        public UnitOfWork(IContainerContext context, IInventoryCountRepository inventoryCountRepository) {
            _context = context;
            InventoryCountRepository = inventoryCountRepository;
        }

        public async Task<IEnumerable<IDataObject<UniqueEntity>>> CommitAsync(CancellationToken cancellationToken = default) {
            return await _context.SaveAsync(cancellationToken);
        }
    }
}
