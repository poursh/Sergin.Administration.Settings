using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Platform.Application.Contracts.Languages.Commands.GetList;

public sealed record GetLanguageListQueryCommand : ListQuery<GetLanguageListItem>
{
    public GetLanguageListQueryCommand(
        Paggination paggination,
        Term? term = default,
        Filtering? filtering = default,
        Sorting? sorting = default)
        : base(paggination, term, filtering, sorting)
    {
    }
}
