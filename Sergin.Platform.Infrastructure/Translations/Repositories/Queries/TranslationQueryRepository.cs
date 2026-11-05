using System.Data.Common;
using Sergin.Platform.Application.Contracts.Translations.Commands.GetOne;
using Sergin.Platform.Application.Translations;
using Sergin.Platform.Domain.Languages;
using Sergin.Platform.Domain.Translations;
using Sergin.SharedKernel.Application.Concurrency;
using Sergin.SharedKernel.Domain;
using Sergin.SharedKernel.Infrastracture.Data;

namespace Sergin.Platform.Infrastructure.Translations.Repositories.Queries;

// Not soft-deletable, so no deleted_at_utc filter; not scoped, so no reach filter.
internal sealed class TranslationQueryRepository(IDbConnectionFactory connectionFactory) : ITranslationAllQueryRepository
{
    public async Task<Versioned<TranslationQueryResponse>?> GetTranslationById(TranslationId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        using DbConnection connection = await connectionFactory.CreateConnectionAsync();

        const string query =
            """
            SELECT id, language_id AS languageId, key, text, source_text AS sourceText, row_version AS rowVersion
            FROM pf.translation
            WHERE id = @Id;
            """;

        IEnumerable<Versioned<TranslationQueryResponse>> rows = await connection.QueryAsync<TranslationQueryResponse, Guid, Versioned<TranslationQueryResponse>>(
            new CommandDefinition(query, new { Id = id.Value }, cancellationToken: cancellationToken),
            (row, rowVersion) => new Versioned<TranslationQueryResponse>(row, RowVersion.Create(rowVersion)),
            splitOn: "rowVersion");

        return rows.SingleOrDefault();
    }

    public async Task<IReadOnlyCollection<TranslationRow>> GetRowsAsync(LanguageId languageId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(languageId);

        using DbConnection connection = await connectionFactory.CreateConnectionAsync();

        const string query =
            """
            SELECT key, text, source_text AS sourceText, id
            FROM pf.translation
            WHERE language_id = @Id;
            """;

        return [.. await connection.QueryAsync<TranslationRow>(
            new CommandDefinition(query, new { Id = languageId.Value }, cancellationToken: cancellationToken))];
    }
}
