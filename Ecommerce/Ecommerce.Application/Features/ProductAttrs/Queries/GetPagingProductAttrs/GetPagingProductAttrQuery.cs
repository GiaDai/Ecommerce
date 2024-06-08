using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttrs.Queries.GetPagingProductAttrs
{
    public class GetPagingProductAttrQuery : IRequest<Response<object>>
    {
        public List<int> id { get; set; }
        public int _start { get; set; }
        public int _end { get; set; }
        public string _sort { get; set; }
        public string _order { get; set; }
        public List<string> _filter { get; set; }

        public class GetPagingProductAttrQueryHandler : IRequestHandler<GetPagingProductAttrQuery, Response<object>>
        {
            private readonly IMapper _mapper;
            private readonly IProductAttributeRepositoryAsync _productAttributeRepository;
            public GetPagingProductAttrQueryHandler(
                IProductAttributeRepositoryAsync productAttributeRepository,
                IMapper mapper)
            {
                _productAttributeRepository = productAttributeRepository;
                _mapper = mapper;
            }

            public async Task<Response<object>> Handle(GetPagingProductAttrQuery request, CancellationToken cancellationToken)
            {
                if (request.id != null && request.id.Count > 0)
                {
                    var productAttributeIds = await _productAttributeRepository.GetProductsByIdsAsync(request.id);
                    return new Response<object>(true, productAttributeIds, message: "Success");
                }
                var validFilter = _mapper.Map<GetPagingProductAttrParameter>(request);
                var productAttribute = await _productAttributeRepository.GetPagedProductAttributesAsync(validFilter);
                return new Response<object>(true, new
                {
                    productAttribute._start,
                    productAttribute._end,
                    productAttribute._total,
                    productAttribute._hasNext,
                    productAttribute._hasPrevious,
                    productAttribute._pages,
                    _data = productAttribute
                }, message: "Success");
            }
        }
    }
}
