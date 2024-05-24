using MassTransit;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Shared.Environments
{
    public interface IRabbitMqSettingProdiver
    {
        string GetHostName();
        string GetUserName();
        string GetPassword();
        string GetVHost();
        string GetPort();
        string GetConnectionString();
        bool IsHealthy();
        ConnectionFactory GetConnectionFactory();
        Task GetUri<T>(IBus _bus,string queueName,T message);
    }
}
