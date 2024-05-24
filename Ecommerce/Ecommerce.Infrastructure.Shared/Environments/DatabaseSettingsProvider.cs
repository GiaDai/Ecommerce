using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Ecommerce.Infrastructure.Shared.Environments
{
    public class DatabaseSettingsProvider : IDatabaseSettingsProvider
    {
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _config;
        public DatabaseSettingsProvider(
            IHostEnvironment env,
            IConfiguration config
            )
        {
            _env = env;
            _config = config;
        }
        public string GetMySQLConnectionString()
        {
            
            var isHasMySQLConnectionString = EnvironmentVariables.HasMySQLConnectionString();
            if (_env.IsProduction() && isHasMySQLConnectionString)
            {
                return Environment.GetEnvironmentVariable(EnvironmentVariables.MySQLConnectionString);
            }
            return _config.GetConnectionString("MySQLConnection");
        }

        public string GetPostgresConnectionString()
        {
            var isHasPostgresConnectionString = EnvironmentVariables.HasPostgresConnectionString();
            if (_env.IsProduction() && isHasPostgresConnectionString)
            {
                return Environment.GetEnvironmentVariable(EnvironmentVariables.PostgresConnectionString);
            }
            return _config.GetConnectionString("PostgresConnection");
        }

        public string GetSQLServerConnectionString()
        {
            var isHasSQLServerConnectionString = EnvironmentVariables.HasSQLServerConnectionString();
            if (_env.IsProduction() && isHasSQLServerConnectionString)
            {
                return Environment.GetEnvironmentVariable(EnvironmentVariables.SQLServerConnectionString);
            }
            return _config.GetConnectionString("SQLServerConnection");
        }
    }
}
