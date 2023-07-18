using System;
namespace Infraestructure.Communication.Messages;

public record DomainMessage : IMessage
{
    public DomainMessage(string messageIdentifier, string name)
    {
        MessageIdentifier = messageIdentifier;
        Name = name;
    }


    public string MessageIdentifier { get; }

    public string Name { get; }
}

public record DomainMessage<T> : DomainMessage
{
    public DomainMessage(string messageIdentifier, string name, T content, Metadata metadata)
        : base(messageIdentifier, name)
    {
        Content = content;
        Metadata = metadata;
    }

    public T Content { get; }
    public Metadata Metadata { get; }
}

