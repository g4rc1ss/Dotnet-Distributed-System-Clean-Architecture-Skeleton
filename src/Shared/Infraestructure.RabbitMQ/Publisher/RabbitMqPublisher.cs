using System.Text;
using Infraestructure.Communication.Messages;
using Infraestructure.Communication.Publisher;
using Infraestructure.RabbitMQ.Serialization;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using IConnectionFactory = RabbitMQ.Client.IConnectionFactory;

namespace Infraestructure.RabbitMQ.Publisher
{
    public class RabbitMqPublisher<TMessage> : IExternalMessagePublisher<TMessage>
        where TMessage : IMessage
    {
        private readonly ISerializer _serializer;
        private readonly RabbitMqSettings _rabbitMqSettings;
        private readonly IConnectionFactory _connectionFactory;

        public RabbitMqPublisher(IOptions<RabbitMqSettings> rabbitMqSettings, ISerializer serializer)
        {
            _rabbitMqSettings = rabbitMqSettings.Value;
            _connectionFactory = new ConnectionFactory()
            {
                HostName = rabbitMqSettings.Value.Hostname,
                Password = rabbitMqSettings.Value.Credentials!.Password,
                UserName = rabbitMqSettings.Value.Credentials!.Username,
            };
            _serializer = serializer;
        }

        public Task Publish(TMessage message, string? routingKey = null, CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var model = connection.CreateModel();

            PublishSingle(message, model, routingKey);

            return Task.CompletedTask;
        }

        public async Task PublishMany(IEnumerable<TMessage> messages, string? routingKey = null, CancellationToken cancellationToken = default)
        {
            var taskList = new List<Task>();
            foreach (var message in messages)
            {
                taskList.Add(Publish(message, routingKey, cancellationToken));
            }

            await Task.WhenAll(taskList);
        }

        private void PublishSingle(TMessage message, IModel model, string? routingKey)
        {
            var properties = model.CreateBasicProperties();
            properties.Persistent = true;
            properties.Type = RemoveVersionFromQualifiedName(message.GetType().AssemblyQualifiedName!, 0);

            model.BasicPublish(exchange: GetCorrectExchange(),
                routingKey: routingKey ?? string.Empty,
                basicProperties: properties,
                body: _serializer.SerializeObjectToByteArray(message));
        }

        private string GetCorrectExchange()
        {
            return (typeof(TMessage) == typeof(IntegrationMessage)
                ? _rabbitMqSettings?.Publisher?.IntegrationExchange
                : _rabbitMqSettings?.Publisher?.DomainExchange)
                ?? throw new ArgumentNullException("Configure the Exchanges on appsettings");
        }

        private string RemoveVersionFromQualifiedName(string assemblyQualifiedName, int indexStart)
        {
            var stringBuilder = new StringBuilder();
            var indexOfGenericClose = assemblyQualifiedName.IndexOf("]]", indexStart + 1, StringComparison.Ordinal);
            var indexOfVersion = assemblyQualifiedName.IndexOf(", Version", indexStart + 1, StringComparison.Ordinal);

            if (indexOfVersion < 0)
                return assemblyQualifiedName;

            stringBuilder.Append(assemblyQualifiedName.Substring(indexStart, indexOfVersion - indexStart));

            if (indexOfGenericClose > 0)
                stringBuilder.Append(RemoveVersionFromQualifiedName(assemblyQualifiedName, indexOfGenericClose));

            return stringBuilder.ToString();
        }
    }
}

