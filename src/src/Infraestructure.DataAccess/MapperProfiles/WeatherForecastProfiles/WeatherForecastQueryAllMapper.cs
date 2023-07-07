using AutoMapper;
using Domain.Application.WeatherForecast.QueryAll;
using Infraestructure.MongoDatabase.MongoDbEntities;

namespace Infraestructure.MySqlEntityFramework.MapperProfiles.WeatherForecastProfiles
{
    public class WeatherForecastQueryAllMapper : Profile
    {
        public WeatherForecastQueryAllMapper()
        {
            CreateMap<WeatherForecast, WeatherForecastQueryAllResponse>()
                .ForMember(x => x.Date, y => y.Ignore())
                .ForMember(x => x.Summary, y => y.MapFrom(x => x.Summary))
                .ForMember(x => x.TemperatureC, y => y.MapFrom(x => x.TemperatureC))
                .ForMember(x => x.TemperatureF, y => y.MapFrom(x => x.TemperatureF))
                .ReverseMap();
        }
    }
}
