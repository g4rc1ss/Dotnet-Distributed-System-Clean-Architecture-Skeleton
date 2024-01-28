
using System.Diagnostics;
using System.Linq;
using Infraestructure.Communication.Messages;

namespace Infraestructure.Communication.Consumers.Handler;

public class HandleMessage(IMessageHandlerRegistry messageHandlerRegistry) 
    : IHandleMessage
{
    public async Task Handle(IMessage message, CancellationToken cancellationToken = default)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var messageType = message.GetType();
        var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);
        var handlers = messageHandlerRegistry.GetMessageHandlersForType(handlerType, messageType);

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
                var traceId = ActivityTraceId.CreateFromString(message.Traces.TraceId);
                var spanId = ActivitySpanId.CreateFromString(message.Traces.SpanId);
                
                using var tracingConsumer = new ActivitySource(nameof(IMessageHandler));
                using var activity = tracingConsumer.CreateActivity("Llamar handler", ActivityKind.Consumer);
                activity?.SetParentId(traceId, spanId, ActivityTraceFlags.Recorded);
                activity?.AddTag("Handler", handler);
                activity?.Start();

                await (Task)handle.Invoke(handler, [message, cancellationToken])!;

                activity?.SetStatus(ActivityStatusCode.Ok);
            }

        }
    }
}

