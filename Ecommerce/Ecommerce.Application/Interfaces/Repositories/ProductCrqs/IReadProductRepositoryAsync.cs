using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Application.Features.Products.Queries.GetAllProducts;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories.ProductCrqs
{
    public interface IReadProductRepositoryAsync : IReadRepositoryAsync<Product>
    {
        Task<bool> IsUniqueBarcodeAsync(string barcode);
        Task<PagedList<Product>> GetPagedProductsAsync(GetAllProductsParameter parameter);
        Task<List<Product>> GetProductsByIdsAsync(List<int> ids);
    }
}
