using DistributedInventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Infrastructure.Context
{
    public interface IDataObject<out T> where T : UniqueEntity
    {
        public string Id { get; }

        public string PartitionKey { get; }

        public EntityType Type { get; } // TODO: Let's put some stronger typing than 'string' here

        public T Data { get; }

        public string ETag { get; set; }

        public int TimeToLive { get; }

        public EntityState EntityState { get; set; }
    }
}
