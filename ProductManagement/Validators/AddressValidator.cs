using FluentValidation;
using ProductManagement.DTOs;

namespace ProductManagement.Validators
{
    public class AddressValidator : AbstractValidator<AddressDTO>
    {
        public AddressValidator()
        {
            RuleFor(a => a.Street)
                .NotEmpty()
                .WithMessage("Street is required.");

            RuleFor(a => a.City)
                .NotEmpty()
                .WithMessage("City is required.");

            RuleFor(a => a.PostalCode)
                .NotEmpty()
                .WithMessage("Postal Code is required.");
        }
    }
}
