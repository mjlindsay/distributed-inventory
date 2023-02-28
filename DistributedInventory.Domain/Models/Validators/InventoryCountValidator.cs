using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Models.Validators
{
    public class InventoryCountValidator : AbstractValidator<InventoryCount>
    {
        public InventoryCountValidator() {
            RuleFor(property => property.Location)
                .SetValidator(new LocationValidator());

            RuleFor(property => property.Description)
                .NotNull()
                .NotEmpty();

            var productValidator = new ProductValidator();
            RuleForEach(property => property.Products)
                .SetValidator(productValidator);
        }
    }
}
