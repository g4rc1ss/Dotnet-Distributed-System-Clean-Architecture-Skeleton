using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Infraestructure.MongoDatabase.MongoDbEntities;

public class WeatherForecast
{
    [BsonId]
    public int Id { get; set; }
    public DateTime? Date { get; set; }
    public int? TemperatureC { get; set; }
    public int? TemperatureF { get; set; }
    public string? Summary { get; set; }
}

