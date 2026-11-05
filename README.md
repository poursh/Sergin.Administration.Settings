# Sergin.Platform

**The platform module of the Sergin platform — schema `pf`. It holds the features the platform itself needs, with no business domain of its own, so any Sergin host can compose it.**

[![.NET 10](https://img.shields.io/badge/.NET-10-512BD4)](https://dotnet.microsoft.com/)
[![Schema](https://img.shields.io/badge/schema-pf-336791)](Sergin.Platform.Infrastructure.Data)
[![Embed-only](https://img.shields.io/badge/builds-embed--only-B45309)](#this-repository-does-not-build-on-its-own)
[![License: MIT](https://img.shields.io/badge/License-MIT-2E7D32)](LICENSE)

Sergin is a .NET 10 modular monolith. Its host, [Sergin.MeterMinder](https://github.com/poursh/Sergin.MeterMinder), composes this module with **DeviceManagement** and **UserAccess**.

## What belongs here

Platform features: localization first (languages and their translations), and later settings, operational monitors and the like. Business features (meters, devices, customers) never do, and identity and access stay in [Sergin.UserAccess](https://github.com/poursh/Sergin.UserAccess).

The module is named `Platform`, not `System`: a `Sergin.System.*` namespace would capture every `System.*` reference inside it, and the `sys` permission prefix is reserved for SharedKernel.

| | |
|---|---|
| Module name | `Platform` |
| Postgres schema | `pf` |
| Text key segment | `platform` (`platform.menu-title`) |
| Page routes | under `/pf/` |
| Drawer group | `Platform`, order 300 (hidden until a feature adds an entry) |

## Projects

The usual Sergin layering, one project per layer:

| Project | Holds |
|---|---|
| `Sergin.Platform.Domain` | Aggregates, strongly-typed ids, repository interfaces |
| `Sergin.Platform.Application.Contracts` | MediatR request and response records |
| `Sergin.Platform.Application.Configurations` | Command and aggregate configurations, permission definitions, declared texts (`PlatformTexts`) |
| `Sergin.Platform.Application` | Handlers, validators, `IPlatformUnitOfWork` |
| `Sergin.Platform.Infrastructure` | EF write repositories, raw-SQL query repositories |
| `Sergin.Platform.Infrastructure.Data` | `PlatformDbContext` (schema `pf`), entity configurations, migrations |
| `Sergin.Platform.Presentation.WebApi` | Minimal-API endpoints |
| `Sergin.Platform.Presentation.Blazor` | Razor class library: pages, views, `PlatformNavigation` |
| `Sergin.Platform` | Composition root: `PlatformModule` |

## This repository does not build on its own

It is **embed-only**, like Sergin.UserAccess: there is no solution file and no `Directory.Build.props` or `Directory.Packages.props` here. It compiles only as a git submodule at `src/Modules/Platform/` inside a host repository that also mounts [Sergin.SharedKernel](https://github.com/poursh/Sergin.SharedKernel) at `src/SharedKernel/`. Build and test through the host:

```bash
git clone --recurse-submodules https://github.com/poursh/Sergin.MeterMinder.git
cd Sergin.MeterMinder
dotnet build Sergin.MeterMinder.slnx
```

## License

[MIT](LICENSE)
