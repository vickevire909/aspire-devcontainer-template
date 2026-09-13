using Modules.Example.Domain;
using Modules.Example.Features.GetWeatherForecast;

namespace Modules.Example.Features.CreateWeatherForecast;

public sealed record CreateWeatherForecastRequest(DateOnly Date, int TemperatureC, string? Summary);

public sealed record CreateWeatherForecastResponse(WeatherForecast Data);

public static class CreateWeatherForecastHandler
{
    public static CreateWeatherForecastResponse Handle(CreateWeatherForecastRequest request)
    {
        var forecast = new WeatherForecast(
            Guid.CreateVersion7(),
            request.Date,
            request.TemperatureC,
            request.Summary
        );
        WeatherForecastStore.Items[forecast.Id] = forecast;
        return new(forecast);
    }
}
