using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedInventory.Application.Queries.Responses;
using DistributedInventory.Domain.Entities;
using DistributedInventory.Domain.Models;
using MediatR;

namespace DistributedInventory.Application.Queries
{
    public class ReadAllInventoryCountsQuery : IRequest<ReadMultipleInventoryCountsQueryResponse>
    {
        public int PageSize { get; set; }

        public string ContinuationToken { get; set; }
    }
}
