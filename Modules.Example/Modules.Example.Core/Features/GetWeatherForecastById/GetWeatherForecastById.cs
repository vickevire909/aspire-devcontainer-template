using Modules.Example.Core.Domain;
using Modules.Example.Core.Features.GetWeatherForecast;

namespace Modules.Example.Core.Features.GetWeatherForecastById;

public sealed record GetWeatherForecastById(Guid Id);

public static class GetWeatherForecastByIdHandler
{
    public static WeatherForecast? Handle(GetWeatherForecastById request)
    {
        return WeatherForecastStore.Items.GetValueOrDefault(request.Id);
    }
}
