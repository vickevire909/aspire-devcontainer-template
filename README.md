# Getting started with Aspire and Dev Containers

This is a repository template to streamline the process of getting started with Aspire using Dev Containers in both Visual Studio Code and GitHub Codespaces. Please refer to our product documentation on how to use these repository templates to get started.

- [Aspire and GitHub Codespaces](https://learn.microsoft.com/dotnet/aspire/get-started/github-codespaces)
- [Aspire and Visual Studio Code Dev Containers](https://learn.microsoft.com/dotnet/aspire/get-started/dev-containers)

## Folder Structure

```
/src
  /Hosts                              ← thin, executable, one per deployable
    /Api                              (Microsoft.NET.Sdk.Web)
    /Functions.Http                   (isolated worker)
    /Functions.Timers
    /Functions.ServiceBus

  /Modules
    /Orders
      Orders.csproj                   ← application code: Features/Domain/Data
      Orders.Contracts.csproj         ← tiny, dependency-free, public integration events
      Orders.Tests.csproj
    /Inventory
      Inventory.csproj
      Inventory.Contracts.csproj
      Inventory.Tests.csproj
    /Notifications
      (same shape)

  /Shared.Kernel                      ← Messaging/Persistence/Caching/Storage/Security helpers
  /ServiceDefaults

/tests
  /ArchitectureTests
```
