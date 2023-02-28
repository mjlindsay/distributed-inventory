using DistributedInventory.Domain.Entities;
using DistributedInventory.Domain.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Models
{
    public record class InventoryCount : DomainEntity
    {
        /**
         * public Guid Id { get; init; } -- as a result of inheriting from DomainEntity
         */

        public Location Location { get; init; }

        public string Description { get; init; }

        [JsonConverter(typeof(StringEnumConverter))]
        public InventoryCountStatus Status { get; private set; }

        public List<Product> Products { get; private set; }

        private InventoryCount(Location location, string description) {
            CreatedAt = DateTime.UtcNow;

            Id = Guid.NewGuid();
            
            Location = location;

            Description = description;

            Status = InventoryCountStatus.Started;

            Products = new List<Product>();
        }

        private InventoryCount(Location location, string description, IEnumerable<Product>? products) {
            CreatedAt = DateTime.UtcNow;

            Id = Guid.NewGuid();

            Location = location;

            Description = description;

            Status = InventoryCountStatus.Started;

            Products = products is null ? new() : products.ToList();
        }

        private InventoryCount(Guid id, Location location, string description, IEnumerable<Product>? products) {
            CreatedAt = DateTime.UtcNow;

            Id = id;

            Location = location;

            Description = description;

            Status = InventoryCountStatus.Started;

            Products = products is null ? new() : products.ToList();
        }

        public static InventoryCount CreateNew(Location location, string description) {
            var count = new InventoryCount(location, description);

            count.AddEvent(new InventoryCountCreatedEvent(count));

            return count;
        }

        public static InventoryCount CreateNew(Location location, string description, IEnumerable<Product>? products) {
            var count = new InventoryCount(location, description, products);

            count.AddEvent(new InventoryCountCreatedEvent(count));

            return count;
        }

        public void UpdateStatus(InventoryCountStatus status) {
            Status = status;

            if (IsNew) return;

            AddEvent(new InventoryCountStatusUpdatedEvent(Location.LocationId, Status));
            ModifiedAt = DateTime.UtcNow;
        }

        public void Delete() {
            if(IsNew) return;

            AddEvent(new InventoryCountDeletedEvent(Location.LocationId));
            DeletedAt = DateTime.UtcNow;
            IsDeleted = true;
        }

        #region Product Changes
        public void UpsertProducts(IEnumerable<Product> products) {
            var productsToBeUpdated = products.Where(
                changedProduct => this.Products.Any(existingProduct =>
                    existingProduct.ProductCode == changedProduct.ProductCode));

            // TODO: Come back to this to verify logic works
            var productsToBeAdded = products.Where(
                changedProduct => !productsToBeUpdated.Any(
                    toBeUpdatedProduct => toBeUpdatedProduct.ProductCode == changedProduct.ProductCode));

            if(productsToBeUpdated.Count() > 0) {
                UpdateProducts(productsToBeUpdated);
            }

            if(productsToBeAdded.Count() > 0) {
                AddProducts(productsToBeAdded);
            }
        }

        public void UpdateProducts(IEnumerable<Product> updatedProducts) {
            var productCodesToUpdate = updatedProducts.Select(updatedProduct => updatedProduct.ProductCode);
            Products = Products.Select(product => {
                if (productCodesToUpdate.Contains(product.ProductCode)) {
                    var updatedProduct = updatedProducts
                        .First(updatedProduct => updatedProduct.ProductCode == product.ProductCode);
                    var newProduct = product with {
                        Description = updatedProduct.Description,
                        UnitPrice = updatedProduct.UnitPrice,
                        ExpectedQuantity = updatedProduct.ExpectedQuantity,
                        ActualQuantity = updatedProduct.ActualQuantity,
                    };

                    return newProduct;
                } else {
                    return product;
                }
            }).ToList();

            if (IsNew) return;

            AddEvent(new InventoryCountProductsUpdatedEvent(Location.LocationId, updatedProducts));
            ModifiedAt = DateTime.Now;
        }

        private void AddProducts(IEnumerable<Product> productsToAdd) {
            Products.AddRange(productsToAdd);

            if (IsNew) return;

            AddEvent(new InventoryCountProductsAddedEvent(Location.LocationId, productsToAdd));
            ModifiedAt = DateTime.UtcNow;
        }

        public void DeleteProducts(IEnumerable<Product> productsToRemove) {
            var productCodesToRemove = productsToRemove
                .Select(updatedProduct => updatedProduct.ProductCode);

            Products = Products
                .Where(existingProduct =>
                    productCodesToRemove.Contains(existingProduct.ProductCode))
                .ToList();

            if (IsNew) return;

            AddEvent(new InventoryCountProductsRemovedEvent(Location.LocationId, productsToRemove));
            ModifiedAt = DateTime.UtcNow;
        }

        #endregion
    }
}
