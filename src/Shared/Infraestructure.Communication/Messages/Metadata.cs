
namespace Infraestructure.Communication.Messages;

public class Metadata(string correlationId, DateTime createdUtc)
{
    public string CorrelationId { get; set; } = correlationId;
    public DateTime CreatedUtc { get; set; } = createdUtc;

}

