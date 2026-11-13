using Microsoft.AspNetCore.Components;

namespace Sergin.Administration.Settings.Presentation.Blazor.Translations.Pages;

/// <summary>The route for the generated translation list, shown as a page.</summary>
public sealed partial class TranslationListPage
{
    [Parameter]
    public Guid LanguageId { get; set; }
}
