using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Example.Core.Domain;
using Modules.Example.Core.Features.GetWeatherForecast;
using Shared.Kernel.Routing;
using Wolverine;

namespace Modules.Example.Core.Features.GetWeatherForecastById;

public sealed record GetWeatherForecastById(Guid Id);

public sealed class GetWeatherForecastByIdEndpoint : IEndpoint
{
    public IEndpointRouteBuilder Map(IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGroup("/v1")
            .MapGet(
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

public static class GetWeatherForecastByIdHandler
{
    public static WeatherForecast? Handle(GetWeatherForecastById request)
    {
        return WeatherForecastStore.Items.GetValueOrDefault(request.Id);
    }
}
