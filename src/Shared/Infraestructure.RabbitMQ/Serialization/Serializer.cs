using System.Text;

using Newtonsoft.Json;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Infraestructure.RabbitMQ.Serialization;

internal class Serializer(JsonSerializerSettings serializerSettings) : ISerializer
{
    private static readonly Encoding Encoding = new UTF8Encoding(false);

    private static readonly JsonSerializerSettings DefaultSerializerSettings =
        new()
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

    private const int DEFAULTBUFFERSIZE = 1024;

    private readonly Newtonsoft.Json.JsonSerializer _jsonSerializer = Newtonsoft.Json.JsonSerializer.Create(serializerSettings);

    public Serializer() : this(DefaultSerializerSettings)
    {
    }

    public T DeserializeObject<T>(string input)
    {
        return JsonSerializer.Deserialize<T>(input)
            ?? throw new InvalidOperationException();
    }

    public T DeserializeObject<T>(byte[] input) where T : class
    {
        return (DeserializeObject(input, typeof(T)) as T)!;
    }

    public object DeserializeObject(byte[] input, Type type)
    {
        using var memoryStream = new MemoryStream(input, false);
        using var streamReader = new StreamReader(memoryStream, Encoding, false, DEFAULTBUFFERSIZE, true);
        using var reader = new JsonTextReader(streamReader);
        return _jsonSerializer.Deserialize(reader, type) ?? throw new InvalidOperationException();
    }

    public string SerializeObject<T>(T obj)
    {
        return JsonSerializer.Serialize(obj);
    }

    public byte[] SerializeObjectToByteArray<T>(T obj)
    {
        using var memoryStream = new MemoryStream(DEFAULTBUFFERSIZE);
        using (var streamWriter = new StreamWriter(memoryStream, Encoding, DEFAULTBUFFERSIZE, true))
        using (var jsonWriter = new JsonTextWriter(streamWriter))
        {
            jsonWriter.Formatting = _jsonSerializer.Formatting;
            _jsonSerializer.Serialize(jsonWriter, obj, obj!.GetType());
        }

        return memoryStream.ToArray();
    }
}

