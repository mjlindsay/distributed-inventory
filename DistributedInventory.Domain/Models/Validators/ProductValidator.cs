using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Models.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        const int ProductCodeMinimumLength = 1;
        public ProductValidator() {
            RuleFor(property => property.ProductCode)
                .NotNull()
                .NotEmpty()
                .MinimumLength(ProductCodeMinimumLength);

            RuleFor(property => property.Description)
                .NotNull()
                .NotEmpty();

            RuleFor(property => property.UnitPrice)
                .NotNull();

            RuleFor(property => property.ExpectedQuantity)
                .NotNull();

            RuleFor(property => property.ActualQuantity)
                .NotNull();
        }

    }
}
