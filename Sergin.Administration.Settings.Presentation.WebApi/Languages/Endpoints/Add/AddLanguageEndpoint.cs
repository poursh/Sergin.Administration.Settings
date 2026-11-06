using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.Add;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.Add;

internal class AddLanguageEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapPost("/languages", async ([FromBody] NewLanguageModel language, ISender sender) =>
            {
                ErrorOr<AddLanguageCommandResponse> res = await sender.Send(
                    new AddLanguageCommand(new CultureName(language.Culture), new LanguageDisplayName(language.DisplayName)));

                return res.ToApiResult();
            })
            .Produces<AddLanguageCommandResponse>();
    }
}
