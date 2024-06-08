using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttrMaps.Queries.GetProdAttrMapById
{
    public class GetProdAttrMapByIdQuery : IRequest<Response<ProductAttributeMapping>>
    {
        public int Id { get; set; }

        public class GetProdAttrMapByIdHandler : IRequestHandler<GetProdAttrMapByIdQuery, Response<ProductAttributeMapping>>
        {
            private readonly IProductAttributeMappingRepositoryAsync _productAttributeMappingRepository;
            public GetProdAttrMapByIdHandler(
                IProductAttributeMappingRepositoryAsync productAttributeMappingRepository)
            {
                _productAttributeMappingRepository = productAttributeMappingRepository;
            }

            public async Task<Response<ProductAttributeMapping>> Handle(GetProdAttrMapByIdQuery request, CancellationToken cancellationToken)
            {
                var productAttributeMapping = await _productAttributeMappingRepository.GetByIdAsync(request.Id);
                if (productAttributeMapping == null) throw new ApplicationException($"Product Attribute Mapping Not Found.");
                return new Response<ProductAttributeMapping>(productAttributeMapping);
            }
        }
    }
}
