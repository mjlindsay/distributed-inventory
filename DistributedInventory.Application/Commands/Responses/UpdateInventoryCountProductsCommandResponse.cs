using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Application.Commands.Responses
{
    public record class UpdateInventoryCountProductsCommandResponse(Guid Id, string ETag)
        : InventoryCountCommandResponse
    { }
}
