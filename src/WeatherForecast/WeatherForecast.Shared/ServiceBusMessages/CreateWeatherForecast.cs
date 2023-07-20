namespace WeatherForecast.Shared.ServiceBusMessages;

public record CreateWeatherForecast(int id, DateTime? date, int? temperatureC, int? temperatureF, string? summary);

