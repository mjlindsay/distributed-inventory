using DistributedInventory.Domain.Entities;
using DistributedInventory.Domain.Events;
using DistributedInventory.Infrastructure.Exceptions;
using MediatR;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Infrastructure.Context
{
    public class CosmosContainerContext : IContainerContext
    {
        #region Fields
        private readonly IMediator _mediator;
        #endregion

        #region Properties
        public Container Container { get; }

        public List<IDataObject<UniqueEntity>> DataObjects { get; }
        #endregion

        public CosmosContainerContext(Container container, IMediator mediator) {
            Container = container;
            _mediator = mediator;

            DataObjects = new List<IDataObject<UniqueEntity>>();
        }

        public void Add(IDataObject<UniqueEntity> entity) {
            if (DataObjects.FindIndex(0,
                    o => o.Id == entity.Id && o.PartitionKey == entity.PartitionKey) == -1)
                DataObjects.Add(entity);
        }

        public void Reset() {
            DataObjects.Clear();
        }

        private async Task<List<IDataObject<UniqueEntity>>> SaveInTransactionalBatchAsync(
            CancellationToken cancellationToken) {
            if (DataObjects.Count > 0) {
                var pk = new PartitionKey(DataObjects[0].PartitionKey);
                var tb = Container.CreateTransactionalBatch(pk);
                DataObjects.ForEach(o => {
                    TransactionalBatchItemRequestOptions tro = null;

                    if (!string.IsNullOrWhiteSpace(o.ETag))
                        tro = new TransactionalBatchItemRequestOptions { IfMatchEtag = o.ETag };

                    switch (o.EntityState) {
                        case EntityState.Created:
                            tb.CreateItem(o);
                            break;
                        case EntityState.Updated or EntityState.Deleted:
                            tb.ReplaceItem(o.Id, o, tro);
                            break;
                    }
                });

                var tbResult = await tb.ExecuteAsync(cancellationToken);

                if (!tbResult.IsSuccessStatusCode)
                    for (var i = 0; i < DataObjects.Count; i++)
                        if (tbResult[i].StatusCode != HttpStatusCode.FailedDependency) {
                            // Not recoverable - clear context
                            DataObjects.Clear();
                            throw EvaluateCosmosError(tbResult[i].StatusCode);
                        }

                for (var i = 0; i < DataObjects.Count; i++)
                    DataObjects[i].ETag = tbResult[i].ETag;
            }

            var result = new List<IDataObject<UniqueEntity>>(DataObjects); // return copy of list as result

            // work has been successfully done - reset DataObjects list
            DataObjects.Clear();
            return result;
        }

        private async Task<List<IDataObject<UniqueEntity>>> SaveSingleAsync(IDataObject<UniqueEntity> dObj,
            CancellationToken cancellationToken = default) {
            var reqOptions = new ItemRequestOptions {
                EnableContentResponseOnWrite = false
            };

            if (!string.IsNullOrWhiteSpace(dObj.ETag)) reqOptions.IfMatchEtag = dObj.ETag;

            var pk = new PartitionKey(dObj.PartitionKey);

            try {
                ItemResponse<IDataObject<UniqueEntity>> response;

                switch (dObj.EntityState) {
                    case EntityState.Created:
                        response = await Container.CreateItemAsync(dObj, pk, reqOptions, cancellationToken);
                        break;
                    case EntityState.Updated:
                    case EntityState.Deleted:
                        response = await Container.ReplaceItemAsync(dObj, dObj.Id, pk, reqOptions, cancellationToken);
                        break;
                    default:
                        DataObjects.Clear();
                        return new List<IDataObject<UniqueEntity>>();
                }

                dObj.ETag = response.ETag;
                var result = new List<IDataObject<UniqueEntity>>(1) { dObj };

                // work has been successfully done - reset DataObjects list
                DataObjects.Clear();
                return result;
            } catch (CosmosException e) {
                // Not recoverable - clear context
                DataObjects.Clear();
                throw EvaluateCosmosError(e, Guid.Parse(dObj.Id), dObj.ETag);
            }
        }

        public async Task<List<IDataObject<UniqueEntity>>> SaveAsync(CancellationToken cancellationToken) {
            RaiseDomainEvents(DataObjects);

            switch (DataObjects.Count) {
                case 1: {
                        var result = await SaveSingleAsync(DataObjects[0], cancellationToken);
                        return result;
                    }
                case > 1: {
                        var result = await SaveInTransactionalBatchAsync(cancellationToken);
                        return result;
                    }
                default:
                    return new List<IDataObject<UniqueEntity>>();
            }
        }


        private void RaiseDomainEvents(List<IDataObject<UniqueEntity>> dObjs) {
            var eventEmitters = new List<IDomainEventEmitter<IDomainEvent>>();

            // Get all EventEmitters
            foreach (var o in dObjs)
                if (o.Data is IDomainEventEmitter<IDomainEvent> ee)
                    eventEmitters.Add(ee);

            // Raise Events
            if (eventEmitters.Count <= 0) return;
            foreach (var evt in eventEmitters.SelectMany(eventEmitter => eventEmitter.DomainEvents))
                _mediator.Publish(evt);
        }

        private Exception EvaluateCosmosError(CosmosException error, Guid? id = null, string etag = null) {
            return EvaluateCosmosError(error.StatusCode, id, etag);
        }

        private Exception EvaluateCosmosError(HttpStatusCode statusCode, Guid? id = null, string etag = null) {
            return statusCode switch {
                HttpStatusCode.NotFound => new DomainObjectNotFoundException(
                    $"Domain object not found for Id: {(id != null ? id.Value : string.Empty)} / ETag: {etag}"),
                HttpStatusCode.NotModified => new DomainObjectNotModifiedException(
                    $"Domain object not modified. Id: {(id != null ? id.Value : string.Empty)} / ETag: {etag}"),
                HttpStatusCode.Conflict => new DomainObjectConflictException(
                    $"Domain object conflict detected. Id: {(id != null ? id.Value : string.Empty)} / ETag: {etag}"),
                HttpStatusCode.PreconditionFailed => new DomainObjectPreconditionFailedException(
                    $"Domain object mid-air collision detected. Id: {(id != null ? id.Value : string.Empty)} / ETag: {etag}"),
                HttpStatusCode.TooManyRequests => new DomainObjectTooManyRequestsException(
                    "Too many requests occurred. Try again later)"),
                _ => new Exception("Cosmos Exception")
            };
        }
    }
}
