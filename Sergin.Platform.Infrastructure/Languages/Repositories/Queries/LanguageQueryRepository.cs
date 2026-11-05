using System.Data.Common;
using Sergin.Platform.Application.Contracts.Languages.Commands.GetList;
using Sergin.Platform.Application.Contracts.Languages.Commands.GetOne;
using Sergin.Platform.Application.Languages;
using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application;
using Sergin.SharedKernel.Application.Commands.Queries;
using Sergin.SharedKernel.Application.Concurrency;
using Sergin.SharedKernel.Domain;
using Sergin.SharedKernel.Infrastracture.Data;
using Sergin.SharedKernel.Infrastructure.Data.EFCore.Queries;

namespace Sergin.Platform.Infrastructure.Languages.Repositories.Queries;

// Not soft-deletable, so no deleted_at_utc filter; not scoped, so no reach filter. The enums are stored by name and
// read back through LanguageRow, which also resolves the direction the way Language.IsRightToLeft does.
internal sealed class LanguageQueryRepository(IDbConnectionFactory connectionFactory) : ILanguageAllQueryRepository
{
    private static readonly ListQueryColumns ListColumns = new(
        LanguageListFields.Set,
        new Dictionary<string, string> { [LanguageListFields.Culture] = "culture" },
        searchColumns: ["culture", "display_name"],
        tieBreaker: "id");

    public async Task<Versioned<LanguageQueryResponse>?> GetLanguageById(LanguageId id, CancellationToken cancellationToken = default)
    {
        using DbConnection connection = await connectionFactory.CreateConnectionAsync();

        const string query =
            """
            SELECT id, culture, display_name AS displayName, direction, status, row_version AS rowVersion
            FROM pf.language
            WHERE id = @Id;
            """;

        IEnumerable<Versioned<LanguageQueryResponse>> rows = await connection.QueryAsync<LanguageRow, Guid, Versioned<LanguageQueryResponse>>(
            new CommandDefinition(query, new { Id = id.Value }, cancellationToken: cancellationToken),
            (row, rowVersion) => new Versioned<LanguageQueryResponse>(row.ToResponse(), RowVersion.Create(rowVersion)),
            splitOn: "rowVersion");

        return rows.SingleOrDefault();
    }

    public async Task<ListQueryResponse<GetLanguageListItem>> GetListAsync(ListQuery query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        using DbConnection connection = await connectionFactory.CreateConnectionAsync();

        DynamicParameters parameters = new(new { PageSize = query.Paggination.Size.Value, Offset = query.Paggination.Skip });
        ListQuerySqlParts criteria = ListQuerySql.Apply(query, ListColumns, parameters);

        string queries =
            $"""
            SELECT count(*) FROM pf.language WHERE TRUE{criteria.Where};

            SELECT id, culture, display_name AS displayName, direction, status
            FROM pf.language
            WHERE TRUE{criteria.Where}
            {criteria.OrderBy}
            LIMIT @PageSize OFFSET @Offset;
            """;

        // The criteria text holds only ListColumns' expressions; every value is a parameter.
#pragma warning disable S2077
        GridReader res = await connection.QueryMultipleAsync(new CommandDefinition(queries, parameters, cancellationToken: cancellationToken));
#pragma warning restore S2077

        int count = await res.ReadSingleAsync<int>();
        IReadOnlyCollection<GetLanguageListItem> list = [.. (await res.ReadAsync<LanguageRow>()).Select(row => row.ToListItem())];

        return new ListQueryResponse<GetLanguageListItem>(list, count);
    }
}
