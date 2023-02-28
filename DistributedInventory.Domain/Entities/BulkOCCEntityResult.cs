using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Entities
{
    public class BulkOCCEntityResult<T> where T : DomainEntity
    {
        public IEnumerable<OCCEntity<T>> Entities { get; init; }

        public bool HasContinuationToken { get; init; }

        public string ContinuationToken { get; init; }

        public BulkOCCEntityResult(IEnumerable<OCCEntity<T>> entities, string continuationToken) {
            Entities = entities;
            HasContinuationToken = !string.IsNullOrWhiteSpace(continuationToken);
            ContinuationToken = continuationToken;
        }

        public BulkOCCEntityResult(IEnumerable<OCCEntity<T>> entities) : this(entities, string.Empty) { }

    }
}
