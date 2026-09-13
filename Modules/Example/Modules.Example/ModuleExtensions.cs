using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modules.Example.Shared;
using Shared.Kernel.Http;

namespace Modules.Example;

public static class ModuleExtensions
{
    /// <summary>
    /// Register module services in DI
    /// </summary>
    /// <param name="host"></param>
    public static IHostApplicationBuilder AddExample(this IHostApplicationBuilder host)
    {
        host.Services.AddSingleton<WeatherForecastStore>();

        return host;
    }

    /// <summary>
    /// Map the module Web API endpoints
    /// </summary>
    /// <param name="builder"></param>
    public static void MapExampleEndpoints(this IEndpointRouteBuilder builder) =>
        builder.MapEndpointsFromAssembly(typeof(ModuleExtensions).Assembly);
}
