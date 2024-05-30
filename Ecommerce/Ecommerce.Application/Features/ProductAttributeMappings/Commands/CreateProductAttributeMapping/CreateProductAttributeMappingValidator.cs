using System;
using FluentValidation;

namespace Ecommerce.Application.Features.ProductAttributeMappings.Commands.CreateProductAttributeMapping
{
    public class CreateProductAttributeMappingValidator : AbstractValidator<CreateProductAttributeMappingCommand>
    {
        public CreateProductAttributeMappingValidator()
        {
            RuleFor(p => p.ProductId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.ProductAttributeId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.TextPrompt)
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

            RuleFor(p => p.DisplayOrder)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.AttributeControlTypeId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
