using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Entities
{
    internal interface IUniqueEntity
    {
        public Guid Id { get; init; }
    }
}
