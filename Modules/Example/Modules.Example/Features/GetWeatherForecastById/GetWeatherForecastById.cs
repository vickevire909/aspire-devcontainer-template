using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Modules.Example.Domain;
using Modules.Example.Shared;
using Shared.Kernel.Http;
using Wolverine;

namespace Modules.Example.Features.GetWeatherForecastById;

public sealed record GetWeatherForecastByIdRequest(Guid Id);

public sealed record GetWeatherForecastByIdResponse(WeatherForecast? Data);

public class GetWeatherForecastByIdEndpoints : IModuleEndpoints
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet("/api/v1/weatherforecast/{id:guid}", GetWeatherForecastById)
            .WithName(nameof(GetWeatherForecastById));
    }

    private static async Task<
        Results<Ok<GetWeatherForecastByIdResponse>, NotFound>
    > GetWeatherForecastById(Guid id, IMessageBus bus, CancellationToken cancellationToken)
    {
        GetWeatherForecastByIdResponse response =
            await bus.InvokeAsync<GetWeatherForecastByIdResponse>(
                new GetWeatherForecastByIdRequest(id),
                cancellationToken
            );

        return response.Data is null ? TypedResults.NotFound() : TypedResults.Ok(response);
    }
}

public static class GetWeatherForecastByIdHandler
{
    public static GetWeatherForecastByIdResponse Handle(
        GetWeatherForecastByIdRequest request,
        WeatherForecastStore weatherForecastStore
    )
    {
        return new(weatherForecastStore.Items.GetValueOrDefault(request.Id));
    }
}
