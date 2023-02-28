using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Infrastructure.Context
{
    public enum EntityState
    {
        Created,
        Updated,
        Deleted,
        Unmodified,
    }
}
