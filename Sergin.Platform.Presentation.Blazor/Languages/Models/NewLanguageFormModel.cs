namespace Sergin.Platform.Presentation.Blazor.Languages.Models;

/// <summary>
/// The binding target for the create form. No DataAnnotations: the fields are validated by the pipeline's own
/// <c>AddLanguageCommandValidator</c> through <c>ISerginFormValidator</c>.
/// </summary>
public sealed class NewLanguageFormModel
{
    public string Culture { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;
}
