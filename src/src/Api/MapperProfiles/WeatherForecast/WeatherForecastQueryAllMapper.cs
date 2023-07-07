using AutoMapper;
using Domain.Application.WeatherForecast.QueryAll;
using Shared.Peticiones.Responses.WeatherForecast;

namespace Api.MapperProfiles.WeatherForecast
{
    public class WeatherForecastQueryAllMapper : Profile
    {
        public WeatherForecastQueryAllMapper()
        {
            CreateMap<WeatherForecastQueryAllResponse, WeatherForecastResponse>();
        }
    }
}
