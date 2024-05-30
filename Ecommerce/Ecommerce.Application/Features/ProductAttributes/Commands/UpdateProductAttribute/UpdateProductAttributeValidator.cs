using System;
using FluentValidation;

namespace Ecommerce.Application.Features.ProductAttributes.Commands.UpdateProductAttribute
{
    public class UpdateProductAttributeValidator : AbstractValidator<UpdateProductAttributeCommand>
    {
        public UpdateProductAttributeValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }
    }
}