using System;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infraestructure.Communication.Consumers.Handler
{
    public class MessageHandlerRegistry : IMessageHandlerRegistry
    {
        private readonly IEnumerable<IMessageHandler> _messageHandlers;
        private readonly ConcurrentDictionary<string, IEnumerable<IMessageHandler>> _cachedHandlers = new();


        public MessageHandlerRegistry(IEnumerable<IMessageHandler> messageHandlers)
        {
            _messageHandlers = messageHandlers;
        }

        public IEnumerable<IMessageHandler> GetMessageHandlersForType(Type messageHandlerType, Type messageType)
        {
            var key = $"{messageHandlerType}-{messageType}";
            if (_cachedHandlers.TryGetValue(key, out var existingHandlers))
            {
                return existingHandlers;
            }

            var handlers = GetMessageHandlersInternal(messageHandlerType, messageType);
            _cachedHandlers.AddOrUpdate(key, handlers, (_, _) => handlers);

            return handlers;
        }

        private IList<IMessageHandler> GetMessageHandlersInternal(Type messageHandlerType, Type messageType)
        {
            return _messageHandlers.Where(messageHandler => messageHandler.GetType().GetInterfaces().Contains(messageHandlerType))
                .Distinct()
                .ToList();
        }
    }
}

