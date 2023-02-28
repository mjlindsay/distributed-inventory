using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Application.Commands.Validators
{
    public class DeleteInventoryCountCommandValidator : AbstractValidator<DeleteInventoryCountCommand>
    {
        public DeleteInventoryCountCommandValidator() {
            RuleFor(property => property.Id)
                .NotNull()
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(property => property.ETag)
                .NotNull()
                .NotEmpty();
        }
    }
}
