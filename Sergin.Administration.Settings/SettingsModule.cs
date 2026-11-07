using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sergin.Administration.Settings.Application;
using Sergin.Administration.Settings.Application.Configurations;
using Sergin.Administration.Settings.Application.Contracts;
using Sergin.Administration.Settings.Infrastructure.Data;
using Sergin.Administration.Settings.Infrastructure.Translations;
using Sergin.Administration.Settings.Languages;
using Sergin.Administration.Settings.Presentation.Blazor;
using Sergin.Administration.Settings.Translations;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Infrastructure.Data.EFCore;
using Sergin.SharedKernel.Modules;

namespace Sergin.Administration.Settings;

public sealed class SettingsModule : ISerginWebApiModule, ISerginWebUiModule
{
    public string Name => "Settings";

    public string Schema => SettingsDbContext.Schema;

    public Assembly ApplicationAssembly => SettingsApplicationAssemblyReference.Assembly;

    public Assembly ContractsAssembly => SettingsApplicationContractsAssemblyReference.Assembly;

    public Assembly ConfigurationsAssembly => SettingsApplicationConfigurationsAssemblyReference.Assembly;

    public Assembly UiAssembly => SettingsBlazorAssemblyReference.Assembly;

    public SerginNavGroup NavGroup => SettingsNavigation.Group;

    public IReadOnlyCollection<SerginNavItem> NavItems => SettingsNavigation.Items;

    public void AddServices(IServiceCollection services, IConfigurationSection configuration)
    {
        services.AddModuleDbContext<SettingsDbContext, ISettingsDbContext, ISettingsUnitOfWork>(configuration, SettingsDbContext.Schema);

        services.AddLanguageDependencies();
        services.AddTranslationDependencies();

        // SharedKernel registered the deny-all answer first; the module owning translations supplies the real one.
        services.Replace(ServiceDescriptor.Scoped<IDraftLanguagePreviewPolicy, TranslatorDraftPreviewPolicy>());
    }

    public Task MigrateAsync(IServiceProvider services) => services.MigrateDbContextAsync<SettingsDbContext>();

    public void MapEndpoints(RouteGroupBuilder group) => group.MapLanguageEndpoints().MapTranslationEndpoints();
}
