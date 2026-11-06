using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetList;

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
