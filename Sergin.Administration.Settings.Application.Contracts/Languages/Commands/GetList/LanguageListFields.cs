using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Administration.Settings.Application.Contracts.Languages.Commands.GetList;

/// <summary>What the language list may be sorted by; it has no filters, and its search reads the culture and the display name.</summary>
public static class LanguageListFields
{
    public const string Culture = "culture";

    public static ListFieldSet Set { get; } = ListFieldSet.Create(ListField.SortOnly(Culture));
}
