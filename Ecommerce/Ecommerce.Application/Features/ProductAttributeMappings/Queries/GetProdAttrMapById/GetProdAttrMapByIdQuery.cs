﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Features.ProductAttributeMappings.Queries.GetProdAttrMapById
{
    public class GetProdAttrMapById : IRequest<Response<ProductAttributeMapping>>
    {
        public int Id { get; set; }

        public class GetProdAttrMapByIdHandler : IRequestHandler<GetProdAttrMapById, Response<ProductAttributeMapping>>
        {
            private readonly IProductAttributeMappingRepositoryAsync _productAttributeMappingRepository;
            public GetProdAttrMapByIdHandler(
                IProductAttributeMappingRepositoryAsync productAttributeMappingRepository)
            {
                _productAttributeMappingRepository = productAttributeMappingRepository;
            }

            public async Task<Response<ProductAttributeMapping>> Handle(GetProdAttrMapById request, CancellationToken cancellationToken)
            {
                var productAttributeMapping = await _productAttributeMappingRepository.GetByIdAsync(request.Id);
                if (productAttributeMapping == null) throw new ApplicationException($"Product Attribute Mapping Not Found.");
                return new Response<ProductAttributeMapping>(productAttributeMapping);
            }
        }
    }
}
