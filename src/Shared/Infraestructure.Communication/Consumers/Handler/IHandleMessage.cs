
using Infraestructure.Communication.Messages;

namespace Infraestructure.Communication.Consumers.Handler;

public interface IHandleMessage
{
    Task Handle(IMessage message, CancellationToken cancellationToken = default);
}

