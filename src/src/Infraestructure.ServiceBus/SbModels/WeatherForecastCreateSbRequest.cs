using System;
using Domain.Application.WeatherForecast.ComandCreate;
using MediatR;

namespace Infraestructure.ServiceBus.SbModels;

public class WeatherForecastCreateSbRequest : IRequest<WeatherForecastCreateSbResponse>
{
    public MySqlDatabase.DatabaseEntities.WeatherForecast? WeatherForecast { get; set; }
}

