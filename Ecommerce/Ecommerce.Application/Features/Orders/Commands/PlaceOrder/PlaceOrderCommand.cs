using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using MediatR;

namespace Ecommerce.Application.Features.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommand : IRequest<Response<int>>
    {
        public int ProductId { get; set; }

        public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Response<int>>
        {
            private readonly IOrderRepositoryAsync _orderRepository;
            public PlaceOrderCommandHandler(
                IOrderRepositoryAsync orderRepository
            )
            {
                _orderRepository = orderRepository;
            }

            public async Task<Response<int>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
            {
                var result = await _orderRepository.PlaceOrderAsync(request.ProductId);
                return new Response<int>(result);
            }
        }
    }
}
