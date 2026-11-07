namespace Sergin.Administration.Settings.Application.Translations;

/// <summary>One stored translation of one language, as the list reads it before merging it with the catalog.</summary>
public sealed record TranslationRow(string Key, string Text, string SourceText, Guid Id);
