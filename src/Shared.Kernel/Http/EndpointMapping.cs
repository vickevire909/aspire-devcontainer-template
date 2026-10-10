using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace Shared.Kernel.Http;

/// <summary>
/// Implementing this interface allows for automatic discovery and add when running the MapEndpoints extension.
/// </summary>
public interface IModuleEndpoints
{
    void Map(IEndpointRouteBuilder builder);
}

public static class ModuleEndpointExtensions
{
    /// <summary>
    /// Automatically discovers and maps all <see cref="IModuleEndpoints"/> in the provided assembly.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapEndpointsFromAssembly(
        this IEndpointRouteBuilder builder,
        Assembly assembly
    )
    {
        foreach (
            Type type in assembly
                .GetTypes()
                .Where(t =>
                    typeof(IModuleEndpoints).IsAssignableFrom(t)
                    && t is { IsAbstract: false, IsInterface: false }
                )
        )
        {
            if (Activator.CreateInstance(type) is IModuleEndpoints endpoints)
            {
                endpoints.Map(builder);
            }
        }

        return builder;
    }
}
