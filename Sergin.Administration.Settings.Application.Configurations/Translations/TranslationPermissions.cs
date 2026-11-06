using Sergin.SharedKernel.Domain.Securities;

namespace Sergin.Administration.Settings.Application.Configurations.Translations;

/// <summary>
/// The permissions the Translation aggregate owns. Translations belong to no scope, so a holder on any scope passes.
/// Reading translations needs <c>LanguagePermissions.Read</c>: they are a language's content.
/// </summary>
public static class TranslationPermissions
{
    public static PermissionDefinition Edit { get; } =
        PermissionDefinition.Create("permission.st.translations.edit", "Add, change and delete translations");
}
