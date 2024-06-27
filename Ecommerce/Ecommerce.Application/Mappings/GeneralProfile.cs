using AutoMapper;
using Ecommerce.Application.Features.ProductAttrMaps.Commands.CreateProductAttrMapping;
using Ecommerce.Application.Features.ProductAttrMaps.Queries.GetPagedProdAttrMap;
using Ecommerce.Application.Features.ProductAttrs.Commands.CreateProductAttr;
using Ecommerce.Application.Features.ProductAttrs.Queries.GetPagingProductAttrs;
using Ecommerce.Application.Features.ProductAttrVals.Commands.CreateProdAttrVal;
using Ecommerce.Application.Features.ProductAttrVals.Queries.GetPagingProductAttrVals;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Queries.Fe.FeGetPagingProducts;
using Ecommerce.Application.Features.Products.Queries.GetAllProducts;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
            CreateMap<FeGetPagingProductsQuery, FeGetPagingProductsParameter>();
            CreateMap<FeGetPagingProductsParameter, GetAllProductsParameter>().ReverseMap();

            CreateMap<CreateProductAttrCommand, ProductAttribute>();
            CreateMap<GetPagingProductAttrQuery, GetPagingProductAttrParameter>();

            CreateMap<CreateProductAttributeMappingCommand, ProductAttributeMapping>();
            CreateMap<GetPagingProdAttrMapQuery, GetPagingProdAttrMapParameter>();

            CreateMap<CreateProdAttrValCommand, ProductAttributeValue>();
            CreateMap<GetPagingProdAttrValQuery, GetPagingProdAttrValParamter>();
        }
    }
}
