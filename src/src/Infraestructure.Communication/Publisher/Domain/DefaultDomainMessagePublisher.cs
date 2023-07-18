﻿using Infraestructure.Communication.Messages;

namespace Infraestructure.Communication.Publisher.Domain;


public class DefaultDomainMessagePublisher
{
    private readonly IExternalMessagePublisher<DomainMessage> _externalPublisher;

    public DefaultDomainMessagePublisher(IExternalMessagePublisher<DomainMessage> externalPublisher)
    {
        _externalPublisher = externalPublisher;
    }

    public Task Publish(object message, Metadata? metadata = null, string? routingKey = null, CancellationToken cancellationToken = default)
    {
        Metadata calculatedMetadata = CalculateMetadata(metadata);
        var domainMessage = DomainMessageMapper.MapToMessage(message, calculatedMetadata);
        return _externalPublisher.Publish(domainMessage, routingKey, cancellationToken);
    }

    public Task PublishMany(IEnumerable<object> messages, Metadata? metadata = null, string? routingKey = null, CancellationToken cancellationToken = default)
    {
        var domainMessages =
            messages.Select(a => DomainMessageMapper.MapToMessage(a, CalculateMetadata(metadata)));
        return _externalPublisher.PublishMany(domainMessages, routingKey, cancellationToken);
    }

    private Metadata CalculateMetadata(Metadata? metadata)
    {
        return metadata ?? new Metadata(Guid.NewGuid().ToString(), DateTime.UtcNow);
    }
}

