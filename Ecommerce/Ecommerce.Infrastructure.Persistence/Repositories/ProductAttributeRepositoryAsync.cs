﻿using System.Threading.Tasks;
using Ecommerce.Application;
using Ecommerce.Application.Features.ProductAttributes.Queries.GetAllProductAttributes;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence.Contexts;
using Ecommerce.Infrastructure.Persistence.Repository;
using Ecommerce.Infrastructure.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence.Repositories;

public class ProductAttributeRepositoryAsync : GenericRepositoryAsync<ProductAttribute>, IProductAttributeRepositoryAsync
{
    private readonly DbSet<ProductAttribute> _productAttributes;
    public ProductAttributeRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _productAttributes = dbContext.Set<ProductAttribute>();
    }

    public async Task<PagedList<ProductAttribute>> GetPagedProductAttributesAsync(GetAllProductAttributeParameter parameter)
    {
        var productAttributeQuery = _productAttributes.AsQueryable();
        if (parameter._filter != null && parameter._filter.Count > 0)
        {
            productAttributeQuery = MethodExtensions.ApplyFilters(productAttributeQuery, parameter._filter);
        }
        return await PagedList<ProductAttribute>.ToPagedList(productAttributeQuery.OrderByDynamic(parameter._sort, parameter._order).AsNoTracking(), parameter._start, parameter._end);
    }
}
