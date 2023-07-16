using AutoMapper;
using WeatherForecast.Domain.Application.WeatherForecast.QueryAll;
using WeatherForecast.Shared.Peticiones.Responses.WeatherForecast;

namespace WeatherForecast.API.MapperProfiles.WeatherForecast;

public class WeatherForecastQueryAllMapper : Profile
{
    public WeatherForecastQueryAllMapper()
    {
        CreateMap<WeatherForecastQueryAllResponse, WeatherForecastResponse>();
    }
}
