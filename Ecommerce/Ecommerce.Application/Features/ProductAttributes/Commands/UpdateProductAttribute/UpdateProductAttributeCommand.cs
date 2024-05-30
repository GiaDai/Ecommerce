using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttributes.Commands.UpdateProductAttribute
{
    public class UpdateProductAttributeCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public class UpdateProductAttributeCommandHandler : IRequestHandler<UpdateProductAttributeCommand, Response<int>>
        {
            private readonly IProductAttributeRepositoryAsync _productAttributeRepository;
            public UpdateProductAttributeCommandHandler(IProductAttributeRepositoryAsync productAttributeRepository)
            {
                _productAttributeRepository = productAttributeRepository;
            }
            public async Task<Response<int>> Handle(UpdateProductAttributeCommand command, CancellationToken cancellationToken)
            {
                var productAttribute = await _productAttributeRepository.GetByIdAsync(command.Id);

                if (productAttribute == null)
                {
                    throw new ApiException($"ProductAttribute Not Found.");
                }
                else
                {
                    productAttribute.Name = command.Name;
                    productAttribute.Description = command.Description;
                    await _productAttributeRepository.UpdateAsync(productAttribute);
                    return new Response<int>(productAttribute.Id);
                }
            }
        }
    }
}
