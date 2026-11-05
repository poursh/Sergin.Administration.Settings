using Sergin.Platform.Application.Contracts.Translations.Commands.GetList;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Commands.Queries;
using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Platform.Application.Translations.Commands.GetList;

// Merged in memory (TranslationListMerge), then filtered, searched and paged here: the catalog is code, so there is no
// SQL to push the filters into. A language holds a few thousand keys at most.
internal sealed class GetTranslationListQueryCommandHandler(
    ITranslationRowQueryRepository rows,
    ILanguageRepository languages,
    TextCatalog catalog) : IListQueryHandler<GetTranslationListQueryCommand, GetTranslationListItem>
{
    public async Task<ErrorOr<ListQueryResponse<GetTranslationListItem>>> Handle(
        GetTranslationListQueryCommand request, CancellationToken cancellationToken)
    {
        if (!await languages.ExistsAsync(request.LanguageId, cancellationToken))
        {
            return Error.NotFound();
        }

        IReadOnlyList<GetTranslationListItem> merged =
            TranslationListMerge.Merge(catalog, await rows.GetRowsAsync(request.LanguageId, cancellationToken));

        IReadOnlyCollection<GetTranslationListItem> matching = [.. merged.Where(item => Matches(item, request))];
        IReadOnlyCollection<GetTranslationListItem> page =
            [.. matching.Skip(request.Paggination.Skip).Take(request.Paggination.Size.Value)];

        return new ListQueryResponse<GetTranslationListItem>(page, matching.Count);
    }

    private static bool Matches(GetTranslationListItem item, GetTranslationListQueryCommand request)
    {
        if (request.Module is { Length: > 0 } module && !string.Equals(item.Module, module, StringComparison.Ordinal))
        {
            return false;
        }

        if (request.Category is { Length: > 0 } category && !InCategory(item.Category, category))
        {
            return false;
        }

        if (request.Status is { } status && item.Status != status)
        {
            return false;
        }

        return request.Term?.Value is not { Length: > 0 } term
            || Contains(item.Key, term)
            || Contains(item.Default, term)
            || item.Text is not null && Contains(item.Text, term);
    }

    // The category itself or one below it: "devices" matches "devices" and "devices.errors", not "devices-old".
    private static bool InCategory(string itemCategory, string category) =>
        string.Equals(itemCategory, category, StringComparison.Ordinal)
        || itemCategory.StartsWith(category + SerginStringKey.Separator, StringComparison.Ordinal);

    private static bool Contains(string value, string term) => value.Contains(term, StringComparison.CurrentCultureIgnoreCase);
}
