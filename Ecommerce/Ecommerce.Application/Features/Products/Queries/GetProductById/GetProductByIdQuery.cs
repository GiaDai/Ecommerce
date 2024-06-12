using MediatR;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories.ProductCrqs;

namespace Ecommerce.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<Response<Product>>
    {
        public int Id { get; set; }
        public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Response<Product>>
        {
            private readonly IReadProductRepositoryAsync _productRepository;
            public GetProductByIdQueryHandler(IReadProductRepositoryAsync productRepository)
            {
                _productRepository = productRepository;
            }
            public async Task<Response<Product>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetByIdAsync(query.Id);
                if (product == null) throw new ApiException($"Product Not Found.");
                return new Response<Product>(product);
            }
        }
    }
}
