using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttrVals.Queries.GetProdAttrValueById
{
    public class GetProdAttrValueByIdQuery : IRequest<Response<ProductAttributeValue>>
    {
        public int Id { get; set; }
        public class GetProdAttrValueByIdQueryHandler : IRequestHandler<GetProdAttrValueByIdQuery, Response<ProductAttributeValue>>
        {
            private readonly IProductAttrValueRepositoryAsync _productAttrValueRepository;
            public GetProdAttrValueByIdQueryHandler(IProductAttrValueRepositoryAsync productAttrValueRepository)
            {
                _productAttrValueRepository = productAttrValueRepository;
            }
            public async Task<Response<ProductAttributeValue>> Handle(GetProdAttrValueByIdQuery query, CancellationToken cancellationToken)
            {
                var productAttributeValue = await _productAttrValueRepository.GetByIdAsync(query.Id);
                if (productAttributeValue == null) throw new ApplicationException($"Product Attribute Value Not Found.");
                return new Response<ProductAttributeValue>(productAttributeValue);
            }
        }
    }
}
