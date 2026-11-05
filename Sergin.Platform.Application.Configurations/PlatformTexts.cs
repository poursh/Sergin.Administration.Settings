using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Platform.Application.Configurations;

/// <summary>The module's own texts that belong to no aggregate: its menu title (the drawer group).</summary>
public static class PlatformTexts
{
    public static readonly TextCategory Category = TextCategory.Create("Platform");

    public static readonly TextDefinition MenuTitle = Category.Text("MenuTitle", "Platform");
}
