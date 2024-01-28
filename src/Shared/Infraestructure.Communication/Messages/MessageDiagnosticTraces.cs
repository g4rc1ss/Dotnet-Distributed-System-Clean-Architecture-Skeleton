using System.Diagnostics;

namespace Infraestructure.Communication;

public record MessageDiagnosticTraces
{
    public string TraceId { get; init; }
    public string SpanId { get; init; }
}
