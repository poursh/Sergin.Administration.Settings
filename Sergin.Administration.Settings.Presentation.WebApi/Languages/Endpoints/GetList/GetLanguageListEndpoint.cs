using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetList;
using Sergin.SharedKernel.Application;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Administration.Settings.Presentation.WebApi.Languages.Endpoints.GetList;

internal class GetLanguageListEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapGet("/languages", async ([AsParameters] ListQueryRequestModel request, ISender sender) =>
            {
                ErrorOr<ListQueryResponse<GetLanguageListItem>> res = await sender.Send(
                    new GetLanguageListQueryCommand(request.ToPaggination(), request.Term, request.Filtering, request.Sorting));

                return res.ToApiResult();
            })
            .Produces<ListQueryResponse<GetLanguageListItem>>();
    }
}
