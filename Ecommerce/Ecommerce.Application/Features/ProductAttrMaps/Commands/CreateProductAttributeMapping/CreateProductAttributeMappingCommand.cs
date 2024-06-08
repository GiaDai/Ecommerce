using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttrMaps.Commands.CreateProductAttrMapping
{
    public class CreateProductAttributeMappingCommand : IRequest<Response<int>>
    {
        public int ProductId { get; set; }
        public int ProductAttributeId { get; set; }
        public string TextPrompt { get; set; }
        public bool IsRequired { get; set; }
        public int AttributeControlTypeId { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CreateProductAttributeMappingCommandHandler : IRequestHandler<CreateProductAttributeMappingCommand, Response<int>>
    {
        private readonly IMapper _mapper;
        private readonly IProductAttributeMappingRepositoryAsync _productAttributeMappingRepository;
        public CreateProductAttributeMappingCommandHandler(
            IProductAttributeMappingRepositoryAsync productAttributeMappingRepository,
            IMapper mapper)
        {
            _productAttributeMappingRepository = productAttributeMappingRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateProductAttributeMappingCommand request, CancellationToken cancellationToken)
        {
            var productAttributeMapping = _mapper.Map<ProductAttributeMapping>(request);
            await _productAttributeMappingRepository.AddAsync(productAttributeMapping);
            return new Response<int>(productAttributeMapping.Id);
        }
    }
}
