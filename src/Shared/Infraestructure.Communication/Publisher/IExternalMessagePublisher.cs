
using Infraestructure.Communication.Messages;

namespace Infraestructure.Communication.Publisher;

public interface IExternalMessagePublisher<in TMessage>
    where TMessage : IMessage
{
    Task Publish(TMessage message, string? routingKey = null, CancellationToken cancellationToken = default);
    Task PublishMany(IEnumerable<TMessage> message, string? routingKey = null, CancellationToken cancellationToken = default);
}

