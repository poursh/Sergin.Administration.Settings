# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Sergin.Platform is the Platform module (Postgres schema `pf`, module name `Platform`, text key segment `platform`, routes under `/pf/`) of the **Sergin** platform — a .NET 10 modular monolith whose host repo, **Sergin.MeterMinder**, composes it with **DeviceManagement** and **UserAccess**. It holds the features the platform itself needs and has no business domain: localization first (languages and translations, implementing SharedKernel's `ITranslationStore`), later settings and operational monitors. Business features never belong here, and identity and access stay in UserAccess. Design: the host repo's `docs/superpowers/specs/2026-11-05-localization-design.md`, decision 20 onwards.

**This repo is embed-only.** There is no solution file, no `Directory.Build.props`/`Directory.Packages.props`, no dev config here. It only compiles as a git submodule at `src/Modules/Platform/` inside a host repo (today: [Sergin.MeterMinder](https://github.com/poursh/Sergin.MeterMinder)) that also provides `Sergin.SharedKernel` at `src/SharedKernel/`. `dotnet build`/`dotnet test` run against the *host's* solution; the host's root `CLAUDE.md` holds every cross-module convention (layering, CQRS, validation, aggregate features, Blazor views) and this file only adds what is specific to Platform.

## Current state

One aggregate, `Language`, with its write slices and reads; no page yet.

- **`Language`** (`Languages/` in every layer, no feature level above it): `Audited().Versioned()`, not soft-deletable, not scoped. `CultureName` (max 20) is normalised by `CultureName.Normalise` to `CultureInfo.GetCultureInfo(value, predefinedOnly: true).Name` (`FA-ir` → `fa-IR`) and refuses an unknown culture, the invariant culture and any English culture (`LanguageErrorKeys`, declared in `LanguageTexts`); `LanguageDisplayName` (max 100); `LanguageDirection` `FromCulture | LeftToRight | RightToLeft` and `LanguageStatus` `Draft | Published`, both stored by name. `IsRightToLeft` is derived (`Language.ResolveRightToLeft`), never stored. `ILanguageRepository` declares `IUniqueKeyRepository<CultureName>`; `IsTakenAsync` compares the normalised culture and `ix_language_culture` is the guarantee (migration `AddLanguages`).
- Slices: `AddLanguage`, `RenameLanguage`, `SetLanguageDirection`, `PublishLanguage`, `UnpublishLanguage`, `DeleteLanguage` (a hard delete), `GetLanguageById` (versioned), `GetLanguageList` (`LanguageListFields`: search culture and display name, sort by culture). Every write requires `LanguagePermissions.Manage` (`permission.pf.languages.manage`), every read `LanguagePermissions.Read`; all writes but Add `RequireExpectedVersion()`. Each write handler calls `ITranslationCache.RefreshLanguagesAsync` after `SaveChangesAsync`, since the cache loads the language list once. A WebApi endpoint ships per slice.
- **`PlatformTranslationStore`** (`.Infrastructure/Localizations`) replaces SharedKernel's empty store (`services.Replace` in `LanguageInstallationExtensions`, since `AddSerginCore` registers the default before the module loop). It is a singleton and opens a scope per call for the scoped `IDbConnectionFactory`. `GetLanguagesAsync` reads `pf.language`, drafts included, with the **resolved** direction in `LanguageInfo.IsRightToLeft`; `GetTranslationsAsync` answers empty until the `Translation` aggregate exists.

- `PlatformModule` (`Sergin.Platform`) implements `ISerginWebApiModule` and `ISerginWebUiModule`: `Name` `"Platform"`, `Schema` `PlatformDbContext.Schema` (`pf`), the three assembly markers, `UiAssembly` → `PlatformBlazorAssemblyReference.Assembly`; `AddServices`/`MapEndpoints` call `LanguageInstallationExtensions`.
- `PlatformTexts` (`.Application.Configurations`) is the module's text category, `TextCategory.Create("Platform")`, holding `MenuTitle` (`platform.menu-title`, "Platform") — the drawer group's title.
- `PlatformNavigation` (`.Presentation.Blazor`): `Group` is `(PlatformTexts.MenuTitle, Icons.Material.Filled.Settings, Order: 300)`, `Items` is empty, so the shell renders no Platform group until a feature adds an entry.
- `PlatformDbContext` (`.Infrastructure.Data`) maps schema `pf` with the `AggregateFeatures` override from the start; it does **not** opt into the outbox. Migration `InitialCreate` only ensures the `pf` schema; it was converted to a file-scoped namespace by hand, as every scaffolded migration must be.

## Conventions specific to this module

- Feature folders may carry a feature level above the aggregate (`Localization/Languages/...`) — a feature's aggregates are its text categories (`platform.languages.*`, `platform.translations.*`).
- Permissions are not scoped and named `permission.pf.<resource>.<action>` (`permission.pf.languages.read`).
- Every page route starts with `/pf/`; the host refuses to start otherwise.
- Composing the module into a host takes **two** `ProjectReference`s — `Sergin.Platform` and `Sergin.Platform.Presentation.Blazor` (static web assets do not flow through the plain-SDK composition root) — and one `new PlatformModule()` in the host's modules collection.
