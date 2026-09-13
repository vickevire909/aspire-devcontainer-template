using Modules.Example.Core.Domain;
using Modules.Example.Core.Features.GetWeatherForecast;

namespace Modules.Example.Core.Features.GetWeatherForecastById;

public sealed record GetWeatherForecastByIdRequest(Guid Id);

public sealed record GetWeatherForecastByIdResponse(WeatherForecast? Data);

public static class GetWeatherForecastByIdHandler
{
    public static GetWeatherForecastByIdResponse Handle(GetWeatherForecastByIdRequest request)
    {
        return new(WeatherForecastStore.Items.GetValueOrDefault(request.Id));
    }
}
