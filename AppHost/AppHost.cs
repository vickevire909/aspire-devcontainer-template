IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ProjectResource> _ = builder
    .AddProject<Projects.WebApi>("web-api")
    .WithHttpHealthCheck("/health");

builder.Build().Run();
