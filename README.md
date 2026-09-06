# Getting started with Aspire and Dev Containers

This is a repository template to streamline the process of getting started with Aspire using Dev Containers in both Visual Studio Code and GitHub Codespaces. Please refer to our product documentation on how to use these repository templates to get started.

- [Aspire and GitHub Codespaces](https://learn.microsoft.com/dotnet/aspire/get-started/github-codespaces)
- [Aspire and Visual Studio Code Dev Containers](https://learn.microsoft.com/dotnet/aspire/get-started/dev-containers)

## Folder Structure

```
.
|-- AppHost/                           Aspire orchestration and deployment model
|-- AppHost.Tests/                     AppHost integration tests
|-- Hosts/                             Deployable applications
|   `-- WebApi/                         Example ASP.NET Core host
|-- Modules.Example.Core/              Module implementation
|-- Modules.Example.Contracts/         Public contracts for the module
|-- Modules.Example.Tests/             Module tests
|-- ServiceDefaults/                   Shared Aspire service configuration
|-- Shared.Kernel/                     Cross-cutting application helpers and infra plumbing
|-- Aspire.Template.slnx               Solution
`-- .scripts/create_module.sh          Module scaffolding script
```

### Create a module

From the repository root, run:

```bash
./.scripts/create_module.sh Orders
```

This creates `Modules.Orders.Core`, `Modules.Orders.Contracts`, and `Modules.Orders.Tests`, adds them to the solution, and restores the repository.

### Project References

- `AppHost` can reference deployable projects under `Hosts/`.
- A host can reference `ServiceDefaults` and `Shared.Kernel`.
- A module can reference its own `.Contracts` project and `Shared.Kernel`.
- A module's tests can reference the module and its `.Contracts` project.
- `.Contracts` projects may not reference any other projects.
- Keep `Shared.Kernel` independent of hosts and modules. Keep packages to a minimum, and absolutely no infrastructure allowed.
