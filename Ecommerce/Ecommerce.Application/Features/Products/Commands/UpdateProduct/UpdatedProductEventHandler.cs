using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Globals;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Ecommerce.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdatedProductEvent : INotification
    {
        public Product Product { get; set; }
    }

    public class UpdatedProductEventHandler : INotificationHandler<UpdatedProductEvent>
    {
        private readonly IDistributedCache _distributedCache;
        public UpdatedProductEventHandler(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task Handle(UpdatedProductEvent notification, CancellationToken cancellationToken)
        {
            if (!RedisConnectionMonitor.IsRedisConnected) return;
            var options = CacheOptionsUtility.GetDefaultCacheEntryOptions();
            var cacheKey = $"product_{notification.Product.Id}";
            // Remove the cache entry for the updated product
            _distributedCache.Remove(cacheKey);
            // Add the updated product to the cache
            await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(notification.Product), options, cancellationToken);
        }
    }
}
