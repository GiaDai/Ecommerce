using System;
using FluentValidation;

namespace Ecommerce.Application.Features.ProductAttrs.Commands.CreateProductAttr
{
    public class CreateProductAttrValidator : AbstractValidator<CreateProductAttrCommand>
    {
        public CreateProductAttrValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }
    }
}
