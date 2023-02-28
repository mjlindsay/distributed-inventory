using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedInventory.Domain.Models;

namespace DistributedInventory.Application.Queries.Responses
{
    public record class ReadMultipleInventoryCountsQueryResponse
        (bool HasMore, string ContinuationToken, List<InventoryCount> inventoryCounts);
}
