using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttributeMappings.Commands.DeleteProductAttributeMapping
{
    public class DeleteProductAttributeMappingCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        public class DeleteProductAttributeMappingCommandHandler : IRequestHandler<DeleteProductAttributeMappingCommand, Response<int>>
        {
            private readonly IProductAttributeMappingRepositoryAsync _productAttributeMappingRepository;

            public DeleteProductAttributeMappingCommandHandler(IProductAttributeMappingRepositoryAsync productAttributeMappingRepository)
            {
                _productAttributeMappingRepository = productAttributeMappingRepository;
            }

            public async Task<Response<int>> Handle(DeleteProductAttributeMappingCommand command, CancellationToken cancellationToken)
            {
                var productAttributeMapping = await _productAttributeMappingRepository.GetByIdAsync(command.Id);
                if (productAttributeMapping == null) throw new ApplicationException($"Product Attribute Mapping Not Found.");
                await _productAttributeMappingRepository.DeleteAsync(productAttributeMapping);
                return new Response<int>(productAttributeMapping.Id);
            }
        }
    }
}
