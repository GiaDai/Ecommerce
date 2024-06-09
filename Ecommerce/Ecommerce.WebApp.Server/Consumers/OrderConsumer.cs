using System.Text;
using Ecommerce.Application.Interfaces.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ecommerce.WebApp.Server.Consumers
{
    public class OrderConsumer : BackgroundService
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OrderConsumer> _logger;

        public OrderConsumer(
            IConnectionFactory connectionFactory,
            IServiceProvider serviceProvider,
            ILogger<OrderConsumer> logger)
        {
            _connectionFactory = connectionFactory;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            IConnection connection = _connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();

            channel.QueueDeclare(queue: "order_queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var orderRequest = JsonConvert.DeserializeObject<dynamic>(message);
                using var scope = _serviceProvider.CreateScope();
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepositoryAsync>();
                // // Process the order
                if (orderRequest != null)
                {
                    _logger.LogInformation("Processing order for ProductId: {ProductId}", (int)orderRequest.ProductId);
                    await orderRepository.PlaceOrderAsync((int)orderRequest.ProductId);
                }
                else
                {
                    _logger.LogWarning("Order request is null");
                }

            };

            _logger.LogInformation("Starting to consume messages from order_queue");
            channel.BasicConsume(queue: "order_queue",
                                 autoAck: true,
                                 consumer: consumer);

            stoppingToken.Register(() =>
            {
                _logger.LogInformation("Closing RabbitMQ channel and connection");
                // channel.Close();
                // connection.Close();
            });

            await Task.CompletedTask;
        }
    }
}
