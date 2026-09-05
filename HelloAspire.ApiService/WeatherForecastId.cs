using HelloAspire.Shared;

namespace HelloAspire.ApiService;

public readonly record struct WeatherForecastId(Guid Value) : IEntityId<WeatherForecastId>
{
    public static WeatherForecastId FromValue(Guid Value) => new(Value);
}
