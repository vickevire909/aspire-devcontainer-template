var builder = DistributedApplication.CreateBuilder(args);

var _ = builder.AddProject<Projects.WebApi>("web-api").WithHttpHealthCheck("/health");

builder.Build().Run();
