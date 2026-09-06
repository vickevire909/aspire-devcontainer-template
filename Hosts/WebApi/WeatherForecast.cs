namespace Hosts.WebApi;

public class WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public WeatherForecast(Guid Id, DateOnly Date, int TemperatureC, string? Summary)
        : this(Date, TemperatureC, Summary)
    {
        this.Id = Id;
    }

    public Guid Id { get; } = Guid.CreateVersion7();

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public DateOnly Date { get; } = Date;
    public int TemperatureC { get; } = TemperatureC;
    public string? Summary { get; } = Summary;
}

public record CreateWeatherForecast(DateOnly Date, int TemperatureC, string? Summary);
