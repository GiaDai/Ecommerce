using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Ecommerce.Infrastructure.Shared.Services
{
    public class RabbitMqService : IRabbitMqService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly BlockingCollection<IModel> _channelPool;
        private readonly int _maxChannels = 10; // Adjust based on your requirements

        public RabbitMqService(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
            _channelPool = new BlockingCollection<IModel>(_maxChannels);
            for (int i = 0; i < _maxChannels; i++)
            {
                _channelPool.Add(_connection.CreateModel());
            }
        }

        public async Task PublishAsync(string queueName, string message)
        {
            var channel = _channelPool.Take();
            try
            {
                // Ensure the queue is declared only once
                lock (channel)
                {
                    if (channel.QueueDeclarePassive(queueName) == null)
                    {
                        channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    }
                }

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                var body = Encoding.UTF8.GetBytes(message);
                await Task.Run(() =>
                {
                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to publish message: {ex.Message}");
                // Handle exception
            }
            finally
            {
                _channelPool.Add(channel);
            }
        }

        public void Dispose()
        {
            foreach (var channel in _channelPool)
            {
                channel.Close();
                channel.Dispose();
            }
            _connection.Close();
            _connection.Dispose();
        }

    }

    public interface IRabbitMqService
    {
        Task PublishAsync(string queueName, string message);
    }
}
