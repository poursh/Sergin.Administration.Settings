using Sergin.SharedKernel.Domain.Securities;

namespace Sergin.Administration.Settings.Application.Configurations.Languages;

/// <summary>The permissions the Language aggregate owns. Languages belong to no scope, so a holder on any scope passes.</summary>
public static class LanguagePermissions
{
    public static PermissionDefinition Read { get; } = PermissionDefinition.Create("permission.st.languages.read", "View languages");

    public static PermissionDefinition Manage { get; } =
        PermissionDefinition.Create("permission.st.languages.manage", "Add, edit, publish and delete languages");
}
