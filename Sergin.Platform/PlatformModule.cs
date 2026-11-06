using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sergin.Platform.Application;
using Sergin.Platform.Application.Configurations;
using Sergin.Platform.Application.Contracts;
using Sergin.Platform.Infrastructure.Data;
using Sergin.Platform.Infrastructure.Translations;
using Sergin.Platform.Languages;
using Sergin.Platform.Presentation.Blazor;
using Sergin.Platform.Translations;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Infrastructure.Data.EFCore;
using Sergin.SharedKernel.Modules;

namespace Sergin.Platform;

public sealed class PlatformModule : ISerginWebApiModule, ISerginWebUiModule
{
    public string Name => "Platform";

    public string Schema => PlatformDbContext.Schema;

    public Assembly ApplicationAssembly => PlatformApplicationAssemblyReference.Assembly;

    public Assembly ContractsAssembly => PlatformApplicationContractsAssemblyReference.Assembly;

    public Assembly ConfigurationsAssembly => PlatformApplicationConfigurationsAssemblyReference.Assembly;

    public Assembly UiAssembly => PlatformBlazorAssemblyReference.Assembly;

    public SerginNavGroup NavGroup => PlatformNavigation.Group;

    public IReadOnlyCollection<SerginNavItem> NavItems => PlatformNavigation.Items;

    public void AddServices(IServiceCollection services, IConfigurationSection configuration)
    {
        services.AddModuleDbContext<PlatformDbContext, IPlatformDbContext, IPlatformUnitOfWork>(configuration, PlatformDbContext.Schema);

        services.AddLanguageDependencies();
        services.AddTranslationDependencies();

        // SharedKernel registered the deny-all answer first; the module owning translations supplies the real one.
        services.Replace(ServiceDescriptor.Scoped<IDraftLanguagePreviewPolicy, TranslatorDraftPreviewPolicy>());
    }

    public Task MigrateAsync(IServiceProvider services) => services.MigrateDbContextAsync<PlatformDbContext>();

    public void MapEndpoints(RouteGroupBuilder group) => group.MapLanguageEndpoints().MapTranslationEndpoints();
}
