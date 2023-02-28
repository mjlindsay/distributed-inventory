using DistributedInventory.Domain.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Entities
{
    public record class DomainEntity : UniqueEntity, IDomainEventEmitter<IDomainEvent>
    {
        [JsonProperty]
        public DateTime CreatedAt { get; protected set; }

        [JsonProperty]
        public DateTime? ModifiedAt { get; protected set; }

        [JsonProperty]
        public DateTime? DeletedAt { get; protected set; }

        [JsonProperty]
        public bool IsDeleted { get; protected set; }

        [JsonIgnore]
        public bool IsNew { get; init; }

        #region Events
        private readonly List<IDomainEvent> _domainEvents = new();

        [JsonIgnore]
        public IEnumerable<IDomainEvent> DomainEvents { get { return _domainEvents; } }
        #endregion

        public void AddEvent(IDomainEvent domainEvent) {
            var i = _domainEvents.FindIndex(0, e => e.Action == domainEvent.Action);
            if (i < 0) {
                _domainEvents.Add(domainEvent);
            } else {
                _domainEvents.RemoveAt(i);
                _domainEvents.Insert(i, domainEvent);
            }
        }

        public void RemoveAllEvents() {
            _domainEvents.Clear();
        }

        public void RemoveEvent(IDomainEvent domainEvent) {
            _domainEvents.Remove(domainEvent);
        }
    }
}
