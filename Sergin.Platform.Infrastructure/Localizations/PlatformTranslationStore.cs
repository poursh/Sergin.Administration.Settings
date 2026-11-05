using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Sergin.Platform.Infrastructure.Languages.Repositories.Queries;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Infrastracture.Data;

namespace Sergin.Platform.Infrastructure.Localizations;

/// <summary>
/// The translation store over the Platform tables: every language in <c>pf.language</c>, drafts included, with its
/// direction resolved (an administrator's override applied), since SharedKernel reads
/// <see cref="LanguageInfo.IsRightToLeft"/> as it is. A singleton, as the cache that calls it is, so each call opens a
/// scope for the scoped connection factory.
/// </summary>
internal sealed class PlatformTranslationStore(IServiceScopeFactory scopeFactory) : ITranslationStore
{
    private static readonly IReadOnlyDictionary<string, StoredTranslation> none =
        new Dictionary<string, StoredTranslation>(StringComparer.Ordinal);

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

        IEnumerable<LanguageRow> rows = await connection.QueryAsync<LanguageRow>(query);

        return [.. rows.Select(row => row.ToLanguageInfo())];
    }

    // The Translation aggregate does not exist yet: every language shows the English defaults until it does.
    public Task<IReadOnlyDictionary<string, StoredTranslation>> GetTranslationsAsync(string culture, CancellationToken cancellationToken) =>
        Task.FromResult(none);
}
