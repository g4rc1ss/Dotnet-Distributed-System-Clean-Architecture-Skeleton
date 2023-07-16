using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infraestructure.MongoDatabase.MongoDbEntities;

public class WeatherForecastMongoEntity
{
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public DateTime? Date { get; set; }
    public int? TemperatureC { get; set; }
    public int? TemperatureF { get; set; }
    public string? Summary { get; set; }
}

