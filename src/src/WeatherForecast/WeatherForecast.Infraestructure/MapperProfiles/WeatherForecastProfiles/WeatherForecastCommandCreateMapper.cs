using AutoMapper;
using Infraestructure.MongoDatabase.MongoDbEntities;
using Infraestructure.MySqlDatabase.DbEntities;
using WeatherForecast.Domain.Application.WeatherForecast.ComandCreate;

namespace WeatherForecast.Infraestructure.MapperProfiles.WeatherForecastProfiles;

public class WeatherForecastCommandCreateMapper : Profile
{
    public WeatherForecastCommandCreateMapper()
    {
        CreateMap<WeatherForecastCommandCreateRequest, WeatherForecastMongoEntity>();
        CreateMap<WeatherForecastCommandCreateRequest, WeatherForecastEfEntity>();
    }
}
