using Microsoft.AspNetCore.Components;

namespace Sergin.Administration.Settings.Presentation.Blazor.Languages.Pages;

/// <summary>The route for the generated language detail view, shown as a page.</summary>
public sealed partial class LanguageDetailPage
{
    [Parameter]
    public Guid Id { get; set; }
}
