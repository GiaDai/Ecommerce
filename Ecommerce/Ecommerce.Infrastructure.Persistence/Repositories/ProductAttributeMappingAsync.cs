using System;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence.Contexts;
using Ecommerce.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.Repositories;

public class ProductAttributeMappingRepositoryAsync : GenericRepositoryAsync<ProductAttributeMapping>, IProductAttributeMappingRepositoryAsync
{
    private readonly DbSet<ProductAttributeMapping> _productAttributeMappings;
    public ProductAttributeMappingRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _productAttributeMappings = dbContext.Set<ProductAttributeMapping>();
    }
}
