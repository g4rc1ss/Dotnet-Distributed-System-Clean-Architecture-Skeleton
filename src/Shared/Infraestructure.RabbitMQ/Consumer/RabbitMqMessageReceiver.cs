using System.Text.Json;
using Infraestructure.Communication.Consumers.Handler;
using Infraestructure.Communication.Messages;
using RabbitMQ.Client;

namespace Infraestructure.RabbitMQ.Consumer
{
    public class RabbitMqMessageReceiver : DefaultBasicConsumer
    {
        private readonly IModel _model;
        private readonly IHandleMessage _handleMessage;

        private Type MessageType { get; set; }
        private byte[]? MessageBody { get; set; }
        private ulong DeliveryTag { get; set; }

        public RabbitMqMessageReceiver(IModel model, IHandleMessage handleMessage)
        {
            _model = model;
            _handleMessage = handleMessage;
        }

        public override async void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            MessageType = Type.GetType(properties.Type)!;
            MessageBody = body.ToArray();
            DeliveryTag = deliveryTag;

            await HandleMessage();
        }

        private async ValueTask HandleMessage()
        {
            if (MessageBody == null || MessageType == null)
            {
                throw new ArgumentException("Neither the body or the messageType has been populated");
            }

            var message = JsonSerializer.Deserialize(MessageBody, MessageType) as IMessage
                ?? throw new ArgumentException("The message did not deserialized properly");

            await _handleMessage.Handle(message, CancellationToken.None);
            _model.BasicAck(DeliveryTag, false);

        }
    }
}