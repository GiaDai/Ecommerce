using System;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Application.Features.ProductAttributeMappings.Queries.GetPagedProdAttrMap;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence.Contexts;
using Ecommerce.Infrastructure.Persistence.Repository;
using Ecommerce.Infrastructure.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.Repositories;

public class ProductAttributeMappingRepositoryAsync : GenericRepositoryAsync<ProductAttributeMapping>, IProductAttributeMappingRepositoryAsync
{
    private readonly DbSet<ProductAttributeMapping> _productAttributeMappings;
    public ProductAttributeMappingRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _productAttributeMappings = dbContext.Set<ProductAttributeMapping>();
    }

    public async Task<PagedList<ProductAttributeMapping>> GetPagedProdAttrMapAsync(GetPagedProdAttrMapByProductIdParameter request)
    {
        var prodAttrMapQuery = _productAttributeMappings.Where(x => x.ProductId.Equals(request._productid)).AsQueryable();
        if (request._filter != null && request._filter.Count > 0)
        {
            prodAttrMapQuery = MethodExtensions.ApplyFilters(prodAttrMapQuery, request._filter);
        }

        return await PagedList<ProductAttributeMapping>.ToPagedList(prodAttrMapQuery.OrderByDynamic(request._sort, request._order).AsNoTracking(), request._start, request._end);
    }
}
