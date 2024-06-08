using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttrVals.Commands.UpdateProdAttrVal
{
    public class UpdateProdAttrValCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
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

        public class UpdateProdAttrValCommandHandler : IRequestHandler<UpdateProdAttrValCommand, Response<int>>
        {
            private readonly IProductAttrValueRepositoryAsync _productAttrValueRepository;
            public UpdateProdAttrValCommandHandler(
                IProductAttrValueRepositoryAsync productAttrValueRepository)
            {
                _productAttrValueRepository = productAttrValueRepository;
            }

            public async Task<Response<int>> Handle(UpdateProdAttrValCommand command, CancellationToken cancellationToken)
            {
                var productAttrValue = await _productAttrValueRepository.GetByIdAsync(command.Id);

                if (productAttrValue == null)
                {
                    throw new ApplicationException($"Product Attribute Value Not Found.");
                }
                else
                {
                    productAttrValue.ProductAttributeMappingId = command.ProductAttributeMappingId;
                    productAttrValue.AttributeValueTypeId = command.AttributeValueTypeId;
                    productAttrValue.Name = command.Name;
                    productAttrValue.PriceAdjustment = command.PriceAdjustment;
                    productAttrValue.PriceAdjustmentUsePercentage = command.PriceAdjustmentUsePercentage;
                    productAttrValue.WeightAdjustment = command.WeightAdjustment;
                    productAttrValue.Cost = command.Cost;
                    productAttrValue.IsPreSelected = command.IsPreSelected;
                    productAttrValue.DisplayOrder = command.DisplayOrder;
                    await _productAttrValueRepository.UpdateAsync(productAttrValue);
                    return new Response<int>(productAttrValue.Id);
                }
            }
        }
    }
}
