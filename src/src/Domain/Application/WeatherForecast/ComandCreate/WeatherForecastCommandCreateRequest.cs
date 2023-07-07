

namespace Domain.Application.WeatherForecast.ComandCreate
{
    public class WeatherForecastCommandCreateRequest
    {
        public DateTime? Date { get; set; }
        public int? TemperatureC { get; set; }
        public int? TemperatureF { get; set; }
        public string? Summary { get; set; }
    }
}
