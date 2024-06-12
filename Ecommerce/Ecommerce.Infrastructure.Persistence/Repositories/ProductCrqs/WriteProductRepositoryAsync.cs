using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Application;
using Ecommerce.Application.Interfaces.Repositories.ProductCrqs;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence.Contexts;

namespace Ecommerce.Infrastructure.Persistence.Repositories.ProductCrqs;

public class WriteProductRepositoryAsync : WriteRepositoryAsync<Product>, IWriteProductRepositoryAsync
{
    public WriteProductRepositoryAsync(WriteDbContext dbContext) : base(dbContext)
    {
    }

    public Task<int> DeleteRangeAsync(List<int> ids)
    {
        throw new NotImplementedException();
    }

    public Task<int> PlaceOrderAsync(int productId, int quantity)
    {
        throw new NotImplementedException();
    }
}
