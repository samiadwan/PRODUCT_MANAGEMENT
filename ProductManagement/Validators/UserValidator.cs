using FluentValidation;
using ProductManagement.DTOs;

namespace ProductManagement.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty()
                .WithMessage("Name is required.");

            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");
      
        }
    }
}
