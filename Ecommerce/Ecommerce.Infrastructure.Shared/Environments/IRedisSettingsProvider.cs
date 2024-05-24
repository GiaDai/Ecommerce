using StackExchange.Redis.Extensions.Core.Configuration;

namespace Ecommerce.Infrastructure.Shared.Environments
{
    public interface IRedisSettingsProvider
    {
        RedisConfiguration GetRedisConfiguration();
    }
}
