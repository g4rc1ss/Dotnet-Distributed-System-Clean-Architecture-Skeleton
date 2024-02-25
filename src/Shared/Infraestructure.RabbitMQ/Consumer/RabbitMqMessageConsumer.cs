using System.Diagnostics;

using Infraestructure.Communication.Consumers;
using Infraestructure.Communication.Consumers.Handler;
using Infraestructure.Communication.Messages;
using Infraestructure.RabbitMQ.Serialization;

using Microsoft.Extensions.Options;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infraestructure.RabbitMQ.Consumer;

public class RabbitMqMessageConsumer<TMessage> : IMessageConsumer<TMessage>
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly IConnectionFactory _connectionFactory;
    private readonly IHandleMessage _handleMessage;
    private readonly ISerializer _serializer;

    private bool disposed;
    private IConnection? connection;
    private IModel? channel;

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
        connection = _connectionFactory.CreateConnection();
        channel = connection.CreateModel();

        return Consume();
    }

    private Task Consume()
    {
        var asyncReceiver = new AsyncEventingBasicConsumer(channel);
        asyncReceiver.Received += HandleMessage;

        var queue = GetCorrectQueue();
        channel.BasicConsume(queue, false, asyncReceiver);
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

        var traceId = ActivityTraceId.CreateFromString(message.Traces.TraceId);
        var spanId = ActivitySpanId.CreateFromString(message.Traces.SpanId);

        using var tracingConsumer = new ActivitySource(nameof(IMessageHandler));
        using var activity = tracingConsumer.CreateActivity("Call ACK", ActivityKind.Consumer);
        activity?.SetParentId(traceId, spanId, ActivityTraceFlags.Recorded);
        activity?.Start();
        ((AsyncEventingBasicConsumer)ch).Model.BasicAck(deliveryTag, false);
        activity?.SetStatus(ActivityStatusCode.Ok);

        await Task.Yield();
    }

    public void Dispose()
    {
        if (!disposed)
        {
            connection.Dispose();
            channel.Dispose();
            disposed = true;

            GC.SuppressFinalize(this);
        }
    }
}

