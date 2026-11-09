using Sergin.Administration.Settings.Domain.Languages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.SetDirection;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.SetDirection;

internal class SetLanguageDirectionEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPut("/languages/{languageId:guid}/direction", async (
            [FromRoute] Guid languageId, [FromBody] SetLanguageDirectionModel language, ISender sender) =>
        {
            ErrorOr<SetLanguageDirectionCommandResponse> res =
                await sender.Send(new SetLanguageDirectionCommand(new LanguageId(languageId), language.Direction));

            return res.ToApiResult();
        });
    }
}
