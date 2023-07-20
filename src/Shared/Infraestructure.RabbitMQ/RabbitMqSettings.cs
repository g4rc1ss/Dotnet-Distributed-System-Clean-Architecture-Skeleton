namespace Infraestructure.RabbitMQ;

public class RabbitMqSettings
{
    public string Hostname { get; set; } = null!;
    public RabbitMqCredentials? Credentials { get; set; }
    public PublisherSettings? Publisher { get; init; }
    public ConsumerSettings? Consumer { get; init; }
}

public record RabbitMqCredentials
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}

public record PublisherSettings
{
    public string? IntegrationExchange { get; init; }
    public string? DomainExchange { get; init; }
}

public record ConsumerSettings
{
    public string? IntegrationQueue { get; init; }
    public string? DomainQueue { get; init; }
}