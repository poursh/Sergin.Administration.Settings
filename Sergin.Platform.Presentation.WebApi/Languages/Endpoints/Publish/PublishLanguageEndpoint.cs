using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Platform.Application.Contracts.Languages.Commands.Publish;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Platform.Presentation.WebApi.Languages.Endpoints.Publish;

internal class PublishLanguageEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/languages/{languageId:guid}/publish", async ([FromRoute] Guid languageId, ISender sender) =>
        {
            ErrorOr<PublishLanguageCommandResponse> res = await sender.Send(new PublishLanguageCommand(languageId));

            return res.ToApiResult();
        });
    }
}
