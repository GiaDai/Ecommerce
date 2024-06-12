using System;


namespace Ecommerce.Infrastructure.Shared.Environments
{
    public static class EnvironmentVariables
    {
        #region Database
        public const string PostgresConnectionString = "POSTGRES_CONNECTION_STRING";
        public static bool HasPostgresConnectionString() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(PostgresConnectionString));

        public const string PostgresReadConnString = "POSTGRES_READ_CONN_STRING";
        public static bool HasPostgresReadConnString() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(PostgresReadConnString));

        public const string PostgresWriteConnString = "POSTGRES_WRITE_CONN_STRING";
        public static bool HasPostgresWriteConnString() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(PostgresWriteConnString));

        public const string MySQLConnectionString = "MYSQL_CONNECTION_STRING";
        public static bool HasMySQLConnectionString() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(MySQLConnectionString));
        public const string SQLServerConnectionString = "SQLSERVER_CONNECTION_STRING";
        public static bool HasSQLServerConnectionString() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(SQLServerConnectionString));

        #endregion

        #region Redis Cache

        public const string RedisHost = "REDIS_HOST";
        public static bool HasRedisHost() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(RedisHost));
        public const string RedisPort = "REDIS_PORT";
        public static bool HasRedisPort() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(RedisPort));
        public const string RedisPassword = "REDIS_PASSWORD";
        public static bool HasRedisPassword() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(RedisPassword));

        #endregion

        #region ElasticSearch

        public const string ElasticCloudId = "ELASTIC_CLOUD_ID";
        public static bool HasElasticCloudId() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(ElasticCloudId));
        public const string ElasticApiKey = "ELASTIC_API_KEY";
        public static bool HasElasticApiKey() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(ElasticApiKey));

        #endregion

        #region RabbitMQ
        public const string RabbitMqHostName = "RABBITMQ_HOSTNAME";
        public static bool HasRabbitMqHostName() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(RabbitMqHostName));
        public const string RabbitMqUserName = "RABBITMQ_USERNAME";
        public static bool HasRabbitMqUserName() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(RabbitMqUserName));
        public const string RabbitMqPassword = "RABBITMQ_PASSWORD";
        public static bool HasRabbitMqPassword() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(RabbitMqPassword));
        public const string RabbitMqVHost = "RABBITMQ_VHOST";
        public static bool HasRabbitMqVHost() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(RabbitMqVHost));
        public const string RabbitMqPort = "RABBITMQ_PORT";
        public static bool HasRabbitMqPort() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(RabbitMqPort));
        #endregion

        #region Cloudinary
        public const string CloudinaryCloudUrl = "CLOUDINARY_URL";
        public static bool HasCloudinaryCloudUrl() => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(CloudinaryCloudUrl));

        #endregion
    }
}
