using Modules.Example.Core.Domain;
using Modules.Example.Core.Features.GetWeatherForecast;

namespace Modules.Example.Core.Features.CreateWeatherForecast;

public sealed record CreateWeatherForecast(DateOnly Date, int TemperatureC, string? Summary);

public static class CreateWeatherForecastHandler
{
    public static WeatherForecast Handle(CreateWeatherForecast request)
    {
        var forecast = new WeatherForecast(
            Guid.CreateVersion7(),
            request.Date,
            request.TemperatureC,
            request.Summary
        );
        WeatherForecastStore.Items[forecast.Id] = forecast;
        return forecast;
    }
}
