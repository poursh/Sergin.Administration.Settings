using MudBlazor;
using Sergin.Administration.Settings.Application.Configurations.Languages;
using Sergin.SharedKernel.Modules;
using Sergin.SharedKernel.Presentation.Blazor.Modules;

namespace Sergin.Administration.Settings.Presentation.Blazor;

/// <summary>
/// The module's drawer entries, in the shared Administration group. Each entry is also named individually, so a
/// breadcrumb step reads the same label and href as the drawer.
/// </summary>
public static class SettingsNavigation
{
    public static SerginNavGroup Group => SerginNavGroups.Administration;

    public static SerginNavItem Languages { get; } = new(
        LanguageUiTexts.Title,
        "/st/languages",
        Icons.Material.Filled.Translate,
        Order: 300,
        RequiredPermission: LanguagePermissions.Read);

    public static IReadOnlyCollection<SerginNavItem> Items { get; } = [Languages];
}
