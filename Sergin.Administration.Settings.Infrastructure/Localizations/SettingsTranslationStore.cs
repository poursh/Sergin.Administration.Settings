using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Sergin.Administration.Settings.Infrastructure.Languages.Repositories.Queries;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Infrastracture.Data;

namespace Sergin.Administration.Settings.Infrastructure.Localizations;

/// <summary>
/// The translation store over the Settings tables: every language in <c>st.language</c>, drafts included, with its
/// direction resolved (an administrator's override applied), since SharedKernel reads
/// <see cref="LanguageInfo.IsRightToLeft"/> as it is; and a culture's rows of <c>st.translation</c>, whatever its
/// language's status. Orphaned rows come too: the catalog never asks for their keys, so they are never shown. A singleton, as the cache that calls it is, so each call opens a
/// scope for the scoped connection factory.
/// </summary>
internal sealed class SettingsTranslationStore(IServiceScopeFactory scopeFactory) : ITranslationStore
{
    public async Task<IReadOnlyList<LanguageInfo>> GetLanguagesAsync(CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = scopeFactory.CreateAsyncScope();
        using DbConnection connection = await scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>().CreateConnectionAsync();

        const string query =
            """
            SELECT id, culture, display_name AS displayName, direction, status
            FROM st.language
            ORDER BY culture;
            """;

        IEnumerable<LanguageRow> rows = await connection.QueryAsync<LanguageRow>(new CommandDefinition(query, cancellationToken: cancellationToken));

        return [.. rows.Select(row => row.ToLanguageInfo())];
    }

    public async Task<IReadOnlyDictionary<string, StoredTranslation>> GetTranslationsAsync(string culture, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(culture);

        await using AsyncServiceScope scope = scopeFactory.CreateAsyncScope();
        using DbConnection connection = await scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>().CreateConnectionAsync();

        // The stored culture is .NET's own name for it, the one the cache asks with; compared without case all the same,
        // as the cache keys its cultures.
        const string query =
            """
            SELECT t.key, t.text, t.source_text AS sourceText
            FROM st.translation t
            JOIN st.language l ON l.id = t.language_id
            WHERE lower(l.culture) = lower(@Culture);
            """;

        IEnumerable<StoredRow> rows = await connection.QueryAsync<StoredRow>(
            new CommandDefinition(query, new { Culture = culture }, cancellationToken: cancellationToken));

        return rows.ToDictionary(row => row.Key, row => new StoredTranslation(row.Text, row.SourceText), StringComparer.Ordinal);
    }

    private sealed record StoredRow(string Key, string Text, string SourceText);
}
