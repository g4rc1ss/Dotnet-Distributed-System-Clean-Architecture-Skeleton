namespace Infraestructure.Communication.Messages;

public record MessageDiagnosticTraces
{
    public string? TraceId { get; init; }
    public string? SpanId { get; init; }
    public string? ParentId { get; init; }
}
