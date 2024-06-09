using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ecommerce.Infrastructure.Shared.Services;

public class RabbitMqService : IRabbitMqService
{
    private readonly IConnectionFactory _connectionFactory;
    public RabbitMqService(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public string Consume(string queueName)
    {
        using (var connection = _connectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            //var response = channel.QueueDeclarePassive(queueName);
            channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            string message = null;
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                message = Encoding.UTF8.GetString(body);
            };
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            return message;
        }
    }

    public IList<string> GetMessageFromQueue(string _key, bool AutoAck = false)
    {
        var _list = new List<string>();

        // Setup synchronization event. 
        var msgsRecievedGate = new ManualResetEventSlim(false);
        using (var connection = _connectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: _key,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var response = channel.QueueDeclarePassive(_key);

            var msgCount = response.MessageCount;
            var msgRecieved = 0;

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                msgRecieved++;

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _list.Add(message);

                if (msgRecieved == msgCount)
                {
                    // Set signal here
                    msgsRecievedGate.Set();

                    // exit function 
                    return;
                }
            };


            channel.BasicConsume(queue: _key,
                                 autoAck: AutoAck,
                                 consumer: consumer);

        }

        // Wait here until all messages are retrieved
        msgsRecievedGate.Wait();

        // now exit function! 
        return _list;
    }

    public void Publish(string queueName, string message)
    {
        using (var connection = _connectionFactory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queueName,
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true; // Ensures the message is marked as persistent
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }
}

public interface IRabbitMqService
{
    void Publish(string queueName, string message);
    string Consume(string queueName);
    IList<string> GetMessageFromQueue(string queueName, bool AutoAck);
}
