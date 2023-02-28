using DistributedInventory.Application.Commands.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Application.Commands
{
    public class DeleteInventoryCountCommand : IRequest<DeleteInventoryCountCommandResponse>
    {
        public Guid Id { get; set; }
        
        public string ETag { get; set; }
    }
}
