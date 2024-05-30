using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttributes.Commands.DeleteProductAttributeById
{
    public class DeleteProductAttributeByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteProductAttributeByIdCommandHandler : IRequestHandler<DeleteProductAttributeByIdCommand, Response<int>>
        {
            private readonly IProductAttributeRepositoryAsync _productAttributeRepository;
            public DeleteProductAttributeByIdCommandHandler(IProductAttributeRepositoryAsync productAttributeRepository)
            {
                _productAttributeRepository = productAttributeRepository;
            }
            public async Task<Response<int>> Handle(DeleteProductAttributeByIdCommand command, CancellationToken cancellationToken)
            {
                var productAttribute = await _productAttributeRepository.GetByIdAsync(command.Id);
                if (productAttribute == null) throw new ApiException($"ProductAttribute Not Found.");
                await _productAttributeRepository.DeleteAsync(productAttribute);
                return new Response<int>(productAttribute.Id);
            }
        }
    }
}
