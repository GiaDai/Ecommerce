using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Application.Features.ProductAttrVals.Queries.GetPagingProductAttrVals;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using MediatR;

namespace Ecommerce.Application
{
    public class GetPagingProdAttrValQuery : IRequest<Response<object>>
    {
        public List<int> id { get; set; }
        public int _start { get; set; }
        public int _end { get; set; }
        public string _sort { get; set; }
        public string _order { get; set; }
        public List<string> _filter { get; set; }

        public class GetPagingProdAttrValQueryHandler : IRequestHandler<GetPagingProdAttrValQuery, Response<object>>
        {
            private readonly IProductAttrValueRepositoryAsync _productAttrValueRepository;
            private readonly IMapper _mapper;
            public GetPagingProdAttrValQueryHandler(IProductAttrValueRepositoryAsync productAttrValueRepository, IMapper mapper)
            {
                _productAttrValueRepository = productAttrValueRepository;
                _mapper = mapper;
            }

            public async Task<Response<object>> Handle(GetPagingProdAttrValQuery request, CancellationToken cancellationToken)
            {
                if (request.id != null && request.id.Count > 0)
                {
                    var productAttrValIds = await _productAttrValueRepository.GetProductAttributeValueByIdsAsync(request.id);
                    return new Response<object>(true, productAttrValIds, message: "Success");
                }

                var validFilter = _mapper.Map<GetPagingProdAttrValParamter>(request);
                var productAttrVal = await _productAttrValueRepository.GetPagedProductAttributesAsync(validFilter);
                return new Response<object>(true, new
                {
                    productAttrVal._start,
                    productAttrVal._end,
                    productAttrVal._total,
                    productAttrVal._hasNext,
                    productAttrVal._hasPrevious,
                    productAttrVal._pages,
                    _data = productAttrVal
                }, message: "Success");
            }
        }
    }
}
