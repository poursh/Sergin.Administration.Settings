using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Sergin.Administration.Settings.Application.Translations;
using Sergin.Administration.Settings.Application.Translations.Commands.GetOne;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.Administration.Settings.Infrastructure.Translations.Repositories;
using Sergin.Administration.Settings.Infrastructure.Translations.Repositories.Queries;
using Sergin.Administration.Settings.Presentation.WebApi.Translations.Endpoints.Add;
using Sergin.Administration.Settings.Presentation.WebApi.Translations.Endpoints.Change;
using Sergin.Administration.Settings.Presentation.WebApi.Translations.Endpoints.Delete;
using Sergin.Administration.Settings.Presentation.WebApi.Translations.Endpoints.GetList;
using Sergin.Administration.Settings.Presentation.WebApi.Translations.Endpoints.GetOne;
using Sergin.Administration.Settings.Presentation.WebApi.Translations.Endpoints.GetProgress;

namespace Sergin.Administration.Settings.Translations;

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
