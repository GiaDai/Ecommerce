using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.Repositories;

public class ReadRepositoryAsync<T> : IReadRepositoryAsync<T> where T : class
{
    private readonly ReadDbContext _dbContext;
    public ReadRepositoryAsync(ReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbContext
                 .Set<T>()
                 .ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
    {
        return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
    }
}
