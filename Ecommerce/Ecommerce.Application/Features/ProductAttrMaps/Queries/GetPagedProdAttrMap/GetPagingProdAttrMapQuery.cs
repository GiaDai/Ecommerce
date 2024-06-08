using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttrMaps.Queries.GetPagedProdAttrMap
{
    public class GetPagingProdAttrMapQuery : IRequest<Response<object>>
    {
        public int _start { get; set; }
        public int _end { get; set; }
        public string _sort { get; set; }
        public string _order { get; set; }
        public List<string> _filter { get; set; }
    }

    public class GetPagedProdAttrMapByProductIdHandler : IRequestHandler<GetPagingProdAttrMapQuery, Response<object>>
    {
        private readonly IProductAttributeMappingRepositoryAsync _productAttributeMappingRepository;
        private readonly IMapper _mapper;
        public GetPagedProdAttrMapByProductIdHandler(IProductAttributeMappingRepositoryAsync productAttributeMappingRepository, IMapper mapper)
        {
            _productAttributeMappingRepository = productAttributeMappingRepository;
            _mapper = mapper;
        }

        public async Task<Response<object>> Handle(GetPagingProdAttrMapQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetPagingProdAttrMapParameter>(request);
            var productAttributeMapping = await _productAttributeMappingRepository.GetPagedProdAttrMapAsync(validFilter);
            return new Response<object>(true, new
            {
                productAttributeMapping._start,
                productAttributeMapping._end,
                productAttributeMapping._total,
                productAttributeMapping._hasNext,
                productAttributeMapping._hasPrevious,
                productAttributeMapping._pages,
                _data = productAttributeMapping
            }, message: "Success");
        }
    }
}
