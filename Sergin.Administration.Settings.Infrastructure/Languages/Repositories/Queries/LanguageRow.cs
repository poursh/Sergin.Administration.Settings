using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetList;
using Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetOne;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Administration.Settings.Infrastructure.Languages.Repositories.Queries;

/// <summary>One st.language row as Dapper reads it: the enums by their stored names, the direction not yet resolved.</summary>
internal sealed record LanguageRow(Guid Id, string Culture, string DisplayName, string Direction, string Status)
{
    private LanguageDirection DirectionValue => Enum.Parse<LanguageDirection>(Direction);

    private LanguageStatus StatusValue => Enum.Parse<LanguageStatus>(Status);

    private bool IsRightToLeft => Language.ResolveRightToLeft(Culture, DirectionValue);

    public LanguageQueryResponse ToResponse() => new(Id, Culture, DisplayName, DirectionValue, IsRightToLeft, StatusValue);

    public GetLanguageListItem ToListItem() => new(Id, Culture, DisplayName, IsRightToLeft, StatusValue);

    public LanguageInfo ToLanguageInfo() => new(Culture, DisplayName, IsRightToLeft, StatusValue == LanguageStatus.Published);
}
