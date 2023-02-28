using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedInventory.Application.Queries.Responses;
using MediatR;

namespace DistributedInventory.Application.Queries
{
    public class ReadAllInventoryCountsFromLocationQuery : IRequest<ReadMultipleInventoryCountsQueryResponse>
    {
        public string LocationId { get; set; }

        public int PageSize { get; set; }

        public string ContinuationToken { get; set; }
    }
}
