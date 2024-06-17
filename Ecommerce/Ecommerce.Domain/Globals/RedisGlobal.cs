using System;

namespace Ecommerce.Domain.Globals
{
    public class RedisGlobal
    {

    }

    public static class RedisConnectionMonitor
    {
        public static bool IsRedisConnected { get; set; } = true;
    }
}
