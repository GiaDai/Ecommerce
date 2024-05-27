using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Ecommerce.Infrastructure.Shared.Environments
{
    public class CloudinarySettingsProvider : ICloudinarySettingsProvider
    {
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _config;
        public CloudinarySettingsProvider(
            IHostEnvironment env,
            IConfiguration config
            )
        {
            _env = env;
            _config = config;
        }

        public string GetConnectionString()
        {
            var isHasMySQLConnectionString = EnvironmentVariables.HasCloudinaryCloudUrl();
            if (_env.IsProduction() && isHasMySQLConnectionString)
            {
                return Environment.GetEnvironmentVariable(EnvironmentVariables.CloudinaryCloudUrl);
            }
            return _config["UploadProvider:CloudinaryUrl"];
        }
    }
}
