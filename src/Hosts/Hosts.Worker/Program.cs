using Azure.Messaging.ServiceBus;
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

    // One way or another, you're probably pulling the Azure Service Bus
    // connection string out of configuration
    var azureServiceBusConnectionString = builder.Configuration.GetConnectionString("servicebus")!;

    // Connect to the broker in the simplest possible way
    var sb = opts.UseAzureServiceBus(azureServiceBusConnectionString)
        // Let Wolverine try to initialize any missing queues
        // on the first usage at runtime
        .AutoProvision();
    if (builder.Environment.IsDevelopment())
    {
        // Direct Wolverine to purge all queues on application startup.
        // This is probably only helpful for testing
        sb.AutoPurgeOnStartup();
    }

    // Or if you need some further specification...
    opts.UseAzureServiceBus(
        azureServiceBusConnectionString,
        azure =>
        {
            azure.RetryOptions.Mode = ServiceBusRetryMode.Exponential;
        }
    );

    opts.ListenToAzureServiceBusSubscription("weather-forecast-created");
});

builder.AddReporting();

IHost host = builder.Build();

host.Run();
