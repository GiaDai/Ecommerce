using Ecommerce.Application.Features.Products.Queries.GetAllProducts;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IProductRepositoryAsync : IGenericRepositoryAsync<Product>
    {
        Task<bool> IsUniqueBarcodeAsync(string barcode);
        Task<int> DeleteRangeAsync(List<int> ids);
        Task<PagedList<Product>> GetPagedProductsAsync(GetAllProductsParameter parameter);
    }
}
