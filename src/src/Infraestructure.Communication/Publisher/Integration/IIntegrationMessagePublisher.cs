using Infraestructure.Communication.Messages;

namespace Infraestructure.Communication.Publisher.Integration;

public interface IIntegrationMessagePublisher
{
    Task Publish(object message, Metadata? metadata = null, string? routingKey = null, CancellationToken cancellationToken = default);
    Task PublishMany(IEnumerable<object> message, Metadata? metadata = null, string? routingKey = null, CancellationToken cancellationToken = default);
}

