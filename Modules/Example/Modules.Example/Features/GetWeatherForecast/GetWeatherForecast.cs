using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Modules.Example.Domain;
using Modules.Example.Shared;
using Shared.Kernel.Http;
using Wolverine;

namespace Modules.Example.Features.GetWeatherForecast;

public sealed record WeatherForecastRequest;

public sealed record WeatherForecastResponse(IReadOnlyList<WeatherForecast> Data);

public class GetWeatherForecastEndpoints : IModuleEndpoints
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/api/v1/weatherforecast", Handler).WithName("GetWeatherForecast");
    }

    private static async Task<WeatherForecastResponse> Handler(
        IMessageBus bus,
        CancellationToken cancellationToken
    )
    {
        return await bus.InvokeAsync<WeatherForecastResponse>(
            new WeatherForecastRequest(),
            cancellationToken
        );
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

    public static WeatherForecastResponse Handle(
        WeatherForecastRequest _,
        WeatherForecastStore weatherForecastStore
    )
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
            weatherForecastStore.Items[forecast.Id] = forecast;
        }

        return new(forecasts);
    }
}
