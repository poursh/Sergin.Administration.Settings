using MudBlazor;
using Sergin.Platform.Application.Configurations;
using Sergin.Platform.Application.Configurations.Languages;
using Sergin.SharedKernel.Modules;

namespace Sergin.Platform.Presentation.Blazor;

/// <summary>
/// The module's drawer group and entries. Each entry is also named individually, so a breadcrumb step reads the same
/// label and href as the drawer.
/// </summary>
public static class PlatformNavigation
{
    public static SerginNavGroup Group { get; } = new(
        PlatformTexts.MenuTitle,
        Icons.Material.Filled.Settings,
        Order: 300);

    public static SerginNavItem Languages { get; } = new(
        LanguageTexts.Title,
        "/pf/languages",
        Icons.Material.Filled.Translate,
        Order: 100,
        RequiredPermission: LanguagePermissions.Read);

    public static IReadOnlyCollection<SerginNavItem> Items { get; } = [Languages];
}
