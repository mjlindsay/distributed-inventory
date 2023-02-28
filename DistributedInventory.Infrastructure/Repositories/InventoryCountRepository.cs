using DistributedInventory.Domain.Entities;
using DistributedInventory.Domain.Models;
using DistributedInventory.Infrastructure.Context;
using DistributedInventory.Infrastructure.Exceptions;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Infrastructure.Repositories
{
    public class InventoryCountRepository : IInventoryCountRepository
    {
        #region Fields
        private IInventoryCountPartitionKeyProvider _partitionKeyProvider;

        private const EntityType InventoryCountEntityType = EntityType.InventoryCount;
        #endregion

        #region Properties
        public IContainerContext Context { get; private init; }
        #endregion

        public InventoryCountRepository(IContainerContext context,
            IInventoryCountPartitionKeyProvider partitionKeyProvider) {
            Context = context;
            _partitionKeyProvider = partitionKeyProvider;
        }


        public void Create(InventoryCount inventoryCount) {
            var newInventoryCountDataObject = new DataObject<InventoryCount>(
                inventoryCount.Id.ToString(),
                _partitionKeyProvider.GetPartitionKey(inventoryCount),
                EntityType.InventoryCount,
                string.Empty, // TODO: Verify this does not cause a bug -- worried about null vs string.Empty
                -1,
                EntityState.Created,
                inventoryCount);

            Context.Add(newInventoryCountDataObject);
        }

        public async Task<OCCEntity<InventoryCount>> ReadAsync(
            Guid id,
            string etag) {
            string idAsString = id.ToString();
            var cosmosRequestOptions = new ItemRequestOptions();
            if(!string.IsNullOrWhiteSpace(etag))
                cosmosRequestOptions.IfNoneMatchEtag = etag;

            try {
                var result = await Context.Container.ReadItemAsync<DataObject<InventoryCount>>(
                    idAsString,
                    new PartitionKey(_partitionKeyProvider.GetPartitionKey(idAsString)),
                    cosmosRequestOptions);

                if(result.Resource.Data.IsDeleted) {
                    throw new DomainObjectNotFoundException($"Domain object not found for Id: {id} / ETag: {etag}");
                }

                return new OCCEntity<InventoryCount>(result.Resource.Data, result.ETag);
            } catch (CosmosException exception) {
                throw EvaluateCosmosError(exception, id, etag);
            }
        }

        public async Task DeleteAsync(Guid id, string etag) {
            try {
                var result = await Context.Container.ReadItemAsync<DataObject<InventoryCount>>(id.ToString(),
                    new PartitionKey(_partitionKeyProvider.GetPartitionKey(id.ToString())));

                var resultDataObject = result.Resource;

                resultDataObject.Data.Delete();
                resultDataObject.EntityState = EntityState.Deleted;
                resultDataObject.ETag = string.IsNullOrWhiteSpace(etag)
                    ? result.ETag
                    : etag; // if etag provided, use from client else from read response

                Context.Add(resultDataObject);
            } catch (CosmosException e) {
                throw EvaluateCosmosError(e, id, etag);
            }
        }


        public void Update(InventoryCount inventoryCount, string etag) {
            var updateDataObject = new DataObject<InventoryCount>(
                inventoryCount.Id.ToString(),
                _partitionKeyProvider.GetPartitionKey(inventoryCount),
                EntityType.InventoryCount,
                etag,
                -1,
                EntityState.Updated,
                inventoryCount);

            Context.Add(updateDataObject);
        }

        public async Task<BulkOCCEntityResult<InventoryCount>> ReadAllAsync(int pageSize,
            string continuationToken) {
            var inventoryCounts = new List<OCCEntity<InventoryCount>>();

            // TODO: Remove magic query string
            const string sqlQueryText = "SELECT * FROM c WHERE c.type = @type AND c.data.isDeleted = false"; 

            var queryDefinition = new QueryDefinition(sqlQueryText).WithParameter("@type", EntityType.InventoryCount.ToString());

            var queryResultSetIterator =
                Context.Container.GetItemQueryIterator<DataObject<InventoryCount>>(queryDefinition,
                    string.IsNullOrEmpty(continuationToken) ? null : continuationToken,
                    new QueryRequestOptions { MaxItemCount = pageSize });

            try {
                if (!queryResultSetIterator.HasMoreResults) {
                    return new BulkOCCEntityResult<InventoryCount>(new List<OCCEntity<InventoryCount>>());
                }

                var currentResultSet = await queryResultSetIterator.ReadNextAsync();

                inventoryCounts.AddRange(
                    currentResultSet.Select(dataObject => new OCCEntity<InventoryCount>(dataObject.Data, dataObject.ETag)));

                return new BulkOCCEntityResult<InventoryCount>(inventoryCounts, continuationToken);

            } catch (CosmosException e) {
                throw EvaluateCosmosError(e);
            }
        }

        public async Task<BulkOCCEntityResult<InventoryCount>> ReadAllFromLocationAsync(
            string locationId,
            int pageSize,
            string continuationToken)
        {
            var inventoryCounts = new List<OCCEntity<InventoryCount>>();

            // TODO: Remove magic query string
            const string sqlQueryText = "SELECT * FROM c WHERE c.type = @type AND c.data.isDeleted = false AND c.partitionKey = @locationId";

            var queryDefinition = new QueryDefinition(sqlQueryText)
                .WithParameter("@type", EntityType.InventoryCount.ToString())
                .WithParameter("@locationId", locationId);

            var queryResultSetIterator =
                Context.Container.GetItemQueryIterator<DataObject<InventoryCount>>(queryDefinition,
                    string.IsNullOrEmpty(continuationToken) ? null : continuationToken,
                    new QueryRequestOptions { MaxItemCount = pageSize });

            try
            {
                if (!queryResultSetIterator.HasMoreResults)
                {
                    return new BulkOCCEntityResult<InventoryCount>(new List<OCCEntity<InventoryCount>>());
                }

                var currentResultSet = await queryResultSetIterator.ReadNextAsync();

                inventoryCounts.AddRange(
                    currentResultSet.Select(dataObject => new OCCEntity<InventoryCount>(dataObject.Data, dataObject.ETag)));

                return new BulkOCCEntityResult<InventoryCount>(inventoryCounts, continuationToken);

            }
            catch (CosmosException e)
            {
                throw EvaluateCosmosError(e);
            }
        }

        private Exception EvaluateCosmosError(CosmosException exception, Guid? id = null, string etag = null) {
            return exception.StatusCode switch {
                HttpStatusCode.NotFound => new DomainObjectNotFoundException(
                    $"Domain object not found for Id: {(id != null ? id.Value : string.Empty)} / ETag: {etag}"),

                HttpStatusCode.NotModified => new DomainObjectNotModifiedException(
                    $"Domain object not modified. Id: {(id != null ? id.Value : string.Empty)} / ETag: {etag}"),

                HttpStatusCode.Conflict => new DomainObjectConflictException(
                    $"Domain object mid-air collision detected. Id: {(id != null ? id.Value : string.Empty)} / ETag: {etag}"),

                HttpStatusCode.TooManyRequests => new DomainObjectTooManyRequestsException(
                    $"Too many requests occurred. Try again later: ({exception.RetryAfter?.Milliseconds ?? -1} ms)"),
                _ => new Exception("Cosmos Exception", exception)
            };
        }
    }
}
