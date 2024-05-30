using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttributes.Commands.CreateProductAttribute
{
    public class CreateProductAttributeCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public class CreateProductAttributeCommandHandler : IRequestHandler<CreateProductAttributeCommand, Response<int>>
        {
            private readonly IMapper _mapper;
            private readonly IProductAttributeRepositoryAsync _productAttributeRepository;
            public CreateProductAttributeCommandHandler(
                IProductAttributeRepositoryAsync productAttributeRepository,
                IMapper mapper)
            {
                _productAttributeRepository = productAttributeRepository;
                _mapper = mapper;
            }

            public async Task<Response<int>> Handle(CreateProductAttributeCommand request, CancellationToken cancellationToken)
            {
                var productAttribute = _mapper.Map<ProductAttribute>(request);
                await _productAttributeRepository.AddAsync(productAttribute);
                return new Response<int>(productAttribute.Id);
            }
        }
    }
}
