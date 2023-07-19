using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace User.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserbyId(string userId, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);

        return Ok($"User: {userId}");
    }
}

