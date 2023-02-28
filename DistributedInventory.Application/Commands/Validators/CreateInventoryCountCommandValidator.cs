using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedInventory.Domain.Models.Validators;
using FluentValidation;

namespace DistributedInventory.Application.Commands.Validators
{
    public class CreateInventoryCountCommandValidator : AbstractValidator<CreateInventoryCountCommand>
    {
        public CreateInventoryCountCommandValidator() {
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
