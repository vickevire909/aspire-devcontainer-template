namespace Modules.Example.Contracts.Events;

public record WeatherForecastCreatedEvent(Guid ForecastId, DateTimeOffset CreatedAt);
