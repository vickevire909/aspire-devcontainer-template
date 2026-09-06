using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Example.Core.Domain;
using Modules.Example.Core.Features.CreateWeatherForecast;
using Modules.Example.Core.Features.GetWeatherForecast;
using Modules.Example.Core.Features.GetWeatherForecastById;
using Wolverine;

namespace Modules.Example.Http;

public static class Endpoints
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapExampleEndpoints()
        {
            var v1 = endpoints.MapGroup("/v1");

            v1.MapPost(
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

            v1.MapGet(
                    "/weatherforecast",
                    async (IMessageBus bus, CancellationToken cancellationToken) =>
                        await bus.InvokeAsync<WeatherForecast[]>(
                            new GetWeatherForecast(),
                            cancellationToken
                        )
                )
                .WithName("GetWeatherForecast");

            v1.MapGet(
                    "/weatherforecast/{id:guid}",
                    async (Guid id, IMessageBus bus, CancellationToken cancellationToken) =>
                    {
                        var forecast = await bus.InvokeAsync<WeatherForecast?>(
                            new GetWeatherForecastById(id),
                            cancellationToken
                        );
                        return forecast is null ? Results.NotFound() : Results.Ok(forecast);
                    }
                )
                .WithName("GetWeatherForecastById");

            return endpoints;
        }
    }
}
