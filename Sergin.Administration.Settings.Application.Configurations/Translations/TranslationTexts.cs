using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Administration.Settings.Application.Configurations.Translations;

/// <summary>
/// The Translation aggregate's texts: one field per command property and list column a person sees, the four
/// statuses, the translation pages' titles, and its validators' messages.
/// </summary>
public static class TranslationTexts
{
    public static readonly TextCategory Category = TextCategory.Create("Settings", "Translations");

    private static readonly TextCategory statuses = Category.Child("Statuses");

    private static readonly TextCategory errors = Category.Child("Errors");

    public static readonly FieldText LanguageId = Category.Field("LanguageId", "Language");

    public static readonly FieldText Key = Category.Field("Key", "Key");

    public static readonly FieldText Text = Category.Field("Text", "Translation");

    public static readonly FieldText Default = Category.Field("Default", "Default (English)");

    public static readonly FieldText Module = Category.Field("Module", "Module");

    public static readonly FieldText CategoryField = Category.Field("Category", "Category");

    public static readonly FieldText Status = Category.Field("Status", "Status");

    public static readonly TextDefinition Title = Category.Text("Title", "Translations");

    public static readonly TextDefinition Singular = Category.Text("Singular", "Translation");

    public static readonly TextDefinition Progress =
        Category.Text("Progress", "{Translated} of {Total} translated", "How many of the declared texts a language translates.");

    public static readonly TextDefinition SearchPlaceholder =
        Category.Text("SearchPlaceholder", "Search key, default or translation…");

    public static readonly TextDefinition FilterChip =
        Category.Text("FilterChip", "{Field}: {Value}", "An applied filter: the field's label, then the chosen value.");

    public static readonly TextDefinition Saved = Category.Text("Saved", "Translation saved.");

    public static readonly TextDefinition ChangedMeanwhile = Category.Text(
        "ChangedMeanwhile",
        "Someone changed this translation after you opened it; it now reads: {Text}. Save again to replace it with yours.",
        "{Text} is the stored translation, shown as it is.");

    public static readonly TextDefinition DeleteTitle = Category.Text("DeleteTitle", "Delete translation");

    public static readonly TextDefinition DeleteConfirm = Category.Text(
        "DeleteConfirm",
        "Delete the translation of {Key}? The application no longer declares this text.",
        "{Key} is the text's key, shown as it is.");

    public static readonly TextDefinition Untranslated =
        statuses.Text("Untranslated", "Untranslated", "No translation yet: the English default is shown.");

    public static readonly TextDefinition Translated = statuses.Text("Translated", "Translated");

    public static readonly TextDefinition NeedsReview =
        statuses.Text("NeedsReview", "Needs review", "The English default changed after this was translated.");

    public static readonly TextDefinition Orphaned =
        statuses.Text("Orphaned", "Orphaned", "The text no longer exists in the application; the translation is never shown.");

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
