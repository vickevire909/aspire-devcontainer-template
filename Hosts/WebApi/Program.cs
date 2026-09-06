using Modules.Example.Core.Features.GetWeatherForecast;
using Modules.Example.Http;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Host.UseWolverine(options =>
    options.Discovery.IncludeAssembly(typeof(GetWeatherForecastHandler).Assembly)
);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet(
    "/",
    () => "API service is running. Navigate to api/v1/weatherforecast to see sample data."
);

var api = app.MapGroup("/api");

api.MapExampleEndpoints();

app.MapDefaultEndpoints();

app.Run();
