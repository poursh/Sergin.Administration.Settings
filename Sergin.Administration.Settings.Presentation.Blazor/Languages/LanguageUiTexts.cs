using Sergin.Administration.Settings.Application.Configurations.Languages;

namespace Sergin.Administration.Settings.Presentation.Blazor.Languages;

/// <summary>
/// The texts only the Blazor pages show. The fields, the name and the error texts are in <see cref="LanguageTexts"/>,
/// whose category these build on, so each key is the one the Application layer declared before.
/// </summary>
public static class LanguageUiTexts
{
    private static readonly TextCategory category = LanguageTexts.Category;

    private static readonly TextCategory directions = category.Child("Directions");

    private static readonly TextCategory statuses = category.Child("Statuses");

    private static readonly TextCategory overview = category.Child("Overview");

    public static readonly TextDefinition Title = category.Text("Title", "Languages");

    public static readonly TextDefinition New = category.Text("New", "New language");

    public static readonly TextDefinition SearchPlaceholder = category.Text("SearchPlaceholder", "Search culture or name…");

    public static readonly TextDefinition Publish = category.Text("Publish", "Publish", "Offers a draft language to every user.");

    public static readonly TextDefinition Unpublish =
        category.Text("Unpublish", "Unpublish", "Takes a published language back to a draft only translators see.");

    public static readonly TextDefinition DeleteTitle = category.Text("DeleteTitle", "Delete language");

    public static readonly TextDefinition DeleteConfirm = category.Text(
        "DeleteConfirm",
        "Delete {DisplayName} and all its translations? Its users see English again.",
        "{DisplayName} is the language's own name for itself.");

    public static readonly TextDefinition Translations =
        category.Text("Translations", "Translations", "Opens the list of a language's translations.");

    public static readonly TextDefinition OverviewTranslated = overview.Text("Translated", "Translated");

    public static readonly TextDefinition OverviewPercent =
        overview.Text("Percent", "{Percent}%", "The share of the declared texts a language translates; {Percent} is a whole number.");

    public static readonly TextDefinition FromCulture =
        directions.Text("FromCulture", "From the culture", "The direction the culture itself is written in.");

    public static readonly TextDefinition LeftToRight = directions.Text("LeftToRight", "Left to right");

    public static readonly TextDefinition RightToLeft = directions.Text("RightToLeft", "Right to left");

    public static readonly TextDefinition DirectionResolved = directions.Text(
        "Resolved", "{Setting} ({Resolved})", "A direction setting and the direction it gives, as in: From the culture (Right to left).");

    public static readonly TextDefinition Draft = statuses.Text("Draft", "Draft", "A language only translators see.");

    public static readonly TextDefinition Published = statuses.Text("Published", "Published", "A language every user may choose.");
}
