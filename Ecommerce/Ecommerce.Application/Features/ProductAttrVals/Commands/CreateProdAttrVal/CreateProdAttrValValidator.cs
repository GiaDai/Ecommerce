using System;
using FluentValidation;

namespace Ecommerce.Application.Features.ProductAttrVals.Commands.CreateProdAttrVal
{
    public class CreateProdAttrValValidator : AbstractValidator<CreateProdAttrValCommand>
    {
        public CreateProdAttrValValidator()
        {
        }
    }
}
