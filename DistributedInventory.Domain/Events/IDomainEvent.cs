using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DistributedInventory.Domain.Events
{
    public interface IDomainEvent : INotification
    {
        public Guid Id { get; }

        public DomainEventTypes Action { get; }
    }
}
