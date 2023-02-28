using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedInventory.Domain.Models.Validators
{
    public class AddressValidator : AbstractValidator<Address>
    {
        const int AddressLine1MinimumLength = 2;

        const int ZipCodeMinimumLength = 5;
        const int ZipCodeMaximumLength = 10;

        const int StateLength = 2;

        public AddressValidator() {
            RuleFor(address => address.AddressLine1)
                .NotNull()
                .NotEmpty()
                .MinimumLength(AddressLine1MinimumLength);

            RuleFor(address => address.ZipCode)
                .NotNull()
                .NotEmpty()
                .MinimumLength(ZipCodeMinimumLength)
                .MaximumLength(ZipCodeMaximumLength);

            RuleFor(address => address.State)
                .NotNull()
                .NotEmpty()
                .Length(StateLength);
        }
    }
}
