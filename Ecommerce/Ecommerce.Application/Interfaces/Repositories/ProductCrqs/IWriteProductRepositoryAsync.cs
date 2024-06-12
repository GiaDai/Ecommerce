using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories.ProductCrqs
{
    public interface IWriteProductRepositoryAsync : IWriteRepositoryAsync<Product>
    {
        Task<int> DeleteRangeAsync(List<int> ids);
        Task<int> PlaceOrderAsync(int productId, int quantity);
    }
}
