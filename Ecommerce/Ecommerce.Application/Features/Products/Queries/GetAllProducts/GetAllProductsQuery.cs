using AutoMapper;
using MediatR;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace Ecommerce.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<Response<object>>
    {
        public int _start { get; set; }
        public int _end { get; set; }
        public string _sort { get; set; }
        public string _order { get; set; }
        public List<string> _filter { get; set; }
    }
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Response<object>>
    {
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IMapper _mapper;
        public GetAllProductsQueryHandler(IProductRepositoryAsync productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Response<object>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllProductsParameter>(request);
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
