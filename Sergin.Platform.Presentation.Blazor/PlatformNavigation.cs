using MudBlazor;
using Sergin.Platform.Application.Configurations;
using Sergin.SharedKernel.Modules;

namespace Sergin.Platform.Presentation.Blazor;

/// <summary>
/// The module's drawer group and entries. The group has no entry yet, so the shell does not render it; each feature's
/// list page adds one, named individually so its breadcrumb step reads the same label and href as the drawer.
/// </summary>
public static class PlatformNavigation
{
    public static SerginNavGroup Group { get; } = new(
        PlatformTexts.MenuTitle,
        Icons.Material.Filled.Settings,
        Order: 300);

    public static IReadOnlyCollection<SerginNavItem> Items { get; } = [];
}
