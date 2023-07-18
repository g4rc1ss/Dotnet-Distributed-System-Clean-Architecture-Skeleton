
namespace Infraestructure.Communication.Messages;

public class Metadata
{
    public Metadata(string correlationId, DateTime createdUtc)
    {
        CorrelationId = correlationId;
        CreatedUtc = createdUtc;
    }

    public string CorrelationId { get; set; }
    public DateTime CreatedUtc { get; set; }

}

