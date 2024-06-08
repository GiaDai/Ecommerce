using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttrVals.Commands.DeleteProdAttrValById
{
    public class DeleteProdAttrValByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteProdAttrValByIdCommandHandler : IRequestHandler<DeleteProdAttrValByIdCommand, Response<int>>
        {
            private readonly IProductAttrValueRepositoryAsync _productAttrValueRepository;
            public DeleteProdAttrValByIdCommandHandler(
                IProductAttrValueRepositoryAsync productAttrValueRepository)
            {
                _productAttrValueRepository = productAttrValueRepository;
            }
            public async Task<Response<int>> Handle(DeleteProdAttrValByIdCommand command, CancellationToken cancellationToken)
            {
                var productAttrValue = await _productAttrValueRepository.GetByIdAsync(command.Id);
                if (productAttrValue == null) throw new ApplicationException($"Product Attribute Value Not Found.");
                await _productAttrValueRepository.DeleteAsync(productAttrValue);
                return new Response<int>(productAttrValue.Id);
            }
        }
    }
}
