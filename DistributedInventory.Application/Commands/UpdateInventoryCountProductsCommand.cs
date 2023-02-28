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
    public class UpdateInventoryCountProductsCommand : IRequest<UpdateInventoryCountProductsCommandResponse>
    {
        public List<Product> Products { get; set; }
    }
}
