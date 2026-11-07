using Sergin.Administration.Settings.Domain.Languages;

namespace Sergin.Administration.Settings.Presentation.Blazor.Languages;

/// <summary>The declared text of each language status and direction setting, so every view names them alike.</summary>
public static class LanguageLabels
{
    public static IReadOnlyList<LanguageDirection> Directions { get; } =
        [LanguageDirection.FromCulture, LanguageDirection.LeftToRight, LanguageDirection.RightToLeft];

    public static TextDefinition Status(LanguageStatus status) =>
        status == LanguageStatus.Published ? LanguageUiTexts.Published : LanguageUiTexts.Draft;

    public static TextDefinition Direction(LanguageDirection direction) => direction switch
    {
        LanguageDirection.LeftToRight => LanguageUiTexts.LeftToRight,
        LanguageDirection.RightToLeft => LanguageUiTexts.RightToLeft,
        _ => LanguageUiTexts.FromCulture,
    };
}
