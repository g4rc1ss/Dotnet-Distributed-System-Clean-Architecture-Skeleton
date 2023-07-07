using AutoMapper;
using Domain.Application.WeatherForecast.ComandCreate;
using Shared.Peticiones.Request;

namespace Api.MapperProfiles.WeatherForecast
{
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
}
