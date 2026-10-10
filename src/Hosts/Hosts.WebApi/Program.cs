using JasperFx;
using Modules.Example;
using Modules.Example.Contracts.Events;
using Modules.Example.Features.GetWeatherForecast;
using Modules.Reporting;
using Modules.Reporting.Features.FileWeatherReport;
using ServiceDefaults;
using Wolverine;
using Wolverine.AzureServiceBus;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

builder.Host.UseWolverine(opts =>
{
    opts.Discovery.IncludeAssembly(typeof(GetWeatherForecastHandler).Assembly);
    opts.Discovery.IncludeAssembly(typeof(FileWeatherReportHandler).Assembly);

    if (builder.Environment.IsDevelopment())
    {
        opts.UseAzureServiceBusEmulator().AutoProvision().AutoPurgeOnStartup();
    }
    else
    {
        var connectionString = builder.Configuration.GetConnectionString("servicebus");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "The Aspire Service Bus connection string was not injected into the web API."
            );
        }
        opts.UseAzureServiceBus(connectionString).AutoProvision();
    }

    opts.PublishMessage<WeatherForecastCreatedEvent>().ToAzureServiceBusQueue("weather");
});

builder.Services.AddOpenApi();

builder.AddExample();
builder.AddReporting();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Map module endpoints
app.MapExampleEndpoints();
app.MapReportingEndpoints();

app.MapDefaultEndpoints();

return await app.RunJasperFxCommands(args);
