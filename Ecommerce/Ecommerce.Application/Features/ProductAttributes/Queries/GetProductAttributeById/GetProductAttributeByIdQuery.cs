using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttributes.Queries.GetProductAttributeById
{
    public class GetProductAttributeByIdQuery : IRequest<Response<ProductAttribute>>
    {
        public int Id { get; set; }

        public class GetProductAttributeByIdQueryHandler : IRequestHandler<GetProductAttributeByIdQuery, Response<ProductAttribute>>
        {
            private readonly IProductAttributeRepositoryAsync _productAttributeRepository;
            public GetProductAttributeByIdQueryHandler(
                IProductAttributeRepositoryAsync productAttributeRepository)
            {
                _productAttributeRepository = productAttributeRepository;
            }

            public async Task<Response<ProductAttribute>> Handle(GetProductAttributeByIdQuery request, CancellationToken cancellationToken)
            {
                var productAttribute = await _productAttributeRepository.GetByIdAsync(request.Id);
                if (productAttribute == null) throw new ApplicationException("ProductAttribute Not Found.");
                return new Response<ProductAttribute>(productAttribute);
            }
        }
    }
}
