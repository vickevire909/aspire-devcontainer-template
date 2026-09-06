using System.Collections.Concurrent;
using Example.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Shared.Kernel.Routing;
using Wolverine;

namespace Example.Features.GetWeatherForecast;

public sealed record GetWeatherForecast;

public sealed class GetWeatherForecastEndpoint : IEndpoint
{
    public IEndpointRouteBuilder Map(IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGroup("/v1")
            .MapGet(
                "/weatherforecast",
                async (IMessageBus bus, CancellationToken cancellationToken) =>
                    await bus.InvokeAsync<WeatherForecast[]>(
                        new GetWeatherForecast(),
                        cancellationToken
                    )
            )
            .WithName("GetWeatherForecast");

        return endpoints;
    }
}

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

    public static WeatherForecast[] Handle(GetWeatherForecast _)
    {
        var forecasts = Enumerable
            .Range(1, 5)
            .Select(index => new WeatherForecast(
                Guid.CreateVersion7(),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ))
            .ToArray();

        foreach (var forecast in forecasts)
        {
            WeatherForecastStore.Items[forecast.Id] = forecast;
        }

        return forecasts;
    }
}

internal static class WeatherForecastStore
{
    internal static readonly ConcurrentDictionary<Guid, WeatherForecast> Items = new();
}
