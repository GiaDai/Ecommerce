using System;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IOrderRepositoryAsync : IGenericRepositoryAsync<Order>
    {
        Task<int> PlaceOrderAsync(int productId);
    }
}
