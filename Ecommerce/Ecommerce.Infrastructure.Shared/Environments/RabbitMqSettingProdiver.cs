using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Shared.Environments
{
    public class RabbitMqSettingProdiver : IRabbitMqSettingProdiver
    {
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _config;
        public RabbitMqSettingProdiver(
            IHostEnvironment env,
            IConfiguration config
            )
        {
            _config = config;
            _env = env;
        }

        public ConnectionFactory GetConnectionFactory()
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(GetConnectionString())
            };
            return connectionFactory;
        }

        public string GetConnectionString()
        {

            return $"amqp://{GetUserName()}:{GetPassword()}@{GetHostName()}:{GetPort()}/{GetVHost()}";
        }

        public string GetHostName()
        {
            var isHasRabbitMqHostName = EnvironmentVariables.HasRabbitMqHostName();
            if (_env.IsProduction() && isHasRabbitMqHostName)
            {
                return Environment.GetEnvironmentVariable(EnvironmentVariables.RabbitMqHostName);
            }
            return _config["RabbitMq:HostName"];
        }

        public string GetPassword()
        {
            var isHasRabbitMqPassword = EnvironmentVariables.HasRabbitMqPassword();
            if (_env.IsProduction() && isHasRabbitMqPassword)
            {
                return Environment.GetEnvironmentVariable(EnvironmentVariables.RabbitMqPassword);
            }
            return _config["RabbitMq:Password"];
        }

        public string GetPort()
        {
            var isHasRabbitMqPort = EnvironmentVariables.HasRabbitMqPort();
            if (_env.IsProduction() && isHasRabbitMqPort)
            {
                return Environment.GetEnvironmentVariable(EnvironmentVariables.RabbitMqPort);
            }
            return _config["RabbitMq:Port"];
        }

        public async Task GetUri<T>(IBus _bus,string queueName, T message)
        {
            Uri uri;
            ISendEndpoint endPoint;
            if (!string.IsNullOrEmpty(GetVHost()) && GetVHost().Length > 0)
            {
                uri = new Uri($"rabbitmq://{GetHostName()}/{GetVHost()}/{queueName}");
            }
            else
            {
                uri = new Uri($"rabbitmq://{GetHostName()}/{queueName}");
            }
            
            endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(message);
        }

        public string GetUserName()
        {
            var isHasRabbitMqUserName = EnvironmentVariables.HasRabbitMqUserName();
            if (_env.IsProduction() && isHasRabbitMqUserName)
            {
                return Environment.GetEnvironmentVariable(EnvironmentVariables.RabbitMqUserName);
            }
            return _config["RabbitMq:UserName"];
        }

        public string GetVHost()
        {
            var isHasRabbitMqVHost = EnvironmentVariables.HasRabbitMqVHost();
            if (_env.IsProduction() && isHasRabbitMqVHost)
            {
                return Environment.GetEnvironmentVariable(EnvironmentVariables.RabbitMqVHost);
            }
            return _config["RabbitMq:VHost"];
        }

        public bool IsHealthy()
        {
            try
            {
                var connectionFactory = GetConnectionFactory();
                using (var connection = connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    // Kiểm tra kết nối tới RabbitMQ bằng cách khởi tạo kết nối và kênh
                    return connection.IsOpen && channel.IsOpen;
                }
            }
            catch (BrokerUnreachableException)
            {
                // Xử lý lỗi khi không kết nối được tới RabbitMQ
                return false;
            }
        }
    }
}
