using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttrVals.Commands.CreateProdAttrVal
{
    public class CreateProdAttrValCommand : IRequest<Response<int>>
    {
        public int ProductAttributeMappingId { get; set; }
        public int AttributeValueTypeId { get; set; }
        public int AssociatedProductId { get; set; }
        public string Name { get; set; }
        public decimal PriceAdjustment { get; set; }
        public bool PriceAdjustmentUsePercentage { get; set; }
        public decimal WeightAdjustment { get; set; }
        public decimal Cost { get; set; }
        public bool IsPreSelected { get; set; }
        public int DisplayOrder { get; set; }

        public class CreateProdAttrValCommandHandler : IRequestHandler<CreateProdAttrValCommand, Response<int>>
        {
            private readonly IMapper _mapper;
            private readonly IProductAttrValueRepositoryAsync _productAttrValueRepository;
            public CreateProdAttrValCommandHandler(
                IProductAttrValueRepositoryAsync productAttrValueRepository,
                IMapper mapper)
            {
                _productAttrValueRepository = productAttrValueRepository;
                _mapper = mapper;
            }

            public async Task<Response<int>> Handle(CreateProdAttrValCommand request, CancellationToken cancellationToken)
            {
                var productAttrValue = _mapper.Map<ProductAttributeValue>(request);
                await _productAttrValueRepository.AddAsync(productAttrValue);
                return new Response<int>(productAttrValue.Id);
            }
        }
    }
}
