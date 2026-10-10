using Scalar.Aspire;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

var sb = builder
    .AddAzureServiceBus("sbemulatorns")
    .RunAsEmulator(emulator =>
    {
        emulator.WithLifetime(ContainerLifetime.Persistent);
        emulator.WithEndpoint(
            "emulator",
            endpoint =>
            {
                endpoint.Port = 5672;
                endpoint.IsProxied = false;
            }
        );
        emulator.WithEndpoint(
            "emulatorhealth",
            endpoint =>
            {
                endpoint.Port = 5300;
                endpoint.IsProxied = false;
            }
        );
    })
    .AddServiceBusQueue("weather");

var api = builder
    .AddProject<Projects.Hosts_WebApi>("web-api")
    .WithHttpHealthCheck("/health")
    .WithReference(sb)
    .WaitFor(sb);

if (builder.ExecutionContext.IsRunMode)
{
    builder.AddScalarApiReference().WithApiReference(api);
}

builder.AddProject<Projects.Hosts_Worker>("worker").WithReference(sb).WaitFor(sb).WaitFor(api);

builder.Build().Run();
