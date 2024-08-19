using WeatherForecast.Interfaces.ApplicationCore;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Shared.Peticiones.Request;
using WeatherForecast.API.MapperProfiles.WeatherForecast;

namespace WeatherForecast.API.Controllers.WeatherForecastControllers;

[ApiController]
[Route("WeatherForecast")]
public class CreateWeatherForecastController(WeatherForecastCommandCreateMapper weatherCommandCreateMapper, ICreateWeatherForecastContract createWeatherForecast) : Controller
{
    private readonly WeatherForecastCommandCreateMapper _weatherCommandCreateMapper = weatherCommandCreateMapper;
    private readonly ICreateWeatherForecastContract _createWeatherForecast = createWeatherForecast;

    [HttpPost("create")]
    public async Task<IActionResult> CreateWeatherForecast(CreateWeatherForecastRequest weatherForecast)
    {
        var newWeather = _weatherCommandCreateMapper.ToWeatherForecastCommandRequest(weatherForecast);
        var createWeather = await _createWeatherForecast.ExecuteAsync(newWeather);

        return Json(createWeather);
    }
}
