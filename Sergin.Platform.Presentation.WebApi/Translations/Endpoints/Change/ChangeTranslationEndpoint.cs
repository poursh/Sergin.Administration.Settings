using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Platform.Application.Contracts.Translations.Commands.Change;
using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Platform.Presentation.WebApi.Translations.Endpoints.Change;

internal class ChangeTranslationEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPut("/translations/{translationId:guid}", async (
            [FromRoute] Guid translationId, [FromBody] ChangeTranslationModel translation, ISender sender) =>
        {
            ErrorOr<ChangeTranslationCommandResponse> res = await sender.Send(
                new ChangeTranslationCommand(new TranslationId(translationId), translation.Text));

            return res.ToApiResult();
        });
    }
}
