using System;
using FluentValidation;

namespace Ecommerce.Application.Features.ProductAttributes.Commands.CreateProductAttribute
{
    public class CreateProductAttributeValidator : AbstractValidator<CreateProductAttributeCommand>
    {
        public CreateProductAttributeValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }
    }
}
