using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetList;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Administration.Settings.Presentation.WebApi.Translations.Endpoints.GetList;

internal class GetTranslationListEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapGet("/languages/{languageId:guid}/translations", async (
                [FromRoute] Guid languageId,
                [AsParameters] ListQueryRequestModel request,
                [FromQuery] string? module,
                [FromQuery] string? category,
                [FromQuery] TranslationStatus? status,
                [FromQuery] TextOrigin? origin,
                ISender sender) =>
            {
                ErrorOr<ListQueryResponse<GetTranslationListItem>> res = await sender.Send(new GetTranslationListQueryCommand(
                    new LanguageId(languageId), request.ToPaggination(), request.Term, module, category, status, origin));

                return res.ToApiResult();
            })
            .Produces<ListQueryResponse<GetTranslationListItem>>();
    }
}
