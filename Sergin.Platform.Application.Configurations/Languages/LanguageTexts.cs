using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Platform.Application.Configurations.Languages;

/// <summary>
/// The Language aggregate's texts: one field per command property a person sees (a list column reads the field of its
/// <c>LanguageListFields</c> id), the direction and status values, the language pages' titles, and the messages of its
/// validators and domain errors (<c>LanguageErrorKeys</c> names the latter).
/// </summary>
public static class LanguageTexts
{
    public static readonly TextCategory Category = TextCategory.Create("Platform", "Languages");

    private static readonly TextCategory directions = Category.Child("Directions");

    private static readonly TextCategory statuses = Category.Child("Statuses");

    private static readonly TextCategory errors = Category.Child("Errors");

    private static readonly TextCategory overview = Category.Child("Overview");

    public static readonly FieldText Culture = Category.Field("Culture", "Culture", hint: "A culture code such as fa-IR.");

    public static readonly FieldText DisplayName =
        Category.Field("DisplayName", "Display name", hint: "The language's name in its own script, as its readers write it.");

    public static readonly FieldText Direction = Category.Field("Direction", "Direction");

    public static readonly FieldText Status = Category.Field("Status", "Status");

    public static readonly TextDefinition Title = Category.Text("Title", "Languages");

    public static readonly TextDefinition Singular = Category.Text("Singular", "Language");

    public static readonly TextDefinition New = Category.Text("New", "New language");

    public static readonly TextDefinition SearchPlaceholder = Category.Text("SearchPlaceholder", "Search culture or name…");

    public static readonly TextDefinition Publish = Category.Text("Publish", "Publish", "Offers a draft language to every user.");

    public static readonly TextDefinition Unpublish =
        Category.Text("Unpublish", "Unpublish", "Takes a published language back to a draft only translators see.");

    public static readonly TextDefinition DeleteTitle = Category.Text("DeleteTitle", "Delete language");

    public static readonly TextDefinition DeleteConfirm = Category.Text(
        "DeleteConfirm",
        "Delete {DisplayName} and all its translations? Its users see English again.",
        "{DisplayName} is the language's own name for itself.");

    public static readonly TextDefinition Translations =
        Category.Text("Translations", "Translations", "Opens the list of a language's translations.");

    public static readonly TextDefinition OverviewTranslated = overview.Text("Translated", "Translated");

    public static readonly TextDefinition OverviewPercent =
        overview.Text("Percent", "{Percent}%", "The share of the declared texts a language translates; {Percent} is a whole number.");

    public static readonly TextDefinition FromCulture =
        directions.Text("FromCulture", "From the culture", "The direction the culture itself is written in.");

    public static readonly TextDefinition LeftToRight = directions.Text("LeftToRight", "Left to right");

    public static readonly TextDefinition RightToLeft = directions.Text("RightToLeft", "Right to left");

    public static readonly TextDefinition Draft = statuses.Text("Draft", "Draft", "A language only translators see.");

    public static readonly TextDefinition Published = statuses.Text("Published", "Published", "A language every user may choose.");

    /// <summary><c>AddLanguageCommandValidator</c> and <c>CultureName.Normalise</c>: a culture .NET does not know, or the invariant culture.</summary>
    public static readonly TextDefinition UnknownCulture = errors.Text("UnknownCulture", "The culture is not one .NET knows.");

    /// <summary><c>AddLanguageCommandValidator</c> and <c>CultureName.Normalise</c>: English is the code default, not a language.</summary>
    public static readonly TextDefinition BuiltInEnglish =
        errors.Text("BuiltInEnglish", "English is built in and cannot be added as a language.");
}
