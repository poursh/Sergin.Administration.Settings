using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetAudit;
using Sergin.SharedKernel.Application.Aggregates;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.GetAudit;

internal class GetLanguageAuditEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/languages/{languageId:guid}/audit", async ([FromRoute] Guid languageId, ISender sender) =>
        {
            ErrorOr<AuditInfo> res = await sender.Send(new GetLanguageAuditQueryCommand(languageId));

            return res.ToApiResult();
        });
    }
}
