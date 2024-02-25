namespace Infraestructure.Communication.Messages;

public record DomainMessage : IMessage
{
    public DomainMessage(string messageIdentifier, string name, MessageDiagnosticTraces traces)
    {
        MessageIdentifier = messageIdentifier;
        Name = name;
        Traces = traces;
    }


    public string MessageIdentifier { get; }

    public string Name { get; }

    public MessageDiagnosticTraces Traces { get; }
}

public record DomainMessage<T> : DomainMessage
{
    public DomainMessage(string messageIdentifier, string name, MessageDiagnosticTraces traces, T content, Metadata metadata)
        : base(messageIdentifier, name, traces)
    {
        Content = content;
        Metadata = metadata;
    }

    public T Content { get; }
    public Metadata Metadata { get; }
}

