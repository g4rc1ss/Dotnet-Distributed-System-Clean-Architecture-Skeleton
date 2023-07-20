using System;
using Infraestructure.Communication.Consumers;
using Infraestructure.Communication.Consumers.Handler;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Infraestructure.RabbitMQ.Consumer
{
    public class RabbitMqMessageConsumer<TMessage> : IMessageConsumer<TMessage>
    {
        private readonly RabbitMqSettings _rabbitMqSettings;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IHandleMessage _handleMessage;

        public RabbitMqMessageConsumer(IHandleMessage handleMessage, IOptions<RabbitMqSettings> rabbitMqSettings)
        {
            _handleMessage = handleMessage;
            _rabbitMqSettings = rabbitMqSettings.Value;

            _connectionFactory = new ConnectionFactory
            {
                HostName = rabbitMqSettings.Value.Hostname,
                Password = rabbitMqSettings.Value.Credentials!.Password,
                UserName = rabbitMqSettings.Value.Credentials!.Username,
            };
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(Consume, cancellationToken);
        }

        private Task Consume()
        {
            var connection = _connectionFactory.CreateConnection();
            using var model = connection.CreateModel();
            var receiver = new RabbitMqMessageReceiver(model, _handleMessage);
            var queue = GetCorrectQueue();

            model.BasicConsume(queue, false, receiver);

            return Task.CompletedTask;
        }

        private string GetCorrectQueue()
        {
            throw new NotImplementedException();
        }
    }
}

