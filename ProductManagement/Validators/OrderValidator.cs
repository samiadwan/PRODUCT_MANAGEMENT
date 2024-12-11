using FluentValidation;
using ProductManagement.DTOs;

namespace ProductManagement.Validators
{
    public class OrderValidator : AbstractValidator<OrderDto>
    {
        public OrderValidator()
        {
            RuleFor(o => o.OrderDate)
                 .NotEmpty()
                 .WithMessage("Order date is required.");

            RuleFor(o => o.UserId)
                .GreaterThan(0)
                .WithMessage("User ID must be valid.");

            RuleForEach(o => o.OrderItems)
                .SetValidator(new OrderItemValidator());
        }
    }
}
