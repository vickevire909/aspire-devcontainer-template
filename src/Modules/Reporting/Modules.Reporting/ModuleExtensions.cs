using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Shared.Kernel.Http;

namespace Modules.Reporting;

public static class ModuleExtensions
{
    /// <summary>
    /// Register module services in DI
    /// </summary>
    /// <param name="host"></param>
    public static IHostApplicationBuilder AddReporting(this IHostApplicationBuilder host)
    {
        return host;
    }

    /// <summary>
    /// Map the module Web API endpoints
    /// </summary>
    /// <param name="builder"></param>
    public static void MapReportingEndpoints(this IEndpointRouteBuilder builder) =>
        builder.MapEndpointsFromAssembly(typeof(ModuleExtensions).Assembly);
}
