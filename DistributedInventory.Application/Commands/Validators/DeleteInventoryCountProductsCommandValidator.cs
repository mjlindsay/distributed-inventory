using DistributedInventory.Domain.Models.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Application.Commands.Validators
{
    public class DeleteInventoryCountProductsCommandValidator : AbstractValidator<DeleteInventoryCountProductsCommand>
    {
        public DeleteInventoryCountProductsCommandValidator() {
            var productsValidator = new ProductValidator();
            RuleForEach(property => property.Products)
                .SetValidator(productsValidator);
        }
    }
}
