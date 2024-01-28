using System;
using System.Diagnostics;
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

    public async Task Publish(object message, Metadata? metadata = null, string? routingKey = null, CancellationToken cancellationToken = default)
    {
        using var tracingConsumer = new ActivitySource(nameof(IExternalMessagePublisher<IMessage>));
        using var activity = tracingConsumer.CreateActivity("Publicar mensaje", ActivityKind.Consumer);

        var calculateMetadata = CalculateMetadata(metadata);

        activity?.Start();
        var integrationMessage = IntegrationMessageMapper.MapToMessage(message, calculateMetadata, activity?.SpanId.ToString());

        await _externalPublisher.Publish(integrationMessage, routingKey, cancellationToken);

        TracerPublisher.TracePublishTags(activity, routingKey, calculateMetadata, integrationMessage);
        activity.SetStatus(ActivityStatusCode.Ok);
    }

    private Metadata CalculateMetadata(Metadata? metadata)
    {
        return metadata ?? new Metadata(Guid.NewGuid().ToString(), DateTime.UtcNow);
    }
}

