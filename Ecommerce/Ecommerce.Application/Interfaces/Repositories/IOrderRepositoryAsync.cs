using System;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Repositories
{
    public interface IOrderRepositoryAsync : IGenericRepositoryAsync<Order>
    {
        int PlaceOrderForwardAsync(int productId);
        Task<int> PlaceOrderAsync(int productId);
    }
}
