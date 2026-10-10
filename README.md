# Getting started with Aspire and Dev Containers

This is a repository template to streamline the process of getting started with Aspire using Dev Containers in both Visual Studio Code and GitHub Codespaces. Please refer to our product documentation on how to use these repository templates to get started.

- [Aspire and GitHub Codespaces](https://learn.microsoft.com/dotnet/aspire/get-started/github-codespaces)
- [Aspire and Visual Studio Code Dev Containers](https://learn.microsoft.com/dotnet/aspire/get-started/dev-containers)

## Folder Structure

```
.
|-- src/                              Application and test projects
|   |-- AppHost/                      Aspire orchestration and deployment model
|   |-- AppHost.Tests/                AppHost integration tests
|   |-- Hosts/                        Deployable applications
|   |   |-- Hosts.WebApi/             ASP.NET Core host
|   |   `-- Hosts.Worker/             Background worker host
|   |-- Modules/                      Application modules
|   |   |-- Example/                  Example module directory
|   |   |   |-- Modules.Example/       Module implementation project
|   |   |   |-- Modules.Example.Contracts/ Public contracts for the module
|   |   |   `-- Modules.Example.Tests/ Module tests
|   |   `-- Reporting/                Reporting module directory
|   |-- ServiceDefaults/              Shared Aspire service configuration
|   `-- Shared.Kernel/                Cross-cutting application helpers and infra plumbing
|-- Aspire.Template.slnx               Root solution for editor discovery
`-- .scripts/create_module.sh          Module scaffolding script
```

## Service Bus emulator

**The following settings are required for this application's Wolverine integration:**

1. **Keep the emulator's host ports fixed to their defaults:** `5672` for AMQP
   and `5300` for HTTP management and health checks. Wolverine's emulator
   auto-provisioning expects these default ports and cannot be configured to use
   different ports in this setup. Aspire otherwise randomizes host ports. Keep
   both explicit port assignments and `IsProxied = false` in the AppHost.
2. **Keep the emulator configured with `ContainerLifetime.Persistent`.** Manual
   testing found that restarting the Service Bus resource in Aspire with fixed
   ports causes port collisions and prevents the emulator from starting.
   Persistent lifetime keeps the emulator and its SQL Server dependency running
   across AppHost stops and starts, avoiding that restart cycle.

These settings must be kept together; changing either breaks this local setup.
Avoid restarting the Service Bus resource from the Aspire dashboard. To stop the
AppHost and remove its persistent resources, use `aspire stop --force`; persistent
volumes are preserved unless `--volumes` is also supplied.

## API documentation

When running locally with Aspire, open <http://localhost:18080/> or the `scalar`
resource endpoint in the Aspire dashboard to browse the Web API and send requests.
Scalar uses a fixed host port of `18080`, which the devcontainer forwards to the
same local port so the URL can be bookmarked. Scalar uses the Web
API's existing OpenAPI document at `/openapi/v1.json`, which is available in the
Development environment. The Scalar resource is only added in Aspire run mode.

## Create a module

From the repository root, run:

```bash
./.scripts/create_module.sh Orders
```

This creates a separate `src/Modules/Orders/` directory containing `Modules.Orders/`, `Modules.Orders.Contracts/`, and `Modules.Orders.Tests/`, adds them to the solution, and restores the repository.

### Project References

- `AppHost` can reference deployable projects under `src/Hosts/`.
- A host can reference `ServiceDefaults` and `Shared.Kernel`.
- A module's implementation project can reference its `.Contracts` project and `Shared.Kernel`.
- A module's tests can reference its implementation project and `.Contracts` project.
- `.Contracts` projects may not reference any other projects.
- Keep `Shared.Kernel` independent of hosts and modules. Keep packages to a minimum.
