using Microsoft.AspNetCore.Mvc;

namespace WeatherForecast.API;

[ApiController]
public class HealthController : Controller
{
    [HttpGet("/health")]
    public IActionResult Health()
    {
        return Ok("Healthy");
    }
}