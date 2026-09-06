using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Example.Core.Domain;
using Modules.Example.Core.Features.GetWeatherForecast;
using Shared.Kernel.Routing;
using Wolverine;

namespace Modules.Example.Core.Features.CreateWeatherForecast;

public sealed record CreateWeatherForecast(DateOnly Date, int TemperatureC, string? Summary);

public sealed class CreateWeatherForecastEndpoint : IEndpoint
{
    public IEndpointRouteBuilder Map(IEndpointRouteBuilder endpoints)
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
