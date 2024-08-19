using Infraestructure.Communication.Messages;

namespace Infraestructure.Communication.Publisher.Domain;

public interface IDomainMessagePublisher
{
    Task Publish(object message, Metadata? metadata = null, string? routingKey = null, CancellationToken cancellationToken = default);
}

