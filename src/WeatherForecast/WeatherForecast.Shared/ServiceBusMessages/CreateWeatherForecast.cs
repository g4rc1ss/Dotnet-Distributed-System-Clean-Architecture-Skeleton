namespace WeatherForecast.Shared.ServiceBusMessages;

public record CreateWeatherForecast(int Id, DateTime? Date, int? TemperatureC, int? TemperatureF, string? Summary);

