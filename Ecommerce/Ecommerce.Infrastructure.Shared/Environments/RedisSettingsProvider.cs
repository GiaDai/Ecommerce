using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis.Extensions.Core.Configuration;
using System;

namespace Ecommerce.Infrastructure.Shared.Environments
{
    public class RedisSettingsProvider : IRedisSettingsProvider
    {
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _config;
        public RedisSettingsProvider(
            IHostEnvironment env,
            IConfiguration config
            )
        {
            _config = config;
            _env = env;
        }
        public RedisConfiguration GetRedisConfiguration()
        {
            var isHasRedisHost = EnvironmentVariables.HasRedisHost();
            var isHasRedisPort = EnvironmentVariables.HasRedisPort();
            var isHasRedisPassword = EnvironmentVariables.HasRedisPassword();

            if (_env.IsProduction() && isHasRedisHost && isHasRedisPort && isHasRedisPassword)
            {
                return new RedisConfiguration
                {
                    Password = Environment.GetEnvironmentVariable(EnvironmentVariables.RedisPassword),
                    AllowAdmin = true,
                    Ssl = false,
                    ConnectTimeout = 5000,
                    Database = 0,
                    ConnectRetry = 3,
                    Hosts = new RedisHost[]
                    {
                        new RedisHost
                        {
                            Host = Environment.GetEnvironmentVariable(EnvironmentVariables.RedisHost),
                            Port = int.Parse(Environment.GetEnvironmentVariable(EnvironmentVariables.RedisPort)),
                            
                        }
                    }
                };
            }

            return _config.GetSection("Redis").Get<RedisConfiguration>();
        }
    }
}
