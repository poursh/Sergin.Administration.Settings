using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Sergin.Platform.Application.Translations;
using Sergin.Platform.Application.Translations.Commands.GetOne;
using Sergin.Platform.Domain.Translations;
using Sergin.Platform.Infrastructure.Translations.Repositories;
using Sergin.Platform.Infrastructure.Translations.Repositories.Queries;
using Sergin.Platform.Presentation.WebApi.Translations.Endpoints.Add;
using Sergin.Platform.Presentation.WebApi.Translations.Endpoints.Change;
using Sergin.Platform.Presentation.WebApi.Translations.Endpoints.Delete;
using Sergin.Platform.Presentation.WebApi.Translations.Endpoints.GetList;
using Sergin.Platform.Presentation.WebApi.Translations.Endpoints.GetOne;
using Sergin.Platform.Presentation.WebApi.Translations.Endpoints.GetProgress;

namespace Sergin.Platform.Translations;

// The translation store and the cache refresher are registered with the languages (LanguageInstallationExtensions):
// one store answers both, and both aggregates' handlers share the refresher.
internal static class TranslationInstallationExtensions
{
    internal static IServiceCollection AddTranslationDependencies(this IServiceCollection services)
    {
        services.AddTransient<ITranslationRepository, TranslationRepository>();
        services.AddTransient<ITranslationAllQueryRepository, TranslationQueryRepository>();
        services.AddTransient<IGetTranslationQueryRepository, TranslationQueryRepository>();
        services.AddTransient<ITranslationRowQueryRepository, TranslationQueryRepository>();

        return services;
    }

    internal static IEndpointRouteBuilder MapTranslationEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        new AddTranslationEndpoint().MapEndpoint(routeBuilder);
        new GetTranslationEndpoint().MapEndpoint(routeBuilder);
        new GetTranslationListEndpoint().MapEndpoint(routeBuilder);
        new GetLanguageProgressEndpoint().MapEndpoint(routeBuilder);
        new ChangeTranslationEndpoint().MapEndpoint(routeBuilder);
        new DeleteTranslationEndpoint().MapEndpoint(routeBuilder);

        return routeBuilder;
    }
}
