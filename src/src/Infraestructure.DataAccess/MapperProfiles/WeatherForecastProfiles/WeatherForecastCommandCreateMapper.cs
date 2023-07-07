using AutoMapper;
using Domain.Application.WeatherForecast.ComandCreate;
using Infraestructure.MySqlDatabase.DatabaseEntities;

namespace Infraestructure.DataAccess.MapperProfiles.WeatherForecastProfiles
{
    public class WeatherForecastCommandCreateMapper : Profile
    {
        public WeatherForecastCommandCreateMapper()
        {
            CreateMap<WeatherForecastCommandCreateRequest, WeatherForecast>();
        }
    }
}
