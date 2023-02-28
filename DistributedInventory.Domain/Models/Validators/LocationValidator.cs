using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Models.Validators
{
    public class LocationValidator : AbstractValidator<Location>
    {
        const int LocationIdMinimumLength = 1;

        public LocationValidator() {
            RuleFor(property => property.Address)
                .SetValidator(new AddressValidator());

            RuleFor(property => property.LocationId)
                .NotNull()
                .NotEmpty()
                .MinimumLength(LocationIdMinimumLength);
        }
    }
}
