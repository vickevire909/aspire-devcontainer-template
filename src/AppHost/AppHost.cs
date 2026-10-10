IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

var sb = builder.AddAzureServiceBus("servicebus").RunAsEmulator().AddServiceBusQueue("weather");

var _ = builder
    .AddProject<Projects.Hosts_WebApi>("web-api")
    .WithHttpHealthCheck("/health")
    .WithReference(sb)
    .WaitFor(sb);

// builder.AddProject<Projects.Hosts_Worker>("worker").WithReference(sb).WaitFor(sb).WaitFor(api);

builder.Build().Run();
