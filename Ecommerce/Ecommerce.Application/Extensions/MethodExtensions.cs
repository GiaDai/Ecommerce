using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace Ecommerce.Infrastructure.Shared.Extensions
{
    public static class MethodExtensions
    {
        public static IQueryable<TSource> OrderByDynamic<TSource>(
        this IQueryable<TSource> query, string propertyName, string ascending)
        {
            var entityType = typeof(TSource);
            var propertyInfo = entityType.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property {propertyName} not found on type {entityType.Name}");
            }

            var parameter = Expression.Parameter(entityType, "x");
            var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            var methodName = ascending == "asc" ? "OrderBy" : "OrderByDescending";
            var resultExpression = Expression.Call(typeof(Queryable), methodName,
                new Type[] { entityType, propertyInfo.PropertyType },
                query.Expression, Expression.Quote(orderByExp));

            return query.Provider.CreateQuery<TSource>(resultExpression);
        }

        public static IQueryable<T> ApplyFilters<T>(IQueryable<T> query, List<string> filters)
        {
            foreach (var filter in filters)
            {
                var filterValues = filter.Split(':');
                var filterKey = filterValues[0];
                var filterValue = filterValues[1];

                var entityType = typeof(T);
                var propertyInfo = entityType.GetProperty(filterKey);
                var propertyType = propertyInfo?.PropertyType;

                if (propertyType != null)
                {
                    if (propertyType == typeof(int) || propertyType == typeof(decimal) || propertyType == typeof(double)) // Add other numeric types if needed
                    {
                        int numericFilterValue;
                        if (int.TryParse(filterValue, out numericFilterValue))
                        {
                            query = query.Where(p => EF.Property<int>(p, filterKey) == numericFilterValue);
                        }
                    }
                    // datetime
                    else if (propertyType == typeof(DateTime?) || propertyType == typeof(DateTime))
                    {
                        string[] dateRange = filterValue.Split('#');
                        if (dateRange.Length != 2)
                        {
                            continue;
                        }
                        DateTime startDate;
                        DateTime endDate;

                        if (DateTime.TryParseExact(dateRange[0], "yyyy-M-d", CultureInfo.CurrentCulture, DateTimeStyles.None, out startDate)
                        && DateTime.TryParseExact(dateRange[1], "yyyy-M-d", CultureInfo.CurrentCulture, DateTimeStyles.None, out endDate))
                        {
                            query = query.Where(p => EF.Property<DateTime>(p, filterKey) >= startDate && EF.Property<DateTime>(p, filterKey) <= endDate);
                        }
                    }
                    else if (propertyType == typeof(string))
                    {
                        query = query.Where(p => EF.Property<string>(p, filterKey).Contains(filterValue));
                    }
                }
            }

            return query;
        }
    }
}
