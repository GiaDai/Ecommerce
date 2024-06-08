using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Application.Features.ProductAttrs.Queries.GetPagingProductAttrs;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IProductAttributeRepositoryAsync : IGenericRepositoryAsync<ProductAttribute>
    {
        Task<List<ProductAttribute>> GetProductsByIdsAsync(List<int> ids);
        Task<PagedList<ProductAttribute>> GetPagedProductAttributesAsync(GetPagingProductAttrParameter parameter);
    }
}
