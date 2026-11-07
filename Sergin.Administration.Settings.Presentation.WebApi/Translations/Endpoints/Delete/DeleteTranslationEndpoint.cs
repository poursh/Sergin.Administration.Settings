using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.Delete;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Administration.Settings.Presentation.WebApi.Translations.Endpoints.Delete;

internal class DeleteTranslationEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapDelete("/translations/{translationId:guid}", async ([FromRoute] Guid translationId, ISender sender) =>
        {
            ErrorOr<DeleteTranslationCommandResponse> res = await sender.Send(new DeleteTranslationCommand(new TranslationId(translationId)));

            return res.ToApiResult();
        });
    }
}
