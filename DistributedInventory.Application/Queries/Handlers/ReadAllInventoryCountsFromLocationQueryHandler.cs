using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedInventory.Application.Queries.Responses;
using DistributedInventory.Domain.Entities;
using DistributedInventory.Domain.Models;
using DistributedInventory.Infrastructure;
using MediatR;

namespace DistributedInventory.Application.Queries.Handlers
{
    public class ReadAllInventoryCountsFromLocationQueryHandler : IRequestHandler<ReadAllInventoryCountsFromLocationQuery, ReadMultipleInventoryCountsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReadAllInventoryCountsFromLocationQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReadMultipleInventoryCountsQueryResponse> Handle(ReadAllInventoryCountsFromLocationQuery request,
            CancellationToken cancellationToken)
        {
            BulkOCCEntityResult<InventoryCount> resultBatch =
                await _unitOfWork.InventoryCountRepository.ReadAllFromLocationAsync(
                    request.LocationId,
                    request.PageSize,
                    request.ContinuationToken);

            if (resultBatch.Entities.Count() > 0)
            {
                return new ReadMultipleInventoryCountsQueryResponse(resultBatch.HasContinuationToken,
                    resultBatch.ContinuationToken, resultBatch.Entities.Select(x => x.Entity).ToList());
            }
            else
            {
                return new ReadMultipleInventoryCountsQueryResponse(resultBatch.HasContinuationToken,
                    resultBatch.ContinuationToken, new List<InventoryCount>());
            }
        }
    }
}
