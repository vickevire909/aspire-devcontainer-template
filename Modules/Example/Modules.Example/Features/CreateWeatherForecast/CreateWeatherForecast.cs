using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
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
        builder.MapPost("/api/v1/weatherforecast", Handler).WithName("CreateWeatherForecast");
    }

    private async Task<Created<CreateWeatherForecastResponse>> Handler(
        CreateWeatherForecastRequest request,
        IMessageBus bus,
        CancellationToken cancellationToken
    )
    {
        CreateWeatherForecastResponse forecast =
            await bus.InvokeAsync<CreateWeatherForecastResponse>(request, cancellationToken);

        return TypedResults.Created($"/api/v1/weatherforecast/{forecast.Data.Id}", forecast);
    }
}

public static class CreateWeatherForecastHandler
{
    public static CreateWeatherForecastResponse Handle(
        CreateWeatherForecastRequest request,
        WeatherForecastStore weatherForecastStore
    )
    {
        var forecast = new WeatherForecast(
            Guid.CreateVersion7(),
            request.Date,
            request.TemperatureC,
            request.Summary
        );
        weatherForecastStore.Items[forecast.Id] = forecast;
        return new(forecast);
    }
}
