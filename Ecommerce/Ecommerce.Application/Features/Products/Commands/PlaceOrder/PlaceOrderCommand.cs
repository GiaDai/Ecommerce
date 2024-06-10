using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using MediatR;

namespace Ecommerce.Application.Features.Products.Commands.PlaceOrder
{
    public class PlaceOrderCommand : IRequest<Response<int>>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Response<int>>
        {
            private readonly IProductRepositoryAsync _productRepository;
            public PlaceOrderCommandHandler(
                IProductRepositoryAsync productRepository
            )
            {
                _productRepository = productRepository;
            }

            public async Task<Response<int>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
            {
                var result = await _productRepository.PlaceOrderAsync(request.ProductId, request.Quantity);
                return new Response<int>(result);
            }
        }
    }
}
