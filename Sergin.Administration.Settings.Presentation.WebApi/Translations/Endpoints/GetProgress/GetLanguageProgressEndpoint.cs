using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetProgress;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Administration.Settings.Presentation.WebApi.Translations.Endpoints.GetProgress;

internal class GetLanguageProgressEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/languages/{languageId:guid}/translations/progress", async ([FromRoute] Guid languageId, ISender sender) =>
        {
            ErrorOr<LanguageProgressQueryResponse> res = await sender.Send(new GetLanguageProgressQueryCommand(new LanguageId(languageId)));

            return res.ToApiResult();
        });
    }
}
