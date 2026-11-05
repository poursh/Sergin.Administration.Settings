using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Platform.Application.Contracts.Translations.Commands.GetList;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Platform.Presentation.WebApi.Translations.Endpoints.GetList;

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
                ISender sender) =>
            {
                ErrorOr<ListQueryResponse<GetTranslationListItem>> res = await sender.Send(new GetTranslationListQueryCommand(
                    new LanguageId(languageId), request.ToPaggination(), request.Term, module, category, status));

                return res.ToApiResult();
            })
            .Produces<ListQueryResponse<GetTranslationListItem>>();
    }
}
