using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Events
{
    public interface IDomainEventEmitter<T> where T : IDomainEvent
    {
        public void AddEvent(T domainEvent);

        public void RemoveEvent(T domainEvent);

        public void RemoveAllEvents();

        public IEnumerable<T> DomainEvents { get; }

    }
}
