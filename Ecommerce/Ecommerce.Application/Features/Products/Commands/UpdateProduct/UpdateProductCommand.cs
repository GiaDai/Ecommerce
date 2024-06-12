using MediatR;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Wrappers;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories.ProductCrqs;

namespace Ecommerce.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Response<int>>
        {
            private readonly IWriteProductRepositoryAsync _writeProductRepository;
            private readonly IReadProductRepositoryAsync _readProductRepository;
            public UpdateProductCommandHandler(
                IWriteProductRepositoryAsync writeProductRepository,
                IReadProductRepositoryAsync readProductRepository
                )
            {
                _writeProductRepository = writeProductRepository;
                _readProductRepository = readProductRepository;
            }
            public async Task<Response<int>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
            {
                var product = await _readProductRepository.GetByIdAsync(command.Id);

                if (product == null)
                {
                    throw new ApiException($"Product Not Found.");
                }
                else
                {
                    product.Name = command.Name;
                    product.Rate = command.Rate;
                    product.Description = command.Description;
                    product.Price = command.Price;
                    await _writeProductRepository.UpdateAsync(product);
                    return new Response<int>(product.Id);
                }
            }
        }
    }
}
