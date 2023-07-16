using System.ComponentModel.DataAnnotations;

namespace WeatherForecast.Shared.Peticiones.Request;

public class CreateWeatherForecastRequest
{
    [Required]
    public int Celsius { get; set; }

    [Required]
    public int Fahrenheit { get; set; }

    [StringLength(100)]
    [Required]
    public string? Descripcion { get; set; }
}