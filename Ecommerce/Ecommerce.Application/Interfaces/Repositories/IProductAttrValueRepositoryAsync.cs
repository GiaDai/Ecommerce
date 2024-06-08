using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Application.Features.ProductAttrVals.Queries.GetPagingProductAttrVals;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IProductAttrValueRepositoryAsync : IGenericRepositoryAsync<ProductAttributeValue>
    {
        Task<List<ProductAttributeValue>> GetProductAttributeValueByIdsAsync(List<int> ids);
        Task<PagedList<ProductAttributeValue>> GetPagedProductAttributesAsync(GetPagingProdAttrValParamter parameter);
    }
}
