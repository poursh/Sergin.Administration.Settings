using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sergin.Administration.Settings.Application.Languages;
using Sergin.Administration.Settings.Application.Languages.Commands.GetAudit;
using Sergin.Administration.Settings.Application.Languages.Commands.GetList;
using Sergin.Administration.Settings.Application.Languages.Commands.GetOne;
using Sergin.Administration.Settings.Application.Localizations;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.Administration.Settings.Infrastructure.Languages.Repositories;
using Sergin.Administration.Settings.Infrastructure.Languages.Repositories.Queries;
using Sergin.Administration.Settings.Infrastructure.Localizations;
using Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.Add;
using Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.Delete;
using Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.GetAudit;
using Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.GetList;
using Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.GetOne;
using Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.Publish;
using Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.Rename;
using Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.SetDirection;
using Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.Unpublish;
using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Administration.Settings.Languages;

internal static class LanguageInstallationExtensions
{
    internal static IServiceCollection AddLanguageDependencies(this IServiceCollection services)
    {
        services.AddTransient<ILanguageRepository, LanguageRepository>();
        services.AddTransient<ILanguageAllQueryRepository, LanguageQueryRepository>();
        services.AddTransient<IGetLanguageQueryRepository, LanguageQueryRepository>();
        services.AddTransient<IGetLanguageListQueryRepository, LanguageQueryRepository>();
        services.AddTransient<IGetLanguageAuditQueryRepository, LanguageQueryRepository>();

        // Replace, not TryAdd: AddSerginCore registers SharedKernel's empty store before the module loop runs.
        services.Replace(ServiceDescriptor.Singleton<ITranslationStore, SettingsTranslationStore>());

        // The handlers' after-commit cache refresh; a singleton over the singleton cache.
        services.AddSingleton<TranslationCacheRefresher>();

        return services;
    }

    internal static IEndpointRouteBuilder MapLanguageEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        new AddLanguageEndpoint().MapEndpoint(routeBuilder);
        new GetLanguageEndpoint().MapEndpoint(routeBuilder);
        new GetLanguageListEndpoint().MapEndpoint(routeBuilder);
        new RenameLanguageEndpoint().MapEndpoint(routeBuilder);
        new SetLanguageDirectionEndpoint().MapEndpoint(routeBuilder);
        new PublishLanguageEndpoint().MapEndpoint(routeBuilder);
        new UnpublishLanguageEndpoint().MapEndpoint(routeBuilder);
        new DeleteLanguageEndpoint().MapEndpoint(routeBuilder);
        new GetLanguageAuditEndpoint().MapEndpoint(routeBuilder);

        return routeBuilder;
    }
}
