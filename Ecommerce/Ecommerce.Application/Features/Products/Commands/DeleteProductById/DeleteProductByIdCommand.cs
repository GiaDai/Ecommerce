using MediatR;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Commands.DeleteProductById
{
    public class DeleteProductByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, Response<int>>
        {
            private readonly IMediator _mediator;
            private readonly IProductRepositoryAsync _productRepository;
            public DeleteProductByIdCommandHandler(
                IMediator mediator,
                IProductRepositoryAsync productRepository)
            {
                _mediator = mediator;
                _productRepository = productRepository;
            }
            public async Task<Response<int>> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetByIdAsync(command.Id);
                if (product == null) throw new ApiException($"Product Not Found.");
                await _productRepository.DeleteAsync(product);
                await _mediator.Publish(new DeleteProdByIdEvent { ProductId = product.Id }, cancellationToken);
                return new Response<int>(product.Id);
            }
        }
    }
}
