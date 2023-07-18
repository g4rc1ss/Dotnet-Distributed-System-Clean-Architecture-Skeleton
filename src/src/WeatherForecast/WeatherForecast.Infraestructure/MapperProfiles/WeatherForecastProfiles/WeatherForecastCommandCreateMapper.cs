using AutoMapper;
using WeatherForecast.Domain.Application.WeatherForecast.ComandCreate;
using WeatherForecast.Infraestructure.Entities.DbEntities;
using WeatherForecast.Infraestructure.Entities.MongoDbEntities;

namespace WeatherForecast.Infraestructure.MapperProfiles.WeatherForecastProfiles;

public class WeatherForecastCommandCreateMapper : Profile
{
    public WeatherForecastCommandCreateMapper()
    {
        CreateMap<WeatherForecastCommandCreateRequest, WeatherForecastMongoEntity>();
        CreateMap<WeatherForecastCommandCreateRequest, WeatherForecastEfEntity>();
    }
}
