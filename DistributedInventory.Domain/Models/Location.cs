using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Models
{
    public record class Location {

        public string LocationId { get; set; }

        public Address Address { get; set; }

        public Location(string locationId, Address address) {
            LocationId = locationId;
            Address = address;
        }
    }

}
