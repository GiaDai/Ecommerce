using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttrs.Commands.CreateProductAttr
{
    public class CreateProductAttrCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public class CreateProductAttrCommandHandler : IRequestHandler<CreateProductAttrCommand, Response<int>>
        {
            private readonly IMapper _mapper;
            private readonly IProductAttributeRepositoryAsync _productAttributeRepository;
            public CreateProductAttrCommandHandler(
                IProductAttributeRepositoryAsync productAttributeRepository,
                IMapper mapper)
            {
                _productAttributeRepository = productAttributeRepository;
                _mapper = mapper;
            }

            public async Task<Response<int>> Handle(CreateProductAttrCommand request, CancellationToken cancellationToken)
            {
                var productAttribute = _mapper.Map<ProductAttribute>(request);
                await _productAttributeRepository.AddAsync(productAttribute);
                return new Response<int>(productAttribute.Id);
            }
        }
    }
}
