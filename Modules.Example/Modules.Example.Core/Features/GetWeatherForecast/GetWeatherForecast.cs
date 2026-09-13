using System.Collections.Concurrent;
using Modules.Example.Core.Domain;

namespace Modules.Example.Core.Features.GetWeatherForecast;

public sealed record WeatherForecastRequest;

public sealed record WeatherForecastResponse(IReadOnlyList<WeatherForecast> Data);

public static class GetWeatherForecastHandler
{
    private static readonly string[] Summaries =
    [
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching",
    ];

    public static WeatherForecastResponse Handle(WeatherForecastRequest _)
    {
        WeatherForecast[] forecasts =
        [
            .. Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecast(
                    Guid.CreateVersion7(),
                    DateOnly.FromDateTime(DateTime.UtcNow.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    Summaries[Random.Shared.Next(Summaries.Length)]
                )),
        ];

        foreach (WeatherForecast? forecast in forecasts)
        {
            WeatherForecastStore.Items[forecast.Id] = forecast;
        }

        return new(forecasts);
    }
}

internal static class WeatherForecastStore
{
    internal static readonly ConcurrentDictionary<Guid, WeatherForecast> Items = new();
}
