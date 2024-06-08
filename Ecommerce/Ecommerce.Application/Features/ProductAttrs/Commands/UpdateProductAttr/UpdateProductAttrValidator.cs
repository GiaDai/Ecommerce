using System;
using FluentValidation;

namespace Ecommerce.Application.Features.ProductAttrs.Commands.UpdateProductAttr
{
    public class UpdateProductAttrValidator : AbstractValidator<UpdateProductAttrCommand>
    {
        public UpdateProductAttrValidator()
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