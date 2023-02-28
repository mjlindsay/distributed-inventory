using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedInventory.Application.Commands.Responses;
using DistributedInventory.Domain.Models;
using DistributedInventory.Infrastructure;
using DistributedInventory.Infrastructure.Context;
using MediatR;

namespace DistributedInventory.Application.Commands.Handlers
{
    public class CreateInventoryCountCommandHandler : IRequestHandler<CreateInventoryCountCommand, CreateInventoryCountCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateInventoryCountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateInventoryCountCommandResponse> Handle(CreateInventoryCountCommand request, CancellationToken cancellationToken)
        {
            var inventoryCount = InventoryCount.CreateNew(
                request.Location,
                request.Description,
                request.Products);

            _unitOfWork.InventoryCountRepository.Create(inventoryCount);

            var result = await _unitOfWork.CommitAsync(cancellationToken);
            var createInventoryCountResult = result.FirstOrDefault(r => r is DataObject<InventoryCount>);
            if (createInventoryCountResult != null)
            {
                return new CreateInventoryCountCommandResponse(
                    Guid.Parse(createInventoryCountResult.Id), createInventoryCountResult.ETag);
            }

            throw new Exception("Error saving inventory count.");
        }
    }
}
