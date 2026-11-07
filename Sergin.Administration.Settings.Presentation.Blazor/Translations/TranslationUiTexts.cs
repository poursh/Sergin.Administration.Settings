using Sergin.Administration.Settings.Application.Configurations.Translations;

namespace Sergin.Administration.Settings.Presentation.Blazor.Translations;

/// <summary>
/// The texts only the Blazor pages show. The fields, the name and the error texts are in <see cref="TranslationTexts"/>,
/// whose category these build on, so each key is the one the Application layer declared before.
/// </summary>
public static class TranslationUiTexts
{
    private static readonly TextCategory category = TranslationTexts.Category;

    private static readonly TextCategory statuses = category.Child("Statuses");

    private static readonly TextCategory origins = category.Child("Origins");

    public static readonly TextDefinition Title = category.Text("Title", "Translations");

    public static readonly TextDefinition Progress =
        category.Text("Progress", "{Translated} of {Total} translated", "How many of the declared texts a language translates.");

    public static readonly TextDefinition SearchPlaceholder =
        category.Text("SearchPlaceholder", "Search key, default or translation…");

    public static readonly TextDefinition FilterChip =
        category.Text("FilterChip", "{Field}: {Value}", "An applied filter: the field's label, then the chosen value.");

    public static readonly TextDefinition Saved = category.Text("Saved", "Translation saved.");

    public static readonly TextDefinition ChangedMeanwhile = category.Text(
        "ChangedMeanwhile",
        "Someone changed this translation after you opened it; it now reads: {Text}. Save again to replace it with yours.",
        "{Text} is the stored translation, shown as it is.");

    public static readonly TextDefinition DeleteTitle = category.Text("DeleteTitle", "Delete translation");

    public static readonly TextDefinition DeleteConfirm = category.Text(
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

    public static readonly TextDefinition Screens =
        origins.Text("Screens", "Screens", "Texts the pages show: titles, buttons, confirmations, menus.");

    public static readonly TextDefinition Messages =
        origins.Text("Messages", "Messages", "Field names, validation messages and error messages.");
}
