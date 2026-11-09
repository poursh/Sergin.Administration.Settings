using Sergin.Administration.Settings.Domain.Languages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetOne;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.GetOne;

internal class GetLanguageEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/languages/{languageId:guid}", async ([FromRoute] Guid languageId, ISender sender) =>
        {
            ErrorOr<LanguageQueryResponse> res = await sender.Send(new GetLanguageByIdQueryCommand(new LanguageId(languageId)));

            return res.ToApiResult();
        });
    }
}
