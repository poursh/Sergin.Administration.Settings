# Sergin.Administration.Settings

**The Settings module of the Sergin platform — schema `st`. It holds the features the platform itself needs, with no business domain of its own, so any Sergin host can compose it.**

[![.NET 10](https://img.shields.io/badge/.NET-10-512BD4)](https://dotnet.microsoft.com/)
[![Schema](https://img.shields.io/badge/schema-st-336791)](Sergin.Administration.Settings.Infrastructure.Data)
[![Embed-only](https://img.shields.io/badge/builds-embed--only-B45309)](#this-repository-does-not-build-on-its-own)
[![License: MIT](https://img.shields.io/badge/License-MIT-2E7D32)](LICENSE)

Sergin is a .NET 10 modular monolith. Its host, [Sergin.MeterMinder](https://github.com/poursh/Sergin.MeterMinder), composes this module with **DeviceManagement** and **UserAccess**. Settings and UserAccess live side by side under `src/Modules/Administration/` and share one **Administration** group in the drawer. (This module was called Platform, schema `pf`, until 2026-11-06.)

## What belongs here

Settings features: localization first (languages and their translations), and later settings. Operational monitors belong in a future sibling module, `Sergin.Administration.Monitoring`, not here. Business features (meters, devices, customers) never do, and identity and access stay in [Sergin.Administration.UserAccess](https://github.com/poursh/Sergin.Administration.UserAccess).

The module is named `Settings`, not `System`: a `Sergin.System.*` namespace would capture every `System.*` reference inside it, and the `sys` permission prefix is reserved for SharedKernel.

| | |
|---|---|
| Module name | `Settings` |
| Postgres schema | `st` |
| Text key segment | `settings` (`settings.languages.title`) |
| Page routes | under `/st/` |
| Drawer group | the shared Administration group; Languages is entry order 300 |

## Projects

The usual Sergin layering, one project per layer:

| Project | Holds |
|---|---|
| `Sergin.Administration.Settings.Domain` | Aggregates, strongly-typed ids, repository interfaces |
| `Sergin.Administration.Settings.Application.Contracts` | MediatR request and response records |
| `Sergin.Administration.Settings.Application.Configurations` | Command and aggregate configurations, permission definitions, declared texts (`LanguageTexts`, `TranslationTexts`) |
| `Sergin.Administration.Settings.Application` | Handlers, validators, `ISettingsUnitOfWork` |
| `Sergin.Administration.Settings.Infrastructure` | EF write repositories, raw-SQL query repositories |
| `Sergin.Administration.Settings.Infrastructure.Data` | `SettingsDbContext` (schema `st`), entity configurations, migrations |
| `Sergin.Administration.Settings.Presentation.WebApi` | Minimal-API endpoints |
| `Sergin.Administration.Settings.Presentation.Blazor` | Razor class library: pages, views, `SettingsNavigation` |
| `Sergin.Administration.Settings` | Composition root: `SettingsModule` |

## This repository does not build on its own

It is **embed-only**, like Sergin.Administration.UserAccess: there is no solution file and no `Directory.Build.props` or `Directory.Packages.props` here. It compiles only as a git submodule at `src/Modules/Administration/Settings/` inside a host repository that also mounts [Sergin.SharedKernel](https://github.com/poursh/Sergin.SharedKernel) at `src/SharedKernel/`. Build and test through the host (on Windows run `git config --global core.longpaths true` first, since some paths in the sibling UserAccess module exceed 260 characters):

```bash
git clone --recurse-submodules https://github.com/poursh/Sergin.MeterMinder.git
cd Sergin.MeterMinder
dotnet build Sergin.MeterMinder.slnx
```

## License

[MIT](LICENSE)
