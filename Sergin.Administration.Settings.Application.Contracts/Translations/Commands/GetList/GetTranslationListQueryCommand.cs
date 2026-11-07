using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Queries;
using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetList;

/// <summary>
/// The catalog's keys merged with one language's translations, ordered by key. <see cref="Module"/> is a module segment
/// (<c>device-management</c>); <see cref="Category"/> the category path below it (<c>devices</c> also matches
/// <c>devices.errors</c>); <see cref="Origin"/> keeps the keys the application or the UI declares, never an orphaned
/// one; the search term reads the key, the default and the translation.
/// </summary>
public sealed record GetTranslationListQueryCommand : ListQuery<GetTranslationListItem>
{
    public GetTranslationListQueryCommand(
        LanguageId languageId,
        Paggination paggination,
        Term? term = default,
        string? module = default,
        string? category = default,
        TranslationStatus? status = default,
        TextOrigin? origin = default)
        : base(paggination, term)
    {
        LanguageId = languageId;
        Module = module;
        Category = category;
        Status = status;
        Origin = origin;
    }

    public LanguageId LanguageId { get; init; }

    public string? Module { get; init; }

    public string? Category { get; init; }

    public TranslationStatus? Status { get; init; }

    public TextOrigin? Origin { get; init; }
}
