using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Platform.Application.Contracts.Languages.Commands.Delete;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Platform.Presentation.WebApi.Languages.Endpoints.Delete;

internal class DeleteLanguageEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapDelete("/languages/{languageId:guid}", async ([FromRoute] Guid languageId, ISender sender) =>
        {
            ErrorOr<DeleteLanguageCommandResponse> res = await sender.Send(new DeleteLanguageCommand(languageId));

            return res.ToApiResult();
        });
    }
}
