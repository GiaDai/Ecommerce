using System;
using Microsoft.Extensions.Caching.Distributed;

namespace Ecommerce.Application
{
    public static class CacheOptionsUtility
    {
        public static DistributedCacheEntryOptions GetDefaultCacheEntryOptions()
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
        }
    }
}
