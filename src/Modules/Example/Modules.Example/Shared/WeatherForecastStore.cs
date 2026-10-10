using System.Collections.Concurrent;
using Modules.Example.Domain;

namespace Modules.Example.Shared;

/// <summary>
/// Example service
/// </summary>
public class WeatherForecastStore
{
    public ConcurrentDictionary<Guid, WeatherForecast> Items { get; } = new();
}
