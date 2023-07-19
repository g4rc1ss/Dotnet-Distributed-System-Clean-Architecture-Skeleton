using AutoMapper;
using WeatherForecast.Domain.Application.WeatherForecast.ComandCreate;
using WeatherForecast.Shared.Peticiones.Request;

namespace WeatherForecast.API.MapperProfiles.WeatherForecast;

public class WeatherForecastCommandCreateMapper : Profile
{
    public WeatherForecastCommandCreateMapper()
    {
        CreateMap<CreateWeatherForecastRequest, WeatherForecastCommandCreateRequest>()
            .ForMember(x => x.TemperatureF, y => y.MapFrom(x => x.Fahrenheit))
            .ForMember(x => x.TemperatureC, y => y.MapFrom(x => x.Celsius))
            .ForMember(x => x.Summary, y => y.MapFrom(x => x.Descripcion));
    }
}
