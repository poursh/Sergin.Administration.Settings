namespace Sergin.Administration.Settings.Presentation.Blazor;

/// <summary>
/// The module's own texts that belong to no aggregate: its caption inside the shared Administration drawer group.
/// Only the UI shows it, so it is declared here rather than in Application.Configurations.
/// </summary>
public static class SettingsUiTexts
{
    public static readonly TextCategory Category = TextCategory.Create("Settings");

    public static readonly TextDefinition MenuTitle = Category.Text("MenuTitle", "Settings");
}
