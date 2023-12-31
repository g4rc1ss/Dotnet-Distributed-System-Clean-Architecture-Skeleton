﻿
using System.Linq;
using Infraestructure.Communication.Messages;

namespace Infraestructure.Communication.Consumers.Handler;

public class HandleMessage : IHandleMessage
{
    private readonly IMessageHandlerRegistry _messageHandlerRegistry;

    public HandleMessage(IMessageHandlerRegistry messageHandlerRegistry)
    {
        _messageHandlerRegistry = messageHandlerRegistry;
    }

    public Task Handle(IMessage message, CancellationToken cancellationToken = default)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        var messageType = message.GetType();
        var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);
        var handlers = _messageHandlerRegistry.GetMessageHandlersForType(handlerType, messageType);

        foreach (var handler in handlers)
        {
            var messageHandlerType = handler.GetType();

            var handle = messageHandlerType.GetMethods()
                .Where(methodInfo => methodInfo.Name == nameof(IMessageHandler<object>.Handle))
                .FirstOrDefault(info => info.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .Contains(message.GetType()));
            if (handle != null)
            {
                return (Task)handle.Invoke(handler, new object[] { message, cancellationToken })!;
            }

        }
        return Task.CompletedTask;
    }
}

