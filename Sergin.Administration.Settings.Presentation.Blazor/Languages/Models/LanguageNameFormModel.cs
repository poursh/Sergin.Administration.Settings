namespace Sergin.Administration.Settings.Presentation.Blazor.Languages.Models;

/// <summary>The binding target for the detail view's rename form, validated by <c>RenameLanguageCommandValidator</c>.</summary>
public sealed class LanguageNameFormModel
{
    public string DisplayName { get; set; } = string.Empty;
}
