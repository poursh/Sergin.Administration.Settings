using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Administration.Settings.Application.Contracts.Translations.Commands.GetList;

/// <summary>
/// The translation list declares no sort or filter field: it is always ordered by key, and its filters (module,
/// category, status) are the query's own properties, since it is merged in memory rather than read by SQL.
/// </summary>
public static class TranslationListFields
{
    public static ListFieldSet Set { get; } = ListFieldSet.Create();
}
