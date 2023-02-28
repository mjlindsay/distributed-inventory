using DistributedInventory.Domain.Entities;
using DistributedInventory.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Infrastructure
{
    public interface IUnitOfWork
    {
        IInventoryCountRepository InventoryCountRepository { get; }

        Task<IEnumerable<IDataObject<UniqueEntity>>> CommitAsync(CancellationToken cancellationToken = default);
    }
}
