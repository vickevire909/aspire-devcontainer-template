using Example.Features.GetWeatherForecast;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wolverine;

namespace Example.Features.CreateWeatherForecast;

public sealed record CreateWeatherForecast(DateOnly Date, int TemperatureC, string? Summary);

public static class CreateWeatherForecastEndpoint
{
    public static IEndpointRouteBuilder MapCreateWeatherForecastEndpoint(
        this IEndpointRouteBuilder endpoints
    )
    {
        endpoints
            .MapGroup("/v1")
            .MapPost(
                "/weatherforecast",
                async (
                    CreateWeatherForecast request,
                    IMessageBus bus,
                    CancellationToken cancellationToken
                ) =>
                {
                    var forecast = await bus.InvokeAsync<WeatherForecast>(
                        request,
                        cancellationToken
                    );
                    return Results.CreatedAtRoute(
                        "GetWeatherForecastById",
                        new { id = forecast.Id },
                        forecast
                    );
                }
            )
            .WithName("CreateWeatherForecast");

        return endpoints;
    }
}

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
