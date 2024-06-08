using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttrs.Queries.GetProductAttrById
{
    public class GetProductAttrByIdQuery : IRequest<Response<ProductAttribute>>
    {
        public int Id { get; set; }

        public class GetProductAttrByIdQueryHandler : IRequestHandler<GetProductAttrByIdQuery, Response<ProductAttribute>>
        {
            private readonly IProductAttributeRepositoryAsync _productAttributeRepository;
            public GetProductAttrByIdQueryHandler(
                IProductAttributeRepositoryAsync productAttributeRepository)
            {
                _productAttributeRepository = productAttributeRepository;
            }

            public async Task<Response<ProductAttribute>> Handle(GetProductAttrByIdQuery request, CancellationToken cancellationToken)
            {
                var productAttribute = await _productAttributeRepository.GetByIdAsync(request.Id);
                if (productAttribute == null) throw new ApplicationException("ProductAttribute Not Found.");
                return new Response<ProductAttribute>(productAttribute);
            }
        }
    }
}
