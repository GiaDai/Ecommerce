using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.GetAllProducts;
using Ecommerce.Application.Interfaces.Repositories.ProductCrqs;
using Ecommerce.Application.Wrappers;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.Fe.FeGetPagingProducts
{
    public class FeGetPagingProductsQuery : IRequest<Response<object>>
    {
        public List<int> id { get; set; }
        public int _start { get; set; }
        public int _end { get; set; }
        public string _sort { get; set; }
        public string _order { get; set; }
        public List<string> _filter { get; set; }
    }

    public class FeGetPagingProductsQueryHandler : IRequestHandler<FeGetPagingProductsQuery, Response<object>>
    {
        private readonly IReadProductRepositoryAsync _productRepository;
        private readonly IMapper _mapper;
        public FeGetPagingProductsQueryHandler(IReadProductRepositoryAsync productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Response<object>> Handle(FeGetPagingProductsQuery request, CancellationToken cancellationToken)
        {
            if (request.id != null && request.id.Count > 0)
            {
                var productIds = await _productRepository.GetProductsByIdsAsync(request.id);
                return new Response<object>(true, productIds, message: "Success");
            }

            var feFilter = _mapper.Map<FeGetPagingProductsParameter>(request);
            var validFilter = _mapper.Map<GetAllProductsParameter>(feFilter);
            var product = await _productRepository.GetPagedProductsAsync(validFilter);
            return new Response<object>(true, new
            {
                product._start,
                product._end,
                product._total,
                product._hasNext,
                product._hasPrevious,
                product._pages,
                _data = product
            }, message: "Success");
        }
    }
}
