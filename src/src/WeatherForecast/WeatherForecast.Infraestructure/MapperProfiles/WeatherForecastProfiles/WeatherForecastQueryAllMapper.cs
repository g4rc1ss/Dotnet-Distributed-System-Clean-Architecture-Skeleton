using AutoMapper;
using Infraestructure.MongoDatabase.MongoDbEntities;
using WeatherForecast.Domain.Application.WeatherForecast.QueryAll;

namespace WeatherForecast.Infraestructure.MapperProfiles.WeatherForecastProfiles;

public class WeatherForecastQueryAllMapper : Profile
{
    public WeatherForecastQueryAllMapper()
    {
        CreateMap<WeatherForecastMongoEntity, WeatherForecastQueryAllResponse>()
            .ForMember(x => x.Date, y => y.Ignore())
            .ForMember(x => x.Summary, y => y.MapFrom(x => x.Summary))
            .ForMember(x => x.TemperatureC, y => y.MapFrom(x => x.TemperatureC))
            .ForMember(x => x.TemperatureF, y => y.MapFrom(x => x.TemperatureF))
            .ReverseMap();
    }
}
