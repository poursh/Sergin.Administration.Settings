namespace Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetProgress;

/// <summary>
/// How much of the catalog a language covers: <see cref="Total"/> declared keys, <see cref="Translated"/> of them with a
/// translation (needing review or not, since both are shown). Orphaned translations count in neither.
/// </summary>
public sealed record LanguageProgressQueryResponse(int Translated, int Total);
