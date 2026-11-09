using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Rename;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.Rename;

internal class RenameLanguageEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPut("/languages/{languageId:guid}/display-name", async (
            [FromRoute] Guid languageId, [FromBody] RenameLanguageModel language, ISender sender) =>
        {
            ErrorOr<RenameLanguageCommandResponse> res = await sender.Send(
                new RenameLanguageCommand(new LanguageId(languageId), new LanguageDisplayName(language.DisplayName)));

            return res.ToApiResult();
        });
    }
}
