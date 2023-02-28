using DistributedInventory.Domain.Entities;
using DistributedInventory.Domain.Events;
using DistributedInventory.Infrastructure.Context;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Infrastructure.Repositories
{
    public class DomainEventRepository : IDomainEventRepository
    {
        private readonly IConfiguration _cfg;
        private IContainerContext Context { get; }

        private const EntityType EventEntityType = EntityType.Event;
        private readonly int DEFAULT_TTL;

        public DomainEventRepository(IContainerContext ctx, IConfiguration cfg) {
            _cfg = cfg;
            DEFAULT_TTL = _cfg.GetSection("Events")?["Ttl"] == null
                ? 120
                : int.Parse(_cfg.GetSection("Events")?["Ttl"]);
            Context = ctx;
        }

        public void Create(InventoryCountDomainEvent domainEvent) {
            var dataObject = new DataObject<InventoryCountDomainEvent>(
                domainEvent.Id.ToString(),
                domainEvent.InventoryCountLocationId,
                EventEntityType,
                string.Empty,
                DEFAULT_TTL,
                EntityState.Created,
                domainEvent);

            Context.Add(dataObject);
        }
    }
}
