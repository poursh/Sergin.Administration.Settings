using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Unpublish;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.Unpublish;

internal class UnpublishLanguageEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/languages/{languageId:guid}/unpublish", async ([FromRoute] Guid languageId, ISender sender) =>
        {
            ErrorOr<UnpublishLanguageCommandResponse> res = await sender.Send(new UnpublishLanguageCommand(languageId));

            return res.ToApiResult();
        });
    }
}
