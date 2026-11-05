using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Sergin.Platform.Infrastructure.Languages.Repositories.Queries;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Infrastracture.Data;

namespace Sergin.Platform.Infrastructure.Localizations;

/// <summary>
/// The translation store over the Platform tables: every language in <c>pf.language</c>, drafts included, with its
/// direction resolved (an administrator's override applied), since SharedKernel reads
/// <see cref="LanguageInfo.IsRightToLeft"/> as it is; and a culture's rows of <c>pf.translation</c>, whatever its
/// language's status. Orphaned rows come too: the catalog never asks for their keys, so they are never shown. A singleton, as the cache that calls it is, so each call opens a
/// scope for the scoped connection factory.
/// </summary>
internal sealed class PlatformTranslationStore(IServiceScopeFactory scopeFactory) : ITranslationStore
{
    public async Task<IReadOnlyList<LanguageInfo>> GetLanguagesAsync(CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = scopeFactory.CreateAsyncScope();
        using DbConnection connection = await scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>().CreateConnectionAsync();

        const string query =
            """
            SELECT id, culture, display_name AS displayName, direction, status
            FROM pf.language
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
            FROM pf.translation t
            JOIN pf.language l ON l.id = t.language_id
            WHERE lower(l.culture) = lower(@Culture);
            """;

        IEnumerable<StoredRow> rows = await connection.QueryAsync<StoredRow>(
            new CommandDefinition(query, new { Culture = culture }, cancellationToken: cancellationToken));

        return rows.ToDictionary(row => row.Key, row => new StoredTranslation(row.Text, row.SourceText), StringComparer.Ordinal);
    }

    private sealed record StoredRow(string Key, string Text, string SourceText);
}
