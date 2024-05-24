using System;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;

namespace Ecommerce.Application
{
    public class CreateRangeProductResponse : CreateProductCommand
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
