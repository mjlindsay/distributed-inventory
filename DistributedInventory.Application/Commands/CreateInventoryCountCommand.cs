using DistributedInventory.Application.Commands.Responses;
using DistributedInventory.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Application.Commands
{
    public class CreateInventoryCountCommand : IRequest<CreateInventoryCountCommandResponse>
    {
        public Location Location { get; set; }

        public string Description { get; set; }

        public InventoryCountStatus Status { get; set; } = InventoryCountStatus.Started;

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
