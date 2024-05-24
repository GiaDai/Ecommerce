using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Ecommerce.Infrastructure.Shared.Environments
{
    public class ElasticSettingsProvider : IElasticSettingsProvider
    {
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _config;
        public ElasticSettingsProvider(
            IHostEnvironment env,
            IConfiguration config
            )
        {
            _config = config;
            _env = env;
        }
        public string GetApiKey()
        {
            var isHasElasticApiKey = EnvironmentVariables.HasElasticApiKey();
            if (_env.IsProduction() && isHasElasticApiKey)
            {
                return Environment.GetEnvironmentVariable(EnvironmentVariables.ElasticApiKey);
            }
            return _config["Elastic:ApiKey"];
        }

        public string GetCloudId()
        {
            var isHasElasticCloudId = EnvironmentVariables.HasElasticCloudId();
            if (_env.IsProduction() && isHasElasticCloudId)
            {
                return Environment.GetEnvironmentVariable(EnvironmentVariables.ElasticCloudId);
            }
            return _config["Elastic:CloudId"];
        }
    }
}
