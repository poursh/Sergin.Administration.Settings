namespace Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetList;

/// <summary>
/// One key in one language. <see cref="Default"/> is the catalog's current default, or for an orphaned key the default
/// it was translated from; <see cref="Text"/> and <see cref="TranslationId"/> are null for an untranslated key.
/// </summary>
public sealed record GetTranslationListItem(
    string Key,
    string Module,
    string Category,
    string Default,
    string? Text,
    TranslationStatus Status,
    Guid? TranslationId);
