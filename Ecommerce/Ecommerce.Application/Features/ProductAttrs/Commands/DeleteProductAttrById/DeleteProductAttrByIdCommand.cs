using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttrs.Commands.DeleteProductAttributeById
{
    public class DeleteProductAttrByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteProductAttrByIdCommandHandler : IRequestHandler<DeleteProductAttrByIdCommand, Response<int>>
        {
            private readonly IProductAttributeRepositoryAsync _productAttributeRepository;
            public DeleteProductAttrByIdCommandHandler(IProductAttributeRepositoryAsync productAttributeRepository)
            {
                _productAttributeRepository = productAttributeRepository;
            }
            public async Task<Response<int>> Handle(DeleteProductAttrByIdCommand command, CancellationToken cancellationToken)
            {
                var productAttribute = await _productAttributeRepository.GetByIdAsync(command.Id);
                if (productAttribute == null) throw new ApiException($"ProductAttribute Not Found.");
                await _productAttributeRepository.DeleteAsync(productAttribute);
                return new Response<int>(productAttribute.Id);
            }
        }
    }
}
