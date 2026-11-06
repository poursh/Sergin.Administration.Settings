using Microsoft.AspNetCore.Components;

namespace Sergin.Platform.Presentation.Blazor.Languages.Pages;

/// <summary>The route for <see cref="Components.LanguageDetailView"/>, shown as a page.</summary>
public sealed partial class LanguageDetailPage
{
    [Parameter]
    public Guid Id { get; set; }
}
