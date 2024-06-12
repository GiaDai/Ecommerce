using MediatR;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Wrappers;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories.ProductCrqs;

namespace Ecommerce.Application.Features.Products.Commands.DeleteProductById
{
    public class DeleteProductByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, Response<int>>
        {
            private readonly IWriteProductRepositoryAsync _writeProductRepository;
            private readonly IReadProductRepositoryAsync _readProductRepository;
            public DeleteProductByIdCommandHandler(IWriteProductRepositoryAsync writeProductRepository, IReadProductRepositoryAsync readProductRepository)
            {
                _writeProductRepository = writeProductRepository;
                _readProductRepository = readProductRepository;
            }
            public async Task<Response<int>> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
            {
                var product = await _readProductRepository.GetByIdAsync(command.Id);
                if (product == null) throw new ApiException($"Product Not Found.");
                await _writeProductRepository.DeleteAsync(product);
                return new Response<int>(product.Id);
            }
        }
    }
}
