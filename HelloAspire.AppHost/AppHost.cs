var builder = DistributedApplication.CreateBuilder(args);

var _ = builder
    .AddProject<Projects.HelloAspire_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.Build().Run();
