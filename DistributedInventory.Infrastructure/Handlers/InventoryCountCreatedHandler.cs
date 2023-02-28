using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedInventory.Domain.Entities;
using DistributedInventory.Domain.Events;
using MediatR;

namespace DistributedInventory.Infrastructure.Handlers
{
    public class InventoryCountCreatedHandler : INotificationHandler<InventoryCountCreatedEvent>
    {
        private readonly IDomainEventRepository _eventRepository;

        public InventoryCountCreatedHandler(IDomainEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Task Handle(InventoryCountCreatedEvent notification, CancellationToken cancellationToken)
        {
            _eventRepository.Create(notification);
            return Task.CompletedTask;
        }
    }
}
