using Modules.Reporting;
using Modules.Reporting.Features.FileWeatherReport;
using ServiceDefaults;
using Wolverine;
using Wolverine.AzureServiceBus;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.UseWolverine(opts =>
{
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

    opts.ListenToAzureServiceBusQueue("weather");
});

builder.AddReporting();

using IHost host = builder.Build();

await host.RunAsync();
