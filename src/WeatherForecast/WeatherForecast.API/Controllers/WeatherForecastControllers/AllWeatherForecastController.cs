using Microsoft.AspNetCore.Mvc;
using WeatherForecast.API.MapperProfiles.WeatherForecast;
using WeatherForecast.Interfaces.ApplicationCore;

namespace WeatherForecast.API.WeatherForecastControllers.Controllers;

[ApiController]
[Route("WeatherForecast")]
public class AllWeatherForecastController : Controller
{
    private readonly WeatherForecastQueryAllMapper _weatherQueryAllMapper;
    private readonly IGetAllWeatherForecastContract _getAllWeatherForecast;

    public AllWeatherForecastController(WeatherForecastQueryAllMapper weatherQueryAllMapper, IGetAllWeatherForecastContract getAllWeatherForecast)
    {
        _weatherQueryAllMapper = weatherQueryAllMapper;
        _getAllWeatherForecast = getAllWeatherForecast;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetWeatherForecast()
    {
        var weatherForecast = await _getAllWeatherForecast.ExecuteAsync();
        var weather = weatherForecast.Select(x => _weatherQueryAllMapper.ToWeatherForecastResponse(x));
        return Json(weather);
    }
}
