using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Models
{
    public record class Product
    {
        public string ProductCode { get; set; }

        public string Description { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal ExpectedQuantity { get; set; }

        public decimal ActualQuantity { get; set; }

        public Product(string productCode,
            string description,
            decimal unitPrice,
            decimal expectedQuantity,
            decimal actualQuantity)
        {

            ProductCode = productCode;
            Description = description;
            UnitPrice = unitPrice;
            ExpectedQuantity = expectedQuantity;
            ActualQuantity = actualQuantity;
        }
    }
}
