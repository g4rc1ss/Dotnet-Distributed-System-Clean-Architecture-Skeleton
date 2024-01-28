using System.Diagnostics;
using System.Reflection;
using Infraestructure.Communication.Messages;

namespace Infraestructure.Communication.Publisher.Integration;

public static class IntegrationMessageMapper
{
    internal static IntegrationMessage MapToMessage(object message, Metadata metadata)
    {
        if (message is IntegrationMessage)
        {
            throw new ArgumentException("Message should not be of type IntegrationMessage");
        }

        var buildWrapperMethodInfo = typeof(IntegrationMessageMapper).GetMethod(
            nameof(ToTypedIntegrationEvent),
            BindingFlags.Static | BindingFlags.NonPublic
        );

        var buildWrapperGenericMethodInfo = buildWrapperMethodInfo?.MakeGenericMethod(new[] { message.GetType() });
        var traces = new MessageDiagnosticTraces
        {
            TraceId = Activity.Current!.TraceId.ToString(),
            SpanId = Activity.Current!.SpanId.ToString()
        };
        var wrapper = buildWrapperGenericMethodInfo?.Invoke(
            null,
            [
                message,
                metadata,
                traces
            ]
        );
        return (wrapper as IntegrationMessage)!;
    }

    private static IntegrationMessage<T> ToTypedIntegrationEvent<T>(T message, Metadata metadata, MessageDiagnosticTraces traces)
    {
        return new IntegrationMessage<T>(Guid.NewGuid().ToString(), typeof(T).Name, traces, message, metadata);
    }
}

