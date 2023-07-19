using System;
using System.Text;
using Infraestructure.Communication.Messages;

namespace Infraestructure.Communication.Publisher.Integration;

public class DefaultIntegrationMessagePublisher : IIntegrationMessagePublisher
{
    private readonly IExternalMessagePublisher<IntegrationMessage> _externalPublisher;

    public DefaultIntegrationMessagePublisher(IExternalMessagePublisher<IntegrationMessage> externalPublisher)
    {
        _externalPublisher = externalPublisher;
    }

    public Task Publish(object message, Metadata? metadata = null, string? routingKey = null, CancellationToken cancellationToken = default)
    {
        var calculateMetadata = CalculateMetadata(metadata);
        var integrationMessage = IntegrationMessageMapper.MapToMessage(message, calculateMetadata);
        return _externalPublisher.Publish(integrationMessage, routingKey, cancellationToken);
    }

    public Task PublishMany(IEnumerable<object> messages, Metadata? metadata = null, string? routingKey = null, CancellationToken cancellationToken = default)
    {
        var integrationMessages = messages.Select(message => IntegrationMessageMapper.MapToMessage(message, CalculateMetadata(metadata)));
        return _externalPublisher.PublishMany(integrationMessages, routingKey, cancellationToken);
    }

    private Metadata CalculateMetadata(Metadata? metadata)
    {
        return metadata ?? new Metadata(Guid.NewGuid().ToString(), DateTime.UtcNow);
    }
}

