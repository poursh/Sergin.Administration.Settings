using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetOne;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Administration.Settings.Presentation.WebApi.Translations.Endpoints.GetOne;

internal class GetTranslationEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/translations/{translationId:guid}", async ([FromRoute] Guid translationId, ISender sender) =>
        {
            ErrorOr<TranslationQueryResponse> res = await sender.Send(new GetTranslationByIdQueryCommand(new TranslationId(translationId)));

            return res.ToApiResult();
        });
    }
}
