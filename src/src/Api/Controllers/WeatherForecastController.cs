using System.Net.Http.Json;
using System.Runtime.InteropServices;
using Application.Interfaces.ApplicationCore;
using AutoMapper;
using Domain.Application.WeatherForecast.ComandCreate;
using Domain.Application.WeatherForecast.QueryAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shared.Peticiones.Request;
using Shared.Peticiones.Responses.WeatherForecast;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ICreateWeatherForecastContract _createWeatherForecast;
        private readonly IGetAllWeatherForecastContract _getAllWeatherForecast;

        public WeatherForecastController(IMapper mapper, ICreateWeatherForecastContract createWeatherForecast, IGetAllWeatherForecastContract getAllWeatherForecast, IConfiguration configuration)
        {
            _mapper = mapper;
            _createWeatherForecast = createWeatherForecast;
            _getAllWeatherForecast = getAllWeatherForecast;
            _configuration = configuration;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetWeatherForecast()
        {
            var weatherForecast = await _getAllWeatherForecast.ExecuteAsync();
            return Json(_mapper.Map<IEnumerable<WeatherForecastResponse>>(weatherForecast));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWeatherForecast(CreateWeatherForecastRequest weatherForecast)
        {
            var newWeather = _mapper.Map<WeatherForecastCommandCreateRequest>(weatherForecast);
            var createWeather = await _createWeatherForecast.ExecuteAsync(newWeather);

            return Json(createWeather);
        }

        [HttpGet("pruebaDocker")]
        public async Task<IActionResult> PruebaImagenesPersistenciaDocker()
        {
            var imageFolderPath = _configuration["imageFolder"];
            var image = await System.IO.File.ReadAllBytesAsync($"{imageFolderPath}1.jpg");

            return File(image, "image/jpeg");
        }
    }
}
