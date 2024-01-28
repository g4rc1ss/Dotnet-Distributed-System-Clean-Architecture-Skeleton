﻿using Infraestructure.Communication.Consumers;
using Infraestructure.Communication.Consumers.Handler;
using Infraestructure.Communication.Messages;
using Infraestructure.RabbitMQ.Serialization;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infraestructure.RabbitMQ.Consumer
{
    public class RabbitMqMessageConsumer<TMessage> : IMessageConsumer<TMessage>
    {
        private readonly RabbitMqSettings _rabbitMqSettings;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IHandleMessage _handleMessage;
        private readonly ISerializer _serializer;

        private bool _disposed;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqMessageConsumer(IHandleMessage handleMessage, IOptions<RabbitMqSettings> rabbitMqSettings, ISerializer serializer)
        {
            _handleMessage = handleMessage;
            _rabbitMqSettings = rabbitMqSettings.Value;

            _connectionFactory = new ConnectionFactory
            {
                HostName = rabbitMqSettings.Value.Hostname,
                Password = rabbitMqSettings.Value.Credentials!.Password,
                UserName = rabbitMqSettings.Value.Credentials!.Username,
                DispatchConsumersAsync = true
            };
            _serializer = serializer;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            return Consume();
        }

        private Task Consume()
        {
            var asyncReceiver = new AsyncEventingBasicConsumer(_channel);
            asyncReceiver.Received += HandleMessage;

            var queue = GetCorrectQueue();
            _channel.BasicConsume(queue, false, asyncReceiver);
            return Task.CompletedTask;
        }

        private string GetCorrectQueue()
        {
            return (typeof(TMessage) == typeof(IntegrationMessage)
                   ? _rabbitMqSettings.Consumer?.IntegrationQueue
                   : _rabbitMqSettings.Consumer?.DomainQueue)
               ?? throw new ArgumentException("please configure the queues on the appsettings");
        }

        private async Task HandleMessage(object ch, BasicDeliverEventArgs eventArgs)
        {
            var messageType = Type.GetType(eventArgs.BasicProperties.Type)!;
            var messageBody = eventArgs.Body.ToArray()!;
            var deliveryTag = eventArgs.DeliveryTag;

            var message = _serializer.DeserializeObject(messageBody, messageType) as IMessage
                ?? throw new ArgumentException("The message did not deserialized properly");


            await _handleMessage.Handle(message, CancellationToken.None);

            ((AsyncEventingBasicConsumer)ch).Model.BasicAck(deliveryTag, false);
            await Task.Yield();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _connection.Dispose();
                _channel.Dispose();
                _disposed = true;

                GC.SuppressFinalize(this);
            }
        }
    }
}

