using MediatR;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Interfaces.Repositories;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using Ecommerce.Domain.Globals;

namespace Ecommerce.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<Response<Product>>
    {
        public int Id { get; set; }

        public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Response<Product>>
        {
            private readonly IDistributedCache _distributedCache;
            private readonly IProductRepositoryAsync _productRepository;
            private readonly ILogger<GetProductByIdQueryHandler> _logger;

            public GetProductByIdQueryHandler(
                IDistributedCache distributedCache,
                IProductRepositoryAsync productRepository,
                ILogger<GetProductByIdQueryHandler> logger)
            {
                _distributedCache = distributedCache;
                _productRepository = productRepository;
                _logger = logger;
            }

            public async Task<Response<Product>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
            {
                var cacheKey = $"product_{query.Id}";
                Product product = null;

                if (RedisConnectionMonitor.IsRedisConnected)
                {
                    try
                    {
                        product = await GetProductFromCacheAsync(cacheKey);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred while accessing the cache");
                        RedisConnectionMonitor.IsRedisConnected = false; // Assume Redis connection is lost if an exception occurs
                    }
                }

                if (product == null)
                {
                    product = await GetProductFromDatabaseAsync(query.Id, cacheKey);
                    if (product == null)
                    {
                        throw new ApiException($"Product Not Found.");
                    }
                }

                return new Response<Product>(product);
            }

            private async Task<Product> GetProductFromCacheAsync(string cacheKey)
            {
                var cachedProduct = await _distributedCache.GetStringAsync(cacheKey);
                if (string.IsNullOrEmpty(cachedProduct))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<Product>(cachedProduct);
            }

            private async Task<Product> GetProductFromDatabaseAsync(int productId, string cacheKey)
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product != null && RedisConnectionMonitor.IsRedisConnected)
                {
                    var jsonProduct = JsonConvert.SerializeObject(product);
                    var options = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    };

                    try
                    {
                        await _distributedCache.SetStringAsync(cacheKey, jsonProduct, options);
                    }
                    catch (RedisException redisEx)
                    {
                        _logger.LogError(redisEx, "Redis error occurred while setting the cache");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An unexpected error occurred while setting the cache");
                    }
                }

                return product;
            }
        }
    }
}
