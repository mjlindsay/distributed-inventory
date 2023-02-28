using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedInventory.Domain.Models;

namespace DistributedInventory.Application.Models
{
    public record class CreateInventoryCountDto(
        Location location,
        string description,
        List<Product> products
        );
}
