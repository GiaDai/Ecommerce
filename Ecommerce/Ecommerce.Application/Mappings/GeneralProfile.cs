using AutoMapper;
using Ecommerce.Application.Features.ProductAttributeMappings.Commands.CreateProductAttributeMapping;
using Ecommerce.Application.Features.ProductAttributes.Commands.CreateProductAttribute;
using Ecommerce.Application.Features.ProductAttributes.Queries.GetAllProductAttributes;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Queries.GetAllProducts;
using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();

            CreateMap<CreateProductAttributeCommand, ProductAttribute>();
            CreateMap<GetAllProductAttributesQuery, GetAllProductAttributeParameter>();

            CreateMap<CreateProductAttributeMappingCommandHandler, ProductAttributeMapping>();

        }
    }
}
