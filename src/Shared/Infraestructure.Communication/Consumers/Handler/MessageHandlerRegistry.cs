using System.Collections.Concurrent;

namespace Infraestructure.Communication.Consumers.Handler;

public class MessageHandlerRegistry(IEnumerable<IMessageHandler> messageHandlers) : IMessageHandlerRegistry
{
    private readonly IEnumerable<IMessageHandler> _messageHandlers = messageHandlers;
    private readonly ConcurrentDictionary<string, IEnumerable<IMessageHandler>> _cachedHandlers = new();

    public IEnumerable<IMessageHandler> GetMessageHandlersForType(Type messageHandlerType, Type messageType)
    {
        var key = $"{messageHandlerType}-{messageType}";
        if (_cachedHandlers.TryGetValue(key, out var existingHandlers))
        {
            return existingHandlers;
        }

        var handlers = GetMessageHandlersInternal(messageHandlerType);
        _cachedHandlers.AddOrUpdate(key, handlers, (_, _) => handlers);

        return handlers;
    }

    private IList<IMessageHandler> GetMessageHandlersInternal(Type messageHandlerType)
    {
        return _messageHandlers.Where(messageHandler => messageHandler.GetType().GetInterfaces().Contains(messageHandlerType))
            .Distinct()
            .ToList();
    }
}

