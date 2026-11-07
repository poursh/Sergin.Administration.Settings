using Sergin.Administration.Settings.Application.Configurations.Languages;
using Sergin.Administration.Settings.Domain.Languages;

namespace Sergin.Administration.Settings.Presentation.Blazor.Languages;

/// <summary>The declared text of each language status and direction setting, so every view names them alike.</summary>
public static class LanguageLabels
{
    public static IReadOnlyList<LanguageDirection> Directions { get; } =
        [LanguageDirection.FromCulture, LanguageDirection.LeftToRight, LanguageDirection.RightToLeft];

    public static TextDefinition Status(LanguageStatus status) =>
        status == LanguageStatus.Published ? LanguageTexts.Published : LanguageTexts.Draft;

    public static TextDefinition Direction(LanguageDirection direction) => direction switch
    {
        LanguageDirection.LeftToRight => LanguageTexts.LeftToRight,
        LanguageDirection.RightToLeft => LanguageTexts.RightToLeft,
        _ => LanguageTexts.FromCulture,
    };
}
