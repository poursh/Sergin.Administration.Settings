using Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetList;
using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Administration.Settings.Application.Translations;

/// <summary>
/// The catalog's keys merged with one language's rows, ordered by key: a declared key with no row is untranslated, one
/// whose row was translated from the current default translated, from another default needing review; a row whose key
/// the catalog no longer declares is orphaned and has no origin. Merged in memory: the catalog lives in code, the rows in the database.
/// </summary>
internal static class TranslationListMerge
{
    public static IReadOnlyList<GetTranslationListItem> Merge(TextCatalog catalog, IReadOnlyCollection<TranslationRow> rows)
    {
        var byKey = rows.ToDictionary(row => row.Key, StringComparer.Ordinal);
        List<GetTranslationListItem> items = new(catalog.Entries.Count + rows.Count);

        foreach (TextCatalogEntry entry in catalog.Entries)
        {
            string key = entry.Definition.Key.Value;
            string category = string.Join(SerginStringKey.Separator, entry.Category);

            if (byKey.Remove(key, out TranslationRow? row))
            {
                TranslationStatus status = string.Equals(row.SourceText, entry.Definition.Default, StringComparison.Ordinal)
                    ? TranslationStatus.Translated
                    : TranslationStatus.NeedsReview;

                items.Add(new GetTranslationListItem(
                    key, entry.Module, category, entry.Definition.Default, row.Text, status, row.Id, entry.Origin));
            }
            else
            {
                items.Add(new GetTranslationListItem(
                    key, entry.Module, category, entry.Definition.Default, Text: null, TranslationStatus.Untranslated, TranslationId: null, entry.Origin));
            }
        }

        // What is left has no declaration: shown with the default it was translated from.
        foreach (TranslationRow orphan in byKey.Values)
        {
            string[] segments = orphan.Key.Split(SerginStringKey.Separator);
            string category = string.Join(SerginStringKey.Separator, segments.Skip(1).SkipLast(1));

            items.Add(new GetTranslationListItem(
                orphan.Key, segments[0], category, orphan.SourceText, orphan.Text, TranslationStatus.Orphaned, orphan.Id, Origin: null));
        }

        items.Sort((left, right) => string.CompareOrdinal(left.Key, right.Key));

        return items;
    }
}
