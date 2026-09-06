namespace Modules.Example.Core.Domain;

public sealed record WeatherForecast(Guid Id, DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
