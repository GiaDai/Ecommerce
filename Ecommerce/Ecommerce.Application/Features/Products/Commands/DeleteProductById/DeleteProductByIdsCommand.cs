using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;

namespace Ecommerce.Application.Features.Products.Commands.DeleteProductByIds
{
    public class DeleteProductByIdsCommand : List<int>, IRequest<Response<int>>
    {
        public class DeleteProductByIdsCommandHanlder : IRequestHandler<DeleteProductByIdsCommand, Response<int>>
        {
            private readonly IProductRepositoryAsync _productRepository;
            public DeleteProductByIdsCommandHanlder(IProductRepositoryAsync productRepository)
            {
                _productRepository = productRepository;
            }
            public async Task<Response<int>> Handle(DeleteProductByIdsCommand command, CancellationToken cancellationToken)
            {
                List<int> ids = command.ToList();
                var deleted = await _productRepository.DeleteRangeAsync(ids);
                await _productRepository.SaveChangesAsync();
                return new Response<int>(deleted);
            }
        }
    }
}
