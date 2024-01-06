﻿using FluentValidation;

namespace Ecommerce.Orders.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(p => p.UserName).NotEmpty().WithMessage("{Username} is required").MaximumLength(50).WithMessage("{Username} must not exceed 50 characters");
            RuleFor(p => p.EmailAddress).NotEmpty().WithMessage("{EmailAddress} is required");
            RuleFor(p => p.TotalPrice).NotEmpty().WithMessage("{Total price} is required").GreaterThan(0).WithMessage("{TotalPrice} must be greater than zero");
        }
    }
}
