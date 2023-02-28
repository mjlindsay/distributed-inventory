using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Application.Commands.Responses
{
    public interface InventoryCountCommandResponse
    {
        public Guid Id { get; init; }

        public string ETag { get; init; }
    }
}
