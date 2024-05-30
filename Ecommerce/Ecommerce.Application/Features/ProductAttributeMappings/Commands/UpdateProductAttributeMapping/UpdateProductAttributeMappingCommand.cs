using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttributeMappings.Commands.UpdateProductAttributeMapping
{
    public class UpdateProductAttributeMappingCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductAttributeId { get; set; }
        public string TextPrompt { get; set; }
        public bool IsRequired { get; set; }
        public int AttributeControlTypeId { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class UpdateProductAttributeMappingCommandHandler : IRequestHandler<UpdateProductAttributeMappingCommand, Response<int>>
    {
        private readonly IProductAttributeMappingRepositoryAsync _productAttributeMappingRepository;
        public UpdateProductAttributeMappingCommandHandler(
            IProductAttributeMappingRepositoryAsync productAttributeMappingRepository
        )
        {
            _productAttributeMappingRepository = productAttributeMappingRepository;
        }

        public async Task<Response<int>> Handle(UpdateProductAttributeMappingCommand request, CancellationToken cancellationToken)
        {
            var productAttributeMapping = await _productAttributeMappingRepository.GetByIdAsync(request.Id);

            if (productAttributeMapping == null)
            {
                throw new ApplicationException($"Product Attribute Mapping Not Found.");
            }
            else
            {
                productAttributeMapping.ProductId = request.ProductId;
                productAttributeMapping.ProductAttributeId = request.ProductAttributeId;
                productAttributeMapping.TextPrompt = request.TextPrompt;
                productAttributeMapping.IsRequired = request.IsRequired;
                productAttributeMapping.AttributeControlTypeId = request.AttributeControlTypeId;
                productAttributeMapping.DisplayOrder = request.DisplayOrder;

                await _productAttributeMappingRepository.UpdateAsync(productAttributeMapping);
                return new Response<int>(productAttributeMapping.Id);
            }
        }
    }
}
