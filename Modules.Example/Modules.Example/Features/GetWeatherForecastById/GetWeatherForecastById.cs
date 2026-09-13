using Modules.Example.Domain;
using Modules.Example.Features.GetWeatherForecast;

namespace Modules.Example.Features.GetWeatherForecastById;

public sealed record GetWeatherForecastByIdRequest(Guid Id);

public sealed record GetWeatherForecastByIdResponse(WeatherForecast? Data);

public static class GetWeatherForecastByIdHandler
{
    public static GetWeatherForecastByIdResponse Handle(GetWeatherForecastByIdRequest request)
    {
        return new(WeatherForecastStore.Items.GetValueOrDefault(request.Id));
    }
}
