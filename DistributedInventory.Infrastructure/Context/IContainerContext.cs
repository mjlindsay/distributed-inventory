using DistributedInventory.Domain.Entities;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Infrastructure.Context
{
    public interface IContainerContext
    {
        public Container Container { get; }

        public List<IDataObject<UniqueEntity>> DataObjects { get; }

        public void Add(IDataObject<UniqueEntity> entity);

        public Task<List<IDataObject<UniqueEntity>>> SaveAsync(CancellationToken cancellationToken);

        public void Reset();
    }
}
