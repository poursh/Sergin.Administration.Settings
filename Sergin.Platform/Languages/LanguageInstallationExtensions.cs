using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sergin.Platform.Application.Languages;
using Sergin.Platform.Application.Languages.Commands.GetAudit;
using Sergin.Platform.Application.Languages.Commands.GetList;
using Sergin.Platform.Application.Languages.Commands.GetOne;
using Sergin.Platform.Application.Localizations;
using Sergin.Platform.Domain.Languages;
using Sergin.Platform.Infrastructure.Languages.Repositories;
using Sergin.Platform.Infrastructure.Languages.Repositories.Queries;
using Sergin.Platform.Infrastructure.Localizations;
using Sergin.Platform.Presentation.WebApi.Languages.Endpoints.Add;
using Sergin.Platform.Presentation.WebApi.Languages.Endpoints.Delete;
using Sergin.Platform.Presentation.WebApi.Languages.Endpoints.GetAudit;
using Sergin.Platform.Presentation.WebApi.Languages.Endpoints.GetList;
using Sergin.Platform.Presentation.WebApi.Languages.Endpoints.GetOne;
using Sergin.Platform.Presentation.WebApi.Languages.Endpoints.Publish;
using Sergin.Platform.Presentation.WebApi.Languages.Endpoints.Rename;
using Sergin.Platform.Presentation.WebApi.Languages.Endpoints.SetDirection;
using Sergin.Platform.Presentation.WebApi.Languages.Endpoints.Unpublish;
using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Platform.Languages;

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
        services.Replace(ServiceDescriptor.Singleton<ITranslationStore, PlatformTranslationStore>());

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
