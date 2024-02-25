using Microsoft.AspNetCore.Mvc;

using WeatherForecast.API.MapperProfiles.WeatherForecast;
using WeatherForecast.Interfaces.ApplicationCore;

namespace WeatherForecast.API.Controllers.WeatherForecastControllers;

[ApiController]
[Route("WeatherForecast")]
public class AllWeatherForecastController(WeatherForecastQueryAllMapper weatherQueryAllMapper, IGetAllWeatherForecastContract getAllWeatherForecast) : Controller
{
    private readonly WeatherForecastQueryAllMapper _weatherQueryAllMapper = weatherQueryAllMapper;
    private readonly IGetAllWeatherForecastContract _getAllWeatherForecast = getAllWeatherForecast;

    [HttpGet("all")]
    public async Task<IActionResult> GetWeatherForecast()
    {
        var weatherForecast = await _getAllWeatherForecast.ExecuteAsync();
        var weather = weatherForecast.Select(_weatherQueryAllMapper.ToWeatherForecastResponse);
        return Json(weather);
    }
}
