using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Administration.Settings.Application.Configurations.Translations;

/// <summary>
/// The Translation aggregate's texts: one field per command property and list column a person sees, the four
/// statuses, the translation pages' titles, and its validators' messages.
/// </summary>
public static class TranslationTexts
{
    public static readonly TextCategory Category = TextCategory.Create("Settings", "Translations");

    private static readonly TextCategory errors = Category.Child("Errors");

    public static readonly FieldText LanguageId = Category.Field("LanguageId", "Language");

    public static readonly FieldText Key = Category.Field("Key", "Key");

    public static readonly FieldText Text = Category.Field("Text", "Translation");

    public static readonly FieldText Default = Category.Field("Default", "Default (English)");

    public static readonly FieldText Module = Category.Field("Module", "Module");

    public static readonly FieldText CategoryField = Category.Field("Category", "Category");

    public static readonly FieldText Status = Category.Field("Status", "Status");

    public static readonly TextDefinition Singular = Category.Text("Singular", "Translation");

    /// <summary>A translation whose placeholders are not the default's; <c>{Expected}</c> lists the default's.</summary>
    public static readonly TextDefinition PlaceholdersDiffer = errors.Text(
        "PlaceholdersDiffer",
        "The translation must use the same placeholders as the default: {Expected}.",
        "{Expected} lists the placeholders, such as {DeviceId}; keep them as they are written.");

    /// <summary>What <see cref="PlaceholdersDiffer"/> lists when the default has no placeholder.</summary>
    public static readonly TextDefinition NoPlaceholders = errors.Text("NoPlaceholders", "none");

    public static readonly TextDefinition UnknownKey = errors.Text("UnknownKey", "The application declares no text with this key.");

    public static readonly TextDefinition AlreadyTranslated =
        errors.Text("AlreadyTranslated", "This text is already translated in this language.");

    public static readonly TextDefinition NotOrphaned =
        errors.Text("NotOrphaned", "Only a translation of a text the application no longer declares can be deleted.");
}
