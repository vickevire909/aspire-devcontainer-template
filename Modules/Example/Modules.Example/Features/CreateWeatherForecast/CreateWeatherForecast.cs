using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Modules.Example.Contracts.Events;
using Modules.Example.Domain;
using Modules.Example.Shared;
using Shared.Kernel.Http;
using Wolverine;

namespace Modules.Example.Features.CreateWeatherForecast;

public record CreateWeatherForecastRequest(DateOnly Date, int TemperatureC, string? Summary);

public record CreateWeatherForecastResponse(WeatherForecast Data);

public class CreateWeatherForecastEndpoints : IModuleEndpoints
{
    public void Map(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost("/api/v1/weatherforecast", CreateWeatherForecast)
            .WithName(nameof(CreateWeatherForecast));
    }

    private async Task<Created> CreateWeatherForecast(
        CreateWeatherForecastRequest request,
        IMessageBus bus,
        CancellationToken cancellationToken
    )
    {
        WeatherForecastCreatedEvent created = await bus.InvokeAsync<WeatherForecastCreatedEvent>(
            request,
            cancellationToken
        );

        return TypedResults.Created($"/api/v1/weatherforecast/{created.ForecastId}");
    }
}

public static class CreateWeatherForecastHandler
{
    public static (WeatherForecast, WeatherForecastCreatedEvent) Handle(
        CreateWeatherForecastRequest request,
        WeatherForecastStore weatherForecastStore
    )
    {
        var response = new WeatherForecast(
            Guid.CreateVersion7(),
            request.Date,
            request.TemperatureC,
            request.Summary
        );

        weatherForecastStore.Items[response.Id] = response;

        var createdEvent = new WeatherForecastCreatedEvent(response.Id, DateTimeOffset.UtcNow);

        return (response, createdEvent);
    }
}
