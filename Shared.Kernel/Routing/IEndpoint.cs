using Microsoft.AspNetCore.Routing;

namespace Shared.Kernel.Routing;

public interface IEndpoint
{
    IEndpointRouteBuilder Map(IEndpointRouteBuilder endpoints);
}
