using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sergin.Platform.Application;
using Sergin.Platform.Application.Configurations;
using Sergin.Platform.Application.Contracts;
using Sergin.Platform.Infrastructure.Data;
using Sergin.Platform.Presentation.Blazor;
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

    public void AddServices(IServiceCollection services, IConfigurationSection configuration) =>
        services.AddModuleDbContext<PlatformDbContext, IPlatformDbContext, IPlatformUnitOfWork>(configuration, PlatformDbContext.Schema);

    public Task MigrateAsync(IServiceProvider services) => services.MigrateDbContextAsync<PlatformDbContext>();

    public void MapEndpoints(RouteGroupBuilder group)
    {
    }
}
