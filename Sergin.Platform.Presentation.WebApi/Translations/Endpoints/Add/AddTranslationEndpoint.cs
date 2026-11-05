using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Platform.Application.Contracts.Translations.Commands.Add;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Platform.Presentation.WebApi.Translations.Endpoints.Add;

internal class AddTranslationEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapPost("/languages/{languageId:guid}/translations", async (
                [FromRoute] Guid languageId, [FromBody] NewTranslationModel translation, ISender sender) =>
            {
                ErrorOr<AddTranslationCommandResponse> res = await sender.Send(
                    new AddTranslationCommand(new LanguageId(languageId), translation.Key, translation.Text));

                return res.ToApiResult();
            })
            .Produces<AddTranslationCommandResponse>();
    }
}
