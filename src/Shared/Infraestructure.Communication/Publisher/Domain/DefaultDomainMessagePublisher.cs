using System.Diagnostics;
using Infraestructure.Communication.Messages;

namespace Infraestructure.Communication.Publisher.Domain;


public class DefaultDomainMessagePublisher : IDomainMessagePublisher
{
    private readonly IExternalMessagePublisher<DomainMessage> _externalPublisher;

    public DefaultDomainMessagePublisher(IExternalMessagePublisher<DomainMessage> externalPublisher)
    {
        _externalPublisher = externalPublisher;
    }

    public async Task Publish(object message, Metadata? metadata = null, string? routingKey = null, CancellationToken cancellationToken = default)
    {
        using var tracingConsumer = new ActivitySource(nameof(IExternalMessagePublisher<IMessage>));
        using var activity = tracingConsumer.CreateActivity("Publicar mensaje", ActivityKind.Consumer);

        var calculateMetadata = CalculateMetadata(metadata);

        activity?.Start();
        var domainMessage = DomainMessageMapper.MapToMessage(message, calculateMetadata, activity?.SpanId.ToString());

        await _externalPublisher.Publish(domainMessage, routingKey, cancellationToken);

        TracerPublisher.TracePublishTags(activity, routingKey, calculateMetadata, domainMessage);
        activity.SetStatus(ActivityStatusCode.Ok);
    }

    private Metadata CalculateMetadata(Metadata? metadata)
    {
        return metadata ?? new Metadata(Guid.NewGuid().ToString(), DateTime.UtcNow);
    }
}

