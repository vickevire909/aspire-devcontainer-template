using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Example.Domain;
using Modules.Example.Features.CreateWeatherForecast;
using Modules.Example.Features.GetWeatherForecast;
using Modules.Example.Features.GetWeatherForecastById;
using Wolverine;

namespace Modules.Example.Http;

public static class EndpointExtensions
{
    extension(IEndpointRouteBuilder endpoints)
    {
        public IEndpointRouteBuilder MapExampleEndpoints()
        {
            RouteGroupBuilder v1 = endpoints.MapGroup("/v1");

            v1.MapPost(
                    "/weatherforecast",
                    async (
                        CreateWeatherForecastRequest request,
                        IMessageBus bus,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        WeatherForecast forecast = await bus.InvokeAsync<WeatherForecast>(
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
                            new WeatherForecastRequest(),
                            cancellationToken
                        )
                )
                .WithName("GetWeatherForecast");

            v1.MapGet(
                    "/weatherforecast/{id:guid}",
                    async (Guid id, IMessageBus bus, CancellationToken cancellationToken) =>
                    {
                        WeatherForecast? forecast = await bus.InvokeAsync<WeatherForecast?>(
                            new GetWeatherForecastByIdRequest(id),
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
