using System;
using System.Threading.Tasks;
using Ecommerce.Application.Features.ProductAttributes.Queries.GetAllProductAttributes;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IProductAttributeRepositoryAsync : IGenericRepositoryAsync<ProductAttribute>
    {
        Task<PagedList<ProductAttribute>> GetPagedProductAttributesAsync(GetAllProductAttributeParameter parameter);
    }
}
