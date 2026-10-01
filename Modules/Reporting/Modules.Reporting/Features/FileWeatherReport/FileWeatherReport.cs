using Microsoft.Extensions.Logging;
using Modules.Example.Contracts.Events;

namespace Modules.Reporting.Features.FileWeatherReport;

public static class FileWeatherReportHandler
{
    public static async Task Handle(WeatherForecastCreatedEvent @event, ILogger logger)
    {
        logger.LogInformation(
            "Filing a heavy report with the new weatherforecast {forecastId}...",
            @event.ForecastId
        );

        await Task.Delay(1000);
    }
}
