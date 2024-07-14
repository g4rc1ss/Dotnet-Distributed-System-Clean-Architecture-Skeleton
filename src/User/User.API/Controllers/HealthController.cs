using Microsoft.AspNetCore.Mvc;

namespace User.API;

[ApiController]
public class HealthController : Controller
{
    [HttpGet("/health")]
    public IActionResult Health()
    {
        return Ok("healthy");
    }
}
