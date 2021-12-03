using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .MaximumLength(50).WithMessage("UserName must not exceed 50 characters.");

            RuleFor(x => x.EmailAddress)
                .EmailAddress().WithMessage(x => $"'{x.EmailAddress}' is not a valid email address.")
                .NotEmpty().WithMessage("EmailAddress is required.");

            RuleFor(x => x.TotalPrice)
                .GreaterThan(0).WithMessage("TotalPrice should be greater than zero.");
        }
    }
}
