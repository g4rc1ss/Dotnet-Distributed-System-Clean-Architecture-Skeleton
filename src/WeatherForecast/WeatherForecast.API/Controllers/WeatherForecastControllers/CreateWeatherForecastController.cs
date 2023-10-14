using WeatherForecast.Interfaces.ApplicationCore;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Shared.Peticiones.Request;
using WeatherForecast.API.MapperProfiles.WeatherForecast;

namespace WeatherForecast.API.WeatherForecastControllers.Controllers;

[ApiController]
[Route("WeatherForecast")]
public class CreateWeatherForecastController : Controller
{
    private readonly WeatherForecastCommandCreateMapper _weatherCommandCreateMapper;
    private readonly ICreateWeatherForecastContract _createWeatherForecast;

    public CreateWeatherForecastController(WeatherForecastCommandCreateMapper weatherCommandCreateMapper, ICreateWeatherForecastContract createWeatherForecast)
    {
        _weatherCommandCreateMapper = weatherCommandCreateMapper;
        _createWeatherForecast = createWeatherForecast;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateWeatherForecast(CreateWeatherForecastRequest weatherForecast)
    {
        var newWeather = _weatherCommandCreateMapper.ToWeatherForecastCommandRequest(weatherForecast);
        var createWeather = await _createWeatherForecast.ExecuteAsync(newWeather);

        return Json(createWeather);
    }
}
