using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Application.Features.Products.Queries.GetAllProducts;
using Ecommerce.Application.Interfaces.Repositories.ProductCrqs;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence.Contexts;
using Ecommerce.Infrastructure.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.Repositories.ProductCrqs;

public class ReadProductRepositoryAsync : ReadRepositoryAsync<Product>, IReadProductRepositoryAsync
{
    private readonly DbSet<Product> _products;
    public ReadProductRepositoryAsync(ReadDbContext dbContext) : base(dbContext)
    {
        _products = dbContext.Set<Product>();
    }

    public async Task<PagedList<Product>> GetPagedProductsAsync(GetAllProductsParameter request)
    {
        var productQuery = _products.AsQueryable();
        if (request._filter != null && request._filter.Count > 0)
        {
            productQuery = MethodExtensions.ApplyFilters(productQuery, request._filter);
        }

        return await PagedList<Product>.ToPagedList(productQuery.OrderByDynamic(request._sort, request._order).AsNoTracking(), request._start, request._end);
    }

    public async Task<List<Product>> GetProductsByIdsAsync(List<int> ids)
    {
        return await _products.AsNoTracking().Where(p => ids.Contains(p.Id)).ToListAsync();
    }

    public async Task<bool> IsUniqueBarcodeAsync(string barcode)
    {
        return await _products
                .AllAsync(p => p.Barcode != barcode);
    }
}
