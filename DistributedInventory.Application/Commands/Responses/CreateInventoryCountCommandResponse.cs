using DistributedInventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Application.Commands.Responses
{
    public record class CreateInventoryCountCommandResponse(Guid Id, string ETag)
        : InventoryCountCommandResponse { }
}
