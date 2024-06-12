using System;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.Repositories;

public class ReadRepositoryAsync<T> : GenericRepositoryBaseAsync<T> where T : class
{
    protected ReadRepositoryAsync(DbContext dbContext) : base(dbContext)
    {
    }
}
