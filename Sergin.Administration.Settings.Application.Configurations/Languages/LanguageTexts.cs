using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Administration.Settings.Application.Configurations.Languages;

/// <summary>
/// The Language aggregate's texts: one field per command property a person sees (a list column reads the field of its
/// <c>LanguageListFields</c> id), the direction and status values, the language pages' titles, and the messages of its
/// validators and domain errors (<c>LanguageErrorKeys</c> names the latter).
/// </summary>
public static class LanguageTexts
{
    public static readonly TextCategory Category = TextCategory.Create("Settings", "Languages");

    private static readonly TextCategory errors = Category.Child("Errors");

    public static readonly FieldText Culture = Category.Field("Culture", "Culture", hint: "A culture code such as fa-IR.");

    public static readonly FieldText DisplayName =
        Category.Field("DisplayName", "Display name", hint: "The language's name in its own script, as its readers write it.");

    public static readonly FieldText Direction = Category.Field("Direction", "Direction");

    public static readonly FieldText Status = Category.Field("Status", "Status");

    public static readonly TextDefinition Singular = Category.Text("Singular", "Language");

    /// <summary><c>AddLanguageCommandValidator</c> and <c>CultureName.Normalise</c>: a culture .NET does not know, or the invariant culture.</summary>
    public static readonly TextDefinition UnknownCulture = errors.Text("UnknownCulture", "The culture is not one .NET knows.");

    /// <summary><c>AddLanguageCommandValidator</c> and <c>CultureName.Normalise</c>: English is the code default, not a language.</summary>
    public static readonly TextDefinition BuiltInEnglish =
        errors.Text("BuiltInEnglish", "English is built in and cannot be added as a language.");
}
