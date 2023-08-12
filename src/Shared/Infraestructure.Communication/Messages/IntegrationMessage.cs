
namespace Infraestructure.Communication.Messages;

public record IntegrationMessage : IMessage
{
    public IntegrationMessage(string messageIdentifier, string name)
    {
        MessageIdentifier = messageIdentifier;
        Name = name;
    }

    public string MessageIdentifier { get; }
    public string Name { get; }
}


public record IntegrationMessage<T> : IntegrationMessage
{
    public IntegrationMessage(string messageIdentifier, string name, T content, Metadata metadata)
        : base(messageIdentifier, name)
    {
        Content = content;
        Metadata = metadata;
    }

    public T Content { get; }
    public Metadata Metadata { get; }
}