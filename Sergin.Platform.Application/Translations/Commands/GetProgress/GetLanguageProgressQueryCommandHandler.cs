using Sergin.Platform.Application.Contracts.Translations.Commands.GetList;
using Sergin.Platform.Application.Contracts.Translations.Commands.GetProgress;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Queries;
using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Platform.Application.Translations.Commands.GetProgress;

// The same merge as the list: a translation needing review is shown, so it counts as translated; an orphan is not a
// declared text, so it counts in neither number.
internal sealed class GetLanguageProgressQueryCommandHandler(
    ITranslationRowQueryRepository rows,
    ILanguageRepository languages,
    TextCatalog catalog) : IQueryHandler<GetLanguageProgressQueryCommand, LanguageProgressQueryResponse>
{
    public async Task<ErrorOr<LanguageProgressQueryResponse>> Handle(
        GetLanguageProgressQueryCommand request, CancellationToken cancellationToken)
    {
        if (!await languages.ExistsAsync(request.LanguageId, cancellationToken))
        {
            return Error.NotFound();
        }

        IReadOnlyList<GetTranslationListItem> merged =
            TranslationListMerge.Merge(catalog, await rows.GetRowsAsync(request.LanguageId, cancellationToken));

        int translated = merged.Count(item => item.Status is TranslationStatus.Translated or TranslationStatus.NeedsReview);

        return new LanguageProgressQueryResponse(translated, catalog.Entries.Count);
    }
}
