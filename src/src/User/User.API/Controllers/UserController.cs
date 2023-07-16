using System;
using Microsoft.AspNetCore.Mvc;

namespace User.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    public UserController()
    {
    }

    [HttpGet("user")]
    public IActionResult EndpointUser()
    {
        return Ok("Hola guapo");
    }
}

