# Getting started with Aspire and Dev Containers

This is a repository template to streamline the process of getting started with Aspire using Dev Containers in both Visual Studio Code and GitHub Codespaces. Please refer to our product documentation on how to use these repository templates to get started.

- [Aspire and GitHub Codespaces](https://learn.microsoft.com/dotnet/aspire/get-started/github-codespaces)
- [Aspire and Visual Studio Code Dev Containers](https://learn.microsoft.com/dotnet/aspire/get-started/dev-containers)

## Devcontainer

The image starts from Ubuntu 24.04. Pin .NET SDK, Node.js and pnpm versions
at the top of `.devcontainer/Dockerfile.dev`, then rebuild the container.
Development HTTPS certificates are created and trusted inside the container
at startup; host browser trust is separate. Bash history stays in its volume.

Codex uses the host's `~/.codex` directory through a writable bind mount at the
same path, sharing configuration, file-based login, skills and plugins across
containers. Changes also apply on the host. Host keyring credentials and
host-only executables are not shared. Use this same mount and `CODEX_HOME`
setting in other devcontainers. Docker must run on the machine holding that
host directory.

## Folder Structure

Use this layout as the project grows. Entries marked `(example)` illustrate where
additional apps, packages, and workspace files belong; create them only when needed.

```text
.
|-- src/
|   |-- Apps/                             Independently runnable applications
|   |   |-- Api/                          ASP.NET Core API
|   |   |-- Worker/                       Background processing
|   |   |-- Storefront/                   Fullstack web app (example)
|   |   |-- Admin/                        Admin web app (example)
|   |   `-- Mobile/                       Mobile app (example)
|   |-- Modules/                          Backend business capabilities
|   |   |-- Example/
|   |   |   |-- Modules.Example/           Module implementation
|   |   |   |-- Modules.Example.Contracts/ Public contracts
|   |   |   `-- Modules.Example.Tests/     Module tests
|   |   `-- Reporting/
|   |-- Packages/                         Shared JS/TS code (example)
|   |   |-- ui/                           Shared UI components (example)
|   |   `-- api-client/                   Shared API client (example)
|   |-- Shared.Kernel/                    Shared backend primitives
|   `-- Aspire/
|       |-- AppHost/                      Orchestration and deployment model
|       |-- AppHost.Tests/                AppHost integration tests
|       `-- ServiceDefaults/              Shared Aspire service configuration
|-- docs/                                 Project documentation
|-- .scripts/                             Repository tools and scaffolding
|-- .devcontainer/                        Development environment
|-- package.json                          Workspace commands
|-- pnpm-workspace.yaml                   JS/TS workspace membership
`-- Aspire.Template.slnx                  .NET solution
```

Name apps by purpose and keep their source, assets, configuration, and tests
together. A fullstack app keeps its frontend and server code in the same app;
PWA support stays there unless it serves a separate product. Keep backend business
logic in modules, and extract shared JS/TS packages only when multiple apps need them.
Tests stay near their owner; add root `tests/` only for tests spanning applications.

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

Control Scalar's endpoint groupings with `.WithTags("Group name")` on each endpoint
mapping. Endpoints with the same tag appear together; the weather endpoints use
`Weather forecast`. `.WithName(...)` sets the operation name rather than its group.

## Create a module

From the repository root, run:

```bash
./.scripts/create_module.sh Orders
```

This creates a separate `src/Modules/Orders/` directory containing `Modules.Orders/`, `Modules.Orders.Contracts/`, and `Modules.Orders.Tests/`, adds them to the solution, and restores the repository.

### Adding applications

Add independently runnable apps under `src/Apps/`, named by purpose, such as
`Storefront`, `Admin`, or `Mobile`. Keep each app's source, assets, configuration,
and tests together using its framework's conventions. PWA support belongs inside
the web app unless it is a separate product.

Register apps that Aspire should manage in `src/Aspire/AppHost/`. Application folders and
project files use matching names.

The pnpm workspace includes JavaScript projects under `src/Apps/*` and
`src/Packages/*`. Each needs its own `package.json`; .NET projects are unaffected.
Add shared packages such as `ui` or `api-client` only when multiple apps need them.

Run `pnpm install` at the repository root after dependency changes. Run `pnpm dev`
to start Aspire and its registered apps. Each app owns its dev/build/test scripts;
Aspire launches its development script and manages its lifecycle.

Run `dotnet tool restore` once to install CSharpier. Use `pnpm format` to apply
.NET style/analyzer fixes and format everything with CSharpier and Prettier.
Use `pnpm lint` to check all of these rules without editing files.

Use `pnpm format:js` or `pnpm lint:js` for ESLint and Prettier only. Shared tooling
and Prettier configuration live at the root; apps keep their ESLint rules.
Prettier sorts Storefront's Tailwind classes using its CSS entry point.

### Project References

- `AppHost` can reference deployable projects under `src/Apps/`.
- A host can reference `ServiceDefaults` and `Shared.Kernel`.
- A module's implementation project can reference its `.Contracts` project and `Shared.Kernel`.
- A module's tests can reference its implementation project and `.Contracts` project.
- `.Contracts` projects may not reference any other projects.
- Keep `Shared.Kernel` independent of hosts and modules. Keep packages to a minimum.
