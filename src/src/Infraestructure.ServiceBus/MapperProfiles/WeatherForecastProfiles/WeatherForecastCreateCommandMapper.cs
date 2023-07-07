using System;
using AutoMapper;
using Domain.Application.WeatherForecast.ComandCreate;

namespace Infraestructure.ServiceBus.MapperProfiles.WeatherForecastProfiles;

public class WeatherForecastCreateCommandMapper : Profile
{
    public WeatherForecastCreateCommandMapper()
    {
        CreateMap<MySqlDatabase.DatabaseEntities.WeatherForecast, MongoDatabase.MongoDbEntities.WeatherForecast>();
    }
}

