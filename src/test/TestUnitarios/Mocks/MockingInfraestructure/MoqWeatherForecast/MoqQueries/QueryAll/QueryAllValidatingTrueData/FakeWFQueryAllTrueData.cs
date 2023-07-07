using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Application.WeatherForecast.QueryAll;

namespace TestUnitarios.Mocks.MockingInfraestructure.MoqWeatherForecast.MoqQueries.QueryAll.QueryAllValidatingTrueData
{
    internal static class FakeWFQueryAllTrueData
    {
        public static List<WeatherForecastQueryAllResponse> GetFakeWeather => Enumerable.Range(0, 10).Select(x => new WeatherForecastQueryAllResponse
        {
            Date = DateTime.Now,
            Summary = "Sumario",
            TemperatureC = new Random().Next(0, 100),
            TemperatureF = new Random().Next(0, 100),
        }).ToList();
    }
}
