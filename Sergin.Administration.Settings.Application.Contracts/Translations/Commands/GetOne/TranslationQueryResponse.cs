namespace Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetOne;

public sealed record TranslationQueryResponse(Guid Id, Guid LanguageId, string Key, string Text, string SourceText);
