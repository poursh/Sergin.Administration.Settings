namespace Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetList;

/// <summary>Where one key stands in one language.</summary>
public enum TranslationStatus
{
    /// <summary>A declared key with no translation: the English default is shown.</summary>
    Untranslated,

    /// <summary>Translated from the default as it reads now.</summary>
    Translated,

    /// <summary>Translated from a default that has changed since; still shown.</summary>
    NeedsReview,

    /// <summary>A translation of a key the catalog no longer declares: kept, listed, deletable, never shown.</summary>
    Orphaned,
}
