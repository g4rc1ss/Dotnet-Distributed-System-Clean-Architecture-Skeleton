using AutoMapper;
using WeatherForecast.Domain.Application.WeatherForecast.ComandCreate;
using WeatherForecast.Infraestructure.Entities.DbEntities;
using WeatherForecast.Infraestructure.Entities.MongoDbEntities;
using WeatherForecast.Shared.ServiceBusMessages;

namespace WeatherForecast.Infraestructure.MapperProfiles.WeatherForecastProfiles;

public class WeatherForecastCommandCreateMapper : Profile
{
    public WeatherForecastCommandCreateMapper()
    {
        CreateMap<WeatherForecastCommandCreateRequest, WeatherForecastMongoEntity>();
        CreateMap<WeatherForecastCommandCreateRequest, WeatherForecastEfEntity>();

        CreateMap<WeatherForecastEfEntity, CreateWeatherForecast>()
            .ForMember(x => x.temperatureC, y => y.MapFrom(x => x.TemperatureC))
            .ForMember(x => x.temperatureF, y => y.MapFrom(x => x.TemperatureF))
            .ForMember(x => x.summary, y => y.MapFrom(x => x.Summary))
            .ForMember(x => x.id, y => y.MapFrom(x => x.Id))
            .ReverseMap();
    }
}
